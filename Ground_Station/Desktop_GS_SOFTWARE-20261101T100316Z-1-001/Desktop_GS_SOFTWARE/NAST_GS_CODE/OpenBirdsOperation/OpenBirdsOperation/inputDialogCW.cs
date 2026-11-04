using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenBirdsOperation
{
    public partial class inputDialogCW : Form
    {
        private string cwAccum = "";

        public event Action<string, Color, string, string, string> OnCwPanelUpdated;

        private static readonly Dictionary<string, char> MorseToChar = new Dictionary<string, char>
        {
            { ".-", 'A' }, { "-...", 'B' }, { "-.-.", 'C' }, { "-..", 'D' },
            { ".", 'E' }, { "..-.", 'F' }, { "--.", 'G' }, { "....", 'H' },
            { "..", 'I' }, { ".---", 'J' }, { "-.-", 'K' }, { ".-..", 'L' },
            { "--", 'M' }, { "-.", 'N' }, { "---", 'O' }, { ".--.", 'P' },
            { "--.-", 'Q' }, { ".-.", 'R' }, { "...", 'S' }, { "-", 'T' },
            { "..-", 'U' }, { "...-", 'V' }, { ".--", 'W' }, { "-..-", 'X' },
            { "-.--", 'Y' }, { "--..", 'Z' },
            { "-----", '0' }, { ".----", '1' }, { "..---", '2' }, { "...--", '3' },
            { "....-", '4' }, { ".....", '5' }, { "-....", '6' }, { "--...", '7' },
            { "---..", '8' }, { "----.", '9' },
            { ".-.-.-", '.' }, { "--..--", ',' }, { "..--..", '?' }, { "-....-", '-' },
            { "-..-.", '/' }, { "---...", ':' }, { "-.-.-.", ';' }, { "-...-", '=' }
        };

        public inputDialogCW()
        {
            InitializeComponent();
        }

        private void inputDialogCW_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lblState.Text))
            {
                SetState("IDLE", Color.FromArgb(51, 65, 85));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cwAccum = "";
            rTBoxCwLog.Clear();
            lblLiveText.Text = "(waiting for Morse beacon)";
            lblLastText.Text = "—";
            lblRssi.Text = "RSSI: -- dBm";
            SetState("IDLE", Color.FromArgb(51, 65, 85));
            ResetTelemetryCards();
            NotifyMainBanner();
        }

        public void SetState(string stateText, Color bgColor)
        {
            lblState.Text = stateText;
            lblState.BackColor = bgColor;
            lblState.ForeColor = Color.White;
            NotifyMainBanner();
        }

        public void SetRssi(int rssi)
        {
            lblRssi.Text = $"RSSI: {rssi} dBm";
            NotifyMainBanner();
        }

        private void NotifyMainBanner()
        {
            OnCwPanelUpdated?.Invoke(lblState.Text, lblState.BackColor, lblRssi.Text, lblLiveText.Text, lblLastText.Text);
        }

        public void SyncStateFromMain(string stateText, Color stateColor, string rssiText, string liveText, string lastText, IEnumerable<string> historyLines)
        {
            lblState.Text = stateText;
            lblState.BackColor = stateColor;
            lblState.ForeColor = Color.White;
            lblRssi.Text = rssiText;
            lblLiveText.Text = liveText;
            lblLastText.Text = lastText;

            if (historyLines != null && rTBoxCwLog.TextLength == 0)
            {
                foreach (var hLine in historyLines)
                {
                    FeedLine(hLine);
                }
            }
        }

        public void ResetTelemetryCards()
        {
            txtBatV.Text = "-- V";
            txtSolV.Text = "-- V";
            txtAntT.Text = "-- °C";
            txtBatT.Text = "-- °C";
            txtBpbt.Text = "-- °C";

            txtI3v3.Text = "-- A";
            txtSolI.Text = "-- A";
            txtBatI.Text = "-- A";
            txtFlag1.Text = "--";
            txtFlag2.Text = "--";
        }

        public void FeedLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return;

            string[] subLines = line.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sub in subLines)
            {
                if (string.IsNullOrWhiteSpace(sub)) continue;
                rTBoxCwLog.AppendText(sub + Environment.NewLine);
                rTBoxCwLog.SelectionStart = rTBoxCwLog.Text.Length;
                rTBoxCwLog.ScrollToCaret();

                ProcessSingleLine(sub);
            }
        }

        private static string Tail120(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length <= 120 ? s : s.Substring(s.Length - 120);
        }

        private void HandleDecodedOrPartialCwText(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt)) return;
            txt = txt.Trim();

            if (IsCwBeaconComplete(txt))
            {
                cwAccum = txt;
                lblLiveText.Text = txt;
                lblLastText.Text = txt;
                SetState("DECODED", Color.FromArgb(21, 128, 61));
                ExtractAndDisplayTelemetry(txt, false);
            }
            else
            {
                if (txt.Length > cwAccum.Length)
                {
                    cwAccum = txt;
                }
                string liveTxt = !string.IsNullOrEmpty(cwAccum) ? cwAccum : txt;
                lblLiveText.Text = Tail120(liveTxt);

                string curLast = lblLastText.Text;
                if (curLast == "—" || string.IsNullOrEmpty(curLast) || curLast == "(waiting for Morse beacon)" || curLast == "(waiting for Morse)")
                {
                    lblLastText.Text = txt;
                }
                SetState("RECEIVING", Color.FromArgb(180, 83, 9));
                ExtractAndDisplayTelemetry(txt, false);
            }
            NotifyMainBanner();
        }

        private void ProcessSingleLine(string line)
        {
            line = line.Trim();

            // 1. JSON Telemetry (matching gs_communicator.py: CWRX, CWSIG, CWCAR, CWCHAR)
            if (line.IndexOf('{') >= 0 && line.EndsWith("}"))
            {
                if (line.Contains("\"CWRX\""))
                {
                    Match m = Regex.Match(line, "\"text\"\\s*:\\s*\"([^\"]*)\"");
                    if (m.Success)
                    {
                        HandleDecodedOrPartialCwText(m.Groups[1].Value);
                    }
                    return;
                }
                if (line.Contains("\"CWSIG\""))
                {
                    Match mOn = Regex.Match(line, "\"on\"\\s*:\\s*(\\d+)");
                    Match mRssi = Regex.Match(line, "\"rssi\"\\s*:\\s*(-?\\d+)");
                    bool on = mOn.Success && mOn.Groups[1].Value != "0";
                    if (mRssi.Success) SetRssi(int.Parse(mRssi.Groups[1].Value));
                    SetState(on ? "CARRIER" : "IDLE", on ? Color.FromArgb(21, 128, 61) : Color.FromArgb(51, 65, 85));
                    return;
                }
                if (line.Contains("\"CWCAR\""))
                {
                    Match mRssi = Regex.Match(line, "\"rssi\"\\s*:\\s*(-?\\d+)");
                    if (IsCwBeaconComplete(cwAccum))
                    {
                        lblLastText.Text = cwAccum;
                    }
                    if (mRssi.Success) SetRssi(int.Parse(mRssi.Groups[1].Value));
                    SetState("CARRIER", Color.FromArgb(21, 128, 61));
                    return;
                }
                if (line.Contains("\"CWCHAR\""))
                {
                    Match mCh = Regex.Match(line, "\"ch\"\\s*:\\s*\"([^\"]*)\"");
                    if (mCh.Success)
                    {
                        string ch = mCh.Groups[1].Value;
                        string cur = cwAccum ?? "";
                        if (IsCwBeaconComplete(cur))
                        {
                            lblLastText.Text = cur;
                            cur = "";
                        }
                        string curLive = lblLiveText.Text ?? "";
                        if (curLive.StartsWith("[") || curLive.StartsWith("(") || curLive.IndexOf("waiting", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            if (string.IsNullOrEmpty(cwAccum)) cur = "";
                        }
                        if (ch == " " && (string.IsNullOrEmpty(cur) || cur.EndsWith(" ")))
                        {
                            // skip duplicate space
                        }
                        else
                        {
                            cur += ch;
                        }
                        cwAccum = cur;
                        lblLiveText.Text = Tail120(cur);
                        SetState("KEYING", Color.FromArgb(180, 83, 9));
                        if (IsCwBeaconComplete(cur))
                        {
                            lblLastText.Text = cur;
                            ExtractAndDisplayTelemetry(cur, false);
                        }
                        NotifyMainBanner();
                    }
                    return;
                }
            }

            // 2. Structured log formats (matching gs_communicator.py)
            if (line.Contains("[CW DECODED]"))
            {
                string txt = line.Substring(line.IndexOf("[CW DECODED]") + "[CW DECODED]".Length).Trim();
                HandleDecodedOrPartialCwText(txt);
                return;
            }

            if (line.Contains("[CW] Carrier detected"))
            {
                Match m = Regex.Match(line, @"RSSI\s+(-?\d+)");
                if (m.Success) SetRssi(int.Parse(m.Groups[1].Value));
                SetState("CARRIER", Color.FromArgb(21, 128, 61));
                return;
            }

            if (line.Contains("[CW] Tuning carrier"))
            {
                Match m = Regex.Match(line, @"RSSI\s+(-?\d+)");
                if (IsCwBeaconComplete(cwAccum))
                {
                    lblLastText.Text = cwAccum;
                }
                if (m.Success) SetRssi(int.Parse(m.Groups[1].Value));
                SetState("CARRIER", Color.FromArgb(21, 128, 61));
                return;
            }

            if (line.Contains("[CW TLM-1]"))
            {
                Match mBatv = Regex.Match(line, @"BatV=([\d\.\-]+)V");
                Match mSolv = Regex.Match(line, @"SolV=([\d\.\-]+)V");
                Match mAntt = Regex.Match(line, @"AntT=([\d\.\-]+)C");
                Match mBatt = Regex.Match(line, @"BatT=([\d\.\-]+)C");
                Match mBpbt = Regex.Match(line, @"BPBT=([\d\.\-]+)C");

                if (mBatv.Success) txtBatV.Text = mBatv.Groups[1].Value + " V";
                if (mSolv.Success) txtSolV.Text = mSolv.Groups[1].Value + " V";
                if (mAntt.Success) txtAntT.Text = mAntt.Groups[1].Value + " °C";
                if (mBatt.Success) txtBatT.Text = mBatt.Groups[1].Value + " °C";
                if (mBpbt.Success) txtBpbt.Text = mBpbt.Groups[1].Value + " °C";

                SetState("DECODED", Color.FromArgb(21, 128, 61));
                return;
            }

            if (line.Contains("[CW TLM-2]"))
            {
                Match m3v3 = Regex.Match(line, @"3V3I=([\d\.\-]+)A");
                Match mSoli = Regex.Match(line, @"SolI=([\d\.\-]+)A");
                Match mBati = Regex.Match(line, @"BatI=([\d\.\-]+)A");
                Match mF1 = Regex.Match(line, @"F1=([\d\-]+)");
                Match mF2 = Regex.Match(line, @"F2=([\d\-]+)");

                if (m3v3.Success) txtI3v3.Text = m3v3.Groups[1].Value + " A";
                if (mSoli.Success) txtSolI.Text = mSoli.Groups[1].Value + " A";
                if (mBati.Success) txtBatI.Text = mBati.Groups[1].Value + " A";
                if (mF1.Success) txtFlag1.Text = mF1.Groups[1].Value;
                if (mF2.Success) txtFlag2.Text = mF2.Groups[1].Value;

                SetState("DECODED", Color.FromArgb(21, 128, 61));
                return;
            }

            // 3. Raw Morse text beacon or dots and dashes
            ExtractAndDisplayTelemetry(line, true);
        }

        public static bool IsCwBeaconComplete(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt)) return false;
            string clean = txt.Trim();
            string[] tokens = clean.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            int vCount = 0, cCount = 0, aCount = 0;
            foreach (var tok in tokens)
            {
                if (tok.IndexOf('V') >= 0 || tok.IndexOf('v') >= 0) vCount++;
                if (tok.IndexOf('C') >= 0 || tok.IndexOf('c') >= 0) cCount++;
                if (tok.IndexOf('A') >= 0 || tok.IndexOf('a') >= 0) aCount++;
            }

            // CW1: 2 voltages + 3 temperatures (e.g. 9NS2S2NEPAL 3.85V 4.20V 22.5C 24.0C 23.1C)
            if (vCount >= 2 && cCount >= 3) return true;
            // CW2: 3 currents + status (e.g. 9NS2S2NEPAL 0.15A 0.35A 0.20A 1 0)
            if (aCount >= 3 && tokens.Length >= 5) return true;

            if (tokens.Length >= 6 && (clean.Contains("9NS2S2") || clean.Contains("NEPAL") || clean.Contains("S2S"))) return true;
            return false;
        }

        public void ExtractAndDisplayTelemetry(string text, bool updateBannerState = true)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            // Check if text is dots and dashes (e.g. "... --- ...")
            if (Regex.IsMatch(text, @"^[.\-\s/]+$") && text.Contains(".") && text.Contains("-"))
            {
                string converted = DecodeMorseStringToAscii(text);
                if (!string.IsNullOrWhiteSpace(converted))
                {
                    rTBoxCwLog.AppendText($"[MORSE DECODED] {converted}" + Environment.NewLine);
                    text = converted;
                }
            }

            string[] tokens = text.Split(new char[] { ' ', '\t', ',', '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return;

            var voltages = new List<string>();
            var temps = new List<string>();
            var currents = new List<string>();
            var numbers = new List<string>();

            foreach (var tok in tokens)
            {
                Match mv = Regex.Match(tok, @"^([\d\.\-]+)[Vv]$");
                if (mv.Success) { voltages.Add(mv.Groups[1].Value); continue; }

                Match mt = Regex.Match(tok, @"^([\d\.\-]+)[Cc]$");
                if (mt.Success) { temps.Add(mt.Groups[1].Value); continue; }

                Match ma = Regex.Match(tok, @"^([\d\.\-]+)[Aa]$");
                if (ma.Success) { currents.Add(ma.Groups[1].Value); continue; }

                double dummy;
                if (double.TryParse(tok, out dummy))
                {
                    numbers.Add(tok);
                }
            }

            // CW-1: Voltages and Temps
            if (voltages.Count >= 1) txtBatV.Text = voltages[0] + " V";
            if (voltages.Count >= 2) txtSolV.Text = voltages[1] + " V";
            if (voltages.Count >= 3) txtSolV.Text += $" (V3={voltages[2]})";

            if (temps.Count >= 1) txtAntT.Text = temps[0] + " °C";
            if (temps.Count >= 2) txtBatT.Text = temps[1] + " °C";
            if (temps.Count >= 3) txtBpbt.Text = temps[2] + " °C";

            // CW-2: Currents
            if (currents.Count >= 1) txtI3v3.Text = currents[0] + " A";
            if (currents.Count >= 2) txtSolI.Text = currents[1] + " A";
            if (currents.Count >= 3) txtBatI.Text = currents[2] + " A";

            // Status numbers (flags)
            if (numbers.Count >= 1 && currents.Count >= 2) txtFlag1.Text = numbers[0];
            if (numbers.Count >= 2 && currents.Count >= 2) txtFlag2.Text = numbers[1];

            if (updateBannerState && (voltages.Count > 0 || temps.Count > 0 || currents.Count > 0))
            {
                cwAccum = text.Trim();
                lblLiveText.Text = text.Trim();
                lblLastText.Text = text.Trim();
                SetState("DECODED", Color.FromArgb(21, 128, 61));
            }
        }

        public static string DecodeMorseStringToAscii(string morse)
        {
            var sb = new StringBuilder();
            string[] words = morse.Split(new string[] { "  ", " / " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                string[] letters = word.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var letter in letters)
                {
                    if (MorseToChar.ContainsKey(letter))
                    {
                        sb.Append(MorseToChar[letter]);
                    }
                    else
                    {
                        sb.Append('?');
                    }
                }
                sb.Append(' ');
            }
            return sb.ToString().Trim();
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            string content = rTBoxCwLog.Text;
            if (string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("Please paste CW serial logs, Morse dots/dashes, or beacon text into the log window.",
                                "Input Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string[] lines = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int decodedLines = 0;
            foreach (var line in lines)
            {
                ProcessSingleLine(line);
                decodedLines++;
            }

            MessageBox.Show($"Processed {decodedLines} line(s).\nTelemetry cards updated.",
                            "CW Decode Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string storageDir = "../Storage/CW_DETAILS";
                if (!Directory.Exists(storageDir)) Directory.CreateDirectory(storageDir);

                string filePath = Path.Combine(storageDir, $"CW_Decoded_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt");
                var sb = new StringBuilder();
                sb.AppendLine("=== CW DECODED TELEMETRY LOG (Slipper2Sat / 9NS2S2) ===");
                sb.AppendLine($"Timestamp UTC: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Last Beacon  : {lblLastText.Text}");
                sb.AppendLine($"RSSI         : {lblRssi.Text}");
                sb.AppendLine();
                sb.AppendLine("--- CW-1 Telemetry ---");
                sb.AppendLine($"Bat Voltage : {txtBatV.Text}");
                sb.AppendLine($"Sol Voltage : {txtSolV.Text}");
                sb.AppendLine($"Antenna Temp: {txtAntT.Text}");
                sb.AppendLine($"Battery Temp: {txtBatT.Text}");
                sb.AppendLine($"BPB Temp    : {txtBpbt.Text}");
                sb.AppendLine();
                sb.AppendLine("--- CW-2 Telemetry ---");
                sb.AppendLine($"3V3 Current : {txtI3v3.Text}");
                sb.AppendLine($"Sol Current : {txtSolI.Text}");
                sb.AppendLine($"Bat Current : {txtBatI.Text}");
                sb.AppendLine($"Flag1       : {txtFlag1.Text}");
                sb.AppendLine($"Flag2       : {txtFlag2.Text}");
                sb.AppendLine();
                sb.AppendLine("--- Raw Log Content ---");
                sb.AppendLine(rTBoxCwLog.Text);

                File.WriteAllText(filePath, sb.ToString());

                DialogResult dr = MessageBox.Show($"CW Telemetry exported successfully to:\n{filePath}\n\nOpen file?",
                                                  "Export Successful", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo { FileName = filePath, UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
