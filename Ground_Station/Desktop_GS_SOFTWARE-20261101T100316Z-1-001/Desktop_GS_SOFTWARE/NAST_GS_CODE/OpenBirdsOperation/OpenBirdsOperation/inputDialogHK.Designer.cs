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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace OpenBirdsOperation
{
    partial class inputDialogHK : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(inputDialogHK));
            this.rTBox_hk_raw_data = new System.Windows.Forms.RichTextBox();
            this.button_hk_parser_ok = new System.Windows.Forms.Button();
            this.button_hk_cancel = new System.Windows.Forms.Button();
            this.button_ver_hk = new System.Windows.Forms.Button();
            this.button_open = new System.Windows.Forms.Button();
            this.button_req_hk1 = new System.Windows.Forms.Button();
            this.button_req_hk5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rTBox_hk_raw_data
            // 
            this.rTBox_hk_raw_data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rTBox_hk_raw_data.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rTBox_hk_raw_data.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rTBox_hk_raw_data.Location = new System.Drawing.Point(18, 17);
            this.rTBox_hk_raw_data.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rTBox_hk_raw_data.Name = "rTBox_hk_raw_data";
            this.rTBox_hk_raw_data.Size = new System.Drawing.Size(882, 329);
            this.rTBox_hk_raw_data.TabIndex = 0;
            this.rTBox_hk_raw_data.Text = "";
            this.rTBox_hk_raw_data.TextChanged += new System.EventHandler(this.rTBox_hk_raw_data_TextChanged);
            // 
            // button_ver_hk
            // 
            this.button_ver_hk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_ver_hk.Font = new System.Drawing.Font("MS UI Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ver_hk.Location = new System.Drawing.Point(912, 16);
            this.button_ver_hk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_ver_hk.Name = "button_ver_hk";
            this.button_ver_hk.Size = new System.Drawing.Size(145, 50);
            this.button_ver_hk.TabIndex = 3;
            this.button_ver_hk.Text = "Verify HK Data";
            this.button_ver_hk.UseVisualStyleBackColor = true;
            this.button_ver_hk.Click += new System.EventHandler(this.button_ver_hk_Click);
            // 
            // button_open
            // 
            this.button_open.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_open.Font = new System.Drawing.Font("MS UI Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_open.Location = new System.Drawing.Point(912, 72);
            this.button_open.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(145, 50);
            this.button_open.TabIndex = 4;
            this.button_open.Text = "Open File";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // button_req_hk1
            // 
            this.button_req_hk1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_req_hk1.BackColor = System.Drawing.Color.FromArgb(29, 78, 216);
            this.button_req_hk1.ForeColor = System.Drawing.Color.White;
            this.button_req_hk1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_req_hk1.Font = new System.Drawing.Font("MS UI Gothic", 9.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_req_hk1.Location = new System.Drawing.Point(912, 128);
            this.button_req_hk1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_req_hk1.Name = "button_req_hk1";
            this.button_req_hk1.Size = new System.Drawing.Size(145, 50);
            this.button_req_hk1.TabIndex = 5;
            this.button_req_hk1.Text = "TX Live HK (HK1)";
            this.button_req_hk1.UseVisualStyleBackColor = false;
            this.button_req_hk1.Click += new System.EventHandler(this.button_req_hk1_Click);
            // 
            // button_req_hk5
            // 
            this.button_req_hk5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_req_hk5.BackColor = System.Drawing.Color.FromArgb(13, 148, 136);
            this.button_req_hk5.ForeColor = System.Drawing.Color.White;
            this.button_req_hk5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_req_hk5.Font = new System.Drawing.Font("MS UI Gothic", 9.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_req_hk5.Location = new System.Drawing.Point(912, 184);
            this.button_req_hk5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_req_hk5.Name = "button_req_hk5";
            this.button_req_hk5.Size = new System.Drawing.Size(145, 50);
            this.button_req_hk5.TabIndex = 6;
            this.button_req_hk5.Text = "TX 5 HK Pkts";
            this.button_req_hk5.UseVisualStyleBackColor = false;
            this.button_req_hk5.Click += new System.EventHandler(this.button_req_hk5_Click);
            // 
            // button_hk_parser_ok
            // 
            this.button_hk_parser_ok.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_hk_parser_ok.Font = new System.Drawing.Font("MS UI Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_hk_parser_ok.Location = new System.Drawing.Point(912, 240);
            this.button_hk_parser_ok.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_hk_parser_ok.Name = "button_hk_parser_ok";
            this.button_hk_parser_ok.Size = new System.Drawing.Size(145, 50);
            this.button_hk_parser_ok.TabIndex = 1;
            this.button_hk_parser_ok.Text = "Save HK";
            this.button_hk_parser_ok.UseVisualStyleBackColor = true;
            this.button_hk_parser_ok.Click += new System.EventHandler(this.button_hk_parser_ok_Click);
            // 
            // button_hk_cancel
            // 
            this.button_hk_cancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_hk_cancel.Font = new System.Drawing.Font("MS UI Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_hk_cancel.Location = new System.Drawing.Point(912, 296);
            this.button_hk_cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_hk_cancel.Name = "button_hk_cancel";
            this.button_hk_cancel.Size = new System.Drawing.Size(145, 50);
            this.button_hk_cancel.TabIndex = 2;
            this.button_hk_cancel.Text = "Cancel";
            this.button_hk_cancel.UseVisualStyleBackColor = true;
            this.button_hk_cancel.Click += new System.EventHandler(this.button_hk_cancel_Click);
            // 
            // inputDialogHK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 366);
            this.ControlBox = false;
            this.Controls.Add(this.button_req_hk5);
            this.Controls.Add(this.button_req_hk1);
            this.Controls.Add(this.button_open);
            this.Controls.Add(this.button_ver_hk);
            this.Controls.Add(this.button_hk_cancel);
            this.Controls.Add(this.button_hk_parser_ok);
            this.Controls.Add(this.rTBox_hk_raw_data);
            try { this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon"))); } catch { }
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "inputDialogHK";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input House Keeping Raw Data";
            this.Load += new System.EventHandler(this.inputDialogHK_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBox rTBox_hk_raw_data;
        private Button button_hk_parser_ok;
        private Button button_hk_cancel;
        private Button button_ver_hk;
        private Button button_open;
        private Button button_req_hk1;
        private Button button_req_hk5;
    }
}
