namespace ExpertMobileOrderSystem
{
    partial class frmSettings
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAfterUpload = new System.Windows.Forms.CheckBox();
            this.lstTimeInterval = new System.Windows.Forms.CheckedListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkPrompt = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDataUploadedOption = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.chkAfterUpload);
            this.groupBox1.Controls.Add(this.lstTimeInterval);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtMin);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkPrompt);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbDataUploadedOption);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(449, 427);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // chkAfterUpload
            // 
            this.chkAfterUpload.AutoSize = true;
            this.chkAfterUpload.Location = new System.Drawing.Point(19, 331);
            this.chkAfterUpload.Name = "chkAfterUpload";
            this.chkAfterUpload.Size = new System.Drawing.Size(239, 20);
            this.chkAfterUpload.TabIndex = 8;
            this.chkAfterUpload.Text = "Prompt Me After Uploading Data";
            this.chkAfterUpload.UseVisualStyleBackColor = true;
            // 
            // lstTimeInterval
            // 
            this.lstTimeInterval.CheckOnClick = true;
            this.lstTimeInterval.FormattingEnabled = true;
            this.lstTimeInterval.Location = new System.Drawing.Point(175, 52);
            this.lstTimeInterval.Name = "lstTimeInterval";
            this.lstTimeInterval.Size = new System.Drawing.Size(195, 202);
            this.lstTimeInterval.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Image = global::ExpertMobileOrderSystem.Properties.Resources.accept;
            this.btnSave.Location = new System.Drawing.Point(175, 366);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 47);
            this.btnSave.TabIndex = 4;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(388, 303);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Mins";
            // 
            // txtMin
            // 
            this.txtMin.Enabled = false;
            this.txtMin.Location = new System.Drawing.Point(296, 300);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(86, 23);
            this.txtMin.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 300);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(273, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Time in Minutes Before Uploading Data :";
            // 
            // chkPrompt
            // 
            this.chkPrompt.AutoSize = true;
            this.chkPrompt.Location = new System.Drawing.Point(19, 263);
            this.chkPrompt.Name = "chkPrompt";
            this.chkPrompt.Size = new System.Drawing.Size(245, 20);
            this.chkPrompt.TabIndex = 2;
            this.chkPrompt.Text = "Prompt Me Before Uploaded Data";
            this.chkPrompt.UseVisualStyleBackColor = true;
            this.chkPrompt.CheckedChanged += new System.EventHandler(this.chkPrompt_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Time Interval : ";
            // 
            // cmbDataUploadedOption
            // 
            this.cmbDataUploadedOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataUploadedOption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDataUploadedOption.FormattingEnabled = true;
            this.cmbDataUploadedOption.Items.AddRange(new object[] {
            "Daily"});
            this.cmbDataUploadedOption.Location = new System.Drawing.Point(175, 21);
            this.cmbDataUploadedOption.Name = "cmbDataUploadedOption";
            this.cmbDataUploadedOption.Size = new System.Drawing.Size(195, 24);
            this.cmbDataUploadedOption.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Uploaded Option : ";
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(454, 434);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setting For Data Uploading";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbDataUploadedOption;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkPrompt;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckedListBox lstTimeInterval;
        private System.Windows.Forms.CheckBox chkAfterUpload;
    }
}