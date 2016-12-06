namespace ExpertMobileSystem_Client_
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
            this.txtmobile = new System.Windows.Forms.TextBox();
            this.Label54 = new System.Windows.Forms.Label();
            this.chkviewPass = new System.Windows.Forms.CheckBox();
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ControlMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.ControlLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.grpInfo.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtm2
            // 
            this.txtm2.BackColor = System.Drawing.Color.White;
            this.txtm2.Location = new System.Drawing.Point(97, 271);
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
            this.label2.Location = new System.Drawing.Point(28, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 16);
            this.label2.TabIndex = 41;
            this.label2.Text = "Mobile:";
            this.label2.Visible = false;
            // 
            // txtabc
            // 
            this.txtabc.BackColor = System.Drawing.Color.White;
            this.txtabc.Location = new System.Drawing.Point(91, 232);
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
            this.Label63.Location = new System.Drawing.Point(21, 239);
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
            this.btnSave.Image = global::ExpertMobileSystem_Client_.Properties.Resources.accept;
            this.btnSave.Location = new System.Drawing.Point(182, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(71, 60);
            this.btnSave.TabIndex = 0;
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
            this.btnView.Image = global::ExpertMobileSystem_Client_.Properties.Resources.search2;
            this.btnView.Location = new System.Drawing.Point(389, 230);
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
            this.btnAdd.Location = new System.Drawing.Point(85, 232);
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
            this.btnDelete.Image = global::ExpertMobileSystem_Client_.Properties.Resources.close11;
            this.btnDelete.Location = new System.Drawing.Point(285, 230);
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
            this.grpInfo.Controls.Add(this.txtmobile);
            this.grpInfo.Controls.Add(this.Label54);
            this.grpInfo.Controls.Add(this.chkviewPass);
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
            this.grpInfo.Location = new System.Drawing.Point(0, 12);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Size = new System.Drawing.Size(556, 213);
            this.grpInfo.TabIndex = 7;
            this.grpInfo.TabStop = false;
            this.grpInfo.Text = "User Information";
            // 
            // txtmobile
            // 
            this.txtmobile.BackColor = System.Drawing.Color.White;
            this.txtmobile.Location = new System.Drawing.Point(168, 87);
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
            this.Label54.Location = new System.Drawing.Point(108, 88);
            this.Label54.Name = "Label54";
            this.Label54.Size = new System.Drawing.Size(55, 16);
            this.Label54.TabIndex = 62;
            this.Label54.Text = "Mobile:";
            // 
            // chkviewPass
            // 
            this.chkviewPass.AutoSize = true;
            this.chkviewPass.Location = new System.Drawing.Point(393, 116);
            this.chkviewPass.Name = "chkviewPass";
            this.chkviewPass.Size = new System.Drawing.Size(125, 20);
            this.chkviewPass.TabIndex = 6;
            this.chkviewPass.Text = "View Password";
            this.chkviewPass.UseVisualStyleBackColor = true;
            this.chkviewPass.CheckedChanged += new System.EventHandler(this.chkviewPass_CheckedChanged);
            // 
            // dtpdate
            // 
            this.dtpdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpdate.Location = new System.Drawing.Point(168, 177);
            this.dtpdate.Name = "dtpdate";
            this.dtpdate.Size = new System.Drawing.Size(218, 23);
            this.dtpdate.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(73, 180);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 16);
            this.label7.TabIndex = 49;
            this.label7.Text = "Expiry Date:";
            // 
            // txtrepass
            // 
            this.txtrepass.BackColor = System.Drawing.Color.White;
            this.txtrepass.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtrepass.Location = new System.Drawing.Point(168, 145);
            this.txtrepass.MaxLength = 100;
            this.txtrepass.Name = "txtrepass";
            this.txtrepass.PasswordChar = '*';
            this.txtrepass.Size = new System.Drawing.Size(218, 23);
            this.txtrepass.TabIndex = 4;
            this.txtrepass.Tag = "Enter Email Id.";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtPassword.Location = new System.Drawing.Point(168, 116);
            this.txtPassword.MaxLength = 100;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(218, 23);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.Tag = "Enter Email Id.";
            // 
            // lnlpass
            // 
            this.lnlpass.AutoSize = true;
            this.lnlpass.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnlpass.Location = new System.Drawing.Point(88, 119);
            this.lnlpass.Name = "lnlpass";
            this.lnlpass.Size = new System.Drawing.Size(76, 16);
            this.lnlpass.TabIndex = 51;
            this.lnlpass.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 16);
            this.label1.TabIndex = 52;
            this.label1.Text = "Re-Enter Password:";
            // 
            // txtFirstname
            // 
            this.txtFirstname.BackColor = System.Drawing.Color.White;
            this.txtFirstname.Location = new System.Drawing.Point(168, 22);
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
            this.label3.Location = new System.Drawing.Point(80, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 16);
            this.label3.TabIndex = 54;
            this.label3.Text = "First Name:";
            // 
            // txtLastname
            // 
            this.txtLastname.BackColor = System.Drawing.Color.White;
            this.txtLastname.Location = new System.Drawing.Point(170, 53);
            this.txtLastname.MaxLength = 250;
            this.txtLastname.Name = "txtLastname";
            this.txtLastname.Size = new System.Drawing.Size(216, 23);
            this.txtLastname.TabIndex = 1;
            this.txtLastname.Tag = "Enter Party Name.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(80, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 16);
            this.label4.TabIndex = 56;
            this.label4.Text = "Last Name:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.ControlMessage,
            this.ControlLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 300);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(551, 22);
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
            this.ControlMessage.Size = new System.Drawing.Size(477, 17);
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
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.txtm2);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.txtabc);
            this.groupBox5.Controls.Add(this.Label63);
            this.groupBox5.Controls.Add(this.btnView);
            this.groupBox5.Controls.Add(this.btnAdd);
            this.groupBox5.Controls.Add(this.btnDelete);
            this.groupBox5.Controls.Add(this.btnSave);
            this.groupBox5.Location = new System.Drawing.Point(-2, -4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(551, 308);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            // 
            // frmUserMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(551, 322);
            this.Controls.Add(this.grpInfo);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblid);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmUserMaster";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Master";
            this.Load += new System.EventHandler(this.frmPartymaster_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPartymaster_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPartymaster_KeyDown);
            this.grpInfo.ResumeLayout(false);
            this.grpInfo.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox5;
        internal System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtrepass;
        internal System.Windows.Forms.Label lnlpass;
        public System.Windows.Forms.TextBox txtPassword;
        internal System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtLastname;
        internal System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtFirstname;
       // public System.Windows.Forms.DateTimePicker dtpdate;
        //this.dtpdate.CustomFormat = "dd/MM/yyyy";
        internal System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpdate;
        private System.Windows.Forms.CheckBox chkviewPass;
        public System.Windows.Forms.TextBox txtmobile;
        internal System.Windows.Forms.Label Label54;
    }
}