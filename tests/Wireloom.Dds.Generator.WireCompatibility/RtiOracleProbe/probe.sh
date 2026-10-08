#!/usr/bin/env bash
# Focused reproduction helper for the RTI C# binding's arrays-of-sequences
# behavior. This is a diagnostic probe, not part of the corpus-wide matrix:
# it compares the RTI-generated C# type with an RTI-generated C++ peer so that
# observed binding limitations can be separated from Wireloom's output.
set -euo pipefail

readonly repo_root="${GITHUB_WORKSPACE:?GITHUB_WORKSPACE must point to the mounted repository}"
readonly probe_dir="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/RtiOracleProbe"
readonly native_template="$repo_root/tests/Wireloom.Dds.Generator.WireCompatibility/Native"
readonly scratch="${RUNNER_TEMP:-/tmp}/wireloom-rti-oracle-probe"
readonly cs_project="$probe_dir/RtiOracleProbe.csproj"
readonly cs_peer="$probe_dir/bin/Release/net10.0/WireCompatibility.RtiOracleProbe.dll"
readonly case_id=05-array-of-sequences
readonly cpp_type='::CorpusArrayOfSequences::Sample'
readonly timeout_seconds=20

if [[ -z "${RTI_LICENSE_FILE:-}" || ! -r "$RTI_LICENSE_FILE" ]]; then
    echo "RTI_LICENSE_FILE must name a readable RTI license file." >&2
    exit 2
fi

rm -rf "$scratch"
mkdir -p "$scratch/native"

dotnet restore "$cs_project" --locked-mode --verbosity quiet
dotnet build "$cs_project" --no-restore --configuration Release --verbosity quiet

"$NDDSHOME/bin/rtiddsgen" \
    -language C++11 -replace -generateIncludeFiles \
    -d "$scratch/native" \
    -I "$repo_root/docs/corpus/idl" \
    -I "$repo_root/docs/corpus/idl/includes" \
    "$repo_root/docs/corpus/idl/features/05-array-of-sequences.idl"

sed "s|@IDL_BASE@|05-array-of-sequences|g; s|@CPP_TYPE@|$cpp_type|g" \
    "$native_template/main.cpp.in" > "$scratch/native/main.cpp"
cp "$native_template/typed-fixture-peer.hpp.in" "$scratch/native/typed-fixture-peer.hpp"
cp "$native_template/CMakeLists.txt" "$scratch/native/CMakeLists.txt"
cmake -S "$scratch/native" -B "$scratch/native/build" \
    -DCMAKE_MODULE_PATH="$NDDSHOME/resource/cmake" \
    -DCONNEXTDDS_ARCH=x64Linux4gcc8.5.0
cmake --build "$scratch/native/build" --parallel 2
readonly cpp_peer="$scratch/native/build/wire-peer"

run_pair() {
    local writer="$1" reader="$2" label="$3"
    local topic="rti_array_sequences_${label}_$RANDOM$RANDOM"
    local reader_log="$scratch/${label}-reader.log"
    local writer_log="$scratch/${label}-writer.log"
    local reader_pid reader_ready=0 writer_status=0 reader_status=0

    # The reader must be alive before starting the one-shot writer, just as in
    # the main matrix runner; otherwise a lost sample could look like a type
    # compatibility failure.
    if [[ "$reader" == "C#" ]]; then
        timeout "$((timeout_seconds + 5))" dotnet "$cs_peer" \
            --case "$case_id" --type CorpusArrayOfSequences.Sample --topic "$topic" \
            --runtime-version 7.7.0 --role reader --fixture default > "$reader_log" 2>&1 &
    else
        timeout "$((timeout_seconds + 5))" "$cpp_peer" \
            reader "$topic" "$timeout_seconds" "$case_id" default > "$reader_log" 2>&1 &
    fi
    reader_pid=$!

    for _ in $(seq 1 100); do
        if grep -q '^READY' "$reader_log"; then
            reader_ready=1
            break
        fi
        if ! kill -0 "$reader_pid" 2>/dev/null; then
            reader_status=1
            break
        fi
        sleep 0.1
    done

    if [[ "$reader_ready" -eq 1 ]]; then
        if [[ "$writer" == "C#" ]]; then
            timeout "$((timeout_seconds + 5))" dotnet "$cs_peer" \
                --case "$case_id" --type CorpusArrayOfSequences.Sample --topic "$topic" \
                --runtime-version 7.7.0 --role writer --fixture default > "$writer_log" 2>&1 || writer_status=$?
        else
            timeout "$((timeout_seconds + 5))" "$cpp_peer" \
                writer "$topic" "$timeout_seconds" "$case_id" default > "$writer_log" 2>&1 || writer_status=$?
        fi
    else
        : > "$writer_log"
        writer_status=1
    fi

    if ! wait "$reader_pid"; then
        reader_status=1
    fi

    echo "=== RTI oracle C# $writer -> $reader ==="
    for log in "$writer_log" "$reader_log"; do
        grep -E '^(READY|PASS|RECEIVED|SENT|FAIL|FAIL_DETAIL|WRITER_MATCHED|READER_STATS)( |$)' "$log" || true
        grep -Ei 'not assignable|type consistency|typeobject|type object|incompatible|type mismatch' "$log" \
            | sed 's/^/RTI_DIAGNOSTIC /' || true
    done

    if [[ "$writer_status" -eq 0 && "$reader_status" -eq 0 ]]; then
        return 0
    fi
    return 1
}

probe_status=0
run_pair "C#" "C#" "oracle_cs_cs" || probe_status=1
run_pair "C#" "C++" "oracle_cs_cpp" || probe_status=1
run_pair "C++" "C#" "oracle_cpp_cs" || probe_status=1

exit "$probe_status"
