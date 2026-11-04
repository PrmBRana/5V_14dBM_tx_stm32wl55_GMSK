using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenBirdsOperation
{
    public partial class inputDialogADCS : Form
    {
        public string savingADCSRootPath;
        public string ADCSRootPath;

        public DateTime dtAppStart = DateTime.UtcNow;

        public inputDialogADCS()
        {
            InitializeComponent();
        }

        private void button_cancel_adcs_Click(object sender, EventArgs e)
        {
            rTBox_adsc_raw_data.Text = "";
            this.Close();
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

        private void btn_verify_adcs_Click(object sender, EventArgs e)
        {
            try
            {
                string rawADCSRx = rTBox_adsc_raw_data.Text;
                rawADCSRx = rawADCSRx.Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");

                rawADCSRx = rawADCSRx.ToUpper();

                string sequenceToExclude = "404040404040E0729C64A692406103F0";

                string result;

                // Loop to handle all occurrences of sequenceToExclude
                result = RemoveAndSeparateSequences(rawADCSRx, sequenceToExclude);

                Console.WriteLine(result);

                savingADCSRootPath = "../Storage/ADCS_DETAILS";
                Properties.Settings.Default.savePathADCS = savingADCSRootPath;
                Properties.Settings.Default.Save();

                ADCSRootPath = Properties.Settings.Default.savePathADCS;

                savingADCSRootPath = ADCSRootPath;

                if (!Directory.Exists(savingADCSRootPath))
                {
                    Directory.CreateDirectory(savingADCSRootPath);
                }

                string rawRxData = result;

                DateTime dt = DateTime.UtcNow;
                string time = dt.ToString("yyyyMMdd_HHmmss");
                string folderPath = savingADCSRootPath;
                string fileName;
                string absolutePath;

                fileName = time + "_ADCS_Data_Verification.xlsx";
                absolutePath = System.IO.Path.Combine(folderPath, fileName);
                saveToExcel(result, absolutePath, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                       $"ADCS Operation Invalid \n {ex.Message}",
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

                    form.Close();

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

        private void inputDialogADCS_Load(object sender, EventArgs e)
        {
            btn_verify_adcs.Enabled = false;
        }

        private void rTBox_adsc_raw_data_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rTBox_adsc_raw_data.Text))
            {
                btn_verify_adcs.Enabled = false;
            }
            else
            {
                btn_verify_adcs.Enabled = true;
            }
        }
    }
}
