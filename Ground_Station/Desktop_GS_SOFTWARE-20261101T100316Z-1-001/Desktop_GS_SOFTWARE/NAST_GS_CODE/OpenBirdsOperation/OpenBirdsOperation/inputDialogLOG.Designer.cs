namespace OpenBirdsOperation
{
    partial class inputDialogLOG
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
            this.rTBox_LOG_raw_data = new System.Windows.Forms.RichTextBox();
            this.btn_verify_LOG = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rTBox_LOG_raw_data
            // 
            this.rTBox_LOG_raw_data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rTBox_LOG_raw_data.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rTBox_LOG_raw_data.Location = new System.Drawing.Point(12, 12);
            this.rTBox_LOG_raw_data.Name = "rTBox_LOG_raw_data";
            this.rTBox_LOG_raw_data.Size = new System.Drawing.Size(675, 139);
            this.rTBox_LOG_raw_data.TabIndex = 0;
            this.rTBox_LOG_raw_data.Text = "";
            this.rTBox_LOG_raw_data.TextChanged += new System.EventHandler(this.rTBox_LOG_raw_data_TextChanged);
            // 
            // btn_verify_LOG
            // 
            this.btn_verify_LOG.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_verify_LOG.Location = new System.Drawing.Point(693, 34);
            this.btn_verify_LOG.Name = "btn_verify_LOG";
            this.btn_verify_LOG.Size = new System.Drawing.Size(88, 48);
            this.btn_verify_LOG.TabIndex = 1;
            this.btn_verify_LOG.Text = "Verify LOG Data";
            this.btn_verify_LOG.UseVisualStyleBackColor = true;
            this.btn_verify_LOG.Click += new System.EventHandler(this.btn_verify_LOG_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cancel.Location = new System.Drawing.Point(693, 88);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(88, 48);
            this.btn_cancel.TabIndex = 4;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // inputDialogLOG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 163);
            this.ControlBox = false;
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_verify_LOG);
            this.Controls.Add(this.rTBox_LOG_raw_data);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "inputDialogLOG";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input LOG Raw Data";
            this.Load += new System.EventHandler(this.inputDialogLOG_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rTBox_LOG_raw_data;
        private System.Windows.Forms.Button btn_verify_LOG;
        private System.Windows.Forms.Button btn_cancel;
    }
}