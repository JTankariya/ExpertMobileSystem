namespace ExpertMobileOrderSystem
{
    partial class frmUserMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserMaster));
            this.txtm2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtabc = new System.Windows.Forms.TextBox();
            this.Label63 = new System.Windows.Forms.Label();
            this.lblid = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.grpInfo = new System.Windows.Forms.GroupBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtmobile = new System.Windows.Forms.TextBox();
            this.Label54 = new System.Windows.Forms.Label();
            this.dtpdate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.txtrepass = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lnlpass = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFirstname = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLastname = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkviewPass = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ControlMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.ControlLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.grpInfo.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtm2
            // 
            this.txtm2.BackColor = System.Drawing.Color.White;
            this.txtm2.Location = new System.Drawing.Point(79, 268);
            this.txtm2.MaxLength = 15;
            this.txtm2.Name = "txtm2";
            this.txtm2.Size = new System.Drawing.Size(23, 23);
            this.txtm2.TabIndex = 3;
            this.txtm2.Tag = "Enter Mobile No.2";
            this.txtm2.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 271);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 16);
            this.label2.TabIndex = 41;
            this.label2.Text = "Mobile:";
            this.label2.Visible = false;
            // 
            // txtabc
            // 
            this.txtabc.BackColor = System.Drawing.Color.White;
            this.txtabc.Location = new System.Drawing.Point(73, 229);
            this.txtabc.MaxLength = 15;
            this.txtabc.Name = "txtabc";
            this.txtabc.Size = new System.Drawing.Size(64, 23);
            this.txtabc.TabIndex = 1;
            this.txtabc.Tag = "Enter Phone No.2";
            this.txtabc.Visible = false;
            this.txtabc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtphone2_KeyPress);
            // 
            // Label63
            // 
            this.Label63.AutoSize = true;
            this.Label63.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label63.Location = new System.Drawing.Point(3, 236);
            this.Label63.Name = "Label63";
            this.Label63.Size = new System.Drawing.Size(62, 16);
            this.Label63.TabIndex = 37;
            this.Label63.Text = "Phone2:";
            this.Label63.Visible = false;
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
            // btnSave
            // 
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = global::ExpertMobileOrderSystem.Properties.Resources.accept;
            this.btnSave.Location = new System.Drawing.Point(243, 156);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(71, 60);
            this.btnSave.TabIndex = 1;
            this.btnSave.Tag = "Save";
            this.btnSave.Text = "&Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnSave, "Save Changes.");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnView
            // 
            this.btnView.FlatAppearance.BorderSize = 0;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView.Image = global::ExpertMobileOrderSystem.Properties.Resources.search2;
            this.btnView.Location = new System.Drawing.Point(450, 156);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(71, 60);
            this.btnView.TabIndex = 3;
            this.btnView.Tag = "View";
            this.btnView.Text = "&View";
            this.btnView.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(146, 157);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(71, 58);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Tag = "Add";
            this.btnAdd.Text = "&Add";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Image = global::ExpertMobileOrderSystem.Properties.Resources.close1;
            this.btnDelete.Location = new System.Drawing.Point(346, 156);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(71, 60);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Tag = "Delete";
            this.btnDelete.Text = "&Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // grpInfo
            // 
            this.grpInfo.BackColor = System.Drawing.Color.Transparent;
            this.grpInfo.Controls.Add(this.txtUserName);
            this.grpInfo.Controls.Add(this.label5);
            this.grpInfo.Controls.Add(this.txtmobile);
            this.grpInfo.Controls.Add(this.Label54);
            this.grpInfo.Controls.Add(this.dtpdate);
            this.grpInfo.Controls.Add(this.label7);
            this.grpInfo.Controls.Add(this.txtrepass);
            this.grpInfo.Controls.Add(this.txtPassword);
            this.grpInfo.Controls.Add(this.lnlpass);
            this.grpInfo.Controls.Add(this.label1);
            this.grpInfo.Controls.Add(this.txtFirstname);
            this.grpInfo.Controls.Add(this.label3);
            this.grpInfo.Controls.Add(this.txtLastname);
            this.grpInfo.Controls.Add(this.label4);
            this.grpInfo.Controls.Add(this.chkviewPass);
            this.grpInfo.Location = new System.Drawing.Point(12, 8);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Size = new System.Drawing.Size(644, 142);
            this.grpInfo.TabIndex = 0;
            this.grpInfo.TabStop = false;
            this.grpInfo.Text = "Employee Information";
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.Color.White;
            this.txtUserName.Location = new System.Drawing.Point(416, 22);
            this.txtUserName.MaxLength = 15;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(218, 23);
            this.txtUserName.TabIndex = 3;
            this.txtUserName.Tag = "Enter Mobile No.1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(329, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 16);
            this.label5.TabIndex = 78;
            this.label5.Text = "User Name :";
            // 
            // txtmobile
            // 
            this.txtmobile.BackColor = System.Drawing.Color.White;
            this.txtmobile.Location = new System.Drawing.Point(105, 82);
            this.txtmobile.MaxLength = 15;
            this.txtmobile.Name = "txtmobile";
            this.txtmobile.Size = new System.Drawing.Size(218, 23);
            this.txtmobile.TabIndex = 2;
            this.txtmobile.Tag = "Enter Mobile No.1";
            // 
            // Label54
            // 
            this.Label54.AutoSize = true;
            this.Label54.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label54.Location = new System.Drawing.Point(45, 83);
            this.Label54.Name = "Label54";
            this.Label54.Size = new System.Drawing.Size(55, 16);
            this.Label54.TabIndex = 76;
            this.Label54.Text = "Mobile:";
            // 
            // dtpdate
            // 
            this.dtpdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpdate.Location = new System.Drawing.Point(105, 111);
            this.dtpdate.Name = "dtpdate";
            this.dtpdate.Size = new System.Drawing.Size(218, 23);
            this.dtpdate.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(10, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 16);
            this.label7.TabIndex = 71;
            this.label7.Text = "Expiry Date:";
            // 
            // txtrepass
            // 
            this.txtrepass.BackColor = System.Drawing.Color.White;
            this.txtrepass.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtrepass.Location = new System.Drawing.Point(416, 80);
            this.txtrepass.MaxLength = 100;
            this.txtrepass.Name = "txtrepass";
            this.txtrepass.PasswordChar = '*';
            this.txtrepass.Size = new System.Drawing.Size(218, 23);
            this.txtrepass.TabIndex = 5;
            this.txtrepass.Tag = "Enter Email Id.";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtPassword.Location = new System.Drawing.Point(416, 51);
            this.txtPassword.MaxLength = 100;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(218, 23);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.Tag = "Enter Email Id.";
            // 
            // lnlpass
            // 
            this.lnlpass.AutoSize = true;
            this.lnlpass.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnlpass.Location = new System.Drawing.Point(329, 54);
            this.lnlpass.Name = "lnlpass";
            this.lnlpass.Size = new System.Drawing.Size(76, 16);
            this.lnlpass.TabIndex = 72;
            this.lnlpass.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(329, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 16);
            this.label1.TabIndex = 73;
            this.label1.Text = "Re-Enter :";
            // 
            // txtFirstname
            // 
            this.txtFirstname.BackColor = System.Drawing.Color.White;
            this.txtFirstname.Location = new System.Drawing.Point(105, 22);
            this.txtFirstname.MaxLength = 250;
            this.txtFirstname.Name = "txtFirstname";
            this.txtFirstname.Size = new System.Drawing.Size(218, 23);
            this.txtFirstname.TabIndex = 0;
            this.txtFirstname.Tag = "Enter Party Name.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 16);
            this.label3.TabIndex = 74;
            this.label3.Text = "First Name:";
            // 
            // txtLastname
            // 
            this.txtLastname.BackColor = System.Drawing.Color.White;
            this.txtLastname.Location = new System.Drawing.Point(105, 53);
            this.txtLastname.MaxLength = 250;
            this.txtLastname.Name = "txtLastname";
            this.txtLastname.Size = new System.Drawing.Size(218, 23);
            this.txtLastname.TabIndex = 1;
            this.txtLastname.Tag = "Enter Party Name.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(17, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 16);
            this.label4.TabIndex = 75;
            this.label4.Text = "Last Name:";
            // 
            // chkviewPass
            // 
            this.chkviewPass.AutoSize = true;
            this.chkviewPass.Location = new System.Drawing.Point(416, 113);
            this.chkviewPass.Name = "chkviewPass";
            this.chkviewPass.Size = new System.Drawing.Size(125, 20);
            this.chkviewPass.TabIndex = 7;
            this.chkviewPass.Text = "View Password";
            this.chkviewPass.UseVisualStyleBackColor = true;
            this.chkviewPass.CheckedChanged += new System.EventHandler(this.chkviewPass_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.ControlMessage,
            this.ControlLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 228);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(666, 22);
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
            this.ControlMessage.Size = new System.Drawing.Size(592, 17);
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
            // frmUserMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(666, 250);
            this.Controls.Add(this.txtm2);
            this.Controls.Add(this.grpInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtabc);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Label63);
            this.Controls.Add(this.lblid);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDelete);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmUserMaster";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Employee Master";
            this.Load += new System.EventHandler(this.frmPartymaster_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPartymaster_KeyDown);
            this.grpInfo.ResumeLayout(false);
            this.grpInfo.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtabc;
        internal System.Windows.Forms.Label Label63;
        public System.Windows.Forms.Label lblid;
        private System.Windows.Forms.ToolTip toolTip1;
        internal System.Windows.Forms.Button btnView;
        internal System.Windows.Forms.Button btnAdd;
        internal System.Windows.Forms.Button btnDelete;
        internal System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.TextBox txtm2;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpInfo;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel ControlMessage;
        private System.Windows.Forms.ToolStripStatusLabel ControlLabel;
        private System.Windows.Forms.ToolTip toolTip2;
        public System.Windows.Forms.TextBox txtUserName;
        internal System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtmobile;
        internal System.Windows.Forms.Label Label54;
        private System.Windows.Forms.DateTimePicker dtpdate;
        internal System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox txtrepass;
        public System.Windows.Forms.TextBox txtPassword;
        internal System.Windows.Forms.Label lnlpass;
        internal System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtFirstname;
        internal System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtLastname;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkviewPass;
       // public System.Windows.Forms.DateTimePicker dtpdate;
        //this.dtpdate.CustomFormat = "dd/MM/yyyy";
    }
}