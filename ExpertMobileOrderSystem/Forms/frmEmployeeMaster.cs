﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Collections;

using System.Drawing.Printing;
using System.Threading;
using System.Drawing.Drawing2D;

namespace ExpertMobileOrderSystem
{
    public partial class frmUserMaster : Form
    {
        public string filename = Guid.NewGuid().ToString();
        bool Loaded = false;

        public frmUserMaster()
        {
            InitializeComponent();
            Load += frmPartymaster_Load;
            Paint += draw;
            Invalidate();
            try
            { this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\MOBILE.ico"); }
            catch { }
        }
        private void draw(object sender, PaintEventArgs e)
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


        private void txtpostalcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void txtphone1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void txtphone2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void txtmobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)8))
                e.Handled = true;
        }


        private void frmPartymaster_Load(object sender, EventArgs e)
        {
            if (Loaded == false)
            {
                btnAdd_Click(sender, e);
                Loaded = true;
            }
        }

        private void frmPartymaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendKeys.Send("{Tab}");
            }

            if ((e.KeyCode == Keys.Escape))
            {
                if (MessageBox.Show("Are you sure to Exit?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Close();
                    return;
                }
            }
        }
        private bool Validate_form()
        {
            if (txtFirstname.Text == "")
            {
                MessageBox.Show("Please Enter First Name.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFirstname.Focus();
                return false;
            }
            if (txtLastname.Text == "")
            {
                MessageBox.Show("Please Enter Last Name.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLastname.Focus();
                return false;
            }
            if (txtmobile.Text == "")
            {
                MessageBox.Show("Please Enter Mobile.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmobile.Focus();
                return false;
            }
            object pass = Operation.ExecuteScalar("Select * from ClientUserMaster where MobileNo = '" + txtmobile.Text + "'", Operation.Conn);
            if ((lblid.Text.ToString() != (pass != null ? pass.ToString() : "")) && (pass != null))
            {
                MessageBox.Show("Mobile Number '" + txtmobile.Text + "' Already Registered.Please Enter Another One.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmobile.Focus();
                return false;
            }
            if (txtPassword.Text == "")
            {
                MessageBox.Show("Please Enter Password.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return false;
            }
            if (txtrepass.Text == "")
            {
                MessageBox.Show("Please Enter Re-Enter Password.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtrepass.Focus();
                return false;
            }

            if (dtpdate.Value < DateTime.Today || dtpdate.Value > Operation.currClient.AccountExpiredOn)
            {
                MessageBox.Show("Expiry date should be Greater Than Today and Less than :" + Operation.currClient.AccountExpiredOn.ToShortDateString(), Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpdate.Value = Operation.currClient.AccountExpiredOn;
                dtpdate.Focus();
                return false;

            }
            var qu = "select * from [Order.ClientUserMaster] where UserName='" + txtUserName.Text.Trim() + "'";
            if (lblid.Text != "0")
            {
                qu += " and Id<>" + lblid.Text;
            }
            DataTable dtUsers = Operation.GetDataTable(qu, Operation.Conn);
            if (dtUsers != null && dtUsers.Rows.Count > 0)
            {
                MessageBox.Show("Username is already exist, Please enter another username.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserName.Focus();
                return false;
            }
            return true;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            lblid.Text = "0";
            txtPassword.Text = "";
            txtrepass.Text = "";
            txtFirstname.Text = "";
            txtLastname.Text = "";
            txtmobile.Text = "";
            dtpdate.Value = Operation.currClient.AccountExpiredOn;
            txtmobile.Text = "";
            btnDelete.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Validate_form())
                return;
            btnSave.Enabled = false;
            try
            {
                ArrayList Queries = new ArrayList();
                if (lblid.Text == "0")
                {
                    Queries.Add("Insert into [Order.ClientUserMaster] ( Clientid ,Password,MobileNo,FirstName,LastName,AccountExpiredOn,CreatedDate,UserTypeId,UserName ) " +
                        "values(" + Operation.currClient.Id + ",'" + Operation.Encryptdata(txtPassword.Text) + "','" + txtmobile.Text + "','" + txtFirstname.Text + "','" + txtLastname.Text + "','" + dtpdate.Value.ToString("yyyy-MM-dd") + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "'," + UserTypes.DISTRIBUTOR + ",'" + txtUserName.Text + "')");
                    Queries.Add("update [Order.ClientMaster] set TotalCreatedUser = " + (Operation.currClient.TotalCreatedUser + 1) + " where ID = " + Operation.currClient.Id);
                }
                else
                {
                    Queries.Add("update [Order.ClientUserMaster] set Password='" + Operation.Encryptdata(txtPassword.Text) + "',MobileNo= '" + txtmobile.Text + "',FirstName='" + txtFirstname.Text.Trim() + "',LastName='" + txtLastname.Text.Trim() + "'" +
                                ",AccountExpiredOn='" + dtpdate.Value.ToString("yyyy-MM-dd") + "',UserTypeId=" + UserTypes.DISTRIBUTOR + ",UserName='" + txtUserName.Text + "' where ClientID = " + lblid.Text + " ");
                }
                if (Operation.ExecuteTransaction(Queries, Operation.Conn))
                {
                    MessageBox.Show("Record Saved Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Operation.currClient.Refresh();
                    btnAdd_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Error while Saving, Please Try after Some Time.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = true;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
            }
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            fillgrid();
        }
        private void fillgrid()
        {
            frmSearch view = new frmSearch();
            Operation.gViewQuery = "select Clientid,CreatedDate,FirstName, LastName, MobileNo, AccountExpiredOn from [Order.ClientUserMaster] where ClientId = " + Operation.currClient.Id + " and PartyCode is null";
            Operation.Bindgrid(Operation.gViewQuery, view.dgvSearch);
            view.dgvSearch.Columns[0].Visible = false;
            view.OrderByColoumn = "CreatedDate";
            view.ShowDialog();

            view.dgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            if (Operation.ViewID != null && Operation.ViewID != string.Empty)
            {
                filldata();
                Operation.ViewID = "";
            }
        }
        public void filldata()
        {
            filldata(Operation.GetDataTable("Select * from [Order.ClientUserMaster] where ClientId=" + Operation.ViewID.ToString(), Operation.Conn));
        }
        private void filldata(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    lblid.Text = dt.Rows[0]["Id"].ToString();
                    txtmobile.Text = dt.Rows[0]["MobileNo"].ToString();
                    txtFirstname.Text = dt.Rows[0]["FirstName"].ToString();
                    txtUserName.Text = dt.Rows[0]["UserName"].ToString();
                    txtLastname.Text = dt.Rows[0]["LastName"].ToString();
                    dtpdate.Value = Convert.ToDateTime(dt.Rows[0]["AccountExpiredOn"]);
                    txtPassword.Text = Operation.Decryptdata(dt.Rows[0]["Password"].ToString());
                    txtrepass.Text = txtPassword.Text;
                    btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lblid.Text.Trim() == "0")
            {
                MessageBox.Show("Please Select Record first.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else if (!Operation.CheckReference(Convert.ToInt32(lblid.Text), "[Order.ClientUserCompanyMaster],ClientID"))
                return;
            else if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = null;
                query = "Delete from [Order.ClientUserMaster] where ClientID= " + lblid.Text.Trim();
                Operation.ExecuteNonQuery(query, Operation.Conn);
                Operation.ExecuteNonQuery("update [Order.ClientMaster] set TotalCreatedUser = " + (Operation.currClient.TotalCreatedUser - 1) + " where ID = " + Operation.currClient.Id, Operation.Conn);
                MessageBox.Show("Record Deleted Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblid.Text = "0";
                btnAdd_Click(sender, e);
                btnDelete.Enabled = false;
            }
        }

        private void txtNoUser_Validated(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text == "")
            {
                txt.Text = "0";
            }
        }
        private void chkviewPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkviewPass.Checked == true)
            {
                txtPassword.PasswordChar = '\0';
                txtrepass.PasswordChar = '\0';

            }
            else
            {
                txtPassword.PasswordChar = '*';
                txtrepass.PasswordChar = '*';
            }
        }
    }
}