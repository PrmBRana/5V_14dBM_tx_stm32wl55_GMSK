#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_GS_DIR="$(cd "$SCRIPT_DIR/../../.." && pwd)"
TOOLCHAIN_DIR="$REPO_GS_DIR/.mono_toolchain"
APP_DIR="$SCRIPT_DIR/OpenBirdsOperation/OpenBirdsOperation/bin/Debug"
STORAGE_DIR="$SCRIPT_DIR/../Storage"

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

# Pick target executable (prefer NAST_GS_CODE/Open_Birds_Operation.exe so relative paths ../Storage & Utils/ always match)
TARGET_EXE="${1:-$SCRIPT_DIR/Open_Birds_Operation.exe}"
if [ ! -f "$TARGET_EXE" ]; then
    TARGET_EXE="$SCRIPT_DIR/Open Birds Operation.exe"
fi
if [ ! -f "$TARGET_EXE" ]; then
    TARGET_EXE="$APP_DIR/Open Birds Operation.exe"
fi
if [ ! -f "$TARGET_EXE" ]; then
    echo "[ERROR] Open Birds Operation.exe not found under:"
    echo "        $SCRIPT_DIR"
    echo "        $APP_DIR"
    exit 1
fi

# Ensure matching .exe.config exists alongside the executable
if [ ! -f "${TARGET_EXE}.config" ] && [ -f "$SCRIPT_DIR/OpenBirdsOperation/OpenBirdsOperation/App.config" ]; then
    cp -f "$SCRIPT_DIR/OpenBirdsOperation/OpenBirdsOperation/App.config" "${TARGET_EXE}.config" 2>/dev/null || true
fi

# 1. Native Windows (Git Bash / MSYS2 / Cygwin)
UNAME_S="$(uname -s 2>/dev/null || echo Linux)"
case "$UNAME_S" in
    MINGW*|MSYS*|CYGWIN*)
        cd "$SCRIPT_DIR"
        exec "$TARGET_EXE"
        ;;
esac

# 2. WSL (Windows Subsystem for Linux) without local XServer -> launch directly on Windows host
if grep -qi microsoft /proc/version 2>/dev/null && command -v wslpath >/dev/null 2>&1 && command -v cmd.exe >/dev/null 2>&1; then
    if [ -z "${DISPLAY:-}" ] || [ ! -d "/tmp/.X11-unix" ]; then
        cd "$SCRIPT_DIR"
        WIN_EXE="$(wslpath -w "$TARGET_EXE")"
        exec cmd.exe /c start "" "$WIN_EXE"
    fi
fi

# 3. Linux X11 / Wayland DISPLAY & XAUTHORITY auto-detection
detect_linux_display() {
    local uid
    uid="$(id -u)"

    # Check if current DISPLAY points to a valid X11 socket
    if [ -n "${DISPLAY:-}" ]; then
        local dnum="${DISPLAY#:}"
        dnum="${dnum%%.*}"
        if [ -S "/tmp/.X11-unix/X${dnum}" ]; then
            return 0
        fi
    fi

    # Check GNOME Wayland setup display (often :1 on Ubuntu)
    if [ -n "${GNOME_SETUP_DISPLAY:-}" ]; then
        local gnum="${GNOME_SETUP_DISPLAY#:}"
        gnum="${gnum%%.*}"
        if [ -S "/tmp/.X11-unix/X${gnum}" ]; then
            export DISPLAY="$GNOME_SETUP_DISPLAY"
            return 0
        fi
    fi

    # Scan /tmp/.X11-unix/X* sockets owned by current user first, then any socket
    local sock num
    for sock in /tmp/.X11-unix/X*; do
        [ -S "$sock" ] || continue
        if [ "$(stat -c '%u' "$sock" 2>/dev/null)" = "$uid" ]; then
            num="${sock#/tmp/.X11-unix/X}"
            export DISPLAY=":${num}"
            return 0
        fi
    done
    for sock in /tmp/.X11-unix/X*; do
        [ -S "$sock" ] || continue
        num="${sock#/tmp/.X11-unix/X}"
        export DISPLAY=":${num}"
        return 0
    done

    export DISPLAY="${DISPLAY:-:0}"
}

detect_linux_xauthority() {
    if [ -n "${XAUTHORITY:-}" ] && [ -f "$XAUTHORITY" ]; then
        return 0
    fi
    local uid
    uid="$(id -u)"
    local cand
    for cand in \
        /run/user/"$uid"/.mutter-Xwaylandauth.* \
        /run/user/"$uid"/gdm/Xauthority \
        /run/user/"$uid"/Xauthority \
        "$HOME/.Xauthority"; do
        if [ -f "$cand" ]; then
            export XAUTHORITY="$cand"
            return 0
        fi
    done
}

detect_linux_display
detect_linux_xauthority

# Ensure bundled libgdiplus compatibility symlinks on newer Linux distros (e.g. Ubuntu 24.04 / Debian 12 / Fedora)
ensure_toolchain_compat_libs() {
    [ -d "$TOOLCHAIN_DIR/usr/lib" ] || return 0
    local sys_lib_dirs="/lib/x86_64-linux-gnu /usr/lib/x86_64-linux-gnu /usr/lib64 /usr/lib"
    local d

    # Map older sonames required by bundled libgdiplus.so.0 to newer distro equivalents if missing
    for pair in \
        "libtiff.so.5:libtiff.so.6" \
        "libwebp.so.6:libwebp.so.7" \
        "libjpeg.so.8:libjpeg.so.62" \
        "libpcre.so.3:libpcre.so.1"; do
        local needed="${pair%%:*}"
        local fallback="${pair#*:}"
        local found=0
        for d in "$TOOLCHAIN_DIR/usr/lib" $sys_lib_dirs; do
            if [ -e "$d/$needed" ]; then
                found=1
                break
            fi
        done
        if [ "$found" -eq 0 ]; then
            for d in $sys_lib_dirs; do
                if [ -e "$d/$fallback" ]; then
                    ln -sf "$d/$fallback" "$TOOLCHAIN_DIR/usr/lib/$needed" 2>/dev/null || true
                    break
                fi
            done
        fi
    done
}

ensure_toolchain_compat_libs

chmod +x "$TOOLCHAIN_DIR/usr/bin/mono-sgen" "$TOOLCHAIN_DIR/usr/bin/mono" 2>/dev/null || true

MONO_BIN=""
USE_BUNDLED=0
if [ "$(uname -m 2>/dev/null)" = "x86_64" ] && [ -x "$TOOLCHAIN_DIR/usr/bin/mono-sgen" ]; then
    MONO_BIN="$TOOLCHAIN_DIR/usr/bin/mono-sgen"
    USE_BUNDLED=1
elif [ -x "$TOOLCHAIN_DIR/usr/bin/mono" ]; then
    MONO_BIN="$TOOLCHAIN_DIR/usr/bin/mono"
    USE_BUNDLED=1
elif command -v mono >/dev/null 2>&1; then
    MONO_BIN="$(command -v mono)"
    USE_BUNDLED=0
fi

if [ -z "$MONO_BIN" ]; then
    echo "[ERROR] Mono runtime not found."
    echo "        Install Mono on Linux with: sudo apt update && sudo apt install -y mono-complete"
    exit 1
fi

if [ "$USE_BUNDLED" -eq 1 ]; then
    export LD_LIBRARY_PATH="$TOOLCHAIN_DIR/usr/lib:${LD_LIBRARY_PATH:-}"
    export MONO_GAC_PREFIX="$TOOLCHAIN_DIR/usr"
    export MONO_PATH="$TOOLCHAIN_DIR/usr/lib/mono/4.5:$TOOLCHAIN_DIR/usr/lib/mono/4.8-api/Facades:$SCRIPT_DIR:$APP_DIR"
    export MONO_CFG_DIR="$TOOLCHAIN_DIR/etc"
fi

cd "$SCRIPT_DIR"
exec "$MONO_BIN" "$TARGET_EXE"
