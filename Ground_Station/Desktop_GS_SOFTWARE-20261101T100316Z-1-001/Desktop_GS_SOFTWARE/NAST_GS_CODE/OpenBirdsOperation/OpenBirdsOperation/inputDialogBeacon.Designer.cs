using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    partial class inputDialogBeacon : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(inputDialogBeacon));
            this.rTBox_beacon_raw_data = new System.Windows.Forms.RichTextBox();
            this.button_beacon_paraser_ok = new System.Windows.Forms.Button();
            this.button_cancel_beacon = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label_call_sign_ans = new System.Windows.Forms.Label();
            this.label_call_sign = new System.Windows.Forms.Label();
            this.label_time_hour_ans = new System.Windows.Forms.Label();
            this.label_time_hour = new System.Windows.Forms.Label();
            this.label_time_day_ans = new System.Windows.Forms.Label();
            this.label_time_day = new System.Windows.Forms.Label();
            this.label_type_ans = new System.Windows.Forms.Label();
            this.label_type = new System.Windows.Forms.Label();
            this.label_head_ans = new System.Windows.Forms.Label();
            this.label_head = new System.Windows.Forms.Label();
            this.button_beacon_save = new System.Windows.Forms.Button();
            this.label_beacon_type = new System.Windows.Forms.Label();
            this.label_1_group_box2_ans = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label_5_group_box2_ans = new System.Windows.Forms.Label();
            this.label_5_group_box2 = new System.Windows.Forms.Label();
            this.label_1_group_box2 = new System.Windows.Forms.Label();
            this.label_4_group_box2_ans = new System.Windows.Forms.Label();
            this.label_4_group_box2 = new System.Windows.Forms.Label();
            this.label_3_group_box2_ans = new System.Windows.Forms.Label();
            this.label_3_group_box2 = new System.Windows.Forms.Label();
            this.label_2_group_box2_ans = new System.Windows.Forms.Label();
            this.label_2_group_box2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_5_group_box3_ans = new System.Windows.Forms.Label();
            this.label_5_group_box3 = new System.Windows.Forms.Label();
            this.label_1_group_box3_ans = new System.Windows.Forms.Label();
            this.label_1_group_box3 = new System.Windows.Forms.Label();
            this.label_4_group_box3_ans = new System.Windows.Forms.Label();
            this.label_4_group_box3 = new System.Windows.Forms.Label();
            this.label_3_group_box3_ans = new System.Windows.Forms.Label();
            this.label_3_group_box3 = new System.Windows.Forms.Label();
            this.label_2_group_box3_ans = new System.Windows.Forms.Label();
            this.label_2_group_box3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label_5_group_box4_ans = new System.Windows.Forms.Label();
            this.label_5_group_box4 = new System.Windows.Forms.Label();
            this.label_1_group_box4_ans = new System.Windows.Forms.Label();
            this.label_1_group_box4 = new System.Windows.Forms.Label();
            this.label_4_group_box4_ans = new System.Windows.Forms.Label();
            this.label_4_group_box4 = new System.Windows.Forms.Label();
            this.label_3_group_box4_ans = new System.Windows.Forms.Label();
            this.label_3_group_box4 = new System.Windows.Forms.Label();
            this.label_2_group_box4_ans = new System.Windows.Forms.Label();
            this.label_2_group_box4 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label_5_group_box5_ans = new System.Windows.Forms.Label();
            this.label_5_group_box5 = new System.Windows.Forms.Label();
            this.label_1_group_box5_ans = new System.Windows.Forms.Label();
            this.label_1_group_box5 = new System.Windows.Forms.Label();
            this.label_4_group_box5_ans = new System.Windows.Forms.Label();
            this.label_4_group_box5 = new System.Windows.Forms.Label();
            this.label_3_group_box5_ans = new System.Windows.Forms.Label();
            this.label_3_group_box5 = new System.Windows.Forms.Label();
            this.label_2_group_box5_ans = new System.Windows.Forms.Label();
            this.label_2_group_box5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label_1_group_box6_ans = new System.Windows.Forms.Label();
            this.label_1_group_box6 = new System.Windows.Forms.Label();
            this.label_4_group_box6_ans = new System.Windows.Forms.Label();
            this.label_4_group_box6 = new System.Windows.Forms.Label();
            this.label_2_group_box6_ans = new System.Windows.Forms.Label();
            this.label_2_group_box6 = new System.Windows.Forms.Label();
            this.groupBox6_stats = new System.Windows.Forms.GroupBox();
            this.label_5_group_box7_ans = new System.Windows.Forms.Label();
            this.label_4_group_box7_ans = new System.Windows.Forms.Label();
            this.label_3_group_box7_ans = new System.Windows.Forms.Label();
            this.label_2_group_box7_ans = new System.Windows.Forms.Label();
            this.label_1_group_box7_ans = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tv_time_date = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6_stats.SuspendLayout();
            this.SuspendLayout();
            // 
            // rTBox_beacon_raw_data
            // 
            this.rTBox_beacon_raw_data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rTBox_beacon_raw_data.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rTBox_beacon_raw_data.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rTBox_beacon_raw_data.Location = new System.Drawing.Point(18, 15);
            this.rTBox_beacon_raw_data.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rTBox_beacon_raw_data.Name = "rTBox_beacon_raw_data";
            this.rTBox_beacon_raw_data.Size = new System.Drawing.Size(1342, 158);
            this.rTBox_beacon_raw_data.TabIndex = 0;
            this.rTBox_beacon_raw_data.Text = "";
            this.rTBox_beacon_raw_data.TextChanged += new System.EventHandler(this.rTBox_beacon_raw_data_TextChanged);
            // 
            // button_beacon_paraser_ok
            // 
            this.button_beacon_paraser_ok.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_beacon_paraser_ok.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_beacon_paraser_ok.Location = new System.Drawing.Point(1380, 21);
            this.button_beacon_paraser_ok.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_beacon_paraser_ok.Name = "button_beacon_paraser_ok";
            this.button_beacon_paraser_ok.Size = new System.Drawing.Size(140, 73);
            this.button_beacon_paraser_ok.TabIndex = 1;
            this.button_beacon_paraser_ok.Text = "Get Beacon";
            this.button_beacon_paraser_ok.UseVisualStyleBackColor = true;
            this.button_beacon_paraser_ok.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_cancel_beacon
            // 
            this.button_cancel_beacon.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_cancel_beacon.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_cancel_beacon.Location = new System.Drawing.Point(1380, 102);
            this.button_cancel_beacon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_cancel_beacon.Name = "button_cancel_beacon";
            this.button_cancel_beacon.Size = new System.Drawing.Size(140, 73);
            this.button_cancel_beacon.TabIndex = 2;
            this.button_cancel_beacon.Text = "Cancel";
            this.button_cancel_beacon.UseVisualStyleBackColor = true;
            this.button_cancel_beacon.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label_call_sign_ans);
            this.groupBox1.Controls.Add(this.label_call_sign);
            this.groupBox1.Controls.Add(this.label_time_hour_ans);
            this.groupBox1.Controls.Add(this.label_time_hour);
            this.groupBox1.Controls.Add(this.label_time_day_ans);
            this.groupBox1.Controls.Add(this.label_time_day);
            this.groupBox1.Controls.Add(this.label_type_ans);
            this.groupBox1.Controls.Add(this.label_type);
            this.groupBox1.Controls.Add(this.label_head_ans);
            this.groupBox1.Controls.Add(this.label_head);
            this.groupBox1.Location = new System.Drawing.Point(28, 225);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(314, 254);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Profile";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(554, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 32);
            this.label1.TabIndex = 19;
            this.label1.Text = "callsign";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(406, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 38);
            this.label2.TabIndex = 18;
            this.label2.Text = "Call Sign";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(554, 185);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 32);
            this.label3.TabIndex = 17;
            this.label3.Text = "hour";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(406, 179);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 38);
            this.label4.TabIndex = 16;
            this.label4.Text = "Hour";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(554, 140);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 32);
            this.label5.TabIndex = 15;
            this.label5.Text = "day";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(406, 135);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 38);
            this.label6.TabIndex = 14;
            this.label6.Text = "Day";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Location = new System.Drawing.Point(554, 94);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 32);
            this.label7.TabIndex = 13;
            this.label7.Text = "type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(406, 88);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 38);
            this.label8.TabIndex = 12;
            this.label8.Text = "Type";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Location = new System.Drawing.Point(554, 52);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 32);
            this.label9.TabIndex = 11;
            this.label9.Text = "head";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(406, 46);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 38);
            this.label10.TabIndex = 10;
            this.label10.Text = "Head";
            // 
            // label_call_sign_ans
            // 
            this.label_call_sign_ans.AutoSize = true;
            this.label_call_sign_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_call_sign_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_call_sign_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_call_sign_ans.Location = new System.Drawing.Point(172, 31);
            this.label_call_sign_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_call_sign_ans.Name = "label_call_sign_ans";
            this.label_call_sign_ans.Size = new System.Drawing.Size(94, 32);
            this.label_call_sign_ans.TabIndex = 9;
            this.label_call_sign_ans.Text = "callsign";
            // 
            // label_call_sign
            // 
            this.label_call_sign.AutoSize = true;
            this.label_call_sign.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_call_sign.Location = new System.Drawing.Point(26, 29);
            this.label_call_sign.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_call_sign.Name = "label_call_sign";
            this.label_call_sign.Size = new System.Drawing.Size(131, 38);
            this.label_call_sign.TabIndex = 8;
            this.label_call_sign.Text = "Call Sign";
            // 
            // label_time_hour_ans
            // 
            this.label_time_hour_ans.AutoSize = true;
            this.label_time_hour_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_time_hour_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_time_hour_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_time_hour_ans.Location = new System.Drawing.Point(172, 204);
            this.label_time_hour_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_time_hour_ans.Name = "label_time_hour_ans";
            this.label_time_hour_ans.Size = new System.Drawing.Size(65, 32);
            this.label_time_hour_ans.TabIndex = 7;
            this.label_time_hour_ans.Text = "hour";
            // 
            // label_time_hour
            // 
            this.label_time_hour.AutoSize = true;
            this.label_time_hour.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_time_hour.Location = new System.Drawing.Point(26, 204);
            this.label_time_hour.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_time_hour.Name = "label_time_hour";
            this.label_time_hour.Size = new System.Drawing.Size(83, 38);
            this.label_time_hour.TabIndex = 6;
            this.label_time_hour.Text = "Hour";
            // 
            // label_time_day_ans
            // 
            this.label_time_day_ans.AutoSize = true;
            this.label_time_day_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_time_day_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_time_day_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_time_day_ans.Location = new System.Drawing.Point(172, 160);
            this.label_time_day_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_time_day_ans.Name = "label_time_day_ans";
            this.label_time_day_ans.Size = new System.Drawing.Size(53, 32);
            this.label_time_day_ans.TabIndex = 5;
            this.label_time_day_ans.Text = "day";
            // 
            // label_time_day
            // 
            this.label_time_day.AutoSize = true;
            this.label_time_day.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_time_day.Location = new System.Drawing.Point(26, 160);
            this.label_time_day.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_time_day.Name = "label_time_day";
            this.label_time_day.Size = new System.Drawing.Size(68, 38);
            this.label_time_day.TabIndex = 4;
            this.label_time_day.Text = "Day";
            // 
            // label_type_ans
            // 
            this.label_type_ans.AutoSize = true;
            this.label_type_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_type_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_type_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_type_ans.Location = new System.Drawing.Point(172, 119);
            this.label_type_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_type_ans.Name = "label_type_ans";
            this.label_type_ans.Size = new System.Drawing.Size(62, 32);
            this.label_type_ans.TabIndex = 3;
            this.label_type_ans.Text = "type";
            // 
            // label_type
            // 
            this.label_type.AutoSize = true;
            this.label_type.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_type.Location = new System.Drawing.Point(26, 117);
            this.label_type.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_type.Name = "label_type";
            this.label_type.Size = new System.Drawing.Size(79, 38);
            this.label_type.TabIndex = 2;
            this.label_type.Text = "Type";
            // 
            // label_head_ans
            // 
            this.label_head_ans.AutoSize = true;
            this.label_head_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_head_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_head_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_head_ans.Location = new System.Drawing.Point(172, 77);
            this.label_head_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_head_ans.Name = "label_head_ans";
            this.label_head_ans.Size = new System.Drawing.Size(68, 32);
            this.label_head_ans.TabIndex = 1;
            this.label_head_ans.Text = "head";
            // 
            // label_head
            // 
            this.label_head.AutoSize = true;
            this.label_head.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_head.Location = new System.Drawing.Point(26, 73);
            this.label_head.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_head.Name = "label_head";
            this.label_head.Size = new System.Drawing.Size(85, 38);
            this.label_head.TabIndex = 0;
            this.label_head.Text = "Head";
            // 
            // button_beacon_save
            // 
            this.button_beacon_save.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_beacon_save.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_beacon_save.Location = new System.Drawing.Point(1386, 496);
            this.button_beacon_save.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_beacon_save.Name = "button_beacon_save";
            this.button_beacon_save.Size = new System.Drawing.Size(140, 73);
            this.button_beacon_save.TabIndex = 6;
            this.button_beacon_save.Text = "Save";
            this.button_beacon_save.UseVisualStyleBackColor = true;
            this.button_beacon_save.Click += new System.EventHandler(this.button_beacon_save_Click);
            // 
            // label_beacon_type
            // 
            this.label_beacon_type.AutoSize = true;
            this.label_beacon_type.BackColor = System.Drawing.Color.Black;
            this.label_beacon_type.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_beacon_type.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_beacon_type.Location = new System.Drawing.Point(28, 179);
            this.label_beacon_type.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_beacon_type.Name = "label_beacon_type";
            this.label_beacon_type.Size = new System.Drawing.Size(259, 45);
            this.label_beacon_type.TabIndex = 7;
            this.label_beacon_type.Text = "BEACON Type 1";
            // 
            // label_1_group_box2_ans
            // 
            this.label_1_group_box2_ans.AutoSize = true;
            this.label_1_group_box2_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_1_group_box2_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_1_group_box2_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_1_group_box2_ans.Location = new System.Drawing.Point(361, 35);
            this.label_1_group_box2_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_1_group_box2_ans.Name = "label_1_group_box2_ans";
            this.label_1_group_box2_ans.Size = new System.Drawing.Size(72, 32);
            this.label_1_group_box2_ans.TabIndex = 9;
            this.label_1_group_box2_ans.Text = "9.999";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label_5_group_box2_ans);
            this.groupBox9.Controls.Add(this.label_5_group_box2);
            this.groupBox9.Controls.Add(this.label_1_group_box2_ans);
            this.groupBox9.Controls.Add(this.label_1_group_box2);
            this.groupBox9.Controls.Add(this.label_4_group_box2_ans);
            this.groupBox9.Controls.Add(this.label_4_group_box2);
            this.groupBox9.Controls.Add(this.label_3_group_box2_ans);
            this.groupBox9.Controls.Add(this.label_3_group_box2);
            this.groupBox9.Controls.Add(this.label_2_group_box2_ans);
            this.groupBox9.Controls.Add(this.label_2_group_box2);
            this.groupBox9.Location = new System.Drawing.Point(352, 225);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox9.Size = new System.Drawing.Size(463, 254);
            this.groupBox9.TabIndex = 20;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Battery Parameter";
            // 
            // label_5_group_box2_ans
            // 
            this.label_5_group_box2_ans.AutoSize = true;
            this.label_5_group_box2_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_5_group_box2_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_5_group_box2_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_5_group_box2_ans.Location = new System.Drawing.Point(361, 203);
            this.label_5_group_box2_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_5_group_box2_ans.Name = "label_5_group_box2_ans";
            this.label_5_group_box2_ans.Size = new System.Drawing.Size(66, 32);
            this.label_5_group_box2_ans.TabIndex = 11;
            this.label_5_group_box2_ans.Text = "solar";
            // 
            // label_5_group_box2
            // 
            this.label_5_group_box2.AutoSize = true;
            this.label_5_group_box2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_5_group_box2.Location = new System.Drawing.Point(26, 200);
            this.label_5_group_box2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_5_group_box2.Name = "label_5_group_box2";
            this.label_5_group_box2.Size = new System.Drawing.Size(267, 38);
            this.label_5_group_box2.TabIndex = 10;
            this.label_5_group_box2.Text = "Solar -Y Voltage(V)";
            // 
            // label_1_group_box2
            // 
            this.label_1_group_box2.AutoSize = true;
            this.label_1_group_box2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_1_group_box2.Location = new System.Drawing.Point(26, 29);
            this.label_1_group_box2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_1_group_box2.Name = "label_1_group_box2";
            this.label_1_group_box2.Size = new System.Drawing.Size(261, 38);
            this.label_1_group_box2.TabIndex = 8;
            this.label_1_group_box2.Text = "Battery Voltage(V)";
            // 
            // label_4_group_box2_ans
            // 
            this.label_4_group_box2_ans.AutoSize = true;
            this.label_4_group_box2_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_4_group_box2_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_4_group_box2_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_4_group_box2_ans.Location = new System.Drawing.Point(361, 163);
            this.label_4_group_box2_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_4_group_box2_ans.Name = "label_4_group_box2_ans";
            this.label_4_group_box2_ans.Size = new System.Drawing.Size(66, 32);
            this.label_4_group_box2_ans.TabIndex = 5;
            this.label_4_group_box2_ans.Text = "solar";
            // 
            // label_4_group_box2
            // 
            this.label_4_group_box2.AutoSize = true;
            this.label_4_group_box2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_4_group_box2.Location = new System.Drawing.Point(26, 160);
            this.label_4_group_box2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_4_group_box2.Name = "label_4_group_box2";
            this.label_4_group_box2.Size = new System.Drawing.Size(245, 38);
            this.label_4_group_box2.TabIndex = 4;
            this.label_4_group_box2.Text = "Raw Current(mA)";
            // 
            // label_3_group_box2_ans
            // 
            this.label_3_group_box2_ans.AutoSize = true;
            this.label_3_group_box2_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_3_group_box2_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_3_group_box2_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_3_group_box2_ans.Location = new System.Drawing.Point(361, 121);
            this.label_3_group_box2_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_3_group_box2_ans.Name = "label_3_group_box2_ans";
            this.label_3_group_box2_ans.Size = new System.Drawing.Size(59, 32);
            this.label_3_group_box2_ans.TabIndex = 3;
            this.label_3_group_box2_ans.Text = "batt";
            // 
            // label_3_group_box2
            // 
            this.label_3_group_box2.AutoSize = true;
            this.label_3_group_box2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_3_group_box2.Location = new System.Drawing.Point(26, 117);
            this.label_3_group_box2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_3_group_box2.Name = "label_3_group_box2";
            this.label_3_group_box2.Size = new System.Drawing.Size(336, 38);
            this.label_3_group_box2.TabIndex = 2;
            this.label_3_group_box2.Text = "Battery Temperature(°C)";
            // 
            // label_2_group_box2_ans
            // 
            this.label_2_group_box2_ans.AutoSize = true;
            this.label_2_group_box2_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_2_group_box2_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_2_group_box2_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_2_group_box2_ans.Location = new System.Drawing.Point(361, 77);
            this.label_2_group_box2_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_2_group_box2_ans.Name = "label_2_group_box2_ans";
            this.label_2_group_box2_ans.Size = new System.Drawing.Size(74, 32);
            this.label_2_group_box2_ans.TabIndex = 1;
            this.label_2_group_box2_ans.Text = "battC";
            // 
            // label_2_group_box2
            // 
            this.label_2_group_box2.AutoSize = true;
            this.label_2_group_box2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_2_group_box2.Location = new System.Drawing.Point(26, 73);
            this.label_2_group_box2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_2_group_box2.Name = "label_2_group_box2";
            this.label_2_group_box2.Size = new System.Drawing.Size(287, 38);
            this.label_2_group_box2.TabIndex = 0;
            this.label_2_group_box2.Text = "Battery Current(mA)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_5_group_box3_ans);
            this.groupBox2.Controls.Add(this.label_5_group_box3);
            this.groupBox2.Controls.Add(this.label_1_group_box3_ans);
            this.groupBox2.Controls.Add(this.label_1_group_box3);
            this.groupBox2.Controls.Add(this.label_4_group_box3_ans);
            this.groupBox2.Controls.Add(this.label_4_group_box3);
            this.groupBox2.Controls.Add(this.label_3_group_box3_ans);
            this.groupBox2.Controls.Add(this.label_3_group_box3);
            this.groupBox2.Controls.Add(this.label_2_group_box3_ans);
            this.groupBox2.Controls.Add(this.label_2_group_box3);
            this.groupBox2.Location = new System.Drawing.Point(825, 225);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(448, 254);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Temperature";
            // 
            // label_5_group_box3_ans
            // 
            this.label_5_group_box3_ans.AutoSize = true;
            this.label_5_group_box3_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_5_group_box3_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_5_group_box3_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_5_group_box3_ans.Location = new System.Drawing.Point(356, 203);
            this.label_5_group_box3_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_5_group_box3_ans.Name = "label_5_group_box3_ans";
            this.label_5_group_box3_ans.Size = new System.Drawing.Size(66, 32);
            this.label_5_group_box3_ans.TabIndex = 11;
            this.label_5_group_box3_ans.Text = "solar";
            // 
            // label_5_group_box3
            // 
            this.label_5_group_box3.AutoSize = true;
            this.label_5_group_box3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_5_group_box3.Location = new System.Drawing.Point(26, 200);
            this.label_5_group_box3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_5_group_box3.Name = "label_5_group_box3";
            this.label_5_group_box3.Size = new System.Drawing.Size(247, 38);
            this.label_5_group_box3.TabIndex = 10;
            this.label_5_group_box3.Text = "OBC Temperature";
            // 
            // label_1_group_box3_ans
            // 
            this.label_1_group_box3_ans.AutoSize = true;
            this.label_1_group_box3_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_1_group_box3_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_1_group_box3_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_1_group_box3_ans.Location = new System.Drawing.Point(356, 35);
            this.label_1_group_box3_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_1_group_box3_ans.Name = "label_1_group_box3_ans";
            this.label_1_group_box3_ans.Size = new System.Drawing.Size(72, 32);
            this.label_1_group_box3_ans.TabIndex = 9;
            this.label_1_group_box3_ans.Text = "9.999";
            // 
            // label_1_group_box3
            // 
            this.label_1_group_box3.AutoSize = true;
            this.label_1_group_box3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_1_group_box3.Location = new System.Drawing.Point(26, 29);
            this.label_1_group_box3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_1_group_box3.Name = "label_1_group_box3";
            this.label_1_group_box3.Size = new System.Drawing.Size(264, 38);
            this.label_1_group_box3.TabIndex = 8;
            this.label_1_group_box3.Text = "Solar Total Voltage";
            // 
            // label_4_group_box3_ans
            // 
            this.label_4_group_box3_ans.AutoSize = true;
            this.label_4_group_box3_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_4_group_box3_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_4_group_box3_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_4_group_box3_ans.Location = new System.Drawing.Point(356, 162);
            this.label_4_group_box3_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_4_group_box3_ans.Name = "label_4_group_box3_ans";
            this.label_4_group_box3_ans.Size = new System.Drawing.Size(66, 32);
            this.label_4_group_box3_ans.TabIndex = 5;
            this.label_4_group_box3_ans.Text = "solar";
            // 
            // label_4_group_box3
            // 
            this.label_4_group_box3.AutoSize = true;
            this.label_4_group_box3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_4_group_box3.Location = new System.Drawing.Point(26, 160);
            this.label_4_group_box3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_4_group_box3.Name = "label_4_group_box3";
            this.label_4_group_box3.Size = new System.Drawing.Size(237, 38);
            this.label_4_group_box3.TabIndex = 4;
            this.label_4_group_box3.Text = "Back Panel Temp";
            // 
            // label_3_group_box3_ans
            // 
            this.label_3_group_box3_ans.AutoSize = true;
            this.label_3_group_box3_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_3_group_box3_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_3_group_box3_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_3_group_box3_ans.Location = new System.Drawing.Point(356, 119);
            this.label_3_group_box3_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_3_group_box3_ans.Name = "label_3_group_box3_ans";
            this.label_3_group_box3_ans.Size = new System.Drawing.Size(62, 32);
            this.label_3_group_box3_ans.TabIndex = 3;
            this.label_3_group_box3_ans.Text = "type";
            // 
            // label_3_group_box3
            // 
            this.label_3_group_box3.AutoSize = true;
            this.label_3_group_box3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_3_group_box3.Location = new System.Drawing.Point(26, 117);
            this.label_3_group_box3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_3_group_box3.Name = "label_3_group_box3";
            this.label_3_group_box3.Size = new System.Drawing.Size(335, 38);
            this.label_3_group_box3.TabIndex = 2;
            this.label_3_group_box3.Text = "Antenna Panel Temp(°C)";
            // 
            // label_2_group_box3_ans
            // 
            this.label_2_group_box3_ans.AutoSize = true;
            this.label_2_group_box3_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_2_group_box3_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_2_group_box3_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_2_group_box3_ans.Location = new System.Drawing.Point(356, 77);
            this.label_2_group_box3_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_2_group_box3_ans.Name = "label_2_group_box3_ans";
            this.label_2_group_box3_ans.Size = new System.Drawing.Size(74, 32);
            this.label_2_group_box3_ans.TabIndex = 1;
            this.label_2_group_box3_ans.Text = "battC";
            // 
            // label_2_group_box3
            // 
            this.label_2_group_box3.AutoSize = true;
            this.label_2_group_box3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_2_group_box3.Location = new System.Drawing.Point(26, 73);
            this.label_2_group_box3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_2_group_box3.Name = "label_2_group_box3";
            this.label_2_group_box3.Size = new System.Drawing.Size(263, 38);
            this.label_2_group_box3.TabIndex = 0;
            this.label_2_group_box3.Text = "Solar Total Current";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label_5_group_box4_ans);
            this.groupBox3.Controls.Add(this.label_5_group_box4);
            this.groupBox3.Controls.Add(this.label_1_group_box4_ans);
            this.groupBox3.Controls.Add(this.label_1_group_box4);
            this.groupBox3.Controls.Add(this.label_4_group_box4_ans);
            this.groupBox3.Controls.Add(this.label_4_group_box4);
            this.groupBox3.Controls.Add(this.label_3_group_box4_ans);
            this.groupBox3.Controls.Add(this.label_3_group_box4);
            this.groupBox3.Controls.Add(this.label_2_group_box4_ans);
            this.groupBox3.Controls.Add(this.label_2_group_box4);
            this.groupBox3.Location = new System.Drawing.Point(28, 485);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(440, 254);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Solar Pannel Temperature(°C)";
            // 
            // label_5_group_box4_ans
            // 
            this.label_5_group_box4_ans.AutoSize = true;
            this.label_5_group_box4_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_5_group_box4_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_5_group_box4_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_5_group_box4_ans.Location = new System.Drawing.Point(318, 205);
            this.label_5_group_box4_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_5_group_box4_ans.Name = "label_5_group_box4_ans";
            this.label_5_group_box4_ans.Size = new System.Drawing.Size(66, 32);
            this.label_5_group_box4_ans.TabIndex = 11;
            this.label_5_group_box4_ans.Text = "solar";
            // 
            // label_5_group_box4
            // 
            this.label_5_group_box4.AutoSize = true;
            this.label_5_group_box4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_5_group_box4.Location = new System.Drawing.Point(26, 200);
            this.label_5_group_box4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_5_group_box4.Name = "label_5_group_box4";
            this.label_5_group_box4.Size = new System.Drawing.Size(278, 38);
            this.label_5_group_box4.TabIndex = 10;
            this.label_5_group_box4.Text = "Solar Panel -Y Temp";
            // 
            // label_1_group_box4_ans
            // 
            this.label_1_group_box4_ans.AutoSize = true;
            this.label_1_group_box4_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_1_group_box4_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_1_group_box4_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_1_group_box4_ans.Location = new System.Drawing.Point(318, 34);
            this.label_1_group_box4_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_1_group_box4_ans.Name = "label_1_group_box4_ans";
            this.label_1_group_box4_ans.Size = new System.Drawing.Size(97, 32);
            this.label_1_group_box4_ans.TabIndex = 9;
            this.label_1_group_box4_ans.Text = "battVtg";
            // 
            // label_1_group_box4
            // 
            this.label_1_group_box4.AutoSize = true;
            this.label_1_group_box4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_1_group_box4.Location = new System.Drawing.Point(26, 29);
            this.label_1_group_box4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_1_group_box4.Name = "label_1_group_box4";
            this.label_1_group_box4.Size = new System.Drawing.Size(287, 38);
            this.label_1_group_box4.TabIndex = 8;
            this.label_1_group_box4.Text = "Solar Panel +Z Temp";
            // 
            // label_4_group_box4_ans
            // 
            this.label_4_group_box4_ans.AutoSize = true;
            this.label_4_group_box4_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_4_group_box4_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_4_group_box4_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_4_group_box4_ans.Location = new System.Drawing.Point(318, 163);
            this.label_4_group_box4_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_4_group_box4_ans.Name = "label_4_group_box4_ans";
            this.label_4_group_box4_ans.Size = new System.Drawing.Size(66, 32);
            this.label_4_group_box4_ans.TabIndex = 5;
            this.label_4_group_box4_ans.Text = "solar";
            // 
            // label_4_group_box4
            // 
            this.label_4_group_box4.AutoSize = true;
            this.label_4_group_box4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_4_group_box4.Location = new System.Drawing.Point(26, 160);
            this.label_4_group_box4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_4_group_box4.Name = "label_4_group_box4";
            this.label_4_group_box4.Size = new System.Drawing.Size(279, 38);
            this.label_4_group_box4.TabIndex = 4;
            this.label_4_group_box4.Text = "Solar Panel -X Temp";
            // 
            // label_3_group_box4_ans
            // 
            this.label_3_group_box4_ans.AutoSize = true;
            this.label_3_group_box4_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_3_group_box4_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_3_group_box4_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_3_group_box4_ans.Location = new System.Drawing.Point(318, 121);
            this.label_3_group_box4_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_3_group_box4_ans.Name = "label_3_group_box4_ans";
            this.label_3_group_box4_ans.Size = new System.Drawing.Size(62, 32);
            this.label_3_group_box4_ans.TabIndex = 3;
            this.label_3_group_box4_ans.Text = "type";
            // 
            // label_3_group_box4
            // 
            this.label_3_group_box4.AutoSize = true;
            this.label_3_group_box4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_3_group_box4.Location = new System.Drawing.Point(26, 117);
            this.label_3_group_box4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_3_group_box4.Name = "label_3_group_box4";
            this.label_3_group_box4.Size = new System.Drawing.Size(278, 38);
            this.label_3_group_box4.TabIndex = 2;
            this.label_3_group_box4.Text = "Solar Panel -Z Temp";
            // 
            // label_2_group_box4_ans
            // 
            this.label_2_group_box4_ans.AutoSize = true;
            this.label_2_group_box4_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_2_group_box4_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_2_group_box4_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_2_group_box4_ans.Location = new System.Drawing.Point(318, 79);
            this.label_2_group_box4_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_2_group_box4_ans.Name = "label_2_group_box4_ans";
            this.label_2_group_box4_ans.Size = new System.Drawing.Size(74, 32);
            this.label_2_group_box4_ans.TabIndex = 1;
            this.label_2_group_box4_ans.Text = "battC";
            // 
            // label_2_group_box4
            // 
            this.label_2_group_box4.AutoSize = true;
            this.label_2_group_box4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_2_group_box4.Location = new System.Drawing.Point(26, 73);
            this.label_2_group_box4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_2_group_box4.Name = "label_2_group_box4";
            this.label_2_group_box4.Size = new System.Drawing.Size(287, 38);
            this.label_2_group_box4.TabIndex = 0;
            this.label_2_group_box4.Text = "Solar Panel +Y Temp";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label_5_group_box5_ans);
            this.groupBox5.Controls.Add(this.label_5_group_box5);
            this.groupBox5.Controls.Add(this.label_1_group_box5_ans);
            this.groupBox5.Controls.Add(this.label_1_group_box5);
            this.groupBox5.Controls.Add(this.label_4_group_box5_ans);
            this.groupBox5.Controls.Add(this.label_4_group_box5);
            this.groupBox5.Controls.Add(this.label_3_group_box5_ans);
            this.groupBox5.Controls.Add(this.label_3_group_box5);
            this.groupBox5.Controls.Add(this.label_2_group_box5_ans);
            this.groupBox5.Controls.Add(this.label_2_group_box5);
            this.groupBox5.Location = new System.Drawing.Point(476, 485);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Size = new System.Drawing.Size(436, 254);
            this.groupBox5.TabIndex = 23;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Solar Panel Status";
            // 
            // label_5_group_box5_ans
            // 
            this.label_5_group_box5_ans.AutoSize = true;
            this.label_5_group_box5_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_5_group_box5_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_5_group_box5_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_5_group_box5_ans.Location = new System.Drawing.Point(332, 204);
            this.label_5_group_box5_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_5_group_box5_ans.Name = "label_5_group_box5_ans";
            this.label_5_group_box5_ans.Size = new System.Drawing.Size(66, 32);
            this.label_5_group_box5_ans.TabIndex = 11;
            this.label_5_group_box5_ans.Text = "solar";
            // 
            // label_5_group_box5
            // 
            this.label_5_group_box5.AutoSize = true;
            this.label_5_group_box5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_5_group_box5.Location = new System.Drawing.Point(26, 200);
            this.label_5_group_box5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_5_group_box5.Name = "label_5_group_box5";
            this.label_5_group_box5.Size = new System.Drawing.Size(296, 38);
            this.label_5_group_box5.TabIndex = 10;
            this.label_5_group_box5.Text = "Solar Panel -Y  Status";
            // 
            // label_1_group_box5_ans
            // 
            this.label_1_group_box5_ans.AutoSize = true;
            this.label_1_group_box5_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_1_group_box5_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_1_group_box5_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_1_group_box5_ans.Location = new System.Drawing.Point(332, 34);
            this.label_1_group_box5_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_1_group_box5_ans.Name = "label_1_group_box5_ans";
            this.label_1_group_box5_ans.Size = new System.Drawing.Size(97, 32);
            this.label_1_group_box5_ans.TabIndex = 9;
            this.label_1_group_box5_ans.Text = "battVtg";
            // 
            // label_1_group_box5
            // 
            this.label_1_group_box5.AutoSize = true;
            this.label_1_group_box5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_1_group_box5.Location = new System.Drawing.Point(26, 29);
            this.label_1_group_box5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_1_group_box5.Name = "label_1_group_box5";
            this.label_1_group_box5.Size = new System.Drawing.Size(297, 38);
            this.label_1_group_box5.TabIndex = 8;
            this.label_1_group_box5.Text = "Solar Panel +Z Status";
            // 
            // label_4_group_box5_ans
            // 
            this.label_4_group_box5_ans.AutoSize = true;
            this.label_4_group_box5_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_4_group_box5_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_4_group_box5_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_4_group_box5_ans.Location = new System.Drawing.Point(332, 162);
            this.label_4_group_box5_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_4_group_box5_ans.Name = "label_4_group_box5_ans";
            this.label_4_group_box5_ans.Size = new System.Drawing.Size(66, 32);
            this.label_4_group_box5_ans.TabIndex = 5;
            this.label_4_group_box5_ans.Text = "solar";
            // 
            // label_4_group_box5
            // 
            this.label_4_group_box5.AutoSize = true;
            this.label_4_group_box5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_4_group_box5.Location = new System.Drawing.Point(26, 160);
            this.label_4_group_box5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_4_group_box5.Name = "label_4_group_box5";
            this.label_4_group_box5.Size = new System.Drawing.Size(297, 38);
            this.label_4_group_box5.TabIndex = 4;
            this.label_4_group_box5.Text = "Solar Panel -X  Status";
            // 
            // label_3_group_box5_ans
            // 
            this.label_3_group_box5_ans.AutoSize = true;
            this.label_3_group_box5_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_3_group_box5_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_3_group_box5_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_3_group_box5_ans.Location = new System.Drawing.Point(332, 120);
            this.label_3_group_box5_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_3_group_box5_ans.Name = "label_3_group_box5_ans";
            this.label_3_group_box5_ans.Size = new System.Drawing.Size(62, 32);
            this.label_3_group_box5_ans.TabIndex = 3;
            this.label_3_group_box5_ans.Text = "type";
            // 
            // label_3_group_box5
            // 
            this.label_3_group_box5.AutoSize = true;
            this.label_3_group_box5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_3_group_box5.Location = new System.Drawing.Point(26, 117);
            this.label_3_group_box5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_3_group_box5.Name = "label_3_group_box5";
            this.label_3_group_box5.Size = new System.Drawing.Size(296, 38);
            this.label_3_group_box5.TabIndex = 2;
            this.label_3_group_box5.Text = "Solar Panel -Z  Status";
            // 
            // label_2_group_box5_ans
            // 
            this.label_2_group_box5_ans.AutoSize = true;
            this.label_2_group_box5_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_2_group_box5_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_2_group_box5_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_2_group_box5_ans.Location = new System.Drawing.Point(332, 78);
            this.label_2_group_box5_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_2_group_box5_ans.Name = "label_2_group_box5_ans";
            this.label_2_group_box5_ans.Size = new System.Drawing.Size(74, 32);
            this.label_2_group_box5_ans.TabIndex = 1;
            this.label_2_group_box5_ans.Text = "battC";
            // 
            // label_2_group_box5
            // 
            this.label_2_group_box5.AutoSize = true;
            this.label_2_group_box5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_2_group_box5.Location = new System.Drawing.Point(26, 73);
            this.label_2_group_box5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_2_group_box5.Name = "label_2_group_box5";
            this.label_2_group_box5.Size = new System.Drawing.Size(305, 38);
            this.label_2_group_box5.TabIndex = 0;
            this.label_2_group_box5.Text = "Solar Panel +Y  Status";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label_1_group_box6_ans);
            this.groupBox4.Controls.Add(this.label_1_group_box6);
            this.groupBox4.Controls.Add(this.label_4_group_box6_ans);
            this.groupBox4.Controls.Add(this.label_4_group_box6);
            this.groupBox4.Controls.Add(this.label_2_group_box6_ans);
            this.groupBox4.Controls.Add(this.label_2_group_box6);
            this.groupBox4.Location = new System.Drawing.Point(920, 485);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(458, 254);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "On-Board-Computer";
            // 
            // label_1_group_box6_ans
            // 
            this.label_1_group_box6_ans.AutoSize = true;
            this.label_1_group_box6_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_1_group_box6_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_1_group_box6_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_1_group_box6_ans.Location = new System.Drawing.Point(304, 35);
            this.label_1_group_box6_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_1_group_box6_ans.Name = "label_1_group_box6_ans";
            this.label_1_group_box6_ans.Size = new System.Drawing.Size(97, 32);
            this.label_1_group_box6_ans.TabIndex = 9;
            this.label_1_group_box6_ans.Text = "battVtg";
            // 
            // label_1_group_box6
            // 
            this.label_1_group_box6.AutoSize = true;
            this.label_1_group_box6.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_1_group_box6.Location = new System.Drawing.Point(26, 29);
            this.label_1_group_box6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_1_group_box6.Name = "label_1_group_box6";
            this.label_1_group_box6.Size = new System.Drawing.Size(233, 38);
            this.label_1_group_box6.TabIndex = 8;
            this.label_1_group_box6.Text = "Operation Mode";
            // 
            // label_4_group_box6_ans
            // 
            this.label_4_group_box6_ans.AutoSize = true;
            this.label_4_group_box6_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_4_group_box6_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_4_group_box6_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_4_group_box6_ans.Location = new System.Drawing.Point(304, 125);
            this.label_4_group_box6_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_4_group_box6_ans.Name = "label_4_group_box6_ans";
            this.label_4_group_box6_ans.Size = new System.Drawing.Size(66, 32);
            this.label_4_group_box6_ans.TabIndex = 5;
            this.label_4_group_box6_ans.Text = "solar";
            // 
            // label_4_group_box6
            // 
            this.label_4_group_box6.AutoSize = true;
            this.label_4_group_box6.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_4_group_box6.Location = new System.Drawing.Point(28, 119);
            this.label_4_group_box6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_4_group_box6.Name = "label_4_group_box6";
            this.label_4_group_box6.Size = new System.Drawing.Size(132, 38);
            this.label_4_group_box6.TabIndex = 4;
            this.label_4_group_box6.Text = "Last Rest";
            // 
            // label_2_group_box6_ans
            // 
            this.label_2_group_box6_ans.AutoSize = true;
            this.label_2_group_box6_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_2_group_box6_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_2_group_box6_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_2_group_box6_ans.Location = new System.Drawing.Point(304, 78);
            this.label_2_group_box6_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_2_group_box6_ans.Name = "label_2_group_box6_ans";
            this.label_2_group_box6_ans.Size = new System.Drawing.Size(74, 32);
            this.label_2_group_box6_ans.TabIndex = 1;
            this.label_2_group_box6_ans.Text = "battC";
            // 
            // label_2_group_box6
            // 
            this.label_2_group_box6.AutoSize = true;
            this.label_2_group_box6.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_2_group_box6.Location = new System.Drawing.Point(26, 73);
            this.label_2_group_box6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_2_group_box6.Name = "label_2_group_box6";
            this.label_2_group_box6.Size = new System.Drawing.Size(151, 38);
            this.label_2_group_box6.TabIndex = 0;
            this.label_2_group_box6.Text = "OBC Reset";
            // 
            // groupBox6_stats
            // 
            this.groupBox6_stats.Controls.Add(this.label_5_group_box7_ans);
            this.groupBox6_stats.Controls.Add(this.label_4_group_box7_ans);
            this.groupBox6_stats.Controls.Add(this.label_3_group_box7_ans);
            this.groupBox6_stats.Controls.Add(this.label_2_group_box7_ans);
            this.groupBox6_stats.Controls.Add(this.label_1_group_box7_ans);
            this.groupBox6_stats.Controls.Add(this.label15);
            this.groupBox6_stats.Controls.Add(this.label14);
            this.groupBox6_stats.Controls.Add(this.label13);
            this.groupBox6_stats.Controls.Add(this.label12);
            this.groupBox6_stats.Controls.Add(this.label11);
            this.groupBox6_stats.Location = new System.Drawing.Point(1281, 225);
            this.groupBox6_stats.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6_stats.Name = "groupBox6_stats";
            this.groupBox6_stats.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6_stats.Size = new System.Drawing.Size(248, 254);
            this.groupBox6_stats.TabIndex = 22;
            this.groupBox6_stats.TabStop = false;
            this.groupBox6_stats.Text = "Data";
            // 
            // label_5_group_box7_ans
            // 
            this.label_5_group_box7_ans.AutoSize = true;
            this.label_5_group_box7_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_5_group_box7_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_5_group_box7_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_5_group_box7_ans.Location = new System.Drawing.Point(204, 199);
            this.label_5_group_box7_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_5_group_box7_ans.Name = "label_5_group_box7_ans";
            this.label_5_group_box7_ans.Size = new System.Drawing.Size(24, 32);
            this.label_5_group_box7_ans.TabIndex = 28;
            this.label_5_group_box7_ans.Text = "1";
            // 
            // label_4_group_box7_ans
            // 
            this.label_4_group_box7_ans.AutoSize = true;
            this.label_4_group_box7_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_4_group_box7_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_4_group_box7_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_4_group_box7_ans.Location = new System.Drawing.Point(204, 162);
            this.label_4_group_box7_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_4_group_box7_ans.Name = "label_4_group_box7_ans";
            this.label_4_group_box7_ans.Size = new System.Drawing.Size(24, 32);
            this.label_4_group_box7_ans.TabIndex = 27;
            this.label_4_group_box7_ans.Text = "1";
            // 
            // label_3_group_box7_ans
            // 
            this.label_3_group_box7_ans.AutoSize = true;
            this.label_3_group_box7_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_3_group_box7_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_3_group_box7_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_3_group_box7_ans.Location = new System.Drawing.Point(204, 118);
            this.label_3_group_box7_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_3_group_box7_ans.Name = "label_3_group_box7_ans";
            this.label_3_group_box7_ans.Size = new System.Drawing.Size(24, 32);
            this.label_3_group_box7_ans.TabIndex = 26;
            this.label_3_group_box7_ans.Text = "1";
            // 
            // label_2_group_box7_ans
            // 
            this.label_2_group_box7_ans.AutoSize = true;
            this.label_2_group_box7_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_2_group_box7_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_2_group_box7_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_2_group_box7_ans.Location = new System.Drawing.Point(204, 81);
            this.label_2_group_box7_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_2_group_box7_ans.Name = "label_2_group_box7_ans";
            this.label_2_group_box7_ans.Size = new System.Drawing.Size(24, 32);
            this.label_2_group_box7_ans.TabIndex = 25;
            this.label_2_group_box7_ans.Text = "1";
            // 
            // label_1_group_box7_ans
            // 
            this.label_1_group_box7_ans.AutoSize = true;
            this.label_1_group_box7_ans.BackColor = System.Drawing.Color.Transparent;
            this.label_1_group_box7_ans.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_1_group_box7_ans.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_1_group_box7_ans.Location = new System.Drawing.Point(204, 37);
            this.label_1_group_box7_ans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_1_group_box7_ans.Name = "label_1_group_box7_ans";
            this.label_1_group_box7_ans.Size = new System.Drawing.Size(24, 32);
            this.label_1_group_box7_ans.TabIndex = 12;
            this.label_1_group_box7_ans.Text = "1";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(22, 196);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(111, 38);
            this.label15.TabIndex = 24;
            this.label15.Text = "UL Stat";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(22, 156);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(125, 38);
            this.label14.TabIndex = 23;
            this.label14.Text = "Ant Stat";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(22, 115);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(170, 38);
            this.label13.TabIndex = 22;
            this.label13.Text = "MSN_3 Stat";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(22, 71);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(170, 38);
            this.label12.TabIndex = 21;
            this.label12.Text = "MSN_2 Stat";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(22, 33);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(170, 38);
            this.label11.TabIndex = 20;
            this.label11.Text = "MSN_1 Stat";
            // 
            // tv_time_date
            // 
            this.tv_time_date.AutoSize = true;
            this.tv_time_date.BackColor = System.Drawing.Color.Black;
            this.tv_time_date.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tv_time_date.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.tv_time_date.Location = new System.Drawing.Point(468, 179);
            this.tv_time_date.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tv_time_date.Name = "tv_time_date";
            this.tv_time_date.Size = new System.Drawing.Size(380, 45);
            this.tv_time_date.TabIndex = 25;
            this.tv_time_date.Text = "yyyy-mm-dd, HH:mm:ss";
            // 
            // inputDialogBeacon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1540, 765);
            this.ControlBox = false;
            this.Controls.Add(this.tv_time_date);
            this.Controls.Add(this.groupBox6_stats);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.label_beacon_type);
            this.Controls.Add(this.button_beacon_save);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_cancel_beacon);
            this.Controls.Add(this.button_beacon_paraser_ok);
            this.Controls.Add(this.rTBox_beacon_raw_data);
            try { this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon"))); } catch { }
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "inputDialogBeacon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input Beacon Raw Data";
            this.Load += new System.EventHandler(this.inputDialogBeacon_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6_stats.ResumeLayout(false);
            this.groupBox6_stats.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RichTextBox rTBox_beacon_raw_data;
        private Button button_beacon_paraser_ok;
        private Button button_cancel_beacon;
        private GroupBox groupBox1;
        private Button button_beacon_save;
        private Label label_beacon_type;
        private Label label_head_ans;
        private Label label_head;
        private Label label_type_ans;
        private Label label_type;
        private Label label_time_hour_ans;
        private Label label_time_hour;
        private Label label_time_day_ans;
        private Label label_time_day;
        private Label label_call_sign_ans;
        private Label label_call_sign;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label_1_group_box2_ans;
        private GroupBox groupBox9;
        private Label label_1_group_box2;
        private Label label_4_group_box2_ans;
        private Label label_4_group_box2;
        private Label label_3_group_box2_ans;
        private Label label_3_group_box2;
        private Label label_2_group_box2_ans;
        private Label label_2_group_box2;
        private Label label_5_group_box2_ans;
        private Label label_5_group_box2;
        private GroupBox groupBox2;
        private Label label_5_group_box3_ans;
        private Label label_5_group_box3;
        private Label label_1_group_box3_ans;
        private Label label_1_group_box3;
        private Label label_4_group_box3_ans;
        private Label label_4_group_box3;
        private Label label_3_group_box3_ans;
        private Label label_3_group_box3;
        private Label label_2_group_box3_ans;
        private Label label_2_group_box3;
        private GroupBox groupBox3;
        private Label label_5_group_box4_ans;
        private Label label_5_group_box4;
        private Label label_1_group_box4_ans;
        private Label label_1_group_box4;
        private Label label_4_group_box4_ans;
        private Label label_4_group_box4;
        private Label label_3_group_box4_ans;
        private Label label_3_group_box4;
        private Label label_2_group_box4_ans;
        private Label label_2_group_box4;
        private GroupBox groupBox5;
        private Label label_5_group_box5_ans;
        private Label label_5_group_box5;
        private Label label_1_group_box5_ans;
        private Label label_1_group_box5;
        private Label label_4_group_box5_ans;
        private Label label_4_group_box5;
        private Label label_3_group_box5_ans;
        private Label label_3_group_box5;
        private Label label_2_group_box5_ans;
        private Label label_2_group_box5;
        private GroupBox groupBox4;
        private Label label_1_group_box6_ans;
        private Label label_1_group_box6;
        private Label label_4_group_box6_ans;
        private Label label_4_group_box6;
        private Label label_2_group_box6_ans;
        private Label label_2_group_box6;
        private GroupBox groupBox6_stats;
        private Label label13;
        private Label label12;
        private Label label11;
        private Label label15;
        private Label label14;
        private Label label_5_group_box7_ans;
        private Label label_4_group_box7_ans;
        private Label label_3_group_box7_ans;
        private Label label_2_group_box7_ans;
        private Label label_1_group_box7_ans;
        private Label tv_time_date;
    }
}