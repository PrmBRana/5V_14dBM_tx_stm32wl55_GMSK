#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
TOOLCHAIN_DIR="$(cd "$SCRIPT_DIR/../../../.mono_toolchain" && pwd)"
APP_DIR="$SCRIPT_DIR/OpenBirdsOperation/OpenBirdsOperation/bin/Debug"

export DISPLAY="${DISPLAY:-:0}"
export LD_LIBRARY_PATH="$TOOLCHAIN_DIR/usr/lib:${LD_LIBRARY_PATH:-}"
export MONO_GAC_PREFIX="$TOOLCHAIN_DIR/usr"
export MONO_PATH="$TOOLCHAIN_DIR/usr/lib/mono/4.5:$TOOLCHAIN_DIR/usr/lib/mono/4.8-api/Facades:$APP_DIR:$SCRIPT_DIR"
export MONO_CFG_DIR="$TOOLCHAIN_DIR/etc"

TARGET_EXE="${1:-$APP_DIR/Open Birds Operation.exe}"
if [ ! -f "$TARGET_EXE" ]; then
    TARGET_EXE="$SCRIPT_DIR/Open_Birds_Operation.exe"
fi

cd "$(dirname "$TARGET_EXE")"
exec "$TOOLCHAIN_DIR/usr/bin/mono-sgen" "$TARGET_EXE"
