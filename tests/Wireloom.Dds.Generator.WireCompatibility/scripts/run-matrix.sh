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
readonly exclusions="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/wire-case-exclusions.json"
readonly rti_version="${WIRE_COMPATIBILITY_RTI_VERSION:-7.7.0}"
readonly timeout_seconds=20
readonly output_dir="${WIRE_COMPATIBILITY_OUTPUT_DIR:-$repo_root/artifacts/wire-compatibility}"
readonly scratch="${RUNNER_TEMP:-/tmp}/wireloom-wire-compatibility"
readonly cases_tsv="$scratch/cases.tsv"
readonly scenarios_tsv="$scratch/scenarios.tsv"
workflow_group_open=0
readonly non_default_fixture_cases=(
    01-boundaries 01-full-widths
    02-names-constants 02-scopes 02-constant-expressions 02-formatting 02-multiple
        03-alias-aggregate 03-alias-collections
    04-strings 04-boundaries
    05-collections 05-shapes 06-aggregates
    06-alias-composition
    07-unions 07-union-enum 07-union-multilabel 07-union-short
    07-union-char 07-union-scoped 07-union-boolean 07-union-aliases 07-union-wchar-label
    08-keys 08-key-nested 08-key-inherited 08-key-boundaries 08-key-union
    09-extensibility 09-default 09-id-gaps 09-evolution-optional
    09-optional-collections 09-autoid-hash 09-defaults-ranges
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
    local level="$1" title message
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
        03-enums-aliases)
            printf '%s\n' default enum-alternate
            ;;
        02-scopes|02-constant-expressions|05-collections)
            printf '%s\n' sequence-empty sequence-single non-default
            ;;
        04-strings)
            printf '%s\n' string-empty string-boundary non-default
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
        07-union-scoped)
            printf '%s\n' union-payload sequence-empty sequence-single
            ;;
        07-union-aliases)
            printf '%s\n' union-zero union-ten
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

if [[ "$rti_version" != "7.7.0" ]]; then
    echo "This initial runner supports exact RTI 7.7.0 only." >&2
    exit 2
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
printf '{"rtiVersion":"%s","scenarios":[]}' "$rti_version" > "$output_dir/results.json"

# Always write a structured report, including when a build or scenario fails.
# Endpoint output is separately reduced to status lines and selected RTI
# diagnostics so the report artifacts do not include arbitrary process output.
write_report() {
    matrix_tool render-report \
        "$rti_version" "$expected_scenario_count" "$scenarios_tsv" \
        "$output_dir/results.json" "$output_dir/results.md" "$output_dir/logs" "$exclusions"
}
matrix_tool() {
    if [[ -f "$scratch/matrix-tools.dll" ]]; then
        dotnet "$scratch/matrix-tools.dll" "$@"
    else
        dotnet run --file "$matrix_tools" -- "$@"
    fi | tr -d '\r'
}

finish_run() {
    local run_status=$?
    trap - EXIT
    end_workflow_group
    start_workflow_group "Generate wire compatibility report"
    if write_report; then
        end_workflow_group
        if [[ "$run_status" -ne 0 ]]; then
            echo "Matrix execution ended with status $run_status; the report was generated successfully, so the workflow is successful. See the report for failed or unrun scenarios." >&2
            workflow_annotation warning "Wire compatibility matrix completed with setup issues" \
                "Matrix execution ended with status $run_status; see the generated report for failed or unrun scenarios."
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
            echo "Unknown or non-positive corpus case: $requested_case" >&2
            exit 2
        fi
    done
    case_ids=("${requested_cases[@]}")
fi
for case_id in "${case_ids[@]}"; do
    mapfile -t case_fixtures < <(fixtures_for_case "$case_id")
    if [[ -n "${rti_only_cases[$case_id]:-}" ]]; then
        expected_scenario_count=$((expected_scenario_count + ${#case_fixtures[@]} * 4))
    else
        expected_scenario_count=$((expected_scenario_count + ${#case_fixtures[@]} * 9))
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
        for pairing in \
            'Wireloom C#|Wireloom C#' 'Wireloom C#|RTI C#' 'Wireloom C#|RTI C++' \
            'RTI C#|Wireloom C#' 'RTI C#|RTI C#' 'RTI C#|RTI C++' \
            'RTI C++|Wireloom C#' 'RTI C++|RTI C#' 'RTI C++|RTI C++'; do
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

end_workflow_group

record_scenario() {
    local case_id="$1" wireloom_type="$2" rti_csharp_type="$3" cpp_type="$4" writer="$5" reader="$6" status="$7" fixture="$8"
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

run_tool() {
    local tool_label="$1" log_path="$2"
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
    local peer="$1" role="$2" topic="$3" case_id="$4" wireloom_type="$5" rti_type="$6" fixture="$7"
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

peer_id() {
    case "$1" in
        'Wireloom C#') printf 'wireloom-cs' ;;
        'RTI C#') printf 'rti-cs' ;;
        'RTI C++') printf 'rti-cpp' ;;
        *) return 2 ;;
    esac
}

run_scenario() {
    local case_id="$1" wireloom_type="$2" rti_type="$3" cpp_type="$4" fixture="$5" writer_peer="$6" reader_peer="$7"
    local writer_id reader_id
    writer_id="$(peer_id "$writer_peer")"
    reader_id="$(peer_id "$reader_peer")"
    local topic="wl_${case_id//-/_}_${fixture//-/_}_${writer_id}_${reader_id}_$RANDOM$RANDOM"
    local label="${case_id}-${fixture}-${writer_id}-${reader_id}"
    local reader_log="$scratch/${label}-reader.log" writer_log="$scratch/${label}-writer.log"
    local reader_pid reader_ready=0 reader_status=0 writer_status=0

    scenario_index=$((scenario_index + 1))
    printf 'WIRE_MATRIX_PROGRESS started=%d/%d completed=%d/%d passed=%d failed=%d elapsed=%ss current=%s [%s] %s -> %s\n' \
        "$scenario_index" "$expected_scenario_count" \
        "$completed_scenario_count" "$expected_scenario_count" \
        "$passed_scenario_count" "$failed_scenario_count" "$SECONDS" \
        "$case_id" "$fixture" "$writer_peer" "$reader_peer"

    if [[ "$rti_cs_available" -eq 0 \
        && ( "$writer_peer" == "RTI C#" || "$reader_peer" == "RTI C#" ) ]]; then
        printf 'NOT_RUN_DETAIL RTI-generated C# peer was unavailable for %s\n' "$case_id" \
            > "$output_dir/logs/${label}.log"
        record_scenario "$case_id" "$wireloom_type" "$rti_type" "$cpp_type" \
            "$writer_peer" "$reader_peer" NOT_RUN "$fixture" || return 1
        completed_scenario_count=$((completed_scenario_count + 1))
        progress_percent=$((completed_scenario_count * 100 / expected_scenario_count))
        printf 'WIRE_MATRIX_PROGRESS completed=%d/%d percent=%d%% passed=%d failed=%d elapsed=%ss latest=NOT_RUN %s [%s] %s -> %s\n' \
            "$completed_scenario_count" "$expected_scenario_count" "$progress_percent" \
            "$passed_scenario_count" "$failed_scenario_count" "$SECONDS" \
            "$case_id" "$fixture" "$writer_peer" "$reader_peer"
        return 0
    fi

    # Start the reader first and wait until it has created its DDS entities.
    # This avoids losing the writer's single sample before discovery completes.
    start_peer "$reader_peer" reader "$topic" "$case_id" "$wireloom_type" "$rti_type" "$fixture" > "$reader_log" 2>&1 &
    reader_pid=$!

    for _ in $(seq 1 100); do
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
        : > "$writer_log"
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

# The package graph is constant across cases; restore it once before the
# case-specific builds so the full matrix does not repeat the same restore.
dotnet restore "$project" \
    -p:WireCompatibilityRtiVersion="$rti_version" \
    --locked-mode \
    --verbosity quiet
dotnet restore "$rti_csharp_project" --locked-mode --verbosity quiet

overall_status=0
scenario_index=0
SECONDS=0
case_index=0
for case_id in "${case_ids[@]}"; do
    case_index=$((case_index + 1))
    start_workflow_group "Case $case_index/${#case_ids[@]}: $case_id"
    printf 'WIRE_MATRIX_CASE %d/%d case=%s phase=csharp-build completed-scenarios=%d/%d elapsed=%ss\n' \
        "$case_index" "${#case_ids[@]}" "$case_id" \
        "$completed_scenario_count" "$expected_scenario_count" "$SECONDS"
    case_dir="$scratch/$case_id"
    native_dir="$case_dir/native"
    mkdir -p "$native_dir"

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
        -p:WireCompatibilityCase="$case_id"
        -p:WireCompatibilityRtiVersion="$rti_version"
        --verbosity quiet
    )
    if [[ "$case_id" == "05-array-of-sequences" ]]; then
        # Preserve DDSG0105 as a warning in this known-limitation case while
        # keeping the repository's warnings-as-errors policy for all others.
        build_args+=( -p:WarningsNotAsErrors=DDSG0105 )
    fi
    dotnet build "$project" "${build_args[@]}"

    generated_root="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/obj/wireloom-generated"
    registry="$case_dir/case.json"
    matrix_tool discover-cases \
        "$manifest" "$generated_root" "$registry" "$case_id" >/dev/null

    mapfile -t discovered < <(matrix_tool case-info "$registry" "$manifest" "$case_id")
    type_name="${discovered[0]}"
    cpp_type="${discovered[1]}"
    idl_relative="${discovered[2]}"
    case_defines=("${discovered[@]:3}")
    fi
    cs_peer="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/bin/Release/net10.0/WireCompatibility.${case_id}.dll"

    define_args=()
    for define in "${case_defines[@]}"; do
        [[ -z "$define" ]] && continue
        define_args+=( -D "$define" )
    done
    idl_path="$repo_root/docs/corpus/$idl_relative"
    if [[ "$case_id" == "02-multiple" || "$case_id" == "03-enum-values-prefix" || "$case_id" == "03-enum-values-explicit" ]]; then
        idl_path="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/IdlWrappers/$case_id.idl"
    fi
    rti_csharp_dir="$case_dir/rti-csharp-generated"
    rti_cs_peer="$case_dir/rti-bin/WireCompatibility.RtiOracleProbe.dll"
    rti_build_log="$case_dir/rti-csharp-build.log"
    oracle_registry="$case_dir/rti-oracle.json"
    rti_csharp_generated=0
    mkdir -p "$rti_csharp_dir" "$case_dir/rti-bin" "$case_dir/rti-obj"
    rti_csharp_codegen_args=(
        -language "C#" -replace -generateIncludeFiles -d "$rti_csharp_dir"
        "${define_args[@]}"
        -I "$repo_root/docs/corpus/idl"
        -I "$repo_root/docs/corpus/idl/includes"
        "$idl_path"
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
        if [[ "$rti_csharp_generated" == 1 && "$case_id" == "06-alias-composition" ]]; then
            # RTI 7.7.0 shadows this generated member with a conversion
            # parameter, which otherwise produces invalid C# calls.
            sed -i -E \
                's/^([[:space:]]*)sample\.(FromNative|ToNative)\(sample\.sample/\1this.sample.\2(sample.sample/' \
                "$rti_csharp_dir/${case_id}Plugin.cs"
        fi
    fi
    if [[ "$rti_csharp_generated" == 1 ]] && matrix_tool discover-rti \
            "$manifest" "$case_id" "$type_name" "$rti_csharp_dir" "$oracle_registry"; then
        rti_type_name="$(matrix_tool json-field "$oracle_registry" rtiCSharpType)"
        if dotnet build "$rti_csharp_project" --no-restore --configuration Release --verbosity quiet \
            -p:RtiOracleSourceDir="$rti_csharp_dir" \
            -p:BaseIntermediateOutputPath="$case_dir/rti-obj/" \
            -p:MSBuildProjectExtensionsPath="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/RtiOracleProbe/obj/" \
            -p:OutputPath="$case_dir/rti-bin/" > "$rti_build_log" 2>&1; then
            rti_cs_available=1
            echo "RTI-generated C# peer compiled: $rti_type_name"
        else
            rti_cs_available=0
            overall_status=1
            echo "RTI-generated C# peer failed to compile for $case_id; its matrix pairings will be marked not run." >&2
            grep -Ei 'error|warning|failed|exception' "$rti_build_log" | head -n 30 >&2 || true
        fi
    else
        rti_cs_available=0
        overall_status=1
        echo "RTI C# generation failed for $case_id; its matrix pairings will be marked not run." >&2
    fi
    # Normally both peers use the same corpus IDL. These two adaptations keep
    # RTI C++ generation possible while preserving the intended wire shape:
    # rtiddsgen cannot parse wchar union labels, and it rejects the C#-only
    # FlatData mapping annotation.
    if [[ "$case_id" == "07-union-wchar-label" ]]; then
        cp "$idl_path" "$native_dir/idl.idl"
    elif [[ "$case_id" == "10-flat-data-binding" ]]; then
        # RTI ignores this mapping on the C# side. Generate a standard C++ peer
        # while preserving the original type shape and XCDR2 requirement.
        sed '/@language_binding(FLAT_DATA)/d' "$idl_path" > "$native_dir/idl.idl"
    else
        cp "$idl_path" "$native_dir/idl.idl"
    fi
    rtiddsgen_args=(
        -language C++11 -replace -generateIncludeFiles -d "$native_dir"
        "${define_args[@]}"
        -I "$repo_root/docs/corpus/idl"
        -I "$repo_root/docs/corpus/idl/includes"
        "$native_dir/idl.idl"
    )
    if [[ "$case_id" != "07-union-wchar-label" ]]; then
        run_tool "rtiddsgen for $case_id" "$case_dir/rtiddsgen.log" \
            "$NDDSHOME/bin/rtiddsgen" "${rtiddsgen_args[@]}"
    fi

    declare -A generated_include_sources=()
    while IFS=$'\t' read -r include_path include_source; do
        [[ -z "$include_path" ]] && continue
        include_base="$(basename "${include_path%.*}")"
        if [[ -z "${generated_include_sources[$include_source]:-}" ]]; then
            dependency_args=(
                -language C++11 -replace -d "$native_dir"
                "${define_args[@]}"
                -I "$repo_root/docs/corpus/idl"
                -I "$repo_root/docs/corpus/idl/includes"
                "$include_source"
            )
            run_tool "rtiddsgen include for $case_id" "$case_dir/rtiddsgen-include.log" \
                "$NDDSHOME/bin/rtiddsgen" "${dependency_args[@]}"
            generated_include_sources[$include_source]=1
        fi

        include_directory="$(dirname "$include_path")"
        if [[ "$include_directory" != "." ]]; then
            mkdir -p "$native_dir/$include_directory"
            cp "$native_dir/$include_base.hpp" "$native_dir/$include_directory/$include_base.hpp"
            cp "$native_dir/${include_base}Plugin.hpp" \
                "$native_dir/$include_directory/${include_base}Plugin.hpp"
        fi
    done < <(matrix_tool includes "$native_dir/idl.idl" "$repo_root/docs/corpus/idl" "$repo_root/docs/corpus/idl/includes")
    cpp_template_type="$cpp_type"
    cmake_adaptation_args=()
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
    if [[ "$case_id" == "04-boundaries" ]]; then
        cmake_adaptation_args+=( -DWIRELOOM_TYPED_STRING_BOUNDARIES_FIXTURE=ON )
    fi
    if [[ "$case_id" == "09-optional-aggregate-member" ]]; then
        cmake_adaptation_args+=( -DWIRELOOM_TYPED_OPTIONAL_AGGREGATE_FIXTURE=ON )
    fi
    sed "s|@IDL_BASE@|idl|g; s|@CPP_TYPE@|$cpp_template_type|g" \
        "$native_template/main.cpp.in" > "$native_dir/main.cpp"
    cp "$native_template/typed-fixture-peer.hpp.in" "$native_dir/typed-fixture-peer.hpp"

    cp "$native_template/CMakeLists.txt" "$native_dir/CMakeLists.txt"
    run_tool "CMake configure for $case_id" "$case_dir/cmake-configure.log" \
        cmake -S "$native_dir" -B "$native_dir/build" \
        -DCMAKE_MODULE_PATH="$NDDSHOME/resource/cmake" \
        -DCONNEXTDDS_ARCH=x64Linux4gcc8.5.0 "${cmake_adaptation_args[@]}"
    run_tool "C++ build for $case_id" "$case_dir/cmake-build.log" \
        cmake --build "$native_dir/build" --parallel 2
    cpp_peer="$native_dir/build/wire-peer"
    printf '%s\t%s\t%s\n' "$case_id" "$type_name" "$cpp_type" >> "$cases_tsv"

    # Run both same-language controls before the cross-language checks. A
    # failed pairing is recorded but does not stop later cases from running.
    mapfile -t case_fixtures < <(fixtures_for_case "$case_id")
    for fixture in "${case_fixtures[@]}"; do
        if [[ -n "${rti_only_cases[$case_id]:-}" ]]; then
            run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'RTI C#' 'RTI C#' || overall_status=1
            run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'RTI C#' 'RTI C++' || overall_status=1
            run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'RTI C++' 'RTI C#' || overall_status=1
            run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'RTI C++' 'RTI C++' || overall_status=1
            continue
        fi
        run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'Wireloom C#' 'Wireloom C#' || overall_status=1
        run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'Wireloom C#' 'RTI C#' || overall_status=1
        run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'Wireloom C#' 'RTI C++' || overall_status=1
        run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'RTI C#' 'Wireloom C#' || overall_status=1
        run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'RTI C#' 'RTI C#' || overall_status=1
        run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'RTI C#' 'RTI C++' || overall_status=1
        run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'RTI C++' 'Wireloom C#' || overall_status=1
        run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'RTI C++' 'RTI C#' || overall_status=1
        run_scenario "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$fixture" 'RTI C++' 'RTI C++' || overall_status=1
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
