#!/usr/bin/env bash
set -e
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
export PYTHONPATH="$SCRIPT_DIR/Ground_Station/utilities/pylibs:${PYTHONPATH:-}"

# Auto-detect display if launching without DISPLAY set
if [ -z "${DISPLAY:-}" ]; then
    for sock in /tmp/.X11-unix/X*; do
        if [ -S "$sock" ]; then
            num="${sock#/tmp/.X11-unix/X}"
            export DISPLAY=":${num}"
            break
        fi
    done
    export DISPLAY="${DISPLAY:-:0}"
fi

# Allow launching legacy OpenBirdsOperation with --legacy flag
if [ "${1:-}" = "--legacy" ] || [ "${1:-}" = "--birds" ]; then
    shift
    exec "$SCRIPT_DIR/Ground_Station/Desktop_GS_SOFTWARE-20261101T100316Z-1-001/Desktop_GS_SOFTWARE/NAST_GS_CODE/run_gs_software.sh" "$@"
fi

# Default: Launch modern Antarikchya Pratisthan Nepal All-In-One Ground Station
exec python3 "$SCRIPT_DIR/Ground_Station/gs_communicator.py" "$@"
