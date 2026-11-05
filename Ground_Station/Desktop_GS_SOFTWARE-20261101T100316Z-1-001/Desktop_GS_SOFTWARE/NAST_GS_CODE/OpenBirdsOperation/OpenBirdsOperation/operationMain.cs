using System;
using System.Drawing;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Text.RegularExpressions;
using One_Sgp4;
using Microsoft.VisualBasic.FileIO;
using OfficeOpenXml;
using System.Diagnostics;
using OfficeOpenXml.Style;

namespace OpenBirdsOperation
{
    public partial class operationMain : Form
    {

        public string CommandString
        {
            set
            {
                Command_HEX.Text = value;
            }
            get
            {
                return Command_HEX.Text;
            }
        }

        public string CommentsString
        {
            set
            {
                Comments.Text = value;
            }
            get
            {
                return Comments.Text;
            }
        }

        public sendDialogDigipeaterCMD digipeaterWindow;
        public inputDialogBeacon inputDialogBeacon;
        public inputDialogHK inputDialogHK;
        public inputDialogCamera inputDialogCamera;
        public inputDialogADCS inputDialogADCS;
        public inputDialogEPDM inputDialogEPDM;
        public inputDialogDigipeater inputDialogDigipeater;
        public inputDialogLOG inputDialogLOG;
        public inputDialogCW inputDialogCW;
        private StringBuilder cwSerialLineBuffer = new StringBuilder();
        private List<string> cwHistoryLines = new List<string>();
        private List<string> hkHistoryLines = new List<string>();
        private List<string> camHistoryLines = new List<string>();
        private string cwAccumMain = "";

        private SerialPort TncPort = new SerialPort();
        private SerialPort RadioPort = new SerialPort();
        private SerialPort RotatorPort = new SerialPort();

        private bool TncPortConnected = false;
        private bool RadioPortConnected = false;
        private bool RotatorPortConnected = false;
        private bool ReceivedDataKissFrameFlag = false;

        int receivedPacketNumber = 0;

        public commandList commandList;

        public string rootPath;
        public DateTime dtAppStart = DateTime.UtcNow;
        public string savingRootPath;
        public string tlePath;
        public string freqPath;

        public double latitude, longitude, height;

        public double ulFreq, dlFreq;
        public double cwFreq;

        public int civAddress;

        public double range;
        public double elevation, azimuth;

        public string call_sign;

        public static string FindRepoDataFile(params string[] relativeCandidates)
        {
            var seedDirs = new List<string>();
            try { seedDirs.Add(AppDomain.CurrentDomain.BaseDirectory); } catch { }
            try { seedDirs.Add(Path.GetDirectoryName(Application.ExecutablePath)); } catch { }
            try { seedDirs.Add(Directory.GetCurrentDirectory()); } catch { }

            var searchRoots = new List<string>();
            foreach (var seed in seedDirs)
            {
                if (string.IsNullOrEmpty(seed)) continue;
                string cur = seed;
                for (int depth = 0; depth <= 6 && !string.IsNullOrEmpty(cur); depth++)
                {
                    if (!searchRoots.Contains(cur)) searchRoots.Add(cur);
                    try
                    {
                        var parent = Directory.GetParent(cur);
                        cur = parent != null ? parent.FullName : null;
                    }
                    catch { cur = null; }
                }
            }

            string[] prefixes = new string[] {
                "",
                "NAST_GS_CODE/",
                "Desktop_GS_SOFTWARE/NAST_GS_CODE/",
                "Desktop_GS_SOFTWARE-20261101T100316Z-1-001/Desktop_GS_SOFTWARE/NAST_GS_CODE/",
                "Ground_Station/Desktop_GS_SOFTWARE-20261101T100316Z-1-001/Desktop_GS_SOFTWARE/NAST_GS_CODE/"
            };

            foreach (var root in searchRoots)
            {
                foreach (var rel in relativeCandidates)
                {
                    if (string.IsNullOrEmpty(rel)) continue;
                    string cleanRel = rel.Replace('\\', '/');
                    while (cleanRel.StartsWith("../")) cleanRel = cleanRel.Substring(3);
                    while (cleanRel.StartsWith("./")) cleanRel = cleanRel.Substring(2);

                    foreach (var pfx in prefixes)
                    {
                        try
                        {
                            string candidate = Path.GetFullPath(Path.Combine(root, (pfx + cleanRel).Replace('/', Path.DirectorySeparatorChar)));
                            if (File.Exists(candidate)) return candidate;
                        }
                        catch { }
                    }
                }
            }
            return null;
        }

        public static string ResolveStorageDir(string subFolder)
        {
            try
            {
                string tle = FindRepoDataFile("Utils/TLE/TLE.txt");
                string baseStorage = null;
                if (!string.IsNullOrEmpty(tle))
                {
                    // tle is at .../Desktop_GS_SOFTWARE/NAST_GS_CODE/Utils/TLE/TLE.txt
                    DirectoryInfo d = Directory.GetParent(tle); // TLE
                    if (d != null) d = d.Parent; // Utils
                    if (d != null) d = d.Parent; // NAST_GS_CODE
                    if (d != null && d.Parent != null)
                    {
                        baseStorage = Path.Combine(d.Parent.FullName, "Storage");
                    }
                }
                if (string.IsNullOrEmpty(baseStorage))
                {
                    baseStorage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");
                }
                string full = string.IsNullOrEmpty(subFolder)
                    ? baseStorage
                    : Path.Combine(baseStorage, subFolder.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar));
                if (!Directory.Exists(full))
                {
                    Directory.CreateDirectory(full);
                }
                return full;
            }
            catch
            {
                string fallback = Path.Combine(Path.GetTempPath(), "OpenBirdsOperation_Storage", subFolder ?? "");
                try { Directory.CreateDirectory(fallback); } catch { }
                return fallback;
            }
        }

        List<Tle> tleList;

        Tle selectedTle;

        System.Windows.Forms.Timer trackingTimer;
        System.Windows.Forms.Timer serialPollTimer;
        private bool isPollingSerial = false;

        bool uplinkFlag = false;

        int freqAddjustValue = 0;

        double azHomeValue;
        double elHomeValue;

        string satFolderName = "LOG_DETAILS";

        private bool _telemetryFlag = true;  // Backing field

        private bool _digipeaterFlag = false;  // Backing field

        public bool TELEMETRY_FLAG
        {
            set
            {
                _telemetryFlag = value;
            }
            get
            {
                return _telemetryFlag;
            }
        }

        public bool DIGIPEATER_FLAG
        {
            set
            {
                _digipeaterFlag = value;
            }
            get
            {
                return _digipeaterFlag;

            }

        }

        public RadioButton FmData
        {
            set
            {
                FmdRadioButton = value;
            }
            get
            {
                return FmdRadioButton;
            }
        }

        private void prosistelRotatorCommand(double azimuth, double elevation)
        {

            if (!RotatorPort.IsOpen)
            {
                return;
            }

            if (elevation < 0)
            {
                elevation = 0;
            }

            byte[] rotatorCMD = new byte[16];

            //azimuth
            rotatorCMD[0] = 0x02;
            rotatorCMD[1] = Convert.ToByte('A');    //azimuth
            rotatorCMD[2] = Convert.ToByte('G');    //GOTO command
            rotatorCMD[3] = Convert.ToByte((int)(azimuth / 100) % 10 + '0');
            rotatorCMD[4] = Convert.ToByte((int)(azimuth / 10) % 10 + '0');
            rotatorCMD[5] = Convert.ToByte((int)(azimuth / 1) % 10 + '0');
            rotatorCMD[6] = Convert.ToByte((int)(azimuth * 10) % 10 + '0');
            rotatorCMD[7] = 0x0D;

            //elevation
            rotatorCMD[8] = 0x02;
            rotatorCMD[9] = Convert.ToByte('B');    //azimuth
            rotatorCMD[10] = Convert.ToByte('G');    //GOTO command
            rotatorCMD[11] = Convert.ToByte((int)(elevation / 100) % 10 + '0');
            rotatorCMD[12] = Convert.ToByte((int)(elevation / 10) % 10 + '0');
            rotatorCMD[13] = Convert.ToByte((int)(elevation / 1) % 10 + '0');
            rotatorCMD[14] = Convert.ToByte((int)(elevation * 10) % 10 + '0');
            rotatorCMD[15] = 0x0D;

            //            Console.WriteLine(System.Text.Encoding.Default.GetString(rotatorCMD));

            RotatorPort.Write(rotatorCMD, 0, rotatorCMD.Length);

        }

        string rotatorCMDhist = "";
        private void yaesuRotatorCommand(double azimuth, double elevation)
        {
            if (!RotatorPort.IsOpen)
            {
                return;
            }

            if (elevation < 0)
            {
                elevation = 0;
            }
            byte[] rotatorCMD = new byte[9];

            rotatorCMD[0] = Convert.ToByte('W');    //set both angle
            rotatorCMD[1] = Convert.ToByte((int)(azimuth / 100) % 10 + '0');
            rotatorCMD[2] = Convert.ToByte((int)(azimuth / 10) % 10 + '0');
            rotatorCMD[3] = Convert.ToByte((int)(azimuth / 1) % 10 + '0');
            rotatorCMD[4] = Convert.ToByte(' ');
            rotatorCMD[5] = Convert.ToByte((int)(elevation / 100) % 10 + '0');
            rotatorCMD[6] = Convert.ToByte((int)(elevation / 10) % 10 + '0');
            rotatorCMD[7] = Convert.ToByte((int)(elevation / 1) % 10 + '0');
            rotatorCMD[8] = 0x0D;

            if (Encoding.GetEncoding("ascii").GetString(rotatorCMD) != rotatorCMDhist)
            {     //if the CMD is not same to before sending
                RotatorPort.Write(rotatorCMD, 0, rotatorCMD.Length);
                rotatorCMDhist = Encoding.GetEncoding("ascii").GetString(rotatorCMD);
                Console.WriteLine(rotatorCMDhist);
            }
        }

        private void setSatelliteComboList()
        {
            if (string.IsNullOrEmpty(tlePath) || !System.IO.File.Exists(tlePath))
            {
                tlePath = FindRepoDataFile("Utils/TLE/TLE.txt") ?? tlePath;
            }
            if (string.IsNullOrEmpty(freqPath) || !System.IO.File.Exists(freqPath))
            {
                freqPath = FindRepoDataFile("Utils/frequency_list/FreqList.csv") ?? freqPath;
            }

            //Parse tle from file
            if (string.IsNullOrEmpty(tlePath) || !System.IO.File.Exists(tlePath))
            {
                Console.WriteLine("Warning: TLE file not found at startup.");
                return;
            }

            try
            {
                tleList = ParserTLE.ParseFile(tlePath);
                SatelliteNameComboBox.Items.Clear();
                SatelliteNameComboBoxSub.Items.Clear();

                foreach (var tle in tleList)
                {
                    if (string.IsNullOrEmpty(freqPath) || !System.IO.File.Exists(freqPath))
                    {
                        SatelliteNameComboBox.Items.Add(tle.getNoradID());
                        SatelliteNameComboBoxSub.Items.Add(tle.getNoradID());
                    }
                    else
                    {
                        using (var parser = new TextFieldParser(freqPath))
                        {
                            parser.Delimiters = new string[] { "," };
                            while (!parser.EndOfData)
                            {
                                var fields = parser.ReadFields();
                                if (fields != null && fields.Length >= 2 && fields[1] == tle.getNoradID())
                                {
                                    SatelliteNameComboBox.Items.Add(fields[0]);
                                    SatelliteNameComboBoxSub.Items.Add(fields[0]);
                                    break;
                                }
                            }
                        }
                    }
                }

                if (SatelliteNameComboBox.Items.Count > 0 && SatelliteNameComboBox.SelectedIndex < 0)
                {
                    SatelliteNameComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Warning: Failed to parse TLE/FreqList: " + ex.Message);
            }
        }

        private void updateSavingPath()
        {
            try
            {
                string cfgPath = null;
                try { cfgPath = Properties.Settings.Default.savingPath; } catch { }
                if (string.IsNullOrEmpty(cfgPath) || cfgPath.StartsWith(".."))
                {
                    rootPath = ResolveStorageDir("LOG_DETAILS");
                }
                else
                {
                    rootPath = cfgPath.Replace('\\', Path.DirectorySeparatorChar);
                }
                savingRootPath = rootPath;
                if (!Directory.Exists(savingRootPath))
                {
                    Directory.CreateDirectory(savingRootPath);
                }
            }
            catch
            {
                savingRootPath = ResolveStorageDir("LOG_DETAILS");
                rootPath = savingRootPath;
            }
        }

        public MaskedTextBox TextBoxCallSign
        {
            get { return mTCallSign; }
        }

        public operationMain()
        {
            InitializeComponent();
            this.Text = "Antarikchya Pratisthan Nepal - Satellite Ground Station (S2S-2)";
            try
            {
                string logoPath = FindRepoDataFile("Images/apn_logo.png", "antarikshya_logo.png");
                if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
                {
                    pictureBox1.Image = Image.FromFile(logoPath);
                }
                string flagPath = FindRepoDataFile("Images/Nepal_FLAG.png", "nepal_flag_hq.png", "nepal_flag.png");
                if (!string.IsNullOrEmpty(flagPath) && File.Exists(flagPath))
                {
                    pictureBox2.Image = Image.FromFile(flagPath);
                }
            }
            catch { }

            TncPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived); //DataReceived is the event handler
            commandList = new commandList();
            commandList.formMain = this;
            commandList.Show();

            updateSavingPath();

        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            //When program is loaded all components related COM port are disenable.
            TncPortComboBox.Enabled = false;
            RadioPortComboBox.Enabled = false;
            TncConnectButton.Enabled = false;
            RadioConnectButton.Enabled = false;
            //            TransmitButton.Enabled = false;       test
            CwRadioButton.Enabled = false;
            FmdRadioButton.Enabled = false;

            TncBaudComboBox.Enabled = false;
            RadioBaudComboBox.Enabled = false;

            RotatorPortComboBox.Enabled = false;
            RotatorConnectButton.Enabled = false;
            RotatorBaudComboBox.Enabled = false;

            savingFolderPathTextBox.Text = savingRootPath;
            SaveReceiveDataButton.Enabled = false;

            //Add port list into each COM port ComboBox (prioritize USB/ACM/COM serial ports)
            string[] rawPorts = new string[0];
            try { rawPorts = SerialPort.GetPortNames(); } catch { }
            List<string> ports = new List<string>();
            foreach (string p in rawPorts)
            {
                if (p.IndexOf("ttyUSB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.IndexOf("ttyACM", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.StartsWith("COM", StringComparison.OrdinalIgnoreCase))
                {
                    ports.Add(p);
                }
            }
            foreach (string p in rawPorts)
            {
                if (!ports.Contains(p))
                {
                    ports.Add(p);
                }
            }
            foreach (string port in ports)
            {
                TncPortComboBox.Items.Add(port);
                RadioPortComboBox.Items.Add(port);
                RotatorPortComboBox.Items.Add(port);
            }

            TncPortComboBox.Enabled = true;
            TncConnectButton.Enabled = true;
            TncBaudComboBox.Enabled = true;
            if (TncBaudComboBox.Items.Contains("115200"))
            {
                TncBaudComboBox.SelectedItem = "115200";
                TncBaudComboBox.Text = "115200";
            }
            if (RadioBaudComboBox.Items.Count > 0)
            {
                RadioBaudComboBox.SelectedIndex = 0;
            }

            RadioPortComboBox.Enabled = true;
            RadioConnectButton.Enabled = true;
            RadioBaudComboBox.Enabled = true;

            RotatorPortComboBox.Enabled = true;
            RotatorConnectButton.Enabled = true;
            RotatorBaudComboBox.Enabled = true;

            if (TncPortComboBox.Items.Count >= 1)
            {
                TncPortComboBox.SelectedIndex = 0;
            }

            if (RadioPortComboBox.Items.Count >= 2)
            {
                RadioPortComboBox.SelectedIndex = 1;
            }

            if (RotatorPortComboBox.Items.Count >= 3)
            {
                RotatorPortComboBox.SelectedIndex = 2;
            }

            if (radioModelComboBox.Items.Count > 0)
            {
                radioModelComboBox.SelectedIndex = 0;
            }
            try
            {
                if (0 <= Properties.Settings.Default.rotatorModel &&
                    Properties.Settings.Default.rotatorModel < rotatorModelComboBox.Items.Count)
                {
                    rotatorModelComboBox.SelectedIndex = Properties.Settings.Default.rotatorModel;
                }
                else if (rotatorModelComboBox.Items.Count > 0)
                {
                    rotatorModelComboBox.SelectedIndex = 0;
                }
            }
            catch { }

            try { tlePath = Properties.Settings.Default.tlePath; } catch { }
            if (string.IsNullOrEmpty(tlePath) || !File.Exists(tlePath))
            {
                tlePath = FindRepoDataFile("Utils/TLE/TLE.txt") ?? "";
            }
            tlePathTextBox.Text = tlePath;

            try { freqPath = Properties.Settings.Default.freqPath; } catch { }
            if (string.IsNullOrEmpty(freqPath) || !File.Exists(freqPath))
            {
                freqPath = FindRepoDataFile("Utils/frequency_list/FreqList.csv") ?? "";
            }
            freqPathTextBox.Text = freqPath;

            SatelliteNameComboBox.Enabled = true;
            setSatelliteComboList();

            try
            {
                latitude = Properties.Settings.Default.latitude;
                longitude = Properties.Settings.Default.longitude;
                height = Properties.Settings.Default.height;
            }
            catch { }

            latitudeTextBox.Text = latitude.ToString("0.0000", CultureInfo.InvariantCulture);
            longitudeTextBox.Text = longitude.ToString("0.0000", CultureInfo.InvariantCulture);
            heightTextBox.Text = height.ToString("0.0", CultureInfo.InvariantCulture);

            try
            {
                civAddress = Properties.Settings.Default.CIVaddress;
            }
            catch { civAddress = 124; }
            civAddressTextBox.Text = civAddress.ToString("X00");

            trackingCheckBox.Checked = false;
            frequencyAddjustValueLavel.Text = freqAddjustValue.ToString(CultureInfo.InvariantCulture);

            trackingTimer = new System.Windows.Forms.Timer();
            trackingTimer.Interval = 100;   //every 0.1sec
            trackingTimer.Tick += satTracking;
            trackingTimer.Enabled = true;

            serialPollTimer = new System.Windows.Forms.Timer();
            serialPollTimer.Interval = 40;  // 40ms poll for Mono Linux SerialPort
            serialPollTimer.Tick += SerialPollTimer_Tick;
            serialPollTimer.Enabled = true;

            try
            {
                azHomeValue = Properties.Settings.Default.azHome;
                elHomeValue = Properties.Settings.Default.elHome;
            }
            catch { azHomeValue = 100; elHomeValue = 90; }

            azHomeTextBox.Text = azHomeValue.ToString("0.0", CultureInfo.InvariantCulture);
            elHomeTextBox.Text = elHomeValue.ToString("0.0", CultureInfo.InvariantCulture);

            mTCallSign.Enabled = false;
            try { call_sign = Properties.Settings.Default.callsign ?? ""; } catch { call_sign = ""; }
            mTCallSign.Text = call_sign;

            TransmitButton.Enabled = false;

            try
            {
                string folderPathAddr = ResolveStorageDir("ADDRESS_DETAILS");
                string fileNameAddr = "Address_Data.xlsx";
                string absolutePathAddr = System.IO.Path.Combine(folderPathAddr, fileNameAddr);

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                if (!File.Exists(absolutePathAddr))
                {
                    using (var package = new ExcelPackage(new FileInfo(absolutePathAddr)))
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Address_Information");

                        worksheet.Cells["A1:D1"].Merge = true;
                        worksheet.Cells["A1:D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:D1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 1].Value = "HK Data";

                        worksheet.Cells["F1:I1"].Merge = true;
                        worksheet.Cells["F1:I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["F1:I1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 6].Value = "ADCS";

                        worksheet.Cells["K1:N1"].Merge = true;
                        worksheet.Cells["K1:N1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["K1:N1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 11].Value = "Camera";

                        worksheet.Cells["P1:S1"].Merge = true;
                        worksheet.Cells["P1:S1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["P1:S1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 16].Value = "EPDM";

                        worksheet.Cells["U1:X1"].Merge = true;
                        worksheet.Cells["U1:X1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["U1:X1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 21].Value = "Digipeater";

                        worksheet.Cells[2, 1].Value = "SN";
                        worksheet.Cells[2, 2].Value = "Time Stamp";
                        worksheet.Cells[2, 3].Value = "Command";
                        worksheet.Cells[2, 4].Value = "No of Packets";

                        worksheet.Cells[2, 6].Value = "SN";
                        worksheet.Cells[2, 7].Value = "Time Stamp";
                        worksheet.Cells[2, 8].Value = "Command";
                        worksheet.Cells[2, 9].Value = "No of Packets";

                        worksheet.Cells[2, 11].Value = "SN";
                        worksheet.Cells[2, 12].Value = "Time Stamp";
                        worksheet.Cells[2, 13].Value = "Command";
                        worksheet.Cells[2, 14].Value = "No of Packets";

                        worksheet.Cells[2, 16].Value = "SN";
                        worksheet.Cells[2, 17].Value = "Time Stamp";
                        worksheet.Cells[2, 18].Value = "Command";
                        worksheet.Cells[2, 19].Value = "No of Packets";

                        worksheet.Cells[2, 21].Value = "SN";
                        worksheet.Cells[2, 22].Value = "Time Stamp";
                        worksheet.Cells[2, 23].Value = "Command";
                        worksheet.Cells[2, 24].Value = "No of Packets";

                        worksheet.Row(1).Style.Font.Bold = true;
                        worksheet.Row(2).Style.Font.Bold = true;

                        package.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Info: Address_Data.xlsx init skipped: " + ex.Message);
            }

            // Auto-connect to Ground Station USB UART (/dev/ttyUSB* or /dev/ttyACM*) on startup if available
            try
            {
                if (!TncPortConnected && TncPortComboBox.Items.Count > 0)
                {
                    string firstPort = TncPortComboBox.Items[0].ToString();
                    if (firstPort.IndexOf("ttyUSB", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        firstPort.IndexOf("ttyACM", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        TncPortComboBox.SelectedIndex = 0;
                        TncConnectButton_Click(this, EventArgs.Empty);
                    }
                }
            }
            catch { }
        }

        public void Transmit(object sender, EventArgs e)
        {
            Transmit_async();
        }


        public async Task Transmit_async()
        {
            string Sending_command = Command_HEX.Text;
            string Sending_coments = Comments.Text;


            //2021/06/13 only check 13bytes
            if (Regex.IsMatch(Sending_command, ".. .. .. .. .. .. .. .. .. .. .. .. .."))
            {
                Console.WriteLine("13 bytes command");
                Sending_command = Sending_command.Substring(0, 38);
            }
            else
            {
                Console.WriteLine("invalid length");
                MessageBox.Show("Invalid length");
                return;
            }

            if (Sending_command == "00 00 00 00 00 00 00 00 00 00 00 00 00")
            {
                MessageBox.Show("The command is all 00\n", "Invalid command");
                return;
            }

            TransmitButton.Enabled = false;
            //byte[] packet = Sending_command.ToString().Split(' ').Select(s => Convert.ToByte(s, 16)).ToArray();
            //byte[] packet_withB = new byte[packet.Length + 1];
            //packet_withB[0] = (byte)'B';     //'B' it is for Addnics receiver
            //Buffer.BlockCopy(packet, 0, packet_withB, 1, packet.Length);
            //byte[] KISSframe_toTNC = AX25packetToKISSframe(packet_withB);
            ////            byte[] KISSframe_toTNC = AX25packetToKISSframe(packet);

            //FmdRadioButton.Checked = true;
            await Task.Delay(100);

            ChangeUplinkFrequency();
            await Task.Delay(750);

            // Normalize HK1 / HK2 data_id (bytes 5..6 must be 00 F2 per gs_communicator.py / gs_app.c)
            string normHex = Sending_command.Trim().ToUpper();
            if (normHex.StartsWith("53 01 1D D1 F2 00 00 "))
            {
                normHex = "53 01 1D D1 F2 00 F2 " + normHex.Substring(21);
                Sending_command = normHex;
                Command_HEX.Text = normHex;
            }
            else if (normHex.StartsWith("53 01 1D D2 F2 00 00 "))
            {
                normHex = "53 01 1D D2 F2 00 F2 " + normHex.Substring(21);
                Sending_command = normHex;
                Command_HEX.Text = normHex;
            }

            if (TncPortConnected)
            {
                string cleanHex = Sending_command.Trim().ToUpper();
                string uplinkCommand;

                // Match gs_communicator.py & gs_app.c built-in command handlers
                if (cleanHex == "53 04 CC 5E BE 00 D0 00 00 00 00 00 45")
                {
                    camHistoryLines.Clear();
                    uplinkCommand = "DC";
                }
                else if (cleanHex == "53 04 CC 5E BD 00 00 00 00 00 00 00 00")
                {
                    camHistoryLines.Clear();
                    uplinkCommand = "CAM";
                }
                else if (cleanHex.StartsWith("53 04 CC 5E BC "))
                {
                    uplinkCommand = "CAMON";
                }
                else if (cleanHex == "53 01 1D D1 F2 00 F2 00 00 00 00 00 01")
                {
                    uplinkCommand = "HK1";
                }
                else if (cleanHex == "53 01 1D D2 F2 00 F2 00 00 00 00 00 01")
                {
                    uplinkCommand = "HK2";
                }
                else if (cleanHex == "53 01 1D D1 F5 00 00 00 00 00 00 00 01")
                {
                    uplinkCommand = "PING";
                }
                else
                {
                    if (cleanHex.StartsWith("53 04 CC 5E BE") || cleanHex.StartsWith("53 04 CC 5E BD") || cleanHex.StartsWith("53 01 1D D2 F5"))
                    {
                        camHistoryLines.Clear();
                    }
                    uplinkCommand = cleanHex.StartsWith("HEX ") ? cleanHex : "HEX " + cleanHex;
                }

                TncPort.WriteLine(uplinkCommand);
            }

            await Task.Delay(750);
            ChangeDownlinkFrequency();
            DateTime dt = DateTime.UtcNow;
            string time = dt.ToString("yyyy/MM/dd HH:mm:ss");

            //            RxDataTextBox.Text += "#(" + time + ")" + " CMD: " + Sending_command + "\n";
            this.RxDataTextBox.Focus();
            this.RxDataTextBox.AppendText("#(" + time + ")" + " CMD: " + Sending_command + "\n\n\n\n");

            await Task.Delay(500);
            TransmitButton.Enabled = true;
        }

        public void TransmitDummyCameraCommand()
        {
            Command_HEX.Text = "53 04 CC 5E BE 00 D0 00 00 00 00 00 45";
            Comments.Text = "Transmit Dummy Image Data (69 Chunks / DC)";
            var _ = Transmit_async();
        }

        public void TransmitRealCameraCommand()
        {
            Command_HEX.Text = "53 04 CC 5E BD 00 00 00 00 00 00 00 00";
            Comments.Text = "Real Camera Data Downlink (All Chunks / CAM)";
            var _ = Transmit_async();
        }

        public void TransmitRealCameraCaptureCommand()
        {
            Command_HEX.Text = "53 04 CC 5E BC 00 00 00 00 00 00 00 00";
            Comments.Text = "Real Camera Capture to Flash (CAMON / SNAP)";
            var _ = Transmit_async();
        }

        public void TransmitHkCommand(int pktCount)
        {
            if (pktCount <= 0) pktCount = 1;
            Command_HEX.Text = string.Format("53 01 1D D1 F2 00 F2 00 00 00 00 {0:X2} {1:X2}", (pktCount >> 8) & 0xFF, pktCount & 0xFF);
            Comments.Text = pktCount == 1
                ? "Request Live HK Telemetry (HK1 - 1 Pkt)"
                : string.Format("Download Stored HK Data ({0} Pkts @ 128B)", pktCount);
            var _ = Transmit_async();
        }

        private byte[] AX25packetToKISSframe(byte[] ax25packet)
        {
            byte[] KISSframe_temp = new byte[3 + 2 * ax25packet.Length];
            int index2 = 0; //index for ax25packet_encoded 

            if (TncBaudComboBox.Text == "115200")
            {
                System.Diagnostics.Trace.WriteLine("ADDNICS TNC");
                //                KISSframe_temp[index2++] = 0x42;  //ADDNICS 600bps header
            }

            KISSframe_temp[index2++] = 0xC0;  //KISS header (index2 = 0)
            KISSframe_temp[index2++] = 0x00;    //port number (index2 = 1)

            for (int index1 = 0; index1 < ax25packet.Length; index1++)
            {
                if (ax25packet[index1] == 0xC0)   //0xC0
                {
                    //Replace with 0xDB 0xDC
                    KISSframe_temp[index2++] = 0xDB;
                    KISSframe_temp[index2++] = 0xDC;
                }
                else if (ax25packet[index1] == 0xDB)   //0xDB
                {
                    //Replace with 0xDB 0xDD
                    KISSframe_temp[index2++] = 0xDB;
                    KISSframe_temp[index2++] = 0xDD;
                }
                else KISSframe_temp[index2++] = ax25packet[index1];

            }

            KISSframe_temp[index2++] = 0xC0;    //KISS footer

            byte[] KISSframe = new byte[index2];
            Buffer.BlockCopy(KISSframe_temp, 0, KISSframe, 0, index2);

            for (int i = 0; i < KISSframe.Length; i++)
            {
                System.Diagnostics.Trace.Write(string.Format("{0,3:X2}", KISSframe[i]));
            }
            System.Diagnostics.Trace.WriteLine("");

            return KISSframe;
        }

        public void ChangeFrequency(int freq)
        {
            //Set radio for command uplink
            if (RadioPortConnected == true)
            {
                //            byte[] frequency_command = { 0xFE, 0xFE, 0x00, 0x9D, 0x00, 0x00, 0x30, 0x31, 0x35, 0x04, 0xFD };   //435.313MHz
                byte[] freqCMD = new byte[11];
                freqCMD[0] = 0xFE;
                freqCMD[1] = 0xFE;
                freqCMD[2] = 0x00;
                freqCMD[3] = BitConverter.GetBytes(civAddress)[0];
                freqCMD[4] = 0x00; //Change freqency command 0x00
                freqCMD[5] = BitConverter.GetBytes(freq / 1 % 10 + freq / 10 % 10 * 16)[0];    //0x12 -> XXXXXXXX12Hz
                freqCMD[6] = BitConverter.GetBytes(freq / 100 % 10 + freq / 1000 % 10 * 16)[0];    //0x34 -> XXXXXX34XXHz
                freqCMD[7] = BitConverter.GetBytes(freq / 10000 % 10 + freq / 100000 % 10 * 16)[0];    //0x56 -> XXXX56XXXXHz
                freqCMD[8] = BitConverter.GetBytes(freq / 1000000 % 10 + freq / 10000000 % 10 * 16)[0];    //0x78 -> XX78XXXXXXHz
                freqCMD[9] = BitConverter.GetBytes(freq / 100000000 % 10 + freq / 1000000000 % 10 * 16)[0];    //0x90 -> 90XXXXXXXXHz
                freqCMD[10] = 0xFD;

                //Console.WriteLine(BitConverter.ToString(freqCMD));
                RadioPort.Write(freqCMD, 0, freqCMD.Length);
            }


        }


        public delegate void UpdateTrackingValueDelegate(int frequency, double azimuth, double elevation);

        public void UpdateTrakingValue(int frequency, double azimuth, double elevation)
        {
            freqLabel.Text = (frequency / 1000000.0).ToString("000.000000");
            elevationLabel.Text = elevation.ToString("00.0");
            azimuthLabel.Text = azimuth.ToString("000.0");

            ChangeFrequency(frequency);


            if (elevation > -10)
            {
                if (rotatorModelComboBox.SelectedIndex == 0)
                {
                    prosistelRotatorCommand(azimuth, elevation);
                }
                else
                {
                    yaesuRotatorCommand(azimuth, elevation);
                }
            }
            else
            {
                if (rotatorModelComboBox.SelectedIndex == 0)
                {
                    prosistelRotatorCommand(azHomeValue, elHomeValue);
                }
                else
                {
                    yaesuRotatorCommand(azHomeValue, elHomeValue);
                }
            }

        }

        //tracking function that timer call every 1 or 0.1 sec
        public void satTracking(object sender, EventArgs e)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
            {
                return;
            }

            try
            {
                double deltaT = 0.1;         //deltaT = 0.1s
                double speedOfLight = 299792458;      //unit : m/s
                int correctedFreqency;
                double rangingSpeed;

                UpdateTrackingValueDelegate UTVD = new UpdateTrackingValueDelegate(UpdateTrakingValue);

                if (selectedTle == null)
                {
                    this.Invoke(UTVD, 0, azHomeValue, elHomeValue);
                    return;
                }

                if (!trackingCheckBox.Checked)
                {
                    if (uplinkFlag)
                    {
                        //FM uplink
                        correctedFreqency = (int)((ulFreq + freqAddjustValue));
                    }
                    else
                    {
                        if (CwRadioButton.Checked)
                        {
                            //CW listning
                            correctedFreqency = (int)((cwFreq + freqAddjustValue));
                        }
                        else
                        {
                            //FM downlink
                            correctedFreqency = (int)((dlFreq + freqAddjustValue));
                        }
                    }
                    this.Invoke(UTVD, correctedFreqency, azHomeValue, elHomeValue);
                    return;
                }

                //Create Time points
                EpochTime currentTime = new EpochTime(DateTime.UtcNow);
                EpochTime nextTime = new EpochTime(DateTime.UtcNow.AddMilliseconds(deltaT * 1000));
                //Calculate Satellite Position and Speed
                One_Sgp4.Sgp4 sgp4Propagator = new Sgp4(selectedTle, Sgp4.wgsConstant.WGS_84);
                //set calculation parameters StartTime, EndTime and caclulation steps in minutes
                sgp4Propagator.runSgp4Cal(currentTime, nextTime, 1 / 60.0 * deltaT);  //100msec
                List<One_Sgp4.Sgp4Data> resultDataList = sgp4Propagator.getResults();
                if (resultDataList == null || resultDataList.Count < 2)
                {
                    return;
                }

                One_Sgp4.Coordinate observer = new Coordinate(latitude, longitude, height);

                //Calculate Sperical Coordinates from an Observer to Satellite
                //returns 3D-Point with range(km), azimuth(radians), elevation(radians) to the Satellite
                One_Sgp4.Point3d spherical = One_Sgp4.SatFunctions.calcSphericalCoordinate(observer, currentTime, resultDataList[0]);
                One_Sgp4.Point3d nextSpherical = One_Sgp4.SatFunctions.calcSphericalCoordinate(observer, currentTime, resultDataList[1]);

                rangingSpeed = (nextSpherical.x - spherical.x) * 1000 / deltaT;  //unit : m/s

                if (uplinkFlag)
                {
                    //FM uplink
                    correctedFreqency = (int)((ulFreq - freqAddjustValue) * (1 + rangingSpeed / speedOfLight));
                }
                else
                {
                    if (CwRadioButton.Checked)
                    {
                        //CW listning
                        correctedFreqency = (int)((cwFreq + freqAddjustValue) * (1 - rangingSpeed / speedOfLight));
                    }
                    else
                    {
                        //FM downlink
                        correctedFreqency = (int)((dlFreq + freqAddjustValue) * (1 - rangingSpeed / speedOfLight));
                    }
                }

                this.Invoke(UTVD, correctedFreqency, spherical.y, spherical.z);
            }
            catch { }
        }

        public void ChangeUplinkFrequency()
        {
            uplinkFlag = true;
        }

        public void ChangeDownlinkFrequency()
        {
            uplinkFlag = false;
        }

        public void ChangeCW()
        {
            //Set radio for command uplink
            if (RadioPortConnected == true)
            {
                byte[] radioMode_CW = { 0xFE, 0xFE, 0x00, 0x7C, 0x01, 0x03, 0x01, 0xFD, 0xFE, 0xFE, 0x00, 0x7C, 0x1A, 0x06, 0x00, 0x00, 0xFD };   //CW, Data mode off
                radioMode_CW[3] = BitConverter.GetBytes(civAddress)[0];
                RadioPort.Write(radioMode_CW, 0, radioMode_CW.Length);
            }
        }

        public void ChangeFMD()
        {
            //Set radio for command uplink
            if (RadioPortConnected == true)
            {
                byte[] radioMode_FMD = { 0xFE, 0xFE, 0x00, 0x7C, 0x01, 0x05, 0x01, 0xFD, 0xFE, 0xFE, 0x00, 0x7C, 0x1A, 0x06, 0x01, 0x01, 0xFD };  //FM, Data mode on
                radioMode_FMD[3] = BitConverter.GetBytes(civAddress)[0];
                RadioPort.Write(radioMode_FMD, 0, radioMode_FMD.Length);
            }
        }

        private void TncConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TncPortConnected)
                {
                    int baud = 115200;
                    string baudStr = TncBaudComboBox.SelectedItem != null ? TncBaudComboBox.SelectedItem.ToString() : TncBaudComboBox.Text;
                    if (!int.TryParse(baudStr, out baud) || baud <= 0)
                    {
                        baud = 115200;
                    }
                    TncBaudComboBox.Text = baud.ToString();

                    TncPort.PortName = TncPortComboBox.Text;
                    TncPort.BaudRate = baud;
                    TncPort.DataBits = 8;
                    TncPort.Parity = Parity.None;
                    TncPort.StopBits = StopBits.One;
                    if (baud != 115200)
                    {
                        TncPort.DtrEnable = true;
                        TncPort.Handshake = Handshake.RequestToSend;
                    }
                    else
                    {
                        TncPort.DtrEnable = false;
                        TncPort.RtsEnable = false;
                        TncPort.Handshake = Handshake.None;
                        System.Diagnostics.Trace.WriteLine("Connect STM32WL55 / ADDNICS TNC @ 115200");
                    }
                    TncPort.Open();
                    TncPortConnected = true;
                    TransmitButton.Enabled = true;
                    TncPortComboBox.Enabled = false;
                    TncBaudComboBox.Enabled = false;
                    TncConnectButton.Text = "DisConnect";

                    FmdRadioButton.Checked = true;
                    FmdRadioButton.Enabled = true; // Telemetry
                    CwRadioButton.Enabled = true;  // Digipeater

                    if (baud == 115200)
                    {
                        try
                        {
                            TncPort.Write("STATUS\r\n");
                        }
                        catch { }
                    }
                }
                else
                {
                    TncPort.Close();
                    TncPortConnected = false;
                    TransmitButton.Enabled = false;
                    TncPortComboBox.Enabled = true;
                    TncBaudComboBox.Enabled = true;
                    TncConnectButton.Text = "Connect";

                    FmdRadioButton.Enabled = false; // Telemetry
                    CwRadioButton.Enabled = false;  //Digipeater

                }
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show(
                              $"Problem in connection to {TncPortComboBox.Text}\n{ex.Message}",
                              "Port not found!");
            }
        }

        private void RadioConnectButton_Click(object sender, EventArgs e)
        {

            try
            {
                if (!RadioPortConnected)
                {
                    RadioPort.PortName = RadioPortComboBox.Text;
                    RadioPort.BaudRate = Convert.ToInt32(RadioBaudComboBox.SelectedItem);
                    RadioPort.DataBits = 8;
                    RadioPort.Parity = Parity.None;
                    RadioPort.StopBits = StopBits.One;
                    RadioPort.RtsEnable = true;
                    RadioPort.Open();
                    RadioPortConnected = true;
                    RadioPortComboBox.Enabled = false;
                    //CwRadioButton.Enabled = true;
                    //FmdRadioButton.Enabled = true;
                    //CwRadioButton.Checked = true;
                    RadioBaudComboBox.Enabled = false;
                    //ChangeCW();
                    RadioConnectButton.Text = "DisConnect";
                }
                else
                {
                    RadioPort.Close();
                    RadioPortConnected = false;
                    RadioPortComboBox.Enabled = true;
                    //CwRadioButton.Enabled = false;
                    //FmdRadioButton.Enabled = false;
                    RadioBaudComboBox.Enabled = true;
                    RadioConnectButton.Text = "Connect";
                }
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show(
                              $"Problem in connection to {RadioPortComboBox.Text}\n{ex.Message}",
                              "Port not found!");
            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //            Properties.Settings.Default.Save();
            TncPort.Close();
            RadioPort.Close();
            RotatorPort.Close();
            trackingTimer.Stop();
        }

        private void CwRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //  ChangeCW();
            TransmitButton.Enabled = false;
            if (TncPortConnected && CwRadioButton != null && CwRadioButton.Checked)
            {
                if (digipeaterWindow == null || digipeaterWindow.IsDisposed)
                {
                    digipeaterWindow = new sendDialogDigipeaterCMD(this);
                    digipeaterWindow.Show();

                }
                else
                {
                    digipeaterWindow.BringToFront();

                }

                TransmitButton.Enabled = false;

            }
            else if (TncPortConnected && CwRadioButton != null && !CwRadioButton.Checked)
            {
                TransmitButton.Enabled = true;
            }
        }

        private void FmdRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //  ChangeFMD();
            TransmitButton.Enabled = false;

            if (TncPortConnected && FmdRadioButton.Checked && FmdRadioButton != null)
            {
                if (digipeaterWindow != null)
                {
                    digipeaterWindow.Close();
                }

                TransmitButton.Enabled = true;

                if (TncPort.BaudRate != 115200)
                {
                    ReceiveData("MODETELE");
                }
                TELEMETRY_FLAG = true;
                DIGIPEATER_FLAG = false;
            }
            else if (TncPortConnected && CwRadioButton.Checked && CwRadioButton != null)
            {
                TransmitButton.Enabled = false;
                TELEMETRY_FLAG = true;
                DIGIPEATER_FLAG = false;
            }
        }

        public void ReceiveData(string data)
        {
            if (TncPort.IsOpen)
            {
                var temp_data = data + " 7e ";

                //var temp_data = data;

                if (temp_data.Length != 51)
                {
                    temp_data = temp_data.PadRight(51, '0');
                }

                TncPort.WriteLine(temp_data);

            }
        }

        private void SerialPollTimer_Tick(object sender, EventArgs e)
        {
            if (!TncPortConnected || TncPort == null || !TncPort.IsOpen || isPollingSerial)
            {
                return;
            }
            try
            {
                int avail = TncPort.BytesToRead;
                if (avail <= 0) return;
                isPollingSerial = true;
                byte[] buf = new byte[avail];
                int n = TncPort.Read(buf, 0, avail);
                if (n > 0)
                {
                    byte[] actual;
                    if (n == avail) actual = buf;
                    else
                    {
                        actual = new byte[n];
                        Buffer.BlockCopy(buf, 0, actual, 0, n);
                    }
                    ProcessSerialBytes(actual);
                }
            }
            catch { }
            finally
            {
                isPollingSerial = false;
            }
        }

        private void ProcessSerialBytes(byte[] SerialGetData)
        {
            if (SerialGetData == null || SerialGetData.Length == 0) return;

            if (TELEMETRY_FLAG == true && DIGIPEATER_FLAG == false)
            {
                AddRxDataDelegate add = new AddRxDataDelegate(AddRxData);
                updateSaveReceiveButton();

                // Check if data is binary KISS frame (0xC0) or printable ASCII text from STM32WL55 GS
                bool hasKissOrBinary = false;
                for (int i = 0; i < SerialGetData.Length; i++)
                {
                    byte b = SerialGetData[i];
                    if (b == 0xC0 || (b < 0x09) || (b > 0x0D && b < 0x20) || b > 0x7E)
                    {
                        hasKissOrBinary = true;
                        break;
                    }
                }

                string displayStr;
                if (!hasKissOrBinary)
                {
                    displayStr = Encoding.ASCII.GetString(SerialGetData).Replace("\r\n", "\n").Replace("\r", "\n");
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < SerialGetData.Length; i++)
                    {
                        byte EachData = SerialGetData[i];
                        sb.AppendFormat("{0:X2} ", EachData);
                        if (EachData == 0xC0)
                        {
                            if (ReceivedDataKissFrameFlag)
                            {
                                ReceivedDataKissFrameFlag = false;
                                sb.Append("\n");
                            }
                            else
                            {
                                ReceivedDataKissFrameFlag = true;
                            }
                        }
                    }
                    sb.Append("\n\n");
                    displayStr = sb.ToString();
                }

                if (this.RxDataTextBox.InvokeRequired)
                {
                    this.RxDataTextBox.Invoke(add, displayStr);
                }
                else
                {
                    AddRxData(displayStr);
                }

                try
                {
                    string rawAscii = Encoding.ASCII.GetString(SerialGetData);
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate {
                            FeedCwSerialChunk(rawAscii);
                        });
                    }
                    else
                    {
                        FeedCwSerialChunk(rawAscii);
                    }
                }
                catch { }
            }
        }

        //Downlink Terminal Setting
        private void DataReceived(object Sender, SerialDataReceivedEventArgs e)
        {
            if (TELEMETRY_FLAG == true && DIGIPEATER_FLAG == false)
            {
                try
                {
                    int avail = TncPort.BytesToRead;
                    if (avail > 0)
                    {
                        byte[] SerialGetData = new byte[avail];
                        int n = TncPort.Read(SerialGetData, 0, avail);
                        if (n > 0)
                        {
                            ProcessSerialBytes(SerialGetData);
                        }
                    }
                }
                catch { }
            }
            else if (TELEMETRY_FLAG == false && DIGIPEATER_FLAG == true)
            {
                AddRxDataDelegate add_dp = new AddRxDataDelegate(AddRxDPData);
                byte[] SerialGetData_DP = new byte[TncPort.BytesToRead];
                string ReceivedDataString_DP = string.Empty;

                try
                {
                    TncPort.Read(SerialGetData_DP, 0, SerialGetData_DP.Length);

                    for (int i = 0; i < SerialGetData_DP.Length; i++)
                    {
                        byte EachData = SerialGetData_DP[i];
                        ReceivedDataString_DP += string.Format("{0:X2} ", EachData);

                    }

                }

                catch
                {
                    //Protect error
                }

                finally
                {

                    DateTime dt = DateTime.Now;
                    string time = dt.ToString("yyyy/MM/dd HH:mm:ss");

                    // Remove spaces to ensure the string has only hex characters
                    ReceivedDataString_DP = ReceivedDataString_DP.Replace(" ", "");
                    string asciiString = "";

                    // Loop through each pair of characters (each byte) in the string
                    for (int i = 0; i < ReceivedDataString_DP.Length; i += 2)
                    {
                        // Extract the byte (2 characters at a time) and convert to byte
                        byte currentByte = Convert.ToByte(ReceivedDataString_DP.Substring(i, 2), 16);

                        // Convert the byte to its ASCII character if it's printable
                        if (currentByte >= 32 && currentByte <= 126) // ASCII printable range
                        {
                            char asciiChar = Convert.ToChar(currentByte);
                            asciiString += asciiChar; // Append the character to the string
                            Console.WriteLine($"Original byte: 0x{currentByte:X2}, ASCII: {asciiChar}");
                        }
                        else
                        {
                            Console.WriteLine($"Original byte: 0x{currentByte:X2}, Non-printable");
                        }
                    }

                    digipeaterWindow.RichTextBox_DigipeaterForm.Invoke(add_dp, asciiString);
                    asciiString = string.Empty;
                    SerialGetData_DP = new byte[0];
                    digipeaterWindow.RichTextBox_DigipeaterForm.Invoke(add_dp, "\n\n");
                }
            }
        }

        private void AddRxDPData(string data)
        {
            digipeaterWindow.RichTextBox_DigipeaterForm.Focus();
            digipeaterWindow.RichTextBox_DigipeaterForm.AppendText(data);
        }

        private void updateSaveReceiveButton()
        {
            // Check if invocation is required
            if (SaveReceiveDataButton.InvokeRequired)
            {
                // Use Invoke to marshal the call to the UI thread
                SaveReceiveDataButton.Invoke(new MethodInvoker(updateSaveReceiveButton));
            }
            else
            {
                // Safe to update the control directly
                SaveReceiveDataButton.Enabled = true; // Example action
            }
        }

        private delegate void AddRxDataDelegate(string data);
        private void AddRxData(string data)
        {

            this.RxDataTextBox.Focus();
            this.RxDataTextBox.AppendText(data);
            receivedPacketNumber += (data.Split('\n').Length - 1);
            receivedPacketsLabel.Text = "Received packet(s) : " + receivedPacketNumber;
        }


        private void ClearRxDataButton_Click(object sender, EventArgs e)
        {
            receivedPacketNumber = 0;
            receivedPacketsLabel.Text = "Received packet(s) : " + receivedPacketNumber;
            RxDataTextBox.Text = "";
        }
        private void SaveReceiveDataButton_Click(object sender, EventArgs e)
        {

            savingRootPath = Properties.Settings.Default.savingPath;

            string rawRxData = RxDataTextBox.Text;

            DateTime dt = DateTime.UtcNow;
            string time = dt.ToString("yyyyMMdd_HHmmss");
            string folderPath = System.IO.Path.Combine(savingRootPath, dtAppStart.ToString("yyyyMMdd"));
            string fileName;
            string absolutePath;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            fileName = time + "_LOG_raw.txt";
            absolutePath = System.IO.Path.Combine(folderPath, fileName);
            StreamWriter sw = new StreamWriter(absolutePath, false, Encoding.ASCII);
            sw.Write(rawRxData);
            sw.Close();

            string[] rxDataRemovedComments = rawRxData.Split('#');
            string[] rxDataComments = new string[rxDataRemovedComments.Length];
            for (int i = 0; i < rxDataRemovedComments.Length; i++)
            {
                rxDataComments[i] = rxDataRemovedComments[i].Split('\n')[0];
                if (rxDataRemovedComments[i].Length > rxDataComments[i].Length)     //something contents are exist
                {
                    rxDataRemovedComments[i] = rxDataRemovedComments[i].Substring(rxDataComments[i].Length + 1);
                }
                else
                {
                    rxDataRemovedComments[i] = "";
                }
                //                System.Diagnostics.Trace.WriteLine("comment" + i + ":" + rxDataComments[i]);
                //                System.Diagnostics.Trace.WriteLine("content" + i + ":" + rxDataRemovedComments[i]);
                //                System.Diagnostics.Trace.WriteLine("");
            }
            //2021/06/13 remove the function for compiling the packets and detecting the missing packets.
            ClearRxDataButton_Click(sender, e);

            DialogResult dialog_result = MessageBox.Show($"File stored to : {absolutePath}\n Click 'OK' to preview LOGS",
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

        private void MainForm_Resize(object sender, EventArgs e)
        {
        }

        private void RxDataTextBox_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void RxDataTextBox_TextChanged(object sender, EventArgs e)
        {
        }



        private const uint IMF_DUALFONT = 0x80;
        private const uint WM_USER = 0x0400;
        private const uint EM_SETLANGOPTIONS = WM_USER + 120;
        private const uint EM_GETLANGOPTIONS = WM_USER + 121;

        [System.Runtime.InteropServices.DllImport("USER32.dll")]
        private static extern uint SendMessage(
            System.IntPtr hWnd,
            uint msg,
            uint wParam,
            uint lParam);
        private void NoRichTextChange(RichTextBox RichTextBoxCtrl)
        {
            try
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    uint lParam;
                    lParam = SendMessage(RichTextBoxCtrl.Handle, EM_GETLANGOPTIONS, 0, 0);
                    lParam &= ~IMF_DUALFONT;
                    SendMessage(RichTextBoxCtrl.Handle, EM_SETLANGOPTIONS, 0, lParam);
                }
            }
            catch { }
        }

        private void RxDataTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        private void RxDataTextBox_VScroll(object sender, EventArgs e)
        {
        }

        private void Coments_Click(object sender, EventArgs e)
        {

        }

        private void Command_HEX_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
        }

        private void SelectPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "Select saving folder";
            //fbd.SelectedPath = savingFolderPathTextBox.Text;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                Console.WriteLine(fbd.SelectedPath);
                var selPath = fbd.SelectedPath;
                savingRootPath = selPath;
                //savingRootPath = fbd.SelectedPath;
                // savingFolderPathTextBox.Text = savingRootPath;
                savingFolderPathTextBox.Text = System.IO.Path.Combine(selPath, "LOG_DETAILS");
                Properties.Settings.Default.savingPath = savingFolderPathTextBox.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void ReceiveLabel_Click(object sender, EventArgs e)
        {

        }

        private void CommentsLabel_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxAddMissinPacket_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void selectTlePathButton_Click(object sender, EventArgs e)
        {
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();

            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            ofd.FileName = "tle.txt";
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = "TLE file(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            //タイトルを設定する
            ofd.Title = "Select the TLE file";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき、選択されたファイル名を表示する
                Console.WriteLine(ofd.FileName);
                tlePath = ofd.FileName;
                tlePathTextBox.Text = tlePath;
                Properties.Settings.Default.tlePath = tlePath;
                Properties.Settings.Default.Save();
                setSatelliteComboList();
            }
        }

        private void latitudeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (latitudeTextBox.Text == "")
            {
                return;
            }
            try
            {
                latitude = Convert.ToDouble(latitudeTextBox.Text);
                Properties.Settings.Default.latitude = latitude;
                Properties.Settings.Default.Save();
            }
            catch
            {
                latitudeTextBox.Text = latitude.ToString("0.0000");
            }
        }

        private void longitudeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (longitudeTextBox.Text == "")
            {
                return;
            }
            try
            {
                longitude = Convert.ToDouble(longitudeTextBox.Text);
                Properties.Settings.Default.longitude = longitude;
                Properties.Settings.Default.Save();
            }
            catch
            {
                longitudeTextBox.Text = longitude.ToString("0.0000");
            }
        }

        private void heightTextBox_TextChanged(object sender, EventArgs e)
        {
            if (heightTextBox.Text == "")
            {
                return;
            }
            try
            {
                height = Convert.ToDouble(heightTextBox.Text);
                Properties.Settings.Default.height = Convert.ToDouble(heightTextBox.Text);
                Properties.Settings.Default.Save();
            }
            catch
            {
                heightTextBox.Text = height.ToString("0.0");
            }
        }

        private void SatelliteNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SatelliteNameComboBoxSub.SelectedIndex = SatelliteNameComboBox.SelectedIndex;
            Command_HEX.Text = "00000000000000000000000000";
            Comments.Text = "None";
            SaveReceiveDataButton.Enabled = true;
            // TransmitButton.Enabled = true;
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void elevationLabel_Click(object sender, EventArgs e)
        {

        }

        private void civAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            if (civAddressTextBox.Text == "")
            {
                return;
            }
            try
            {
                civAddress = Convert.ToInt32(civAddressTextBox.Text, 16);
                if (civAddress > 255)
                {
                    civAddress = Properties.Settings.Default.CIVaddress;
                    civAddressTextBox.Text = civAddress.ToString("X00");
                }
                Properties.Settings.Default.CIVaddress = civAddress;
                Properties.Settings.Default.Save();
            }
            catch
            {
                civAddressTextBox.Text = civAddress.ToString("X00");
            }

        }

        private void trackingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void RotatorConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RotatorPortConnected)
                {
                    RotatorPort.PortName = RotatorPortComboBox.Text;
                    RotatorPort.BaudRate = Convert.ToInt32(RotatorBaudComboBox.SelectedItem);
                    RotatorPort.DataBits = 8;
                    RotatorPort.Parity = Parity.None;
                    RotatorPort.StopBits = StopBits.One;
                    RotatorPort.RtsEnable = true;
                    RotatorPort.Open();
                    RotatorPortConnected = true;
                    RotatorPortComboBox.Enabled = false;
                    RotatorBaudComboBox.Enabled = false;
                    RotatorConnectButton.Text = "DisConnect";
                }
                else
                {
                    RotatorPort.Close();
                    RotatorPortConnected = false;
                    RotatorPortComboBox.Enabled = true;
                    RotatorBaudComboBox.Enabled = true;
                    RotatorConnectButton.Text = "Connect";
                }
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show(
                             $"Problem in connection to {RotatorPortComboBox.Text}\n{ex.Message}",
                             "Port not found!");
            }
        }

        private void frequencyAddjustValueLavel_Click(object sender, EventArgs e)
        {

        }

        private void buttonPulus500Hz_Click(object sender, EventArgs e)
        {
            freqAddjustValue += 500;
            frequencyAddjustValueLavel.Text = freqAddjustValue.ToString();
        }

        private void buttonMinus500Hz_Click(object sender, EventArgs e)
        {
            freqAddjustValue -= 500;
            frequencyAddjustValueLavel.Text = freqAddjustValue.ToString();
        }

        private void buttonPulus1kHz_Click(object sender, EventArgs e)
        {
            freqAddjustValue += 1000;
            frequencyAddjustValueLavel.Text = freqAddjustValue.ToString();
        }

        private void buttonMinus1kHz_Click(object sender, EventArgs e)
        {
            freqAddjustValue -= 1000;
            frequencyAddjustValueLavel.Text = freqAddjustValue.ToString();
        }

        private void buttonPulus5kHz_Click(object sender, EventArgs e)
        {
            freqAddjustValue += 5000;
            frequencyAddjustValueLavel.Text = freqAddjustValue.ToString();
        }

        private void buttonMinus5kHz_Click(object sender, EventArgs e)
        {
            freqAddjustValue -= 5000;
            frequencyAddjustValueLavel.Text = freqAddjustValue.ToString();
        }

        private void AzHome_TextChanged(object sender, EventArgs e)
        {
            if (azHomeTextBox.Text == "")
            {
                return;
            }
            try
            {
                azHomeValue = Convert.ToDouble(azHomeTextBox.Text);
                Properties.Settings.Default.azHome = azHomeValue;
                Properties.Settings.Default.Save();
            }
            catch
            {
                azHomeTextBox.Text = azHomeValue.ToString("0.0");
            }

        }

        private void elHomeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (elHomeTextBox.Text == "")
            {
                return;
            }
            try
            {
                elHomeValue = Convert.ToDouble(elHomeTextBox.Text);
                Properties.Settings.Default.elHome = elHomeValue;
                Properties.Settings.Default.Save();
            }
            catch
            {
                elHomeTextBox.Text = elHomeValue.ToString("0.0");
            }

        }

        private void rotatorModelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.rotatorModel = rotatorModelComboBox.SelectedIndex;
            Properties.Settings.Default.Save();

        }

        private void RefleshButton_Click(object sender, EventArgs e)
        {
            if (!TncPortConnected && !RadioPortConnected && !RotatorPortConnected)
            {
                //Add port list into each COM port ComboBox
                string[] ports = SerialPort.GetPortNames();
                TncPortComboBox.Items.Clear();
                RadioPortComboBox.Items.Clear();
                RotatorPortComboBox.Items.Clear();
                foreach (string port in ports)
                {
                    TncPortComboBox.Items.Add(port);
                    RadioPortComboBox.Items.Add(port);
                    RotatorPortComboBox.Items.Add(port);
                }

                //Number of COM port is more than 1 -> TNC COM port is enabled
                if (TncPortComboBox.Items.Count >= 1)
                {
                    TncPortComboBox.SelectedIndex = 0;
                    //From BIRDS-1, for TNC Port, added Radio Port
                }

                //Number of COM port is more than 2 -> Radio COM port is enabled
                if (RadioPortComboBox.Items.Count >= 2)
                {
                    RadioPortComboBox.SelectedIndex = 1;
                }

                //Number of COM port is more than 3 -> Rotator COM port is enabled
                if (RotatorPortComboBox.Items.Count >= 3)
                {
                    RotatorPortComboBox.SelectedIndex = 2;
                }

            }
            else
            {
                MessageBox.Show("Please disconnect all com port");
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            SaveReceiveDataButton.Enabled = false;
            ClearRxDataButton_Click(sender, e);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (mTCallSign.Enabled == false)
            {
                mTCallSign.Enabled = true;
            }
            else if (mTCallSign.Enabled == true)
            {
                mTCallSign.Enabled = false;
            }
        }

        private void mTCallSign_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.callsign = mTCallSign.Text;
            Properties.Settings.Default.Save();

            try
            {
                if (digipeaterWindow == null || digipeaterWindow.IsDisposed)
                {

                }
                else
                {
                    digipeaterWindow.MaskedTextBox_SourceCallSign.Text = Properties.Settings.Default.callsign;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_beacon_parser_Click(object sender, EventArgs e)
        {
            if (inputDialogBeacon == null || inputDialogBeacon.IsDisposed)
            {
                inputDialogBeacon = new inputDialogBeacon();
                inputDialogBeacon.Show();
            }
            else
            {
                inputDialogBeacon.BringToFront();
            }
        }

        private void button_hk_data_parser_Click(object sender, EventArgs e)
        {
            if (inputDialogHK == null || inputDialogHK.IsDisposed)
            {
                inputDialogHK = new inputDialogHK();
                inputDialogHK.formMain = this;
                inputDialogHK.Show();
                inputDialogHK.SyncHkLines(hkHistoryLines);
            }
            else
            {
                inputDialogHK.formMain = this;
                inputDialogHK.BringToFront();
            }
        }

        private void button_camera_parser_Click(object sender, EventArgs e)
        {
            if (inputDialogCamera == null || inputDialogCamera.IsDisposed)
            {
                inputDialogCamera = new inputDialogCamera();
                inputDialogCamera.formMain = this;
                inputDialogCamera.Show();
                inputDialogCamera.SyncCameraLines(camHistoryLines);
            }
            else
            {
                inputDialogCamera.formMain = this;
                inputDialogCamera.BringToFront();
            }
        }

        private void button_adcs_parser_Click(object sender, EventArgs e)
        {
            if (inputDialogADCS == null || inputDialogADCS.IsDisposed)
            {
                inputDialogADCS = new inputDialogADCS();
                inputDialogADCS.Show();
            }
            else
            {
                inputDialogADCS.BringToFront();
            }
        }

        private void button_epdm_parser_Click(object sender, EventArgs e)
        {
            if (inputDialogEPDM == null || inputDialogEPDM.IsDisposed)
            {
                inputDialogEPDM = new inputDialogEPDM();
                inputDialogEPDM.Show();
            }
            else
            {
                inputDialogEPDM.BringToFront();
            }
        }

        private void button_cw_decoder_Click(object sender, EventArgs e)
        {
            if (inputDialogCW == null || inputDialogCW.IsDisposed)
            {
                inputDialogCW = new inputDialogCW();
                inputDialogCW.OnCwPanelUpdated += (st, col, rssi, live, last) =>
                {
                    if (lblCwMainState != null)
                    {
                        lblCwMainState.Text = st;
                        lblCwMainState.BackColor = col;
                        lblCwMainRssi.Text = rssi;
                        lblCwMainLive.Text = string.IsNullOrEmpty(live) ? "(waiting for Morse)" : live;
                        lblCwMainLast.Text = string.IsNullOrEmpty(last) ? "—" : last;
                    }
                };
                inputDialogCW.Show();
                inputDialogCW.SyncStateFromMain(
                    lblCwMainState.Text,
                    lblCwMainState.BackColor,
                    lblCwMainRssi.Text,
                    lblCwMainLive.Text,
                    lblCwMainLast.Text,
                    cwHistoryLines);
            }
            else
            {
                inputDialogCW.BringToFront();
            }
        }

        private void UpdateMainCwBanner(string stateTxt = null, int? rssi = null, string live = null, string last = null, Color? color = null)
        {
            if (stateTxt != null && lblCwMainState != null)
            {
                lblCwMainState.Text = stateTxt;
                Color bg = color ?? (
                    (stateTxt == "CARRIER" || stateTxt == "DECODED") ? Color.FromArgb(21, 128, 61) :
                    (stateTxt == "KEYING" || stateTxt == "RECEIVING") ? Color.FromArgb(180, 83, 9) :
                    Color.FromArgb(51, 65, 85));
                lblCwMainState.BackColor = bg;
                lblCwMainState.ForeColor = Color.White;
            }
            if (rssi.HasValue && lblCwMainRssi != null)
            {
                lblCwMainRssi.Text = $"RSSI: {rssi.Value} dBm";
            }
            if (live != null && lblCwMainLive != null)
            {
                lblCwMainLive.Text = string.IsNullOrEmpty(live) ? "(waiting for Morse)" : live;
            }
            if (last != null && lblCwMainLast != null)
            {
                lblCwMainLast.Text = last;
            }
        }

        private void FeedCwSerialChunk(string chunk)
        {
            if (string.IsNullOrEmpty(chunk)) return;
            cwSerialLineBuffer.Append(chunk);
            string buf = cwSerialLineBuffer.ToString();
            int lastNl = buf.LastIndexOfAny(new char[] { '\r', '\n' });
            if (lastNl >= 0)
            {
                string completePart = buf.Substring(0, lastNl + 1);
                cwSerialLineBuffer.Remove(0, lastNl + 1);
                string[] lines = completePart.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    ProcessIncomingCwLine(line);
                }
            }
        }

        private static string TailCw120(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length <= 120 ? s : s.Substring(s.Length - 120);
        }

        public void ProcessIncomingCwLine(string rawLine)
        {
            if (string.IsNullOrWhiteSpace(rawLine)) return;
            string line = rawLine.Trim();

            bool isCwLine = line.Contains("\"CWRX\"") || line.Contains("\"CWSIG\"") ||
                            line.Contains("\"CWCAR\"") || line.Contains("\"CWCHAR\"") ||
                            line.Contains("[CW DECODED]") || line.Contains("[CW]") ||
                            line.Contains("[CW TLM-1]") || line.Contains("[CW TLM-2]") ||
                            line.Contains("9NS2S2") || line.Contains("NEPAL");

            if (isCwLine)
            {
                cwHistoryLines.Add(line);
                if (cwHistoryLines.Count > 500) cwHistoryLines.RemoveAt(0);
            }

            if (inputDialogCW != null && !inputDialogCW.IsDisposed)
            {
                inputDialogCW.FeedLine(line);
            }

            // Capture and forward live HK telemetry packets ({"type":"HK",...} or [FLASH-HEX])
            bool isHkLine = (line.IndexOf("\"type\":\"HK\"", StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (line.IndexOf("[FLASH-HEX]", StringComparison.OrdinalIgnoreCase) >= 0);
            if (isHkLine)
            {
                hkHistoryLines.Add(line);
                if (hkHistoryLines.Count > 500) hkHistoryLines.RemoveAt(0);
                if (inputDialogHK != null && !inputDialogHK.IsDisposed)
                {
                    inputDialogHK.AppendLiveHkLine(line);
                }
            }

            // Capture and forward live Camera chunks ({"type":"CAMERA",...} or [CAMERA-HEX])
            bool isCamLine = (line.IndexOf("\"type\":\"CAMERA\"", StringComparison.OrdinalIgnoreCase) >= 0) ||
                             (line.IndexOf("[CAMERA-HEX]", StringComparison.OrdinalIgnoreCase) >= 0);
            if (isCamLine)
            {
                camHistoryLines.Add(line);
                if (camHistoryLines.Count > 1000) camHistoryLines.RemoveAt(0);
                if (inputDialogCamera != null && !inputDialogCamera.IsDisposed)
                {
                    inputDialogCamera.AppendLiveCameraLine(line);
                }
            }

            // Update main window LIVE CW DECODE panel (matching gs_communicator.py)
            if (line.IndexOf('{') >= 0 && line.EndsWith("}"))
            {
                if (line.Contains("\"CWRX\""))
                {
                    Match m = Regex.Match(line, "\"text\"\\s*:\\s*\"([^\"]*)\"");
                    if (m.Success)
                    {
                        string txt = m.Groups[1].Value.Trim();
                        if (inputDialogCW.IsCwBeaconComplete(txt))
                        {
                            cwAccumMain = txt;
                            UpdateMainCwBanner("DECODED", null, txt, txt, Color.FromArgb(21, 128, 61));
                        }
                        else
                        {
                            if (txt.Length > cwAccumMain.Length) cwAccumMain = txt;
                            string liveTxt = !string.IsNullOrEmpty(cwAccumMain) ? cwAccumMain : txt;
                            string curLast = lblCwMainLast != null ? lblCwMainLast.Text : "—";
                            bool shouldSetLast = (curLast == "—" || string.IsNullOrEmpty(curLast) || curLast == "(waiting for Morse)");
                            UpdateMainCwBanner("RECEIVING", null, TailCw120(liveTxt), shouldSetLast ? txt : null, Color.FromArgb(180, 83, 9));
                        }
                    }
                    return;
                }
                if (line.Contains("\"CWSIG\""))
                {
                    Match mOn = Regex.Match(line, "\"on\"\\s*:\\s*(\\d+)");
                    Match mRssi = Regex.Match(line, "\"rssi\"\\s*:\\s*(-?\\d+)");
                    bool on = mOn.Success && mOn.Groups[1].Value != "0";
                    int? rssi = mRssi.Success ? (int?)int.Parse(mRssi.Groups[1].Value) : null;
                    UpdateMainCwBanner(on ? "CARRIER" : "IDLE", rssi, null, null, on ? Color.FromArgb(21, 128, 61) : Color.FromArgb(51, 65, 85));
                    return;
                }
                if (line.Contains("\"CWCAR\""))
                {
                    Match mRssi = Regex.Match(line, "\"rssi\"\\s*:\\s*(-?\\d+)");
                    int? rssi = mRssi.Success ? (int?)int.Parse(mRssi.Groups[1].Value) : null;
                    string last = inputDialogCW.IsCwBeaconComplete(cwAccumMain) ? cwAccumMain : null;
                    UpdateMainCwBanner("CARRIER", rssi, null, last, Color.FromArgb(21, 128, 61));
                    return;
                }
                if (line.Contains("\"CWCHAR\""))
                {
                    Match mCh = Regex.Match(line, "\"ch\"\\s*:\\s*\"([^\"]*)\"");
                    if (mCh.Success)
                    {
                        string ch = mCh.Groups[1].Value;
                        string cur = cwAccumMain ?? "";
                        if (inputDialogCW.IsCwBeaconComplete(cur))
                        {
                            UpdateMainCwBanner(null, null, null, cur, null);
                            cur = "";
                        }
                        string curLive = lblCwMainLive != null ? lblCwMainLive.Text : "";
                        if (curLive.StartsWith("[") || curLive.StartsWith("(") || curLive.IndexOf("waiting", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            if (string.IsNullOrEmpty(cwAccumMain)) cur = "";
                        }
                        if (ch == " " && (string.IsNullOrEmpty(cur) || cur.EndsWith(" ")))
                        {
                        }
                        else
                        {
                            cur += ch;
                        }
                        cwAccumMain = cur;
                        UpdateMainCwBanner("KEYING", null, TailCw120(cur), null, Color.FromArgb(180, 83, 9));
                    }
                    return;
                }
            }

            if (line.Contains("[CW DECODED]"))
            {
                string txt = line.Substring(line.IndexOf("[CW DECODED]") + "[CW DECODED]".Length).Trim();
                if (inputDialogCW.IsCwBeaconComplete(txt))
                {
                    cwAccumMain = txt;
                    UpdateMainCwBanner("DECODED", null, txt, txt, Color.FromArgb(21, 128, 61));
                }
                else
                {
                    if (txt.Length > cwAccumMain.Length) cwAccumMain = txt;
                    string liveTxt = !string.IsNullOrEmpty(cwAccumMain) ? cwAccumMain : txt;
                    string curLast = lblCwMainLast != null ? lblCwMainLast.Text : "—";
                    bool shouldSetLast = (curLast == "—" || string.IsNullOrEmpty(curLast) || curLast == "(waiting for Morse)");
                    UpdateMainCwBanner("RECEIVING", null, TailCw120(liveTxt), shouldSetLast ? txt : null, Color.FromArgb(180, 83, 9));
                }
                return;
            }

            if (line.Contains("[CW] Carrier detected"))
            {
                Match m = Regex.Match(line, @"RSSI\s+(-?\d+)");
                int? rssi = m.Success ? (int?)int.Parse(m.Groups[1].Value) : null;
                UpdateMainCwBanner("CARRIER", rssi, null, null, Color.FromArgb(21, 128, 61));
                return;
            }

            if (line.Contains("[CW] Tuning carrier"))
            {
                Match m = Regex.Match(line, @"RSSI\s+(-?\d+)");
                int? rssi = m.Success ? (int?)int.Parse(m.Groups[1].Value) : null;
                string last = inputDialogCW.IsCwBeaconComplete(cwAccumMain) ? cwAccumMain : null;
                UpdateMainCwBanner("CARRIER", rssi, null, last, Color.FromArgb(21, 128, 61));
                return;
            }

            if (line.Contains("[CW TLM-1]") || line.Contains("[CW TLM-2]"))
            {
                UpdateMainCwBanner("DECODED", null, null, null, Color.FromArgb(21, 128, 61));
                return;
            }
        }

        public void SetCommandPacketNumber(ushort count)
        {
            string current = Command_HEX.Text.Trim();
            string[] parts = current.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 13)
            {
                parts[11] = ((count >> 8) & 0xFF).ToString("X2");
                parts[12] = (count & 0xFF).ToString("X2");
                Command_HEX.Text = string.Join(" ", parts);
            }
        }

        public void SetCommandAddress(uint addr)
        {
            string current = Command_HEX.Text.Trim();
            string[] parts = current.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 13)
            {
                parts[7] = ((addr >> 24) & 0xFF).ToString("X2");
                parts[8] = ((addr >> 16) & 0xFF).ToString("X2");
                parts[9] = ((addr >> 8) & 0xFF).ToString("X2");
                parts[10] = (addr & 0xFF).ToString("X2");
                Command_HEX.Text = string.Join(" ", parts);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            Console.WriteLine("refersh clicked");

            try
            {

                if (TncPortConnected)
                {
                    TncPort.Close();
                    TncPortConnected = false;
                    TransmitButton.Enabled = false;
                    TncPortComboBox.Enabled = true;
                    TncBaudComboBox.Enabled = true;
                    TncConnectButton.Text = "Connect";

                    int refreshBaud = 115200;
                    string refreshBaudStr = TncBaudComboBox.SelectedItem != null ? TncBaudComboBox.SelectedItem.ToString() : TncBaudComboBox.Text;
                    if (!int.TryParse(refreshBaudStr, out refreshBaud) || refreshBaud <= 0) refreshBaud = 115200;
                    TncPort.PortName = TncPortComboBox.Text;
                    TncPort.BaudRate = refreshBaud;
                    TncPort.DataBits = 8;
                    TncPort.Parity = Parity.None;
                    TncPort.StopBits = StopBits.One;
                    TncPort.DtrEnable = true;
                    if (Convert.ToInt32(TncBaudComboBox.SelectedItem) != 115200)
                    {
                        TncPort.Handshake = Handshake.RequestToSend;
                    }
                    else
                    {
                        TncPort.Handshake = Handshake.None;
                        System.Diagnostics.Trace.WriteLine("Connect ADDNICS TNC");
                    }
                    TncPort.Open();
                    TncPortConnected = true;
                    TransmitButton.Enabled = true;
                    TncPortComboBox.Enabled = false;
                    TncBaudComboBox.Enabled = false;
                    TncConnectButton.Text = "DisConnect";

                }
                else
                {
                    MessageBox.Show($"COM Port not connected: {TncPortComboBox.Text}\n",
                                   "Referesh Failed.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"COM Port not connected: {TncPortComboBox.Text}\n",
                                   "Referesh Failed.");
            }
        }

        private void btn_digipeater_parser_Click(object sender, EventArgs e)
        {
            if (inputDialogDigipeater == null || inputDialogDigipeater.IsDisposed)
            {
                inputDialogDigipeater = new inputDialogDigipeater();
                inputDialogDigipeater.Show();
            }
            else
            {
                inputDialogDigipeater.BringToFront();
            }
        }

        private void btn_LOG_Click(object sender, EventArgs e)
        {
            if (inputDialogLOG == null || inputDialogLOG.IsDisposed)
            {
                inputDialogLOG = new inputDialogLOG();
                inputDialogLOG.Show();
            }
            else
            {
                inputDialogLOG.BringToFront();
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void SatelliteNameComboBoxSub_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (SatelliteNameComboBox.SelectedIndex != SatelliteNameComboBoxSub.SelectedIndex &&
                    SatelliteNameComboBoxSub.SelectedIndex >= 0 &&
                    SatelliteNameComboBoxSub.SelectedIndex < SatelliteNameComboBox.Items.Count)
                {
                    SatelliteNameComboBox.SelectedIndex = SatelliteNameComboBoxSub.SelectedIndex;
                }

                if (string.IsNullOrEmpty(freqPath) || !File.Exists(freqPath) || SatelliteNameComboBoxSub.SelectedItem == null)
                {
                    if (tleList != null && tleList.Count > 0 && selectedTle == null)
                    {
                        selectedTle = tleList[0];
                    }
                    return;
                }

                using (var parser = new TextFieldParser(freqPath))
                {
                    parser.Delimiters = new string[] { "," };
                    while (!parser.EndOfData)
                    {
                        var fields = parser.ReadFields();
                        if (fields != null && fields.Length >= 7 &&
                            fields[0] == SatelliteNameComboBoxSub.SelectedItem.ToString())
                        {
                            noradIdLabel.Text = fields[1];

                            double.TryParse(fields[2], NumberStyles.Any, CultureInfo.InvariantCulture, out cwFreq);
                            double.TryParse(fields[3], NumberStyles.Any, CultureInfo.InvariantCulture, out ulFreq);
                            double.TryParse(fields[4], NumberStyles.Any, CultureInfo.InvariantCulture, out dlFreq);

                            dlFreqLabel.Text = (dlFreq / 1000000).ToString(CultureInfo.InvariantCulture);
                            ulFreqLabel.Text = (ulFreq / 1000000).ToString(CultureInfo.InvariantCulture);
                            remarkLabel.Text = fields[5];
                            folderNameLabel.Text = fields[6];
                            satFolderName = fields[6];

                            if (tleList != null)
                            {
                                foreach (var tle in tleList)
                                {
                                    if (tle.getNoradID() == noradIdLabel.Text)
                                    {
                                        selectedTle = tle;
                                        break;
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)  //Freq List Button
        {
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();

            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            ofd.FileName = "FreqList.csv";
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = "TLE file(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
            //タイトルを設定する
            ofd.Title = "Select the frequency list file";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき、選択されたファイル名を表示する
                Console.WriteLine(ofd.FileName);
                freqPath = ofd.FileName;
                freqPathTextBox.Text = freqPath;
                Properties.Settings.Default.freqPath = freqPath;
                Properties.Settings.Default.Save();
                setSatelliteComboList();
            }

        }

        private void latitudeTextBox_Enter(object sender, EventArgs e)
        {

        }
    }
}
