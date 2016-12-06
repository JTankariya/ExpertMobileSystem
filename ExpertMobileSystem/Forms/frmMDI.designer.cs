namespace ExpertMobileSystem
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
            this.tBackGround = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ControlMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.ControlLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.MDIMenu = new System.Windows.Forms.MenuStrip();
            this.masterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.clientMasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientMenuAllocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientDashboardMasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientMasterToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.clientLoginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.MDIMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tBackGround
            // 
            this.tBackGround.Enabled = true;
            this.tBackGround.Interval = 10000;
            this.tBackGround.Tick += new System.EventHandler(this.tBackGround_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.ControlMessage,
            this.ControlLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 429);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(676, 24);
            this.statusStrip1.TabIndex = 72;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(51, 19);
            this.toolStripStatusLabel2.Text = "User : ";
            // 
            // ControlMessage
            // 
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
            // MDIMenu
            // 
            this.MDIMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.masterToolStripMenuItem,
            this.clientMasterToolStripMenuItem1});
            this.MDIMenu.Location = new System.Drawing.Point(0, 0);
            this.MDIMenu.Name = "MDIMenu";
            this.MDIMenu.Size = new System.Drawing.Size(676, 26);
            this.MDIMenu.TabIndex = 82;
            this.MDIMenu.Text = "menuStrip1";
            // 
            // masterToolStripMenuItem
            // 
            this.masterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.menuMasterToolStripMenuItem,
            this.toolStripMenuItem1,
            this.clientMasterToolStripMenuItem,
            this.clientMenuAllocationToolStripMenuItem,
            this.clientDashboardMasterToolStripMenuItem,
            this.toolStripSeparator1,
            this.changePasswordToolStripMenuItem});
            this.masterToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterToolStripMenuItem.Name = "masterToolStripMenuItem";
            this.masterToolStripMenuItem.Size = new System.Drawing.Size(71, 22);
            this.masterToolStripMenuItem.Text = "Master";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(332, 22);
            this.toolStripMenuItem2.Text = "Admin User Master";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // menuMasterToolStripMenuItem
            // 
            this.menuMasterToolStripMenuItem.Name = "menuMasterToolStripMenuItem";
            this.menuMasterToolStripMenuItem.Size = new System.Drawing.Size(332, 22);
            this.menuMasterToolStripMenuItem.Text = "Menu Master";
            this.menuMasterToolStripMenuItem.Click += new System.EventHandler(this.menuMasterToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(329, 6);
            // 
            // clientMasterToolStripMenuItem
            // 
            this.clientMasterToolStripMenuItem.Name = "clientMasterToolStripMenuItem";
            this.clientMasterToolStripMenuItem.Size = new System.Drawing.Size(332, 22);
            this.clientMasterToolStripMenuItem.Text = "Client Master With Menu Allocation";
            this.clientMasterToolStripMenuItem.Click += new System.EventHandler(this.clientMasterToolStripMenuItem_Click);
            // 
            // clientMenuAllocationToolStripMenuItem
            // 
            this.clientMenuAllocationToolStripMenuItem.Name = "clientMenuAllocationToolStripMenuItem";
            this.clientMenuAllocationToolStripMenuItem.Size = new System.Drawing.Size(332, 22);
            this.clientMenuAllocationToolStripMenuItem.Text = "Client SubMenu Master";
            this.clientMenuAllocationToolStripMenuItem.Click += new System.EventHandler(this.clientMenuAllocationToolStripMenuItem_Click);
            // 
            // clientDashboardMasterToolStripMenuItem
            // 
            this.clientDashboardMasterToolStripMenuItem.Name = "clientDashboardMasterToolStripMenuItem";
            this.clientDashboardMasterToolStripMenuItem.Size = new System.Drawing.Size(332, 22);
            this.clientDashboardMasterToolStripMenuItem.Text = "Client Dashboard Master";
            this.clientDashboardMasterToolStripMenuItem.Click += new System.EventHandler(this.clientDashboardMasterToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(329, 6);
            // 
            // changePasswordToolStripMenuItem
            // 
            this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            this.changePasswordToolStripMenuItem.Size = new System.Drawing.Size(332, 22);
            this.changePasswordToolStripMenuItem.Text = "Change Password";
            this.changePasswordToolStripMenuItem.Click += new System.EventHandler(this.changePasswordToolStripMenuItem_Click);
            // 
            // clientMasterToolStripMenuItem1
            // 
            this.clientMasterToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientLoginToolStripMenuItem});
            this.clientMasterToolStripMenuItem1.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.clientMasterToolStripMenuItem1.Name = "clientMasterToolStripMenuItem1";
            this.clientMasterToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.clientMasterToolStripMenuItem1.Text = "Client Master";
            // 
            // clientLoginToolStripMenuItem
            // 
            this.clientLoginToolStripMenuItem.Name = "clientLoginToolStripMenuItem";
            this.clientLoginToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.clientLoginToolStripMenuItem.Text = "Client Login";
            this.clientLoginToolStripMenuItem.Click += new System.EventHandler(this.clientLoginToolStripMenuItem_Click);
            // 
            // frmMDI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(676, 453);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MDIMenu);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.MDIMenu;
            this.Name = "frmMDI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Expert Mobile";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMDI_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMDI_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMDI_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.MDIMenu.ResumeLayout(false);
            this.MDIMenu.PerformLayout();
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
        private System.Windows.Forms.MenuStrip MDIMenu;
        private System.Windows.Forms.ToolStripMenuItem masterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuMasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientMasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem clientMenuAllocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem clientDashboardMasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem clientMasterToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem clientLoginToolStripMenuItem;
    }
}

