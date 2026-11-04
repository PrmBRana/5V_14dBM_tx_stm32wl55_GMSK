# =============================================================================
# NAST Ground Station Software (OpenBirdsOperation / Slippers2Sat Operation)
# Makefile for building and running "Open Birds Operation.exe"
# =============================================================================

PROJ_DIR      := OpenBirdsOperation/OpenBirdsOperation
TOOLCHAIN_DIR := $(abspath ../../../.mono_toolchain)

MONO_BIN      := $(TOOLCHAIN_DIR)/usr/bin/mono-sgen
MCS_EXE       := $(TOOLCHAIN_DIR)/usr/lib/mono/4.5/mcs.exe
API_DIR       := $(TOOLCHAIN_DIR)/usr/lib/mono/4.8-api
GAC_DIR       := $(TOOLCHAIN_DIR)/usr/lib/mono/gac
MONO_ENV      := LD_LIBRARY_PATH=$(TOOLCHAIN_DIR)/usr/lib MONO_GAC_PREFIX=$(TOOLCHAIN_DIR)/usr MONO_PATH=$(TOOLCHAIN_DIR)/usr/lib/mono/4.5:$(API_DIR)/Facades:$(abspath $(PROJ_DIR)/bin/Debug) MONO_CFG_DIR=$(TOOLCHAIN_DIR)/etc

OUT_DEBUG     := $(PROJ_DIR)/bin/Debug/Open Birds Operation.exe
OUT_RELEASE   := $(PROJ_DIR)/bin/Release/Open Birds Operation.exe
OUT_X64_DBG   := $(PROJ_DIR)/bin/x64/Debug/Open Birds Operation.exe
OUT_X64_REL   := $(PROJ_DIR)/bin/x64/Release/Open Birds Operation.exe
OUT_ROOT_EXE  := Open_Birds_Operation.exe

CS_SOURCES    := $(wildcard $(PROJ_DIR)/*.cs) $(wildcard $(PROJ_DIR)/Properties/*.cs)

RESOURCES     := \
	-resource:$(PROJ_DIR)/obj/Debug/OpenBirdsOperation.commandList.resources \
	-resource:$(PROJ_DIR)/obj/Debug/OpenBirdsOperation.inputDialogADCS.resources \
	-resource:$(PROJ_DIR)/obj/Debug/OpenBirdsOperation.inputDialogBeacon.resources \
	-resource:$(PROJ_DIR)/obj/Debug/OpenBirdsOperation.inputDialogCamera.resources \
	-resource:$(PROJ_DIR)/obj/Debug/OpenBirdsOperation.inputDialogDigipeater.resources \
	-resource:$(PROJ_DIR)/obj/Debug/OpenBirdsOperation.inputDialogEPDM.resources \
	-resource:$(PROJ_DIR)/obj/Debug/OpenBirdsOperation.inputDialogHK.resources \
	-resource:$(PROJ_DIR)/obj/Debug/OpenBirdsOperation.inputDialogLOG.resources \
	-resource:$(PROJ_DIR)/obj/Debug/OpenBirdsOperation.operationMain.resources \
	-resource:$(PROJ_DIR)/obj/Debug/OpenBirdsOperation.Properties.Resources.resources \
	-resource:$(PROJ_DIR)/obj/Debug/OpenBirdsOperation.sendDialogDigipeaterCMD.resources

REFERENCES    := \
	-lib:"$(API_DIR)","$(API_DIR)/Facades","$(PROJ_DIR)/bin/Debug" \
	-r:mscorlib.dll \
	-r:System.dll \
	-r:System.Core.dll \
	-r:System.Data.dll \
	-r:System.Data.DataSetExtensions.dll \
	-r:System.Drawing.dll \
	-r:System.Windows.Forms.dll \
	-r:System.Xml.dll \
	-r:System.Xml.Linq.dll \
	-r:System.Numerics.dll \
	-r:System.Security.dll \
	-r:System.Configuration.dll \
	-r:System.ComponentModel.DataAnnotations.dll \
	-r:System.Net.Http.dll \
	-r:WindowsBase.dll \
	-r:Microsoft.CSharp.dll \
	-r:"$(GAC_DIR)/Microsoft.VisualBasic/10.0.0.0__b03f5f7f11d50a3a/Microsoft.VisualBasic.dll" \
	-r:netstandard.dll \
	-r:DocumentFormat.OpenXml.dll \
	-r:DocumentFormat.OpenXml.Framework.dll \
	-r:EPPlus.dll \
	-r:EPPlus.Interfaces.dll \
	-r:EPPlus.System.Drawing.dll \
	-r:Microsoft.IO.RecyclableMemoryStream.dll \
	-r:One_Sgp4.dll \
	-r:System.Buffers.dll \
	-r:System.ComponentModel.Annotations.dll \
	-r:System.Memory.dll \
	-r:System.Numerics.Vectors.dll \
	-r:System.Runtime.CompilerServices.Unsafe.dll

.PHONY: all build run clean

all: build

build:
	@echo "=================================================================="
	@echo " Building NAST Ground Station Software (Open Birds Operation.exe) "
	@echo " Features: Live CW Beacon Received (gs_communicator.py parity),   "
	@echo "           CW-1/CW-2 Telemetry Cards, Morse Decoder, HK & Camera  "
	@echo "=================================================================="
	@mkdir -p "$(PROJ_DIR)/bin/Debug" "$(PROJ_DIR)/bin/Release" "$(PROJ_DIR)/bin/x64/Debug" "$(PROJ_DIR)/bin/x64/Release"
	$(MONO_ENV) "$(MONO_BIN)" "$(MCS_EXE)" \
		-nostdlib \
		-target:winexe \
		-optimize+ \
		-out:"$(OUT_DEBUG)" \
		-win32icon:"$(PROJ_DIR)/apn_logo.ico" \
		$(REFERENCES) \
		$(RESOURCES) \
		$(CS_SOURCES)
	@cp -f "$(OUT_DEBUG)" "$(OUT_RELEASE)"
	@cp -f "$(OUT_DEBUG)" "$(OUT_X64_DBG)"
	@cp -f "$(OUT_DEBUG)" "$(OUT_X64_REL)"
	@cp -f "$(OUT_DEBUG)" "$(OUT_ROOT_EXE)"
	@cp -f "$(OUT_DEBUG)" "Open Birds Operation.exe"
	@cp -f $(PROJ_DIR)/bin/Debug/*.dll . 2>/dev/null || true
	@echo "=================================================================="
	@echo " Build Complete! Generated Executables:"
	@ls -lh "$(OUT_DEBUG)" "$(OUT_RELEASE)" "$(OUT_ROOT_EXE)"
	@echo " To launch on Linux, run: ./run_gs_software.sh  or  make run"
	@echo "=================================================================="

run:
	./run_gs_software.sh

clean:
	rm -f "$(OUT_DEBUG)" "$(OUT_RELEASE)" "$(OUT_X64_DBG)" "$(OUT_X64_REL)" "$(OUT_ROOT_EXE)" "Open Birds Operation.exe"
