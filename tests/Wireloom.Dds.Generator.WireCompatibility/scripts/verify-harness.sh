#!/usr/bin/env bash
# Checks report finalization without DDS peers, a toolchain, or a license.
set -euo pipefail

runner="$(dirname "$0")/run-matrix.sh"
matrix_tools="$(dirname "$0")/matrix-tools.cs"
scratch="$(mktemp -d)"
trap 'rm -rf -- "$scratch"' EXIT

dotnet build "$matrix_tools" --output "$scratch/matrix-tools" --verbosity quiet

# Load only the production EXIT handler; do not provision RTI or launch peers.
sed -n '/^finish_run() {/,/^trap finish_run EXIT/p' "$runner" | sed '$d' > "$scratch/finish-run.sh"

check_finalization() {
    local name="$1"
    local run_status="$2"
    local report_status="$3"
    local expected_status="$4"
    local observed_status=0

    # Arrange: peer execution has started and a scenario failure is recorded.
    (
        source "$scratch/finish-run.sh"
        end_workflow_group() { :; }
        start_workflow_group() { :; }
        workflow_annotation() { printf '%s\n' "$*"; }
        write_report() {
            touch "$scratch/$name.report"
            return "$report_status"
        }
        cpp_compile_only=0
        output_dir="$scratch"
        overall_status=1
        passed_scenario_count=0
        expected_scenario_count=9

        # Act: finalize after a late setup error or a handled peer failure.
        trap finish_run EXIT
        exit "$run_status"
    ) > "$scratch/$name.log" 2>&1 || observed_status=$?

    # Assert: reports are attempted, but they never hide unexpected setup errors.
    [[ "$observed_status" -eq "$expected_status" && -f "$scratch/$name.report" ]]
    if [[ "$expected_status" -ne 0 ]]; then
        grep -q '^error ' "$scratch/$name.log"
    else
        grep -q '^warning ' "$scratch/$name.log"
    fi
}

# Arguments: case name, original exit code, report exit code, expected final exit code.
check_finalization late-cpp-setup 7 0 7
check_finalization handled-peer-failure 0 0 0
check_finalization report-generation-failure 7 1 1
echo 'Harness finalization passed: 3 checks, no RTI license required.'

# Arrange: load the production C# fixture catalog without provisioning RTI.
fixtures_for_case() {
    dotnet "$scratch/matrix-tools/matrix-tools.dll" case-fixtures "$1" "$rti_version"
}
rti_version=7.3.1

# Act / Assert: the dedicated IDL case must retain every sequence-shape fixture.
expected_fixtures=$'sequence-empty\nsequence-unequal\nsequence-mixed-empty\nsequence-boundary'
[[ "$(fixtures_for_case 05-array-of-sequences-idl)" == "$expected_fixtures" ]]
echo 'Sequence-array fixture selection passed: 4 fixtures.'

# Arrange: each new IDL case has a named sample shape rather than a generic
# populated record. Run-matrix uses these names as the full wire exchange plan.
assert_fixtures() {
    local case_id="$1"
    local expected="$2"
    [[ "$(fixtures_for_case "$case_id")" == "$expected" ]]
}

assert_fixtures 14-extensibility-full 'non-default'
assert_fixtures 06-aggregates $'non-default\ndirect-recursive-sequence'
assert_fixtures 14-autoid-scope 'scope-nested'
assert_fixtures 14-annotation-arguments $'default\nnon-default'
assert_fixtures 14-member-declarators 'non-default'
assert_fixtures 14-union-label-expressions $'symbolic\narithmetic\nhexadecimal'
assert_fixtures 14-union-scoped-enum $'enum-selected\nenum-first'
assert_fixtures 14-union-array-branch $'array-branch\nscalar-branch'
assert_fixtures 14-string-expression-bound 'string-boundary'
assert_fixtures 14-typed-defaults $'default\nnon-default'
assert_fixtures 14-enum-constant 'constant-and-label'
rti_version=7.7.0
assert_fixtures 14-typed-defaults $'default\nnon-default\nsparse-omitted'
echo 'C163–C172 fixture selection passed: 10 cases, 17 fixtures; sparse C171 is RTI 7.7-only.'

# Arrange: the wrapper and canonical IDL have the same basename. RTI emits
# the wrapper pair at the output root before dependencies are generated.
source <(sed -n '/^find_rti_csharp_include_files() {/,/^}/p' "$runner")
include_scratch="$(mktemp -d)"
check_include_outputs() (
    trap 'rm -rf -- "$include_scratch"' EXIT
    local root_idl='IdlWrappers/04-string-alias-chains.idl'
    local source_name='04-string-alias-chains.cs'
    local plugin_name='04-string-alias-chainsPlugin.cs'
    touch "$include_scratch/$source_name" "$include_scratch/$plugin_name"

    # Act / Assert: root outputs are not canonical include outputs; the
    # production loop must reach its separate dependency-generation path.
    [[ -z "$(find_rti_csharp_include_files "$include_scratch" "$root_idl" "$source_name")" ]]
    [[ -z "$(find_rti_csharp_include_files "$include_scratch" "$root_idl" "$plugin_name")" ]]

    # Arrange: dependency generation places its own pair below the root.
    mkdir -p "$include_scratch/includes/0"
    touch "$include_scratch/includes/0/$source_name" "$include_scratch/includes/0/$plugin_name"

    # Act / Assert: only the dependency pair is counted, and a wrapper with
    # a different basename produces the same include candidate set.
    [[ "$(find_rti_csharp_include_files "$include_scratch" "$root_idl" "$source_name")" == "$include_scratch/includes/0/$source_name" ]]
    [[ "$(find_rti_csharp_include_files "$include_scratch" "$root_idl" "$plugin_name")" == "$include_scratch/includes/0/$plugin_name" ]]
    [[ "$(find_rti_csharp_include_files "$include_scratch" 'other-topic.idl' "$source_name" | wc -l)" -eq 2 ]]

    # Arrange: an incomplete or duplicated dependency set must remain visible
    # to the production guards, rather than being accepted or silently removed.
    mkdir -p "$include_scratch/includes/1"
    touch "$include_scratch/includes/1/$source_name"

    # Act / Assert: both sources and the single plugin remain discoverable.
    [[ "$(find_rti_csharp_include_files "$include_scratch" "$root_idl" "$source_name" | wc -l)" -eq 2 ]]
    [[ "$(find_rti_csharp_include_files "$include_scratch" "$root_idl" "$plugin_name" | wc -l)" -eq 1 ]]
)
check_include_outputs
echo 'RTI C# include output discovery passed: 7 checks, including wrapper basename collisions.'
