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
    public partial class frmClientMaster : Form
    {
        public string filename = Guid.NewGuid().ToString();
        ArrayList Queries = new ArrayList();
        DateTime CurrentDate = Operation.GetNetworkTime();
        public frmClientMaster()
        {
            InitializeComponent();
        }
        private void txtmobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)8))
                e.Handled = true;
        }
        private void frmPartymaster_Load(object sender, EventArgs e)
        {
            btnAdd_Click(sender, e);
        }
        private void frmPartymaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendKeys.Send("{Tab}");
            }
            if (e.KeyCode == Keys.OemQuotes)
            {
                e.SuppressKeyPress = true;
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
            if (txtCompanyname.Text == "")
            {
                MessageBox.Show("Please Enter Company Name.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCompanyname.Focus();
                return false;
            }
            if (txtadress.Text == "")
            {
                MessageBox.Show("Please Enter Company-Adress.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtadress.Focus();
                return false;
            }
            if (txtmobile.Text == "")
            {
                MessageBox.Show("Please Enter Mobile.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmobile.Focus();
                return false;
            }
            if (txtemail.Text == "")
            {
                MessageBox.Show("Please Enter Email Adress.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtemail.Focus();
                return false;
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
            if (txtUsername.Text == "")
            {
                MessageBox.Show("Please Enter Valid Party Name.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtUsername.Focus();
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
            if (txtNoUser.Text == "")
            {
                MessageBox.Show("Please Enter No. Of User.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNoUser.Focus();
                return false;
            }
            if (txtNoComp.Text == "")
            {
                MessageBox.Show("Please Enter No. of Company.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNoComp.Focus();
                return false;
            }
            if (CurrentDate >= dtpdate.Value)
            {
                MessageBox.Show("Expiry date should be Greater Then Today.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpdate.Focus();
                return false;

            }
            if (!Regex.IsMatch(txtemail.Text, "\\b[A-Z0-9._%-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}\\b", RegexOptions.IgnoreCase) && txtemail.Text != "")
            {
                MessageBox.Show("Please Enter Valid Email Address.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtemail.Focus();
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Queries.Clear();
            lblid.Text = "0";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtrepass.Text = "";
            txtFirstname.Text = "";
            txtLastname.Text = "";
            txtCompanyname.Text = "";
            txtadress.Text = "";
            txtmobile.Text = "";
            txtemail.Text = "";
            txtNoUser.Text = "0";
            txtNoComp.Text = "0";
            dtpdate.Value = System.DateTime.Today;
            dgvMenu.Rows.Clear();
            BindMenuGrid();
            grpDash.Visible = false;
            dgvDash.Rows.Clear();
            txtmobile.Text = "";
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Validate_form())
                return;
            try
            {
                int NoOfDays = Convert.ToInt32(((dtpdate.Value - CurrentDate).Days));
                if (lblid.Text == "0")
                {
                    Queries.Add("Insert into ClientMaster ( ClientCreatedDate , UserName,Password,MobileNo,FirstName,LastName,CompanyName,CompanyAddress,Email,NoOfDays,NoOfAccessUsers,NoOfCompanyPerUser,AccountExpiredOn,IsUpLoadingProcessStart,CreatedAdminID ) " +
                        "values('" + CurrentDate.ToString("yyyy-MM-dd") + "','" + txtUsername.Text + "','" + Operation.Encryptdata(txtPassword.Text) + "'," + txtmobile.Text + ",'" + txtFirstname.Text + "','" + txtLastname.Text + "','" + txtCompanyname.Text + "','" + txtadress.Text + "','" + txtemail.Text + "'," + NoOfDays + "," + txtNoUser.Text + "," + txtNoComp.Text + ",'" + dtpdate.Value.ToString("yyyy-MM-dd") + "',False," + Operation.AdminUserId + ")");
                }
                else
                {
                    Queries.Add("update ClientMaster set UserName = '" + txtUsername.Text + "',Password='" + Operation.Encryptdata(txtPassword.Text) + "',MobileNo= " + txtmobile.Text + ",FirstName='" + txtFirstname.Text + "',LastName='" + txtLastname.Text + "'" +
                                ",CompanyName='" + txtCompanyname.Text + "',CompanyAddress='" + txtadress.Text + "',Email='" + txtemail.Text + "',NoOfDays=" + NoOfDays + ",NoOfAccessUsers=" + txtNoUser.Text + ",NoOfCompanyPerUser=" + txtNoComp.Text + ",AccountExpiredOn='" + dtpdate.Value.ToString("yyyy-MM-dd") + "' where ClientID = " + lblid.Text + "");
                }
                if (Operation.ExecuteTransaction(Queries, Operation.Conn))
                {
                    ArrayList Menuq = new ArrayList();
                    if (ChekForMenu())
                    {

                        string ClientID;
                        if (lblid.Text == "0")
                            ClientID = Operation.ExecuteScalar("SELECT max(ClientID) from ClientMaster where CreatedAdminID = " + Operation.AdminUserId + "", Operation.Conn).ToString();
                        else
                            ClientID = lblid.Text;
                        string chiddata = "0";
                        for (int i = 0; i < dgvMenu.Rows.Count; i++)
                        {
                            if (dgvMenu.Rows[i].DefaultCellStyle.BackColor == Color.Green)
                            {
                                if (dgvMenu.Rows[i].Cells["HasChildData"].Value.ToString() == "True")
                                    chiddata = "1";
                                if (dgvMenu.Rows[i].Cells["Link"].Value == null)
                                    Menuq.Add("Insert into ClientMenuMaster (ClientID,MenuID,Query,HasChildData) values (" + ClientID + "," + dgvMenu.Rows[i].Cells["MenuID"].Value + ",'" + dgvMenu.Rows[i].Cells["Query"].Value + "'," + chiddata.Trim() + "  )");
                            }
                        }
                        if (dgvDash.Visible == true)
                        {
                            if (!InsertDashMenu(Menuq, ClientID))
                            {
                                MessageBox.Show("Please Select Atlease One Dashboard Record", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                               return;
                            }
                        }
                        Operation.ExecuteTransaction(Menuq, Operation.Conn);
                    }
                    MessageBox.Show("Record Saved Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private bool InsertDashMenu(ArrayList ar, string ClientID)
        {
            bool ans = false;

            for (int i = 0; i < dgvDash.Rows.Count; i++)
            {
                if (dgvDash.Rows[i].DefaultCellStyle.BackColor == Color.Blue)
                {
                    if (dgvDash.Rows[i].Cells["DashLink"].Value == null)
                        ar.Add("Insert into ClientDashBoardMaster (ClientID,Query,DashBoardID) values (" + ClientID + ",'" + dgvDash.Rows[i].Cells["DashQuery"].Value + "'," + dgvDash.Rows[i].Cells["DashBoardID"].Value + "  )");
                   ans = true;
                }
            }
            return ans;
        }
        private bool ChekForMenu()
        {
            for (int i = 0; i < dgvMenu.Rows.Count; i++)
            {
                if (dgvMenu.Rows[i].DefaultCellStyle.BackColor == Color.Green)
                {
                    return true;
                }
            }
            return false;
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            // Operation.ShowSplash();
            fillgrid();
        }
        private void fillgrid()
        {
            frmSearch view = new frmSearch();
            Operation.gViewQuery = "Select ClientID, ClientCreatedDate ,FirstName,LastName,CompanyName,CompanyAddress,City,State,Country, UserName,MobileNo,Email,NoOfAccessUsers,NoOfCompanyPerUser,AccountExpiredOn from ClientMaster";
            Operation.Bindgrid(Operation.gViewQuery, view.dgvSearch);
            view.dgvSearch.Columns[0].Visible = false;
            view.OrderByColoumn = "ClientCreatedDate";
            //  Operation.CloseSplash();
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
            filldata(Operation.GetDataTable("Select * from ClientMaster where Clientid=" + Operation.ViewID.ToString(), Operation.Conn));
        }
        private void filldata(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    lblid.Text = dt.Rows[0]["ClientID"].ToString();
                    txtCompanyname.Text = dt.Rows[0]["CompanyName"].ToString();
                    txtadress.Text = dt.Rows[0]["CompanyAddress"].ToString();
                    txtmobile.Text = dt.Rows[0]["MobileNo"].ToString();
                    txtemail.Text = dt.Rows[0]["Email"].ToString();
                    txtFirstname.Text = dt.Rows[0]["FirstName"].ToString();
                    txtLastname.Text = dt.Rows[0]["LastName"].ToString();
                    txtUsername.Text = dt.Rows[0]["UserName"].ToString();
                    txtPassword.Text = Operation.Decryptdata(dt.Rows[0]["Password"].ToString());
                    txtrepass.Text = txtPassword.Text;
                    txtNoUser.Text = dt.Rows[0]["NoOfAccessUsers"].ToString();
                    txtNoComp.Text = dt.Rows[0]["NoOfCompanyPerUser"].ToString();
                    dtpdate.Value = Convert.ToDateTime(dt.Rows[0]["AccountExpiredOn"]);
                    BindMenuGrid();
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
            else if (!Operation.CheckReference(Convert.ToInt32(lblid.Text), "ClientUserMaster,ClientID,ClientCompanyMaster,ClientID"))
                return;
            else if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ArrayList QueriesDel = new ArrayList();
                QueriesDel.Add("Delete from ClientMaster where ClientID= " + lblid.Text.Trim());
                QueriesDel.Add(" delete from ClientMenuMaster Where ClientID = " + lblid.Text + " ");
                Operation.ExecuteTransaction(QueriesDel, Operation.Conn);
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
        private void txtNoUser_Validated(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text == "")
              txt.Text = "0";
        }
        private void btnselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvMenu.Rows.Count; i++)
            {
                dgvMenu.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                dgvMenu.Rows[i].Cells["Select"].Value = true;
            }
        }
        private void btnUnselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvMenu.Rows.Count; i++)
            {
                dgvMenu.Rows[i].DefaultCellStyle.BackColor = Color.White;
                dgvMenu.Rows[i].Cells["Select"].Value = false;
            }
        }
        private void EnableDisabledgvDas(int index)
        {
            dgvDash.DataSource = null;
            dgvDash.Rows.Clear();

            //string q = "select DashboardID, MenuID, DashboardName, Type, Query from  DashboardMaster where MenuID = " + dgvMenu.Rows[index].Cells["MenuID"].Value + " ";
            string q = "SELECT    clientdash.ClientDashBoardID, DashboardMaster.DashboardID,    DashboardMaster.MenuID,    DashboardMaster.Type," +
                       " DashboardMaster.DashboardName,    if(isnull( clientdash.Query),DashboardMaster.query,clientdash.query) as Query,    IF(ISNULL(clientdash.ClientDashBoardID),        0,        1) AS AllocatedDash" +
                       " FROM    (SELECT    *   FROM     ClientDashBoardMaster   WHERE    ClientID = " + lblid.Text + ") AS clientdash     right JOIN   DashboardMaster ON  clientdash.DashBoardID = DashboardMaster.DashboardID";
            DataTable dt = Operation.GetDataTable(q, Operation.Conn);
            dgvDash.ReadOnly = true;
            dgvDash.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvDash.Rows.Add();

                    //   dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashLink"].Value = dt.Rows[i]["DashboardID"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashboardID"].Value = dt.Rows[i]["DashboardID"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashMenuID"].Value = dt.Rows[i]["MenuID"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashboardName"].Value = dt.Rows[i]["DashboardName"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["Type"].Value = dt.Rows[i]["Type"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashQuery"].Value = dt.Rows[i]["Query"];
                    if (dt.Rows[i]["AllocatedDash"].ToString() == "1")
                    {
                        dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["Dashselect"].Value = true;
                        dgvDash.Rows[dgvDash.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Blue;
                        dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashLink"].Value = dt.Rows[i]["ClientDashBoardID"];
                    }
                    else
                    {
                        dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["Dashselect"].Value = false;
                        dgvDash.Rows[dgvDash.Rows.Count - 1].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }
        private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                if (dgvMenu.Rows[dgvMenu.CurrentRow.Index].DefaultCellStyle.BackColor == Color.White)
                {
                    dgvMenu.Rows[e.RowIndex].Cells["Select"].Value = true;
                    dgvMenu.Rows[dgvMenu.CurrentRow.Index].DefaultCellStyle.BackColor = Color.Green;
                    if (dgvMenu.Rows[e.RowIndex].Cells["HasDashboard"].Value.ToString() == "True")
                    {
                        EnableDisabledgvDas(e.RowIndex);
                        grpDash.Visible = true;
                    }
                }
                else
                {
                    if (dgvMenu.Rows[e.RowIndex].Cells["Link"].Value != null)
                    {
                        if (MessageBox.Show("Are you sure you want to remove this menu?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Queries.Add("delete from ClientMenuMaster Where ClientMenuID = " + dgvMenu.Rows[e.RowIndex].Cells["Link"].Value + " ");
                            Queries.Add("delete from ClientUserMenuMaster Where ClientMenuID = " + dgvMenu.Rows[e.RowIndex].Cells["Link"].Value + " ");
                            Queries.Add("delete from ClientDashBoardMaster Where ClientID = " + lblid.Text + " ");
                            if (dgvMenu.Rows[e.RowIndex].Cells["HasDashboard"].Value.ToString() == "True")
                            {
                                for (int i = 0; i < dgvDash.Rows.Count; i++)
                                {
                                    if (dgvDash.Rows[i].Cells["DashLink"].Value != null)
                                        Queries.Add("delete from ClientUserDashBoardMaster Where ClientDashBoardID = " + dgvDash.Rows[i].Cells["DashLink"].Value + " ");
                                }
                                grpDash.Visible = false;

                            }
                        }

                    }

                    else
                    {
                        dgvMenu.Rows[e.RowIndex].Cells["Select"].Value = false;
                        dgvMenu.Rows[dgvMenu.CurrentRow.Index].DefaultCellStyle.BackColor = Color.White;
                    }
                    dgvMenu.Rows[e.RowIndex].Cells["Select"].Value = false;
                    dgvMenu.Rows[dgvMenu.CurrentRow.Index].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
        private void BindMenuGrid()
        {
            dgvMenu.Rows.Clear();
            grpDash.Visible = false;
            string Q;
            DataTable dt = new DataTable();
            Q = "SELECT  MenuMaster.MenuName,    MenuMaster.SrNo,    ClientMenu.ClientMenuId,    ClientMenu.ClientID,    MenuMaster.MenuID,If(Isnull(ClientMenu.HasChildData),MenuMaster.HasChildData,ClientMenu.HasChildData) as HasChildData,If(Isnull(ClientMenu.IsDashboard),MenuMaster.IsDashboard,ClientMenu.IsDashboard) as IsDashboard," +
                    "If(isnull(ClientMenu.Query),MenuMaster.Query,ClientMenu.Query) as Query, IF(ISNULL(ClientMenu.ClientId), 0, 1) AS AllocatedToClient FROM  (Select * From ClientMenuMaster where ClientMenuMaster.ClientId = " + lblid.Text + ") as ClientMenu right JOIN   MenuMaster ON ClientMenu.MenuID = MenuMaster.MenuID ";
            dt = Operation.GetDataTable(Q, Operation.Conn);
            dgvMenu.ReadOnly = true;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                   dgvMenu.Rows.Add();
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["MenuID"].Value = dt.Rows[i]["MenuID"];
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["SrNo"].Value = dt.Rows[i]["SrNo"];
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["MenuName"].Value = dt.Rows[i]["MenuName"];
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["Query"].Value = dt.Rows[i]["Query"];
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].DefaultCellStyle.BackColor = Color.White;
                    if (dt.Rows[i]["HasChildData"].ToString() == "1")
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["HasChildData"].Value = true;
                    else
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["HasChildData"].Value = false;
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["HasDashboard"].Value = Convert.ToBoolean(dt.Rows[i]["IsDashboard"]);
                    if (dt.Rows[i]["AllocatedToClient"].ToString() == "1")
                    {
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["select"].Value = true;
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Green;
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["Link"].Value = dt.Rows[i]["ClientMenuID"];
                        if (dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["HasDashboard"].Value.ToString() == "True")
                        {
                            EnableDisabledgvDas(dgvMenu.Rows.Count - 1);
                            grpDash.Visible = true;
                        }
                    }
                    else
                    {
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["select"].Value = false;
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].DefaultCellStyle.BackColor = Color.White;
                    }
                    if (i == 1)
                        dgvMenu.Columns[i].ReadOnly = false;
                }
            }
        }
        private void dgvDash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                if (dgvDash.Rows[dgvDash.CurrentRow.Index].DefaultCellStyle.BackColor == Color.White)
                {
                    dgvDash.Rows[e.RowIndex].Cells["DashSelect"].Value = true;
                    dgvDash.Rows[dgvDash.CurrentRow.Index].DefaultCellStyle.BackColor = Color.Blue;
                }
                else
                {
                    if (dgvDash.Rows[e.RowIndex].Cells["DashLink"].Value != null)
                    {
                        if (MessageBox.Show("Are you sure you want to remove this menu?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Queries.Add("delete from ClientDashBoardMaster Where ClientDashBoardID = " + dgvDash.Rows[e.RowIndex].Cells["DashLink"].Value + " ");
                            //  Queries.Add("delete from ClientUserMenuMaster Where ClientMenuID = " + dgvDash.Rows[e.RowIndex].Cells["Link"].Value + " ");
                       }
                    }
                    dgvDash.Rows[e.RowIndex].Cells["DashSelect"].Value = false;
                    dgvDash.Rows[dgvDash.CurrentRow.Index].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
        private void brnDashSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvDash.Rows.Count; i++)
            {
                dgvDash.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                dgvDash.Rows[i].Cells["DashSelect"].Value = true;
            }
        }
        private void btndashUnselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvDash.Rows.Count; i++)
            {
                dgvDash.Rows[i].DefaultCellStyle.BackColor = Color.White;
                dgvDash.Rows[i].Cells["DashSelect"].Value = false;
            }
        }
        private void chkviewPass_CheckedChanged_1(object sender, EventArgs e)
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
