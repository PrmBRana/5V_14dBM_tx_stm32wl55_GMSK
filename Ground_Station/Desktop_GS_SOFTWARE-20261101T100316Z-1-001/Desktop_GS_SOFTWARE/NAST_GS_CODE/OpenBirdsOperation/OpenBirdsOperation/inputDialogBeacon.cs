using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OfficeOpenXml;
using System.Diagnostics;
using OfficeOpenXml.Style;
using System.Linq;

namespace OpenBirdsOperation
{
    public partial class inputDialogBeacon : Form
    {

        public string savingBeaconRootPath;
        public string BeaconRootPath;

        public DateTime dtAppStart = DateTime.UtcNow;

        public inputDialogBeacon()
        {
            InitializeComponent();
        }

        public static string callSignDecoder(string callsign_raw)
        {
            StringBuilder source_callsign_ascii = new StringBuilder(); // Store the decoded ASCII characters

            // Loop through each pair of characters (each byte) in the string
            for (int i = 0; i < callsign_raw.Length; i += 2)
            {
                // Extract the byte (2 characters at a time) and convert to byte
                byte currentByte = Convert.ToByte(callsign_raw.Substring(i, 2), 16);

                // Perform a right shift by 1 bit
                byte shiftedByte = (byte)(currentByte >> 1);

                // Convert the shifted byte to its ASCII character if it's printable
                if (shiftedByte >= 32 && shiftedByte <= 126) // ASCII printable range
                {
                    char asciiChar = Convert.ToChar(shiftedByte);
                    source_callsign_ascii.Append(asciiChar); // Add the character to the StringBuilder
                    Console.WriteLine($"Original byte: 0x{currentByte:X2}, Shifted byte: 0x{shiftedByte:X2}, ASCII: {asciiChar}");
                }
                else
                {
                    Console.WriteLine($"Original byte: 0x{currentByte:X2}, Shifted byte: 0x{shiftedByte:X2}, Non-printable");
                }
            }

            return source_callsign_ascii.ToString(); // Return the full decoded string
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            rTBox_beacon_raw_data.Text = "";
            resetFields();
            this.Close();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {

            resetFields();

            tv_time_date.Text = DateTime.Now.ToString("yyyy-MM-dd, HH:mm:ss");

            try
            {
                string rawBeaconRx = rTBox_beacon_raw_data.Text;
                rawBeaconRx = rawBeaconRx.Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");

                rawBeaconRx = rawBeaconRx.ToUpper();

                var destination_callsign = callSignDecoder(rawBeaconRx.Substring(0, 12));

                var source_callsign = callSignDecoder(rawBeaconRx.Substring(14, 12));

                var control_bit = rawBeaconRx.Substring(28, 2);

                var protocol_bit = rawBeaconRx.Substring(30, 2);

                var beacon_type = rawBeaconRx.Substring(32, 2);

                if (beacon_type.Equals("b1") || beacon_type.Equals("B1"))
                {
                    getBeaconType1();

                    var head = rawBeaconRx.Substring(32, 2);
                    var type = rawBeaconRx.Substring(34, 2);

                    if (type == "00")
                    {
                        type = "TYPE_1";
                    }

                    var tim_day = Convert.ToInt16(rawBeaconRx.Substring(38, 2), 16);

                    var tim_hour = Convert.ToInt16(rawBeaconRx.Substring(40, 2), 16);

                    var batt_vtg = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(42, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(44, 2), 16)) / 1000.00;

                    var batt_curr = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(46, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(48, 2), 16)) / 100.00;

                    var batt_temp = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(50, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(52, 2), 16)) / 10.00;

                    var raw_current = Convert.ToInt16(rawBeaconRx.Substring(54, 2), 16) / 100.00;
                    var sol_tot_vtg = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(56, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(58, 2), 16)) / 1000.00;
                    var sol_tot_current = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(60, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(62, 2), 16)) / 100.00;

                    var ant_pannel_temp = logic_sum(Convert.ToInt16(rawBeaconRx.Substring(64, 2), 16)) / 10.00;
                    var bpb_temp = logic_sum(Convert.ToInt16(rawBeaconRx.Substring(66, 2), 16)) / 10.00;
                    var obc_temp = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(68, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(70, 2), 16)) / 10.00;

                    var sol_y_temp = Convert.ToInt16(rawBeaconRx.Substring(70, 2), 16) / 10.00;
                    var sol_y1_temp = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(72, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(74, 2), 16)) / 10.00;
                    var sol_z_temp = Convert.ToInt16(rawBeaconRx.Substring(74, 2), 16) / 10.00;
                    var sol_z1_temp = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(76, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(78, 2), 16)) / 10.00;
                    var sol_x_temp = Convert.ToInt16(rawBeaconRx.Substring(78, 2), 16) / 10.00;

                    var sol_p1_stat = (Convert.ToInt16(rawBeaconRx.Substring(80, 2), 16) & 128) / 128;
                    var sol_p2_stat = (Convert.ToInt16(rawBeaconRx.Substring(80, 2), 16) & 64) / 64;
                    var sol_p3_stat = (Convert.ToInt16(rawBeaconRx.Substring(80, 2), 16) & 32) / 32;
                    var sol_p4_stat = (Convert.ToInt16(rawBeaconRx.Substring(80, 2), 16) & 16) / 16;
                    var sol_p5_stat = (Convert.ToInt16(rawBeaconRx.Substring(80, 2), 16) & 8) / 8;

                    var msn_1_stat = (Convert.ToInt16(rawBeaconRx.Substring(80, 2), 16) & 4) / 4;
                    var msn_2_stat = (Convert.ToInt16(rawBeaconRx.Substring(80, 2), 16) & 2) / 2;
                    var msn_3_stat = (Convert.ToInt16(rawBeaconRx.Substring(80, 2), 16) & 1) / 1;

                    var ant_stat = logic_sum(Convert.ToInt16(rawBeaconRx.Substring(82, 2), 16) & 15);
                    var ul_stat = logic_sum(Convert.ToInt16(rawBeaconRx.Substring(82, 2), 16) & 15);

                    var oper_mode = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(84, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(86, 2), 16));
                    var obc_rst_count = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(86, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(88, 2), 16));

                    DateTime epochStart = new DateTime(1970, 1, 1); // Unix epoch start date
                    var date_time_a = Convert.ToInt16(rawBeaconRx.Substring(90, 2), 16) * Math.Pow(256, 3);
                    var date_time_b = Convert.ToInt16(rawBeaconRx.Substring(92, 2), 16) * Math.Pow(256, 2);
                    var date_time_c = Convert.ToInt16(rawBeaconRx.Substring(94, 2), 16) * Math.Pow(256, 1);
                    var date_time_d = Convert.ToInt16(rawBeaconRx.Substring(96, 2), 16) * Math.Pow(256, 0);
                    var date_time = date_time_a + date_time_b + date_time_c + date_time_d;
                    DateTime obc_last_reset = epochStart.AddSeconds(date_time);
                    obc_last_reset = obc_last_reset.AddHours(-5 / 24);

                    var check_sum1 = rawBeaconRx.Substring(88, 2);

                    label_call_sign_ans.Text = source_callsign;
                    label_head_ans.Text = head;
                    label_type_ans.Text = type;
                    label_time_day_ans.Text = tim_day.ToString();
                    label_time_hour_ans.Text = tim_hour.ToString();

                    label_1_group_box2_ans.Text = batt_vtg.ToString();
                    label_2_group_box2_ans.Text = batt_curr.ToString();
                    label_3_group_box2_ans.Text = batt_temp.ToString();
                    label_4_group_box2_ans.Text = raw_current.ToString();

                    label_1_group_box3_ans.Text = sol_tot_vtg.ToString();
                    label_2_group_box3_ans.Text = sol_tot_current.ToString();
                    label_3_group_box3_ans.Text = ant_pannel_temp.ToString();
                    label_4_group_box3_ans.Text = bpb_temp.ToString();
                    label_5_group_box3_ans.Text = obc_temp.ToString();

                    label_1_group_box4_ans.Text = sol_y_temp.ToString();
                    label_2_group_box4_ans.Text = sol_y1_temp.ToString();
                    label_3_group_box4_ans.Text = sol_z_temp.ToString();
                    label_4_group_box4_ans.Text = sol_z1_temp.ToString();
                    label_5_group_box4_ans.Text = sol_x_temp.ToString();

                    label_1_group_box5_ans.Text = sol_p1_stat.ToString();
                    label_2_group_box5_ans.Text = sol_p2_stat.ToString();
                    label_3_group_box5_ans.Text = sol_p3_stat.ToString();
                    label_4_group_box5_ans.Text = sol_p4_stat.ToString();
                    label_5_group_box5_ans.Text = sol_p5_stat.ToString();

                    label_1_group_box7_ans.Text = msn_1_stat.ToString();
                    label_2_group_box7_ans.Text = msn_2_stat.ToString();
                    label_3_group_box7_ans.Text = msn_3_stat.ToString();
                    label_4_group_box7_ans.Text = ant_stat.ToString();
                    label_5_group_box7_ans.Text = ul_stat.ToString();

                    label_1_group_box6_ans.Text = oper_mode.ToString();
                    label_2_group_box6_ans.Text = obc_rst_count.ToString();
                    label_4_group_box6_ans.Text = obc_last_reset.ToShortDateString();
                }
                else if (beacon_type.Equals("b2") || beacon_type.Equals("B2"))
                {
                    getBeaconType2();

                    var head = rawBeaconRx.Substring(32, 2);
                    var type = rawBeaconRx.Substring(34, 2);

                    if (type == "01")
                    {
                        type = "TYPE_2";
                    }

                    var tim_day = Convert.ToInt16(rawBeaconRx.Substring(36, 2), 16);

                    var sol_p1_vtg = Convert.ToInt16(rawBeaconRx.Substring(40, 2), 16) / 1000.00;
                    var sol_p2_vtg = Convert.ToInt16(rawBeaconRx.Substring(42, 2), 16) / 1000.00;
                    var sol_p3_vtg = Convert.ToInt16(rawBeaconRx.Substring(44, 2), 16) / 1000.00;
                    var sol_p4_vtg = Convert.ToInt16(rawBeaconRx.Substring(46, 2), 16) / 1000.00;
                    var sol_p5_vtg = Convert.ToInt16(rawBeaconRx.Substring(48, 2), 16) / 1000.00;

                    var sol_p1_current = Convert.ToInt16(rawBeaconRx.Substring(50, 2), 16) / 100.00;
                    var sol_p2_current = Convert.ToInt16(rawBeaconRx.Substring(52, 2), 16) / 100.00;
                    var sol_p3_current = Convert.ToInt16(rawBeaconRx.Substring(54, 2), 16) / 100.00;
                    var sol_p4_current = Convert.ToInt16(rawBeaconRx.Substring(56, 2), 16) / 100.00;
                    var sol_p5_current = Convert.ToInt16(rawBeaconRx.Substring(58, 2), 16) / 100.00;

                    var gyro_X = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(60, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(62, 2), 16)) / 100.00;
                    var gyro_Y = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(64, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(66, 2), 16)) / 100.00;
                    var gyro_Z = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(68, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(70, 2), 16)) / 100.00;

                    var acc_X = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(72, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(74, 2), 16)) / 100.00;
                    var acc_Y = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(76, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(78, 2), 16)) / 100.00;
                    var acc_Z = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(80, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(82, 2), 16)) / 100.00;

                    var mag_X = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(84, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(86, 2), 16)) / 100.00;
                    var mag_Y = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(88, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(90, 2), 16)) / 100.00;
                    var mag_Z = decode_16_byte(Convert.ToInt16(rawBeaconRx.Substring(92, 2), 16), Convert.ToInt16(rawBeaconRx.Substring(94, 2), 16)) / 100.00;

                    //var check_sum2 = rawBeaconRx.Substring(0, Math.Min(20, rawBeaconRx.Length));
                    var check_sum2 = rawBeaconRx.Substring(96, 2);

                    label_call_sign_ans.Text = source_callsign;
                    label_head_ans.Text = head;
                    label_type_ans.Text = type;
                    label_time_day_ans.Text = tim_day.ToString();

                    label_1_group_box2_ans.Text = sol_p1_vtg.ToString();
                    label_2_group_box2_ans.Text = sol_p2_vtg.ToString();
                    label_3_group_box2_ans.Text = sol_p3_vtg.ToString();
                    label_4_group_box2_ans.Text = sol_p4_vtg.ToString();
                    label_5_group_box2_ans.Text = sol_p5_vtg.ToString();

                    label_1_group_box3_ans.Text = sol_p1_current.ToString();
                    label_2_group_box3_ans.Text = sol_p2_current.ToString();
                    label_3_group_box3_ans.Text = sol_p3_current.ToString();
                    label_4_group_box3_ans.Text = sol_p4_current.ToString();
                    label_5_group_box3_ans.Text = sol_p5_current.ToString();

                    label_1_group_box4_ans.Text = gyro_X.ToString();
                    label_2_group_box4_ans.Text = gyro_Y.ToString();
                    label_3_group_box4_ans.Text = gyro_Z.ToString();

                    label_1_group_box5_ans.Text = acc_X.ToString();
                    label_2_group_box5_ans.Text = acc_Y.ToString();
                    label_3_group_box5_ans.Text = acc_Z.ToString();

                    label_1_group_box6_ans.Text = mag_X.ToString();
                    label_2_group_box6_ans.Text = mag_Y.ToString();
                    label_4_group_box6_ans.Text = mag_Z.ToString();
                }

                button_beacon_save.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                       $"Beacon Operation Invalid \n {ex.Message}",
                       "Please input valid raw data",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Exclamation,
                       MessageBoxDefaultButton.Button1
                );
            }
        }

        private int decode_16_byte(int higher_byte, int lower_byte)
        {
            var temp = (higher_byte * 256) + lower_byte;
            if (temp >= 32768)
            {
                temp = temp - 65536;
            }
            return temp;
        }

        private int logic_sum(int byte_data)
        {

            var temp = byte_data;
            if (temp >= 32768)
            {
                temp = temp - 65536;
            }

            return temp;
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

        private void button_beacon_save_Click(object sender, EventArgs e)
        {
            try
            {
                string rawBeaconRx = rTBox_beacon_raw_data.Text;
                string rawBeaconRx_temp = rawBeaconRx.Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");

                rawBeaconRx_temp = rawBeaconRx_temp.ToUpper();

                var beacon_type = rawBeaconRx_temp.Substring(32, 2);

                string result;

                result = InsertSpacesAfterEveryTwoBytes(rawBeaconRx_temp);

                Console.WriteLine(result);

                // Set the license context to NonCommercial or Commercial based on your use case
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;  // or LicenseContext.Commercial for commercial use

                savingBeaconRootPath = "../Storage/BEACON_DETAIL";
                Properties.Settings.Default.savePathBeacon = savingBeaconRootPath;
                Properties.Settings.Default.Save();

                BeaconRootPath = Properties.Settings.Default.savePathBeacon;

                savingBeaconRootPath = BeaconRootPath;

                if (!Directory.Exists(savingBeaconRootPath))
                {
                    Directory.CreateDirectory(savingBeaconRootPath);
                }

                string rawRxData = result;

                DateTime dt = DateTime.UtcNow;
                string present_date = dt.ToString("yyyy/MM/dd");
                string present_time = dt.ToString("HH:mm:ss");

                string folderPath = savingBeaconRootPath;
                string fileName;
                string absolutePath;

                fileName = "Beacon_details.xlsx";
                absolutePath = System.IO.Path.Combine(folderPath, fileName);

                if (!File.Exists(absolutePath))
                {
                    // Create a new Excel file if it doesn't exist
                    using (var package = new ExcelPackage(new FileInfo(absolutePath)))
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Raw_Data");
                        var worksheet_1 = package.Workbook.Worksheets.Add("Beacon_Type_1");
                        var worksheet_2 = package.Workbook.Worksheets.Add("Beacon_Type_2");
                        var hidden_worksheet_type_1 = package.Workbook.Worksheets.Add("Type_1_Backend");
                        var hidden_worksheet_type_2 = package.Workbook.Worksheets.Add("Type_2_Backend");

                        // Set headers in the first row
                        worksheet.Cells[1, 1].Value = "SN"; // Serial Number header
                        worksheet.Cells[1, 2].Value = "Date"; // Date header
                        worksheet.Cells[1, 3].Value = "Time"; // Time header
                        worksheet.Cells[1, 4].Value = "Raw Data"; // Raw Data header
                        worksheet.Cells[1, 5].Value = "Beacon Type"; // Beacon Type header

                        worksheet.Columns[1].AutoFit();
                        worksheet.Columns[2].AutoFit();
                        worksheet.Columns[3].AutoFit();
                        worksheet.Columns[4].AutoFit();
                        worksheet.Columns[5].AutoFit();

                        // Set headers in the first row
                        worksheet_1.Cells[1, 1].Value = "SN"; // Serial Number header
                        worksheet_1.Cells[1, 2].Value = "DATE"; // Date header
                        worksheet_1.Cells[1, 3].Value = "TIME"; // Time header
                        worksheet_1.Cells[1, 4].Value = "RAW_DATA"; // Date header
                        worksheet_1.Cells[1, 5].Value = "HEAD";
                        worksheet_1.Cells[1, 6].Value = "TYPE";
                        worksheet_1.Cells[1, 7].Value = "TIM_DAY";
                        worksheet_1.Cells[1, 8].Value = "TIM_HOUR";
                        worksheet_1.Cells[1, 9].Value = "BATT_V";
                        worksheet_1.Cells[1, 10].Value = "BATT_C";
                        worksheet_1.Cells[1, 11].Value = "BATT_T";
                        worksheet_1.Cells[1, 12].Value = "RAW_C";
                        worksheet_1.Cells[1, 13].Value = "SOL_TOT_V";
                        worksheet_1.Cells[1, 14].Value = "SOL_TOT_C";
                        worksheet_1.Cells[1, 15].Value = "ANT_P_T";
                        worksheet_1.Cells[1, 16].Value = "BPB_T";
                        worksheet_1.Cells[1, 17].Value = "OBC_T";
                        worksheet_1.Cells[1, 18].Value = "SOL_+Z_T";
                        worksheet_1.Cells[1, 19].Value = "SOL_+Y_T";
                        worksheet_1.Cells[1, 20].Value = "SOL_-Z_T";
                        worksheet_1.Cells[1, 21].Value = "SOL_-X_T";
                        worksheet_1.Cells[1, 22].Value = "SOL_-Y_T";
                        worksheet_1.Cells[1, 23].Value = "SOL_+Z_STAT";
                        worksheet_1.Cells[1, 24].Value = "SOL_+Y_STAT";
                        worksheet_1.Cells[1, 25].Value = "SOL_-Z_STAT";
                        worksheet_1.Cells[1, 26].Value = "SOL_-X_STAT";
                        worksheet_1.Cells[1, 27].Value = "SOL_-Y_STAT";
                        worksheet_1.Cells[1, 28].Value = "MSN1_STAT";
                        worksheet_1.Cells[1, 29].Value = "MSN2_STAT";
                        worksheet_1.Cells[1, 30].Value = "MSN3_STAT";
                        worksheet_1.Cells[1, 31].Value = "ANT_STAT";
                        worksheet_1.Cells[1, 32].Value = "UL_STAT";
                        worksheet_1.Cells[1, 33].Value = "OPER_MODE";
                        worksheet_1.Cells[1, 34].Value = "OBC_RST_COUNT";
                        worksheet_1.Cells[1, 35].Value = "LAST_RST";



                        // Set headers in the first row
                        worksheet_2.Cells[1, 1].Value = "SN"; // Serial Number header
                        worksheet_2.Cells[1, 2].Value = "DATE"; // Date header
                        worksheet_2.Cells[1, 3].Value = "TIME"; // Time header
                        worksheet_2.Cells[1, 4].Value = "RAW_DATA"; // Date header
                        worksheet_2.Cells[1, 5].Value = "HEAD";
                        worksheet_2.Cells[1, 6].Value = "TYPE";
                        worksheet_2.Cells[1, 7].Value = "TIM_DAY";
                        worksheet_2.Cells[1, 8].Value = "SOL_+Z_V";
                        worksheet_2.Cells[1, 9].Value = "SOL_+Y_V";
                        worksheet_2.Cells[1, 10].Value = "SOL_-Z_V";
                        worksheet_2.Cells[1, 11].Value = "SOL_-X_V";
                        worksheet_2.Cells[1, 12].Value = "SOL_-Y_V";
                        worksheet_2.Cells[1, 13].Value = "SOL_+Z_C";
                        worksheet_2.Cells[1, 14].Value = "SOL_+Y_C";
                        worksheet_2.Cells[1, 15].Value = "SOL_-Z_C";
                        worksheet_2.Cells[1, 16].Value = "SOL_-X_C";
                        worksheet_2.Cells[1, 17].Value = "SOL_-Y_C";
                        worksheet_2.Cells[1, 18].Value = "GYRO_X";
                        worksheet_2.Cells[1, 19].Value = "GYRO_Y";
                        worksheet_2.Cells[1, 20].Value = "GYRO_Z";
                        worksheet_2.Cells[1, 21].Value = "ACCEL_X";
                        worksheet_2.Cells[1, 22].Value = "ACCEL_Y";
                        worksheet_2.Cells[1, 23].Value = "ACCEL_Z";
                        worksheet_2.Cells[1, 24].Value = "MAG_X";
                        worksheet_2.Cells[1, 25].Value = "MAG_Y";
                        worksheet_2.Cells[1, 26].Value = "MAG_Z";



                        // Set headers in the first row
                        hidden_worksheet_type_1.Cells[1, 1].Value = "SN"; // Serial Number header
                        hidden_worksheet_type_1.Cells[1, 2].Value = "Raw Data"; // Raw Data header
                        hidden_worksheet_type_1.Cells[1, 3].Value = "Beacon Data"; // Raw Data header
                        hidden_worksheet_type_1.Cells[1, 4].Value = "Head"; // Beacon Type header
                        hidden_worksheet_type_1.Cells[1, 5].Value = "Type"; // Beacon Type header
                        hidden_worksheet_type_1.Cells[1, 6].Value = "Time Day";
                        hidden_worksheet_type_1.Cells[1, 7].Value = "Time Hour";

                        hidden_worksheet_type_1.Cells["H1:I1"].Merge = true;
                        hidden_worksheet_type_1.Cells["H1:I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_1.Cells["H1:I1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_1.Cells[1, 8].Value = "Battery Voltage";

                        hidden_worksheet_type_1.Cells["J1:K1"].Merge = true;
                        hidden_worksheet_type_1.Cells["J1:K1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_1.Cells["J1:K1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_1.Cells[1, 10].Value = "Battery Current";

                        hidden_worksheet_type_1.Cells["L1:M1"].Merge = true;
                        hidden_worksheet_type_1.Cells["L1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_1.Cells["L1:M1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_1.Cells[1, 12].Value = "Battery Temperature";

                        hidden_worksheet_type_1.Cells[1, 14].Value = "Raw Current";

                        hidden_worksheet_type_1.Cells["O1:P1"].Merge = true;
                        hidden_worksheet_type_1.Cells["O1:P1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_1.Cells["O1:P1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_1.Cells[1, 15].Value = "Solar Total Voltage";

                        hidden_worksheet_type_1.Cells["Q1:R1"].Merge = true;
                        hidden_worksheet_type_1.Cells["Q1:R1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_1.Cells["Q1:R1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_1.Cells[1, 17].Value = "Solar Total Current";

                        hidden_worksheet_type_1.Cells[1, 19].Value = "Antenna Panel Temperature";
                        hidden_worksheet_type_1.Cells[1, 20].Value = "Backplane Board Temperature";
                        hidden_worksheet_type_1.Cells[1, 21].Value = "OBC Temperature";
                        hidden_worksheet_type_1.Cells[1, 22].Value = "Solar_Panel_+Z Temperature";
                        hidden_worksheet_type_1.Cells[1, 23].Value = "Solar_Panel_+Y Temperature";
                        hidden_worksheet_type_1.Cells[1, 24].Value = "Solar_Panel_-Z Temperature";
                        hidden_worksheet_type_1.Cells[1, 25].Value = "Solar_Panel_-X Temperature";
                        hidden_worksheet_type_1.Cells[1, 26].Value = "Solar_Panel_-Y Temperature";
                        hidden_worksheet_type_1.Cells[1, 27].Value = "Solar_Panel_+Z Status";
                        hidden_worksheet_type_1.Cells[1, 28].Value = "Solar_Panel_+Y Status";
                        hidden_worksheet_type_1.Cells[1, 29].Value = "Solar_Panel_-Z Status";
                        hidden_worksheet_type_1.Cells[1, 30].Value = "Solar_Panel_-X Status";
                        hidden_worksheet_type_1.Cells[1, 31].Value = "Solar_Panel_-Y Status";
                        hidden_worksheet_type_1.Cells[1, 32].Value = "Mission_1 Status";
                        hidden_worksheet_type_1.Cells[1, 33].Value = "Mission_2 Status";
                        hidden_worksheet_type_1.Cells[1, 34].Value = "Mission_3 Status";
                        hidden_worksheet_type_1.Cells[1, 35].Value = "Antenna Status";
                        hidden_worksheet_type_1.Cells[1, 36].Value = "UL Status";
                        hidden_worksheet_type_1.Cells[1, 37].Value = "Operation Mode";

                        hidden_worksheet_type_1.Cells["AL1:AM1"].Merge = true;
                        hidden_worksheet_type_1.Cells["AL1:AM1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_1.Cells["AL1:AM1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_1.Cells[1, 38].Value = "OBC Reset";

                        hidden_worksheet_type_1.Cells["AN1:AQ1"].Merge = true;
                        hidden_worksheet_type_1.Cells["AN1:AQ1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_1.Cells["AN1:AQ1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_1.Cells[1, 40].Value = "Last Reset";

                        hidden_worksheet_type_1.Hidden = eWorkSheetHidden.Hidden;



                        // Set headers in the first row
                        hidden_worksheet_type_2.Cells[1, 1].Value = "SN"; // Serial Number header
                        hidden_worksheet_type_2.Cells[1, 2].Value = "Raw Data"; // Raw Data header
                        hidden_worksheet_type_2.Cells[1, 3].Value = "Beacon Data"; // Raw Data header
                        hidden_worksheet_type_2.Cells[1, 4].Value = "Head"; // Beacon Type header
                        hidden_worksheet_type_2.Cells[1, 5].Value = "Type"; // Beacon Type header
                        hidden_worksheet_type_2.Cells[1, 6].Value = "Time Day";
                        hidden_worksheet_type_2.Cells[1, 7].Value = "Solar_Panel_+Z Voltage";
                        hidden_worksheet_type_2.Cells[1, 8].Value = "Solar_Panel_+Y Voltage";
                        hidden_worksheet_type_2.Cells[1, 9].Value = "Solar_Panel_-Z Voltage";
                        hidden_worksheet_type_2.Cells[1, 10].Value = "Solar_Panel_X Voltage";
                        hidden_worksheet_type_2.Cells[1, 11].Value = "Solar_Panel_Y Voltage";
                        hidden_worksheet_type_2.Cells[1, 12].Value = "Solar_Panel_+Z Current";
                        hidden_worksheet_type_2.Cells[1, 13].Value = "Solar_Panel_+Y Current";
                        hidden_worksheet_type_2.Cells[1, 14].Value = "Solar_Panel_-Z Current";
                        hidden_worksheet_type_2.Cells[1, 15].Value = "Solar_Panel_-X Current";
                        hidden_worksheet_type_2.Cells[1, 16].Value = "Solar_Panel_-Y Current";

                        hidden_worksheet_type_2.Cells["Q1:R1"].Merge = true;
                        hidden_worksheet_type_2.Cells["Q1:R1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_2.Cells["Q1:R1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_2.Cells[1, 17].Value = "Gyro_X";

                        hidden_worksheet_type_2.Cells["S1:T1"].Merge = true;
                        hidden_worksheet_type_2.Cells["S1:T1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_2.Cells["S1:T1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_2.Cells[1, 19].Value = "Gyro_Y";

                        hidden_worksheet_type_2.Cells["U1:V1"].Merge = true;
                        hidden_worksheet_type_2.Cells["U1:V1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_2.Cells["U1:V1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_2.Cells[1, 21].Value = "Gyro_Z";

                        hidden_worksheet_type_2.Cells["W1:X1"].Merge = true;
                        hidden_worksheet_type_2.Cells["W1:X1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_2.Cells["W1:X1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_2.Cells[1, 23].Value = "Accel_X";

                        hidden_worksheet_type_2.Cells["Y1:Z1"].Merge = true;
                        hidden_worksheet_type_2.Cells["Y1:Z1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_2.Cells["Y1:Z1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_2.Cells[1, 25].Value = "Accel_Y";

                        hidden_worksheet_type_2.Cells["AA1:AB1"].Merge = true;
                        hidden_worksheet_type_2.Cells["AA1:AB1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_2.Cells["AA1:AB1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_2.Cells[1, 27].Value = "Accel_Z";

                        hidden_worksheet_type_2.Cells["AC1:AD1"].Merge = true;
                        hidden_worksheet_type_2.Cells["AC1:AD1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_2.Cells["AC1:AD1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_2.Cells[1, 29].Value = "MAG_X";

                        hidden_worksheet_type_2.Cells["AE1:AF1"].Merge = true;
                        hidden_worksheet_type_2.Cells["AE1:AF1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_2.Cells["AE1:AF1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_2.Cells[1, 31].Value = "MAG_Y";

                        hidden_worksheet_type_2.Cells["AG1:AH1"].Merge = true;
                        hidden_worksheet_type_2.Cells["AG1:AH1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_2.Cells["AG1:AH1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_2.Cells[1, 33].Value = "MAG_Z";

                        hidden_worksheet_type_2.Cells[1, 35].Value = "CRC";

                        hidden_worksheet_type_2.Hidden = eWorkSheetHidden.Hidden;


                        // Make the first row bold
                        worksheet.Row(1).Style.Font.Bold = true;
                        worksheet_1.Row(1).Style.Font.Bold = true;
                        worksheet_2.Row(1).Style.Font.Bold = true;
                        hidden_worksheet_type_1.Row(1).Style.Font.Bold = true;
                        hidden_worksheet_type_2.Row(1).Style.Font.Bold = true;

                        // Split the raw data into lines (assuming each line represents a row)
                        var rows = rawRxData.Split(new[] { "\n" }, StringSplitOptions.None);

                        int serialNumber = 1; // Starting serial number

                        // Loop through the rows of data and write to the Excel sheet
                        for (int i = 0; i < rows.Length; i++)
                        {
                            string serial = serialNumber.ToString("D2"); // Format serial as 01, 02, etc.
                                                                         // Get current Date and Time
                            string currentDate = DateTime.Now.ToString("yyyy-MM-dd"); // Current Date (format: yyyy-MM-dd)
                            string currentTime = DateTime.Now.ToString("HH:mm:ss"); // Current Time (format: HH:mm:ss

                            worksheet.Cells[i + 2, 1].Value = serial; // Write Serial Number
                            worksheet.Cells[i + 2, 2].Value = currentDate; // Date
                            worksheet.Cells[i + 2, 3].Value = currentTime; // Time
                            worksheet.Cells[i + 2, 4].Value = rows[i].Trim(); // Write Raw Data

                            // Insert the Beacon Type formula based on the row
                            string formula_raw_data_type = "=IF(ISBLANK(A" + (i + 2) + "),\" \",IF(MID(D" + (i + 2) + ",52,2)=\"00\",\"TYPE_1\", IF(MID(D" + (i + 2) + ",52,2)=\"01\", \"TYPE_2\", \"ERROR\")))";

                            worksheet.Cells[i + 2, 5].Formula = formula_raw_data_type; // Insert formula dynamically based on row number

                            serialNumber++;
                        }

                        //Beacon_parser
                        string formula_beacon_SN = "=IF(ISBLANK(D2),\"\",ROW()-1)";

                        //BEACON_TYPE_1 Sheet
                        string formula_beacon_head_1 = "=IF(ISBLANK(Type_1_Backend!D2),\"\",Type_1_Backend!D2)";
                        string formula_beacon_type_1 = "=IF(ISBLANK(Type_1_Backend!E2),\"\",IFERROR(IF(Type_1_Backend!E2 = \"00\",\"TYPE_1\"),\"ERROR\"))";
                        string formula_time_day_1 = "=IF(ISBLANK(Type_1_Backend!F2),\"\",IFERROR(HEX2DEC(Type_1_Backend!F2),\"ERROR\"))";
                        string formula_time_hour_1 = "=IF(ISBLANK(Type_1_Backend!G2),\"\",IFERROR(HEX2DEC(Type_1_Backend!G2),\"ERROR\"))";
                        string formula_batt_vtg = "=IF(ISBLANK(Type_1_Backend!H2),\"\",IFERROR(IF((HEX2DEC(Type_1_Backend!H2)*256 + HEX2DEC(Type_1_Backend!I2))>=32768, ((HEX2DEC(Type_1_Backend!H2)*256 + HEX2DEC(Type_1_Backend!I2)) - 65536)/1000, ((HEX2DEC(Type_1_Backend!H2)*256 + HEX2DEC(Type_1_Backend!I2)))/1000),\"ERROR\"))";
                        string formula_batt_current = "=IF(ISBLANK(Type_1_Backend!J2),\"\",IFERROR(IF((HEX2DEC(Type_1_Backend!J2)*256 + HEX2DEC(Type_1_Backend!K2))>=32768, ((HEX2DEC(Type_1_Backend!J2)*256 + HEX2DEC(Type_1_Backend!K2)) - 65536)/100, ((HEX2DEC(Type_1_Backend!J2)*256 + HEX2DEC(Type_1_Backend!K2)))/100),\"ERROR\"))";
                        string formnula_batt_temp = "=IF(ISBLANK(Type_1_Backend!L2),\"\",IFERROR(IF((HEX2DEC(Type_1_Backend!L2)*256 + HEX2DEC(Type_1_Backend!M2))>=32768, ((HEX2DEC(Type_1_Backend!L2)*256 + HEX2DEC(Type_1_Backend!M2)) - 65536)/10, ((HEX2DEC(Type_1_Backend!L2)*256 + HEX2DEC(Type_1_Backend!M2)))/10),\"ERROR\"))";
                        string formula_raw_c = "=IF(ISBLANK(Type_1_Backend!N2),\"\",IFERROR(HEX2DEC(Type_1_Backend!N2)/100,\"ERROR\"))";
                        string formula_sol_tot_vtg = "=IF(ISBLANK(Type_1_Backend!O2),\"\",IFERROR(IF((HEX2DEC(Type_1_Backend!O2)*256 + HEX2DEC(Type_1_Backend!P2))>=32768, ((HEX2DEC(Type_1_Backend!O2)*256 + HEX2DEC(Type_1_Backend!P2)) - 65536)/1000, ((HEX2DEC(Type_1_Backend!O2)*256 + HEX2DEC(Type_1_Backend!P2)))/1000),\"ERROR\"))";
                        string formula_sol_tot_curr = "=IF(ISBLANK(Type_1_Backend!Q2),\"\",IFERROR(IF((HEX2DEC(Type_1_Backend!Q2)*256 + HEX2DEC(Type_1_Backend!R2))>=32768, ((HEX2DEC(Type_1_Backend!Q2)*256 + HEX2DEC(Type_1_Backend!R2)) - 65536)/100, ((HEX2DEC(Type_1_Backend!Q2)*256 + HEX2DEC(Type_1_Backend!R2)))/100),\"ERROR\"))";
                        string formula_ant_p_temp = "=IF(ISBLANK(Type_1_Backend!S2),\"\",IFERROR(IF(HEX2DEC(Type_1_Backend!S2)>=32768, ((HEX2DEC(Type_1_Backend!S2)) - 65536)/10, (HEX2DEC(Type_1_Backend!S2))/10),\"ERROR\"))";
                        string formula_bpb_temp = "=IF(ISBLANK(Type_1_Backend!T2),\"\",IFERROR(IF(HEX2DEC(Type_1_Backend!T2)>=32768, ((HEX2DEC(Type_1_Backend!T2)) - 65536)/10, (HEX2DEC(Type_1_Backend!T2))/10),\"ERROR\"))";
                        string formula_obc_temp = "=IF(ISBLANK(Type_1_Backend!U2),\"\",IFERROR(IF((HEX2DEC(Type_1_Backend!U2)*256 + HEX2DEC(Type_1_Backend!V2))>=32768, ((HEX2DEC(Type_1_Backend!U2)*256 + HEX2DEC(Type_1_Backend!V2)) - 65536)/10, ((HEX2DEC(Type_1_Backend!U2)*256 + HEX2DEC(Type_1_Backend!V2)))/10),\"ERROR\"))";
                        string formula_sol_p1_temp = "=IF(ISBLANK(Type_1_Backend!V2),\"\",IFERROR(HEX2DEC(Type_1_Backend!V2)/10,\"ERROR\"))";
                        string formula_sol_p2_temp = "=IF(ISBLANK(Type_1_Backend!W2),\"\",IFERROR(IF((HEX2DEC(Type_1_Backend!W2)*256 + HEX2DEC(Type_1_Backend!X2))>=32768, ((HEX2DEC(Type_1_Backend!W2)*256 + HEX2DEC(Type_1_Backend!X2)) - 65536)/10, ((HEX2DEC(Type_1_Backend!W2)*256 + HEX2DEC(Type_1_Backend!X2)))/10),\"ERROR\"))";
                        string formula_sol_p3_temp = "=IF(ISBLANK(Type_1_Backend!X2),\"\",IFERROR(HEX2DEC(Type_1_Backend!X2)/10,\"ERROR\"))";
                        string formula_sol_p4_temp = "=IF(ISBLANK(Type_1_Backend!Y2),\"\",IFERROR(IF((HEX2DEC(Type_1_Backend!Y2)*256 + HEX2DEC(Type_1_Backend!Z2))>=32768, ((HEX2DEC(Type_1_Backend!Y2)*256 + HEX2DEC(Type_1_Backend!Z2)) - 65536)/10, ((HEX2DEC(Type_1_Backend!Y2)*256 + HEX2DEC(Type_1_Backend!Z2)))/10),\"ERROR\"))";
                        string formula_sol_p5_temp = "=IF(ISBLANK(Type_1_Backend!Z2),\"\",IFERROR(HEX2DEC(Type_1_Backend!Z2)/10,\"ERROR\"))";
                        string formula_sol_p1_stat = "=IF(ISBLANK(Type_1_Backend!AA2),\"\",IFERROR(IF(BITAND(HEX2DEC(Type_1_Backend!AA2),128)/128>=32768,BITAND(HEX2DEC(Type_1_Backend!AA2),128)/128 - 65536, BITAND(HEX2DEC(Type_1_Backend!AA2),128)/128),\"ERROR\"))";
                        string formula_sol_p2_stat = "=IF(ISBLANK(Type_1_Backend!AA2),\"\",IFERROR(IF(BITAND(HEX2DEC(Type_1_Backend!AA2),64)/64>=32768,BITAND(HEX2DEC(Type_1_Backend!AA2),64)/64 - 65536, BITAND(HEX2DEC(Type_1_Backend!AA2),64)/64),\"ERROR\"))";
                        string formula_sol_p3_stat = "=IF(ISBLANK(Type_1_Backend!AA2),\"\",IFERROR(IF(BITAND(HEX2DEC(Type_1_Backend!AA2),32)/32>=32768,BITAND(HEX2DEC(Type_1_Backend!AA2),32)/32 - 65536, BITAND(HEX2DEC(Type_1_Backend!AA2),32)/32),\"ERROR\"))";
                        string formula_sol_p4_stat = "=IF(ISBLANK(Type_1_Backend!AA2),\"\",IFERROR(IF(BITAND(HEX2DEC(Type_1_Backend!AA2),16)/16>=32768,BITAND(HEX2DEC(Type_1_Backend!AA2),16)/16 - 65536, BITAND(HEX2DEC(Type_1_Backend!AA2),16)/16),\"ERROR\"))";
                        string formula_sol_p5_stat = "=IF(ISBLANK(Type_1_Backend!AA2),\"\",IFERROR(IF(BITAND(HEX2DEC(Type_1_Backend!AA2),8)/8>=32768,BITAND(HEX2DEC(Type_1_Backend!AA2),8)/8 - 65536, BITAND(HEX2DEC(Type_1_Backend!AA2),8)/8),\"ERROR\"))";
                        string formula_msn1_stat = "=IF(ISBLANK(Type_1_Backend!AA2),\"\",IFERROR(IF(BITAND(HEX2DEC(Type_1_Backend!AA2),4)/4>=32768,BITAND(HEX2DEC(Type_1_Backend!AA2),4)/4 - 65536, BITAND(HEX2DEC(Type_1_Backend!AA2),4)/4),\"ERROR\"))";
                        string formula_msn2_stat = "=IF(ISBLANK(Type_1_Backend!AA2),\"\",IFERROR(IF(BITAND(HEX2DEC(Type_1_Backend!AA2),2)/2>=32768,BITAND(HEX2DEC(Type_1_Backend!AA2),2)/2 - 65536, BITAND(HEX2DEC(Type_1_Backend!AA2),2)/2),\"ERROR\"))";
                        string formula_msn3_stat = "=IF(ISBLANK(Type_1_Backend!AA2),\"\",IFERROR(IF(BITAND(HEX2DEC(Type_1_Backend!AA2),1)/1>=32768,BITAND(HEX2DEC(Type_1_Backend!AA2),1)/1 - 65536, BITAND(HEX2DEC(Type_1_Backend!AA2),1)/1),\"ERROR\"))";
                        string formula_ant_stat = "=IF(ISBLANK(Type_1_Backend!AI2),\"\",IFERROR(IF(BITAND(ROUND(HEX2DEC(Type_1_Backend!AI2),16),15)>=32768, BITAND(ROUND(HEX2DEC(Type_1_Backend!AI2),16),15)-65536, BITAND(ROUND(HEX2DEC(Type_1_Backend!AI2),16),15)),\"ERROR\"))";
                        string formula_ul_stat = "=IF(ISBLANK(Type_1_Backend!AI2),\"\",IFERROR(IF(BITAND(HEX2DEC(Type_1_Backend!AI2),15)>=32768, (BITAND(HEX2DEC(Type_1_Backend!AI2),15))-65536, (BITAND(HEX2DEC(Type_1_Backend!AI2),15))),\"ERROR\"))";
                        string formula_opera_mode = "=IF(ISBLANK(Type_1_Backend!AK2),\"\",IFERROR(IF((HEX2DEC(Type_1_Backend!AK2)*256 + HEX2DEC(Type_1_Backend!AL2))>=32768, ((HEX2DEC(Type_1_Backend!AK2)*256 + HEX2DEC(Type_1_Backend!AL2)) - 65536)/10, ((HEX2DEC(Type_1_Backend!AK2)*256 + HEX2DEC(Type_1_Backend!AL2)))/10),\"ERROR\"))";
                        string formula_obc_rst = "=IF(ISBLANK(Type_1_Backend!AL2),\"\",IFERROR(IF((HEX2DEC(Type_1_Backend!AL2)*256 + HEX2DEC(Type_1_Backend!AM2))>=32768, ((HEX2DEC(Type_1_Backend!AL2)*256 + HEX2DEC(Type_1_Backend!AM2)) - 65536), ((HEX2DEC(Type_1_Backend!AL2)*256 + HEX2DEC(Type_1_Backend!AM2)))),\"ERROR\"))";
                        string formula_last_rst = "=IF(ISBLANK(Type_1_Backend!AN2),\"\",IFERROR(((((HEX2DEC(Type_1_Backend!AN2)*256^3 + HEX2DEC(Type_1_Backend!AO2)*256^2 + HEX2DEC(Type_1_Backend!AP2)*256^1 + HEX2DEC(Type_1_Backend!AQ2)*256)    /86400)+DATE(1970,1,1))+(-5/24)),\"ERROR\"))";


                        worksheet_1.Cells[2, 1].Formula = formula_beacon_SN;
                        worksheet_1.Cells[2, 5].Formula = formula_beacon_head_1;
                        worksheet_1.Cells[2, 6].Formula = formula_beacon_type_1;
                        worksheet_1.Cells[2, 7].Formula = formula_time_day_1;
                        worksheet_1.Cells[2, 8].Formula = formula_time_hour_1;
                        worksheet_1.Cells[2, 9].Formula = formula_batt_vtg;
                        worksheet_1.Cells[2, 10].Formula = formula_batt_current;
                        worksheet_1.Cells[2, 11].Formula = formnula_batt_temp;
                        worksheet_1.Cells[2, 12].Formula = formula_raw_c;
                        worksheet_1.Cells[2, 13].Formula = formula_sol_tot_vtg;
                        worksheet_1.Cells[2, 14].Formula = formula_sol_tot_curr;
                        worksheet_1.Cells[2, 15].Formula = formula_ant_p_temp;
                        worksheet_1.Cells[2, 16].Formula = formula_bpb_temp;
                        worksheet_1.Cells[2, 17].Formula = formula_obc_temp;
                        worksheet_1.Cells[2, 18].Formula = formula_sol_p1_temp;
                        worksheet_1.Cells[2, 19].Formula = formula_sol_p2_temp;
                        worksheet_1.Cells[2, 20].Formula = formula_sol_p3_temp;
                        worksheet_1.Cells[2, 21].Formula = formula_sol_p4_temp;
                        worksheet_1.Cells[2, 22].Formula = formula_sol_p5_temp;
                        worksheet_1.Cells[2, 23].Formula = formula_sol_p1_stat;
                        worksheet_1.Cells[2, 24].Formula = formula_sol_p2_stat;
                        worksheet_1.Cells[2, 25].Formula = formula_sol_p3_stat;
                        worksheet_1.Cells[2, 26].Formula = formula_sol_p4_stat;
                        worksheet_1.Cells[2, 27].Formula = formula_sol_p5_stat;
                        worksheet_1.Cells[2, 28].Formula = formula_msn1_stat;
                        worksheet_1.Cells[2, 29].Formula = formula_msn2_stat;
                        worksheet_1.Cells[2, 30].Formula = formula_msn3_stat;
                        worksheet_1.Cells[2, 31].Formula = formula_ant_stat;
                        worksheet_1.Cells[2, 32].Formula = formula_ul_stat;
                        worksheet_1.Cells[2, 33].Formula = formula_opera_mode;
                        worksheet_1.Cells[2, 34].Formula = formula_obc_rst;
                        worksheet_1.Cells[2, 35].Style.Numberformat.Format = "dd/mm/yyyy";
                        worksheet_1.Cells[2, 35].Formula = formula_last_rst;


                        // BEACON_TYPE_2 Sheet
                        string formula_beacon_head_2 = "=IF(ISBLANK(Type_2_Backend!D2),\"\",Type_2_Backend!D2)";
                        string formula_beacon_type_2 = "=IF(ISBLANK(Type_2_Backend!E2),\"\",IFERROR(IF(Type_2_Backend!E2 = \"01\",\"TYPE_2\"),\"ERROR\"))";
                        string formula_time_day_2 = "=IF(ISBLANK(Type_2_Backend!F2),\"\",IFERROR(HEX2DEC(Type_2_Backend!F2),\"ERROR\"))";
                        string formula_sol_p1_vtg = "=IF(ISBLANK(Type_2_Backend!G2),\"\",IFERROR(HEX2DEC(Type_2_Backend!G2)/1000,\"ERROR\"))";
                        string formula_sol_p2_vtg = "=IF(ISBLANK(Type_2_Backend!H2),\"\",IFERROR(HEX2DEC(Type_2_Backend!H2)/1000,\"ERROR\"))";
                        string formula_sol_p3_vtg = "=IF(ISBLANK(Type_2_Backend!I2),\"\",IFERROR(HEX2DEC(Type_2_Backend!I2)/1000,\"ERROR\"))";
                        string formula_sol_p4_vtg = "=IF(ISBLANK(Type_2_Backend!J2),\"\",IFERROR(HEX2DEC(Type_2_Backend!J2)/1000,\"ERROR\"))";
                        string formula_sol_p5_vtg = "=IF(ISBLANK(Type_2_Backend!K2),\"\",IFERROR(HEX2DEC(Type_2_Backend!K2)/1000,\"ERROR\"))";
                        string formula_sol_p1_curr = "=IF(ISBLANK(Type_2_Backend!L2),\"\",IFERROR(HEX2DEC(Type_2_Backend!L2)/100,\"ERROR\"))";
                        string formula_sol_p2_curr = "=IF(ISBLANK(Type_2_Backend!M2),\"\",IFERROR(HEX2DEC(Type_2_Backend!M2)/100,\"ERROR\"))";
                        string formula_sol_p3_curr = "=IF(ISBLANK(Type_2_Backend!N2),\"\",IFERROR(HEX2DEC(Type_2_Backend!N2)/100,\"ERROR\"))";
                        string formula_sol_p4_curr = "=IF(ISBLANK(Type_2_Backend!O2),\"\",IFERROR(HEX2DEC(Type_2_Backend!O2)/100,\"ERROR\"))";
                        string formula_sol_p5_curr = "=IF(ISBLANK(Type_2_Backend!P2),\"\",IFERROR(HEX2DEC(Type_2_Backend!P2)/100,\"ERROR\"))";
                        string formula_gyro_x = "=IF(ISBLANK(Type_2_Backend!Q2),\"\",IFERROR(IF((HEX2DEC(Type_2_Backend!Q2)*256 + HEX2DEC(Type_2_Backend!R2))>=32768, ((HEX2DEC(Type_2_Backend!Q2)*256 + HEX2DEC(Type_2_Backend!R2)) - 65536)/100, ((HEX2DEC(Type_2_Backend!Q2)*256 + HEX2DEC(Type_2_Backend!R2))/100)),\"ERROR\"))";
                        string formula_gyro_y = "=IF(ISBLANK(Type_2_Backend!S2),\"\",IFERROR(IF((HEX2DEC(Type_2_Backend!S2)*256 + HEX2DEC(Type_2_Backend!T2))>=32768, ((HEX2DEC(Type_2_Backend!S2)*256 + HEX2DEC(Type_2_Backend!T2)) - 65536)/100, ((HEX2DEC(Type_2_Backend!S2)*256 + HEX2DEC(Type_2_Backend!T2))/100)),\"ERROR\"))";
                        string formula_gyro_z = "=IF(ISBLANK(Type_2_Backend!U2),\"\",IFERROR(IF((HEX2DEC(Type_2_Backend!U2)*256 + HEX2DEC(Type_2_Backend!V2))>=32768, ((HEX2DEC(Type_2_Backend!U2)*256 + HEX2DEC(Type_2_Backend!V2)) - 65536)/100, ((HEX2DEC(Type_2_Backend!U2)*256 + HEX2DEC(Type_2_Backend!V2))/100)),\"ERROR\"))";
                        string formula_accel_x = "=IF(ISBLANK(Type_2_Backend!W2),\"\",IFERROR(IF((HEX2DEC(Type_2_Backend!W2)*256 + HEX2DEC(Type_2_Backend!X2))>=32768, ((HEX2DEC(Type_2_Backend!W2)*256 + HEX2DEC(Type_2_Backend!X2)) - 65536)/100, ((HEX2DEC(Type_2_Backend!W2)*256 + HEX2DEC(Type_2_Backend!X2))/100)),\"ERROR\"))";
                        string formula_accel_y = "=IF(ISBLANK(Type_2_Backend!Y2),\"\",IFERROR(IF((HEX2DEC(Type_2_Backend!Y2)*256 + HEX2DEC(Type_2_Backend!Z2))>=32768, ((HEX2DEC(Type_2_Backend!Y2)*256 + HEX2DEC(Type_2_Backend!Z2)) - 65536)/100, ((HEX2DEC(Type_2_Backend!Y2)*256 + HEX2DEC(Type_2_Backend!Z2))/100)),\"ERROR\"))";
                        string formula_accel_z = "=IF(ISBLANK(Type_2_Backend!AA2),\"\",IFERROR(IF((HEX2DEC(Type_2_Backend!AA2)*256 + HEX2DEC(Type_2_Backend!AB2))>=32768, ((HEX2DEC(Type_2_Backend!AA2)*256 + HEX2DEC(Type_2_Backend!AB2)) - 65536)/100, ((HEX2DEC(Type_2_Backend!AA2)*256 + HEX2DEC(Type_2_Backend!AB2))/100)),\"ERROR\"))";
                        string formula_mag_x = "=IF(ISBLANK(Type_2_Backend!AC2),\"\",IFERROR(IF((HEX2DEC(Type_2_Backend!AC2)*256 + HEX2DEC(Type_2_Backend!AD2))>=32768, ((HEX2DEC(Type_2_Backend!AC2)*256 + HEX2DEC(Type_2_Backend!AD2)) - 65536)/100, ((HEX2DEC(Type_2_Backend!AC2)*256 + HEX2DEC(Type_2_Backend!AD2))/100)),\"ERROR\"))";
                        string formula_mag_y = "=IF(ISBLANK(Type_2_Backend!AE2),\"\",IFERROR(IF((HEX2DEC(Type_2_Backend!AE2)*256 + HEX2DEC(Type_2_Backend!AF2))>=32768, ((HEX2DEC(Type_2_Backend!AE2)*256 + HEX2DEC(Type_2_Backend!AF2)) - 65536)/100, ((HEX2DEC(Type_2_Backend!AE2)*256 + HEX2DEC(Type_2_Backend!AF2))/100)),\"ERROR\"))";
                        string formula_mag_z = "=IF(ISBLANK(Type_2_Backend!AG2),\"\",IFERROR(IF((HEX2DEC(Type_2_Backend!AG2)*256 + HEX2DEC(Type_2_Backend!AH2))>=32768, ((HEX2DEC(Type_2_Backend!AG2)*256 + HEX2DEC(Type_2_Backend!AH2)) - 65536)/100, ((HEX2DEC(Type_2_Backend!AG2)*256 + HEX2DEC(Type_2_Backend!AH2))/100)),\"ERROR\"))";

                        worksheet_2.Cells[2, 1].Formula = formula_beacon_SN;
                        worksheet_2.Cells[2, 5].Formula = formula_beacon_head_2;
                        worksheet_2.Cells[2, 6].Formula = formula_beacon_type_2;
                        worksheet_2.Cells[2, 7].Formula = formula_time_day_2;
                        worksheet_2.Cells[2, 8].Formula = formula_sol_p1_vtg;
                        worksheet_2.Cells[2, 9].Formula = formula_sol_p2_vtg;
                        worksheet_2.Cells[2, 10].Formula = formula_sol_p3_vtg;
                        worksheet_2.Cells[2, 11].Formula = formula_sol_p4_vtg;
                        worksheet_2.Cells[2, 12].Formula = formula_sol_p5_vtg;
                        worksheet_2.Cells[2, 13].Formula = formula_sol_p1_curr;
                        worksheet_2.Cells[2, 14].Formula = formula_sol_p2_curr;
                        worksheet_2.Cells[2, 15].Formula = formula_sol_p3_curr;
                        worksheet_2.Cells[2, 16].Formula = formula_sol_p4_curr;
                        worksheet_2.Cells[2, 17].Formula = formula_sol_p5_curr;
                        worksheet_2.Cells[2, 18].Formula = formula_gyro_x;
                        worksheet_2.Cells[2, 19].Formula = formula_gyro_y;
                        worksheet_2.Cells[2, 20].Formula = formula_gyro_z;
                        worksheet_2.Cells[2, 21].Formula = formula_accel_x;
                        worksheet_2.Cells[2, 22].Formula = formula_accel_y;
                        worksheet_2.Cells[2, 23].Formula = formula_accel_z;
                        worksheet_2.Cells[2, 24].Formula = formula_mag_x;
                        worksheet_2.Cells[2, 25].Formula = formula_mag_y;
                        worksheet_2.Cells[2, 26].Formula = formula_mag_z;


                        //Beacon_hidden_data_parser
                        string formula_hidden_SN = "=IF(ISBLANK(B2),\"\",ROW()-1)";
                        string formula_hidden_beacon_data = "=IF(ISBLANK(B2),\"\",IFERROR(TRIM(MID(B2,FIND(\"40 40 40 40 40 40 E0 72 9C 64 A6 92 40 61 03 F0 \",B2)+48,LEN(B2))),\"ERROR\"))";
                        string formula_hidden_beacon_head = "=IF(ISBLANK(C2),\"\",IFERROR(UPPER(MID(C2,1,2)),\"ERROR\"))";
                        string formula_hidden_beacon_type = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,4,2),\"ERROR\"))";

                        //Beacon_type_1_backend
                        string formula_raw_data_1 = "=IF(ISBLANK(Beacon_Type_1!D2),\"\",Beacon_Type_1!D2)";
                        string formula_raw_time_day_1 = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,10,2),\"ERROR\"))";
                        string formula_raw_time_hour_1 = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,13,2),\"ERROR\"))";
                        string formula_raw_batt_vtg_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,16,2),\"ERROR\"))";
                        string formula_raw_batt_vtg_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,19,2),\"ERROR\"))";
                        string formula_raw_batt_curr_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,22,2),\"ERROR\"))";
                        string formula_raw_batt_curr_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,25,2),\"ERROR\"))";
                        string formula_raw_batt_temp_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,28,2),\"ERROR\"))";
                        string formula_raw_batt_temp_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,31,2),\"ERROR\"))";
                        string formula_raw_raw_current = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,34,2),\"ERROR\"))";
                        string formula_raw_sol_total_vtg_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,37,2),\"ERROR\"))";
                        string formula_raw_sol_tatal_vtg_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,40,2),\"ERROR\"))";
                        string formula_raw_sol_total_curr_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,43,2),\"ERROR\"))";
                        string formula_raw_sol_tatal_curr_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,46,2),\"ERROR\"))";
                        string formula_raw_ant_p_temp = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,49,2),\"ERROR\"))";
                        string formula_raw_bpb_p_temp = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,52,2),\"ERROR\"))";
                        string formula_raw_obc_p_temp = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,55,2),\"ERROR\"))";
                        string formula_raw_sol_p1_temp = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,58,2),\"ERROR\"))";
                        string formula_raw_sol_p2_temp = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,61,2),\"ERROR\"))";
                        string formula_raw_sol_p3_temp = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,64,2),\"ERROR\"))";
                        string formula_raw_sol_p4_temp = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,67,2),\"ERROR\"))";
                        string formula_raw_sol_p5_temp = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,70,2),\"ERROR\"))";
                        string formula_sol_msn_status = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,73,2),\"ERROR\"))";
                        string formula_antenna_ul_status = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,76,2),\"ERROR\"))";
                        string formula_operation_mode = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,79,2),\"ERROR\"))";
                        string formula_obc_rst_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,82,2),\"ERROR\"))";
                        string formula_obc_rst_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,85,2),\"ERROR\"))";
                        string formula_last_rst_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,88,2),\"ERROR\"))";
                        string formula_last_rst_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,91,2),\"ERROR\"))";
                        string formula_last_rst_c = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,94,2),\"ERROR\"))";
                        string formula_last_rst_d = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,97,2),\"ERROR\"))";

                        hidden_worksheet_type_1.Cells[2, 1].Formula = formula_hidden_SN;
                        hidden_worksheet_type_1.Cells[2, 2].Formula = formula_raw_data_1;
                        hidden_worksheet_type_1.Cells[2, 3].Formula = formula_hidden_beacon_data;
                        hidden_worksheet_type_1.Cells[2, 4].Formula = formula_hidden_beacon_head;
                        hidden_worksheet_type_1.Cells[2, 5].Formula = formula_hidden_beacon_type;
                        hidden_worksheet_type_1.Cells[2, 6].Formula = formula_raw_time_day_1;
                        hidden_worksheet_type_1.Cells[2, 7].Formula = formula_raw_time_hour_1;
                        hidden_worksheet_type_1.Cells[2, 8].Formula = formula_raw_batt_vtg_a;
                        hidden_worksheet_type_1.Cells[2, 9].Formula = formula_raw_batt_vtg_b;
                        hidden_worksheet_type_1.Cells[2, 10].Formula = formula_raw_batt_curr_a;
                        hidden_worksheet_type_1.Cells[2, 11].Formula = formula_raw_batt_curr_b;
                        hidden_worksheet_type_1.Cells[2, 12].Formula = formula_raw_batt_temp_a;
                        hidden_worksheet_type_1.Cells[2, 13].Formula = formula_raw_batt_temp_b;
                        hidden_worksheet_type_1.Cells[2, 14].Formula = formula_raw_raw_current;
                        hidden_worksheet_type_1.Cells[2, 15].Formula = formula_raw_sol_total_vtg_a;
                        hidden_worksheet_type_1.Cells[2, 16].Formula = formula_raw_sol_tatal_vtg_b;
                        hidden_worksheet_type_1.Cells[2, 17].Formula = formula_raw_sol_total_curr_a;
                        hidden_worksheet_type_1.Cells[2, 18].Formula = formula_raw_sol_tatal_curr_b;
                        hidden_worksheet_type_1.Cells[2, 19].Formula = formula_raw_ant_p_temp;
                        hidden_worksheet_type_1.Cells[2, 20].Formula = formula_raw_bpb_p_temp;
                        hidden_worksheet_type_1.Cells[2, 21].Formula = formula_raw_obc_p_temp;
                        hidden_worksheet_type_1.Cells[2, 22].Formula = formula_raw_sol_p1_temp;
                        hidden_worksheet_type_1.Cells[2, 23].Formula = formula_raw_sol_p2_temp;
                        hidden_worksheet_type_1.Cells[2, 24].Formula = formula_raw_sol_p3_temp;
                        hidden_worksheet_type_1.Cells[2, 25].Formula = formula_raw_sol_p4_temp;
                        hidden_worksheet_type_1.Cells[2, 26].Formula = formula_raw_sol_p5_temp;
                        // Merge cells from column 27 (AA) to column 34 (AH) in row 2
                        hidden_worksheet_type_1.Cells[2, 27, 2, 34].Merge = true;
                        hidden_worksheet_type_1.Cells[2, 27, 2, 34].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_1.Cells[2, 27, 2, 34].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_1.Cells[2, 27].Formula = formula_sol_msn_status;
                        // Merge cells from column 35 (AI) to column 36 (AJ) in row 2
                        hidden_worksheet_type_1.Cells[2, 35, 2, 36].Merge = true;
                        hidden_worksheet_type_1.Cells[2, 35, 2, 36].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hidden_worksheet_type_1.Cells[2, 35, 2, 36].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        hidden_worksheet_type_1.Cells[2, 35].Formula = formula_antenna_ul_status;
                        hidden_worksheet_type_1.Cells[2, 37].Formula = formula_operation_mode;
                        hidden_worksheet_type_1.Cells[2, 38].Formula = formula_obc_rst_a;
                        hidden_worksheet_type_1.Cells[2, 39].Formula = formula_obc_rst_b;
                        hidden_worksheet_type_1.Cells[2, 40].Formula = formula_last_rst_a;
                        hidden_worksheet_type_1.Cells[2, 41].Formula = formula_last_rst_b;
                        hidden_worksheet_type_1.Cells[2, 42].Formula = formula_last_rst_c;
                        hidden_worksheet_type_1.Cells[2, 43].Formula = formula_last_rst_d;


                        //Beacon_type_2_backend
                        string formula_raw_data_2 = "=IF(ISBLANK(Beacon_Type_2!D2),\"\",Beacon_Type_2!D2)";
                        string formula_raw_time_day_2 = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,7,2),\"ERROR\"))";
                        string formula_raw_sol_p1_vtg = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,13,2),\"ERROR\"))";
                        string formula_raw_sol_p2_vtg = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,16,2),\"ERROR\"))";
                        string formula_raw_sol_p3_vtg = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,19,2),\"ERROR\"))";
                        string formula_raw_sol_p4_vtg = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,22,2),\"ERROR\"))";
                        string formula_raw_sol_p5_vtg = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,25,2),\"ERROR\"))";
                        string formula_raw_sol_p1_curr = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,28,2),\"ERROR\"))";
                        string formula_raw_sol_p2_curr = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,31,2),\"ERROR\"))";
                        string formula_raw_sol_p3_curr = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,34,2),\"ERROR\"))";
                        string formula_raw_sol_p4_curr = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,37,2),\"ERROR\"))";
                        string formula_raw_sol_p5_curr = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,40,2),\"ERROR\"))";
                        string formula_raw_gyro_x_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,43,2),\"ERROR\"))";
                        string formula_raw_gyro_x_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,46,2),\"ERROR\"))";
                        string formula_raw_gyro_y_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,49,2),\"ERROR\"))";
                        string formula_raw_gyro_y_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,52,2),\"ERROR\"))";
                        string formula_raw_gyro_z_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,55,2),\"ERROR\"))";
                        string formula_raw_gyro_z_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,58,2),\"ERROR\"))";
                        string formula_raw_accel_x_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,61,2),\"ERROR\"))";
                        string formula_raw_accel_x_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,64,2),\"ERROR\"))";
                        string formula_raw_accel_y_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,67,2),\"ERROR\"))";
                        string formula_raw_accel_y_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,70,2),\"ERROR\"))";
                        string formula_raw_accel_z_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,73,2),\"ERROR\"))";
                        string formula_raw_accel_z_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,76,2),\"ERROR\"))";
                        string formula_raw_mag_x_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,79,2),\"ERROR\"))";
                        string formula_raw_mag_x_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,82,2),\"ERROR\"))";
                        string formula_raw_mag_y_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,85,2),\"ERROR\"))";
                        string formula_raw_mag_y_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,88,2),\"ERROR\"))";
                        string formula_raw_mag_z_a = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,91,2),\"ERROR\"))";
                        string formula_raw_mag_z_b = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,94,2),\"ERROR\"))";
                        string formula_crc = "=IF(ISBLANK(C2),\"\",IFERROR(MID(C2,97,2),\"ERROR\"))";

                        hidden_worksheet_type_2.Cells[2, 1].Formula = formula_hidden_SN;
                        hidden_worksheet_type_2.Cells[2, 2].Formula = formula_raw_data_2;
                        hidden_worksheet_type_2.Cells[2, 3].Formula = formula_hidden_beacon_data;
                        hidden_worksheet_type_2.Cells[2, 4].Formula = formula_hidden_beacon_head;
                        hidden_worksheet_type_2.Cells[2, 5].Formula = formula_hidden_beacon_type;
                        hidden_worksheet_type_2.Cells[2, 6].Formula = formula_raw_time_day_2;
                        hidden_worksheet_type_2.Cells[2, 7].Formula = formula_raw_sol_p1_vtg;
                        hidden_worksheet_type_2.Cells[2, 8].Formula = formula_raw_sol_p2_vtg;
                        hidden_worksheet_type_2.Cells[2, 9].Formula = formula_raw_sol_p3_vtg;
                        hidden_worksheet_type_2.Cells[2, 10].Formula = formula_raw_sol_p4_vtg;
                        hidden_worksheet_type_2.Cells[2, 11].Formula = formula_raw_sol_p5_vtg;
                        hidden_worksheet_type_2.Cells[2, 12].Formula = formula_raw_sol_p1_curr;
                        hidden_worksheet_type_2.Cells[2, 13].Formula = formula_raw_sol_p2_curr;
                        hidden_worksheet_type_2.Cells[2, 14].Formula = formula_raw_sol_p3_curr;
                        hidden_worksheet_type_2.Cells[2, 15].Formula = formula_raw_sol_p4_curr;
                        hidden_worksheet_type_2.Cells[2, 16].Formula = formula_raw_sol_p5_curr;
                        hidden_worksheet_type_2.Cells[2, 17].Formula = formula_raw_gyro_x_a;
                        hidden_worksheet_type_2.Cells[2, 18].Formula = formula_raw_gyro_x_b;
                        hidden_worksheet_type_2.Cells[2, 19].Formula = formula_raw_gyro_y_a;
                        hidden_worksheet_type_2.Cells[2, 20].Formula = formula_raw_gyro_y_b;
                        hidden_worksheet_type_2.Cells[2, 21].Formula = formula_raw_gyro_z_a;
                        hidden_worksheet_type_2.Cells[2, 22].Formula = formula_raw_gyro_z_b;
                        hidden_worksheet_type_2.Cells[2, 23].Formula = formula_raw_accel_x_a;
                        hidden_worksheet_type_2.Cells[2, 24].Formula = formula_raw_accel_x_b;
                        hidden_worksheet_type_2.Cells[2, 25].Formula = formula_raw_accel_y_a;
                        hidden_worksheet_type_2.Cells[2, 26].Formula = formula_raw_accel_y_b;
                        hidden_worksheet_type_2.Cells[2, 27].Formula = formula_raw_accel_z_a;
                        hidden_worksheet_type_2.Cells[2, 28].Formula = formula_raw_accel_z_b;
                        hidden_worksheet_type_2.Cells[2, 29].Formula = formula_raw_mag_x_a;
                        hidden_worksheet_type_2.Cells[2, 30].Formula = formula_raw_mag_x_b;
                        hidden_worksheet_type_2.Cells[2, 31].Formula = formula_raw_mag_y_a;
                        hidden_worksheet_type_2.Cells[2, 32].Formula = formula_raw_mag_y_b;
                        hidden_worksheet_type_2.Cells[2, 33].Formula = formula_raw_mag_z_a;
                        hidden_worksheet_type_2.Cells[2, 34].Formula = formula_raw_mag_z_b;
                        hidden_worksheet_type_2.Cells[2, 35].Formula = formula_crc;

                        insert_border_to_excel(worksheet);
                        insert_border_to_excel(worksheet_1);
                        insert_border_to_excel(worksheet_2);
                        insert_border_to_excel(hidden_worksheet_type_1);
                        insert_border_to_excel(hidden_worksheet_type_2);

                        // Save the Excel file
                        package.Save();
                    }
                }
                else
                {
                    // Append data to the existing Excel file
                    using (var package = new ExcelPackage(new FileInfo(absolutePath)))
                    {
                        var worksheet = package.Workbook.Worksheets["Raw_Data"];

                        // Find the last used row
                        int lastRow = worksheet.Dimension?.End.Row ?? 1;
                        int serialNumber = lastRow; // Continue serial number from last row

                        // Split the raw data into lines
                        var rows = rawRxData.Split(new[] { "\n" }, StringSplitOptions.None);

                        // Loop through the rows of data and append to the Excel sheet
                        for (int i = 0; i < rows.Length; i++)
                        {
                            string serial = serialNumber.ToString("D2"); // Format serial as 01, 02, etc.
                            string currentDate = DateTime.Now.ToString("yyyy-MM-dd"); // Current Date (format: yyyy-MM-dd)
                            string currentTime = DateTime.Now.ToString("HH:mm:ss"); // Current Time (format: HH:mm:ss)

                            // Append data starting from the next available row
                            int currentRow = lastRow + i + 1;

                            worksheet.Cells[currentRow, 1].Value = serial; // Write Serial Number
                            worksheet.Cells[currentRow, 2].Value = currentDate; // Date
                            worksheet.Cells[currentRow, 3].Value = currentTime; // Time
                            worksheet.Cells[currentRow, 4].Value = rows[i].Trim(); // Write Raw Data

                            // Insert the Beacon Type formula based on the row
                            string formula = "=IF(ISBLANK(A" + currentRow + "),\" \",IF(MID(D" + currentRow + ",53,1)=\"0\",\"TYPE_1\", IF(MID(D" + currentRow + ",53,1)=\"1\", \"TYPE_2\", \"ERROR\")))";
                            worksheet.Cells[currentRow, 5].Formula = formula; // Insert formula dynamically based on row number

                            serialNumber++;
                        }
                        // Save the updated Excel file
                        package.Save();
                    }
                }

                rTBox_beacon_raw_data.Text = "";
                this.Close();

                DialogResult result_2 = MessageBox.Show($"File stored to : {absolutePath}\n Click 'OK' to preview",
                        "Check the downloaded data",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                if (result_2 == DialogResult.OK)
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
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show(
                       $"Beacon Operation Invalid \n {ex.Message}",
                       "Error Storing",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Exclamation,
                       MessageBoxDefaultButton.Button1
                );
            }
        }

        void insert_border_to_excel(ExcelWorksheet worksheet_all)
        {
            int lastRow = worksheet_all.Dimension.End.Row;
            int lastColumn = worksheet_all.Dimension.End.Column;

            // Define the dynamic range based on data
            var range = worksheet_all.Cells[1, 1, lastRow, lastColumn];

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

        }

        private void inputDialogBeacon_Load(object sender, EventArgs e)
        {
            resetFields();
            button_beacon_paraser_ok.Enabled = false;

        }

        void resetFields()
        {
            button_beacon_save.Enabled = false;
            label_beacon_type.Text = "XXXX";

            label_call_sign_ans.Text = "XXXX";
            label_head_ans.Text = "XXXX";
            label_type_ans.Text = "XXXX";
            label_time_day_ans.Text = "XXXX";
            label_time_hour_ans.Text = "XXXX";

            label_1_group_box2_ans.Text = "XXXX";
            label_2_group_box2_ans.Text = "XXXX";
            label_3_group_box2_ans.Text = "XXXX";
            label_4_group_box2_ans.Text = "XXXX";
            label_5_group_box2_ans.Text = "XXXX";

            label_1_group_box3_ans.Text = "XXXX";
            label_2_group_box3_ans.Text = "XXXX";
            label_3_group_box3_ans.Text = "XXXX";
            label_4_group_box3_ans.Text = "XXXX";
            label_5_group_box3_ans.Text = "XXXX";

            label_1_group_box4_ans.Text = "XXXX";
            label_2_group_box4_ans.Text = "XXXX";
            label_3_group_box4_ans.Text = "XXXX";
            label_4_group_box4_ans.Text = "XXXX";
            label_5_group_box4_ans.Text = "XXXX";

            label_1_group_box5_ans.Text = "XXXX";
            label_2_group_box5_ans.Text = "XXXX";
            label_3_group_box5_ans.Text = "XXXX";
            label_4_group_box5_ans.Text = "XXXX";
            label_5_group_box5_ans.Text = "XXXX";

            label_1_group_box6_ans.Text = "XXXX";
            label_2_group_box6_ans.Text = "XXXX";
            label_4_group_box6_ans.Text = "XXXX";

            label_1_group_box7_ans.Text = "X";
            label_2_group_box7_ans.Text = "X";
            label_3_group_box7_ans.Text = "X";
            label_4_group_box7_ans.Text = "X";
            label_5_group_box7_ans.Text = "X";


            tv_time_date.Text = "XXXX-XX-XX, XX:XX:XX";
        }

        void getBeaconType1()
        {
            label_beacon_type.Text = "BEACON Type 1";

            label_time_hour.Visible = true;
            label_time_hour_ans.Visible = true;

            groupBox9.Text = "Battery Parameter";
            label_1_group_box2.Text = "Battery Voltage(V)";
            label_2_group_box2.Text = "Battery Current(mA)";
            label_3_group_box2.Text = "Battery Temperature(°C)";
            label_4_group_box2.Text = "Raw Current(mA)";
            label_5_group_box2.Visible = false;
            label_5_group_box2_ans.Visible = false;

            groupBox2.Text = "Temperature";
            label_1_group_box3.Text = "Solar Total Voltage(V)";
            label_2_group_box3.Text = "Solar Total Current(mA)";
            label_3_group_box3.Text = "Antenna Panel Temp(°C)";
            label_4_group_box3.Text = "Back Panel Temp(°C)";
            label_5_group_box3.Text = "OBC Temperature(°C)";

            groupBox3.Text = "Solar Panel Temperature(°C)";
            label_1_group_box4.Text = "Solar Panel +Z Temp";
            label_2_group_box4.Text = "Solar Panel +Y Temp";
            label_3_group_box4.Text = "Solar Panel -Z Temp";
            label_4_group_box4.Visible = true;
            label_4_group_box4_ans.Visible = true;
            label_4_group_box4.Text = "Solar Panel -X Temp";
            label_5_group_box4.Visible = true;
            label_5_group_box4_ans.Visible = true;
            label_5_group_box4.Text = "Solar Panel -Y Temp";

            groupBox5.Text = "Solar Panel Status";
            label_1_group_box5.Text = "Solar Panel +Z Status";
            label_2_group_box5.Text = "Solar Panel +Y Status";
            label_3_group_box5.Text = "Solar Panel -Z Status";
            label_4_group_box5.Visible = true;
            label_4_group_box5_ans.Visible = true;
            label_4_group_box5.Text = "Solar Panel -X Status";
            label_5_group_box5.Visible = true;
            label_5_group_box5_ans.Visible = true;
            label_5_group_box5.Text = "Solar Panel -Y Status";

            groupBox4.Text = "On-Board-Computer";
            label_1_group_box6.Text = "Operation Mode";
            label_2_group_box6.Text = "OBC Reset";
            label_4_group_box6.Text = "Last Reset";

            groupBox6_stats.Visible = true;
        }

        void getBeaconType2()
        {
            label_beacon_type.Text = "BEACON Type 2";

            label_time_hour.Visible = false;
            label_time_hour_ans.Visible = false;

            groupBox9.Text = "Solar Panel Voltage(V)";
            label_1_group_box2.Text = "Solar +Z Voltage";
            label_2_group_box2.Text = "Solar +Y Voltage";
            label_3_group_box2.Text = "Solar -Z Voltage";
            label_4_group_box2.Text = "Solar -X Voltage";
            label_5_group_box2.Visible = true;
            label_5_group_box2_ans.Visible = true;
            label_5_group_box2.Text = "Solar -Y Voltage";

            groupBox2.Text = "Solar Panel Current(mA)";
            label_1_group_box3.Text = "Solar +Z Current";
            label_2_group_box3.Text = "Solar +Y Current";
            label_3_group_box3.Text = "Solar -Z Current";
            label_4_group_box3.Text = "Solar -X Current";
            label_5_group_box3.Text = "Solar -Y Current";

            groupBox3.Text = "Gyro";
            label_1_group_box4.Text = "GYRO -X";
            label_2_group_box4.Text = "GYRO -Y";
            label_3_group_box4.Text = "GYRO -Z";
            label_4_group_box4.Visible = false;
            label_4_group_box4_ans.Visible = false;
            label_5_group_box4.Visible = false;
            label_5_group_box4_ans.Visible = false;

            groupBox5.Text = "ACCEL";
            label_1_group_box5.Text = "ACCEL -X";
            label_2_group_box5.Text = "ACCEL -Y";
            label_3_group_box5.Text = "ACCEL -Z";
            label_4_group_box5.Visible = false;
            label_4_group_box5_ans.Visible = false;
            label_5_group_box5.Visible = false;
            label_5_group_box5_ans.Visible = false;

            groupBox4.Text = "MAG";
            label_1_group_box6.Text = "MAG -X";
            label_2_group_box6.Text = "MAG -Y";
            label_4_group_box6.Text = "MAG -Z";
            label_4_group_box6.Visible = true;
            label_4_group_box6_ans.Visible = true;

            groupBox6_stats.Visible = false;
        }

        private void rTBox_beacon_raw_data_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rTBox_beacon_raw_data.Text))
            {
                button_beacon_paraser_ok.Enabled = false;
            }
            else
            {
                button_beacon_paraser_ok.Enabled = true;
            }
        }
    }
}
