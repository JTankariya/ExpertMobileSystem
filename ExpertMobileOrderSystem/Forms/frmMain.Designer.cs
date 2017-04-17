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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolUploadStatus = new System.Windows.Forms.StatusStrip();
            this.toolUploadProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolUserInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.btnUserCompanyAllocation = new System.Windows.Forms.Button();
            this.btnCustomers = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnCompanies = new System.Windows.Forms.Button();
            this.btnEmployees = new System.Windows.Forms.Button();
            this.btnSync = new System.Windows.Forms.Button();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolUploadStatus.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolUploadStatus
            // 
            this.toolUploadStatus.GripMargin = new System.Windows.Forms.Padding(10);
            this.toolUploadStatus.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.toolUploadStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolUploadProgress,
            this.toolUserInfo});
            this.toolUploadStatus.Location = new System.Drawing.Point(0, 244);
            this.toolUploadStatus.Name = "toolUploadStatus";
            this.toolUploadStatus.Size = new System.Drawing.Size(724, 22);
            this.toolUploadStatus.TabIndex = 1;
            // 
            // toolUploadProgress
            // 
            this.toolUploadProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolUploadProgress.Name = "toolUploadProgress";
            this.toolUploadProgress.Size = new System.Drawing.Size(100, 16);
            this.toolUploadProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolUploadProgress.Visible = false;
            // 
            // toolUserInfo
            // 
            this.toolUserInfo.Name = "toolUserInfo";
            this.toolUserInfo.Size = new System.Drawing.Size(709, 17);
            this.toolUserInfo.Spring = true;
            this.toolUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolUserInfo.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "Expert Order Sync Utility is minimized to system tray.";
            this.notifyIcon1.BalloonTipTitle = "Expert Order Sync";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Expert Order Sync";
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(121, 60);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(120, 28);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnChangePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangePassword.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangePassword.Image = global::ExpertMobileOrderSystem.Properties.Resources._1486416542_key;
            this.btnChangePassword.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnChangePassword.Location = new System.Drawing.Point(485, 17);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(225, 96);
            this.btnChangePassword.TabIndex = 7;
            this.btnChangePassword.Text = "Change Password";
            this.btnChangePassword.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnChangePassword.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnChangePassword.UseVisualStyleBackColor = true;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // btnUserCompanyAllocation
            // 
            this.btnUserCompanyAllocation.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUserCompanyAllocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserCompanyAllocation.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.btnUserCompanyAllocation.Image = global::ExpertMobileOrderSystem.Properties.Resources._1486416909_lock_24;
            this.btnUserCompanyAllocation.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUserCompanyAllocation.Location = new System.Drawing.Point(178, 130);
            this.btnUserCompanyAllocation.Name = "btnUserCompanyAllocation";
            this.btnUserCompanyAllocation.Size = new System.Drawing.Size(315, 96);
            this.btnUserCompanyAllocation.TabIndex = 9;
            this.btnUserCompanyAllocation.Text = "User Company Allocation";
            this.btnUserCompanyAllocation.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUserCompanyAllocation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnUserCompanyAllocation.UseVisualStyleBackColor = true;
            this.btnUserCompanyAllocation.Click += new System.EventHandler(this.btnUserCompanyAllocation_Click);
            // 
            // btnCustomers
            // 
            this.btnCustomers.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCustomers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomers.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.btnCustomers.Image = global::ExpertMobileOrderSystem.Properties.Resources._1486415013_icons_user;
            this.btnCustomers.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCustomers.Location = new System.Drawing.Point(332, 17);
            this.btnCustomers.Name = "btnCustomers";
            this.btnCustomers.Size = new System.Drawing.Size(147, 96);
            this.btnCustomers.TabIndex = 11;
            this.btnCustomers.Text = "Customers";
            this.btnCustomers.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCustomers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCustomers.UseVisualStyleBackColor = true;
            this.btnCustomers.Click += new System.EventHandler(this.btnCustomers_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.Image = global::ExpertMobileOrderSystem.Properties.Resources._1486416649_settings;
            this.btnSettings.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSettings.Location = new System.Drawing.Point(17, 130);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(153, 96);
            this.btnSettings.TabIndex = 8;
            this.btnSettings.Text = "Settings";
            this.btnSettings.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnCompanies
            // 
            this.btnCompanies.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCompanies.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompanies.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCompanies.Image = global::ExpertMobileOrderSystem.Properties.Resources._1486415757_03_Office;
            this.btnCompanies.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCompanies.Location = new System.Drawing.Point(15, 17);
            this.btnCompanies.Name = "btnCompanies";
            this.btnCompanies.Size = new System.Drawing.Size(155, 96);
            this.btnCompanies.TabIndex = 5;
            this.btnCompanies.Text = "Companies";
            this.btnCompanies.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCompanies.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCompanies.UseVisualStyleBackColor = true;
            this.btnCompanies.Click += new System.EventHandler(this.btnCompanies_Click);
            // 
            // btnEmployees
            // 
            this.btnEmployees.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEmployees.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmployees.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.btnEmployees.Image = global::ExpertMobileOrderSystem.Properties.Resources._1486415013_icons_user;
            this.btnEmployees.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEmployees.Location = new System.Drawing.Point(178, 17);
            this.btnEmployees.Name = "btnEmployees";
            this.btnEmployees.Size = new System.Drawing.Size(147, 96);
            this.btnEmployees.TabIndex = 3;
            this.btnEmployees.Text = "Employees";
            this.btnEmployees.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEmployees.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEmployees.UseVisualStyleBackColor = true;
            this.btnEmployees.Click += new System.EventHandler(this.btnEmployees_Click);
            // 
            // btnSync
            // 
            this.btnSync.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSync.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSync.Image = global::ExpertMobileOrderSystem.Properties.Resources._1482787339_sync;
            this.btnSync.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSync.Location = new System.Drawing.Point(499, 131);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(186, 96);
            this.btnSync.TabIndex = 0;
            this.btnSync.Text = "Start Sync";
            this.btnSync.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSync.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(120, 28);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(724, 266);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.btnUserCompanyAllocation);
            this.Controls.Add(this.toolUploadStatus);
            this.Controls.Add(this.btnCustomers);
            this.Controls.Add(this.btnCompanies);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.btnEmployees);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order Sync Utility";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.toolUploadStatus.ResumeLayout(false);
            this.toolUploadStatus.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.StatusStrip toolUploadStatus;
        private System.Windows.Forms.ToolStripProgressBar toolUploadProgress;
        private System.Windows.Forms.Button btnEmployees;
        private System.Windows.Forms.Button btnCompanies;
        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnUserCompanyAllocation;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolUserInfo;
        private System.Windows.Forms.Button btnCustomers;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    }
}

