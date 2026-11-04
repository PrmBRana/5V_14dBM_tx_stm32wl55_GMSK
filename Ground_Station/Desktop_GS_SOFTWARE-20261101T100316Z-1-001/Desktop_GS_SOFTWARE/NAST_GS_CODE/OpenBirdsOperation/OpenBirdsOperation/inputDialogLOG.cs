using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBirdsOperation
{
    public partial class inputDialogLOG : Form
    {

        public string savingLOGRootPath;
        public string LOGRootPath;


        public inputDialogLOG()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            rTBox_LOG_raw_data.Text = "";
            this.Close();
        }

        private void btn_verify_LOG_Click(object sender, EventArgs e)
        {
            try
            {
                string rawDigipeaterRx = rTBox_LOG_raw_data.Text;
                rawDigipeaterRx = rawDigipeaterRx.Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");

                rawDigipeaterRx = rawDigipeaterRx.ToUpper();

                string sequenceToExclude = "404040404040E0729C64A692406103F0";

                string result;

                // Loop to handle all occurrences of sequenceToExclude
                result = RemoveAndSeparateSequences(rawDigipeaterRx, sequenceToExclude);

                Console.WriteLine(result);

                savingLOGRootPath = "../Storage/MSN_LOG";
                Properties.Settings.Default.savePathLOG = savingLOGRootPath;
                Properties.Settings.Default.Save();

                LOGRootPath = Properties.Settings.Default.savePathLOG;

                savingLOGRootPath = LOGRootPath;

                if (!Directory.Exists(savingLOGRootPath))
                {
                    Directory.CreateDirectory(savingLOGRootPath);
                }

                string rawRxData = result;

                DateTime dt = DateTime.UtcNow;
                string time = dt.ToString("yyyyMMdd_HHmmss");
                string folderPath = savingLOGRootPath;
                string fileName;
                string absolutePath;

                fileName = time + "_MSN_LOG_Data_Verification.xlsx";
                absolutePath = System.IO.Path.Combine(folderPath, fileName);
                saveToExcel(result, absolutePath, this);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                       $"LOG Operation Invalid \n {ex.Message}",
                       "Please input valid raw data",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Warning
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

        static void saveToExcel(string result, string absolutePath, Form form)
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
                    worksheet.Cells[1, 5].Value = "ADDR_1";
                    worksheet.Cells[1, 6].Value = "ADDR_2";
                    worksheet.Cells[1, 7].Value = "ADDR_3";
                    worksheet.Cells[1, 8].Value = "ADDR_4";
                    worksheet.Cells[1, 9].Value = "ADDR_5";
                    worksheet.Cells[1, 10].Value = "ADDD_6";
                    worksheet.Cells[1, 11].Value = "ADDR_7";
                    worksheet.Cells[1, 12].Value = "ADDR_8";
                    worksheet.Cells[1, 13].Value = "ADDR_9";
                    worksheet.Cells[1, 14].Value = "ADDR_10";
                    worksheet.Cells[1, 15].Value = "ADDR_11";
                    worksheet.Cells[1, 16].Value = "ADDR_12";
                    worksheet.Cells[1, 17].Value = "ADDR_13";
                    worksheet.Cells[1, 18].Value = "ADDR_14";
                    worksheet.Cells[1, 19].Value = "ADDR_15";
                    worksheet.Cells[1, 20].Value = "ADDR_16";
                    worksheet.Cells[1, 21].Value = "ADDR_17";
                    worksheet.Cells[1, 22].Value = "ADDR_18";
                    worksheet.Cells[1, 23].Value = "ADDR_19";
                    worksheet.Cells[1, 24].Value = "ADDR_20";

                    worksheet.Columns[1].AutoFit();
                    worksheet.Columns[2].AutoFit();
                    worksheet.Columns[3].AutoFit();

                    // Make the first row bold for both worksheets
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

                            worksheet.Cells[rowIndex, 5].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},1,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},4,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},7,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},10,2),\"\"))";

                            worksheet.Cells[rowIndex, 6].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},13,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},16,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},19,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},22,2),\"\"))";

                            worksheet.Cells[rowIndex, 7].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},25,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},28,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},31,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},34,2),\"\"))";
                            worksheet.Cells[rowIndex, 8].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},37,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},40,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},43,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},46,2),\"\"))";
                            worksheet.Cells[rowIndex, 9].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},49,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},52,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},55,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},58,2),\"\"))";
                            worksheet.Cells[rowIndex, 10].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},61,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},64,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},67,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},70,2),\"\"))";
                            worksheet.Cells[rowIndex, 11].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},73,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},76,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},79,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},82,2),\"\"))";
                            worksheet.Cells[rowIndex, 12].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},85,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},88,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},91,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},94,2),\"\"))";
                            worksheet.Cells[rowIndex, 13].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},97,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},100,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},103,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},106,2),\"\"))";
                            worksheet.Cells[rowIndex, 14].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},109,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},112,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},115,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},118,2),\"\"))";
                            worksheet.Cells[rowIndex, 15].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},121,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},124,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},127,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},130,2),\"\"))";
                            worksheet.Cells[rowIndex, 16].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},133,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},136,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},139,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},142,2),\"\"))";
                            worksheet.Cells[rowIndex, 17].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},145,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},148,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},151,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},154,2),\"\"))";
                            worksheet.Cells[rowIndex, 18].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},157,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},160,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},163,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},166,2),\"\"))";
                            worksheet.Cells[rowIndex, 19].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},169,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},172,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},175,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},178,2),\"\"))";
                            worksheet.Cells[rowIndex, 20].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},181,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},184,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},187,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},190,2),\"\"))";
                            worksheet.Cells[rowIndex, 21].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},193,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},196,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},199,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},202,2),\"\"))";
                            worksheet.Cells[rowIndex, 22].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},205,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},208,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},211,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},214,2),\"\"))";
                            worksheet.Cells[rowIndex, 23].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},217,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},220,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},223,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},226,2),\"\"))";
                            worksheet.Cells[rowIndex, 24].Formula = $"=IF(ISBLANK(D{rowIndex}),\"\",IFERROR(MID(D{rowIndex},229,2),\"\")  & \" \" & IFERROR(MID(D{rowIndex},232,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},235,2),\"\") & \" \" & IFERROR(MID(D{rowIndex},238,2),\"\"))";

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

        private void inputDialogLOG_Load(object sender, EventArgs e)
        {
            btn_verify_LOG.Enabled = false;
        }

        private void rTBox_LOG_raw_data_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rTBox_LOG_raw_data.Text))
            {
                btn_verify_LOG.Enabled = false;
            }
            else
            {
                btn_verify_LOG.Enabled = true;
            }
        }
    }
}
