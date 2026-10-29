#include <string.h>

#include "radio_app.h"
#include "radio_driver.h"
#include "radio_board_if.h"
#include "ax25.h"
#include "uart_debug.h"

extern void SUBGRF_SetBufferBaseAddress(
    uint8_t txBaseAddress,
    uint8_t rxBaseAddress);

#define SUBGHZ_TX_BUFFER_BASE  0x00U
#define SUBGHZ_RX_BUFFER_BASE  0x80U

static RadioConfig_t s_config;
static RadioEvents_t s_radio_events;
static bool s_initialized = false;
static int8_t s_radio_tx_power_dbm = RADIO_TX_POWER_DBM;
static uint8_t s_radio_pa_select = RFO_LP; /* Satellite routes SX1262 RFO_LP (PB2) to external PAs & RF switch */

void RadioApp_SetTxPower(int8_t power)
{
    if (power > 14) power = 14;
    if (power < -17) power = -17;
    s_radio_tx_power_dbm = power;
}

int8_t RadioApp_GetTxPower(void)
{
    return s_radio_tx_power_dbm;
}

void RadioApp_SetPaSelect(uint8_t paSelect)
{
    s_radio_pa_select = paSelect;
}

uint8_t RadioApp_GetPaSelect(void)
{
    return s_radio_pa_select;
}

static void RadioApp_ArmCommon(
    uint32_t channelHz);

/*
 * ============================================================
 * INITIALIZE
 * ============================================================
 */

void RadioApp_Init(
    const RadioConfig_t *config,
    RadioEvents_t *events)
{
    if (config == NULL ||
        events == NULL)
    {
        return;
    }

    s_config = *config;
    s_radio_events = *events;

    s_initialized = true;

    uart2_printf(
        "RadioApp_Init: ENTER\r\n");

    Radio.Init(&s_radio_events);

    uart2_printf(
        "RadioApp_Init: Radio.Init done\r\n");

    /*
     * TX/RX FIFO regions.
     */
    SUBGRF_SetBufferBaseAddress(
        SUBGHZ_TX_BUFFER_BASE,
        SUBGHZ_RX_BUFFER_BASE);

    /*
     * Initial channel.
     */
    Radio.SetChannel(
        config->rxFrequency);

    uart2_printf(
        "Radio RX = %lu Hz\r\n",
        (unsigned long)
        config->rxFrequency);

    uart2_printf(
        "Radio TX = %lu Hz\r\n",
        (unsigned long)
        config->txFrequency);

    uart2_printf(
        "Bitrate = %d\r\n",
        RADIO_BIT_RATE_BPS);

    uart2_printf(
        "Deviation = %d Hz\r\n",
        RADIO_FDEV_HZ);

    /* SetRxConfig is required so SubgRf.RxContinuous stays true (Radio.Rx(0)
     * otherwise times out immediately). It also programs ST FSK defaults
     * (sync C1 94 C1, BT=1, whitening ON) — ForceOpen overwrites those. */
    Radio.SetRxConfig(
        MODEM_FSK,
        RADIO_RX_BANDWIDTH_HZ,
        RADIO_BIT_RATE_BPS,
        0,
        RADIO_RX_BANDWIDTH_HZ,
        (uint16_t)(RADIO_PREAMBLE_LENGTH_BITS / 8U),
        0,
        true,
        RADIO_FIXED_PACKET_LEN,
        false,
        false,
        0,
        false,
        true);
    RadioApp_ForceOpenGfskParams();

    uart2_puts("RadioApp_Init: FW-ID SAT-G3RUH-UPLINK8\r\n");
    uart2_puts("RadioApp_Init: GFSK RX = preamble + sync 93 0B 51 DE + 200 B G3RUH\r\n");
    uart2_printf(
        "RadioApp_Init: EXIT\r\n");
}

/*
 * ============================================================
 * COMMON RADIO ARM
 * ============================================================
 */

static void RadioApp_ArmCommon(
    uint32_t channelHz)
{
    /*
     * Always restore FIFO addresses.
     */
    SUBGRF_SetBufferBaseAddress(
        SUBGHZ_TX_BUFFER_BASE,
        SUBGHZ_RX_BUFFER_BASE);

    /*
     * Restore GFSK packet parameters.
     */
    RadioApp_ForceOpenGfskParams();

    /*
     * Select frequency.
     */
    Radio.SetChannel(channelHz);
}

/*
 * ============================================================
 * START RX
 * ============================================================
 */

void RadioApp_StartRx(void)
{
    if (!s_initialized)
        return;

    /* Do not call Radio.SetRxConfig() here: it writes sync C1 94 C1 and
     * whitening, which never match a GS uplink (sync 93 0B 51 DE, no whitening).
     * RxContinuous was set true in RadioApp_Init. */
    RadioApp_ArmCommon(
        s_config.rxFrequency);

    /* Antenna on the RX path. Both external PAs stay off. */
    RBI_ConfigRFSwitch(RBI_SWITCH_RX);

    extern void CPU2_Delay_Ms(uint32_t ms);
    SUBGRF_SetStandby(STDBY_XOSC);
    CPU2_Delay_Ms(5);
    Radio.Rx(0);
}

/*
 * ============================================================
 * SEND
 * ============================================================
 */

void RadioApp_Send(
    uint8_t *buffer,
    uint16_t length)
{
    if (!s_initialized ||
        buffer == NULL ||
        length == 0)
    {
        return;
    }

    extern void CPU2_Delay_Ms(uint32_t ms);
    SUBGRF_SetStandby(STDBY_XOSC);
    SUBGRF_SetPacketType(PACKET_TYPE_GFSK);

    SUBGRF_SetRfFrequency(s_config.txFrequency);

    static uint8_t s_downlink_sync[8] = RADIO_DOWNLINK_SYNC_WORD;
    SUBGRF_SetSyncWord(s_downlink_sync);

    PacketParams_t packetParams;
    memset(&packetParams, 0, sizeof(packetParams));
    packetParams.PacketType = PACKET_TYPE_GFSK;
    packetParams.Params.Gfsk.PreambleLength = RADIO_PREAMBLE_LENGTH_BITS;
    packetParams.Params.Gfsk.PreambleMinDetect = RADIO_PREAMBLE_DETECTOR_OFF;
    packetParams.Params.Gfsk.SyncWordLength = RADIO_DOWNLINK_SYNC_WORD_BITS;
    packetParams.Params.Gfsk.AddrComp = RADIO_ADDRESSCOMP_FILT_OFF;
    packetParams.Params.Gfsk.HeaderType = RADIO_PACKET_FIXED_LENGTH;
    packetParams.Params.Gfsk.PayloadLength = length;
    packetParams.Params.Gfsk.CrcLength = RADIO_CRC_OFF;
    packetParams.Params.Gfsk.DcFree = RADIO_DC_FREE_OFF;
    SUBGRF_SetPacketParams(&packetParams);
    SUBGRF_SetSyncWord(s_downlink_sync);

    ModulationParams_t modParams;
    memset(&modParams, 0, sizeof(modParams));
    modParams.PacketType = PACKET_TYPE_GFSK;
    modParams.Params.Gfsk.BitRate = RADIO_BIT_RATE_BPS;
    modParams.Params.Gfsk.Fdev = RADIO_FDEV_HZ;
    modParams.Params.Gfsk.ModulationShaping = MOD_SHAPING_G_BT_05;
    modParams.Params.Gfsk.Bandwidth = SUBGRF_GetFskBandwidthRegValue(RADIO_RX_BANDWIDTH_HZ);
    SUBGRF_SetModulationParams(&modParams);

    SUBGRF_SetTxParams(s_radio_pa_select, s_radio_tx_power_dbm, RADIO_RAMP_200_US);

    /* PA / RF switch already set by Satellite_GMSK_Prepare(). Do not write
     * REG_DRV_CTRL SMPS_MAX or SUBGRF_SetSwitch() here — that brown-outs 5V PA TX. */
    const bool pa5v_ready =
        (RBI_GetTxSwitchConfig() == RBI_SWITCH_RFO_LP5V) &&
        (HAL_GPIO_ReadPin(AMP_5V_EN_PORT, AMP_5V_EN_PIN) == GPIO_PIN_SET);
    if (!pa5v_ready) {
        SUBGRF_SetSwitch(s_radio_pa_select, RFSWITCH_TX);
        CPU2_Delay_Ms(20);
    }

    SUBGRF_SetBufferBaseAddress(SUBGHZ_TX_BUFFER_BASE, SUBGHZ_RX_BUFFER_BASE);
    SUBGRF_SetPayload(buffer, (uint8_t)length);

    SUBGRF_SetDioIrqParams(
        IRQ_TX_DONE | IRQ_RX_TX_TIMEOUT,
        IRQ_TX_DONE | IRQ_RX_TX_TIMEOUT,
        IRQ_RADIO_NONE,
        IRQ_RADIO_NONE);
    SUBGRF_ClearIrqStatus(IRQ_RADIO_ALL);

    uart1_puts("[GMSK-TX] Keying RF...\r\n");
    SUBGRF_SetTx(0);
    uart1_puts("[GMSK-TX] RF Keyed!\r\n");
}

/*
 * ============================================================
 * GFSK PACKET PARAMETERS
 * ============================================================
 */

void RadioApp_ForceOpenGfskParams(void)
{
    SUBGRF_SetStandby(STDBY_XOSC);
    SUBGRF_SetPacketType(PACKET_TYPE_GFSK);

    PacketParams_t packetParams;

    memset(
        &packetParams,
        0,
        sizeof(packetParams));

    packetParams.PacketType =
        PACKET_TYPE_GFSK;

    /* Uplink RX must match Ground Station TX:
     *   128-bit preamble + sync 93 0B 51 DE + 200-byte G3RUH payload.
     * SyncWordLength=0 used to start the FIFO mid-preamble (and left the
     * ST default C1 94 C1 in the chip after SetRxConfig), so no GS packet
     * ever produced a valid G3RUH frame. */
    static uint8_t s_uplink_sync[8] = RADIO_DOWNLINK_SYNC_WORD;
    SUBGRF_SetSyncWord(s_uplink_sync);

    packetParams.Params.Gfsk.PreambleLength =
        RADIO_PREAMBLE_LENGTH_BITS;

    packetParams.Params.Gfsk.PreambleMinDetect =
        RADIO_PREAMBLE_DETECTOR_16_BITS;

    packetParams.Params.Gfsk.SyncWordLength =
        RADIO_DOWNLINK_SYNC_WORD_BITS;

    packetParams.Params.Gfsk.AddrComp =
        RADIO_ADDRESSCOMP_FILT_OFF;

    /*
     * FIXED LENGTH:
     *
     * Every SX126x payload is exactly RADIO_FIXED_PACKET_LEN
     * bytes.
     */
    packetParams.Params.Gfsk.HeaderType =
        RADIO_PACKET_FIXED_LENGTH;

    packetParams.Params.Gfsk.PayloadLength =
        RADIO_FIXED_PACKET_LEN;

    /*
     * Hardware CRC OFF.
     *
     * AX.25 supplies the FCS.
     */
    packetParams.Params.Gfsk.CrcLength =
        RADIO_CRC_OFF;

    /*
     * Hardware whitening OFF.
     *
     * G3RUH is performed in software.
     */
    packetParams.Params.Gfsk.DcFree =
        RADIO_DC_FREE_OFF;

    SUBGRF_SetPacketParams(
        &packetParams);
    SUBGRF_SetSyncWord(s_uplink_sync);

    /*
     * Modulation parameters.
     */
    ModulationParams_t modParams;

    memset(
        &modParams,
        0,
        sizeof(modParams));

    modParams.PacketType =
        PACKET_TYPE_GFSK;

    modParams.Params.Gfsk.BitRate =
        RADIO_BIT_RATE_BPS;

    modParams.Params.Gfsk.Fdev =
        RADIO_FDEV_HZ;

    modParams.Params.Gfsk.ModulationShaping =
        MOD_SHAPING_G_BT_05;

    modParams.Params.Gfsk.Bandwidth =
        SUBGRF_GetFskBandwidthRegValue(
            RADIO_RX_BANDWIDTH_HZ);

    SUBGRF_SetModulationParams(
        &modParams);

    /*
     * Configure PA power and PA Ramp Time.
     */
    if (s_radio_pa_select == RFO_HP) {
        SUBGRF_SetPaConfig(0x04, 0x07, 0x00, 0x01);
        SUBGRF_SetTxParams(RFO_HP, s_radio_tx_power_dbm, RADIO_RAMP_800_US);
    } else {
        SUBGRF_SetPaConfig((RBI_GetTxSwitchConfig() == RBI_SWITCH_RFO_LP5V) ? RADIO_GMSK_5V_PA_DUTY : 0x04,
                           0x00, 0x01, 0x01);
        SUBGRF_SetTxParams(RFO_LP, s_radio_tx_power_dbm, RADIO_RAMP_800_US);
    }
    SUBGRF_WriteRegister(REG_DRV_CTRL, 0x7 << 1);
}

/*
 * ============================================================
 * RESUME RX
 * ============================================================
 */

void RadioApp_ResumeRx(void)
{
    RadioApp_StartRx();
}