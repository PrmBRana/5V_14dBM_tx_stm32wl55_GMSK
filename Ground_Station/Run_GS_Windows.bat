@echo off
setlocal
cd /d "%~dp0"
set PYTHONPATH=%~dp0utilities\pylibs;%PYTHONPATH%

if "%1"=="--legacy" goto LEGACY
if "%1"=="--birds" goto LEGACY

python "%~dp0gs_communicator.py" %*
goto END

:LEGACY
shift
cd /d "%~dp0Desktop_GS_SOFTWARE-20261101T100316Z-1-001\Desktop_GS_SOFTWARE\NAST_GS_CODE"
call "Run_GS_Windows.bat" %*

:END
