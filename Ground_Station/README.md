# Ground Station Software

Python GUI and desktop GS tools that talk to the STM32WL55JC2 ground-station
radio over USB-UART (115200 8N1). All paths are relative to this folder, so a
fresh clone works on any machine.

Repository: https://github.com/PrmBRana/5V_14dBM_tx_stm32wl55_GMSK

## Python console (recommended)

```bash
git clone https://github.com/PrmBRana/5V_14dBM_tx_stm32wl55_GMSK.git nuttxspace
cd nuttxspace/Ground_Station

# Ubuntu/Debian
sudo apt install python3 python3-pip python3-tk
pip3 install -r requirements.txt

# Run (do not copy gs_communicator.py out of this folder — logos stay beside it)
python3 gs_communicator.py
# or
./run_gs_python.sh
```

| Need | Package |
|:---|:---|
| Serial port | `pyserial` |
| Logos / camera preview | `pillow` |
| Excel export | `openpyxl` |
| GUI | `python3-tk` (`tkinter`) |

Serial port is auto-detected (`/dev/ttyUSB*`, `/dev/ttyACM*`, macOS `cu.usb*`, Windows `COMx`). Pick another port in the GUI, or:

```bash
python3 gs_communicator.py -p /dev/ttyUSB0
python3 gs_communicator.py -p COM3
python3 gs_communicator.py --cli
```

Logos (`antarikshya_logo.png`, `nepal_flag.png`) and telemetry files are loaded
from this directory, not from `/home/prem/...`.

## Desktop GS (OpenBirdsOperation)

Linux launcher (no hardcoded home directory):

```bash
cd Desktop_GS_SOFTWARE-20261101T100316Z-1-001/Desktop_GS_SOFTWARE/NAST_GS_CODE
./run_gs_software.sh
```

The `.desktop` file uses the launcher script location, so it works after clone.

## Flash the GS radio firmware

`gs_m0plus.bin` is included so clones can flash without building:

```bash
cd ../..   # repository root
make flash_GS
```
