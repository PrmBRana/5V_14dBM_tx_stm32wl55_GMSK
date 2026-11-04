using System.Windows.Forms;

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

namespace OpenBirdsOperation
{
    partial class inputDialogCamera : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(inputDialogCamera));
            this.rTBox_cam_raw_data = new System.Windows.Forms.RichTextBox();
            this.button_cam_paraser_ok = new System.Windows.Forms.Button();
            this.button_cancel_cam = new System.Windows.Forms.Button();
            this.button_save_pic = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox_img = new System.Windows.Forms.PictureBox();
            this.btn_verify_image_data = new System.Windows.Forms.Button();
            this.btn_get_image_data = new System.Windows.Forms.Button();
            this.btn_decode_log = new System.Windows.Forms.Button();
            this.btn_get_log = new System.Windows.Forms.Button();
            this.btn_save_actual_log = new System.Windows.Forms.Button();
            this.btn_tx_dummy_cam = new System.Windows.Forms.Button();
            this.btn_tx_real_cam = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_img)).BeginInit();
            this.SuspendLayout();
            // 
            // rTBox_cam_raw_data
            // 
            this.rTBox_cam_raw_data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rTBox_cam_raw_data.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rTBox_cam_raw_data.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rTBox_cam_raw_data.Location = new System.Drawing.Point(12, 12);
            this.rTBox_cam_raw_data.Name = "rTBox_cam_raw_data";
            this.rTBox_cam_raw_data.Size = new System.Drawing.Size(602, 175);
            this.rTBox_cam_raw_data.TabIndex = 0;
            this.rTBox_cam_raw_data.Text = "";
            this.rTBox_cam_raw_data.TextChanged += new System.EventHandler(this.rTBox_cam_raw_data_TextChanged);
            // 
            // button_cam_paraser_ok
            // 
            this.button_cam_paraser_ok.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_cam_paraser_ok.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_cam_paraser_ok.Location = new System.Drawing.Point(630, 336);
            this.button_cam_paraser_ok.Name = "button_cam_paraser_ok";
            this.button_cam_paraser_ok.Size = new System.Drawing.Size(90, 59);
            this.button_cam_paraser_ok.TabIndex = 1;
            this.button_cam_paraser_ok.Text = "Get Image";
            this.button_cam_paraser_ok.UseVisualStyleBackColor = true;
            this.button_cam_paraser_ok.Click += new System.EventHandler(this.button_cam_paraser_ok_Click);
            // 
            // button_cancel_cam
            // 
            this.button_cancel_cam.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_cancel_cam.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_cancel_cam.Location = new System.Drawing.Point(630, 410);
            this.button_cancel_cam.Name = "button_cancel_cam";
            this.button_cancel_cam.Size = new System.Drawing.Size(90, 51);
            this.button_cancel_cam.TabIndex = 2;
            this.button_cancel_cam.Text = "Cancel";
            this.button_cancel_cam.UseVisualStyleBackColor = true;
            this.button_cancel_cam.Click += new System.EventHandler(this.button_cancel_cam_Click);
            // 
            // button_save_pic
            // 
            this.button_save_pic.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_save_pic.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_save_pic.Location = new System.Drawing.Point(626, 475);
            this.button_save_pic.Name = "button_save_pic";
            this.button_save_pic.Size = new System.Drawing.Size(94, 55);
            this.button_save_pic.TabIndex = 4;
            this.button_save_pic.Text = "Save";
            this.button_save_pic.UseVisualStyleBackColor = true;
            this.button_save_pic.Click += new System.EventHandler(this.button_save_pic_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox_img);
            this.groupBox1.Location = new System.Drawing.Point(10, 205);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(607, 377);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image Parser";
            // 
            // pictureBox_img
            // 
            this.pictureBox_img.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox_img.ErrorImage")));
            this.pictureBox_img.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_img.Image")));
            this.pictureBox_img.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox_img.InitialImage")));
            this.pictureBox_img.Location = new System.Drawing.Point(7, 19);
            this.pictureBox_img.Name = "pictureBox_img";
            this.pictureBox_img.Size = new System.Drawing.Size(594, 343);
            this.pictureBox_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_img.TabIndex = 6;
            this.pictureBox_img.TabStop = false;
            // 
            // btn_verify_image_data
            // 
            this.btn_verify_image_data.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_verify_image_data.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_verify_image_data.Location = new System.Drawing.Point(630, 215);
            this.btn_verify_image_data.Name = "btn_verify_image_data";
            this.btn_verify_image_data.Size = new System.Drawing.Size(90, 59);
            this.btn_verify_image_data.TabIndex = 6;
            this.btn_verify_image_data.Text = "Verify Image Data";
            this.btn_verify_image_data.UseVisualStyleBackColor = true;
            this.btn_verify_image_data.Click += new System.EventHandler(this.btn_verify_image_data_Click);
            // 
            // btn_get_image_data
            // 
            this.btn_get_image_data.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_get_image_data.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_get_image_data.Location = new System.Drawing.Point(630, 276);
            this.btn_get_image_data.Name = "btn_get_image_data";
            this.btn_get_image_data.Size = new System.Drawing.Size(90, 59);
            this.btn_get_image_data.TabIndex = 7;
            this.btn_get_image_data.Text = "Open Image Data";
            this.btn_get_image_data.UseVisualStyleBackColor = true;
            this.btn_get_image_data.Click += new System.EventHandler(this.btn_get_image_data_Click);
            // 
            // btn_decode_log
            // 
            this.btn_decode_log.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_decode_log.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_decode_log.Location = new System.Drawing.Point(630, 12);
            this.btn_decode_log.Name = "btn_decode_log";
            this.btn_decode_log.Size = new System.Drawing.Size(90, 59);
            this.btn_decode_log.TabIndex = 8;
            this.btn_decode_log.Text = "Verify RGB LOG";
            this.btn_decode_log.UseVisualStyleBackColor = true;
            this.btn_decode_log.Click += new System.EventHandler(this.btn_decode_log_Click);
            // 
            // btn_get_log
            // 
            this.btn_get_log.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_get_log.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_get_log.Location = new System.Drawing.Point(630, 72);
            this.btn_get_log.Name = "btn_get_log";
            this.btn_get_log.Size = new System.Drawing.Size(90, 59);
            this.btn_get_log.TabIndex = 9;
            this.btn_get_log.Text = "Open RGB LOG Data";
            this.btn_get_log.UseVisualStyleBackColor = true;
            this.btn_get_log.Click += new System.EventHandler(this.btn_get_log_Click);
            // 
            // btn_save_actual_log
            // 
            this.btn_save_actual_log.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_save_actual_log.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_save_actual_log.Location = new System.Drawing.Point(630, 131);
            this.btn_save_actual_log.Name = "btn_save_actual_log";
            this.btn_save_actual_log.Size = new System.Drawing.Size(90, 42);
            this.btn_save_actual_log.TabIndex = 10;
            this.btn_save_actual_log.Text = "Save RGB LOG";
            this.btn_save_actual_log.UseVisualStyleBackColor = true;
            this.btn_save_actual_log.Click += new System.EventHandler(this.btn_save_actual_log_Click);
            // 
            // btn_tx_real_cam
            // 
            this.btn_tx_real_cam.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_tx_real_cam.BackColor = System.Drawing.Color.FromArgb(13, 148, 136);
            this.btn_tx_real_cam.ForeColor = System.Drawing.Color.White;
            this.btn_tx_real_cam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_tx_real_cam.Font = new System.Drawing.Font("MS UI Gothic", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_tx_real_cam.Location = new System.Drawing.Point(630, 175);
            this.btn_tx_real_cam.Name = "btn_tx_real_cam";
            this.btn_tx_real_cam.Size = new System.Drawing.Size(90, 38);
            this.btn_tx_real_cam.TabIndex = 12;
            this.btn_tx_real_cam.Text = "TX Cam (CAM)";
            this.btn_tx_real_cam.UseVisualStyleBackColor = false;
            this.btn_tx_real_cam.Click += new System.EventHandler(this.btn_tx_real_cam_Click);
            // 
            // btn_tx_dummy_cam
            // 
            this.btn_tx_dummy_cam.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_tx_dummy_cam.BackColor = System.Drawing.Color.FromArgb(124, 58, 237);
            this.btn_tx_dummy_cam.ForeColor = System.Drawing.Color.White;
            this.btn_tx_dummy_cam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_tx_dummy_cam.Font = new System.Drawing.Font("MS UI Gothic", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_tx_dummy_cam.Location = new System.Drawing.Point(626, 535);
            this.btn_tx_dummy_cam.Name = "btn_tx_dummy_cam";
            this.btn_tx_dummy_cam.Size = new System.Drawing.Size(94, 45);
            this.btn_tx_dummy_cam.TabIndex = 11;
            this.btn_tx_dummy_cam.Text = "TX Dummy (DC)";
            this.btn_tx_dummy_cam.UseVisualStyleBackColor = false;
            this.btn_tx_dummy_cam.Click += new System.EventHandler(this.btn_tx_dummy_cam_Click);
            // 
            // inputDialogCamera
            // 
            this.AllowDrop = true;
            this.ClientSize = new System.Drawing.Size(736, 586);
            this.ControlBox = false;
            this.Controls.Add(this.btn_tx_real_cam);
            this.Controls.Add(this.btn_tx_dummy_cam);
            this.Controls.Add(this.btn_save_actual_log);
            this.Controls.Add(this.btn_get_log);
            this.Controls.Add(this.btn_decode_log);
            this.Controls.Add(this.btn_get_image_data);
            this.Controls.Add(this.btn_verify_image_data);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_save_pic);
            this.Controls.Add(this.button_cancel_cam);
            this.Controls.Add(this.button_cam_paraser_ok);
            this.Controls.Add(this.rTBox_cam_raw_data);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            try { this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon"))); } catch { }
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "inputDialogCamera";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input Camera Raw Data";
            this.Load += new System.EventHandler(this.inputDialogCamera_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_img)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBox rTBox_cam_raw_data;
        private Button button_cam_paraser_ok;
        private Button button_cancel_cam;
        private Button button_save_pic;
        private GroupBox groupBox1;
        private PictureBox pictureBox_img;
        private Button btn_verify_image_data;
        private Button btn_get_image_data;
        private Button btn_decode_log;
        private Button btn_get_log;
        private Button btn_save_actual_log;
        private Button btn_tx_dummy_cam;
        private Button btn_tx_real_cam;
    }
}
