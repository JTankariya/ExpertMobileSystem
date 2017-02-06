using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ExpertMobileOrderSystem
{
    public partial class frmChangePassword : Form
    {
        bool blEventExit = false;
        bool blBlue = true;
        public frmChangePassword()
        {
            InitializeComponent();
            Load += frmChagePassword_Load;
            txtUserName.Text = Operation.ClientUserName;
            lblUserId.Text = Operation.Clientid;
            try
            { this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\MOBILE.ico"); }
            catch { }
        }
        private void SetEventHandlers(Control ctrlContainer)
        {
            if (ctrlContainer.Text.ToString().ToUpper() == "FRMTURBO")
                return;
            foreach (Control ctrl in ctrlContainer.Controls)
            {
                if (ctrl is TextBox)
                {
                    ctrl.Enter += ProcessEnter;
                    ctrl.Leave += ProcessLeave;
                    ctrl.DoubleClick += ProcessClick;
                }
                if (ctrl is DateTimePicker)
                {
                    ctrl.Enter += Datetnter;
                }
                if (ctrl is CheckBox)
                {
                    ctrl.Enter += CheckBoxEnter;

                }
                if (ctrl is ComboBox)
                {
                    ctrl.Enter += ComboBoxEnter;
                }
                if (ctrl is Button)
                {
                    ctrl.Enter += ButtonEnter;
                }
                else if (ctrl is DataGridView)
                {
                    clearstatuslabel();
                    ctrl.BindingContextChanged += ProcessBindingContextChanged;
                    ctrl.Enter += Gridenter;
                }
                if (ctrl.HasChildren)
                {
                    SetEventHandlers(ctrl);
                }
            }
        }
        private void ProcessBindingContextChanged(object sender, System.EventArgs e)
        {
            ((DataGridView)sender).BackgroundColor = System.Drawing.Color.White;
            ((DataGridView)sender).BackColor = Color.White;
            ControlMessage.Text = ((DataGridView)sender).Tag.ToString();

        }
        private void ProcessClick(object sender, System.EventArgs e)
        {
            if (this.blEventExit)
                return;
            if (blBlue == false)
                return;
            ((TextBox)sender).BackColor = System.Drawing.Color.FromArgb(Convert.ToByte(214), Convert.ToByte(223), Convert.ToByte(245));
        }
        private void ProcessLeave(object sender, System.EventArgs e)
        {
            if (this.blEventExit)
                return;
            if (blBlue == false)
                return;
            ((TextBox)sender).BackColor = Color.White;


        }
        private void ProcessEnter(object sender, System.EventArgs e)
        {
            if (this.blEventExit)
                return;
            if (blBlue == false)
                return;
            ((TextBox)sender).BackColor = System.Drawing.Color.FromArgb(Convert.ToByte(214), Convert.ToByte(223), Convert.ToByte(245));
            clearstatuslabel();
            ControlMessage.Text = ((TextBox)sender).Tag.ToString();
            ControlLabel.Text = "Esc : Exit";
        }
        private void Datetnter(object sender, System.EventArgs e)
        {
            clearstatuslabel();
            ControlMessage.Text = ((DateTimePicker)sender).Tag.ToString();
            ControlLabel.Text = "Esc : Exit";
        }

        private void clearstatuslabel()
        {
            ControlMessage.Text = "";

        }

        private void Gridenter(object sender, System.EventArgs e)
        {
            clearstatuslabel();
            ControlMessage.Text = ((DataGridView)sender).Tag.ToString();
            ControlLabel.Text = "Esc : Exit";
        }
        private void CheckBoxEnter(object sender, System.EventArgs e)
        {
            clearstatuslabel();
            ControlMessage.Text = ((CheckBox)sender).Tag.ToString();
            ControlLabel.Text = "Esc : Exit";
        }
        private void ButtonEnter(object sender, System.EventArgs e)
        {
            clearstatuslabel();
            ControlMessage.Text = ((Button)sender).Tag.ToString();
            ControlLabel.Text = "Esc : Exit";
        }
        private void ComboBoxEnter(object sender, System.EventArgs e)
        {
            clearstatuslabel();
            ControlMessage.Text = ((ComboBox)sender).Tag.ToString();
            ControlLabel.Text = "Esc : Exit";
        }


        public frmChangePassword(int UserId, string UserName)
        {
            InitializeComponent();
            lblUserId.Text = UserId.ToString();
            txtUserName.Text = UserName;
            if ((!string.IsNullOrEmpty(txtUserName.Text)))
            {
                txtUserName.ReadOnly = true;
            }
            else
            {
                txtUserName.ReadOnly = false;
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (validation())
                {
                    int result;
                    result = Operation.ExecuteNonQuery("update [Order.ClientMaster] set password='" + Operation.Encryptdata(txtNew.Text) + "' where UserName='" + txtUserName.Text + "'", Operation.Conn);
                    if (result == 1)
                    {
                        MessageBox.Show("Password changed successfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtConfirm.Text = "";
                        txtNew.Text = "";
                        txtOld.Text = "";
                        lblUserId.Text = "0";
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error while changing password, Please try after some time." + Environment.NewLine + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblUserId.Text = "0";
            }
        }

        private bool validation()
        {
            if (txtUserName.Text == "")
            {
                MessageBox.Show("Please enter User name.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtUserName.Focus();
                return false;
            }
            if (txtOld.Text == "")
            {
                MessageBox.Show("Please enter old password.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtOld.Focus();
                return false;
            }
            if (txtNew.Text == "")
            {
                MessageBox.Show("Please enter new password.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtNew.Focus();
                return false;
            }
            if (txtConfirm.Text == "")
            {
                MessageBox.Show("Please enter confirm password.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtConfirm.Focus();
                return false;
            }
            if (Operation.Encryptdata(txtOld.Text) != Operation.objComp.Password)
            {
                MessageBox.Show("Old password doesn't match with system, Please enter correct old password.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtConfirm.Focus();
                return false;
            }
            if (txtNew.Text != txtConfirm.Text)
            {
                MessageBox.Show("Your new password and confirm password are not mathcing, Please try again.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtConfirm.Focus();
                return false;
            }
            return true;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmChagePassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.Control && e.KeyCode == Keys.S)
                btnUpdate_Click(sender, e);
        }

        private void frmChagePassword_Load(object sender, EventArgs e)
        {
            SetEventHandlers(groupBox1);
            Paint += draw;
            Invalidate();
        }

        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == (char)8))
                e.Handled = true;
        }
        public void draw(object sender, PaintEventArgs e)
        {
            try
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, Operation.strFirstColor, Operation.strSecondColor, LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }
            catch { }
        }
    }
}
