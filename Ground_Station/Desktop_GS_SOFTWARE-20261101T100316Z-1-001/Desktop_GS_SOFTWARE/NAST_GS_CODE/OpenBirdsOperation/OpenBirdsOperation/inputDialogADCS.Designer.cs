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

namespace OpenBirdsOperation
{
    partial class inputDialogADCS : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(inputDialogADCS));
            this.rTBox_adsc_raw_data = new System.Windows.Forms.RichTextBox();
            this.button_cancel_adcs = new System.Windows.Forms.Button();
            this.btn_verify_adcs = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rTBox_adsc_raw_data
            // 
            this.rTBox_adsc_raw_data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rTBox_adsc_raw_data.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rTBox_adsc_raw_data.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rTBox_adsc_raw_data.Location = new System.Drawing.Point(9, 9);
            this.rTBox_adsc_raw_data.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rTBox_adsc_raw_data.Name = "rTBox_adsc_raw_data";
            this.rTBox_adsc_raw_data.Size = new System.Drawing.Size(440, 122);
            this.rTBox_adsc_raw_data.TabIndex = 0;
            this.rTBox_adsc_raw_data.Text = "";
            this.rTBox_adsc_raw_data.TextChanged += new System.EventHandler(this.rTBox_adsc_raw_data_TextChanged);
            // 
            // button_cancel_adcs
            // 
            this.button_cancel_adcs.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_cancel_adcs.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_cancel_adcs.Location = new System.Drawing.Point(458, 74);
            this.button_cancel_adcs.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_cancel_adcs.Name = "button_cancel_adcs";
            this.button_cancel_adcs.Size = new System.Drawing.Size(70, 38);
            this.button_cancel_adcs.TabIndex = 2;
            this.button_cancel_adcs.Text = "Cancel";
            this.button_cancel_adcs.UseVisualStyleBackColor = true;
            this.button_cancel_adcs.Click += new System.EventHandler(this.button_cancel_adcs_Click);
            // 
            // btn_verify_adcs
            // 
            this.btn_verify_adcs.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_verify_adcs.Font = new System.Drawing.Font("MS UI Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_verify_adcs.Location = new System.Drawing.Point(458, 23);
            this.btn_verify_adcs.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_verify_adcs.Name = "btn_verify_adcs";
            this.btn_verify_adcs.Size = new System.Drawing.Size(70, 38);
            this.btn_verify_adcs.TabIndex = 3;
            this.btn_verify_adcs.Text = "Verify ADCS";
            this.btn_verify_adcs.UseVisualStyleBackColor = true;
            this.btn_verify_adcs.Click += new System.EventHandler(this.btn_verify_adcs_Click);
            // 
            // inputDialogADCS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 134);
            this.ControlBox = false;
            this.Controls.Add(this.btn_verify_adcs);
            this.Controls.Add(this.button_cancel_adcs);
            this.Controls.Add(this.rTBox_adsc_raw_data);
            try { this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon"))); } catch { }
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "inputDialogADCS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input ADCS Raw Data";
            this.Load += new System.EventHandler(this.inputDialogADCS_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBox rTBox_adsc_raw_data;
        private Button button_cancel_adcs;
        private Button btn_verify_adcs;
    }
}