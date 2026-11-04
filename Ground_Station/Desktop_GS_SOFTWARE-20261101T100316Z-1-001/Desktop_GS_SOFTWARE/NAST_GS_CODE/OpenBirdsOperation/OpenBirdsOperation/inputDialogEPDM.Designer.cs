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
    partial class inputDialogEPDM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(inputDialogEPDM));
            this.rTBox_epdm_raw_data = new System.Windows.Forms.RichTextBox();
            this.button_cancel_epdm = new System.Windows.Forms.Button();
            this.btn_verify_EPDM = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rTBox_epdm_raw_data
            // 
            this.rTBox_epdm_raw_data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rTBox_epdm_raw_data.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rTBox_epdm_raw_data.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rTBox_epdm_raw_data.Location = new System.Drawing.Point(9, 8);
            this.rTBox_epdm_raw_data.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rTBox_epdm_raw_data.Name = "rTBox_epdm_raw_data";
            this.rTBox_epdm_raw_data.Size = new System.Drawing.Size(452, 107);
            this.rTBox_epdm_raw_data.TabIndex = 0;
            this.rTBox_epdm_raw_data.Text = "";
            this.rTBox_epdm_raw_data.TextChanged += new System.EventHandler(this.rTBox_epdm_raw_data_TextChanged);
            // 
            // button_cancel_epdm
            // 
            this.button_cancel_epdm.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_cancel_epdm.Font = new System.Drawing.Font("MS UI Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_cancel_epdm.Location = new System.Drawing.Point(466, 63);
            this.button_cancel_epdm.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_cancel_epdm.Name = "button_cancel_epdm";
            this.button_cancel_epdm.Size = new System.Drawing.Size(70, 38);
            this.button_cancel_epdm.TabIndex = 2;
            this.button_cancel_epdm.Text = "Cancel";
            this.button_cancel_epdm.UseVisualStyleBackColor = true;
            this.button_cancel_epdm.Click += new System.EventHandler(this.button_cancel_epdm_Click);
            // 
            // btn_verify_EPDM
            // 
            this.btn_verify_EPDM.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_verify_EPDM.Font = new System.Drawing.Font("MS UI Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_verify_EPDM.Location = new System.Drawing.Point(466, 21);
            this.btn_verify_EPDM.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_verify_EPDM.Name = "btn_verify_EPDM";
            this.btn_verify_EPDM.Size = new System.Drawing.Size(70, 38);
            this.btn_verify_EPDM.TabIndex = 3;
            this.btn_verify_EPDM.Text = "Verify EPDM";
            this.btn_verify_EPDM.UseVisualStyleBackColor = true;
            this.btn_verify_EPDM.Click += new System.EventHandler(this.btn_verify_EPDM_Click);
            // 
            // inputDialogEPDM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 121);
            this.ControlBox = false;
            this.Controls.Add(this.btn_verify_EPDM);
            this.Controls.Add(this.button_cancel_epdm);
            this.Controls.Add(this.rTBox_epdm_raw_data);
            try { this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon"))); } catch { }
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "inputDialogEPDM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input EPDM Raw Data";
            this.Load += new System.EventHandler(this.inputDialogEPDM_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBox rTBox_epdm_raw_data;
        private Button button_cancel_epdm;
        private Button btn_verify_EPDM;
    }
}