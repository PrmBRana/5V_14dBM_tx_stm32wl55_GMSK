namespace OpenBirdsOperation
{
    partial class inputDialogDigipeater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(inputDialogDigipeater));
            this.rTBox_digipeater_raw_data = new System.Windows.Forms.RichTextBox();
            this.btn_verify_digipeater_data = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.button_open_file = new System.Windows.Forms.Button();
            this.button_test = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rTBox_digipeater_raw_data
            // 
            this.rTBox_digipeater_raw_data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rTBox_digipeater_raw_data.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rTBox_digipeater_raw_data.Location = new System.Drawing.Point(20, 17);
            this.rTBox_digipeater_raw_data.Margin = new System.Windows.Forms.Padding(4);
            this.rTBox_digipeater_raw_data.Name = "rTBox_digipeater_raw_data";
            this.rTBox_digipeater_raw_data.Size = new System.Drawing.Size(886, 366);
            this.rTBox_digipeater_raw_data.TabIndex = 0;
            this.rTBox_digipeater_raw_data.Text = "";
            this.rTBox_digipeater_raw_data.TextChanged += new System.EventHandler(this.rTBox_digipeater_raw_data_TextChanged);
            // 
            // btn_verify_digipeater_data
            // 
            this.btn_verify_digipeater_data.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_verify_digipeater_data.Font = new System.Drawing.Font("MS UI Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_verify_digipeater_data.Location = new System.Drawing.Point(914, 203);
            this.btn_verify_digipeater_data.Margin = new System.Windows.Forms.Padding(4);
            this.btn_verify_digipeater_data.Name = "btn_verify_digipeater_data";
            this.btn_verify_digipeater_data.Size = new System.Drawing.Size(149, 87);
            this.btn_verify_digipeater_data.TabIndex = 1;
            this.btn_verify_digipeater_data.Text = "Save Data";
            this.btn_verify_digipeater_data.UseVisualStyleBackColor = true;
            this.btn_verify_digipeater_data.Click += new System.EventHandler(this.btn_verify_digipeater_data_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cancel.Font = new System.Drawing.Font("MS UI Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cancel.Location = new System.Drawing.Point(914, 298);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(149, 87);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // button_open_file
            // 
            this.button_open_file.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_open_file.Font = new System.Drawing.Font("MS UI Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_open_file.Location = new System.Drawing.Point(914, 108);
            this.button_open_file.Margin = new System.Windows.Forms.Padding(4);
            this.button_open_file.Name = "button_open_file";
            this.button_open_file.Size = new System.Drawing.Size(149, 87);
            this.button_open_file.TabIndex = 3;
            this.button_open_file.Text = "Open File";
            this.button_open_file.UseVisualStyleBackColor = true;
            this.button_open_file.Click += new System.EventHandler(this.button_open_file_Click);
            // 
            // button_test
            // 
            this.button_test.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_test.Font = new System.Drawing.Font("MS UI Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_test.Location = new System.Drawing.Point(914, 13);
            this.button_test.Margin = new System.Windows.Forms.Padding(4);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(149, 87);
            this.button_test.TabIndex = 4;
            this.button_test.Text = "Verify Data";
            this.button_test.UseVisualStyleBackColor = true;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // inputDialogDigipeater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 396);
            this.ControlBox = false;
            this.Controls.Add(this.button_test);
            this.Controls.Add(this.button_open_file);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_verify_digipeater_data);
            this.Controls.Add(this.rTBox_digipeater_raw_data);
            try { this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon"))); } catch { }
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "inputDialogDigipeater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input Digipeater Raw Data";
            this.Load += new System.EventHandler(this.inputDialogDigipeater_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rTBox_digipeater_raw_data;
        private System.Windows.Forms.Button btn_verify_digipeater_data;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button button_open_file;
        private System.Windows.Forms.Button button_test;
    }
}