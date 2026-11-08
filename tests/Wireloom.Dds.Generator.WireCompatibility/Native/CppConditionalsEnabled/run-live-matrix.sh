#!/usr/bin/env bash
set -euo pipefail

rti_peer="$(realpath "$1")"
wireloom_peer="$(realpath "$2")"
output_root="$(realpath -m "$3")"
mkdir -p "$output_root"

run_pair() {
    local representation="$1" name="$2" writer_peer="$3" reader_peer="$4"
    local scenario="$output_root/$name" topic="WireloomConditionalsEnabled_${name}_$RANDOM"
    mkdir -p "$scenario"
    "$reader_peer" reader "$representation" "$topic" "$scenario" >"$scenario/reader.log" 2>&1 &
    local reader_pid=$!
    "$writer_peer" writer "$representation" "$topic" "$scenario" >"$scenario/writer.log" 2>&1 &
    local writer_pid=$!
    local reader_status=0 writer_status=0
    wait "$reader_pid" || reader_status=$?
    wait "$writer_pid" || writer_status=$?
    if [[ "$reader_status" -ne 0 || "$writer_status" -ne 0 ]]; then
        echo "$name failed (reader=$reader_status writer=$writer_status)" >&2
        cat "$scenario/reader.log" "$scenario/writer.log" >&2
        return 1
    fi
    test -f "$scenario/received"
    echo "$name passed"
}

for representation in xcdr1 xcdr2; do
    run_pair "$representation" "${representation}-rti-to-rti" "$rti_peer" "$rti_peer"
    run_pair "$representation" "${representation}-rti-to-wireloom" "$rti_peer" "$wireloom_peer"
    run_pair "$representation" "${representation}-wireloom-to-rti" "$wireloom_peer" "$rti_peer"
done
