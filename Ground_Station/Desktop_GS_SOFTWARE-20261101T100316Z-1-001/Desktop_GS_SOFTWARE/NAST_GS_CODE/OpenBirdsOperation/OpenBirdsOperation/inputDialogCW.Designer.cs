using System.Drawing;
using System.Windows.Forms;

namespace OpenBirdsOperation
{
    partial class inputDialogCW
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.lblState = new System.Windows.Forms.Label();
            this.lblRssi = new System.Windows.Forms.Label();
            this.lblLiveHeader = new System.Windows.Forms.Label();
            this.lblLiveText = new System.Windows.Forms.Label();
            this.lblLastHeader = new System.Windows.Forms.Label();
            this.lblLastText = new System.Windows.Forms.Label();
            this.groupBoxCW1 = new System.Windows.Forms.GroupBox();
            this.lblBatV = new System.Windows.Forms.Label();
            this.txtBatV = new System.Windows.Forms.TextBox();
            this.lblSolV = new System.Windows.Forms.Label();
            this.txtSolV = new System.Windows.Forms.TextBox();
            this.lblAntT = new System.Windows.Forms.Label();
            this.txtAntT = new System.Windows.Forms.TextBox();
            this.lblBatT = new System.Windows.Forms.Label();
            this.txtBatT = new System.Windows.Forms.TextBox();
            this.lblBpbt = new System.Windows.Forms.Label();
            this.txtBpbt = new System.Windows.Forms.TextBox();
            this.groupBoxCW2 = new System.Windows.Forms.GroupBox();
            this.lblI3v3 = new System.Windows.Forms.Label();
            this.txtI3v3 = new System.Windows.Forms.TextBox();
            this.lblSolI = new System.Windows.Forms.Label();
            this.txtSolI = new System.Windows.Forms.TextBox();
            this.lblBatI = new System.Windows.Forms.Label();
            this.txtBatI = new System.Windows.Forms.TextBox();
            this.lblFlag1 = new System.Windows.Forms.Label();
            this.txtFlag1 = new System.Windows.Forms.TextBox();
            this.lblFlag2 = new System.Windows.Forms.Label();
            this.txtFlag2 = new System.Windows.Forms.TextBox();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.rTBoxCwLog = new System.Windows.Forms.RichTextBox();
            this.btnDecode = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBoxStatus.SuspendLayout();
            this.groupBoxCW1.SuspendLayout();
            this.groupBoxCW2.SuspendLayout();
            this.groupBoxLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.groupBoxStatus.Controls.Add(this.lblLastText);
            this.groupBoxStatus.Controls.Add(this.lblLastHeader);
            this.groupBoxStatus.Controls.Add(this.lblLiveText);
            this.groupBoxStatus.Controls.Add(this.lblLiveHeader);
            this.groupBoxStatus.Controls.Add(this.lblRssi);
            this.groupBoxStatus.Controls.Add(this.lblState);
            this.groupBoxStatus.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.groupBoxStatus.Location = new System.Drawing.Point(12, 12);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Size = new System.Drawing.Size(860, 115);
            this.groupBoxStatus.TabIndex = 0;
            this.groupBoxStatus.TabStop = false;
            this.groupBoxStatus.Text = "CW DECODER 435.000 MHz (Slipper2Sat / 9NS2S2)";
            // 
            // lblState
            // 
            this.lblState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblState.Font = new System.Drawing.Font("MS Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblState.ForeColor = System.Drawing.Color.White;
            this.lblState.Location = new System.Drawing.Point(15, 23);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(120, 24);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "IDLE";
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRssi
            // 
            this.lblRssi.AutoSize = true;
            this.lblRssi.Font = new System.Drawing.Font("MS Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRssi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(191)))), ((int)(((byte)(36)))));
            this.lblRssi.Location = new System.Drawing.Point(150, 28);
            this.lblRssi.Name = "lblRssi";
            this.lblRssi.Size = new System.Drawing.Size(124, 15);
            this.lblRssi.TabIndex = 1;
            this.lblRssi.Text = "RSSI: -- dBm";
            // 
            // lblLiveHeader
            // 
            this.lblLiveHeader.AutoSize = true;
            this.lblLiveHeader.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLiveHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblLiveHeader.Location = new System.Drawing.Point(15, 56);
            this.lblLiveHeader.Name = "lblLiveHeader";
            this.lblLiveHeader.Size = new System.Drawing.Size(42, 13);
            this.lblLiveHeader.TabIndex = 2;
            this.lblLiveHeader.Text = "LIVE:";
            // 
            // lblLiveText
            // 
            this.lblLiveText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLiveText.Font = new System.Drawing.Font("MS Gothic", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLiveText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(222)))), ((int)(((byte)(128)))));
            this.lblLiveText.Location = new System.Drawing.Point(63, 54);
            this.lblLiveText.Name = "lblLiveText";
            this.lblLiveText.Size = new System.Drawing.Size(780, 20);
            this.lblLiveText.TabIndex = 3;
            this.lblLiveText.Text = "(waiting for Morse beacon)";
            // 
            // lblLastHeader
            // 
            this.lblLastHeader.AutoSize = true;
            this.lblLastHeader.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblLastHeader.Location = new System.Drawing.Point(15, 82);
            this.lblLastHeader.Name = "lblLastHeader";
            this.lblLastHeader.Size = new System.Drawing.Size(42, 13);
            this.lblLastHeader.TabIndex = 4;
            this.lblLastHeader.Text = "LAST:";
            // 
            // lblLastText
            // 
            this.lblLastText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastText.Font = new System.Drawing.Font("MS Gothic", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.lblLastText.Location = new System.Drawing.Point(63, 80);
            this.lblLastText.Name = "lblLastText";
            this.lblLastText.Size = new System.Drawing.Size(780, 20);
            this.lblLastText.TabIndex = 5;
            this.lblLastText.Text = "—";
            // 
            // groupBoxCW1
            // 
            this.groupBoxCW1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCW1.Controls.Add(this.txtBpbt);
            this.groupBoxCW1.Controls.Add(this.lblBpbt);
            this.groupBoxCW1.Controls.Add(this.txtBatT);
            this.groupBoxCW1.Controls.Add(this.lblBatT);
            this.groupBoxCW1.Controls.Add(this.txtAntT);
            this.groupBoxCW1.Controls.Add(this.lblAntT);
            this.groupBoxCW1.Controls.Add(this.txtSolV);
            this.groupBoxCW1.Controls.Add(this.lblSolV);
            this.groupBoxCW1.Controls.Add(this.txtBatV);
            this.groupBoxCW1.Controls.Add(this.lblBatV);
            this.groupBoxCW1.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxCW1.Location = new System.Drawing.Point(12, 133);
            this.groupBoxCW1.Name = "groupBoxCW1";
            this.groupBoxCW1.Size = new System.Drawing.Size(860, 65);
            this.groupBoxCW1.TabIndex = 1;
            this.groupBoxCW1.TabStop = false;
            this.groupBoxCW1.Text = "CW-1 Telemetry: Voltages & Temperatures (Format: 9NS2S2 NEPAL [BatV] [SolV] [AntT] [BatT] [BPBT])";
            // 
            // lblBatV
            // 
            this.lblBatV.AutoSize = true;
            this.lblBatV.Location = new System.Drawing.Point(15, 30);
            this.lblBatV.Name = "lblBatV";
            this.lblBatV.Size = new System.Drawing.Size(55, 13);
            this.lblBatV.TabIndex = 0;
            this.lblBatV.Text = "BatV:";
            // 
            // txtBatV
            // 
            this.txtBatV.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBatV.Location = new System.Drawing.Point(75, 27);
            this.txtBatV.Name = "txtBatV";
            this.txtBatV.ReadOnly = true;
            this.txtBatV.Size = new System.Drawing.Size(85, 20);
            this.txtBatV.TabIndex = 1;
            this.txtBatV.Text = "-- V";
            this.txtBatV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSolV
            // 
            this.lblSolV.AutoSize = true;
            this.lblSolV.Location = new System.Drawing.Point(180, 30);
            this.lblSolV.Name = "lblSolV";
            this.lblSolV.Size = new System.Drawing.Size(55, 13);
            this.lblSolV.TabIndex = 2;
            this.lblSolV.Text = "SolV:";
            // 
            // txtSolV
            // 
            this.txtSolV.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSolV.Location = new System.Drawing.Point(240, 27);
            this.txtSolV.Name = "txtSolV";
            this.txtSolV.ReadOnly = true;
            this.txtSolV.Size = new System.Drawing.Size(85, 20);
            this.txtSolV.TabIndex = 3;
            this.txtSolV.Text = "-- V";
            this.txtSolV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblAntT
            // 
            this.lblAntT.AutoSize = true;
            this.lblAntT.Location = new System.Drawing.Point(345, 30);
            this.lblAntT.Name = "lblAntT";
            this.lblAntT.Size = new System.Drawing.Size(55, 13);
            this.lblAntT.TabIndex = 4;
            this.lblAntT.Text = "AntT:";
            // 
            // txtAntT
            // 
            this.txtAntT.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAntT.Location = new System.Drawing.Point(405, 27);
            this.txtAntT.Name = "txtAntT";
            this.txtAntT.ReadOnly = true;
            this.txtAntT.Size = new System.Drawing.Size(85, 20);
            this.txtAntT.TabIndex = 5;
            this.txtAntT.Text = "-- °C";
            this.txtAntT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblBatT
            // 
            this.lblBatT.AutoSize = true;
            this.lblBatT.Location = new System.Drawing.Point(510, 30);
            this.lblBatT.Name = "lblBatT";
            this.lblBatT.Size = new System.Drawing.Size(55, 13);
            this.lblBatT.TabIndex = 6;
            this.lblBatT.Text = "BatT:";
            // 
            // txtBatT
            // 
            this.txtBatT.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBatT.Location = new System.Drawing.Point(570, 27);
            this.txtBatT.Name = "txtBatT";
            this.txtBatT.ReadOnly = true;
            this.txtBatT.Size = new System.Drawing.Size(85, 20);
            this.txtBatT.TabIndex = 7;
            this.txtBatT.Text = "-- °C";
            this.txtBatT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblBpbt
            // 
            this.lblBpbt.AutoSize = true;
            this.lblBpbt.Location = new System.Drawing.Point(675, 30);
            this.lblBpbt.Name = "lblBpbt";
            this.lblBpbt.Size = new System.Drawing.Size(55, 13);
            this.lblBpbt.TabIndex = 8;
            this.lblBpbt.Text = "BPBT:";
            // 
            // txtBpbt
            // 
            this.txtBpbt.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBpbt.Location = new System.Drawing.Point(735, 27);
            this.txtBpbt.Name = "txtBpbt";
            this.txtBpbt.ReadOnly = true;
            this.txtBpbt.Size = new System.Drawing.Size(85, 20);
            this.txtBpbt.TabIndex = 9;
            this.txtBpbt.Text = "-- °C";
            this.txtBpbt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxCW2
            // 
            this.groupBoxCW2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCW2.Controls.Add(this.txtFlag2);
            this.groupBoxCW2.Controls.Add(this.lblFlag2);
            this.groupBoxCW2.Controls.Add(this.txtFlag1);
            this.groupBoxCW2.Controls.Add(this.lblFlag1);
            this.groupBoxCW2.Controls.Add(this.txtBatI);
            this.groupBoxCW2.Controls.Add(this.lblBatI);
            this.groupBoxCW2.Controls.Add(this.txtSolI);
            this.groupBoxCW2.Controls.Add(this.lblSolI);
            this.groupBoxCW2.Controls.Add(this.txtI3v3);
            this.groupBoxCW2.Controls.Add(this.lblI3v3);
            this.groupBoxCW2.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxCW2.Location = new System.Drawing.Point(12, 204);
            this.groupBoxCW2.Name = "groupBoxCW2";
            this.groupBoxCW2.Size = new System.Drawing.Size(860, 65);
            this.groupBoxCW2.TabIndex = 2;
            this.groupBoxCW2.TabStop = false;
            this.groupBoxCW2.Text = "CW-2 Telemetry: Currents & Status (Format: 9NS2S2 NEPAL [3V3I] [SolI] [BatI] [Flag1] [Flag2])";
            // 
            // lblI3v3
            // 
            this.lblI3v3.AutoSize = true;
            this.lblI3v3.Location = new System.Drawing.Point(15, 30);
            this.lblI3v3.Name = "lblI3v3";
            this.lblI3v3.Size = new System.Drawing.Size(55, 13);
            this.lblI3v3.TabIndex = 0;
            this.lblI3v3.Text = "3V3I:";
            // 
            // txtI3v3
            // 
            this.txtI3v3.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtI3v3.Location = new System.Drawing.Point(75, 27);
            this.txtI3v3.Name = "txtI3v3";
            this.txtI3v3.ReadOnly = true;
            this.txtI3v3.Size = new System.Drawing.Size(85, 20);
            this.txtI3v3.TabIndex = 1;
            this.txtI3v3.Text = "-- A";
            this.txtI3v3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSolI
            // 
            this.lblSolI.AutoSize = true;
            this.lblSolI.Location = new System.Drawing.Point(180, 30);
            this.lblSolI.Name = "lblSolI";
            this.lblSolI.Size = new System.Drawing.Size(55, 13);
            this.lblSolI.TabIndex = 2;
            this.lblSolI.Text = "SolI:";
            // 
            // txtSolI
            // 
            this.txtSolI.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSolI.Location = new System.Drawing.Point(240, 27);
            this.txtSolI.Name = "txtSolI";
            this.txtSolI.ReadOnly = true;
            this.txtSolI.Size = new System.Drawing.Size(85, 20);
            this.txtSolI.TabIndex = 3;
            this.txtSolI.Text = "-- A";
            this.txtSolI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblBatI
            // 
            this.lblBatI.AutoSize = true;
            this.lblBatI.Location = new System.Drawing.Point(345, 30);
            this.lblBatI.Name = "lblBatI";
            this.lblBatI.Size = new System.Drawing.Size(55, 13);
            this.lblBatI.TabIndex = 4;
            this.lblBatI.Text = "BatI:";
            // 
            // txtBatI
            // 
            this.txtBatI.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBatI.Location = new System.Drawing.Point(405, 27);
            this.txtBatI.Name = "txtBatI";
            this.txtBatI.ReadOnly = true;
            this.txtBatI.Size = new System.Drawing.Size(85, 20);
            this.txtBatI.TabIndex = 5;
            this.txtBatI.Text = "-- A";
            this.txtBatI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFlag1
            // 
            this.lblFlag1.AutoSize = true;
            this.lblFlag1.Location = new System.Drawing.Point(510, 30);
            this.lblFlag1.Name = "lblFlag1";
            this.lblFlag1.Size = new System.Drawing.Size(55, 13);
            this.lblFlag1.TabIndex = 6;
            this.lblFlag1.Text = "Flag1:";
            // 
            // txtFlag1
            // 
            this.txtFlag1.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFlag1.Location = new System.Drawing.Point(570, 27);
            this.txtFlag1.Name = "txtFlag1";
            this.txtFlag1.ReadOnly = true;
            this.txtFlag1.Size = new System.Drawing.Size(85, 20);
            this.txtFlag1.TabIndex = 7;
            this.txtFlag1.Text = "--";
            this.txtFlag1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFlag2
            // 
            this.lblFlag2.AutoSize = true;
            this.lblFlag2.Location = new System.Drawing.Point(675, 30);
            this.lblFlag2.Name = "lblFlag2";
            this.lblFlag2.Size = new System.Drawing.Size(55, 13);
            this.lblFlag2.TabIndex = 8;
            this.lblFlag2.Text = "Flag2:";
            // 
            // txtFlag2
            // 
            this.txtFlag2.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFlag2.Location = new System.Drawing.Point(735, 27);
            this.txtFlag2.Name = "txtFlag2";
            this.txtFlag2.ReadOnly = true;
            this.txtFlag2.Size = new System.Drawing.Size(85, 20);
            this.txtFlag2.TabIndex = 9;
            this.txtFlag2.Text = "--";
            this.txtFlag2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxLog
            // 
            this.groupBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLog.Controls.Add(this.rTBoxCwLog);
            this.groupBoxLog.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxLog.Location = new System.Drawing.Point(12, 275);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Size = new System.Drawing.Size(860, 240);
            this.groupBoxLog.TabIndex = 3;
            this.groupBoxLog.TabStop = false;
            this.groupBoxLog.Text = "CW Live Morse / Raw Serial Log / Text Input";
            // 
            // rTBoxCwLog
            // 
            this.rTBoxCwLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.rTBoxCwLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rTBoxCwLog.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rTBoxCwLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.rTBoxCwLog.Location = new System.Drawing.Point(3, 16);
            this.rTBoxCwLog.Name = "rTBoxCwLog";
            this.rTBoxCwLog.Size = new System.Drawing.Size(854, 221);
            this.rTBoxCwLog.TabIndex = 0;
            this.rTBoxCwLog.Text = "";
            // 
            // btnDecode
            // 
            this.btnDecode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDecode.Font = new System.Drawing.Font("MS Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDecode.Location = new System.Drawing.Point(12, 525);
            this.btnDecode.Name = "btnDecode";
            this.btnDecode.Size = new System.Drawing.Size(210, 35);
            this.btnDecode.TabIndex = 4;
            this.btnDecode.Text = "Decode Text / Morse Log";
            this.btnDecode.UseVisualStyleBackColor = true;
            this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Font = new System.Drawing.Font("MS Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(235, 525);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(110, 35);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Font = new System.Drawing.Font("MS Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(622, 525);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 35);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "Export to File";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("MS Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(752, 525);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 35);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // inputDialogCW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 572);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDecode);
            this.Controls.Add(this.groupBoxLog);
            this.Controls.Add(this.groupBoxCW2);
            this.Controls.Add(this.groupBoxCW1);
            this.Controls.Add(this.groupBoxStatus);
            this.Name = "inputDialogCW";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CW Morse & Telemetry Decoder (9NS2S2 / Slipper2Sat)";
            this.Load += new System.EventHandler(this.inputDialogCW_Load);
            this.groupBoxStatus.ResumeLayout(false);
            this.groupBoxStatus.PerformLayout();
            this.groupBoxCW1.ResumeLayout(false);
            this.groupBoxCW1.PerformLayout();
            this.groupBoxCW2.ResumeLayout(false);
            this.groupBoxCW2.PerformLayout();
            this.groupBoxLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.GroupBox groupBoxStatus;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblRssi;
        private System.Windows.Forms.Label lblLiveHeader;
        private System.Windows.Forms.Label lblLiveText;
        private System.Windows.Forms.Label lblLastHeader;
        private System.Windows.Forms.Label lblLastText;
        private System.Windows.Forms.GroupBox groupBoxCW1;
        private System.Windows.Forms.Label lblBatV;
        private System.Windows.Forms.TextBox txtBatV;
        private System.Windows.Forms.Label lblSolV;
        private System.Windows.Forms.TextBox txtSolV;
        private System.Windows.Forms.Label lblAntT;
        private System.Windows.Forms.TextBox txtAntT;
        private System.Windows.Forms.Label lblBatT;
        private System.Windows.Forms.TextBox txtBatT;
        private System.Windows.Forms.Label lblBpbt;
        private System.Windows.Forms.TextBox txtBpbt;
        private System.Windows.Forms.GroupBox groupBoxCW2;
        private System.Windows.Forms.Label lblI3v3;
        private System.Windows.Forms.TextBox txtI3v3;
        private System.Windows.Forms.Label lblSolI;
        private System.Windows.Forms.TextBox txtSolI;
        private System.Windows.Forms.Label lblBatI;
        private System.Windows.Forms.TextBox txtBatI;
        private System.Windows.Forms.Label lblFlag1;
        private System.Windows.Forms.TextBox txtFlag1;
        private System.Windows.Forms.Label lblFlag2;
        private System.Windows.Forms.TextBox txtFlag2;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.RichTextBox rTBoxCwLog;
        private System.Windows.Forms.Button btnDecode;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnClose;
    }
}
