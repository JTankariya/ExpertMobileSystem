namespace ExpertMobileOrderSystem
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolUploadStatus = new System.Windows.Forms.StatusStrip();
            this.toolUploadProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnUserCompanyAllocation = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.btnCompanies = new System.Windows.Forms.Button();
            this.btnUsers = new System.Windows.Forms.Button();
            this.btnSync = new System.Windows.Forms.Button();
            this.toolUploadStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolUploadStatus
            // 
            this.toolUploadStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolUploadProgress,
            this.toolStripStatusLabel1});
            this.toolUploadStatus.Location = new System.Drawing.Point(0, 384);
            this.toolUploadStatus.Name = "toolUploadStatus";
            this.toolUploadStatus.Size = new System.Drawing.Size(665, 22);
            this.toolUploadStatus.TabIndex = 1;
            // 
            // toolUploadProgress
            // 
            this.toolUploadProgress.Name = "toolUploadProgress";
            this.toolUploadProgress.Size = new System.Drawing.Size(100, 16);
            this.toolUploadProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(45, 17);
            this.toolStripStatusLabel1.Text = "Name :";
            // 
            // btnUserCompanyAllocation
            // 
            this.btnUserCompanyAllocation.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUserCompanyAllocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserCompanyAllocation.Font = new System.Drawing.Font("Calibri", 15.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.btnUserCompanyAllocation.Image = global::ExpertMobileOrderSystem.Properties.Resources._1486416909_lock_24;
            this.btnUserCompanyAllocation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUserCompanyAllocation.Location = new System.Drawing.Point(467, 40);
            this.btnUserCompanyAllocation.Name = "btnUserCompanyAllocation";
            this.btnUserCompanyAllocation.Size = new System.Drawing.Size(186, 80);
            this.btnUserCompanyAllocation.TabIndex = 9;
            this.btnUserCompanyAllocation.Text = "User Company Allocation";
            this.btnUserCompanyAllocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUserCompanyAllocation.UseVisualStyleBackColor = true;
            this.btnUserCompanyAllocation.Click += new System.EventHandler(this.btnUserCompanyAllocation_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.Image = global::ExpertMobileOrderSystem.Properties.Resources._1486416649_settings;
            this.btnSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettings.Location = new System.Drawing.Point(229, 153);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(203, 80);
            this.btnSettings.TabIndex = 8;
            this.btnSettings.Text = "         Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnChangePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangePassword.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangePassword.Image = global::ExpertMobileOrderSystem.Properties.Resources._1486416542_key;
            this.btnChangePassword.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChangePassword.Location = new System.Drawing.Point(12, 153);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(183, 80);
            this.btnChangePassword.TabIndex = 7;
            this.btnChangePassword.Text = "         Change         Password";
            this.btnChangePassword.UseVisualStyleBackColor = true;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // btnCompanies
            // 
            this.btnCompanies.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCompanies.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompanies.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCompanies.Image = global::ExpertMobileOrderSystem.Properties.Resources._1486415757_03_Office;
            this.btnCompanies.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCompanies.Location = new System.Drawing.Point(229, 40);
            this.btnCompanies.Name = "btnCompanies";
            this.btnCompanies.Size = new System.Drawing.Size(203, 80);
            this.btnCompanies.TabIndex = 5;
            this.btnCompanies.Text = "Companies";
            this.btnCompanies.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCompanies.UseVisualStyleBackColor = true;
            this.btnCompanies.Click += new System.EventHandler(this.btnCompanies_Click);
            // 
            // btnUsers
            // 
            this.btnUsers.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsers.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUsers.Image = global::ExpertMobileOrderSystem.Properties.Resources._1486415013_icons_user;
            this.btnUsers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUsers.Location = new System.Drawing.Point(12, 40);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(183, 80);
            this.btnUsers.TabIndex = 3;
            this.btnUsers.Text = "          Users";
            this.btnUsers.UseVisualStyleBackColor = true;
            this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
            // 
            // btnSync
            // 
            this.btnSync.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSync.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSync.Image = global::ExpertMobileOrderSystem.Properties.Resources._1482787339_sync;
            this.btnSync.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSync.Location = new System.Drawing.Point(467, 153);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(186, 80);
            this.btnSync.TabIndex = 0;
            this.btnSync.Text = "Start Sync";
            this.btnSync.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(665, 406);
            this.Controls.Add(this.btnUserCompanyAllocation);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.btnCompanies);
            this.Controls.Add(this.btnUsers);
            this.Controls.Add(this.toolUploadStatus);
            this.Controls.Add(this.btnSync);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order Sync Utility";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.toolUploadStatus.ResumeLayout(false);
            this.toolUploadStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.StatusStrip toolUploadStatus;
        private System.Windows.Forms.ToolStripProgressBar toolUploadProgress;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Button btnCompanies;
        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnUserCompanyAllocation;
    }
}

