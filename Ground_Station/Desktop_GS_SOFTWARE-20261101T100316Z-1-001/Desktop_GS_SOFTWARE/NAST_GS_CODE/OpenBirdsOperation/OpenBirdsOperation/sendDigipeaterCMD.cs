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
using System.Xml.Linq;

namespace OpenBirdsOperation
{
    public partial class sendDialogDigipeaterCMD : Form
    {

        private operationMain _mainForm;

        public string savingDigipeaterRootPath;
        public string digipeaterRootPath;
        private Label label_destination_call_sign;
        private MaskedTextBox mTBoxDpDestiCallSign;
        private Label label_path_call_sign;
        private MaskedTextBox mTBoxDpPathCallSign;
        private CheckBox cBox_rx_mode;
        private MaskedTextBox mTDpMessageBox;
        public DateTime dtAppStart = DateTime.UtcNow;

        public RichTextBox RichTextBox_DigipeaterForm
        {
            get { return rTBox_digipeater_raw_data; }
        }

        public MaskedTextBox MaskedTextBox_SourceCallSign
        {
            get { return mTBoxDpSourceCallSign; }
        }

        public sendDialogDigipeaterCMD(operationMain mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;  // Store reference to the main form
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(sendDialogDigipeaterCMD));
            this.mTBoxDpSourceCallSign = new System.Windows.Forms.MaskedTextBox();
            this.label_source_call_sign = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_send_dp_cmd = new System.Windows.Forms.Button();
            this.rTBox_digipeater_raw_data = new System.Windows.Forms.RichTextBox();
            this.button_clear_dp_rtBox = new System.Windows.Forms.Button();
            this.button_save_dp_rtBox = new System.Windows.Forms.Button();
            this.button_cancel_dp_dialog = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label_destination_call_sign = new System.Windows.Forms.Label();
            this.mTBoxDpDestiCallSign = new System.Windows.Forms.MaskedTextBox();
            this.label_path_call_sign = new System.Windows.Forms.Label();
            this.mTBoxDpPathCallSign = new System.Windows.Forms.MaskedTextBox();
            this.cBox_rx_mode = new System.Windows.Forms.CheckBox();
            this.mTDpMessageBox = new System.Windows.Forms.MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // mTBoxDpSourceCallSign
            // 
            this.mTBoxDpSourceCallSign.BackColor = System.Drawing.SystemColors.Window;
            this.mTBoxDpSourceCallSign.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mTBoxDpSourceCallSign.Font = new System.Drawing.Font("MS Gothic", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mTBoxDpSourceCallSign.Location = new System.Drawing.Point(226, 14);
            this.mTBoxDpSourceCallSign.Mask = ">CCCCCCC";
            this.mTBoxDpSourceCallSign.Name = "mTBoxDpSourceCallSign";
            this.mTBoxDpSourceCallSign.Size = new System.Drawing.Size(93, 25);
            this.mTBoxDpSourceCallSign.TabIndex = 0;
            this.mTBoxDpSourceCallSign.TextChanged += new System.EventHandler(this.mTBoxDpSourceCallSign_TextChanged_1);
            // 
            // label_source_call_sign
            // 
            this.label_source_call_sign.AutoSize = true;
            this.label_source_call_sign.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_source_call_sign.Location = new System.Drawing.Point(12, 18);
            this.label_source_call_sign.Name = "label_source_call_sign";
            this.label_source_call_sign.Size = new System.Drawing.Size(134, 16);
            this.label_source_call_sign.TabIndex = 1;
            this.label_source_call_sign.Text = "Source Call Sign";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Message";
            // 
            // button_send_dp_cmd
            // 
            this.button_send_dp_cmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_send_dp_cmd.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_send_dp_cmd.Location = new System.Drawing.Point(392, 14);
            this.button_send_dp_cmd.Name = "button_send_dp_cmd";
            this.button_send_dp_cmd.Size = new System.Drawing.Size(94, 73);
            this.button_send_dp_cmd.TabIndex = 4;
            this.button_send_dp_cmd.Text = "SEND";
            this.button_send_dp_cmd.UseVisualStyleBackColor = true;
            this.button_send_dp_cmd.Click += new System.EventHandler(this.button_send_dp_cmd_Click_1);
            // 
            // rTBox_digipeater_raw_data
            // 
            this.rTBox_digipeater_raw_data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rTBox_digipeater_raw_data.Location = new System.Drawing.Point(16, 151);
            this.rTBox_digipeater_raw_data.Name = "rTBox_digipeater_raw_data";
            this.rTBox_digipeater_raw_data.Size = new System.Drawing.Size(470, 281);
            this.rTBox_digipeater_raw_data.TabIndex = 5;
            this.rTBox_digipeater_raw_data.Text = "";
            // 
            // button_clear_dp_rtBox
            // 
            this.button_clear_dp_rtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_clear_dp_rtBox.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_clear_dp_rtBox.Location = new System.Drawing.Point(26, 438);
            this.button_clear_dp_rtBox.Name = "button_clear_dp_rtBox";
            this.button_clear_dp_rtBox.Size = new System.Drawing.Size(94, 37);
            this.button_clear_dp_rtBox.TabIndex = 6;
            this.button_clear_dp_rtBox.Text = "Clear";
            this.button_clear_dp_rtBox.UseVisualStyleBackColor = true;
            this.button_clear_dp_rtBox.Click += new System.EventHandler(this.button_clear_dp_rtBox_Click_1);
            // 
            // button_save_dp_rtBox
            // 
            this.button_save_dp_rtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save_dp_rtBox.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_save_dp_rtBox.Location = new System.Drawing.Point(381, 438);
            this.button_save_dp_rtBox.Name = "button_save_dp_rtBox";
            this.button_save_dp_rtBox.Size = new System.Drawing.Size(94, 37);
            this.button_save_dp_rtBox.TabIndex = 7;
            this.button_save_dp_rtBox.Text = "Save";
            this.button_save_dp_rtBox.UseVisualStyleBackColor = true;
            this.button_save_dp_rtBox.Click += new System.EventHandler(this.button_save_dp_rtBox_Click_1);
            // 
            // button_cancel_dp_dialog
            // 
            this.button_cancel_dp_dialog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel_dp_dialog.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_cancel_dp_dialog.Location = new System.Drawing.Point(208, 438);
            this.button_cancel_dp_dialog.Name = "button_cancel_dp_dialog";
            this.button_cancel_dp_dialog.Size = new System.Drawing.Size(94, 37);
            this.button_cancel_dp_dialog.TabIndex = 8;
            this.button_cancel_dp_dialog.Text = "Cancel";
            this.button_cancel_dp_dialog.UseVisualStyleBackColor = true;
            this.button_cancel_dp_dialog.Click += new System.EventHandler(this.button_cancel_dp_dialog_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(327, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(49, 46);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // label_destination_call_sign
            // 
            this.label_destination_call_sign.AutoSize = true;
            this.label_destination_call_sign.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_destination_call_sign.Location = new System.Drawing.Point(12, 52);
            this.label_destination_call_sign.Name = "label_destination_call_sign";
            this.label_destination_call_sign.Size = new System.Drawing.Size(166, 16);
            this.label_destination_call_sign.TabIndex = 11;
            this.label_destination_call_sign.Text = "Destination Call Sign";
            // 
            // mTBoxDpDestiCallSign
            // 
            this.mTBoxDpDestiCallSign.BackColor = System.Drawing.SystemColors.Window;
            this.mTBoxDpDestiCallSign.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mTBoxDpDestiCallSign.Font = new System.Drawing.Font("MS Gothic", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mTBoxDpDestiCallSign.Location = new System.Drawing.Point(226, 48);
            this.mTBoxDpDestiCallSign.Mask = ">CCCCCCC";
            this.mTBoxDpDestiCallSign.Name = "mTBoxDpDestiCallSign";
            this.mTBoxDpDestiCallSign.Size = new System.Drawing.Size(93, 25);
            this.mTBoxDpDestiCallSign.TabIndex = 10;
            // 
            // label_path_call_sign
            // 
            this.label_path_call_sign.AutoSize = true;
            this.label_path_call_sign.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_path_call_sign.Location = new System.Drawing.Point(12, 88);
            this.label_path_call_sign.Name = "label_path_call_sign";
            this.label_path_call_sign.Size = new System.Drawing.Size(113, 16);
            this.label_path_call_sign.TabIndex = 13;
            this.label_path_call_sign.Text = "Message Path";
            // 
            // mTBoxDpPathCallSign
            // 
            this.mTBoxDpPathCallSign.BackColor = System.Drawing.SystemColors.Window;
            this.mTBoxDpPathCallSign.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mTBoxDpPathCallSign.Enabled = false;
            this.mTBoxDpPathCallSign.Font = new System.Drawing.Font("MS Gothic", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mTBoxDpPathCallSign.Location = new System.Drawing.Point(226, 84);
            this.mTBoxDpPathCallSign.Mask = ">CCCCCCC";
            this.mTBoxDpPathCallSign.Name = "mTBoxDpPathCallSign";
            this.mTBoxDpPathCallSign.Size = new System.Drawing.Size(93, 25);
            this.mTBoxDpPathCallSign.TabIndex = 12;
            this.mTBoxDpPathCallSign.Text = "WIDE1-1";
            // 
            // cBox_rx_mode
            // 
            this.cBox_rx_mode.AutoSize = true;
            this.cBox_rx_mode.Location = new System.Drawing.Point(368, 122);
            this.cBox_rx_mode.Name = "cBox_rx_mode";
            this.cBox_rx_mode.Size = new System.Drawing.Size(77, 17);
            this.cBox_rx_mode.TabIndex = 14;
            this.cBox_rx_mode.Text = "RX Station";
            this.cBox_rx_mode.UseVisualStyleBackColor = true;
            this.cBox_rx_mode.CheckedChanged += new System.EventHandler(this.cBox_rx_mode_CheckedChanged_1);
            // 
            // mTDpMessageBox
            // 
            this.mTDpMessageBox.BackColor = System.Drawing.SystemColors.Window;
            this.mTDpMessageBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mTDpMessageBox.Font = new System.Drawing.Font("MS Gothic", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mTDpMessageBox.Location = new System.Drawing.Point(89, 114);
            this.mTDpMessageBox.Mask = ">CCCCCCCCCCCCCCCCCCCC";
            this.mTDpMessageBox.Name = "mTDpMessageBox";
            this.mTDpMessageBox.Size = new System.Drawing.Size(230, 25);
            this.mTDpMessageBox.TabIndex = 15;
            // 
            // sendDialogDigipeaterCMD
            // 
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(498, 485);
            this.ControlBox = false;
            this.Controls.Add(this.mTDpMessageBox);
            this.Controls.Add(this.cBox_rx_mode);
            this.Controls.Add(this.label_path_call_sign);
            this.Controls.Add(this.mTBoxDpPathCallSign);
            this.Controls.Add(this.label_destination_call_sign);
            this.Controls.Add(this.mTBoxDpDestiCallSign);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_cancel_dp_dialog);
            this.Controls.Add(this.button_save_dp_rtBox);
            this.Controls.Add(this.button_clear_dp_rtBox);
            this.Controls.Add(this.rTBox_digipeater_raw_data);
            this.Controls.Add(this.button_send_dp_cmd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_source_call_sign);
            this.Controls.Add(this.mTBoxDpSourceCallSign);
            try { this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon"))); } catch { }
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "sendDialogDigipeaterCMD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Digipeater Mode";
            this.Load += new System.EventHandler(this.sendDialogDigipeaterCMD_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private MaskedTextBox mTBoxDpSourceCallSign;
        private Label label1;
        private Button button_send_dp_cmd;
        private RichTextBox rTBox_digipeater_raw_data;
        private Button button_clear_dp_rtBox;
        private Button button_save_dp_rtBox;
        private Button button_cancel_dp_dialog;
        private PictureBox pictureBox1;
        private Label label_source_call_sign;

        public async void transmitDpAsync()
        {

            // Disable the button
            button_send_dp_cmd.Enabled = false;
            // Run a task with a delay (e.g., 2000 ms delay)
            await Task.Run(async () =>
            {
                // Wait for (777 milliseconds) prevent multiple tx at a time
                await Task.Delay(777);
            });

            string source_call_sign = mTBoxDpSourceCallSign.Text;
            string destination_call_sign = mTBoxDpDestiCallSign.Text;
            string call_sign_path = mTBoxDpPathCallSign.Text;
            string message = mTDpMessageBox.Text;

            if (!string.IsNullOrEmpty(source_call_sign) && !string.IsNullOrEmpty(call_sign_path) && !string.IsNullOrEmpty(message))
            {

                if (string.IsNullOrEmpty(destination_call_sign))
                {
                    destination_call_sign = "NOCALL ";
                }

                //var send_to_radio = System.IO.Path.Combine(destination_call_sign, source_call_sign, call_sign_path, message);
                //  var send_to_radio = source_call_sign + ">" + destination_call_sign + "," + call_sign_path + ":" + message;
                var send_to_radio = "S2S" + destination_call_sign + ">" + source_call_sign + "," + call_sign_path + ":" + message;

                _mainForm.ChangeUplinkFrequency();
                await Task.Delay(750);

                _mainForm.ReceiveData(send_to_radio);


                await Task.Delay(750);
                _mainForm.ChangeDownlinkFrequency(); 

                DateTime dt = DateTime.Now;
                string time = dt.ToString("yyyy/MM/dd HH:mm:ss");

                this.rTBox_digipeater_raw_data.Focus();
                this.rTBox_digipeater_raw_data.AppendText("#(" + time + ")" + " DP-CMD:\n" + source_call_sign + ">" + destination_call_sign + "," + call_sign_path + ":" + message + "\n\n");

            }
            else
            {
                MessageBox.Show(
                        "Digipeater Command not complete\n",
                        "Invalid Command",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button2);
            }
            // Enable the button after the delay
            button_send_dp_cmd.Invoke((Action)(() => button_send_dp_cmd.Enabled = true));
        }

        private void button_cancel_dp_dialog_Click_1(object sender, EventArgs e)
        {
            _mainForm.TELEMETRY_FLAG = true;
            _mainForm.DIGIPEATER_FLAG = false;

            _mainForm.FmData.Checked = true;

            rTBox_digipeater_raw_data.Text = "";
            this.Close();
        }

        private void button_clear_dp_rtBox_Click_1(object sender, EventArgs e)
        {
            rTBox_digipeater_raw_data.Text = "";
        }

        private void button_save_dp_rtBox_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(rTBox_digipeater_raw_data.Text))
            {
                try
                {
                    string rawDigipeaterRx = rTBox_digipeater_raw_data.Text;

                    string result;

                    result = rawDigipeaterRx;

                    Console.WriteLine(result);

                    savingDigipeaterRootPath = "../Storage/DIGIPEATER_LOG";
                    Properties.Settings.Default.savePathDigipeaterLog = savingDigipeaterRootPath;
                    Properties.Settings.Default.Save();

                    digipeaterRootPath = Properties.Settings.Default.savePathDigipeaterLog;

                    savingDigipeaterRootPath = System.IO.Path.Combine(digipeaterRootPath, dtAppStart.ToString("yyyyMMdd"));

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

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    fileName = time + "_Digipeater_raw.txt";
                    absolutePath = System.IO.Path.Combine(folderPath, fileName);
                    StreamWriter sw = new StreamWriter(absolutePath, false, Encoding.ASCII);
                    sw.Write(rawRxData);
                    sw.Close();

                    rTBox_digipeater_raw_data.Text = "";
                    this.Close();

                    DialogResult result_2 = MessageBox.Show(
                         $"Digipeater Data Stored to {absolutePath}\n Click 'OK' to preview",
                         "Saving completed!",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1
                    );

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

                    _mainForm.TELEMETRY_FLAG = true;
                    _mainForm.DIGIPEATER_FLAG = false;

                    _mainForm.FmData.Checked = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                           $"Digipeater Operation Invalid \n {ex.Message}",
                           "Please input valid raw data",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Exclamation,
                           MessageBoxDefaultButton.Button1
                    );
                }
            }
            else
            {
                MessageBox.Show(
                         "Digipeater Operation Invalid \n",
                         "Empty Field",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Exclamation,
                         MessageBoxDefaultButton.Button1
                );
            }
        }

        private void button_send_dp_cmd_Click_1(object sender, EventArgs e)
        {
            transmitDpAsync();
        }

        private void cBox_rx_mode_CheckedChanged_1(object sender, EventArgs e)
        {
            if (cBox_rx_mode.Checked)
            {
                mTBoxDpSourceCallSign.Enabled = false;
                mTBoxDpDestiCallSign.Enabled = false;
                mTBoxDpPathCallSign.Enabled = false;
                mTDpMessageBox.Enabled = false;
                button_send_dp_cmd.Enabled = false;

                _mainForm.ReceiveData("MODEDIGI");

                _mainForm.TELEMETRY_FLAG = false;
                _mainForm.DIGIPEATER_FLAG = true;
            }
            else
            {
                mTBoxDpSourceCallSign.Enabled = true;
                mTBoxDpDestiCallSign.Enabled = true;
                mTBoxDpPathCallSign.Enabled = false;
                mTDpMessageBox.Enabled = true;
                button_send_dp_cmd.Enabled = true;

                _mainForm.ReceiveData("MODETELE");

                _mainForm.TELEMETRY_FLAG = true;
                _mainForm.DIGIPEATER_FLAG = false;
            }
        }

        private void mTBoxDpSourceCallSign_TextChanged_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.callsign = mTBoxDpSourceCallSign.Text;
            Properties.Settings.Default.Save();
            _mainForm.TextBoxCallSign.Text = Properties.Settings.Default.callsign;
        }

        private void sendDialogDigipeaterCMD_Load_1(object sender, EventArgs e)
        {
            mTBoxDpSourceCallSign.Text = Properties.Settings.Default.callsign;
        }
    }
}
