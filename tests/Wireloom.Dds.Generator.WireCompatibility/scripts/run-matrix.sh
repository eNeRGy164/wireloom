#!/usr/bin/env bash
# Drives the wire-compatibility matrix from inside the pinned RTI toolchain.
# Wireloom-generated C# is compared with checked-in RTI C# and C++ reference
# peers. Each eligible fixture is exercised across every directed peer pairing.
# Oracle generation happens during local corpus maintenance; this runner only
# compiles the checked-in peers and launches isolated producer/consumer processes.
set -euo pipefail

readonly repo_root="${GITHUB_WORKSPACE:-$(git rev-parse --show-toplevel)}"
readonly project="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/Wireloom.Dds.Generator.WireCompatibility.csproj"
readonly native_template="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/Native"
readonly rti_csharp_project="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/RtiOracleProbe/RtiOracleProbe.csproj"
readonly matrix_tools="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/scripts/matrix-tools.cs"
readonly exchange_runner="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/scripts/wire-exchange-runner.cs"
readonly case_build_runner="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/scripts/wire-case-build-runner.cs"
readonly manifest="$repo_root/docs/corpus/manifest.json"
readonly rti_version="${WIRE_COMPATIBILITY_RTI_VERSION:-7.7.0}"
readonly wire_oracle_manifest="$repo_root/docs/corpus/$rti_version/rtiddsgen/wire-oracles.json"
readonly exclusions="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/wire-case-exclusions-$rti_version.json"
readonly timeout_seconds=20
readonly cpp_compile_only="${WIRE_COMPATIBILITY_CPP_COMPILE_ONLY:-0}"
readonly output_dir="${WIRE_COMPATIBILITY_OUTPUT_DIR:-$repo_root/artifacts/wire-compatibility}"
readonly scratch="${RUNNER_TEMP:-/tmp}/wireloom-wire-compatibility"
readonly scenarios_tsv="$scratch/scenarios.tsv"
readonly case_builds_tsv="$scratch/case-builds.tsv"
readonly case_build_queue_tsv="$scratch/case-build-queue.tsv"
readonly case_build_results_tsv="$scratch/case-build-results.tsv"
readonly managed_build_queue_tsv="$scratch/managed-build-queue.tsv"
readonly managed_build_results_tsv="$scratch/managed-build-results.tsv"
readonly cpp_compilation_tsv="$scratch/cpp-compilation.tsv"
readonly nuget_packages="${NUGET_PACKAGES:-$scratch/nuget-packages}"
workflow_group_open=0
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
if [[ -z "${NDDSHOME:-}" || ! -d "$NDDSHOME/include" || ! -d "$NDDSHOME/resource/cmake" ]]; then
    echo "NDDSHOME must point to the selected RTI development libraries and CMake modules." >&2
    exit 2
fi

rm -rf "$scratch"
mkdir -p "$scratch" "$output_dir/logs"
rm -f "$output_dir/logs"/*.log
SECONDS=0
: > "$scenarios_tsv"
: > "$case_builds_tsv"
: > "$case_build_queue_tsv"
: > "$case_build_results_tsv"
: > "$managed_build_queue_tsv"
: > "$managed_build_results_tsv"
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
        matrix_tool case-profile-pairings "$manifest" "$case_id" "$rti_version" "$fixture"
    else
        matrix_tool case-profile-pairings "$manifest" "$case_id" "$rti_version"
    fi
}

wire_oracle_source() {
    matrix_tool case-wire-oracle-source "$wire_oracle_manifest" "$1" "$2"
}

stage_rti_cpp_oracle() {
    local case_id="$1"
    local source_dir="$2"
    local source_file relative_file file_name staged_name staged_path
    while IFS= read -r -d '' source_file; do
        relative_file="${source_file#"$source_dir"/}"
        file_name="$(basename "$relative_file")"
        if [[ "$source_dir" != */wire-wrapped/* && "$(dirname "$relative_file")" == "." ]]; then
            case "$file_name" in
                "$case_id.cxx") staged_name=idl.cxx ;;
                "$case_id.hpp") staged_name=idl.hpp ;;
                "$case_id"Plugin.cxx) staged_name=idlPlugin.cxx ;;
                "$case_id"Plugin.hpp) staged_name=idlPlugin.hpp ;;
                *) staged_name="$file_name" ;;
            esac
            staged_path="$staged_name"
        elif [[ "$relative_file" == idl.cxx || "$relative_file" == idl.hpp \
            || "$relative_file" == idlPlugin.cxx || "$relative_file" == idlPlugin.hpp ]]; then
            case "$file_name" in
                "$case_id.cxx") staged_name=idl.cxx ;;
                "$case_id.hpp") staged_name=idl.hpp ;;
                "$case_id"Plugin.cxx) staged_name=idlPlugin.cxx ;;
                "$case_id"Plugin.hpp) staged_name=idlPlugin.hpp ;;
                *) staged_name="$file_name" ;;
            esac
            staged_path="$staged_name"
        elif [[ "$(dirname "$relative_file")" == "." ]]; then
            source_stem="${file_name%.*}"
            source_idl_stem="${source_stem%Plugin}"
            if [[ -f "$repo_root/docs/corpus/idl/features/$source_idl_stem.idl" ]]; then
                staged_path="features/$file_name"
            elif [[ -f "$repo_root/docs/corpus/idl/includes/$source_idl_stem.idl" ]]; then
                staged_path="includes/$file_name"
            else
                staged_path="$file_name"
            fi
        else
            staged_path="$relative_file"
        fi
        mkdir -p "$native_dir/$(dirname "$staged_path")"
        cp "$source_file" "$native_dir/$staged_path"
    done < <(find "$source_dir" -type f \( -name '*.cxx' -o -name '*.hpp' \) -print0)

    if [[ "$source_dir" != */wire-wrapped/* && "$case_id" != "05-array-of-sequences-idl" ]]; then
        find "$native_dir" -maxdepth 1 -type f \( -name '*.cxx' -o -name '*.hpp' \) -print0 \
            | xargs -0 -r sed -i \
                -e "s/\"${case_id}Plugin\.hpp\"/\"idlPlugin.hpp\"/g" \
                -e "s/\"${case_id}\.hpp\"/\"idl.hpp\"/g"
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
if [[ "$cpp_compile_only" != "1" ]] && ! dotnet build "$exchange_runner" --output "$scratch" --verbosity quiet; then
    echo "Failed to build the C# wire exchange runner." >&2
    exit 2
fi
if ! dotnet build "$case_build_runner" --output "$scratch" --verbosity quiet; then
    echo "Failed to build the C# native peer build runner." >&2
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
    matrix_tool create-plan "$manifest" "$exclusions" "$rti_version" \
        "${WIRE_COMPATIBILITY_CASE_FILTER:--}" \
        "${WIRE_COMPATIBILITY_SHARD_INDEX:-0}" "${WIRE_COMPATIBILITY_SHARD_COUNT:-1}" "$output_dir"
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

# Restore: the package graph is constant across cases; restore it once before the
# case-specific builds so the full matrix does not repeat the same restore.
dotnet restore "$project" \
    -p:WireCompatibilityRtiVersion="$rti_version" \
    -p:RtiConnextDdsVersion="$rti_version" \
    -p:RestorePackagesPath="$nuget_packages" \
    --locked-mode \
    --verbosity quiet
if [[ "$cpp_compile_only" != "1" ]]; then
    dotnet restore "$rti_csharp_project" \
        -p:WireCompatibilityRtiVersion="$rti_version" \
        -p:RtiConnextDdsVersion="$rti_version" \
        -p:RestorePackagesPath="$nuget_packages" \
        --locked-mode \
        --verbosity quiet
fi

# Unexpected setup errors must fail the job even after peer execution starts.
# Handled peer failures are recorded as FAIL rows and allow the matrix to continue.
overall_status=0
case_index=0
declare -A managed_build_status=()
declare -A managed_build_detail=()
bootstrap_case=""
for candidate_case in "${case_ids[@]}"; do
    if [[ -z "${rti_only_cases[$candidate_case]:-}" ]]; then
        bootstrap_case="$candidate_case"
        break
    fi
done

if [[ -n "$bootstrap_case" ]]; then
    case_dir="$scratch/$bootstrap_case"
    wireloom_output_dir="$case_dir/wireloom-bin"
    generated_root="$case_dir/wireloom-generated"
    mkdir -p "$wireloom_output_dir"
    case_idl="$(matrix_tool case-field "$manifest" "$bootstrap_case" idl)"
    sequence_array_mapping="$(matrix_tool case-field "$manifest" "$bootstrap_case" sequence-array-mapping)"
    bootstrap_args=(
        --no-restore --configuration Release -t:Rebuild
        -p:WireCompatibilityCase="$bootstrap_case"
        -p:WireCompatibilityRtiVersion="$rti_version"
        -p:RtiConnextDdsVersion="$rti_version"
        -p:RestorePackagesPath="$nuget_packages"
        "-p:OutputPath=$wireloom_output_dir/"
        "-p:WireCompatibilityIdl=$repo_root/docs/corpus/$case_idl"
        "-p:DdsIdlSequenceArrayMapping=$sequence_array_mapping"
        "-p:WireCompatibilityGeneratedOutputPath=$generated_root"
        --verbosity quiet
    )
    if [[ "$bootstrap_case" == "05-array-of-sequences" ]]; then
        bootstrap_args+=( -p:WarningsNotAsErrors=DDSG0105 )
    fi
    bootstrap_log="$case_dir/wireloom-csharp-build.log"
    mkdir -p "$case_dir"
    if dotnet build "$project" "${bootstrap_args[@]}" > "$bootstrap_log" 2>&1; then
        managed_build_status["$bootstrap_case"]="compiled"
        managed_build_detail["$bootstrap_case"]="$wireloom_output_dir"
    else
        managed_build_status["$bootstrap_case"]="build-failed"
        managed_build_detail["$bootstrap_case"]="$bootstrap_log"
        overall_status=1
    fi

    for case_id in "${case_ids[@]}"; do
        if [[ -n "${rti_only_cases[$case_id]:-}" || "$case_id" == "$bootstrap_case" ]]; then
            continue
        fi
        case_dir="$scratch/$case_id"
        case_idl="$(matrix_tool case-field "$manifest" "$case_id" idl)"
        sequence_array_mapping="$(matrix_tool case-field "$manifest" "$case_id" sequence-array-mapping)"
        generated_root="$case_dir/wireloom-generated"
        wireloom_output_dir="$case_dir/wireloom-bin"
        intermediate_output_dir="$case_dir/wireloom-obj"
        mkdir -p "$wireloom_output_dir" "$intermediate_output_dir"
        warning_override="-"
        if [[ "$case_id" == "05-array-of-sequences" ]]; then
            warning_override="DDSG0105"
        fi
        printf '%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\n' \
            "$case_id" "$repo_root/docs/corpus/$case_idl" "${sequence_array_mapping:--}" \
            "$wireloom_output_dir" "$generated_root" "$intermediate_output_dir" \
            "$case_dir/wireloom-csharp-build.log" "$warning_override" \
            >> "$managed_build_queue_tsv"
    done
fi

if [[ -s "$managed_build_queue_tsv" ]]; then
    managed_runner_status=0
    dotnet "$scratch/wire-case-build-runner.dll" managed \
        --builds "$managed_build_queue_tsv" \
        --results "$managed_build_results_tsv" \
        --project "$project" \
        --rti-version "$rti_version" \
        --restore-packages "$nuget_packages" \
        --project-extensions "$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/obj" \
        --max-concurrency "${WIRE_COMPATIBILITY_CSHARP_BUILD_CONCURRENCY:-4}" || managed_runner_status=$?
    if [[ "$managed_runner_status" -gt 1 ]]; then
        echo "The C# managed peer build runner failed with exit code $managed_runner_status." >&2
        exit "$managed_runner_status"
    fi
    while IFS=$'\t' read -r case_id build_status build_detail build_elapsed; do
        managed_build_status["$case_id"]="$build_status"
        managed_build_detail["$case_id"]="$build_detail"
        if [[ "$build_status" != "compiled" ]]; then
            overall_status=1
        fi
    done < "$managed_build_results_tsv"
    if [[ "$managed_runner_status" -ne 0 ]]; then
        overall_status=1
    fi
fi
echo "WIRE_MATRIX_PHASE managed-peer-build queued=$(wc -l < "$managed_build_queue_tsv") concurrency=${WIRE_COMPATIBILITY_CSHARP_BUILD_CONCURRENCY:-4} elapsed=${SECONDS}s"

for case_id in "${case_ids[@]}"; do
    case_index=$((case_index + 1))
    start_workflow_group "Case $case_index/${#case_ids[@]}: $case_id"
    build_phase='peer-prepare'

    printf 'WIRE_MATRIX_CASE %d/%d case=%s phase=%s completed-scenarios=%d/%d elapsed=%ss\n' \
        "$case_index" "${#case_ids[@]}" "$case_id" \
        "$build_phase" \
        "$completed_scenario_count" "$expected_scenario_count" "$SECONDS"
    case_dir="$scratch/$case_id"
    native_dir="$case_dir/native"
    wireloom_output_dir="$case_dir/wireloom-bin"
    cs_peer="$wireloom_output_dir/WireCompatibility.${case_id}.dll"
    mkdir -p "$native_dir" "$wireloom_output_dir"
    wireloom_cs_available=1
    wireloom_cs_failure_reason="Wireloom C# build failed for $case_id"
    mapfile -t case_pairings_for_case < <(case_pairings "$case_id")
    rti_csharp_required=0
    if case_uses_rti_csharp "$case_id"; then
        rti_csharp_required=1
    fi
    rti_cs_available=0
    rti_type_name="-"
    rti_cs_failure_reason="RTI C# peer is not required for $case_id"
    rti_cs_peer="$case_dir/rti-bin/WireCompatibility.RtiOracleProbe.dll"

    # Build Wireloom's C# output against the selected RTI runtime. The resulting
    # generated type name is discovered from Wireloom's emitted TypeSupport.
    if [[ -n "${rti_only_cases[$case_id]:-}" ]]; then
        type_name="-"
        cpp_type="CorpusOptionalAggregate::Holder"
        idl_relative="idl/features/09-optional-aggregate-member.idl"
        case_defines=()
    else
        generated_root="$case_dir/wireloom-generated"
        wireloom_build_log="$case_dir/wireloom-csharp-build.log"
        if [[ "${managed_build_status[$case_id]:-build-failed}" != "compiled" ]]; then
            wireloom_cs_available=0
            build_detail="${managed_build_detail[$case_id]:-$wireloom_build_log}"
            compiler_error="$(sed -n 's/.*: error \(CS[0-9]*: .*\) \[.*$/\1/p' "$wireloom_build_log" | head -n 1 || true)"
            if [[ -n "$compiler_error" ]]; then
                wireloom_cs_failure_reason+="; $compiler_error"
            fi
            echo "$wireloom_cs_failure_reason; see $build_detail; pairings requiring this peer will be marked failed." >&2
            grep -Ei 'error|warning|failed|exception' "$build_detail" | head -n 30 >&2 || true
        else
            if [[ "$cpp_compile_only" == "1" ]]; then
                tail -n 8 "$wireloom_build_log"
            fi
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
        rti_csharp_oracle_source="$(wire_oracle_source "$case_id" rtiCSharp)"
        if [[ -n "$rti_csharp_oracle_source" ]]; then
            rti_csharp_oracle_dir="$repo_root/docs/corpus/$rti_version/rtiddsgen/$rti_csharp_oracle_source"
            cp -R "$rti_csharp_oracle_dir/." "$rti_csharp_dir/"
            printf 'Using checked-in RTI C# oracle (%s; selection and adjustments are recorded in %s).\n' \
                "$rti_csharp_oracle_source" "${wire_oracle_manifest#"$repo_root"/}" \
                > "$output_dir/logs/$case_id-rti-csharp-oracle.log"
            rti_csharp_generated=1
        else
            rti_csharp_generated=0
            rti_cs_failure_reason="No checked-in RTI C# wire oracle is available for $case_id"
            echo "$rti_cs_failure_reason; refusing to generate a peer during wire execution." >&2
            fi
        if [[ "$rti_csharp_generated" == 1 && -z "$rti_csharp_oracle_source" \
            && "$case_id" == 07-union-char-numeric* ]]; then
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
                -p:RestorePackagesPath="$nuget_packages" \
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
    rti_cpp_oracle_source="$(wire_oracle_source "$case_id" rtiCpp)"
    if [[ -n "$rti_cpp_oracle_source" ]]; then
        rti_cpp_oracle_dir="$repo_root/docs/corpus/$rti_version/rtiddsgen/$rti_cpp_oracle_source"
        stage_rti_cpp_oracle "$case_id" "$rti_cpp_oracle_dir"
        printf 'Using checked-in RTI C++ oracle (%s; selection and adjustments are recorded in %s).\n' \
            "$rti_cpp_oracle_source" "${wire_oracle_manifest#"$repo_root"/}" \
            > "$output_dir/logs/$case_id-rti-cpp-oracle.log"
    elif [[ "$case_id" != "07-union-wchar-label" ]]; then
        printf "%s\\tmissing-captured-oracle\\t%s\\n" "$case_id" "$wire_oracle_manifest" >> "$cpp_compilation_tsv"
        overall_status=1
        echo "No checked-in RTI C++ wire oracle is available for $case_id; refusing to generate a peer during wire execution." >&2
        end_workflow_group
        continue
    fi
    if [[ "$case_id" == "10-resolve-name-wstring-aliases" && -z "$rti_cpp_oracle_source" ]]; then
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

    # Checked-in C++ oracle directories include all source dependencies.
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
        cmake_adaptation_args+=( -DWIRELOOM_TYPED_OPTIONAL_RECURSIVE_ARRAY_FIXTURE=ON )
    fi
    if [[ "$case_id" == "06-aggregates" ]]; then
        cp "$native_template/recursive-sequence-peer.cpp" "$native_dir/recursive-sequence-peer.cpp"
    fi
    if [[ "$case_id" == "09-optional-recursive-array" && -z "$rti_cpp_oracle_source" ]]; then
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
    fi
    if [[ "$case_id" == "06-aggregates" && "$rti_version" == "7.3.1" && -z "$rti_cpp_oracle_source" ]]; then
        # Code Generator 4.3.1 has the same missing qualification for the
        # recursive aggregate TypeCode reference.
        sed -i 's/dynamic_type< Recursive>/dynamic_type< ::CorpusAggregates::Recursive>/g' \
            "$native_dir/idl.cxx"
    fi
    if [[ "$case_id" == "07-union-char-numeric-255" && -z "$rti_cpp_oracle_source" ]]; then
        # RTI's native char label lookup requires the signed byte representation.
        matrix_tool adapt-rti-characters cpp "$case_id" "$native_dir"
    fi
    sed "s|@IDL_BASE@|idl|g; s|@CPP_TYPE@|$cpp_template_type|g" \
        "$native_template/main.cpp.in" > "$native_dir/main.cpp"
    cp "$native_template/typed-fixture-peer.hpp.in" "$native_dir/typed-fixture-peer.hpp"
    cp "$native_template/peer-qos.hpp" "$native_dir/peer-qos.hpp"

    [[ -n "$type_name" ]] || type_name="-"
    [[ -n "$rti_type_name" ]] || rti_type_name="-"

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
    printf '%s\t%s\t%s\t%s\n' "$case_id" "$native_dir/build" \
        "$case_dir/cmake-build.log" "$native_dir/build/wire-peer" >> "$case_build_queue_tsv"
    printf '%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\n' \
        "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$case_dir" "$cs_peer" "$rti_cs_peer" \
        "$wireloom_cs_available" "$wireloom_cs_failure_reason" "$rti_cs_available" "$rti_cs_failure_reason" \
        > "$case_dir/build-metadata.tsv"

    if [[ "$case_id" == 14-* ]]; then
        mapfile -t case_fixtures < <(matrix_tool case-fixtures "$case_id" "$rti_version")
        for fixture in "${case_fixtures[@]}"; do
            verify_managed_fixture "wireloom-cs" "$cs_peer" "$type_name" "$case_id" "$fixture" || true
            if [[ "${rti_cs_available:-0}" == "1" ]]; then
                verify_managed_fixture "rti-cs" "$rti_cs_peer" "$rti_type_name" "$case_id" "$fixture" || true
            fi
        done
    fi

    end_workflow_group
done

if [[ -s "$case_build_queue_tsv" ]]; then
    start_workflow_group "Build native peers concurrently"
    echo "WIRE_MATRIX_PHASE native-peer-build queued=$(wc -l < "$case_build_queue_tsv") concurrency=${WIRE_COMPATIBILITY_CASE_BUILD_CONCURRENCY:-2} elapsed=${SECONDS}s"
    build_runner_status=0
    dotnet "$scratch/wire-case-build-runner.dll" \
        --builds "$case_build_queue_tsv" \
        --results "$case_build_results_tsv" \
        --max-concurrency "${WIRE_COMPATIBILITY_CASE_BUILD_CONCURRENCY:-2}" \
        --jobs-per-build 2 || build_runner_status=$?
    if [[ "$build_runner_status" -gt 1 ]]; then
        echo "The C# native peer build runner failed with exit code $build_runner_status." >&2
        exit "$build_runner_status"
    fi
    while IFS=$'\t' read -r case_id build_status build_detail build_elapsed; do
        case_dir="$scratch/$case_id"
        native_dir="$case_dir/native"
        if [[ "$build_status" == "compiled" ]]; then
            IFS=$'\t' read -r _ type_name rti_type_name cpp_type case_dir cs_peer rti_cs_peer \
                wireloom_cs_available wireloom_cs_failure_reason rti_cs_available rti_cs_failure_reason \
                < "$case_dir/build-metadata.tsv"
            cpp_peer="$native_dir/build/wire-peer"
            printf '%s\tcompiled\t%s\n' "$case_id" "$cpp_peer" >> "$cpp_compilation_tsv"
            printf '%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\n' \
                "$case_id" "$type_name" "$rti_type_name" "$cpp_type" "$case_dir" "$cs_peer" "$rti_cs_peer" "$cpp_peer" \
                "$wireloom_cs_available" "$wireloom_cs_failure_reason" "$rti_cs_available" "$rti_cs_failure_reason" \
                >> "$case_builds_tsv"
            if [[ "$cpp_compile_only" == "1" ]]; then
                echo "RTI C++ peer compiled: $case_id (${build_elapsed}s)"
            fi
        else
            overall_status=1
            printf '%s\tcompile-failed\tsee %s\n' "$case_id" "$build_detail" >> "$cpp_compilation_tsv"
            echo "RTI C++ peer failed to compile for $case_id; see $build_detail." >&2
            grep -Ei 'error|warning|failed|exception' "$build_detail" | head -n 30 >&2 || true
        fi
    done < "$case_build_results_tsv"
    if [[ "$build_runner_status" -ne 0 ]]; then
        overall_status=1
    fi
    end_workflow_group
fi
echo "WIRE_MATRIX_PHASE peer-preparation elapsed=${SECONDS}s wireloom-cases=${#case_ids[@]} native-peers=$(awk -F '\t' '$2 == "compiled" { count++ } END { print count + 0 }' "$cpp_compilation_tsv")"

if [[ "$cpp_compile_only" != "1" && -s "$case_builds_tsv" ]]; then
    echo "WIRE_MATRIX_PHASE exchange-run built-cases=$(wc -l < "$case_builds_tsv") planned-exchanges=$expected_scenario_count elapsed=${SECONDS}s"
    runner_status=0
    dotnet "$scratch/wire-exchange-runner.dll" \
        --scenarios "$scenarios_tsv" \
        --case-builds "$case_builds_tsv" \
        --rti-version "$rti_version" \
        --output-directory "$output_dir" \
        --scratch "$scratch" \
        --timeout-seconds "$timeout_seconds" \
        --max-concurrency "${WIRE_COMPATIBILITY_MAX_CONCURRENCY:-16}" || runner_status=$?
    if [[ "$runner_status" -eq 1 ]]; then
        overall_status=1
    elif [[ "$runner_status" -ne 0 ]]; then
        echo "The C# exchange runner failed with exit code $runner_status." >&2
        exit "$runner_status"
    fi
fi

passed_scenario_count=$(awk -F '\t' '$9 == "PASS" { count++ } END { print count + 0 }' "$scenarios_tsv")
failed_scenario_count=$(awk -F '\t' '$9 == "FAIL" { count++ } END { print count + 0 }' "$scenarios_tsv")
completed_scenario_count=$((passed_scenario_count + failed_scenario_count))
actual_scenario_count=$(wc -l < "$scenarios_tsv")
if [[ "$actual_scenario_count" -ne "$expected_scenario_count" ]]; then
    echo "Expected $expected_scenario_count scenarios but recorded $actual_scenario_count." >&2
    overall_status=1
fi

if [[ "$overall_status" -ne 0 ]]; then
    echo "Matrix completed with scenario or peer-build issues in ${SECONDS}s; see the generated report." >&2
else
    echo "Matrix completed without scenario or peer-build issues in ${SECONDS}s."
fi
