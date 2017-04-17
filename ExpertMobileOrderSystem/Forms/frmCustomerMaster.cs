using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Collections;
using System.Drawing.Printing;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Data.OleDb;

namespace ExpertMobileOrderSystem
{
    public partial class frmCustomerMaster : Form
    {
        public string filename = Guid.NewGuid().ToString();
        bool Loaded = false;

        public frmCustomerMaster()
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

        private void frmPartymaster_Load(object sender, EventArgs e)
        {
            if (Loaded == false)
            {
                btnAdd_Click(sender, e);
                Loaded = true;
                FillPartyCombo(false);
            }

        }
        private void FillPartyCombo(bool isUpdate)
        {
            if (Operation.currClient.IsWithout)
            {
                var company = Operation.currClient.BillableCompanies[0];
                var query = "";
                if (!isUpdate)
                {
                    query = "select * from [Order.ACT] where [Group]='100003' and clientcompanyid=" +
                    company["ClientCompanyId"] + " and Code not in (select PartyCode from [Order.ClientUserMaster]" +
                    " where ClientId=" + Operation.currClient.Id + " and UserTypeId=" +
                    UserTypes.CUSTOMER.ToString() + ")";
                }
                else
                {
                    query = "select * from [Order.ACT] where [Group]='100003' and clientcompanyid=" +
                        company["ClientCompanyId"] + " and Code not in (select PartyCode from [Order.ClientUserMaster]" +
                        " where ClientId=" + Operation.currClient.Id + " and UserTypeId=" +
                        UserTypes.CUSTOMER.ToString() + " and Id!=" + lblid.Text + ")";
                }
                var dtParty = Operation.GetDataTable(query, Operation.Conn);
                if (dtParty != null && dtParty.Rows.Count > 0)
                {
                    cmbParty.DataSource = dtParty;
                    cmbParty.DisplayMember = "Name";
                    cmbParty.ValueMember = "Code";
                }
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
            object pass = Operation.ExecuteScalar("Select * from [Order.ClientUserMaster] where MobileNo = '" + txtmobile.Text + "'", Operation.Conn);
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

            qu = "select * from [Order.ClientUserMaster] where PartyCode='" + cmbParty.SelectedValue.ToString() + "'";
            if (lblid.Text != "0")
            {
                qu += " and Id<>" + lblid.Text;
            }
            dtUsers = Operation.GetDataTable(qu, Operation.Conn);
            if (dtUsers != null && dtUsers.Rows.Count > 0)
            {
                MessageBox.Show("This party has been already given credentials, Please select another party.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbParty.Focus();
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
                    Queries.Add("Insert into [Order.ClientUserMaster] ( Clientid ,Password,MobileNo,FirstName,LastName," +
                        "AccountExpiredOn,CreatedDate,UserName,UserTypeId,PartyCode) " +
                        "values(" + Operation.currClient.Id + ",'" + Operation.Encryptdata(txtPassword.Text) + "','" +
                        txtmobile.Text + "','" + txtFirstname.Text + "','" + txtLastname.Text + "','" +
                        dtpdate.Value.ToString("yyyy-MM-dd") + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "','" +
                        txtUserName.Text + "'," + UserTypes.CUSTOMER.ToString() + ",'" + cmbParty.SelectedValue + "')");
                    Queries.Add("update [Order.ClientMaster] set TotalCreatedUser = " + (Operation.currClient.TotalCreatedUser + 1) + " where ID = " + Operation.currClient.Id);
                }
                else
                {
                    Queries.Add("update [Order.ClientUserMaster] set Password='" + Operation.Encryptdata(txtPassword.Text) +
                        "',MobileNo= '" + txtmobile.Text + "',FirstName='" + txtFirstname.Text.Trim() +
                        "',LastName='" + txtLastname.Text.Trim() + "',AccountExpiredOn='" +
                        dtpdate.Value.ToString("yyyy-MM-dd") + "',UserName='" + txtUserName.Text +
                        "',UserTypeId=" + UserTypes.CUSTOMER.ToString() + ",PartyCode='" + cmbParty.SelectedValue + "' where ClientID = " + lblid.Text + " ");
                    
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
            Operation.gViewQuery = "select Id,CreatedDate,FirstName, LastName, MobileNo, AccountExpiredOn from [Order.ClientUserMaster] where ClientId = " + Operation.currClient.Id + " and UserTypeId=" + UserTypes.CUSTOMER.ToString() + " and PartyCode is not null";
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
            filldata(Operation.GetDataTable("Select * from [Order.ClientUserMaster] where Id=" + Operation.ViewID.ToString(), Operation.Conn));
        }
        private void filldata(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    lblid.Text = dt.Rows[0]["Id"].ToString();
                    txtmobile.Text = dt.Rows[0]["MobileNo"].ToString();
                    txtUserName.Text = dt.Rows[0]["UserName"].ToString();
                    txtFirstname.Text = dt.Rows[0]["FirstName"].ToString();
                    txtLastname.Text = dt.Rows[0]["LastName"].ToString();
                    dtpdate.Value = Convert.ToDateTime(dt.Rows[0]["AccountExpiredOn"]);
                    txtPassword.Text = Operation.Decryptdata(dt.Rows[0]["Password"].ToString());
                    txtrepass.Text = txtPassword.Text;
                    btnDelete.Enabled = true;
                    FillPartyCombo(true);
                    cmbParty.SelectedValue = dt.Rows[0]["PartyCode"].ToString();
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