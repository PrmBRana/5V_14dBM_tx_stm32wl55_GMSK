using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenBirdsOperation
{
    public class HKDecodedPacket
    {
        public byte[] RawBytes { get; set; }
        public string RawHex { get; set; }

        // ADC1 Voltages (V) & Temperatures (°C)
        public double BatV { get; set; }
        public double SolV { get; set; }
        public double BusV { get; set; }
        public double SP5V { get; set; }
        public double SP4V { get; set; }
        public double SP3V { get; set; }
        public double SP1V { get; set; }
        public double SP2V { get; set; }
        public double AntT { get; set; }
        public double BatT { get; set; }
        public double BPBT { get; set; }

        // Footer 1 (0x55AA or 0xAA55)
        public ushort Footer1 { get; set; }
        public bool Footer1Ok { get; set; }

        // ADC2 Currents (A) & Flags
        public double UnregI { get; set; }
        public double I3V3 { get; set; }
        public double I5V { get; set; }
        public double BatI { get; set; }
        public double SP1I { get; set; }
        public double SP2I { get; set; }
        public double SP3I { get; set; }
        public double SP4I { get; set; }
        public double SP5I { get; set; }
        public double BusI { get; set; }
        public short Flag1 { get; set; }
        public short Flag2 { get; set; }

        // IMU (Gyro: c-dps, Mag: uT)
        public short GyroX { get; set; }
        public short GyroY { get; set; }
        public short GyroZ { get; set; }
        public short MagX { get; set; }
        public short MagY { get; set; }
        public short MagZ { get; set; }

        // Footer 2 (0x66BB or 0xBB66)
        public ushort Footer2 { get; set; }
        public bool Footer2Ok { get; set; }

        // Metadata
        public uint Seq { get; set; }
        public uint TimestampMs { get; set; }
        public uint TotalSent { get; set; }

        // CRC & End Footer (0xAACC or 0xCCAA)
        public ushort CRC16 { get; set; }
        public ushort EndFooter { get; set; }
        public bool EndFooterOk { get; set; }

        public bool IsAllValid
        {
            get { return Footer1Ok && Footer2Ok && EndFooterOk; }
        }
    }

    public partial class inputDialogHK : Form
    {
        public string savingHKRootPath;
        public string HKRootPath;
        public operationMain formMain;

        public string savingHKRawRootPath;
        public string HKRawRootPath;

        public DateTime dtAppStart = DateTime.UtcNow;

        private List<HKDecodedPacket> _lastDecodedPackets = new List<HKDecodedPacket>();

        public inputDialogHK()
        {
            InitializeComponent();
        }

        public void SyncHkLines(List<string> lines)
        {
            if (lines == null || lines.Count == 0) return;
            rTBox_hk_raw_data.Text = string.Join(Environment.NewLine, lines) + Environment.NewLine;
            button_ver_hk.Enabled = true;
            button_hk_parser_ok.Enabled = true;
            UpdateHkTitleSummary();
        }

        public void AppendLiveHkLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return;
            rTBox_hk_raw_data.AppendText(line + Environment.NewLine);
            rTBox_hk_raw_data.SelectionStart = rTBox_hk_raw_data.Text.Length;
            rTBox_hk_raw_data.ScrollToCaret();
            button_ver_hk.Enabled = true;
            button_hk_parser_ok.Enabled = true;
            UpdateHkTitleSummary();
        }

        private void UpdateHkTitleSummary()
        {
            try
            {
                var pkts = ParseHKPackets(rTBox_hk_raw_data.Text);
                if (pkts.Count > 0)
                {
                    var latest = pkts[pkts.Count - 1];
                    this.Text = $"House Keeping Data — {pkts.Count} Pkt(s) | Seq #{latest.Seq} | Bat={latest.BatV:F2}V Sol={latest.SolV:F2}V Bus={latest.BusV:F2}V Temp={latest.BatT:F1}°C";
                }
            }
            catch { }
        }

        private void button_req_hk1_Click(object sender, EventArgs e)
        {
            if (formMain != null)
            {
                this.Text = "House Keeping Data — Requesting Live HK1 (1 Pkt)...";
                formMain.TransmitHkCommand(1);
            }
        }

        private void button_req_hk5_Click(object sender, EventArgs e)
        {
            if (formMain != null)
            {
                this.Text = "House Keeping Data — Requesting Stored HK (5 Pkts)...";
                formMain.TransmitHkCommand(5);
            }
        }

        private void inputDialogHK_Load(object sender, EventArgs e)
        {
            button_hk_parser_ok.Enabled = false;
            button_ver_hk.Enabled = false;
        }

        private void button_hk_cancel_Click(object sender, EventArgs e)
        {
            rTBox_hk_raw_data.Text = "";
            this.Close();
        }

        private void rTBox_hk_raw_data_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rTBox_hk_raw_data.Text))
            {
                button_ver_hk.Enabled = false;
                button_hk_parser_ok.Enabled = false;
            }
            else
            {
                button_ver_hk.Enabled = true;
                button_hk_parser_ok.Enabled = true;
            }
        }

        private void button_ver_hk_Click(object sender, EventArgs e)
        {
            try
            {
                string rawText = rTBox_hk_raw_data.Text;
                _lastDecodedPackets = ParseHKPackets(rawText);

                if (_lastDecodedPackets.Count == 0)
                {
                    MessageBox.Show(
                        "No 128-byte Slipper2Sat HK packets could be identified.\n\n" +
                        "Please verify input contains 128 bytes (256 hex characters) per packet with footers AA55/BB66.",
                        "HK Packet Verification Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    button_hk_parser_ok.Enabled = false;
                    return;
                }

                int validCount = _lastDecodedPackets.Count(p => p.IsAllValid);
                HKDecodedPacket first = _lastDecodedPackets[0];

                string summary = $"Found {_lastDecodedPackets.Count} HK packet(s) ({validCount} with matching structural footers).\n\n" +
                                 $"Sample Packet #1 (Seq #{first.Seq}):\n" +
                                 $"  Bat Voltage: {first.BatV:F2} V | Sol Voltage: {first.SolV:F2} V | Bus: {first.BusV:F2} V\n" +
                                 $"  Antenna Temp: {first.AntT:F1} °C | Bat Temp: {first.BatT:F1} °C | BPB: {first.BPBT:F1} °C\n" +
                                 $"  Currents: 3V3={first.I3V3:F2} A, 5V={first.I5V:F2} A, Bat={first.BatI:F2} A\n" +
                                 $"  IMU Gyro: [{first.GyroX}, {first.GyroY}, {first.GyroZ}] c-dps\n" +
                                 $"  IMU Mag : [{first.MagX}, {first.MagY}, {first.MagZ}] uT\n" +
                                 $"  Footers: F1={first.Footer1:X4} ({(first.Footer1Ok ? "OK" : "MISMATCH")}), " +
                                 $"F2={first.Footer2:X4} ({(first.Footer2Ok ? "OK" : "MISMATCH")}), " +
                                 $"End={first.EndFooter:X4} ({(first.EndFooterOk ? "OK" : "MISMATCH")})\n\n" +
                                 "Click 'Save HK' to export full decoded telemetry table to Excel.";

                MessageBox.Show(
                    summary,
                    "HK Verification Result",
                    MessageBoxButtons.OK,
                    validCount > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning
                );

                button_hk_parser_ok.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error verifying HK data:\n{ex.Message}",
                    "Verification Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void button_hk_parser_ok_Click(object sender, EventArgs e)
        {
            try
            {
                string rawText = rTBox_hk_raw_data.Text;
                _lastDecodedPackets = ParseHKPackets(rawText);

                if (_lastDecodedPackets.Count == 0)
                {
                    MessageBox.Show(
                        "No valid HK packets found to save. Please paste raw hex telemetry (128 bytes).",
                        "Save Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                savingHKRootPath = "../Storage/HK_DETAILS";
                if (!Directory.Exists(savingHKRootPath))
                {
                    Directory.CreateDirectory(savingHKRootPath);
                }

                DateTime dt = DateTime.UtcNow;
                string time = dt.ToString("yyyyMMdd_HHmmss");
                string fileName = time + "_HK_Data.xlsx";
                string absolutePath = Path.Combine(savingHKRootPath, fileName);

                saveToExcel(_lastDecodedPackets, absolutePath, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"HK Operation Failed:\n{ex.Message}",
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        public static List<HKDecodedPacket> ParseHKPackets(string text)
        {
            var packets = new List<HKDecodedPacket>();
            if (string.IsNullOrWhiteSpace(text)) return packets;

            // Check if JSON containing hex was pasted
            var jsonHexMatches = Regex.Matches(text, "\"hex\"\\s*:\\s*\"([0-9a-fA-F]+)\"");
            if (jsonHexMatches.Count > 0)
            {
                foreach (Match m in jsonHexMatches)
                {
                    string hx = m.Groups[1].Value;
                    if (hx.Length >= 256)
                    {
                        var pkt = DecodeSingleHKPayload(StringToByteArray(hx.Substring(0, 256)));
                        if (pkt != null) packets.Add(pkt);
                    }
                }
                if (packets.Count > 0) return packets;
            }

            // Extract all hex bytes from the text (both 2-char space-separated tokens and continuous hex strings)
            var hexTokens = Regex.Matches(text, @"\b[0-9a-fA-F]{2,}\b");
            var byteList = new List<byte>();
            foreach (Match m in hexTokens)
            {
                string val = m.Value;
                if (val.Length == 2)
                {
                    byteList.Add(Convert.ToByte(val, 16));
                }
                else if (val.Length >= 16 && val.Length % 2 == 0)
                {
                    for (int k = 0; k < val.Length; k += 2)
                    {
                        byteList.Add(Convert.ToByte(val.Substring(k, 2), 16));
                    }
                }
            }

            byte[] allBytes = byteList.ToArray();

            // Try to align on footers 0x55 0xAA (or 0xAA 0x55) at offset 32..33
            int idx = 0;
            while (idx <= allBytes.Length - 128)
            {
                // Check if this window looks like an HK packet
                bool f1Ok = (allBytes[idx + 32] == 0x55 && allBytes[idx + 33] == 0xAA) ||
                            (allBytes[idx + 32] == 0xAA && allBytes[idx + 33] == 0x55);
                bool f2Ok = (allBytes[idx + 70] == 0x66 && allBytes[idx + 71] == 0xBB) ||
                            (allBytes[idx + 70] == 0xBB && allBytes[idx + 71] == 0x66);

                if (f1Ok || f2Ok || (allBytes.Length - idx == 128))
                {
                    byte[] slice = new byte[128];
                    Array.Copy(allBytes, idx, slice, 0, 128);
                    var pkt = DecodeSingleHKPayload(slice);
                    if (pkt != null)
                    {
                        packets.Add(pkt);
                        idx += 128;
                        continue;
                    }
                }
                idx++;
            }

            // Fallback: If no packets found via footer scan, but we have multiple of 128 bytes
            if (packets.Count == 0 && allBytes.Length >= 128)
            {
                for (int i = 0; i + 128 <= allBytes.Length; i += 128)
                {
                    byte[] slice = new byte[128];
                    Array.Copy(allBytes, i, slice, 0, 128);
                    var pkt = DecodeSingleHKPayload(slice);
                    if (pkt != null) packets.Add(pkt);
                }
            }

            return packets;
        }

        public static HKDecodedPacket DecodeSingleHKPayload(byte[] raw)
        {
            if (raw == null || raw.Length < 128) return null;

            var pkt = new HKDecodedPacket();
            pkt.RawBytes = raw;
            pkt.RawHex = BitConverter.ToString(raw).Replace("-", " ");

            // ADC1 (16 x int16 little-endian, bytes 0..31)
            short[] adc1 = new short[16];
            for (int i = 0; i < 16; i++)
            {
                adc1[i] = BitConverter.ToInt16(raw, i * 2);
            }
            pkt.BatV = adc1[0] / 100.0;
            pkt.SolV = adc1[1] / 100.0;
            pkt.BusV = adc1[2] / 100.0;
            pkt.SP5V = adc1[3] / 100.0;
            pkt.SP4V = adc1[4] / 100.0;
            pkt.SP3V = adc1[5] / 100.0;
            pkt.SP1V = adc1[6] / 100.0;
            pkt.SP2V = adc1[7] / 100.0;
            pkt.AntT = adc1[8] / 10.0;
            pkt.BatT = adc1[9] / 10.0;
            pkt.BPBT = adc1[10] / 10.0;

            // Footer 1 at bytes 32..33 (0x55AA or 0xAA55)
            pkt.Footer1 = BitConverter.ToUInt16(raw, 32);
            pkt.Footer1Ok = (pkt.Footer1 == 0xAA55 || pkt.Footer1 == 0x55AA ||
                             (raw[32] == 0x55 && raw[33] == 0xAA) ||
                             (raw[32] == 0xAA && raw[33] == 0x55));

            // ADC2 (12 x int16 little-endian, bytes 34..57)
            short[] adc2 = new short[12];
            for (int i = 0; i < 12; i++)
            {
                adc2[i] = BitConverter.ToInt16(raw, 34 + i * 2);
            }
            pkt.UnregI = adc2[0] / 100.0;
            pkt.I3V3 = adc2[1] / 100.0;
            pkt.I5V = adc2[2] / 100.0;
            pkt.BatI = adc2[3] / 100.0;
            pkt.SP1I = adc2[4] / 100.0;
            pkt.SP2I = adc2[5] / 100.0;
            pkt.SP3I = adc2[6] / 100.0;
            pkt.SP4I = adc2[7] / 100.0;
            pkt.SP5I = adc2[8] / 100.0;
            pkt.BusI = adc2[9] / 100.0;
            pkt.Flag1 = adc2[10];
            pkt.Flag2 = adc2[11];

            // IMU (6 x int16 little-endian, bytes 58..69)
            pkt.GyroX = BitConverter.ToInt16(raw, 58);
            pkt.GyroY = BitConverter.ToInt16(raw, 60);
            pkt.GyroZ = BitConverter.ToInt16(raw, 62);
            pkt.MagX = BitConverter.ToInt16(raw, 64);
            pkt.MagY = BitConverter.ToInt16(raw, 66);
            pkt.MagZ = BitConverter.ToInt16(raw, 68);

            // Footer 2 at bytes 70..71 (0x66BB or 0xBB66)
            pkt.Footer2 = BitConverter.ToUInt16(raw, 70);
            pkt.Footer2Ok = (pkt.Footer2 == 0xBB66 || pkt.Footer2 == 0x66BB ||
                             (raw[70] == 0x66 && raw[71] == 0xBB) ||
                             (raw[70] == 0xBB && raw[71] == 0x66));

            // Metadata at bytes 72..83
            pkt.Seq = BitConverter.ToUInt32(raw, 72);
            pkt.TimestampMs = BitConverter.ToUInt32(raw, 76);
            pkt.TotalSent = BitConverter.ToUInt32(raw, 80);

            // CRC16 & End Footer at bytes 124..127
            pkt.CRC16 = BitConverter.ToUInt16(raw, 124);
            pkt.EndFooter = BitConverter.ToUInt16(raw, 126);
            pkt.EndFooterOk = (pkt.EndFooter == 0xAACC || pkt.EndFooter == 0xCCAA ||
                               (raw[126] == 0xAA && raw[127] == 0xCC) ||
                               (raw[126] == 0xCC && raw[127] == 0xAA));

            return pkt;
        }

        private static byte[] StringToByteArray(string hex)
        {
            hex = hex.Replace(" ", "").Replace("\r", "").Replace("\n", "");
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        static void saveToExcel(List<HKDecodedPacket> packets, string absolutePath, Form form)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(absolutePath)))
            {
                var ws = package.Workbook.Worksheets.Add("Extracted_Data");
                var wsRaw = package.Workbook.Worksheets.Add("HK_Backend");

                // Headers
                string[] headers = new string[] {
                    "SN", "Timestamp UTC", "Seq #", "Pkt Time (ms)", "Total Sent",
                    "Bat Voltage (V)", "Solar Voltage (V)", "Bus Voltage (V)",
                    "SP5 (V)", "SP4 (V)", "SP3 (V)", "SP1 (V)", "SP2 (V)",
                    "Antenna Temp (°C)", "Battery Temp (°C)", "BPB Temp (°C)",
                    "Unreg Current (A)", "3V3 Current (A)", "5V Current (A)", "Battery Current (A)",
                    "SP1 Current (A)", "Bus Current (A)", "Flag1", "Flag2",
                    "Gyro X (c-dps)", "Gyro Y (c-dps)", "Gyro Z (c-dps)",
                    "Mag X (uT)", "Mag Y (uT)", "Mag Z (uT)",
                    "Footer1 (AA55)", "Footer2 (BB66)", "EndFooter (AACC)", "CRC-16",
                    "Raw Hex"
                };

                for (int col = 0; col < headers.Length; col++)
                {
                    ws.Cells[1, col + 1].Value = headers[col];
                    ws.Cells[1, col + 1].Style.Font.Bold = true;
                    ws.Cells[1, col + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[1, col + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSteelBlue);
                    ws.Cells[1, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Raw sheet headers
                wsRaw.Cells[1, 1].Value = "SN";
                wsRaw.Cells[1, 2].Value = "Seq #";
                wsRaw.Cells[1, 3].Value = "Raw Data (128 Bytes)";
                wsRaw.Row(1).Style.Font.Bold = true;

                int row = 2;
                foreach (var p in packets)
                {
                    ws.Cells[row, 1].Value = row - 1;
                    ws.Cells[row, 2].Value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                    ws.Cells[row, 3].Value = p.Seq;
                    ws.Cells[row, 4].Value = p.TimestampMs;
                    ws.Cells[row, 5].Value = p.TotalSent;

                    // Voltages
                    ws.Cells[row, 6].Value = p.BatV;
                    ws.Cells[row, 7].Value = p.SolV;
                    ws.Cells[row, 8].Value = p.BusV;
                    ws.Cells[row, 9].Value = p.SP5V;
                    ws.Cells[row, 10].Value = p.SP4V;
                    ws.Cells[row, 11].Value = p.SP3V;
                    ws.Cells[row, 12].Value = p.SP1V;
                    ws.Cells[row, 13].Value = p.SP2V;

                    // Temps
                    ws.Cells[row, 14].Value = p.AntT;
                    ws.Cells[row, 15].Value = p.BatT;
                    ws.Cells[row, 16].Value = p.BPBT;

                    // Currents
                    ws.Cells[row, 17].Value = p.UnregI;
                    ws.Cells[row, 18].Value = p.I3V3;
                    ws.Cells[row, 19].Value = p.I5V;
                    ws.Cells[row, 20].Value = p.BatI;
                    ws.Cells[row, 21].Value = p.SP1I;
                    ws.Cells[row, 22].Value = p.BusI;

                    // Flags
                    ws.Cells[row, 23].Value = p.Flag1;
                    ws.Cells[row, 24].Value = p.Flag2;

                    // IMU
                    ws.Cells[row, 25].Value = p.GyroX;
                    ws.Cells[row, 26].Value = p.GyroY;
                    ws.Cells[row, 27].Value = p.GyroZ;
                    ws.Cells[row, 28].Value = p.MagX;
                    ws.Cells[row, 29].Value = p.MagY;
                    ws.Cells[row, 30].Value = p.MagZ;

                    // Footers
                    ws.Cells[row, 31].Value = p.Footer1Ok ? "OK (0xAA55)" : $"MISMATCH (0x{p.Footer1:X4})";
                    ws.Cells[row, 32].Value = p.Footer2Ok ? "OK (0xBB66)" : $"MISMATCH (0x{p.Footer2:X4})";
                    ws.Cells[row, 33].Value = p.EndFooterOk ? "OK (0xAACC)" : $"MISMATCH (0x{p.EndFooter:X4})";
                    ws.Cells[row, 34].Value = $"0x{p.CRC16:X4}";

                    // Raw
                    ws.Cells[row, 35].Value = p.RawHex;

                    // Raw Sheet
                    wsRaw.Cells[row, 1].Value = row - 1;
                    wsRaw.Cells[row, 2].Value = p.Seq;
                    wsRaw.Cells[row, 3].Value = p.RawHex;

                    row++;
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                wsRaw.Cells[wsRaw.Dimension.Address].AutoFitColumns();
                insert_border_to_excel(ws);
                insert_border_to_excel(wsRaw);

                package.Save();

                DialogResult dr = MessageBox.Show(
                    $"Housekeeping Telemetry saved successfully to:\n{absolutePath}\n\nClick 'OK' to open Excel preview.",
                    "Save Complete",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information
                );

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = absolutePath,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Could not open file automatically: {ex.Message}");
                    }
                }
            }
        }

        static void insert_border_to_excel(ExcelWorksheet worksheet_all)
        {
            if (worksheet_all.Dimension == null) return;
            int lastRow = worksheet_all.Dimension.End.Row;
            int lastColumn = worksheet_all.Dimension.End.Column;

            var range = worksheet_all.Cells[1, 1, lastRow, lastColumn];
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            rTBox_hk_raw_data.Text = "";
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text / Log / Excel Files|*.txt;*.log;*.csv;*.xlsx;*.xlsm|All Files|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = ofd.FileName;
                    if (filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                        filePath.EndsWith(".xlsm", StringComparison.OrdinalIgnoreCase))
                    {
                        ReadExcelFile(filePath);
                    }
                    else
                    {
                        try
                        {
                            rTBox_hk_raw_data.Text = File.ReadAllText(filePath);
                            button_hk_parser_ok.Enabled = true;
                            button_ver_hk.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error reading text file: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void ReadExcelFile(string filePath)
        {
            if (!File.Exists(filePath)) return;
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    if (worksheet.Dimension == null) return;
                    int totalRows = worksheet.Dimension.End.Row;

                    // Look for Raw Hex column or column 35 / column 4
                    int hexCol = 35;
                    for (int c = 1; c <= worksheet.Dimension.End.Column; c++)
                    {
                        string header = worksheet.Cells[1, c].Text;
                        if (header.IndexOf("Raw", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            header.IndexOf("Hex", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            hexCol = c;
                            break;
                        }
                    }

                    for (int r = 2; r <= totalRows; r++)
                    {
                        string val = worksheet.Cells[r, hexCol].Text;
                        if (!string.IsNullOrWhiteSpace(val))
                        {
                            rTBox_hk_raw_data.AppendText(val + Environment.NewLine);
                        }
                    }
                    button_hk_parser_ok.Enabled = true;
                    button_ver_hk.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading Excel file: {ex.Message}", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
