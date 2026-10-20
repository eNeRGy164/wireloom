#!/usr/bin/env bash
# Checks report finalization without DDS peers, a toolchain, or a license.
set -euo pipefail

runner="$(dirname "$0")/run-matrix.sh"
scratch="$(mktemp -d)"
trap 'rm -f "$scratch"/*; rmdir "$scratch"' EXIT

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

# Arrange: load the production fixture selector without provisioning RTI.
source <(sed -n '/^fixtures_for_case() {/,/^}/p' "$runner")

# Act / Assert: the dedicated IDL case must retain every sequence-shape fixture.
expected_fixtures=$'sequence-empty\nsequence-unequal\nsequence-mixed-empty\nsequence-boundary'
[[ "$(fixtures_for_case 05-array-of-sequences-idl)" == "$expected_fixtures" ]]
echo 'Sequence-array fixture selection passed: 4 fixtures.'
