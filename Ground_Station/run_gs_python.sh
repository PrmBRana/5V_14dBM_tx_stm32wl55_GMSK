#!/usr/bin/env bash
# Portable launcher for gs_communicator.py — works after any clone.
set -e
DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$DIR"

PYTHON="${PYTHON:-python3}"
if ! command -v "$PYTHON" >/dev/null 2>&1; then
    echo "[ERROR] python3 not found. Install Python 3.8+ and retry."
    exit 1
fi

if ! "$PYTHON" -c "import serial" 2>/dev/null; then
    echo "[INFO] Installing Ground Station Python dependencies..."
    "$PYTHON" -m pip install -r "$DIR/requirements.txt"
fi

if ! "$PYTHON" -c "import tkinter" 2>/dev/null; then
    echo "[WARN] python3-tk / tkinter is missing. GUI needs it."
    echo "       Ubuntu/Debian:  sudo apt install python3-tk python3-pip"
    echo "       Fedora:         sudo dnf install python3-tkinter"
    echo "       Falling back to CLI if no display is available."
fi

exec "$PYTHON" "$DIR/gs_communicator.py" "$@"
