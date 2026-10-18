#!/usr/bin/env bash
# Executes the argument files prepared by Capture-RtiOracles.ps1 in one pinned image.
set -euo pipefail
root="$1" version="$2" generator="$3" preprocessing="$4"
[[ "$NDDSHOME" == *"rti_connext_dds-$version" ]]
rtiddsgen -version > "$root/toolchain.txt" 2>&1
actual="$(sed -n 's/.*rtiddsgen version //p' "$root/toolchain.txt" | head -n 1)"
[[ "${actual%% *}" == "$generator" ]]
if [[ "$preprocessing" == enabled ]]; then
    if ! command -v cpp >/dev/null; then
        apt-get update -qq > "$root/preprocessor-setup.log" 2>&1
        DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends g++ >> "$root/preprocessor-setup.log" 2>&1
    fi
    cpp --version >> "$root/toolchain.txt"
fi
uname -a >> "$root/toolchain.txt"
for job in "$root"/.capture/jobs/*; do
    mapfile -t arguments < "$job/arguments.txt"
    read -r destination < "$job/destination.txt"
    output="$root/$destination"
    mkdir -p "$output"
    status=0
    rtiddsgen "${arguments[@]}" > "$output/rtiddsgen.log" 2>&1 || status=$?
    printf '%s\n' "$status" > "$job/exit-code.txt"
    if [[ "$status" == 0 ]]; then
        cp -R "$job/generated/." "$output/"
    fi
    printf '%s\t%s\n' "$destination" "$status"
done
