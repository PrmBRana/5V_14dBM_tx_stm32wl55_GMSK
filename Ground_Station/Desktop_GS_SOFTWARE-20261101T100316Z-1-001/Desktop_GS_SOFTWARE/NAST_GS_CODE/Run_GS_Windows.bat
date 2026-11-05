@echo off
setlocal
cd /d "%~dp0"

if not exist "..\Storage\LOG_DETAILS" mkdir "..\Storage\LOG_DETAILS"
if not exist "..\Storage\ADDRESS_DETAILS" mkdir "..\Storage\ADDRESS_DETAILS"
if not exist "..\Storage\CAM_DETAILS" mkdir "..\Storage\CAM_DETAILS"
if not exist "..\Storage\HK_DETAILS" mkdir "..\Storage\HK_DETAILS"
if not exist "..\Storage\CW_DETAILS" mkdir "..\Storage\CW_DETAILS"
if not exist "..\Storage\ADCS_DETAILS" mkdir "..\Storage\ADCS_DETAILS"
if not exist "..\Storage\EPDM_DETAILS" mkdir "..\Storage\EPDM_DETAILS"
if not exist "..\Storage\BEACON_DETAIL" mkdir "..\Storage\BEACON_DETAIL"
if not exist "..\Storage\DIGIPEATER_LOG" mkdir "..\Storage\DIGIPEATER_LOG"
if not exist "..\Storage\DIGIPEATER_DETAILS" mkdir "..\Storage\DIGIPEATER_DETAILS"
if not exist "..\Storage\MSN_LOG" mkdir "..\Storage\MSN_LOG"

if exist "Open_Birds_Operation.exe" (
    start "" "%~dp0Open_Birds_Operation.exe"
    exit /b 0
)

if exist "Open Birds Operation.exe" (
    start "" "%~dp0Open Birds Operation.exe"
    exit /b 0
)

if exist "OpenBirdsOperation\OpenBirdsOperation\bin\Debug\Open Birds Operation.exe" (
    start "" "%~dp0OpenBirdsOperation\OpenBirdsOperation\bin\Debug\Open Birds Operation.exe"
    exit /b 0
)

echo [ERROR] Open_Birds_Operation.exe not found!
pause
exit /b 1
