using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenBirdsOperation
{
    public partial class inputDialogDigipeater : Form
    {
        public string savingDigipeaterRootPath;
        public string DigipeaterRootPath;
        public string savingDigipeaterRawRootPath;
        public string DigipeaterRawRootPath;

        public inputDialogDigipeater()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            rTBox_digipeater_raw_data.Text = "";
            this.Close();
        }

        private void inputDialogDigipeater_Load(object sender, EventArgs e)
        {
            btn_verify_digipeater_data.Enabled = false;
            button_test.Enabled = false;
        }

        private void rTBox_digipeater_raw_data_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rTBox_digipeater_raw_data.Text))
            {
                btn_verify_digipeater_data.Enabled = false;
                button_test.Enabled = false;
            }
            else
            {
                button_test.Enabled = true;
            }
        }

        private void btn_verify_digipeater_data_Click(object sender, EventArgs e)
        {
            try
            {
                string rawDigipeaterRx = rTBox_digipeater_raw_data.Text;
                rawDigipeaterRx = rawDigipeaterRx.Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");

                rawDigipeaterRx = rawDigipeaterRx.ToUpper();

                string result;

                // Loop to handle all occurrences of sequenceToExclude
                result = FormatHexData(rawDigipeaterRx);

                Console.WriteLine(result);

                savingDigipeaterRootPath = "../Storage/DIGIPEATER_DETAILS";
                Properties.Settings.Default.savePathDigipeater = savingDigipeaterRootPath;
                Properties.Settings.Default.Save();

                DigipeaterRootPath = Properties.Settings.Default.savePathDigipeater;

                savingDigipeaterRootPath = DigipeaterRootPath;

                if (!Directory.Exists(savingDigipeaterRootPath))
                {
                    Directory.CreateDirectory(savingDigipeaterRootPath);
                }

                string rawRxData = result;

                DateTime dt = DateTime.UtcNow;
                string time = dt.ToString("yyyyMMdd_HHmmss");
                string folderPath = savingDigipeaterRootPath;
                string fileName;
                string absolutePath;

                fileName = time + "_Digipeater_Data.xlsx";
                absolutePath = System.IO.Path.Combine(folderPath, fileName);
                saveToExcel(result, absolutePath, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                       $"Digipeater Operation Invalid \n {ex.Message}",
                       "Please input valid raw data",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Warning
                );
            }
        }

        static string FormatHexData(string hexData)
        {
            StringBuilder output = new StringBuilder();
            int currentIndex = 0;

            while (currentIndex < hexData.Length)
            {
                // Find the next occurrence of "7E00"
                int endIndex = hexData.IndexOf("7E00", currentIndex, StringComparison.OrdinalIgnoreCase);
                if (endIndex == -1)
                {
                    // No more blocks found; append the rest
                    output.Append(hexData.Substring(currentIndex));
                    break;
                }

                endIndex += 4; // Move past "7E00"

                // Move past trailing 00s
                while (endIndex + 2 <= hexData.Length && hexData.Substring(endIndex, 2) == "00")
                {
                    endIndex += 2;
                }

                string block = hexData.Substring(currentIndex, endIndex - currentIndex);
                output.AppendLine(block);
                currentIndex = endIndex;
            }

            return output.ToString();
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
                    var worksheet_1 = package.Workbook.Worksheets.Add("Extracted_Data");

                    // Add headers to the first row

                    worksheet_1.Cells[1, 1].Value = "SN";
                    worksheet_1.Cells[1, 2].Value = "Raw Data";
                    worksheet_1.Cells[1, 3].Value = "Raw Message";
                    worksheet_1.Cells[1, 4].Value = "Time Stamp";
                    worksheet_1.Cells[1, 5].Value = "Destination Call Sign";
                    worksheet_1.Cells[1, 6].Value = "Source Call Sign";
                    worksheet_1.Cells[1, 7].Value = "Message";

                    worksheet_1.Columns[3].AutoFit();
                    worksheet_1.Columns[4].AutoFit();
                    worksheet_1.Columns[5].AutoFit();
                    worksheet_1.Columns[6].AutoFit();

                    // Make the first row bold for both worksheets
                    worksheet_1.Row(1).Style.Font.Bold = true;

                    // Split the result into parts based on spaces (subsequent sequences)
                    var sequences = result.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    int rowIndex = 2; // Starting row in Excel (row 1)
        
                    foreach (var sequence in sequences)
                    {
                        if (sequence.Length >= 2)
                        {

                            worksheet_1.Cells[rowIndex, 1].Formula = $"=IF(ISBLANK(B{rowIndex}),\"\",ROW()-1)"; ;
                            //raw_data
                            worksheet_1.Cells[rowIndex, 2].Value = InsertSpacesAfterEveryTwoBytes(sequence);
                            //raw message
                            worksheet_1.Cells[rowIndex, 3].Formula = $"=IF(ISBLANK(B{rowIndex}),\"\",MID(B{rowIndex}, 79, LEN(B2)-79-4))";
                            //timestamp
                            worksheet_1.Cells[rowIndex, 4].Style.Numberformat.Format = "yyyy-mm-dd, hh:mm:ss";
                            worksheet_1.Cells[rowIndex, 4].Formula = $"=IF(ISBLANK(B{rowIndex}),\"\",(((HEX2DEC(LEFT(SUBSTITUTE(B{rowIndex}, \" \", \"\"), 8))/60)/60)/24) + DATE(1970,1,1))";
                            //destination 
                            worksheet_1.Cells[rowIndex, 5].Formula = $"=IF(ISBLANK(B{rowIndex}),\"\",CHAR(INT(HEX2DEC(MID(B{rowIndex},37,2))/2)) & CHAR(INT(HEX2DEC(MID(B{rowIndex},40,2))/2)) & CHAR(INT(HEX2DEC(MID(B{rowIndex},43,2))/2)) & CHAR(INT(HEX2DEC(MID(B{rowIndex},46,2))/2)) & CHAR(INT(HEX2DEC(MID(B{rowIndex},49,2))/2)) & CHAR(INT(HEX2DEC(MID(B{rowIndex},52,2))/2)) & CHAR(INT(HEX2DEC(MID(B{rowIndex},55,2))/2)))";
                            //source
                            worksheet_1.Cells[rowIndex, 6].Formula = $"=IF(ISBLANK(B{rowIndex}),\"\",CHAR(INT(HEX2DEC(MID(B{rowIndex},58,2))/2)) & CHAR(INT(HEX2DEC(MID(B{rowIndex},61,2))/2)) & CHAR(INT(HEX2DEC(MID(B{rowIndex},64,2))/2)) & CHAR(INT(HEX2DEC(MID(B{rowIndex},67,2))/2)) & CHAR(INT(HEX2DEC(MID(B{rowIndex},70,2))/2)) & CHAR(INT(HEX2DEC(MID(B{rowIndex},73,2))/2)) & CHAR(INT(HEX2DEC(MID(B{rowIndex},76,2))/2)))";
                            //message
                            worksheet_1.Cells[rowIndex, 7].Formula = $"=IF(ISBLANK(C{rowIndex}), \"\", IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 1, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 4, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 7, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 10, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 13, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 16, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 19, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 22, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 25, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 28, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 31, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 34, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 37, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 40, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 43, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 46, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 49, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 52, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 55, 2))), \"\") & IFERROR(CHAR(HEX2DEC(MID(C{rowIndex}, 58, 2))), \"\"))";

                            rowIndex++; // Move to the next row
                       
                        }
                    }

                    int lastRow = worksheet_1.Dimension.End.Row;
                    int lastColumn = worksheet_1.Dimension.End.Column;

                    // Define the dynamic range based on data
                    var range = worksheet_1.Cells[1, 1, lastRow, lastColumn];

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

        private void button_test_Click(object sender, EventArgs e)
        {
            try
            {
                string rawDigipeaterRx = rTBox_digipeater_raw_data.Text;
                rawDigipeaterRx = rawDigipeaterRx.Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");

                rawDigipeaterRx = rawDigipeaterRx.ToUpper();

                string sequenceToExclude = "404040404040E0729C64A692406103F0";

                string result;

                // Loop to handle all occurrences of sequenceToExclude
                result = RemoveAndSeparateSequences(rawDigipeaterRx, sequenceToExclude);

                Console.WriteLine(result);

                savingDigipeaterRawRootPath = "../Storage/DIGIPEATER_DETAILS";
                Properties.Settings.Default.savePathDigipeaterRaw = savingDigipeaterRawRootPath;
                Properties.Settings.Default.Save();

                DigipeaterRawRootPath = Properties.Settings.Default.savePathDigipeaterRaw;

                savingDigipeaterRawRootPath = DigipeaterRawRootPath;

                if (!Directory.Exists(savingDigipeaterRawRootPath))
                {
                    Directory.CreateDirectory(savingDigipeaterRawRootPath);
                }

                string rawRxData = result;

                DateTime dt = DateTime.UtcNow;
                string time = dt.ToString("yyyyMMdd_HHmmss");
                string folderPath = savingDigipeaterRawRootPath;
                string fileName;
                string absolutePath;

                fileName = time + "_Digipeater_Raw_Data_Verification.xlsx";
                absolutePath = System.IO.Path.Combine(folderPath, fileName);
                saveRawToExcel(result, absolutePath, this);
                rTBox_digipeater_raw_data.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                       $"Digipeater Operation Invalid \n {ex.Message}",
                       "Please input valid raw data",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Warning
                );
            }
        }

        static void saveRawToExcel(string result, string absolutePath, Form form)
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

        private void button_open_file_Click(object sender, EventArgs e)
        {
            rTBox_digipeater_raw_data.Text = "";
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
                            rTBox_digipeater_raw_data.AppendText(rawData + Environment.NewLine);

                            btn_verify_digipeater_data.Enabled = true;

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
    }
}
