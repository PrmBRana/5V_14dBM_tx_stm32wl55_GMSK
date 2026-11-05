############################################################################
# Top-Level Makefile for Dual-Core STM32WL55JC Satellite & Ground Station
#
# Hardware Targets:
#   JC1 = STM32WL55JC1 (SATELLITE BOARD):
#      - CPU1 (Cortex-M4): NuttX RTOS (0x08000000 - 0x08031FFF, 200 KB)
#      - CPU2 (Cortex-M0+): Satellite Radio Coprocessor in JC1/cpu2/ (0x08032000, 56 KB)
#   JC2 = STM32WL55JC2 (GROUND STATION BOARD):
#      - CPU1 (Cortex-M4): NuttX RTOS (0x08000000 - 0x08031FFF, 200 KB) [same nuttx/]
#      - CPU2 (Cortex-M0+): Ground Station Firmware in CPU2/ (0x08032000, 56 KB)
############################################################################

SAT_CPU2_DIR ?= Satellite_M0+/Com_sat
GS_CPU2_DIR  ?= Ground_Station
M4_DIR       ?= nuttx

STM32_PROG   ?= $(shell which STM32_Programmer_CLI 2>/dev/null || \
	ls $(HOME)/STMicroelectronics/STM32Cube/STM32CubeProgrammer/bin/STM32_Programmer_CLI 2>/dev/null || \
	ls /home/*/STMicroelectronics/STM32Cube/STM32CubeProgrammer/bin/STM32_Programmer_CLI 2>/dev/null | head -n 1 || \
	ls /opt/st/stm32cubeide_*/plugins/com.st.stm32cube.ide.mcu.externaltools.cubeprogrammer.linux64_*/tools/bin/STM32_Programmer_CLI 2>/dev/null | head -n 1 || \
	ls /opt/st/stm32cubeprogrammer_*/bin/STM32_Programmer_CLI 2>/dev/null | head -n 1 || \
	echo STM32_Programmer_CLI)

# ==========================================================================
# DEFAULT BUILD TARGET
# Builds Satellite M4 (NuttX), Satellite M0+ (JC2), and Ground Station (CPU2)
# ==========================================================================
all: build_all

build_all: build_sat_m0 build_gs build_m4
	@echo ""
	@echo "================================================================="
	@echo "  ALL BUILDS SUCCESSFUL!"
	@echo "  1. Satellite M4 Binary  : $(M4_DIR)/nuttx.bin"
	@echo "  2. Satellite M0+ Binary : $(SAT_CPU2_DIR)/satellite.bin"
	@echo "  3. Ground Station Binary: $(GS_CPU2_DIR)/gs_m0plus.bin"
	@echo "================================================================="
	@echo "  Flashing Commands:"
	@echo "    make flash_M4    -> Flash Satellite Cortex-M4 (0x08000000)"
	@echo "    make flashsatM0  -> Flash Satellite Cortex-M0+ (0x08032000)"
	@echo "    make flash_GS    -> Flash Ground Station M0+   (0x08032000)"
	@echo "================================================================="

M4_CONFIG    ?= nucleo-wl55jc:nsh

configure_m4:
	cd $(M4_DIR) && ./tools/configure.sh -l $(M4_CONFIG)

$(M4_DIR)/.config:
	cd $(M4_DIR) && ./tools/configure.sh -l $(M4_CONFIG)

build_m4: $(M4_DIR)/.config
	@echo ">>> Building JC1/JC2 Cortex-M4 (NuttX - shared for both boards)..."
	$(MAKE) -C $(M4_DIR)

build_sat_m0:
	@echo ">>> Building JC1 Satellite Cortex-M0+ (Low Power PA) in $(SAT_CPU2_DIR)..."
	$(MAKE) -C $(SAT_CPU2_DIR)

build_gs:
	@echo ">>> Building JC2 Ground Station Cortex-M0+ (High Power PA) in $(GS_CPU2_DIR)..."
	$(MAKE) -C $(GS_CPU2_DIR)

# ==========================================================================
# 1. FLASH JC1 OR JC2 CORTEX-M4 (NUTTX: 0x08000000 - 0x08031FFF)
# Same NuttX binary works on both JC1 (Satellite) and JC2 (Ground Station)
# This writes ONLY to sectors 0-99 (200 KB max).
# CPU2 at 0x08032000 (sectors 100-127) is strictly preserved!
# ==========================================================================
flash_M4 flash_m4 flash_cpu1 flash_CPU1: $(M4_DIR)/.config
	@if [ ! -f $(M4_DIR)/nuttx.bin ]; then \
	  echo "$(M4_DIR)/nuttx.bin not found. Building NuttX..."; \
	  $(MAKE) -C $(M4_DIR); \
	fi
	@if [ -x "$$(which $(STM32_PROG) 2>/dev/null)" ] || [ -x "$(STM32_PROG)" ]; then \
	  echo "Flashing Satellite CPU1 (NuttX) to 0x08000000 using STM32_Programmer_CLI (CPU2 preserved)..."; \
	  $(STM32_PROG) -c port=SWD mode=UR -w $(M4_DIR)/nuttx.bin 0x08000000 -v -hardRst 2>/dev/null || \
	  $(STM32_PROG) -c port=SWD mode=HOTPLUG -w $(M4_DIR)/nuttx.bin 0x08000000 -v -hardRst 2>/dev/null || \
	  $(STM32_PROG) -c port=SWD -w $(M4_DIR)/nuttx.bin 0x08000000 -v -hardRst || \
	  (echo "STM32_Programmer_CLI failed. Trying OpenOCD fallback..." && \
	  openocd -f interface/stlink.cfg -f target/stm32wlx.cfg -c "gdb_port disabled; tcl_port disabled; telnet_port disabled; init; halt; mww 0xE004203C 0x00001800; mww 0x40003000 0x0000AAAA; reg primask 1; mww 0x58004010 0x0000C3FB; mww 0x58004060 0x0000C3FB; flash write_image erase $(M4_DIR)/nuttx.bin 0x08000000 bin; verify_image $(M4_DIR)/nuttx.bin 0x08000000 bin; mww 0x58004000 [expr {[mrw 0x58004000] & ~0x10000}]; reset run; exit" 2>/dev/null || \
	  openocd -f interface/stlink-dap.cfg -f target/stm32wlx.cfg -c "gdb_port disabled; tcl_port disabled; telnet_port disabled; init; halt; mww 0xE004203C 0x00001800; mww 0x40003000 0x0000AAAA; reg primask 1; mww 0x58004010 0x0000C3FB; mww 0x58004060 0x0000C3FB; flash write_image erase $(M4_DIR)/nuttx.bin 0x08000000 bin; verify_image $(M4_DIR)/nuttx.bin 0x08000000 bin; mww 0x58004000 [expr {[mrw 0x58004000] & ~0x10000}]; reset run; exit"); \
	else \
	  echo "Flashing Satellite CPU1 (NuttX) to 0x08000000 using OpenOCD (CPU2 preserved)..."; \
	  openocd -f interface/stlink.cfg -f target/stm32wlx.cfg -c "gdb_port disabled; tcl_port disabled; telnet_port disabled; init; halt; mww 0xE004203C 0x00001800; mww 0x40003000 0x0000AAAA; reg primask 1; mww 0x58004010 0x0000C3FB; mww 0x58004060 0x0000C3FB; flash write_image erase $(M4_DIR)/nuttx.bin 0x08000000 bin; verify_image $(M4_DIR)/nuttx.bin 0x08000000 bin; mww 0x58004000 [expr {[mrw 0x58004000] & ~0x10000}]; reset run; exit" 2>/dev/null || \
	  openocd -f interface/stlink-dap.cfg -f target/stm32wlx.cfg -c "gdb_port disabled; tcl_port disabled; telnet_port disabled; init; halt; mww 0xE004203C 0x00001800; mww 0x40003000 0x0000AAAA; reg primask 1; mww 0x58004010 0x0000C3FB; mww 0x58004060 0x0000C3FB; flash write_image erase $(M4_DIR)/nuttx.bin 0x08000000 bin; verify_image $(M4_DIR)/nuttx.bin 0x08000000 bin; mww 0x58004000 [expr {[mrw 0x58004000] & ~0x10000}]; reset run; exit"; \
	fi

# ==========================================================================
# 2. FLASH JC1 SATELLITE CORTEX-M0+ (JC1/cpu2: 0x08032000 - 0x0803FFFF)
# JC1 = Satellite board. Low Power RF path (external PA via PC2/PC3).
# This writes ONLY to sectors 100-127 (56 KB max).
# CPU1 at 0x08000000 (sectors 0-99) is strictly preserved!
# ==========================================================================
flashsatM0 flash_satM0 flash_satm0 flashsat flash_sat_m0 flash_sat flash_SAT flash_m0 flash_M0:
	@echo ">>> Flashing Satellite Cortex-M0+ from $(SAT_CPU2_DIR)..."
	$(MAKE) -C $(SAT_CPU2_DIR) flash_sat

# ==========================================================================
# 3. FLASH JC2 GROUND STATION CORTEX-M0+ (CPU2: 0x08032000 - 0x0803FFFF)
# JC2 = Ground Station board. High Power RF path (+22 dBm internal PA).
# Flashes the Ground Station firmware to the JC2 board.
# ==========================================================================
flash_GS flash_gs flash_cpu2_gs:
	@echo ">>> Flashing Ground Station M0+ from $(GS_CPU2_DIR)..."
	$(MAKE) -C $(GS_CPU2_DIR) flash_gs

# ==========================================================================
# COMBINED SATELLITE FLASHING (CPU1 + CPU2)
# ==========================================================================
flash_all_sat: flash_M4 flashsatM0

# ==========================================================================
# CLEAN
# ==========================================================================
clean:
	$(MAKE) -C $(SAT_CPU2_DIR) clean
	$(MAKE) -C $(GS_CPU2_DIR) clean
	$(MAKE) -C $(M4_DIR) clean

clean_m0:
	$(MAKE) -C $(SAT_CPU2_DIR) clean
	$(MAKE) -C $(GS_CPU2_DIR) clean

# ==========================================================================
# DESKTOP GROUND STATION GUI (All-In-One APN Console & Legacy C#)
# ==========================================================================
gs_gui desktop_gs run_gs_gui run_gs:
	./Run_GS_Linux.sh

legacy_gs_gui:
	./Ground_Station/Desktop_GS_SOFTWARE-20261101T100316Z-1-001/Desktop_GS_SOFTWARE/NAST_GS_CODE/run_gs_software.sh

build_gs_gui:
	$(MAKE) -C Ground_Station/Desktop_GS_SOFTWARE-20261101T100316Z-1-001/Desktop_GS_SOFTWARE/NAST_GS_CODE build

.PHONY: all build_all configure_m4 build_m4 build_sat_m0 build_gs flash_M4 flash_m4 flash_cpu1 flash_CPU1 flashsatM0 flash_sat_m0 flash_sat flash_SAT flash_m0 flash_M0 flash_GS flash_gs flash_cpu2_gs flash_all_sat clean clean_m0 gs_gui desktop_gs run_gs_gui run_gs legacy_gs_gui build_gs_gui
