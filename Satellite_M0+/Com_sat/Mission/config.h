#ifndef CONFIG_H
#define CONFIG_H

#include <stdint.h>
#include <stdbool.h>

#ifdef __cplusplus
extern "C" {
#endif

/*
 * ============================================================
 * FREQUENCIES
 * ============================================================
 *
 * Satellite:
 *
 *     TX = 435.000 MHz
 *     RX = 437.375 MHz
 *
 * Ground station must use:
 *
 *     TX = 437.375 MHz
 *     RX = 435.000 MHz
 * ============================================================
 */

#define DEFAULT_TX_FREQUENCY_HZ        435000000UL
#define DEFAULT_RX_FREQUENCY_HZ        437375000UL
#define DEFAULT_GMSK_TX_FREQ_HZ        437375000UL

/*
 * ============================================================
 * DEFAULT AX.25 ADDRESS
 * ============================================================
 */

#define DEFAULT_SOURCE_CALLSIGN       "9NS2S2"
#define DEFAULT_SOURCE_SSID            1

#define DEFAULT_DEST_CALLSIGN         "GROUND"
#define DEFAULT_DEST_SSID              0

/*
 * ============================================================
 * GFSK / G3RUH
 * ============================================================
 */
/*
 * CW Morse drive (dBm) on RFO_LP into the 3.3V PA (PC2).
 * Standard +14 dBm with paDutyCycle 0x04 produces +26 dBm at the antenna.
 * The 3.3V PA does not load the 5V DC/DC boost and operates cleanly without brownout.
 */
#define RADIO_TX_POWER_DBM             14

/*
 * GMSK ACK and data downlink drive into the 5V PA (PC3 + PA0 boost).
 * Full +14 dBm drive into the 5V External PA delivers maximum saturated downlink power.
 */
#define RADIO_GMSK_TX_POWER_DBM        14

/* 5V LP power-up: settle the 5V DC/DC (PA0) before the 5V PA (PC3) is
 * enabled, and bias the 5V PA before the RF switch goes to TX. */
#define SAT_5V_DCDC_SETTLE_MS          500
#define SAT_5V_PA_BIAS_MS              60

/*
 * SX126x RFO_LP paDutyCycle used for 5V PA GMSK drive.
 * 0x03 provides safe +14 dBm drive without the excessive peak current spikes of 0x04,
 * eliminating brownout reset on current-limited 3.3V rails.
 */
#define RADIO_GMSK_5V_PA_DUTY          0x03

/*
 * External Power Amplifier mapping (fixed by hardware, both fed from RFO_LP / PB2):
 *   CW Morse beacon      -> 3.3V External PA (PC2 / SO2), 5V DC/DC (PA0) OFF
 *   GMSK AX.25 + G3RUH   -> 5V External PA (PC3 / SI2) with 5V DC/DC Boost (PA0) ON
 * Only one PA is enabled at a time; both are OFF in RX.
 */

/*
 * GMSK / G3RUH data rate: 4800 bps (matched to Ground Station).
 */
#define RADIO_BIT_RATE_BPS             4800

/*
 * GMSK frequency deviation (+/- 1.2 kHz for 4800 bps, BT 0.5, h=0.5).
 */
#define RADIO_FDEV_HZ                  1200

/*
 * SX126x GFSK bandwidth.
 */
#define RADIO_RX_BANDWIDTH_HZ         23400

/*
 * 128 bits = 16 bytes.
 */
#define RADIO_PREAMBLE_LENGTH_BITS     128

/*
 * Downlink (satellite TX -> GS RX) hardware sync word.
 * Sent between the SX126x preamble and the G3RUH bitstream so the GS radio
 * captures every 200-byte packet bit-aligned from its first byte.
 * SDR G3RUH decoders ignore it: the leading HDLC flags re-sync the descrambler.
 * MUST be identical to Ground_Station/Mission/config.h.
 */
#define RADIO_DOWNLINK_SYNC_WORD_BITS  32
#define RADIO_DOWNLINK_SYNC_WORD       { 0x93, 0x0B, 0x51, 0xDE, 0x00, 0x00, 0x00, 0x00 }

#define RADIO_TX_TIMEOUT_MS            3000

/*
 * ============================================================
 * PHYSICAL PACKET LENGTH
 *
 * The SX126x packet is deliberately fixed length.
 *
 * This is NOT an AX.25 requirement.
 * It is our radio transport configuration.
 * ============================================================
 */

#define RADIO_FIXED_PACKET_LEN         200

/*
 * Number of preamble HDLC flags before opening frame flag.
 * 16 preamble flags (128 bits) gives PLL & G3RUH descrambler ample time to lock
 * while leaving sufficient bit budget for 144-byte camera payloads inside the fixed 200B buffer.
 */
#define PROTOCOL_LEADING_FLAG_COUNT    16

/*
 * ============================================================
 * RADIO CONFIG
 * ============================================================
 */

typedef struct
{
    uint32_t txFrequency;
    uint32_t rxFrequency;

    const char *sourceCallsign;
    uint8_t sourceSSID;

    const char *destCallsign;
    uint8_t destSSID;

    bool isSatelliteMode;

} RadioConfig_t;

/*
 * ============================================================
 * COMMANDS
 * ============================================================
 */

typedef enum
{
    CMD_NONE          = 0x00,

    CMD_REQUEST_BURST = 0x01,

    CMD_ACK           = 0x02,

    CMD_NACK          = 0x03,

    CMD_HEARTBEAT     = 0x04

} CommandOpcode_t;

#ifdef __cplusplus
}
#endif

#endif