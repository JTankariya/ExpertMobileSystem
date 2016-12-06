using System;
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

namespace ExpertMobileSystem_Client_
{
    public partial class frmUserMaster : Form
    {
        public string filename = Guid.NewGuid().ToString();
        //bool blEventExit = false;

        bool Loaded = false;
        //private Thread WaitingThread;

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
                CheckMaximumUser();
                Loaded = true;
            }

        }
        private bool CheckMaximumUser()
        {
            if (Operation.objComp.NoOfAccessUser <= Operation.objComp.TotalCreatedUser)
            {
                MessageBox.Show("You have Created Maximum Number Of User.\n To Add More user Please Contact Administrator.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void frmPartymaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendKeys.Send("{Tab}");
            }
            //if (e.KeyCode == Keys.OemQuotes)
            //{
            //    e.SuppressKeyPress = true;
            //}

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
            if (lblid.Text == "0")
            {
                if (!CheckMaximumUser())
                {
                    return false;

                }
            }

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
            if ((lblid.Text.ToString() != (pass!=null ? pass.ToString() : "")) && (pass!=null))
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

            if (dtpdate.Value < DateTime.Today || dtpdate.Value > Operation.objComp.AccountExpiredOn)
            {
                MessageBox.Show("Expiry date should be Greater Than Today and Less than :" + Operation.objComp.AccountExpiredOn.ToShortDateString(), Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpdate.Value = Operation.objComp.AccountExpiredOn;
                dtpdate.Focus();
                return false;

            }
            //if (!Regex.IsMatch(txtemail.Text, "\\b[A-Z0-9._%-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}\\b", RegexOptions.IgnoreCase) && txtemail.Text != "")
            //{
            //    MessageBox.Show("Please Enter Valid Email Address.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    txtemail.Focus();
            //    return false;
            //}

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
            dtpdate.Value = Operation.objComp.AccountExpiredOn;
            txtmobile.Text = "";
            btnDelete.Enabled = false;
        }

        private bool IncreaseCreatedUserCount()
        {
            ArrayList IncreaseQ = new ArrayList();
            Operation.objComp.TotalCreatedUser += 1;

            IncreaseQ.Add("update ClientMaster set TotalCreatedUser = " + Operation.objComp.TotalCreatedUser + " where ClientID = " + Operation.objComp.ClientId + "");
            if (!Operation.ExecuteTransaction(IncreaseQ, Operation.Conn))
                return false;
            else
                return true;
        }
        private bool DecreaseCreatedUserCount()
        {

            ArrayList DecreaseQ = new ArrayList();
            Operation.objComp.TotalCreatedUser -= 1;

            DecreaseQ.Add("update ClientMaster set TotalCreatedUser = " + Operation.objComp.TotalCreatedUser + " where ClientID = " + Operation.objComp.ClientId + "");
            if (!Operation.ExecuteTransaction(DecreaseQ, Operation.Conn))
                return false;
            else
                return true;
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
                    Queries.Add("Insert into ClientUserMaster ( Clientid ,Password,MobileNo,FirstName,LastName,AccountExpiredOn,CreatedDate ) " +
                        "values(" + Operation.Clientid + ",'" + Operation.Encryptdata(txtPassword.Text) + "','" + txtmobile.Text + "','" + txtFirstname.Text + "','" + txtLastname.Text + "','" + dtpdate.Value.ToString("yyyy-MM-dd") + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "')");

                    //Queries.Add("Insert into ClientUserMaster ( Clientid ,Password,MobileNo,FirstName,LastName,AccountExpiredOn,CreatedDate ) " +
                    //    "values(" + Operation.Clientid + ",'" + CryptorEngine.Encrypt(txtPassword.Text,true) + "','" + txtmobile.Text + "','" + txtFirstname.Text + "','" + txtLastname.Text + "','" + dtpdate.Value.ToString("yyyy-MM-dd") + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "')");
                }
                else
                {
                    Queries.Add("update ClientUserMaster set Password='" + Operation.Encryptdata(txtPassword.Text) + "',MobileNo= '" + txtmobile.Text + "',FirstName='" + txtFirstname.Text.Trim() + "',LastName='" + txtLastname.Text.Trim() + "'" +
                                ",AccountExpiredOn='" + dtpdate.Value.ToString("yyyy-MM-dd") + "' where ClientUserID = " + lblid.Text + " ");

                    //Queries.Add("update ClientUserMaster set Password='" + CryptorEngine.Encrypt(txtPassword.Text,true) + "',MobileNo= '" + txtmobile.Text + "',FirstName='" + txtFirstname.Text.Trim() + "',LastName='" + txtLastname.Text.Trim() + "'" +
                    //            ",AccountExpiredOn='" + dtpdate.Value.ToString("yyyy-MM-dd") + "' where ClientUserID = " + lblid.Text + " ");
                }
                if (Operation.ExecuteTransaction(Queries, Operation.Conn))
                {
                    MessageBox.Show("Record Saved Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (lblid.Text == "0")
                        IncreaseCreatedUserCount();
                    CheckMaximumUser();
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
            Operation.gViewQuery = "select ClientUserid,CreatedDate,FirstName, LastName, MobileNo, AccountExpiredOn from ClientUserMaster where ClientId = " + Operation.Clientid + "";

            ////            view.dgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            Operation.Bindgrid(Operation.gViewQuery, view.dgvSearch);
            view.dgvSearch.Columns[0].Visible = false;
            view.OrderByColoumn = "CreatedDate";
            view.ShowDialog();

            view.dgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            if (Operation.ViewID != null && Operation.ViewID != string.Empty)
            {
                //if (!Operation.CheckLock("Partymaster,link", false))
                //    return;
                filldata();
                Operation.ViewID = "";
            }
        }
        public void filldata()
        {
            filldata(Operation.GetDataTable("Select * from ClientUserMaster where ClientUserId=" + Operation.ViewID.ToString(), Operation.Conn));
        }
        private void filldata(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    lblid.Text = dt.Rows[0]["ClientUserID"].ToString();
                    txtmobile.Text = dt.Rows[0]["MobileNo"].ToString();
                    txtFirstname.Text = dt.Rows[0]["FirstName"].ToString();
                    txtLastname.Text = dt.Rows[0]["LastName"].ToString();
                    dtpdate.Value = Convert.ToDateTime(dt.Rows[0]["AccountExpiredOn"]);
                    txtPassword.Text = Operation.Decryptdata(dt.Rows[0]["Password"].ToString());
                    //txtPassword.Text = CryptorEngine.Decrypt(dt.Rows[0]["Password"].ToString(),true);
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
                //ElseIf MsgBox("Are you sure you want to Delete the record?", MsgBoxStyle.YesNo, "Shah Software") = MsgBoxResult.Yes Then
            }
            //else if (!(Operation.RightsStr.Contains(Convert.ToInt32(Operation.Rights.DELETE).ToString())))
            //{
            //    MessageBox.Show("You don't have rights, Please contact your Administrator.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            else if (!Operation.CheckReference(Convert.ToInt32(lblid.Text), "ClientUserCompanyMaster,ClientUserID"))
                return;
            else if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = null;
                query = "Delete from ClientUserMaster where ClientUserID= " + lblid.Text.Trim();
                Operation.ExecuteNonQuery(query, Operation.Conn);
                {
                    DecreaseCreatedUserCount();
                    CheckMaximumUser();
                    //ArrayList Queries =  new ArrayList();
                    //Queries.Add(DecreaseCreatedUserCount());
                    //Operation.ExecuteTransaction(Queries, Operation.Conn);

                }
                //  Operation.UserLog(query, this.Name, Operation.Rights.DELETE.ToString());
                MessageBox.Show("Record Deleted Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblid.Text = "0";
                btnAdd_Click(sender, e);
                btnDelete.Enabled = false;
            }
        }

        private void frmPartymaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Operation.ExecuteNonQuery("Update PartyMaster set inedit=False where link=" + lblid.Text, Operation.Conn);


        }

        private void cmbAgentName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                frmUserMaster p1 = new frmUserMaster();
                //  frmMDI.partyMaster = 3;
                p1.ShowDialog();
                //  Operation.BindComboBox(cmbAgentName, "SELECT max(PartyMaster.link) as link, iif(isnull(PartyMaster.partyName),'-- Select Agent Name--',PartyMaster.partyName) as partyName FROM PartyMaster   where (((PartyMaster.partyType)=" + Convert.ToInt32(Operation.PartyType.Agent) + ")) group by partyName order by partyName", "-- Select Agent Name--", "partyName", "link");

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