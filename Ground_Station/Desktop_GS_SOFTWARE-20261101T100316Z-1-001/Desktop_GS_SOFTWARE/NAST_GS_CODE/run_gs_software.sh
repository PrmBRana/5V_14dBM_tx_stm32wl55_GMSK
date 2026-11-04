#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_GS_DIR="$(cd "$SCRIPT_DIR/../../.." && pwd)"
TOOLCHAIN_DIR="$REPO_GS_DIR/.mono_toolchain"
APP_DIR="$SCRIPT_DIR/OpenBirdsOperation/OpenBirdsOperation/bin/Debug"
STORAGE_DIR="$SCRIPT_DIR/../Storage"

export DISPLAY="${DISPLAY:-${WAYLAND_DISPLAY:+:0}}"
export DISPLAY="${DISPLAY:-:0}"
export LD_LIBRARY_PATH="$TOOLCHAIN_DIR/usr/lib:${LD_LIBRARY_PATH:-}"
export MONO_GAC_PREFIX="$TOOLCHAIN_DIR/usr"
export MONO_PATH="$TOOLCHAIN_DIR/usr/lib/mono/4.5:$TOOLCHAIN_DIR/usr/lib/mono/4.8-api/Facades:$APP_DIR:$SCRIPT_DIR"
export MONO_CFG_DIR="$TOOLCHAIN_DIR/etc"

mkdir -p \
    "$STORAGE_DIR/LOG_DETAILS" \
    "$STORAGE_DIR/ADDRESS_DETAILS" \
    "$STORAGE_DIR/CAM_DETAILS" \
    "$STORAGE_DIR/HK_DETAILS" \
    "$STORAGE_DIR/CW_DETAILS" \
    "$STORAGE_DIR/ADCS_DETAILS" \
    "$STORAGE_DIR/EPDM_DETAILS" \
    "$STORAGE_DIR/BEACON_DETAIL" \
    "$STORAGE_DIR/DIGIPEATER_LOG" \
    "$STORAGE_DIR/DIGIPEATER_DETAILS" \
    "$STORAGE_DIR/MSN_LOG" \
    "$APP_DIR/../Storage/LOG_DETAILS" \
    "$APP_DIR/../Storage/CAM_DETAILS"

TARGET_EXE="${1:-$APP_DIR/Open Birds Operation.exe}"
if [ ! -f "$TARGET_EXE" ]; then
    TARGET_EXE="$SCRIPT_DIR/Open_Birds_Operation.exe"
fi
if [ ! -f "$TARGET_EXE" ]; then
    TARGET_EXE="$SCRIPT_DIR/Open Birds Operation.exe"
fi
if [ ! -f "$TARGET_EXE" ]; then
    echo "[ERROR] Open Birds Operation.exe not found under:"
    echo "        $APP_DIR"
    echo "        $SCRIPT_DIR"
    exit 1
fi

MONO_BIN="$TOOLCHAIN_DIR/usr/bin/mono-sgen"
if [ ! -x "$MONO_BIN" ]; then
    MONO_BIN="$TOOLCHAIN_DIR/usr/bin/mono"
fi
if [ ! -x "$MONO_BIN" ]; then
    MONO_BIN="$(command -v mono || true)"
fi
if [ -z "$MONO_BIN" ]; then
    echo "[ERROR] Mono runtime not found."
    echo "        Bundled toolchain: $TOOLCHAIN_DIR"
    echo "        Or install: sudo apt install mono-complete"
    exit 1
fi

cd "$(dirname "$TARGET_EXE")"
exec "$MONO_BIN" "$TARGET_EXE"
