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

namespace ExpertMobileSystem
{
    public partial class frmAdminMaster : Form
    {
        

        public frmAdminMaster()
        {
            InitializeComponent();
            //  Load += frmPartymaster_Load;
            btnAdd_Click(null, null);
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


        private void btnAdd_Click(object sender, EventArgs e)
        {
            //bindcombo();

            txtAdminname.Text = "";
            lblid.Text = "0";
            txtPassword.Text = "";
            txtrepass.Text = "";
            txtMobile.Text = "";
            chkSuperAdmin.Checked = false;
            //txtphone1.Text = "";
            //txtphone2.Text = "";
            //txtmobile1.Text = "";
            //txtmobile2.Text = "";
            //txtCST.Text = "";
            //txtVAT.Text = "";
            //txtemail.Text = "";
            //txtaddress1.Text = "";
            //txtaddress2.Text = "";
            //txtaddress3.Text = "";
            //cmbcity.SelectedIndex = cmbcity.Items.Count - 1;
            //cmbcountry.SelectedIndex = cmbcountry.Items.Count - 1;
            //cmbstate.SelectedIndex = cmbstate.Items.Count - 1;
            //txtAdminname.Focus();
        }

        private bool Validate_form()
        {
            if (txtAdminname.Text == "")
            {
                MessageBox.Show("Please Enter Admin Name.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAdminname.Focus();
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
            if (txtMobile.Text == "")
            {
                MessageBox.Show("Please Enter Mobile.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMobile.Focus();
                return false;
            }
          

            if (txtPassword.Text != txtrepass.Text)
            {
                MessageBox.Show("Password Does not match Re-Enter Password.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtrepass.Focus();
                return false;

            }
            return true;





        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!Validate_form())
                return;

            string superadmin = (chkSuperAdmin.Checked == true ? "True" : "False");
            try
            {
                ArrayList Queries = new ArrayList();


                if (lblid.Text == "0")
                {
                    Queries.Add("insert into AdminMaster (AdminName,password,IsSuperAdmin,mobileno) values ('" + txtAdminname.Text + "','" + Operation.Encryptdata(txtPassword.Text) + "'," + superadmin + "," + txtMobile.Text + ")");
                    //Queries.Add("insert into AdminMaster (AdminName,password,IsSuperAdmin,mobileno) values ('" + txtAdminname.Text + "','" + CryptorEngine.Encrypt(txtPassword.Text,true) + "'," + superadmin + "," + txtMobile.Text + ")");
                }
                else
                {
                    Queries.Add("update AdminMaster set AdminName = '" + txtAdminname.Text + "',password = '" + Operation.Encryptdata(txtPassword.Text) + "',IsSuperAdmin = " + superadmin + ", mobileno= " + txtMobile.Text + " where Adminid = " + lblid.Text + "");
                    //Queries.Add("update AdminMaster set AdminName = '" + txtAdminname.Text + "',password = '" + (txtPassword.Text) + "',IsSuperAdmin = " + superadmin + ", mobileno= " + txtMobile.Text + " where Adminid = " + lblid.Text + "");
                }
                if (Operation.ExecuteTransaction(Queries, Operation.Conn))
                {
                    MessageBox.Show("Record Saved Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnAdd_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Error while Saving, Please Try after Some Time.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //if ((this.Modal == true))
                    //    this.Dispose();
                    btnAdd_Click(sender, e);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            //if ((this.Modal == true))
            //    this.Dispose();
        }


        private void btnView_Click(object sender, EventArgs e)
        {

            //if (!(lblid.Text == "0"))
            //{
            //    Operation.CheckLock("partymaster,link," + lblid.Text, true);
            //}
            //else
            //{
            //    Operation.ExecuteNonQuery("Update partymaster set InEdit=0 where link=" + lblid.Text, Operation.Conn);
            //}
            fillgrid();
        }
        private void fillgrid()
        {
            frmSearch view = new frmSearch();
            Operation.gViewQuery = "select Adminid,AdminName,MobileNo,IsSuperAdmin from AdminMaster ";

            ////            view.dgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            Operation.Bindgrid(Operation.gViewQuery, view.dgvSearch);
            view.dgvSearch.Columns[0].Visible = false;
            view.OrderByColoumn = view.dgvSearch.Columns[0].Name.ToString();
            view.ShowDialog();

            view.dgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            if (Operation.ViewID != null && Operation.ViewID != string.Empty)
            {
                //if (!Operation.CheckLock("Partymaster,link", false))
                //    return;
                filldata();
                Operation.ViewID = "";
                btnDelete.Enabled = true;
            }
        }
        public void filldata()
        {
            filldata(Operation.GetDataTable("Select * from AdminMaster where adminid=" + Operation.ViewID.ToString(), Operation.Conn));
        }


        private void WaitWindow()
        {
            //frmLoading Loading = new frmLoading();
            //Loading.Show();
            //while (!Loaded)
            //{
            //    if (Loading.ProgressBar1.Value + 3 > Loading.ProgressBar1.Maximum)
            //    {
            //        Loading.ProgressBar1.Value = 0;
            //    }
            //    Loading.ProgressBar1.Value += 3;
            //    Thread.Sleep(100);
            //}
            //Loading.Close();
        }
        private void filldata(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {

                    txtAdminname.Text = dt.Rows[0]["AdminName"].ToString();
                    lblid.Text = dt.Rows[0]["Adminid"].ToString();
                    txtPassword.Text = Operation.Decryptdata(dt.Rows[0]["password"].ToString());
                    //txtPassword.Text = CryptorEngine.Decrypt(dt.Rows[0]["password"].ToString(),true);
                    txtrepass.Text = txtPassword.Text;
                    txtMobile.Text = dt.Rows[0]["MobileNo"].ToString();
                    chkSuperAdmin.Checked = Convert.ToBoolean(dt.Rows[0]["IsSuperAdmin"]);
                    //bindcombo();
                    //lblid.Text = dt.Rows[0]["link"].ToString();
                    //txtAdminname.Text = dt.Rows[0]["partyname"].ToString();
                    //txtaddress1.Text = dt.Rows[0]["add1"].ToString();
                    //txtaddress2.Text = dt.Rows[0]["add2"].ToString();
                    //txtaddress3.Text = dt.Rows[0]["add3"].ToString();
                    //txtCST.Text = dt.Rows[0]["cstno"].ToString();
                    //txtVAT.Text = dt.Rows[0]["vatno"].ToString();

                    //txtphone1.Text = dt.Rows[0]["phone1"].ToString();
                    //txtphone2.Text = dt.Rows[0]["phone2"].ToString();
                    //txtmobile1.Text = dt.Rows[0]["mobile1"].ToString();
                    //txtmobile2.Text = dt.Rows[0]["mobile2"].ToString();
                    //txtemail.Text = dt.Rows[0]["emailid"].ToString();
                    //cmbcity.SelectedValue = (dt.Rows[0]["citylink"].ToString() == "" ? "0" : dt.Rows[0]["citylink"]);
                    //cmbstate.SelectedValue = (dt.Rows[0]["statelink"].ToString() == "" ? "0" : dt.Rows[0]["statelink"]);
                    //cmbcountry.SelectedValue = (dt.Rows[0]["countrylink"].ToString() == "" ? "0" : dt.Rows[0]["countrylink"]);
                    //cmbAgentName.SelectedValue = (dt.Rows[0]["agentLink"].ToString() == "" ? "0" : dt.Rows[0]["agentLink"]);

                    //btnDelete.Enabled = true;
                    //btnSave.Text = "Update"
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
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
                else if (!Operation.CheckReference(Convert.ToInt32(lblid.Text), "ClientMaster,CreatedAdminID"))
                    return;
                else if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string query = null;
                    query = "Delete from AdminMaster where Adminid= " + lblid.Text.Trim();
                    Operation.ExecuteNonQuery(query, Operation.Conn);
                    // Operation.UserLog(query, this.Name, Operation.Rights.DELETE.ToString());
                    MessageBox.Show("Record Deleted Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblid.Text = "0";
                    btnAdd_Click(sender, e);
                    btnDelete.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time." + Environment.NewLine + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            
            }
        }

        private void frmPartymaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Operation.ExecuteNonQuery("Update PartyMaster set inedit=False where link=" + lblid.Text, Operation.Conn);


        }


        private void txtmobile1_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkSuperAdmin_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void txtMobile_Validated(object sender, EventArgs e)
        {
            object pass = Operation.ExecuteScalar("Select * from AdminMaster where MobileNo = '" + txtMobile.Text + "'", Operation.Conn);
            if (pass.ToString()!= lblid.Text && pass != null)
            {
                MessageBox.Show("Mobile Number:'" + txtMobile.Text + "'  Already Registered.Please Enter Another One.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMobile.Focus();
                return;
            }
        }

        private void txtAdminname_Validated(object sender, EventArgs e)
        {
            object name = Operation.ExecuteScalar("Select * from AdminMaster where AdminName = '" + txtAdminname.Text + "'", Operation.Conn);
            if (name != null)
            {
                MessageBox.Show("Admin Name:'" + txtAdminname.Text + "'  Already Registered.Please Enter Another One.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAdminname.Focus();
                return;
            }
        }
    }
}