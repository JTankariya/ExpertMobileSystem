namespace ExpertMobileSystem_Client_
{
    partial class frmMDI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMDI));
            this.tBackGround = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ControlMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.ControlLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolUploadStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolUploadProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolNextUpload = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.masterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.companyMasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userCreationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.userwiseCompanyAllocationMasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userCompanyMenuAllocationMasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.dashboardMasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userDashboardMasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsForDataUploadingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadDataNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tGetExpertData = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tNextUploadStatus = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tBackGround
            // 
            this.tBackGround.Enabled = true;
            this.tBackGround.Interval = 1000;
            this.tBackGround.Tick += new System.EventHandler(this.tBackGround_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.SkyBlue;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.ControlMessage,
            this.ControlLabel,
            this.toolUploadStatus,
            this.toolUploadProgress,
            this.toolNextUpload});
            this.statusStrip1.Location = new System.Drawing.Point(0, 429);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(676, 24);
            this.statusStrip1.TabIndex = 72;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(51, 19);
            this.toolStripStatusLabel2.Text = "User : ";
            // 
            // ControlMessage
            // 
            this.ControlMessage.BackColor = System.Drawing.Color.SkyBlue;
            this.ControlMessage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.ControlMessage.Name = "ControlMessage";
            this.ControlMessage.Size = new System.Drawing.Size(610, 19);
            this.ControlMessage.Spring = true;
            this.ControlMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ControlLabel
            // 
            this.ControlLabel.Name = "ControlLabel";
            this.ControlLabel.Size = new System.Drawing.Size(0, 19);
            // 
            // toolUploadStatus
            // 
            this.toolUploadStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolUploadStatus.Name = "toolUploadStatus";
            this.toolUploadStatus.Size = new System.Drawing.Size(0, 19);
            this.toolUploadStatus.Visible = false;
            // 
            // toolUploadProgress
            // 
            this.toolUploadProgress.AutoSize = false;
            this.toolUploadProgress.Name = "toolUploadProgress";
            this.toolUploadProgress.Size = new System.Drawing.Size(200, 18);
            this.toolUploadProgress.Visible = false;
            // 
            // toolNextUpload
            // 
            this.toolNextUpload.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolNextUpload.Name = "toolNextUpload";
            this.toolNextUpload.Size = new System.Drawing.Size(0, 19);
            this.toolNextUpload.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.SkyBlue;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.masterToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(676, 26);
            this.menuStrip1.TabIndex = 82;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // masterToolStripMenuItem
            // 
            this.masterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.companyMasterToolStripMenuItem,
            this.userCreationToolStripMenuItem,
            this.toolStripMenuItem2,
            this.userwiseCompanyAllocationMasterToolStripMenuItem,
            this.userCompanyMenuAllocationMasterToolStripMenuItem,
            this.toolStripMenuItem1,
            this.dashboardMasterToolStripMenuItem,
            this.userDashboardMasterToolStripMenuItem,
            this.toolStripSeparator1,
            this.changePasswordToolStripMenuItem});
            this.masterToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 11F);
            this.masterToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.masterToolStripMenuItem.Name = "masterToolStripMenuItem";
            this.masterToolStripMenuItem.Size = new System.Drawing.Size(71, 22);
            this.masterToolStripMenuItem.Text = "Master";
            this.masterToolStripMenuItem.Click += new System.EventHandler(this.masterToolStripMenuItem_Click);
            // 
            // companyMasterToolStripMenuItem
            // 
            this.companyMasterToolStripMenuItem.Name = "companyMasterToolStripMenuItem";
            this.companyMasterToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.companyMasterToolStripMenuItem.Text = "Company Setup Master";
            this.companyMasterToolStripMenuItem.Click += new System.EventHandler(this.companyMasterToolStripMenuItem_Click);
            // 
            // userCreationToolStripMenuItem
            // 
            this.userCreationToolStripMenuItem.Name = "userCreationToolStripMenuItem";
            this.userCreationToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.userCreationToolStripMenuItem.Text = "User Creation Master";
            this.userCreationToolStripMenuItem.Click += new System.EventHandler(this.userCreationToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(365, 6);
            // 
            // userwiseCompanyAllocationMasterToolStripMenuItem
            // 
            this.userwiseCompanyAllocationMasterToolStripMenuItem.Name = "userwiseCompanyAllocationMasterToolStripMenuItem";
            this.userwiseCompanyAllocationMasterToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.userwiseCompanyAllocationMasterToolStripMenuItem.Text = "Userwise Company Allocation Master";
            this.userwiseCompanyAllocationMasterToolStripMenuItem.Click += new System.EventHandler(this.userwiseCompanyAllocationMasterToolStripMenuItem_Click);
            // 
            // userCompanyMenuAllocationMasterToolStripMenuItem
            // 
            this.userCompanyMenuAllocationMasterToolStripMenuItem.Name = "userCompanyMenuAllocationMasterToolStripMenuItem";
            this.userCompanyMenuAllocationMasterToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.userCompanyMenuAllocationMasterToolStripMenuItem.Text = "User-Company-Menu Allocation Master";
            this.userCompanyMenuAllocationMasterToolStripMenuItem.Click += new System.EventHandler(this.userCompanyMenuAllocationMasterToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(365, 6);
            // 
            // dashboardMasterToolStripMenuItem
            // 
            this.dashboardMasterToolStripMenuItem.Name = "dashboardMasterToolStripMenuItem";
            this.dashboardMasterToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.dashboardMasterToolStripMenuItem.Text = "User-SubMenu-Allocation Master";
            this.dashboardMasterToolStripMenuItem.Click += new System.EventHandler(this.dashboardMasterToolStripMenuItem_Click);
            // 
            // userDashboardMasterToolStripMenuItem
            // 
            this.userDashboardMasterToolStripMenuItem.Name = "userDashboardMasterToolStripMenuItem";
            this.userDashboardMasterToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.userDashboardMasterToolStripMenuItem.Text = "User-Dashboard-Allocation Master";
            this.userDashboardMasterToolStripMenuItem.Click += new System.EventHandler(this.userDashboardMasterToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(365, 6);
            // 
            // changePasswordToolStripMenuItem
            // 
            this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            this.changePasswordToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.changePasswordToolStripMenuItem.Text = "Change Password";
            this.changePasswordToolStripMenuItem.Click += new System.EventHandler(this.changePasswordToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsForDataUploadingToolStripMenuItem,
            this.uploadDataNowToolStripMenuItem});
            this.settingsToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(80, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // settingsForDataUploadingToolStripMenuItem
            // 
            this.settingsForDataUploadingToolStripMenuItem.Name = "settingsForDataUploadingToolStripMenuItem";
            this.settingsForDataUploadingToolStripMenuItem.Size = new System.Drawing.Size(283, 22);
            this.settingsForDataUploadingToolStripMenuItem.Text = "Settings For Data Uploading";
            this.settingsForDataUploadingToolStripMenuItem.Click += new System.EventHandler(this.settingsForDataUploadingToolStripMenuItem_Click);
            // 
            // uploadDataNowToolStripMenuItem
            // 
            this.uploadDataNowToolStripMenuItem.Name = "uploadDataNowToolStripMenuItem";
            this.uploadDataNowToolStripMenuItem.Size = new System.Drawing.Size(283, 22);
            this.uploadDataNowToolStripMenuItem.Text = "Upload Data Now";
            this.uploadDataNowToolStripMenuItem.Click += new System.EventHandler(this.uploadDataNowToolStripMenuItem_Click);
            // 
            // tGetExpertData
            // 
            this.tGetExpertData.Enabled = true;
            this.tGetExpertData.Interval = 5000;
            this.tGetExpertData.Tick += new System.EventHandler(this.tGetExpertData_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "Information";
            this.notifyIcon1.BalloonTipTitle = "ExpertMobile System";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "ExpertMobile System";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(107, 30);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(106, 26);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // tNextUploadStatus
            // 
            this.tNextUploadStatus.Enabled = true;
            this.tNextUploadStatus.Interval = 1000;
            this.tNextUploadStatus.Tick += new System.EventHandler(this.tNextUploadStatus_Tick);
            // 
            // frmMDI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::ExpertMobileSystem_Client_.Properties.Resources.BackImage_Main;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(676, 453);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMDI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Expert Mobile System - Client";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMDI_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMDI_FormClosing);
            this.Resize += new System.EventHandler(this.frmMDI_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMDI_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tBackGround;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel ControlMessage;
        private System.Windows.Forms.ToolStripStatusLabel ControlLabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem masterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem companyMasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userCreationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem userwiseCompanyAllocationMasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userCompanyMenuAllocationMasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dashboardMasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userDashboardMasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsForDataUploadingToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem uploadDataNowToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolNextUpload;
        private System.Windows.Forms.Timer tNextUploadStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolUploadStatus;
        private System.Windows.Forms.ToolStripProgressBar toolUploadProgress;
        private System.Windows.Forms.Timer tGetExpertData;
    }
}

