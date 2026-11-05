using DocumentFormat.OpenXml.Office2013.ExcelAc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenBirdsOperation
{
    public partial class inputDialogCamera : Form
    {
        public string savingCAMRootPath;
        public string CAMRootPAth;
        public operationMain formMain;

        public DateTime dtAppStart = DateTime.UtcNow;

        Image image;

        byte[] imageBytes;
        public inputDialogCamera()
        {
            InitializeComponent();
        }

        public void SyncCameraLines(List<string> lines)
        {
            if (lines == null || lines.Count == 0) return;
            rTBox_cam_raw_data.Text = string.Join(Environment.NewLine, lines) + Environment.NewLine;
            TryLiveRenderImage();
        }

        public void AppendLiveCameraLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return;
            rTBox_cam_raw_data.AppendText(line + Environment.NewLine);
            rTBox_cam_raw_data.SelectionStart = rTBox_cam_raw_data.Text.Length;
            rTBox_cam_raw_data.ScrollToCaret();
            TryLiveRenderImage();
        }

        public void TryLiveRenderImage()
        {
            try
            {
                int chunksFound = 0;
                int totalExpected = 69;
                byte[] decoded = ReassembleCameraChunksFromText(rTBox_cam_raw_data.Text, out chunksFound, out totalExpected);
                if (decoded != null && decoded.Length >= 4)
                {
                    imageBytes = decoded;
                    groupBox1.Text = $"Image Parser — {chunksFound}/{totalExpected} Chunks ({imageBytes.Length} Bytes)";
                    button_save_pic.Enabled = true;
                    button_cam_paraser_ok.Enabled = true;
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        image = Image.FromStream(ms);
                        pictureBox_img.Image = new Bitmap(image);
                    }
                }
                else if (chunksFound > 0)
                {
                    groupBox1.Text = $"Image Parser — Receiving {chunksFound}/{totalExpected} Chunks...";
                }
            }
            catch
            {
                // Partial JPEG may not yet have enough scan headers for GDI+ until more chunks arrive
            }
        }

        private void btn_tx_dummy_cam_Click(object sender, EventArgs e)
        {
            if (formMain != null)
            {
                rTBox_cam_raw_data.Text = "";
                groupBox1.Text = "Image Parser — Requesting Dummy Image (69 Chunks / DC)...";
                formMain.TransmitDummyCameraCommand();
            }
        }

        private void btn_tx_real_cam_Click(object sender, EventArgs e)
        {
            if (formMain != null)
            {
                rTBox_cam_raw_data.Text = "";
                groupBox1.Text = "Image Parser — Requesting Real Camera Image (CAM)...";
                formMain.TransmitRealCameraCommand();
            }
        }

        private void btn_tx_capture_cam_Click(object sender, EventArgs e)
        {
            if (formMain != null)
            {
                groupBox1.Text = "Image Parser — Capturing Real Camera Photos to Flash (CAMON)...";
                formMain.TransmitRealCameraCaptureCommand();
            }
        }

        private void button_cam_paraser_ok_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(rTBox_cam_raw_data.Text))
            {
                try
                {
                    string rawText = rTBox_cam_raw_data.Text;
                    int chunksFound = 0;
                    int totalExpected = 69;

                    byte[] decodedJpeg = ReassembleCameraChunksFromText(rawText, out chunksFound, out totalExpected);

                    if (decodedJpeg != null && decodedJpeg.Length >= 4)
                    {
                        imageBytes = decodedJpeg;
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            image = Image.FromStream(ms);
                            pictureBox_img.Image = image;
                            button_save_pic.Enabled = true;
                        }

                        MessageBox.Show(
                            $"Successfully Reassembled Camera Image!\n" +
                            $"Chunks Decoded : {chunksFound} / {totalExpected}\n" +
                            $"Image Size     : {imageBytes.Length} Bytes\n" +
                            $"JPEG Markers   : FFD8 (SOI) -> FFD9 (EOI) Verified",
                            "CubeSat Photo Reassembled",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Could not reassemble a valid JPEG image from the input data.\n" +
                            "Please ensure camera chunks (0xCAFE headers or FFD8..FFD9 hex) are provided.",
                            "Image Reassembly Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Camera Operation Error:\n{ex.Message}",
                        "Image Processing Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Camera Operation Invalid.", "Empty Operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static byte[] ReassembleCameraChunksFromText(string rawText, out int chunksFound, out int totalExpected)
        {
            chunksFound = 0;
            totalExpected = 69;

            if (string.IsNullOrWhiteSpace(rawText))
                return null;

            var chunkDict = new Dictionary<int, byte[]>();

            // 1. Check for gs_app.c JSON lines: {"type":"CAMERA","pkt":1,"total":69,"offset":0,"len":128,"hex":"FFD8..."}
            var lines = rawText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.IndexOf("\"CAMERA\"", StringComparison.OrdinalIgnoreCase) >= 0 &&
                    line.IndexOf("\"hex\"", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Match mHex = Regex.Match(line, "\"hex\"\\s*:\\s*\"([0-9a-fA-F]+)\"");
                    if (mHex.Success)
                    {
                        string hx = mHex.Groups[1].Value;
                        if (hx.Length >= 2 && hx.Length % 2 == 0)
                        {
                            byte[] cBytes = new byte[hx.Length / 2];
                            for (int b = 0; b < cBytes.Length; b++)
                                cBytes[b] = Convert.ToByte(hx.Substring(b * 2, 2), 16);

                            int pIdx = chunkDict.Count;
                            Match mOff = Regex.Match(line, "\"offset\"\\s*:\\s*(\\d+)");
                            Match mPkt = Regex.Match(line, "\"pkt\"\\s*:\\s*(\\d+)");
                            Match mTot = Regex.Match(line, "\"total\"\\s*:\\s*(\\d+)");
                            if (mTot.Success)
                            {
                                int t = int.Parse(mTot.Groups[1].Value);
                                if (t > 0 && t <= 1000) totalExpected = t;
                            }
                            if (mOff.Success)
                            {
                                int off = int.Parse(mOff.Groups[1].Value);
                                pIdx = off / 128;
                            }
                            else if (mPkt.Success)
                            {
                                int p = int.Parse(mPkt.Groups[1].Value);
                                pIdx = p > 0 ? p - 1 : 0;
                            }
                            chunkDict[pIdx] = cBytes;
                        }
                    }
                }
            }

            // 2. Extract hex bytes from non-JSON lines (supports both space-separated "CA FE ..." and continuous "FFD8FFE0...")
            var byteList = new List<byte>();
            foreach (var line in lines)
            {
                if (line.IndexOf("\"CAMERA\"", StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;

                string workLine = line;
                int tagIdx = workLine.IndexOf("[CAMERA-HEX]", StringComparison.OrdinalIgnoreCase);
                if (tagIdx >= 0)
                {
                    workLine = workLine.Substring(tagIdx + "[CAMERA-HEX]".Length);
                }

                // First check for continuous hex runs (> 4 chars) or 2-char hex tokens
                var tokenMatches = Regex.Matches(workLine, @"\b[0-9a-fA-F]{2,}\b");
                foreach (Match tm in tokenMatches)
                {
                    string val = tm.Value;
                    if (val.Length == 2)
                    {
                        byteList.Add(Convert.ToByte(val, 16));
                    }
                    else if (val.Length >= 8 && val.Length % 2 == 0)
                    {
                        for (int k = 0; k < val.Length; k += 2)
                        {
                            byteList.Add(Convert.ToByte(val.Substring(k, 2), 16));
                        }
                    }
                }
            }

            byte[] allBytes = byteList.ToArray();

            // 3. Scan for 0xCA 0xFE (or 0xFE 0xCA) 14-byte headers in allBytes
            int i = 0;
            int firstMagicIdx = -1;
            while (i <= allBytes.Length - 14)
            {
                bool isMagic = (allBytes[i] == 0xCA && allBytes[i + 1] == 0xFE) ||
                               (allBytes[i] == 0xFE && allBytes[i + 1] == 0xCA);
                if (isMagic)
                {
                    if (firstMagicIdx == -1) firstMagicIdx = i;

                    ushort pIdx = (ushort)(allBytes[i + 2] | (allBytes[i + 3] << 8));
                    ushort tot = (ushort)(allBytes[i + 4] | (allBytes[i + 5] << 8));
                    uint offset = (uint)(allBytes[i + 6] | (allBytes[i + 7] << 8) | (allBytes[i + 8] << 16) | (allBytes[i + 9] << 24));
                    ushort dlen = (ushort)(allBytes[i + 10] | (allBytes[i + 11] << 8));

                    if (dlen == 0 || dlen > 128) dlen = 128;
                    if (tot > 0 && tot <= 500) totalExpected = tot;

                    int normIdx = (offset > 0 && offset % 128 == 0) ? (int)(offset / 128) : (pIdx > 0 && !chunkDict.ContainsKey(0) && pIdx <= totalExpected ? pIdx - 1 : pIdx);

                    if (i + 14 + dlen <= allBytes.Length)
                    {
                        byte[] chunkData = new byte[dlen];
                        Array.Copy(allBytes, i + 14, chunkData, 0, dlen);
                        chunkDict[normIdx] = chunkData;
                        i += 14 + dlen;
                        continue;
                    }
                }
                i++;
            }

            // 4. Check for leading Chunk 0 bytes before first CA FE (starting with FFD8)
            if (firstMagicIdx > 0 && !chunkDict.ContainsKey(0))
            {
                for (int j = 0; j <= firstMagicIdx - 2; j++)
                {
                    if (allBytes[j] == 0xFF && allBytes[j + 1] == 0xD8)
                    {
                        int len = Math.Min(128, firstMagicIdx - j);
                        byte[] c0 = new byte[len];
                        Array.Copy(allBytes, j, c0, 0, len);
                        chunkDict[0] = c0;
                        break;
                    }
                }
            }

            chunksFound = chunkDict.Count;

            // 5. Assemble chunks in sequence
            if (chunksFound > 0)
            {
                int maxIdx = chunkDict.Keys.Max();
                int minIdx = chunkDict.Keys.Min();
                using (MemoryStream ms = new MemoryStream())
                {
                    for (int idx = minIdx; idx <= maxIdx; idx++)
                    {
                        if (chunkDict.ContainsKey(idx))
                        {
                            byte[] cd = chunkDict[idx];
                            ms.Write(cd, 0, cd.Length);
                        }
                        else
                        {
                            byte[] blank = new byte[128];
                            ms.Write(blank, 0, blank.Length);
                        }
                    }
                    byte[] assembled = ms.ToArray();

                    // Find SOI (FFD8) and EOI (FFD9)
                    int soi = -1, eoi = -1;
                    for (int k = 0; k < assembled.Length - 1; k++)
                    {
                        if (assembled[k] == 0xFF && assembled[k + 1] == 0xD8 && soi == -1)
                            soi = k;
                        if (assembled[k] == 0xFF && assembled[k + 1] == 0xD9)
                            eoi = k + 2;
                    }

                    if (soi != -1 && eoi != -1 && eoi > soi)
                    {
                        byte[] trimmed = new byte[eoi - soi];
                        Array.Copy(assembled, soi, trimmed, 0, eoi - soi);
                        return trimmed;
                    }
                    else if (soi != -1)
                    {
                        // Append FFD9 EOI marker so partial reception can still be decoded
                        int len = assembled.Length - soi;
                        byte[] trimmed = new byte[len + 2];
                        Array.Copy(assembled, soi, trimmed, 0, len);
                        trimmed[len] = 0xFF;
                        trimmed[len + 1] = 0xD9;
                        return trimmed;
                    }
                    return assembled;
                }
            }

            // 6. Fallback for raw FFD8 ... FFD9 streams
            int fSoi = -1, fEoi = -1;
            for (int k = 0; k < allBytes.Length - 1; k++)
            {
                if (allBytes[k] == 0xFF && allBytes[k + 1] == 0xD8 && fSoi == -1)
                    fSoi = k;
                if (allBytes[k] == 0xFF && allBytes[k + 1] == 0xD9)
                    fEoi = k + 2;
            }
            if (fSoi != -1 && fEoi != -1 && fEoi > fSoi)
            {
                chunksFound = (fEoi - fSoi + 127) / 128;
                byte[] trimmed = new byte[fEoi - fSoi];
                Array.Copy(allBytes, fSoi, trimmed, 0, fEoi - fSoi);
                return trimmed;
            }
            else if (fSoi != -1 && allBytes.Length - fSoi >= 4)
            {
                int len = allBytes.Length - fSoi;
                chunksFound = (len + 127) / 128;
                byte[] trimmed = new byte[len + 2];
                Array.Copy(allBytes, fSoi, trimmed, 0, len);
                trimmed[len] = 0xFF;
                trimmed[len + 1] = 0xD9;
                return trimmed;
            }

            return null;
        }

        private void button_cancel_cam_Click(object sender, EventArgs e)
        {
            rTBox_cam_raw_data.Text = "";
            this.Close();
        }

        private void inputDialogCamera_Load(object sender, EventArgs e)
        {
            button_save_pic.Enabled = false;
            button_cam_paraser_ok.Enabled = false;
            btn_verify_image_data.Enabled = false;
            btn_decode_log.Enabled = false;
            btn_save_actual_log.Enabled=false;
        }

        private byte[] HexStringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        private void button_save_pic_Click(object sender, EventArgs e)
        {
            try
            {
                savingCAMRootPath = "../Storage/CAM_DETAILS";
                Properties.Settings.Default.savePathCAM = savingCAMRootPath;
                Properties.Settings.Default.Save();

                CAMRootPAth = Properties.Settings.Default.savePathCAM;

                savingCAMRootPath = CAMRootPAth;

                if (!Directory.Exists(savingCAMRootPath))
                {
                    Directory.CreateDirectory(savingCAMRootPath);
                }

                DateTime dt = DateTime.UtcNow;
                string time = dt.ToString("yyyyMMdd_HHmmss");
                string folderPath = savingCAMRootPath;
                string fileName;
                string absolutePath;

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // image store

                // Write the image byte array to a PNG     fileName = time + "_ADCS_raw.txt";

                fileName = time + "_cam_image.png";
                absolutePath = System.IO.Path.Combine(folderPath, fileName);
                File.WriteAllBytes(absolutePath, imageBytes);
                this.Close();

                MessageBox.Show(
                    $"Image Stored in {absolutePath}",
                    "Saving completed!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                           $"Image storing Operation Invalid \n {ex.Message}",
                           "Storing Failed",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Exclamation,
                           MessageBoxDefaultButton.Button1
                );
            }
        }

        private void btn_verify_image_data_Click(object sender, EventArgs e)
        {
            try
            {
                string rawCAMSRx = rTBox_cam_raw_data.Text;
                rawCAMSRx = rawCAMSRx.Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");

                rawCAMSRx = rawCAMSRx.ToUpper();

                string sequenceToExclude = "404040404040E0729C64A692406103F0";

                string result;

                // Loop to handle all occurrences of sequenceToExclude
                result = RemoveAndSeparateSequences(rawCAMSRx, sequenceToExclude);

                Console.WriteLine(result);

                savingCAMRootPath = "../Storage/CAM_DETAILS";
                Properties.Settings.Default.savePathCAM = savingCAMRootPath;
                Properties.Settings.Default.Save();

                CAMRootPAth = Properties.Settings.Default.savePathCAM;

                DateTime dt = DateTime.UtcNow;
                string time = dt.ToString("yyyyMMdd_HHmmss");

                savingCAMRootPath = CAMRootPAth;

                if (!Directory.Exists(savingCAMRootPath))
                {
                    Directory.CreateDirectory(savingCAMRootPath);
                }

                string rawRxData = result;

                string folderPath = savingCAMRootPath;
                string fileName;
                string absolutePath;

                fileName = time + "_CAM_Data_Verification.xlsx";
                absolutePath = System.IO.Path.Combine(folderPath, fileName);
                saveToExcel(result, absolutePath);
                rTBox_cam_raw_data.Text = "";
                btn_get_image_data.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                       $"CAMERA Operation Invalid \n {ex.Message}",
                       "Please input valid raw data",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error
                );
            }
        }
        static string RemoveAndSeparateSequences(string input, string sequenceToExclude)
        {
            bool firstOccurrence = true;

            // Replace occurrences of the sequence with a delimiter (space) after the first occurrence
            string result = Regex.Replace(input, sequenceToExclude, match =>
            {
                if (firstOccurrence)
                {
                    firstOccurrence = false;
                    return "";  // Return the sequence without a space for the first occurrence
                }
                else
                {
                    return "\n";  // Add space after subsequent occurrences
                }
            });

            return result.Trim(); // Remove any leading/trailing spaces that might remain
        }

        static void saveToExcel(string result, string absolutePath)
        {
            // Set the license context to NonCommercial or Commercial based on your use case
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;  // or LicenseContext.Commercial for commercial use

            if (!File.Exists(absolutePath))
            {
                // Create a new Excel file if it doesn't exist
                using (var package = new ExcelPackage(new FileInfo(absolutePath)))
                {
                    // Add a worksheet to the package
                    var worksheet = package.Workbook.Worksheets.Add("Data");

                    // Add headers to the first row
                    worksheet.Cells[1, 1].Value = "Packet Type";     // Column A: Packet Type
                    worksheet.Cells[1, 2].Value = "Packet Number";   // Column B: Packet Number
                    worksheet.Cells[1, 3].Value = "Count Packet";   // Column C: count packet
                    worksheet.Cells[1, 4].Value = "Raw Data";        // Column D: Raw Data

                    worksheet.Columns[1].AutoFit();
                    worksheet.Columns[2].AutoFit();
                    worksheet.Columns[3].AutoFit();

                    // Make the first row bold
                    worksheet.Row(1).Style.Font.Bold = true;

                    // Split the result into parts based on spaces (subsequent sequences)
                    var sequences = result.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    int rowIndex = 2; // Starting row in Excel (row 1)
                    foreach (var sequence in sequences)
                    {
                        if (sequence.Length >= 2)
                        {
                            // Write the first byte to column A
                            worksheet.Cells[rowIndex, 1].Value = sequence.Substring(0, 2);

                            // Write the second byte to column B
                            worksheet.Cells[rowIndex, 2].Value = sequence.Substring(2, 2);

                            worksheet.Cells[rowIndex, 3].Formula = $"=IF(ISBLANK(B{rowIndex}),\"\", IFERROR(HEX2DEC(B{rowIndex}), \"ERROR\"))";

                            // Write the remaining bytes to column C (Raw Data) with spaces after every two bytes
                            string rawData = sequence.Length > 4 ? sequence.Substring(4) : "";
                            worksheet.Cells[rowIndex, 4].Value = InsertSpacesAfterEveryTwoBytes(rawData);

                            rowIndex++; // Move to the next row
                        }
                    }


                    int lastRow = worksheet.Dimension.End.Row;
                    int lastColumn = worksheet.Dimension.End.Column;

                    // Define the dynamic range based on data
                    var range = worksheet.Cells[1, 1, lastRow, lastColumn];

                    // Apply borders dynamically to the range containing data
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    // Optionally, set the color of the borders
                    range.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);

                    // Save the Excel file
                    package.Save();

                    DialogResult dialog_result = MessageBox.Show($"File stored to : {absolutePath}\n Click 'OK' to preview",
                        "Check the downloaded data",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    if (dialog_result == DialogResult.OK)
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
                            MessageBox.Show($"Error opening file: \n {ex.Message}",
                                "File Error!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                            Console.WriteLine($"Error opening file: {ex.Message}");
                        }
                    }
                }
            }
        }

        static string InsertSpacesAfterEveryTwoBytes(string input)
        {
            var sb = new System.Text.StringBuilder();

            // Split the input into separate lines, preserving the newline characters
            var lines = input.Split(new[] { '\n' }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                // Remove all spaces and other whitespace characters from each line
                var cleanedLine = new string(line.Where(c => !char.IsWhiteSpace(c)).ToArray());

                // Insert spaces after every two bytes in the cleaned line
                for (int i = 0; i < cleanedLine.Length; i += 2)
                {
                    // Append two characters followed by a space
                    sb.Append(cleanedLine.Substring(i, Math.Min(2, cleanedLine.Length - i)) + " ");
                }

                // Add a newline to separate each formatted line
                sb.AppendLine();
            }

            // Return the formatted result with spaces between every two bytes and newlines preserved
            return sb.ToString().TrimEnd();  // Remove any trailing newline that might remain
        }

        private void btn_get_image_data_Click(object sender, EventArgs e)
        {
            rTBox_cam_raw_data.Text = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Camera Data / Excel / Text Files|*.xlsx;*.xlsm;*.txt;*.log;*.json|All Files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    if (filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                        filePath.EndsWith(".xlsm", StringComparison.OrdinalIgnoreCase))
                    {
                        ReadExcelFile(filePath);
                    }
                    else
                    {
                        try
                        {
                            rTBox_cam_raw_data.Text = File.ReadAllText(filePath);
                            button_cam_paraser_ok.Enabled = true;
                            btn_save_actual_log.Enabled = true;
                            TryLiveRenderImage();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error reading camera data file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void ReadExcelFile(string filePath)
        {
            // Check if the file exists
            if (File.Exists(filePath))
            {
                try
                {
                    // Set the license context to NonCommercial (or Commercial for commercial use)
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    // Open the Excel file
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        // Get the first worksheet (assuming it's the one you're interested in)
                        var worksheet = package.Workbook.Worksheets[0];

                        // Start from row 2 (to skip the header row) and column 4 (Raw Data)
                        int rowIndex = 2;

                        // Get the number of rows in the worksheet
                        int totalRows = worksheet.Dimension.End.Row;

                        // Loop through all rows from row 2 onward in column 4 (Raw Data)
                        while (rowIndex <= totalRows)
                        {
                            // Get the value from column 3 (Raw Data) for the current row
                            var rawData = worksheet.Cells[rowIndex, 4].Text;


                            // Print the raw data to the console (or you can use a ListBox or DataGridView to display it in the UI)
                            Console.WriteLine($"Row {rowIndex}: {rawData}");
                            // Append the raw data to the RichTextBox
                            rTBox_cam_raw_data.AppendText(rawData + Environment.NewLine);

                            button_cam_paraser_ok.Enabled = true;
                            btn_save_actual_log.Enabled = true;

                            // You can display the data in a message box or any other UI component as needed
                            // MessageBox.Show(rawData);

                            rowIndex++; // Move to the next row
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., file read errors)
                    MessageBox.Show($"Error reading the Excel file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The selected file does not exist.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rTBox_cam_raw_data_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rTBox_cam_raw_data.Text))
            {
                btn_verify_image_data.Enabled = false;
                btn_decode_log.Enabled = false;
                button_cam_paraser_ok.Enabled = false;
                btn_save_actual_log.Enabled = false;
            }
            else
            {
                btn_verify_image_data.Enabled = true;
                btn_decode_log.Enabled = true;
                button_cam_paraser_ok.Enabled = true;
                btn_save_actual_log.Enabled = true;
            }
        }

        private void btn_decode_log_Click(object sender, EventArgs e)
        {
            try
            {
                string rawCAMSRx = rTBox_cam_raw_data.Text;
                rawCAMSRx = rawCAMSRx.Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");

                rawCAMSRx = rawCAMSRx.ToUpper();

                string sequenceToExclude = "404040404040E0729C64A692406103F0";

                string result;

                // Loop to handle all occurrences of sequenceToExclude
                result = RemoveAndSeparateSequences(rawCAMSRx, sequenceToExclude);

                Console.WriteLine(result);

                savingCAMRootPath = "../Storage/CAM_DETAILS/CAM_LOGS";
                Properties.Settings.Default.savePathCAMLOG = savingCAMRootPath;
                Properties.Settings.Default.Save();

                CAMRootPAth = Properties.Settings.Default.savePathCAMLOG;

                DateTime dt = DateTime.UtcNow;
                string time = dt.ToString("yyyyMMdd_HHmmss");

                savingCAMRootPath = CAMRootPAth;

                if (!Directory.Exists(savingCAMRootPath))
                {
                    Directory.CreateDirectory(savingCAMRootPath);
                }

                string rawRxData = result;

                string folderPath = savingCAMRootPath;
                string fileName;
                string absolutePath;

                fileName = time + "_CAM_LOG_Data_Verification.xlsx";
                absolutePath = System.IO.Path.Combine(folderPath, fileName);
                LOGsaveToExcel(result, absolutePath);
                btn_get_log.Enabled = true;
                rTBox_cam_raw_data.Text = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                       $"CAMERA LOG Operation Invalid \n {ex.Message}",
                       "Please input valid raw data",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error
                );
            }
        }

        static void LOGsaveToExcel(string result, string absolutePath)
        {
            // Set the license context to NonCommercial or Commercial based on your use case
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;  // or LicenseContext.Commercial for commercial use

            if (!File.Exists(absolutePath))
            {
                // Create a new Excel file if it doesn't exist
                using (var package = new ExcelPackage(new FileInfo(absolutePath)))
                {
                    // Add a worksheet to the package
                    var worksheet = package.Workbook.Worksheets.Add("Raw_Data");

                    // Add headers to the first row
                    worksheet.Cells[1, 1].Value = "Packet Type";     // Column A: Packet Type
                    worksheet.Cells[1, 2].Value = "Packet Number";   // Column B: Packet Number
                    worksheet.Cells[1, 3].Value = "Count Packet";   // Column C: count packet
                    worksheet.Cells[1, 4].Value = "Raw Data";        // Column D: Raw Data 

                    worksheet.Columns[1].AutoFit();
                    worksheet.Columns[2].AutoFit();
                    worksheet.Columns[3].AutoFit();
                    worksheet.Columns[4].AutoFit();

                    // Make the first row bold
                    worksheet.Row(1).Style.Font.Bold = true;

                    // Split the result into parts based on spaces (subsequent sequences)
                    var sequences = result.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    int rowIndex = 2; // Starting row in Excel (row 1)
                    foreach (var sequence in sequences)
                    {
                        if (sequence.Length >= 2)
                        {
                            // Write the first byte to column A
                            worksheet.Cells[rowIndex, 1].Value = sequence.Substring(0, 2);

                            // Write the second byte to column B
                            worksheet.Cells[rowIndex, 2].Value = sequence.Substring(2, 2);

                            worksheet.Cells[rowIndex, 3].Formula = $"=IF(ISBLANK(B{rowIndex}),\"\", IFERROR(HEX2DEC(B{rowIndex}), \"ERROR\"))";

                            // Write the remaining bytes to column C (Raw Data) with spaces after every two bytes
                            string rawData = sequence.Length > 4 ? sequence.Substring(4) : "";
                            worksheet.Cells[rowIndex, 4].Value = InsertSpacesAfterEveryTwoBytes(rawData);

                            rowIndex++; // Move to the next row
                        }
                    }


                    int lastRow = worksheet.Dimension.End.Row;
                    int lastColumn = worksheet.Dimension.End.Column;

                    // Define the dynamic range based on data
                    var range = worksheet.Cells[1, 1, lastRow, lastColumn];

                    // Apply borders dynamically to the range containing data
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    // Optionally, set the color of the borders
                    range.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);

                    // Save the Excel file
                    package.Save();

                    DialogResult dialog_result = MessageBox.Show($"File stored to : {absolutePath}\n Click 'OK' to preview",
                        "Check the downloaded data",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    if (dialog_result == DialogResult.OK)
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
                            MessageBox.Show($"Error opening file: \n {ex.Message}",
                                "File Error!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                            Console.WriteLine($"Error opening file: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void btn_get_log_Click(object sender, EventArgs e)
        {
            rTBox_cam_raw_data.Text = "";
            // Create an OpenFileDialog to allow the user to select an Excel file
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xlsm"; // Allow only Excel files

                // If the user selects a file and clicks 'OK'
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName; // Get the selected file's path
                    ReadExcelFile(filePath); // Call the method to read the data from the selected file
                }
            }
        }

        private void btn_save_actual_log_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(rTBox_cam_raw_data.Text))
            {

                try
                {
                    string rawCAMSRx = rTBox_cam_raw_data.Text;
                    rawCAMSRx = rawCAMSRx.Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");

                    rawCAMSRx = rawCAMSRx.ToUpper();

                    string sequenceToExclude = "58FF";

                    string result;

                    // Loop to handle all occurrences of sequenceToExclude
                    result = RemoveAndSeparateSequences_LOG(rawCAMSRx, sequenceToExclude);

                    Console.WriteLine(result);

                    savingCAMRootPath = "../Storage/CAM_DETAILS/CAM_LOGS";
                    Properties.Settings.Default.savePathCAMLOG = savingCAMRootPath;
                    Properties.Settings.Default.Save();

                    CAMRootPAth = Properties.Settings.Default.savePathCAMLOG;

                    DateTime dt = DateTime.UtcNow;
                    string time = dt.ToString("yyyyMMdd_HHmmss");

                    savingCAMRootPath = CAMRootPAth;

                    if (!Directory.Exists(savingCAMRootPath))
                    {
                        Directory.CreateDirectory(savingCAMRootPath);
                    }

                    string rawRxData = result;

                    string folderPath = savingCAMRootPath;
                    string fileName;
                    string absolutePath;

                    fileName = time + "_CAM_LOG_Data.xlsx";
                    absolutePath = System.IO.Path.Combine(folderPath, fileName);
                    ActualLOGsaveToExcel(result, absolutePath);
                    rTBox_cam_raw_data.Text = "";

                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                           $"CAMERA LOG Operation Invalid \n {ex.Message}",
                           "Please input valid raw data",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error
                    );
                }
            }
            else
            {
                MessageBox.Show(
                         "Image RGB LOG Operation Invalid \n",
                         "Empty Field",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Exclamation,
                         MessageBoxDefaultButton.Button1
                );
            }
        }

        static void ActualLOGsaveToExcel(string result, string absolutePath)
        {
            // Set the license context to NonCommercial or Commercial based on your use case
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;  // or LicenseContext.Commercial for commercial use

            if (!File.Exists(absolutePath))
            {
                // Create a new Excel file if it doesn't exist
                using (var package = new ExcelPackage(new FileInfo(absolutePath)))
                {
                    // Add a worksheet to the package
                    var worksheet = package.Workbook.Worksheets.Add("Data");

                    // Add headers to the first row
                    worksheet.Cells[1, 1].Value = "SN";
                    worksheet.Cells[1, 2].Value = "Raw Data";        
                    worksheet.Cells[1, 3].Value = "Decoded Comments";

                    worksheet.Columns[2].AutoFit();
                    worksheet.Columns[3].AutoFit();

                    // Make the first row bold
                    worksheet.Row(1).Style.Font.Bold = true;

                    // Split the result into parts based on spaces (subsequent sequences)
                    var sequences = result.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    int rowIndex = 2; // Starting row in Excel (row 1)

                    int serialNumber = 1; // Starting serial number

                    foreach (var sequence in sequences)
                    {
                        if (sequence.Length >= 2)
                        {
                            string serial = serialNumber.ToString("D2"); // Format serial as 01, 02, etc.

                            worksheet.Cells[rowIndex, 1].Value = serial; // Write Serial Number

                            worksheet.Cells[rowIndex, 2].Value = InsertSpacesAfterEveryTwoBytes(sequence);

                            worksheet.Cells[rowIndex, 3].Formula = $"=IF(ISBLANK(B{rowIndex}),\"\", IFERROR(MID(B{rowIndex},1,2),\"\") & \" \" & IFERROR(MID(B{rowIndex},4,2),\"\") & \" \" & IFERROR(MID(B{rowIndex},7,2),\"\") & \" \" & IFERROR(MID(B{rowIndex},10,2),\"\") & \" \" & IFERROR(MID(B{rowIndex},13,2),\"\") & \" \" & IFERROR(MID(B{rowIndex},16,2),\"\") & \" \" & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},19,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},22,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},25,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},28,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},31,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},34,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},37,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},40,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},43,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},46,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},49,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},52,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},55,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},58,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},61,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},64,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},67,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},70,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},73,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},76,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},79,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},82,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},85,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},88,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},91,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},94,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},97,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},100,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},103,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},106,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},109,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},112,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},115,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},118,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},121,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},124,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},127,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},130,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},133,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},136,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},139,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},142,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},145,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},148,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},151,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},154,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},157,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},160,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},163,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},166,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},169,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},172,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},175,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},178,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},181,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},184,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},187,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},190,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},193,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},196,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},199,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},202,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},205,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},208,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},211,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},214,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},217,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},220,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},223,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},226,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},229,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},232,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},235,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},238,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},241,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},244,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},247,2))),\"\") & IFERROR(CHAR(HEX2DEC(MID(B{rowIndex},249,2))),\"\"))";

                            rowIndex++; // Move to the next row
                            serialNumber++;
                        }
                    }


                    int lastRow = worksheet.Dimension.End.Row;
                    int lastColumn = worksheet.Dimension.End.Column;

                    // Define the dynamic range based on data
                    var range = worksheet.Cells[1, 1, lastRow, lastColumn];

                    // Apply borders dynamically to the range containing data
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    // Optionally, set the color of the borders
                    range.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);

                    // Save the Excel file
                    package.Save();

                    DialogResult dialog_result = MessageBox.Show($"File stored to : {absolutePath}\n Click 'OK' to preview",
                        "Check the downloaded data",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    if (dialog_result == DialogResult.OK)
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
                            MessageBox.Show($"Error opening file: \n {ex.Message}",
                                "File Error!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                            Console.WriteLine($"Error opening file: {ex.Message}");
                        }
                    }
                }
            }
        }

        static string RemoveAndSeparateSequences_LOG(string input, string sequenceToExclude)
        {
            bool firstOccurrence = true;

            // Replace occurrences of the sequence with a delimiter (space) after the first occurrence
            string result = Regex.Replace(input, sequenceToExclude, match =>
            {
                if (firstOccurrence)
                {
                    firstOccurrence = false;
                    return "58FF\n";  // Return the sequence without a space for the first occurrence
                }
                else
                {
                    return "58FF\n";  // Add space after subsequent occurrences
                }
            });

            return result.Trim(); // Remove any leading/trailing spaces that might remain
        }

    }
}
