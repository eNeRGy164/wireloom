#!/usr/bin/env bash
# Drives the wire-compatibility matrix from inside the pinned RTI toolchain.
# Wireloom-generated C# is compared with RTI-generated C# and C++ reference
# peers. Each eligible fixture is exercised across every directed peer pairing.
# Keep this orchestration here rather than in C# so it can provision RTI's
# native generator/compiler and launch isolated producer/consumer processes.
set -euo pipefail

readonly repo_root="${GITHUB_WORKSPACE:-$(git rev-parse --show-toplevel)}"
readonly project="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/Wireloom.Dds.Generator.WireCompatibility.csproj"
readonly native_template="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/Native"
readonly rti_csharp_project="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/RtiOracleProbe/RtiOracleProbe.csproj"
readonly matrix_tools="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/scripts/matrix-tools.cs"
readonly manifest="$repo_root/docs/corpus/manifest.json"
readonly rti_version="${WIRE_COMPATIBILITY_RTI_VERSION:-7.7.0}"
readonly exclusions="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/wire-case-exclusions-$rti_version.json"
readonly timeout_seconds=20
readonly cpp_compile_only="${WIRE_COMPATIBILITY_CPP_COMPILE_ONLY:-0}"
readonly output_dir="${WIRE_COMPATIBILITY_OUTPUT_DIR:-$repo_root/artifacts/wire-compatibility}"
readonly scratch="${RUNNER_TEMP:-/tmp}/wireloom-wire-compatibility"
readonly cases_tsv="$scratch/cases.tsv"
readonly scenarios_tsv="$scratch/scenarios.tsv"
readonly cpp_compilation_tsv="$scratch/cpp-compilation.tsv"
workflow_group_open=0
readonly non_default_fixture_cases=(
    01-boundaries 01-full-widths
    02-names-constants 02-scopes 02-constant-expressions 02-formatting 02-multiple
    03-alias-aggregate 03-alias-collections
    04-strings 04-boundaries 04-unbounded-string-aliases 04-string-alias-chains
    05-unbounded-sequence-aliases
    05-collections 05-shapes 06-aggregates
    06-alias-composition
    07-unions 07-union-enum 07-union-multilabel 07-union-short
    07-union-char 07-union-char-numeric 07-union-char-numeric-255 07-union-scoped 07-union-boolean 07-union-aliases 07-union-wchar-label
    07-union-string-sequence-aliases 07-union-string-sequence-bounded-direct
    08-keys 08-key-nested 08-key-inherited 08-key-boundaries 08-key-union
    09-extensibility 09-default 09-id-gaps 09-evolution-optional
    09-optional-collections 09-autoid-hash 09-must-understand 09-defaults-ranges
    10-annotations 10-annotations-extended 10-annotations-rti 10-flat-data-binding
    09-data-representation 09-allowed-data-representation
    11-preprocessing 11-conditionals 11-macros 11-macro-operators
    11-preprocessor-advanced 11-include-search 11-comments
)
expected_scenario_count=0
completed_scenario_count=0
passed_scenario_count=0
failed_scenario_count=0

# GitHub Actions renders these workflow commands as expandable log groups and
# annotations. Keep local runs free of the command text.
workflow_command_escape() {
    local value="$1"
    value="${value//'%'/'%25'}"
    value="${value//$'\r'/'%0D'}"
    value="${value//$'\n'/'%0A'}"
    printf '%s' "$value"
}

start_workflow_group() {
    local title
    if [[ "${GITHUB_ACTIONS:-}" != "true" || "$workflow_group_open" -eq 1 ]]; then
        return
    fi
    title="$(workflow_command_escape "$1")"
    printf '::group::%s\n' "$title"
    workflow_group_open=1
}

end_workflow_group() {
    if [[ "${GITHUB_ACTIONS:-}" == "true" && "$workflow_group_open" -eq 1 ]]; then
        printf '::endgroup::\n'
        workflow_group_open=0
    fi
}

workflow_annotation() {
    local level="$1"
    local title
    local message
    title="$(workflow_command_escape "$2")"
    message="$(workflow_command_escape "$3")"
    if [[ "${GITHUB_ACTIONS:-}" == "true" ]]; then
        printf '::%s title=%s::%s\n' "$level" "$title" "$message"
    fi
}

# Return fixture names instead of a yes/no fixture flag. Keeping each variant
# in a separate process exchange makes a missing branch or sequence shape easy
# to identify in the uploaded results.
fixtures_for_case() {
    case "$1" in
        05-array-of-sequences-idl)
            printf '%s\n' sequence-empty sequence-unequal sequence-mixed-empty sequence-boundary
            ;;
        03-enums-aliases)
            printf '%s\n' default enum-alternate
            ;;
        02-scopes|02-constant-expressions|05-collections)
            printf '%s\n' sequence-empty sequence-single non-default
            ;;
        04-strings)
            printf '%s\n' string-empty string-boundary non-default
            ;;
        04-unbounded-string-aliases)
            printf '%s\n' string-empty string-boundary
            ;;
        04-string-alias-chains)
            printf '%s\n' string-empty string-boundary
            ;;
        05-unbounded-sequence-aliases)
            printf '%s\n' sequence-empty sequence-multiple
            ;;
        04-boundaries)
            printf '%s\n' string-boundary non-default
            ;;
        03-alias-collections)
            printf '%s\n' sequence-empty sequence-single non-default
            ;;
        09-evolution-optional)
            printf '%s\n' optional-absent optional-value-only optional-text-only optional-present
            ;;
        09-optional-collections)
            printf '%s\n' optional-absent optional-empty optional-single optional-multiple
            ;;
        09-optional-recursive-array)
            printf '%s\n' optional-absent optional-present optional-nested
            ;;
        09-optional-aggregate-member)
            printf '%s\n' optional-absent optional-present
            ;;
        09-optional-string-sequences)
            printf '%s\n' optional-absent optional-narrow-only optional-wide-only optional-empty optional-multiple
            ;;
        07-unions)
            printf '%s\n' union-number union-text union-default
            ;;
        07-union-enum)
            printf '%s\n' union-number union-text union-payload
            ;;
        07-union-multilabel)
            printf '%s\n' union-number union-text
            ;;
        07-union-short)
            printf '%s\n' union-number union-text union-default
            ;;
        07-union-char|07-union-wchar-label)
            printf '%s\n' union-number union-text union-default
            ;;
        07-union-boolean)
            printf '%s\n' union-enabled union-disabled
            ;;
        07-union-octet|07-union-short-boundaries|07-union-long|07-union-ushort|07-union-ulong|07-union-int8|07-union-uint8|07-union-int16|07-union-uint16|07-union-int32|07-union-uint32|07-union-char-boundaries)
            printf '%s\n' union-minimum union-maximum union-default
            ;;
        07-union-scoped)
            printf '%s\n' union-payload sequence-empty sequence-single
            ;;
        07-union-aliases)
            printf '%s\n' union-zero union-ten
            ;;
        07-union-char-numeric)
            printf '%s\n' union-127 union-default
            ;;
        07-union-char-numeric-255)
            printf '%s\n' union-255 union-default
            ;;
        10-resolve-name-wstring-aliases)
            printf '%s\n' string-empty string-boundary
            ;;
        07-union-string-sequence-aliases)
            printf '%s\n' union-direct-narrow union-direct-wide union-alias-narrow union-alias-wide union-bounded-narrow union-bounded-wide
            ;;
        07-union-string-sequence-bounded-direct)
            printf '%s\n' union-narrow-empty union-narrow-multiple union-wide-empty union-wide-multiple
            ;;
        14-extensibility-full|14-member-declarators)
            printf '%s\n' non-default
            ;;
        14-autoid-scope)
            printf '%s\n' scope-nested
            ;;
        14-annotation-arguments)
            printf '%s\n' default non-default
            ;;
        14-union-label-expressions)
            printf '%s\n' symbolic arithmetic hexadecimal
            ;;
        14-union-scoped-enum)
            printf '%s\n' enum-selected enum-first
            ;;
        14-union-array-branch)
            printf '%s\n' array-branch scalar-branch
            ;;
        14-string-expression-bound)
            printf '%s\n' string-boundary
            ;;
        14-typed-defaults)
            printf '%s\n' default non-default
            if [[ "${rti_version:-}" == "7.7.0" ]]; then
                printf '%s\n' sparse-omitted
            fi
            ;;
        14-enum-constant)
            printf '%s\n' constant-and-label
            ;;
        *)
            if [[ " ${non_default_fixture_cases[*]} " == *" $1 "* ]]; then
                printf '%s\n' non-default
            else
                printf '%s\n' default
            fi
            ;;
    esac
}

if [[ "$rti_version" != "7.3.1" && "$rti_version" != "7.7.0" ]]; then
    echo "The wire runner supports exact RTI 7.3.1 and 7.7.0 only." >&2
    exit 2
fi
if [[ "$rti_version" == "7.3.1" ]]; then
    readonly connextdds_arch="x64Linux4gcc7.3.0"
else
    readonly connextdds_arch="x64Linux4gcc8.5.0"
fi
if [[ -z "${RTI_LICENSE_FILE:-}" || ! -r "$RTI_LICENSE_FILE" ]]; then
    echo "RTI_LICENSE_FILE must name a readable RTI license file." >&2
    exit 2
fi
if [[ -z "${NDDSHOME:-}" || ! -x "$NDDSHOME/bin/rtiddsgen" ]]; then
    echo "NDDSHOME must point to the selected RTI toolchain." >&2
    exit 2
fi

rm -rf "$scratch"
mkdir -p "$scratch" "$output_dir/logs"
rm -f "$output_dir/logs"/*.log
: > "$cases_tsv"
: > "$scenarios_tsv"
printf 'case_id\tstatus\tdetail\n' > "$cpp_compilation_tsv"
printf '{"rtiVersion":"%s","scenarios":[]}' "$rti_version" > "$output_dir/results.json"

# Always write a structured report, including when a build or scenario fails.
# Endpoint output is separately reduced to status lines and selected RTI
# diagnostics so the report artifacts do not include arbitrary process output.
write_report() {
    cp "$scenarios_tsv" "$output_dir/scenarios.tsv"
    if [[ "$cpp_compile_only" == "1" ]]; then
        cp "$cpp_compilation_tsv" "$output_dir/cpp-compilation.tsv"
        return 0
    fi
    matrix_tool render-report \
        "$rti_version" "$expected_scenario_count" "$scenarios_tsv" \
        "$output_dir/results.json" "$output_dir/results.md" "$output_dir/logs" "$exclusions" "$manifest"
}
matrix_tool() {
    if [[ -f "$scratch/matrix-tools.dll" ]]; then
        dotnet "$scratch/matrix-tools.dll" "$@"
    else
        dotnet run --file "$matrix_tools" -- "$@"
    fi | tr -d '\r'
}

case_pairings() {
    local case_id="$1"
    local fixture="${2:-}"
    if [[ -n "$fixture" ]]; then
        matrix_tool case-pairings "$manifest" "$case_id" "$fixture"
    else
        matrix_tool case-pairings "$manifest" "$case_id"
    fi
}

case_uses_rti_csharp() {
    local case_id="$1"
    local pairing
    while IFS= read -r pairing; do
        if [[ "$pairing" == *"RTI C#"* ]]; then
            return 0
        fi
    done < <(case_pairings "$case_id")
    return 1
}

finish_run() {
    local run_status=$?
    trap - EXIT
    end_workflow_group
    if [[ "$cpp_compile_only" == "1" ]]; then
        if write_report; then
            local compiled failed
            compiled=$(awk -F '\t' '$2 == "compiled" { count++ } END { print count + 0 }' "$cpp_compilation_tsv")
            failed=$(awk -F '\t' 'NR > 1 && $2 != "compiled" { count++ } END { print count + 0 }' "$cpp_compilation_tsv")
            echo "RTI C++ compile sweep: $compiled compiled, $failed failed; see $output_dir/cpp-compilation.tsv."
            if [[ "$run_status" -ne 0 || "${overall_status:-0}" -ne 0 ]]; then
                exit 1
            fi
            exit 0
        fi
        echo "Failed to save the RTI C++ compilation report." >&2
        exit 1
    fi
    start_workflow_group "Generate wire compatibility report"
    if write_report; then
        end_workflow_group
        if [[ "$run_status" -ne 0 ]]; then
            echo "Matrix setup ended with status $run_status. See the generated report for selected exchanges that did not run." >&2
            workflow_annotation error "Wire compatibility matrix setup failed" \
                "Matrix setup ended with status $run_status; inspect the generated report for unrun scenarios."
            exit "$run_status"
        elif [[ "${overall_status:-0}" -ne 0 || "$passed_scenario_count" -lt "$expected_scenario_count" ]]; then
            workflow_annotation warning "Wire compatibility matrix completed with issues" \
                "$passed_scenario_count of $expected_scenario_count scenario exchanges passed; see the generated report for failed or unrun scenarios."
        else
            workflow_annotation notice "Wire compatibility matrix passed" \
                "$passed_scenario_count of $expected_scenario_count scenario exchanges passed in ${SECONDS}s."
        fi
        exit 0
    fi
    end_workflow_group
    echo "Failed to generate the wire-compatibility report." >&2
    workflow_annotation error "Wire compatibility report generation failed" \
        "The matrix report could not be generated; inspect the preceding log output."
    exit 1
}
trap finish_run EXIT

start_workflow_group "Prepare wire compatibility matrix"

if ! dotnet build "$matrix_tools" --output "$scratch" --verbosity quiet; then
    echo "Failed to build the .NET matrix utility." >&2
    exit 2
fi

if ! matrix_tool list-cases "$manifest" "$exclusions" > "$scratch/case-ids.txt"; then
    echo "Failed to discover wire-testable cases from the corpus manifest and exclusion registry." >&2
    exit 2
fi
mapfile -t case_ids < "$scratch/case-ids.txt"
mapfile -t rti_only_case_ids < <(matrix_tool list-rti-only-cases "$exclusions")
case_ids+=("${rti_only_case_ids[@]}")
declare -A rti_only_cases=()
for case_id in "${rti_only_case_ids[@]}"; do
    rti_only_cases["$case_id"]=1
done
if [[ "${#case_ids[@]}" -eq 0 ]]; then
    echo "Case discovery returned no wire-testable cases." >&2
    exit 2
fi
if [[ -n "${WIRE_COMPATIBILITY_CASE_FILTER:-}" ]]; then
    IFS=',' read -r -a requested_cases <<< "$WIRE_COMPATIBILITY_CASE_FILTER"
    for requested_case in "${requested_cases[@]}"; do
        if [[ ! " ${case_ids[*]} " =~ " ${requested_case} " ]]; then
            echo "Unknown or inapplicable corpus case for RTI $rti_version: $requested_case" >&2
            exit 2
        fi
    done
    case_ids=("${requested_cases[@]}")
fi
if [[ "$cpp_compile_only" != "1" ]]; then
    for case_id in "${case_ids[@]}"; do
        mapfile -t case_fixtures < <(fixtures_for_case "$case_id")
        if [[ -n "${rti_only_cases[$case_id]:-}" ]]; then
            case_scenario_count=0
            for fixture in "${case_fixtures[@]}"; do
                mapfile -t case_pairings_for_fixture < <(case_pairings "$case_id" "$fixture")
                for pairing in "${case_pairings_for_fixture[@]}"; do
                    writer_peer="${pairing%%|*}"
                    reader_peer="${pairing#*|}"
                    if [[ "$writer_peer" != "Wireloom C#" && "$reader_peer" != "Wireloom C#" ]]; then
                        case_scenario_count=$((case_scenario_count + 1))
                    fi
                done
            done
            expected_scenario_count=$((expected_scenario_count + case_scenario_count))
        else
            case_scenario_count=0
            for fixture in "${case_fixtures[@]}"; do
                mapfile -t case_pairings_for_fixture < <(case_pairings "$case_id" "$fixture")
                case_scenario_count=$((case_scenario_count + ${#case_pairings_for_fixture[@]}))
            done
            expected_scenario_count=$((expected_scenario_count + case_scenario_count))
        fi
    done

    printf 'RTI Connext %s: %d cases, %d scenario exchanges planned.\n' \
        "$rti_version" "${#case_ids[@]}" "$expected_scenario_count"

    # Seed every selected fixture/pairing before building. If restore, generation,
    # or native compilation exits early, the final report still names every
    # exchange that was selected but did not run.
    for case_id in "${case_ids[@]}"; do
        mapfile -t case_fixtures < <(fixtures_for_case "$case_id")
        for fixture in "${case_fixtures[@]}"; do
            mapfile -t case_pairings_for_fixture < <(case_pairings "$case_id" "$fixture")
            for pairing in "${case_pairings_for_fixture[@]}"; do
                writer_peer="${pairing%%|*}"
                reader_peer="${pairing#*|}"
                if [[ -n "${rti_only_cases[$case_id]:-}" ]] \
                    && [[ "$writer_peer" == "Wireloom C#" || "$reader_peer" == "Wireloom C#" ]]; then
                    continue
                fi
                printf '%s\t\t\t\t%s\t%s\t%s\t%s\tNOT_RUN\t%s\n' \
                    "$case_id" "$writer_peer" "$rti_version" \
                    "$reader_peer" "$rti_version" "$fixture" >> "$scenarios_tsv"
            done
        done
    done

    matrix_tool partition-shard "$rti_version" "${WIRE_COMPATIBILITY_SHARD_INDEX:-0}" \
        "${WIRE_COMPATIBILITY_SHARD_COUNT:-1}" "$scenarios_tsv" "$output_dir"
    cp "$output_dir/scenarios.tsv" "$scenarios_tsv"
    mapfile -t case_ids < "$output_dir/selected-cases.txt"
    expected_scenario_count=$(wc -l < "$scenarios_tsv")
    printf 'Shard %s/%s: %d cases, %d scenario exchanges planned.\n' \
        "${WIRE_COMPATIBILITY_SHARD_INDEX:-0}" "${WIRE_COMPATIBILITY_SHARD_COUNT:-1}" \
        "${#case_ids[@]}" "$expected_scenario_count"
else
    printf 'RTI Connext %s: compiling C++ peers for %d cases (wire exchanges disabled).\n' \
        "$rti_version" "${#case_ids[@]}"
fi

end_workflow_group

record_scenario() {
    local case_id="$1"
    local wireloom_type="$2"
    local rti_csharp_type="$3"
    local cpp_type="$4"
    local writer="$5"
    local reader="$6"
    local status="$7"
    local fixture="$8"
    local updated_scenarios="$scenarios_tsv.updated"
    if ! awk -F '\t' -v case_id="$case_id" -v wireloom_type="$wireloom_type" \
        -v rti_csharp_type="$rti_csharp_type" -v cpp_type="$cpp_type" \
        -v writer="$writer" -v reader="$reader" \
        -v version="$rti_version" -v status="$status" -v fixture="$fixture" \
        'BEGIN { OFS = "\t" } {
            if ($1 == case_id && $5 == writer && $7 == reader && $10 == fixture) {
                $2 = wireloom_type
                $3 = rti_csharp_type
                $4 = cpp_type
                $6 = version
                $8 = version
                $9 = status
                updated++
            }
            print
        } END { if (updated != 1) exit 3 }' \
        "$scenarios_tsv" > "$updated_scenarios"; then
        rm -f "$updated_scenarios"
        echo "Could not update the seeded scenario for $case_id [$fixture] $writer -> $reader." >&2
        return 1
    fi
    mv "$updated_scenarios" "$scenarios_tsv"
}

# Wrapper and included IDL can share a basename. The root-generated pair
# belongs to the wrapper and must not count as output for that dependency.
find_rti_csharp_include_files() {
    local generated_dir="$1"
    local root_idl="$2"
    local file_name="$3"
    local root_base
    root_base="$(basename "${root_idl%.*}")"
    find "$generated_dir" -type f -name "$file_name" \
        ! -path "$generated_dir/$root_base.cs" \
        ! -path "$generated_dir/${root_base}Plugin.cs" | sort
}

run_tool() {
    local tool_label="$1"
    local log_path="$2"
    shift 2
    if "$@" > "$log_path" 2>&1; then
        return 0
    fi

    echo "$tool_label failed." >&2
    grep -Ei '(^|[[:space:]])(error|fatal)([:[:space:]]|$)|cannot|not found|undefined reference' \
        "$log_path" | head -n 40 >&2 || true
    return 1
}

start_peer() {
    local peer="$1"
    local role="$2"
    local topic="$3"
    local case_id="$4"
    local wireloom_type="$5"
    local rti_type="$6"
    local fixture="$7"
    if [[ "$peer" == "Wireloom C#" ]]; then
        timeout "$((timeout_seconds + 5))" dotnet "$cs_peer" \
            --case "$case_id" --type "$wireloom_type" --topic "$topic" \
            --runtime-version "$rti_version" --role "$role" --fixture "$fixture"
    elif [[ "$peer" == "RTI C#" ]]; then
        timeout "$((timeout_seconds + 5))" dotnet "$rti_cs_peer" \
            --case "$case_id" --type "$rti_type" --topic "$topic" \
            --runtime-version "$rti_version" --role "$role" --fixture "$fixture"
    else
        timeout "$((timeout_seconds + 5))" "$cpp_peer" "$role" "$topic" "$timeout_seconds" "$case_id" "$fixture"
    fi
}

verify_managed_fixture() {
    local peer_label="$1"
    local peer_dll="$2"
    local peer_type="$3"
    local case_id="$4"
    local fixture="$5"
    local log_path="$case_dir/fixture-verify-${peer_label}-${fixture}.log"
    if ! run_tool "$peer_label typed fixture verification for $case_id [$fixture]" "$log_path" \
        timeout "$((timeout_seconds + 5))" dotnet "$peer_dll" \
        --case "$case_id" --type "$peer_type" --topic "fixture-validation-$case_id" \
        --runtime-version "$rti_version" --role writer --fixture "$fixture" --fixture-only true; then
        overall_status=1
        return 1
    fi
    printf '%s fixture verified: %s\n' "$peer_label" "$fixture"
}

peer_id() {
    case "$1" in
        'Wireloom C#')
            printf 'wireloom-cs'
            ;;
        'RTI C#')
            printf 'rti-cs'
            ;;
        'RTI C++')
            printf 'rti-cpp'
            ;;
        *)
            return 2
            ;;
    esac
}

run_scenario() {
    local case_id="$1"
    local wireloom_type="$2"
    local rti_type="$3"
    local cpp_type="$4"
    local fixture="$5"
    local writer_peer="$6"
    local reader_peer="$7"
    local writer_id
    local reader_id
    writer_id="$(peer_id "$writer_peer")"
    reader_id="$(peer_id "$reader_peer")"
    local topic="wl_${case_id//-/_}_${fixture//-/_}_${writer_id}_${reader_id}_$RANDOM$RANDOM"
    local label="${case_id}-${fixture}-${writer_id}-${reader_id}"
    local reader_log="$scratch/${label}-reader.log"
    local writer_log="$scratch/${label}-writer.log"
    local reader_pid
    local reader_ready=0
    local reader_status=0
    local writer_status=0

    scenario_index=$((scenario_index + 1))
    printf 'WIRE_MATRIX_PROGRESS started=%d/%d completed=%d/%d passed=%d failed=%d elapsed=%ss current=%s [%s] %s -> %s\n' \
        "$scenario_index" "$expected_scenario_count" \
        "$completed_scenario_count" "$expected_scenario_count" \
        "$passed_scenario_count" "$failed_scenario_count" "$SECONDS" \
        "$case_id" "$fixture" "$writer_peer" "$reader_peer"

    if [[ "$rti_cs_available" -eq 0 \
        && ( "$writer_peer" == "RTI C#" || "$reader_peer" == "RTI C#" ) ]]; then
        printf 'FAIL_DETAIL %s\n' "$rti_cs_failure_reason" \
            > "$output_dir/logs/${label}.log"
        record_scenario "$case_id" "$wireloom_type" "$rti_type" "$cpp_type" \
            "$writer_peer" "$reader_peer" FAIL "$fixture" || return 1
        completed_scenario_count=$((completed_scenario_count + 1))
        failed_scenario_count=$((failed_scenario_count + 1))
        progress_percent=$((completed_scenario_count * 100 / expected_scenario_count))
        printf 'WIRE_MATRIX_PROGRESS completed=%d/%d percent=%d%% passed=%d failed=%d elapsed=%ss latest=FAIL %s [%s] %s -> %s\n' \
            "$completed_scenario_count" "$expected_scenario_count" "$progress_percent" \
            "$passed_scenario_count" "$failed_scenario_count" "$SECONDS" \
            "$case_id" "$fixture" "$writer_peer" "$reader_peer"
        return 0
    fi

    if [[ "${wireloom_cs_available:-1}" -eq 0 \
        && ( "$writer_peer" == "Wireloom C#" || "$reader_peer" == "Wireloom C#" ) ]]; then
        printf 'FAIL_DETAIL %s\n' "$wireloom_cs_failure_reason" \
            > "$output_dir/logs/${label}.log"
        record_scenario "$case_id" "$wireloom_type" "$rti_type" "$cpp_type" \
            "$writer_peer" "$reader_peer" FAIL "$fixture" || return 1
        completed_scenario_count=$((completed_scenario_count + 1))
        failed_scenario_count=$((failed_scenario_count + 1))
        progress_percent=$((completed_scenario_count * 100 / expected_scenario_count))
        printf 'WIRE_MATRIX_PROGRESS completed=%d/%d percent=%d%% passed=%d failed=%d elapsed=%ss latest=FAIL %s [%s] %s -> %s\n' \
            "$completed_scenario_count" "$expected_scenario_count" "$progress_percent" \
            "$passed_scenario_count" "$failed_scenario_count" "$SECONDS" \
            "$case_id" "$fixture" "$writer_peer" "$reader_peer"
        return 0
    fi

    # Start the reader first and wait until it has created its DDS entities.
    # This avoids losing the writer's single sample before discovery completes.
    start_peer "$reader_peer" reader "$topic" "$case_id" "$wireloom_type" "$rti_type" "$fixture" > "$reader_log" 2>&1 &
    reader_pid=$!

    for _ in $(seq 1 "$((timeout_seconds * 10))"); do
        if grep -q '^READY ' "$reader_log"; then
            reader_ready=1
            break
        fi
        if ! kill -0 "$reader_pid" 2>/dev/null; then
            reader_status=1
            break
        fi
        sleep 0.1
    done

    if [[ "$reader_status" -eq 0 && "$reader_ready" -eq 1 ]]; then
        if start_peer "$writer_peer" writer "$topic" "$case_id" "$wireloom_type" "$rti_type" "$fixture" > "$writer_log" 2>&1; then
            :
        else
            writer_status=$?
        fi
    else
        printf 'FAIL ReaderReadiness Reader did not become ready for %s fixture %s\n' \
            "$case_id" "$fixture" > "$writer_log"
        writer_status=1
    fi

    if wait "$reader_pid"; then
        :
    else
        reader_status=$?
    fi

    local status=PASS
    if [[ "$writer_status" -ne 0 || "$reader_status" -ne 0 ]]; then
        status=FAIL
    fi

    for endpoint_log in "$writer_log" "$reader_log"; do
        if [[ -f "$endpoint_log" ]]; then
            grep -E '^(READY|PASS|RECEIVED|SENT|FAIL|FAIL_DETAIL|WRITER_MATCHED|OPTIONAL_FIXTURE|READER_STATS|EXPECTED_SAMPLE|ACTUAL_SAMPLE)( |$)' "$endpoint_log" \
                >> "$output_dir/logs/${label}.log" || true
            grep -Ei 'not assignable|type consistency|typeobject|type object|incompatible|data representation|qos policy|representation' "$endpoint_log" \
                | sed 's/^/RTI_DIAGNOSTIC /' >> "$output_dir/logs/${label}.log" || true
        fi
    done

    # Keep child exit codes in the sanitized log. This distinguishes a timeout
    # from native crashes such as SIGSEGV (139), even if the child emitted no
    # structured FAIL line before terminating.
    printf 'ENDPOINT_STATUS writer_peer=%s writer_exit_code=%s reader_peer=%s reader_exit_code=%s\n' \
        "$writer_id" "$writer_status" "$reader_id" "$reader_status" \
        >> "$output_dir/logs/${label}.log"

    record_scenario "$case_id" "$wireloom_type" "$rti_type" "$cpp_type" \
        "$writer_peer" "$reader_peer" "$status" "$fixture" || return 1
    completed_scenario_count=$((completed_scenario_count + 1))
    if [[ "$status" == PASS ]]; then
        passed_scenario_count=$((passed_scenario_count + 1))
    else
        failed_scenario_count=$((failed_scenario_count + 1))
    fi
    progress_percent=$((completed_scenario_count * 100 / expected_scenario_count))
    printf 'WIRE_MATRIX_PROGRESS completed=%d/%d percent=%d%% passed=%d failed=%d elapsed=%ss latest=%s %s [%s] %s -> %s\n' \
        "$completed_scenario_count" "$expected_scenario_count" "$progress_percent" \
        "$passed_scenario_count" "$failed_scenario_count" "$SECONDS" \
        "$status" "$case_id" "$fixture" "$writer_peer" "$reader_peer"
    printf '%s %s [%s] %s -> %s\n' "$status" "$case_id" "$fixture" "$writer_peer" "$reader_peer"
    [[ "$status" == PASS ]]
}

# Restore: the package graph is constant across cases; restore it once before the
# case-specific builds so the full matrix does not repeat the same restore.
dotnet restore "$project" \
    -p:WireCompatibilityRtiVersion="$rti_version" \
    -p:RtiConnextDdsVersion="$rti_version" \
    --locked-mode \
    --verbosity quiet
if [[ "$cpp_compile_only" != "1" ]]; then
    dotnet restore "$rti_csharp_project" \
        -p:WireCompatibilityRtiVersion="$rti_version" \
        -p:RtiConnextDdsVersion="$rti_version" \
        --locked-mode \
        --verbosity quiet
fi

# Unexpected setup errors must fail the job even after peer execution starts.
# Handled peer failures are recorded as FAIL rows and allow the matrix to continue.
overall_status=0
scenario_index=0
SECONDS=0
case_index=0
for case_id in "${case_ids[@]}"; do
    case_index=$((case_index + 1))
    start_workflow_group "Case $case_index/${#case_ids[@]}: $case_id"
    build_phase='csharp-build'
    if [[ "$cpp_compile_only" == "1" ]]; then
        build_phase='cpp-compile'
    fi

    printf 'WIRE_MATRIX_CASE %d/%d case=%s phase=%s completed-scenarios=%d/%d elapsed=%ss\n' \
        "$case_index" "${#case_ids[@]}" "$case_id" \
        "$build_phase" \
        "$completed_scenario_count" "$expected_scenario_count" "$SECONDS"
    case_dir="$scratch/$case_id"
    native_dir="$case_dir/native"
    mkdir -p "$native_dir"
    wireloom_cs_available=1
    wireloom_cs_failure_reason="Wireloom C# build failed for $case_id"
    mapfile -t case_pairings_for_case < <(case_pairings "$case_id")
    rti_csharp_required=0
    if case_uses_rti_csharp "$case_id"; then
        rti_csharp_required=1
    fi
    rti_cs_available=0
    rti_type_name=""

    # Build Wireloom's C# output against the selected RTI runtime. The resulting
    # generated type name is discovered from Wireloom's emitted TypeSupport.
    if [[ -n "${rti_only_cases[$case_id]:-}" ]]; then
        type_name=""
        cpp_type="CorpusOptionalAggregate::Holder"
        idl_relative="idl/features/09-optional-aggregate-member.idl"
        case_defines=()
    else
        build_args=(
            --no-restore --configuration Release
            -t:Rebuild
            -p:WireCompatibilityCase="$case_id"
            -p:WireCompatibilityRtiVersion="$rti_version"
            -p:RtiConnextDdsVersion="$rti_version"
            --verbosity quiet
        )
        case_idl="$(matrix_tool case-field "$manifest" "$case_id" idl)"
        sequence_array_mapping="$(matrix_tool case-field "$manifest" "$case_id" sequence-array-mapping)"
        generated_root="$case_dir/wireloom-generated"
        build_args+=( "-p:WireCompatibilityIdl=$repo_root/docs/corpus/$case_idl" "-p:DdsIdlSequenceArrayMapping=$sequence_array_mapping" )
        build_args+=( "-p:WireCompatibilityGeneratedOutputPath=$generated_root" )
        if [[ "$case_id" == "05-array-of-sequences" ]]; then
            # Preserve DDSG0105 as a warning in this known-limitation case while
            # keeping the repository's warnings-as-errors policy for all others.
            build_args+=( -p:WarningsNotAsErrors=DDSG0105 )
        fi
        wireloom_build_log="$case_dir/wireloom-csharp-build.log"
        if ! dotnet build "$project" "${build_args[@]}" > "$wireloom_build_log" 2>&1; then
            wireloom_cs_available=0
            overall_status=1
            compiler_error="$(sed -n 's/.*: error \(CS[0-9]*: .*\) \[.*$/\1/p' "$wireloom_build_log" | head -n 1 || true)"
            if [[ -n "$compiler_error" ]]; then
                wireloom_cs_failure_reason+="; $compiler_error"
            fi
            echo "$wireloom_cs_failure_reason; pairings requiring this peer will be marked failed." >&2
            grep -Ei 'error|warning|failed|exception' "$wireloom_build_log" | head -n 30 >&2 || true
        elif [[ "$cpp_compile_only" == "1" ]]; then
            tail -n 8 "$wireloom_build_log"
        fi

        if [[ "$wireloom_cs_available" == "1" ]]; then
            matrix_tool generation-provenance "$manifest" "$case_id" "$generated_root" "$rti_version" \
                "$output_dir/logs/$case_id-generation.json"
        fi
        registry="$case_dir/case.json"
        matrix_tool discover-cases \
            "$manifest" "$generated_root" "$registry" "$case_id" >/dev/null

        mapfile -t discovered < <(matrix_tool case-info "$registry" "$manifest" "$case_id")
        type_name="${discovered[0]}"
        cpp_type="${discovered[1]}"
        idl_relative="${discovered[2]}"
        case_defines=("${discovered[@]:3}")
    fi
    mapfile -t rti_cpp_codegen_options < <(matrix_tool case-field "$manifest" "$case_id" rti-cpp-codegen-options)
    mapfile -t rti_cpp_idl_adaptations < <(matrix_tool case-field "$manifest" "$case_id" rti-cpp-idl-adaptations)
    cs_peer="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/bin/Release/net10.0/WireCompatibility.${case_id}.dll"

    define_args=()
    for define in "${case_defines[@]}"; do
        [[ -z "$define" ]] && continue
        define_args+=( -D "$define" )
    done
    idl_path="$repo_root/docs/corpus/$idl_relative"
    wrapper_name="$case_id.idl"
    case "$case_id" in
        14-autoid-scope) wrapper_name=14-autoid-scope-topic.idl ;;
        14-enum-constant) wrapper_name=14-enum-constant-topic.idl ;;
    esac
    if [[ -f "$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/IdlWrappers/$wrapper_name" ]]; then
        idl_path="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/IdlWrappers/$wrapper_name"
    fi
    if [[ "$cpp_compile_only" != "1" && "$rti_csharp_required" == "1" ]]; then
        rti_csharp_dir="$case_dir/rti-csharp-generated"
        rti_cs_peer="$case_dir/rti-bin/WireCompatibility.RtiOracleProbe.dll"
        rti_build_log="$case_dir/rti-csharp-build.log"
        oracle_registry="$case_dir/rti-oracle.json"
        rti_csharp_generated=0
        rti_cs_failure_reason="RTI C# generation failed for $case_id"
        rti_type_name=""
        mkdir -p "$rti_csharp_dir" "$case_dir/rti-bin" "$case_dir/rti-obj"
        rti_csharp_codegen_input="$idl_path"
        if [[ "$rti_version" == "7.3.1" \
            && ( "$case_id" == "14-autoid-scope" || "$case_id" == "14-enum-constant" ) ]]; then
            # RTI 4.3.1 resolves the canonical include by basename beside the
            # wrapper, even when the include retains its features/ path. Feed
            # exact copies through an isolated scratch tree to satisfy that
            # resolution while leaving the canonical corpus untouched.
            rti_csharp_input_dir="$case_dir/rti-csharp-input"
            rti_csharp_input_evidence="$output_dir/raw/$case_id/rti-csharp-input"
            mkdir -p "$rti_csharp_input_dir" "$rti_csharp_input_evidence"
            cp "$idl_path" "$rti_csharp_input_dir/$(basename "$idl_path")"
            cp "$idl_path" "$rti_csharp_input_evidence/$(basename "$idl_path")"
            case "$case_id" in
                14-autoid-scope)
                    canonical_source="$repo_root/docs/corpus/idl/features/14-autoid-scope.idl"
                    dependency_source="$repo_root/docs/corpus/idl/includes/autoid-hash-include.idl"
                    ;;
                14-enum-constant)
                    canonical_source="$repo_root/docs/corpus/idl/features/14-enum-constant.idl"
                    dependency_source=""
                    ;;
            esac
            cp "$canonical_source" "$rti_csharp_input_dir/$case_id.idl"
            cp "$canonical_source" "$rti_csharp_input_evidence/$case_id.idl"
            if [[ -n "$dependency_source" ]]; then
                cp "$dependency_source" "$rti_csharp_input_dir/$(basename "$dependency_source")"
                cp "$dependency_source" "$rti_csharp_input_evidence/$(basename "$dependency_source")"
            fi
            {
                printf 'source\tstaged\tsource_sha256\tstaged_sha256\n'
                for staged_source in "$idl_path" "$canonical_source"; do
                    staged_name="$(basename "$staged_source")"
                    if [[ "$staged_source" == "$idl_path" ]]; then
                        staged_name="$(basename "$idl_path")"
                    else
                        staged_name="$case_id.idl"
                    fi
                    printf '%s\t%s\t%s\t%s\n' \
                        "${staged_source#"$repo_root"/}" "$staged_name" \
                        "$(sha256sum "$staged_source" | cut -d' ' -f1)" \
                        "$(sha256sum "$rti_csharp_input_dir/$staged_name" | cut -d' ' -f1)"
                done
                if [[ -n "$dependency_source" ]]; then
                    staged_name="$(basename "$dependency_source")"
                    printf '%s\t%s\t%s\t%s\n' \
                        "${dependency_source#"$repo_root"/}" "$staged_name" \
                        "$(sha256sum "$dependency_source" | cut -d' ' -f1)" \
                        "$(sha256sum "$rti_csharp_input_dir/$staged_name" | cut -d' ' -f1)"
                fi
            } > "$rti_csharp_input_evidence/source-hashes.tsv"
            printf 'RTI 7.3.1 C# include staging: preserved original wrapper/canonical/dependency bytes and staged copies under %s; SHA-256 mappings are in %s/source-hashes.tsv.\n' \
                "$rti_csharp_input_dir" "$rti_csharp_input_evidence" \
                >> "$output_dir/logs/$case_id-rti-csharp-include-adaptation.log"
            rti_csharp_codegen_input="$rti_csharp_input_dir/$(basename "$idl_path")"
        fi
        rti_csharp_codegen_args=(
            -language "C#" -replace -generateIncludeFiles -d "$rti_csharp_dir"
            "${define_args[@]}"
            -I "$repo_root/docs/corpus/idl"
            -I "$repo_root/docs/corpus/idl/includes"
            "$rti_csharp_codegen_input"
        )
        if run_tool "RTI C# generation for $case_id" "$case_dir/rtiddsgen-csharp.log" \
            "$NDDSHOME/bin/rtiddsgen" "${rti_csharp_codegen_args[@]}"; then
            rti_csharp_generated=1
            mapfile -t rti_includes < <(matrix_tool includes-recursive \
                "$idl_path" \
                "$repo_root/docs/corpus/idl" \
                "$repo_root/docs/corpus/idl/includes")
            include_index=0
            for include_entry in "${rti_includes[@]}"; do
                IFS=$'\t' read -r include_path include_source <<< "$include_entry"
                [[ -z "$include_source" ]] && continue
                include_base="$(basename "${include_path%.*}")"
                main_include_source="$rti_csharp_dir/includes/$include_base.cs"
                main_include_plugin="$rti_csharp_dir/includes/${include_base}Plugin.cs"
                include_adaptation_log="$output_dir/logs/$case_id-rti-csharp-include-adaptation.log"
                mapfile -t include_sources < <(find_rti_csharp_include_files "$rti_csharp_dir" "$idl_path" "$include_base.cs")
                mapfile -t include_plugins < <(find_rti_csharp_include_files "$rti_csharp_dir" "$idl_path" "${include_base}Plugin.cs")
                source_count="${#include_sources[@]}"
                plugin_count="${#include_plugins[@]}"
                if [[ ( "$case_id" == "14-autoid-scope" || "$case_id" == "14-enum-constant" ) \
                    && "$source_count" == "1" && "$plugin_count" == "1" \
                    && -s "${include_sources[0]}" && -s "${include_plugins[0]}" \
                    && "$(dirname "${include_sources[0]}")" == "$(dirname "${include_plugins[0]}")" ]]; then
                    expected_content_valid=1
                    case "$include_path" in
                        features/14-autoid-scope.idl)
                            for expected_type in Hashed Reopened Sequential UsesInclude; do
                                if ! grep -Fq "$expected_type" "${include_sources[0]}" \
                                    && ! grep -Fq "$expected_type" "${include_plugins[0]}"; then
                                    expected_content_valid=0
                                fi
                            done
                            ;;
                        includes/autoid-hash-include.idl)
                            if ! grep -Fq "Included" "${include_sources[0]}" \
                                && ! grep -Fq "Included" "${include_plugins[0]}"; then
                                expected_content_valid=0
                            fi
                            ;;
                        features/14-enum-constant.idl)
                            for expected_type in EnumConstant EnumUnion; do
                                if ! grep -Fq "$expected_type" "${include_sources[0]}" \
                                    && ! grep -Fq "$expected_type" "${include_plugins[0]}"; then
                                    expected_content_valid=0
                                fi
                            done
                            ;;
                    esac
                    if [[ "$expected_content_valid" == "1" ]]; then
                        raw_include_dir="$output_dir/raw/$case_id/rti-csharp-include-raw"
                        mkdir -p "$raw_include_dir"
                        cp "${include_sources[0]}" "$raw_include_dir/$include_base-$include_index.cs"
                        cp "${include_plugins[0]}" "$raw_include_dir/${include_base}Plugin-$include_index.cs"
                        printf 'Reused the unique RTI C# root-generated outputs for %s from %s; SHA-256 %s / %s.\n' \
                            "$include_path" "$(dirname "${include_sources[0]}")" \
                            "$(sha256sum "$raw_include_dir/$include_base-$include_index.cs" | cut -d' ' -f1)" \
                            "$(sha256sum "$raw_include_dir/${include_base}Plugin-$include_index.cs" | cut -d' ' -f1)" \
                            >> "$include_adaptation_log"
                        continue
                    fi
                    echo "RTI C# output for $include_path is missing expected canonical types." >&2
                    rti_csharp_generated=0
                    rti_cs_failure_reason="RTI C# include content mismatch for $case_id"
                    break
                fi
                if [[ "$case_id" != "14-autoid-scope" && "$case_id" != "14-enum-constant" && "$source_count" == "1" \
                    && "$plugin_count" == "1" && -f "$main_include_source" \
                    && -f "$main_include_plugin" ]]; then
                    printf 'Reused RTI C# include outputs from root generation: %s (%s, %s).\n' \
                        "$include_path" "$main_include_source" "$main_include_plugin" \
                        >> "$case_dir/rti-csharp-include-adaptation.log"
                    continue
                fi
                nested_include_source="$rti_csharp_dir/includes/includes/$include_base.cs"
                nested_include_plugin="$rti_csharp_dir/includes/includes/${include_base}Plugin.cs"
                if [[ ( "$case_id" == "14-autoid-scope" || "$case_id" == "14-enum-constant" ) && "$source_count" == "2" \
                    && "$plugin_count" == "2" && -f "$main_include_source" \
                    && -f "$main_include_plugin" && -f "$nested_include_source" \
                    && -f "$nested_include_plugin" ]] \
                    && cmp -s "$main_include_source" "$nested_include_source" \
                    && cmp -s "$main_include_plugin" "$nested_include_plugin"; then
                    raw_include_dir="$output_dir/raw/$case_id/rti-csharp-include-raw"
                    mkdir -p "$raw_include_dir"
                    cp "$main_include_source" "$raw_include_dir/$include_base-main.cs"
                    cp "$nested_include_source" "$raw_include_dir/$include_base-nested.cs"
                    cp "$main_include_plugin" "$raw_include_dir/${include_base}Plugin-main.cs"
                    cp "$nested_include_plugin" "$raw_include_dir/${include_base}Plugin-nested.cs"
                    printf 'RTI %s C# root generation emitted duplicate identical include files; retained raw copies under %s, SHA-256: %s / %s / %s / %s; removed only nested copies %s and %s.\n' \
                        "$rti_version" \
                        "$raw_include_dir" \
                        "$(sha256sum "$raw_include_dir/$include_base-main.cs" | cut -d' ' -f1)" \
                        "$(sha256sum "$raw_include_dir/$include_base-nested.cs" | cut -d' ' -f1)" \
                        "$(sha256sum "$raw_include_dir/${include_base}Plugin-main.cs" | cut -d' ' -f1)" \
                        "$(sha256sum "$raw_include_dir/${include_base}Plugin-nested.cs" | cut -d' ' -f1)" \
                        "$nested_include_source" "$nested_include_plugin" \
                        >> "$include_adaptation_log"
                    rm -f -- "$nested_include_source" "$nested_include_plugin"
                    continue
                fi
                if [[ "$source_count" != "0" || "$plugin_count" != "0" ]]; then
                    echo "RTI C# include output set is incomplete or duplicated for $include_path ($source_count source, $plugin_count plugin files)." >&2
                    rti_csharp_generated=0
                    rti_cs_failure_reason="RTI C# include output mismatch for $case_id"
                    break
                fi
                include_dir="$rti_csharp_dir/includes/$include_index"
                mkdir -p "$include_dir"
                include_codegen_args=(
                    -language "C#" -replace -generateIncludeFiles -d "$include_dir"
                    "${define_args[@]}"
                    -I "$repo_root/docs/corpus/idl"
                    -I "$repo_root/docs/corpus/idl/includes"
                    "$include_source"
                )
                if ! run_tool "RTI C# generation for included IDL $include_path" \
                    "$case_dir/rtiddsgen-csharp-include-$include_index.log" \
                    "$NDDSHOME/bin/rtiddsgen" "${include_codegen_args[@]}"; then
                    rti_csharp_generated=0
                    break
                fi
                include_index=$((include_index + 1))
            done
            if [[ "$rti_csharp_generated" == 1 && "$case_id" == "10-resolve-name-wstring-aliases" ]]; then
                # RTI emits two global::Implementation references from a
                # top-level namespace. Qualify them with the IDL module in the
                # scratch peer so this otherwise-valid C# oracle can be tested.
                rti_plugin_file="$rti_csharp_dir/${case_id}Plugin.cs"
                if [[ "$(grep -Fc 'global::Implementation.BoundedWideTextUnmanaged' "$rti_plugin_file" || true)" -ne 1 \
                    || "$(grep -Fc 'global::Implementation.BoundedWideTextAliasUnmanaged' "$rti_plugin_file" || true)" -ne 1 ]]; then
                    echo "RTI C# generated member types changed for $case_id; update the guarded namespace workaround." >&2
                    rti_csharp_generated=0
                    rti_cs_failure_reason="RTI C# workaround source mismatch for $case_id"
                else
                    sed -i \
                        's/global::Implementation\.BoundedWideTextUnmanaged/global::CorpusResolveNameWstringAliases.Implementation.BoundedWideTextUnmanaged/; s/global::Implementation\.BoundedWideTextAliasUnmanaged/global::CorpusResolveNameWstringAliases.Implementation.BoundedWideTextAliasUnmanaged/' \
                        "$rti_plugin_file"
                fi
            fi
            if [[ "$rti_csharp_generated" == 1 && "$case_id" == "06-alias-composition" ]]; then
                # RTI 7.7.0 shadows this generated member with a conversion
                # parameter, which otherwise produces invalid C# calls.
                sed -i -E \
                    's/^([[:space:]]*)sample\.(FromNative|ToNative)\(sample\.sample/\1this.sample.\2(sample.sample/' \
                    "$rti_csharp_dir/${case_id}Plugin.cs"
            fi
        fi
        if [[ "$rti_csharp_generated" == 1 && "$case_id" == 07-union-char-numeric* ]]; then
            # Apply the documented post-generation binding corrections, preserving
            # declared character values and the incoming fallback discriminator.
            matrix_tool adapt-rti-characters csharp "$case_id" "$rti_csharp_dir"
        fi
        if [[ "$rti_csharp_generated" == 1 ]]; then
            rti_cs_failure_reason="RTI C# generated-type discovery failed for $case_id"
        fi
        if [[ "$rti_csharp_generated" == 1 ]] && matrix_tool discover-rti \
                "$manifest" "$case_id" "$type_name" "$rti_csharp_dir" "$oracle_registry"; then
            rti_type_name="$(matrix_tool json-field "$oracle_registry" rtiCSharpType)"
            if dotnet build "$rti_csharp_project" --no-restore --configuration Release --verbosity quiet \
                -p:RtiOracleSourceDir="$rti_csharp_dir" \
                -p:WireCompatibilityCase="$case_id" \
                -p:BaseIntermediateOutputPath="$case_dir/rti-obj/" \
                -p:MSBuildProjectExtensionsPath="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/RtiOracleProbe/obj/" \
                -p:OutputPath="$case_dir/rti-bin/" > "$rti_build_log" 2>&1; then
                rti_cs_available=1
                echo "RTI-generated C# peer compiled: $rti_type_name"
            else
                rti_cs_available=0
                overall_status=1
                rti_cs_failure_reason="RTI-generated C# peer failed to compile for $case_id"
                compiler_error="$(sed -n 's/.*: error \(CS[0-9]*: .*\) \[.*$/\1/p' "$rti_build_log" | head -n 1 || true)"
                if [[ -n "$compiler_error" ]]; then
                    rti_cs_failure_reason+="; $compiler_error"
                fi
                echo "$rti_cs_failure_reason; its matrix pairings will be marked failed." >&2
                grep -Ei 'error|warning|failed|exception' "$rti_build_log" | head -n 30 >&2 || true
            fi
        else
            rti_cs_available=0
            overall_status=1
            echo "$rti_cs_failure_reason; its matrix pairings will be marked failed." >&2
        fi
    fi
    # RTI C++ build: normally both peers use the same corpus IDL. These two adaptations keep
    # RTI C++ generation possible while preserving the intended wire shape:
    # rtiddsgen rejects wchar discriminators; the FlatData input has incompatible
    # default extensibility, so its reference uses plain C++ mapping.
    if [[ "$case_id" == "07-union-wchar-label" ]]; then
        cp "$idl_path" "$native_dir/idl.idl"
    elif [[ "$case_id" == "10-flat-data-binding" ]]; then
        # RTI ignores this mapping on the C# side. Generate a standard C++ peer
        # while preserving the original type shape and XCDR2 requirement.
        sed '/@language_binding(FLAT_DATA)/d' "$idl_path" > "$native_dir/idl.idl"
    elif [[ " ${rti_cpp_idl_adaptations[*]} " == *" ${rti_version}:numeric-char-labels "* ]]; then
        # Numeric char labels preserve the declared values in RTI scratch IDL.
        sed "s/case '\\\\0':/case 0:/; s/case '~':/case 126:/" \
            "$idl_path" > "$native_dir/idl.idl"
    else
        cp "$idl_path" "$native_dir/idl.idl"
    fi
    rtiddsgen_args=(
        -language C++11 -replace
        "${rti_cpp_codegen_options[@]}"
        "${define_args[@]}"
        -I "$repo_root/docs/corpus/idl"
        -I "$repo_root/docs/corpus/idl/includes"
    )
    if [[ "$rti_version" == "7.7.0" ]]; then
        rtiddsgen_args+=( -generateIncludeFiles )
    fi
    rtiddsgen_args+=( -d "$native_dir" "$native_dir/idl.idl" )
    if [[ "$case_id" != "07-union-wchar-label" ]]; then
        if ! run_tool "rtiddsgen for $case_id" "$case_dir/rtiddsgen.log" \
            "$NDDSHOME/bin/rtiddsgen" "${rtiddsgen_args[@]}"; then
            printf '%s\trtiddsgen-failed\tsee %s\n' "$case_id" "$case_dir/rtiddsgen.log" >> "$cpp_compilation_tsv"
            overall_status=1
            end_workflow_group
            continue
        fi
    fi
    if [[ "$case_id" == "10-resolve-name-wstring-aliases" ]]; then
        if [[ "$(grep -Fc 'dynamic_type< BoundedWideText>' "$native_dir/idl.cxx" || true)" -ne 1 \
            || "$(grep -Fc 'dynamic_type< BoundedWideTextAlias>' "$native_dir/idl.cxx" || true)" -ne 1 ]]; then
            echo "RTI C++ generated TypeCode references changed for $case_id; update the guarded alias-tag workaround." >&2
            printf '%s\tworkaround-source-mismatch\tsee %s/rtiddsgen.log\n' "$case_id" "$case_dir" >> "$cpp_compilation_tsv"
            overall_status=1
            end_workflow_group
            continue
        fi
        sed -i \
            's/dynamic_type< BoundedWideText>/dynamic_type< ::CorpusResolveNameWstringAliases::BoundedWideText_AliasTag_t>/g; s/dynamic_type< BoundedWideTextAlias>/dynamic_type< ::CorpusResolveNameWstringAliases::BoundedWideTextAlias_AliasTag_t>/g' \
            "$native_dir/idl.cxx"
    fi

    declare -A generated_include_sources=()
    while IFS=$'\t' read -r include_path include_source; do
        [[ -z "$include_path" ]] && continue
        include_base="$(basename "${include_path%.*}")"
        include_directory="$(dirname "$include_path")"
        reuse_generated_include=0
        if [[ ( "$case_id" == "14-autoid-scope" || "$case_id" == "14-enum-constant" ) \
            && "$include_directory" != "." \
            && "$rti_version" == "7.7.0" ]]; then
            expected_include_dir="$native_dir/$include_directory"
            expected_include_files=(
                "$expected_include_dir/$include_base.cxx"
                "$expected_include_dir/$include_base.hpp"
                "$expected_include_dir/${include_base}Plugin.cxx"
                "$expected_include_dir/${include_base}Plugin.hpp"
            )
            present_count=0
            for expected_file in "${expected_include_files[@]}"; do
                if [[ -s "$expected_file" ]]; then
                    present_count=$((present_count + 1))
                fi
            done
            if [[ "$present_count" == "4" ]]; then
                for expected_file in "${expected_include_files[@]}"; do
                    file_count="$(find "$native_dir" -type f -name "$(basename "$expected_file")" | wc -l)"
                    if [[ "$file_count" != "1" ]]; then
                        echo "RTI C++ generated include count is $file_count for $expected_file (expected one)." >&2
                        overall_status=1
                        end_workflow_group
                        continue 2
                    fi
                done
                reuse_generated_include=1
            elif [[ "$present_count" != "0" ]]; then
                echo "RTI C++ generated include set is incomplete for $include_path ($present_count of 4 files)." >&2
                overall_status=1
                end_workflow_group
                continue 2
            fi
        fi
        if [[ "$reuse_generated_include" == "0" \
            && -z "${generated_include_sources[$include_source]:-}" ]]; then
            dependency_args=(
                -language C++11 -replace -d "$native_dir"
                "${define_args[@]}"
                -I "$repo_root/docs/corpus/idl"
                -I "$repo_root/docs/corpus/idl/includes"
                "$include_source"
            )
            if ! run_tool "rtiddsgen include for $case_id" "$case_dir/rtiddsgen-include.log" \
                "$NDDSHOME/bin/rtiddsgen" "${dependency_args[@]}"; then
                printf '%s\trtiddsgen-include-failed\tsee %s\n' "$case_id" "$case_dir/rtiddsgen-include.log" >> "$cpp_compilation_tsv"
                overall_status=1
                end_workflow_group
                continue 2
            fi
            generated_include_sources[$include_source]=1
        fi

        if [[ "$reuse_generated_include" == "1" ]]; then
            raw_include_dir="$output_dir/raw/$case_id/rti-cpp-include-raw"
            mkdir -p "$raw_include_dir"
            for expected_file in "${expected_include_files[@]}"; do
                cp "$expected_file" "$raw_include_dir/$include_base-$(basename "$expected_file")"
            done
            {
                printf 'Reused RTI C++ recursively generated outputs for %s from %s; hashes follow.\n' \
                    "$include_path" "$expected_include_dir"
                sha256sum "${expected_include_files[@]}"
            } >> "$output_dir/logs/$case_id-rti-cpp-include-adaptation.log"
            continue
        fi

        if [[ "$include_directory" != "." ]]; then
            mkdir -p "$native_dir/$include_directory"
            cp "$native_dir/$include_base.hpp" "$native_dir/$include_directory/$include_base.hpp"
            cp "$native_dir/${include_base}Plugin.hpp" \
                "$native_dir/$include_directory/${include_base}Plugin.hpp"
            cmp -s "$native_dir/$include_base.hpp" "$native_dir/$include_directory/$include_base.hpp"
            cmp -s "$native_dir/${include_base}Plugin.hpp" \
                "$native_dir/$include_directory/${include_base}Plugin.hpp"
        fi
        if [[ "$case_id" == "14-autoid-scope" ]]; then
            raw_include_dir="$output_dir/raw/$case_id/rti-cpp-include-raw"
            mkdir -p "$raw_include_dir"
            cp "$native_dir/$include_base.cxx" "$raw_include_dir/$include_base-generated.cxx"
            cp "$native_dir/${include_base}Plugin.cxx" "$raw_include_dir/${include_base}Plugin-generated.cxx"
            cp "$native_dir/$include_base.hpp" "$raw_include_dir/$include_base-generated.hpp"
            cp "$native_dir/${include_base}Plugin.hpp" "$raw_include_dir/${include_base}Plugin-generated.hpp"
            if [[ "$include_directory" != "." ]]; then
                cp "$native_dir/$include_directory/$include_base.hpp" "$raw_include_dir/$include_base-staged.hpp"
                cp "$native_dir/$include_directory/${include_base}Plugin.hpp" "$raw_include_dir/${include_base}Plugin-staged.hpp"
                {
                    printf 'RTI C++ include output %s staged under %s and compared byte-for-byte.\n' \
                        "$include_path" "$include_directory"
                    sha256sum "$raw_include_dir/$include_base-generated.cxx" \
                        "$raw_include_dir/${include_base}Plugin-generated.cxx" \
                        "$raw_include_dir/$include_base-generated.hpp" \
                        "$raw_include_dir/${include_base}Plugin-generated.hpp" \
                        "$raw_include_dir/$include_base-staged.hpp" \
                        "$raw_include_dir/${include_base}Plugin-staged.hpp"
                } >> "$output_dir/logs/$case_id-rti-cpp-include-adaptation.log"
            fi
        fi
    done < <(
        if [[ "$case_id" == "14-autoid-scope" ]]; then
            matrix_tool includes-recursive "$native_dir/idl.idl" \
                "$repo_root/docs/corpus/idl" "$repo_root/docs/corpus/idl/includes"
        else
            matrix_tool includes "$native_dir/idl.idl" \
                "$repo_root/docs/corpus/idl" "$repo_root/docs/corpus/idl/includes"
        fi
    )
    cpp_template_type="$cpp_type"
    cmake_adaptation_args=()
    if [[ "$rti_version" == "7.3.1" ]]; then
        cmake_adaptation_args+=( -DWIRELOOM_RTI_7_3_1=ON )
    fi
    if ccache_path="$(command -v ccache || true)" && [[ -n "$ccache_path" ]]; then
        cmake_adaptation_args+=( "-DCMAKE_CXX_COMPILER_LAUNCHER=$ccache_path" )
    fi
    if [[ "$case_id" == "07-union-wchar-label" ]]; then
        cpp_template_type="dds::core::xtypes::DynamicData"
        cmake_adaptation_args+=( -DWIRELOOM_DYNAMIC_WCHAR_UNION=ON )
    fi
    if [[ "$case_id" == "09-optional-string-sequences" ]]; then
        cmake_adaptation_args+=( -DWIRELOOM_TYPED_STRING_SEQUENCE_FIXTURE=ON )
    fi
    if [[ "$case_id" == "04-strings" ]]; then
        cmake_adaptation_args+=( -DWIRELOOM_TYPED_WIDE_STRINGS_FIXTURE=ON )
    fi
    case "$case_id" in
        04-unbounded-string-aliases) cmake_adaptation_args+=( -DWIRELOOM_TYPED_UNBOUNDED_STRING_ALIASES=ON ) ;;
        10-resolve-name-wstring-aliases) cmake_adaptation_args+=( -DWIRELOOM_TYPED_RESOLVE_NAME_WSTRING_ALIASES=ON ) ;;
        10-default-literal-enum-array) cmake_adaptation_args+=( -DWIRELOOM_TYPED_DEFAULT_LITERAL_ENUM_ARRAY=ON ) ;;
        07-union-char-numeric|07-union-char-numeric-255)
            cmake_adaptation_args+=( -DWIRELOOM_TYPED_NUMERIC_CHAR_UNION=ON )
            if [[ "$case_id" == "07-union-char-numeric-255" ]]; then
                cmake_adaptation_args+=( -DWIRELOOM_UNSIGNED_CHAR_NUMERIC_UNION=ON )
            fi
            ;;
        04-string-alias-chains) cmake_adaptation_args+=( -DWIRELOOM_TYPED_STRING_ALIAS_CHAINS=ON ) ;;
        05-unbounded-sequence-aliases) cmake_adaptation_args+=( -DWIRELOOM_TYPED_UNBOUNDED_SEQUENCE_ALIASES=ON ) ;;
        05-array-of-sequences-idl) cmake_adaptation_args+=( -DWIRELOOM_TYPED_SEQUENCE_ARRAYS=ON ) ;;
        07-union-string-sequence-aliases) cmake_adaptation_args+=( -DWIRELOOM_TYPED_UNION_STRING_SEQUENCE_ALIASES=ON ) ;;
        07-union-string-sequence-bounded-direct) cmake_adaptation_args+=( -DWIRELOOM_TYPED_UNION_STRING_SEQUENCE_BOUNDED=ON ) ;;
        14-extensibility-full) cmake_adaptation_args+=( -DWIRELOOM_TYPED_C163_EXTENSIBILITY=ON ) ;;
        14-autoid-scope) cmake_adaptation_args+=( -DWIRELOOM_TYPED_C164_AUTOID_SCOPE=ON ) ;;
        14-annotation-arguments) cmake_adaptation_args+=( -DWIRELOOM_TYPED_C165_ANNOTATION_ARGUMENTS=ON ) ;;
        14-member-declarators) cmake_adaptation_args+=( -DWIRELOOM_TYPED_C166_MEMBER_DECLARATORS=ON ) ;;
        14-union-label-expressions) cmake_adaptation_args+=( -DWIRELOOM_TYPED_C167_UNION_LABELS=ON ) ;;
        14-union-scoped-enum) cmake_adaptation_args+=( -DWIRELOOM_TYPED_C168_SCOPED_ENUM=ON ) ;;
        14-union-array-branch) cmake_adaptation_args+=( -DWIRELOOM_TYPED_C169_UNION_ARRAY=ON ) ;;
        14-string-expression-bound) cmake_adaptation_args+=( -DWIRELOOM_TYPED_C170_STRING_BOUND=ON ) ;;
        14-typed-defaults) cmake_adaptation_args+=( -DWIRELOOM_TYPED_C171_DEFAULTS=ON ) ;;
        14-enum-constant) cmake_adaptation_args+=( -DWIRELOOM_TYPED_C172_ENUM_CONSTANT=ON ) ;;
    esac
    if [[ "$case_id" == "04-boundaries" ]]; then
        cmake_adaptation_args+=( -DWIRELOOM_TYPED_STRING_BOUNDARIES_FIXTURE=ON )
    fi
    if [[ "$case_id" == "09-optional-aggregate-member" ]]; then
        cmake_adaptation_args+=( -DWIRELOOM_TYPED_OPTIONAL_AGGREGATE_FIXTURE=ON )
    fi
    if [[ "$case_id" == "09-optional-recursive-array" ]]; then
        # RTI 4.7.0 omits the visited-node chain when initializing the recursive
        # array element TypeCode, causing unbounded metadata recursion.
        # Adjust scratch reference code only; the declared IDL shape is intact.
        grep -F 'Node_g_tc_children_array._data._typeCode' "$native_dir/idl.cxx" >/dev/null
        sed -i '/Node_g_tc_children_array._data._typeCode/s/::get().native()/::get(\&node).native()/' "$native_dir/idl.cxx"
        if [[ "$rti_version" == "7.3.1" ]]; then
            # Code Generator 4.3.1 emits this recursive TypeCode reference
            # without the containing IDL module qualification.
            sed -i 's/dynamic_type< Node>/dynamic_type< ::CorpusOptionalRecursiveArray::Node>/g' \
                "$native_dir/idl.cxx"
        fi
        cmake_adaptation_args+=( -DWIRELOOM_TYPED_OPTIONAL_RECURSIVE_ARRAY_FIXTURE=ON )
    fi
    if [[ "$case_id" == "06-aggregates" && "$rti_version" == "7.3.1" ]]; then
        # Code Generator 4.3.1 has the same missing qualification for the
        # recursive aggregate TypeCode reference.
        sed -i 's/dynamic_type< Recursive>/dynamic_type< ::CorpusAggregates::Recursive>/g' \
            "$native_dir/idl.cxx"
    fi
    if [[ "$case_id" == "07-union-char-numeric-255" ]]; then
        # RTI's native char label lookup requires the signed byte representation.
        matrix_tool adapt-rti-characters cpp "$case_id" "$native_dir"
    fi
    sed "s|@IDL_BASE@|idl|g; s|@CPP_TYPE@|$cpp_template_type|g" \
        "$native_template/main.cpp.in" > "$native_dir/main.cpp"
    cp "$native_template/typed-fixture-peer.hpp.in" "$native_dir/typed-fixture-peer.hpp"
    cp "$native_template/peer-qos.hpp" "$native_dir/peer-qos.hpp"

    cp "$native_template/CMakeLists.txt" "$native_dir/CMakeLists.txt"
    if ! run_tool "CMake configure for $case_id" "$case_dir/cmake-configure.log" \
        cmake -S "$native_dir" -B "$native_dir/build" \
        -DCMAKE_MODULE_PATH="$NDDSHOME/resource/cmake" \
        -DCONNEXTDDS_ARCH="$connextdds_arch" "${cmake_adaptation_args[@]}"; then
        printf '%s\tcmake-configure-failed\tsee %s\n' "$case_id" "$case_dir/cmake-configure.log" >> "$cpp_compilation_tsv"
        overall_status=1
        end_workflow_group
        continue
    fi
    if ! run_tool "C++ build for $case_id" "$case_dir/cmake-build.log" \
        cmake --build "$native_dir/build" --parallel 2; then
        overall_status=1
        echo "RTI C++ peer failed to compile for $case_id; see $case_dir/cmake-build.log." >&2
        printf '%s\tcompile-failed\tsee %s\n' "$case_id" "$case_dir/cmake-build.log" >> "$cpp_compilation_tsv"
        end_workflow_group
        continue
    fi
    printf '%s\tcompiled\t%s\n' "$case_id" "$native_dir/build/wire-peer" >> "$cpp_compilation_tsv"
    cpp_peer="$native_dir/build/wire-peer"
    printf '%s\t%s\t%s\n' "$case_id" "$type_name" "$cpp_type" >> "$cases_tsv"

    if [[ "$case_id" == 14-* ]]; then
        mapfile -t case_fixtures < <(fixtures_for_case "$case_id")
        for fixture in "${case_fixtures[@]}"; do
            verify_managed_fixture "wireloom-cs" "$cs_peer" "$type_name" "$case_id" "$fixture" || true
            if [[ "${rti_cs_available:-0}" == "1" ]]; then
                verify_managed_fixture "rti-cs" "$rti_cs_peer" "$rti_type_name" "$case_id" "$fixture" || true
            fi
        done
    fi

    if [[ "$cpp_compile_only" == "1" ]]; then
        echo "RTI C++ peer compiled: $case_id"
        end_workflow_group
        continue
    fi

    # Run the same directed pairings used when seeding the plan. A
    # failed pairing is recorded but does not stop later cases from running.
    mapfile -t case_fixtures < <(fixtures_for_case "$case_id")
    for fixture in "${case_fixtures[@]}"; do
        mapfile -t case_pairings_for_fixture < <(case_pairings "$case_id" "$fixture")
        for pairing in "${case_pairings_for_fixture[@]}"; do
            writer_peer="${pairing%%|*}"
            reader_peer="${pairing#*|}"
            if [[ -n "${rti_only_cases[$case_id]:-}" ]] \
                && [[ "$writer_peer" == "Wireloom C#" || "$reader_peer" == "Wireloom C#" ]]; then
                continue
            fi

            if ! run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" \
                "$fixture" "$writer_peer" "$reader_peer"; then
                overall_status=1
            fi
        done
    done
    end_workflow_group
done

actual_scenario_count=$(wc -l < "$scenarios_tsv")
if [[ "$actual_scenario_count" -ne "$expected_scenario_count" ]]; then
    echo "Expected $expected_scenario_count scenarios but recorded $actual_scenario_count." >&2
    overall_status=1
fi

if [[ "$overall_status" -ne 0 ]]; then
    echo "Matrix completed with scenario or peer-build issues; see the generated report." >&2
else
    echo "Matrix completed without scenario or peer-build issues."
fi
