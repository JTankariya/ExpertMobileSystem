namespace ExpertMobileSystem
{
    partial class frmAdminMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdminMaster));
            this.lblid = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ControlMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.ControlLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.txtAdminname = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.lblPArtyNAme = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lnlpass = new System.Windows.Forms.Label();
            this.txtrepass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSuperAdmin = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMobile = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.statusStrip1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblid
            // 
            this.lblid.AutoSize = true;
            this.lblid.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblid.Location = new System.Drawing.Point(82, 803);
            this.lblid.Name = "lblid";
            this.lblid.Size = new System.Drawing.Size(15, 17);
            this.lblid.TabIndex = 54;
            this.lblid.Text = "0";
            this.lblid.Visible = false;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Information";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.ControlMessage,
            this.ControlLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 235);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(446, 22);
            this.statusStrip1.TabIndex = 90;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel2.Text = "Message :";
            // 
            // ControlMessage
            // 
            this.ControlMessage.Name = "ControlMessage";
            this.ControlMessage.Size = new System.Drawing.Size(372, 17);
            this.ControlMessage.Spring = true;
            this.ControlMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ControlLabel
            // 
            this.ControlLabel.Name = "ControlLabel";
            this.ControlLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // toolTip2
            // 
            this.toolTip2.AutoPopDelay = 5000;
            this.toolTip2.InitialDelay = 100;
            this.toolTip2.IsBalloon = true;
            this.toolTip2.ReshowDelay = 100;
            this.toolTip2.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip2.ToolTipTitle = "Information";
            // 
            // txtAdminname
            // 
            this.txtAdminname.BackColor = System.Drawing.Color.White;
            this.txtAdminname.Location = new System.Drawing.Point(162, 17);
            this.txtAdminname.MaxLength = 250;
            this.txtAdminname.Name = "txtAdminname";
            this.txtAdminname.Size = new System.Drawing.Size(218, 23);
            this.txtAdminname.TabIndex = 0;
            this.txtAdminname.Tag = "Enter Party Name.";
            this.txtAdminname.Validated += new System.EventHandler(this.txtAdminname_Validated);
            // 
            // btnSave
            // 
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = global::ExpertMobileSystem.Properties.Resources.accept;
            this.btnSave.Location = new System.Drawing.Point(142, 169);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(71, 60);
            this.btnSave.TabIndex = 5;
            this.btnSave.Tag = "Save";
            this.btnSave.Text = "&Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Image = global::ExpertMobileSystem.Properties.Resources.close;
            this.btnDelete.Location = new System.Drawing.Point(213, 171);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(71, 60);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Tag = "Delete";
            this.btnDelete.Text = "&Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(65, 171);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(71, 58);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Tag = "Add";
            this.btnAdd.Text = "&Add";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnView
            // 
            this.btnView.FlatAppearance.BorderSize = 0;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView.Image = global::ExpertMobileSystem.Properties.Resources.search2;
            this.btnView.Location = new System.Drawing.Point(290, 172);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(71, 60);
            this.btnView.TabIndex = 8;
            this.btnView.Tag = "View";
            this.btnView.Text = "&View";
            this.btnView.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblPArtyNAme
            // 
            this.lblPArtyNAme.AutoSize = true;
            this.lblPArtyNAme.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPArtyNAme.Location = new System.Drawing.Point(54, 20);
            this.lblPArtyNAme.Name = "lblPArtyNAme";
            this.lblPArtyNAme.Size = new System.Drawing.Size(94, 16);
            this.lblPArtyNAme.TabIndex = 42;
            this.lblPArtyNAme.Text = "Admin Name:";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtPassword.Location = new System.Drawing.Point(162, 48);
            this.txtPassword.MaxLength = 100;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(218, 23);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Tag = "Enter Email Id.";
            // 
            // lnlpass
            // 
            this.lnlpass.AutoSize = true;
            this.lnlpass.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnlpass.Location = new System.Drawing.Point(72, 51);
            this.lnlpass.Name = "lnlpass";
            this.lnlpass.Size = new System.Drawing.Size(76, 16);
            this.lnlpass.TabIndex = 44;
            this.lnlpass.Text = "Password:";
            // 
            // txtrepass
            // 
            this.txtrepass.BackColor = System.Drawing.Color.White;
            this.txtrepass.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtrepass.Location = new System.Drawing.Point(162, 82);
            this.txtrepass.MaxLength = 100;
            this.txtrepass.Name = "txtrepass";
            this.txtrepass.PasswordChar = '*';
            this.txtrepass.Size = new System.Drawing.Size(218, 23);
            this.txtrepass.TabIndex = 2;
            this.txtrepass.Tag = "Enter Email Id.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 16);
            this.label1.TabIndex = 46;
            this.label1.Text = "Re-Enter Password:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // chkSuperAdmin
            // 
            this.chkSuperAdmin.AutoSize = true;
            this.chkSuperAdmin.Location = new System.Drawing.Point(263, 147);
            this.chkSuperAdmin.Name = "chkSuperAdmin";
            this.chkSuperAdmin.Size = new System.Drawing.Size(114, 20);
            this.chkSuperAdmin.TabIndex = 4;
            this.chkSuperAdmin.Text = "isSuperAdmin";
            this.chkSuperAdmin.UseVisualStyleBackColor = true;
            this.chkSuperAdmin.CheckedChanged += new System.EventHandler(this.chkSuperAdmin_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(90, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 16);
            this.label3.TabIndex = 41;
            this.label3.Text = "Mobile:";
            // 
            // txtMobile
            // 
            this.txtMobile.BackColor = System.Drawing.Color.White;
            this.txtMobile.Location = new System.Drawing.Point(161, 119);
            this.txtMobile.MaxLength = 15;
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new System.Drawing.Size(219, 23);
            this.txtMobile.TabIndex = 3;
            this.txtMobile.Tag = "Enter Mobile No.1";
            this.txtMobile.Validated += new System.EventHandler(this.txtMobile_Validated);
            this.txtMobile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtphone1_KeyPress);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtMobile);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.chkSuperAdmin);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.txtrepass);
            this.groupBox5.Controls.Add(this.lnlpass);
            this.groupBox5.Controls.Add(this.txtPassword);
            this.groupBox5.Controls.Add(this.lblPArtyNAme);
            this.groupBox5.Controls.Add(this.btnView);
            this.groupBox5.Controls.Add(this.btnAdd);
            this.groupBox5.Controls.Add(this.btnDelete);
            this.groupBox5.Controls.Add(this.btnSave);
            this.groupBox5.Controls.Add(this.txtAdminname);
            this.groupBox5.Location = new System.Drawing.Point(0, -5);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(446, 237);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Enter += new System.EventHandler(this.groupBox5_Enter);
            // 
            // frmAdminMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(446, 257);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblid);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAdminMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Master";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPartymaster_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPartymaster_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblid;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel ControlMessage;
        private System.Windows.Forms.ToolStripStatusLabel ControlLabel;
        private System.Windows.Forms.ToolTip toolTip2;
        public System.Windows.Forms.TextBox txtAdminname;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnDelete;
        internal System.Windows.Forms.Button btnAdd;
        internal System.Windows.Forms.Button btnView;
        internal System.Windows.Forms.Label lblPArtyNAme;
        public System.Windows.Forms.TextBox txtPassword;
        internal System.Windows.Forms.Label lnlpass;
        public System.Windows.Forms.TextBox txtrepass;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSuperAdmin;
        internal System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtMobile;
        private System.Windows.Forms.GroupBox groupBox5;
    }
}