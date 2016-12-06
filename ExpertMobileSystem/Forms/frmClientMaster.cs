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
        //DateTime CurrentDate = Operation.GetNetworkTime();
        DateTime CurrentDate = DateTime.Now;
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
            object pass = Operation.ExecuteScalar("Select * from ClientMaster where UserName = '" + txtUsername.Text + "'", Operation.Conn);
            if ((pass != null) && (pass.ToString() != lblid.Text ))
            {
                MessageBox.Show("User Name : " + txtUsername.Text + " already Exist.Please Enter Another One.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmobile.Focus();
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
            picUser.Visible = false;
            picCompany.Visible = false;
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
            txtmobile.Text = "";
            chkQuery.Checked = false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Validate_form())
                return;
            int QRights = (chkQuery.Checked == true ? 1 : 0);
            try
            {
                int NoOfDays = Convert.ToInt32(((dtpdate.Value - CurrentDate).Days));
                if (lblid.Text == "0")
                {
                    Queries.Add("Insert into ClientMaster ( ClientCreatedDate , UserName,Password,MobileNo,FirstName,LastName,CompanyName,CompanyAddress,Email,NoOfDays,NoOfAccessUsers,NoOfCompanyPerUser,AccountExpiredOn,IsUpLoadingProcessStart,CreatedAdminID,QueryRights ) " +
                        "values('" + CurrentDate.ToString("yyyy-MM-dd") + "','" + txtUsername.Text + "','" + Operation.Encryptdata(txtPassword.Text) + "'," + txtmobile.Text + ",'" + txtFirstname.Text + "','" + txtLastname.Text + "','" + txtCompanyname.Text + "','" + txtadress.Text + "','" + txtemail.Text + "'," + NoOfDays + "," + txtNoUser.Text + "," + txtNoComp.Text + ",'" + dtpdate.Value.ToString("yyyy-MM-dd") + "',0," + Operation.AdminUserId + "," + QRights + ")");
                    //Queries.Add("Insert into ClientMaster ( ClientCreatedDate , UserName,Password,MobileNo,FirstName,LastName,CompanyName,CompanyAddress,Email,NoOfDays,NoOfAccessUsers,NoOfCompanyPerUser,AccountExpiredOn,IsUpLoadingProcessStart,CreatedAdminID,QueryRights ) " +
                    //    "values('" + CurrentDate.ToString("yyyy-MM-dd") + "','" + txtUsername.Text + "','" + CryptorEngine.Encrypt(txtPassword.Text,true) + "'," + txtmobile.Text + ",'" + txtFirstname.Text + "','" + txtLastname.Text + "','" + txtCompanyname.Text + "','" + txtadress.Text + "','" + txtemail.Text + "'," + NoOfDays + "," + txtNoUser.Text + "," + txtNoComp.Text + ",'" + dtpdate.Value.ToString("yyyy-MM-dd") + "',False," + Operation.AdminUserId + "," + QRights + ")");

                }
                else
                {
                    Queries.Add("update ClientMaster set UserName = '" + txtUsername.Text + "',Password='" + Operation.Encryptdata(txtPassword.Text) + "',MobileNo= " + txtmobile.Text + ",FirstName='" + txtFirstname.Text + "',LastName='" + txtLastname.Text + "'" +
                                ",CompanyName='" + txtCompanyname.Text + "',CompanyAddress='" + txtadress.Text + "',Email='" + txtemail.Text + "',NoOfDays=" + NoOfDays + ",NoOfAccessUsers=" + txtNoUser.Text + ",NoOfCompanyPerUser=" + txtNoComp.Text + ",AccountExpiredOn='" + dtpdate.Value.ToString("yyyy-MM-dd") + "',QueryRights=" + QRights + " where ClientID = " + lblid.Text + "");
                    //Queries.Add("update ClientMaster set UserName = '" + txtUsername.Text + "',Password='" + CryptorEngine.Encrypt(txtPassword.Text,true) + "',MobileNo= " + txtmobile.Text + ",FirstName='" + txtFirstname.Text + "',LastName='" + txtLastname.Text + "'" +
                    //              ",CompanyName='" + txtCompanyname.Text + "',CompanyAddress='" + txtadress.Text + "',Email='" + txtemail.Text + "',NoOfDays=" + NoOfDays + ",NoOfAccessUsers=" + txtNoUser.Text + ",NoOfCompanyPerUser=" + txtNoComp.Text + ",AccountExpiredOn='" + dtpdate.Value.ToString("yyyy-MM-dd") + "',QueryRights=" + QRights + " where ClientID = " + lblid.Text + "");
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
                                    Menuq.Add("Insert into ClientMenuMaster (ClientID,MenuID) values (" + ClientID + "," + dgvMenu.Rows[i].Cells["MenuID"].Value + "  )");
                            }
                            else if (dgvMenu.Rows[i].Cells["Link"].Value != null && dgvMenu.Rows[i].DefaultCellStyle.BackColor == Color.White)
                            {
                                Menuq.Add("delete from ClientMenuMaster Where ClientMenuID = " + dgvMenu.Rows[i].Cells["Link"].Value + " ");
                                Menuq.Add("delete from ClientUserMenuMaster Where ClientMenuID = " + dgvMenu.Rows[i].Cells["Link"].Value + " ");

                                if (dgvMenu.Rows[i].Cells["HasDashBoard"].Value.ToString() == "True")
                                {
                                    Menuq.Add("delete from ClientDashBoardMaster Where ClientMenuID = " + dgvMenu.Rows[i].Cells["Link"].Value + " ");

                                    object obj = Operation.ExecuteScalar("SELECT  ClientUserDashBoardMaster.ClientDashBoardID FROM " +
                                                                             " ClientUserDashBoardMaster   INNER JOIN " +
                                                                             "  ClientDashBoardMaster ON ClientDashBoardMaster.ClientDashBoardID = ClientUserDashBoardMaster.ClientDashBoardID " +
                                                                             " WHERE  ClientDashBoardMaster.ClientMenuID = " + dgvMenu.Rows[i].Cells["Link"].Value + "", Operation.Conn);
                                    if (obj != null)
                                        Menuq.Add("delete from ClientUserDashBoardMaster Where ClientDashBoardID = " + obj.ToString() + " ");

                                }
                                if (dgvMenu.Rows[i].Cells["chkHasSubMenu"].Value.ToString() == "True")
                                {
                                    Menuq.Add("delete from ClientChildMenuMaster Where ClientMenuID = " + dgvMenu.Rows[i].Cells["Link"].Value + " ");

                                    object obj = Operation.ExecuteScalar("SELECT ClientChildMenuMaster.ClientChildMenuID FROM ClientUserChildMenuMaster    INNER JOIN " +
                                                                                 " ClientChildMenuMaster ON ClientChildMenuMaster.ClientChildMenuID = ClientUserChildMenuMaster.ClientChildMenuID " +
                                                                                  " where  ClientChildMenuMaster.ClientMenuID =" + dgvMenu.Rows[i].Cells["Link"].Value + "", Operation.Conn);
                                    if (obj != null)
                                        Menuq.Add("delete from ClientUserChildMenuMaster Where ClientChildMenuID = " + obj.ToString() + " ");

                                }//Menuq.Add("delete from ClientChildMenuMaster Where ClientMenuID = " + dgvMenu.Rows[i].Cells["Link"].Value + " ");

                            }
                        }

                        if (!Operation.ExecuteTransaction(Menuq, Operation.Conn))
                        {
                            MessageBox.Show("Error while Saving, Please Try after Some Time.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Queries = new ArrayList();
                            return;

                        }

                        MessageBox.Show("Record Saved Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnAdd_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Error while Saving, Please Try after Some Time.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Queries = new ArrayList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Queries = new ArrayList();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
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
            picCompany.Visible = false;
            picUser.Visible = false;
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
                    //txtPassword.Text = CryptorEngine.Decrypt(dt.Rows[0]["Password"].ToString(),true);
                    txtrepass.Text = txtPassword.Text;
                    txtNoUser.Text = dt.Rows[0]["NoOfAccessUsers"].ToString();
                    txtNoComp.Text = dt.Rows[0]["NoOfCompanyPerUser"].ToString();
                    dtpdate.Value = Convert.ToDateTime(dt.Rows[0]["AccountExpiredOn"]);

                    chkQuery.Checked = Convert.ToBoolean(dt.Rows[0]["QueryRights"]);
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
            else if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ArrayList QueriesDel = new ArrayList();
                QueriesDel.Add("Delete from ClientMaster where ClientID= " + lblid.Text.Trim());
                QueriesDel.Add("delete from ClientDashBoardMaster Using ClientDashBoardMaster,ClientMenuMaster Where ClientMenuMaster.ClientMenuId = ClientDashBoardMaster.ClientMenuId AND ClientID = " + lblid.Text + " ");
                QueriesDel.Add("delete from ClientChildMenuMaster Using ClientChildMenuMaster,ClientMenuMaster Where ClientMenuMaster.ClientMenuId = ClientChildMenuMaster.ClientMenuId AND ClientID = " + lblid.Text + " ");
                QueriesDel.Add(" delete from ClientMenuMaster Where ClientID = " + lblid.Text + " ");
                QueriesDel.Add("delete from ClientCompanyMaster Where ClientID = " + lblid.Text + " ");
                QueriesDel.Add("DELETE FROM ClientUserChildMenuMaster USING ClientUserMenuMaster, ClientUserChildMenuMaster, ClientMenuMaster WHERE    ClientUserMenuMaster.ClientUserMenuID = ClientUserChildMenuMaster.ClientUserMenuID AND ClientUserMenuMaster.ClientMenuID = ClientMenuMaster.ClientMenuID    AND ClientMenuMaster.ClientID = " + lblid.Text + " ");
                QueriesDel.Add("DELETE FROM ClientUserDashBoardMaster USING ClientUserMenuMaster, ClientUserDashBoardMaster, ClientMenuMaster WHERE    ClientUserMenuMaster.ClientUserMenuID = ClientUserDashBoardMaster.ClientUserMenuID AND ClientUserMenuMaster.ClientMenuID = ClientMenuMaster.ClientMenuID    AND ClientMenuMaster.ClientID = " + lblid.Text + " ");
                QueriesDel.Add("DELETE FROM ClientUserMenuMaster USING ClientUserMenuMaster, ClientMenuMaster WHERE    ClientUserMenuMaster.ClientMenuID = ClientMenuMaster.ClientMenuID    AND ClientMenuMaster.ClientID = " + lblid.Text + " ");
                QueriesDel.Add("delete from ClientUserCompanyMaster Where ClientID = " + lblid.Text + " ");
                QueriesDel.Add("delete from ClientUserMaster Where ClientID = " + lblid.Text + " ");
                if (Operation.ExecuteTransaction(QueriesDel, Operation.Conn))
                //  Operation.UserLog(query, this.Name, Operation.Rights.DELETE.ToString());
                {
                    MessageBox.Show("Record Deleted Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblid.Text = "0";
                    btnAdd_Click(sender, e);
                    btnDelete.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Error While Deleting Record" , Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
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

        private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                if (dgvMenu.Rows[dgvMenu.CurrentRow.Index].DefaultCellStyle.BackColor == Color.White)
                {
                    dgvMenu.Rows[e.RowIndex].Cells["Select"].Value = true;
                    dgvMenu.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;

                }
                else
                {
                    if (dgvMenu.Rows[e.RowIndex].Cells["Link"].Value != null)
                    {
                        if (MessageBox.Show("Are you sure you want to remove this menu?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            //Queries.Add("delete from ClientMenuMaster Where ClientMenuID = " + dgvMenu.Rows[e.RowIndex].Cells["Link"].Value + " ");
                            //Queries.Add("delete from ClientUserMenuMaster Where ClientMenuID = " + dgvMenu.Rows[e.RowIndex].Cells["Link"].Value + " ");
                            //Queries.Add("delete from ClientDashBoardMaster Where ClientID = " + lblid.Text + " ");
                            //Queries.Add("delete from ClientChildMenuMaster Where ClientID = " + lblid.Text + " ");

                            dgvMenu.Rows[e.RowIndex].Cells["Select"].Value = false;
                            dgvMenu.Rows[dgvMenu.CurrentRow.Index].DefaultCellStyle.BackColor = Color.White;
                        }

                    }

                    else
                    {
                        dgvMenu.Rows[e.RowIndex].Cells["Select"].Value = false;
                        dgvMenu.Rows[dgvMenu.CurrentRow.Index].DefaultCellStyle.BackColor = Color.White;

                    }

                }
            }
        }
        private void BindMenuGrid()
        {
            dgvMenu.Rows.Clear();
            string Q;
            DataTable dt = new DataTable();
            //Q = "SELECT  MenuMaster.MenuName,    MenuMaster.SrNo, MenuMaster.HasGraph,MenuMaster.HasChildMenu,   ClientMenu.ClientMenuId,    ClientMenu.ClientID,    MenuMaster.MenuID,If(Isnull(ClientMenu.HasChildData),MenuMaster.HasChildData,ClientMenu.HasChildData) as HasChildData,If(Isnull(ClientMenu.IsDashboard),MenuMaster.IsDashboard,ClientMenu.IsDashboard) as IsDashboard," +
            //        "If(isnull(ClientMenu.Query),MenuMaster.Query,ClientMenu.Query) as Query, IF(ISNULL(ClientMenu.ClientId), 0, 1) AS AllocatedToClient FROM  (Select * From ClientMenuMaster where ClientMenuMaster.ClientId = " + lblid.Text + ") as ClientMenu right JOIN   MenuMaster ON ClientMenu.MenuID = MenuMaster.MenuID ";
            //Q = "SELECT  MenuMaster.MenuName,MenuMaster.ZoomQuery,    MenuMaster.SrNo, MenuMaster.HasGraph,MenuMaster.HasChildMenu,   ClientMenu.ClientMenuId,    ClientMenu.ClientID,    MenuMaster.MenuID,  MenuMaster.HasChildData,    MenuMaster.IsDashboard ,   MenuMaster.Query, IF(ISNULL(ClientMenu.ClientId), 0, 1) AS AllocatedToClient FROM  (Select * From ClientMenuMaster where ClientMenuMaster.ClientId = " + lblid.Text + ") as ClientMenu right JOIN   MenuMaster ON ClientMenu.MenuID = MenuMaster.MenuID ";
            Q = "SELECT  MenuMaster.MenuName,MenuMaster.ZoomQuery,    MenuMaster.SrNo, MenuMaster.HasGraph,MenuMaster.HasChildMenu,   ClientMenu.ClientMenuId,    ClientMenu.ClientID,    MenuMaster.MenuID,  MenuMaster.HasChildData,    MenuMaster.IsDashboard ,   MenuMaster.Query, CASE WHEN ISNULL(ClientMenu.ClientId,0)=0 THEN 0 ELSE 1 END AS AllocatedToClient FROM  (Select * From ClientMenuMaster where ClientMenuMaster.ClientId = " + lblid.Text + ") as ClientMenu right JOIN   MenuMaster ON ClientMenu.MenuID = MenuMaster.MenuID ";

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
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["ZoomQuery"].Value = dt.Rows[i]["ZoomQuery"];
                  
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].DefaultCellStyle.BackColor = Color.White;
                    if (dt.Rows[i]["HasChildData"].ToString() == "1")
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["HasChildData"].Value = true;
                    else
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["HasChildData"].Value = false;
                    if (dt.Rows[i]["HasGraph"].ToString() == "1")
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["ChkHasGraph"].Value = true;
                    else
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["ChkHasGraph"].Value = false;
                    if (dt.Rows[i]["HasChildMenu"].ToString() == "1")
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["ChkHasSubMenu"].Value = true;
                    else
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["ChkHasSubMenu"].Value = false;

                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["HasDashboard"].Value = Convert.ToBoolean(dt.Rows[i]["IsDashboard"]);
                    if (dt.Rows[i]["AllocatedToClient"].ToString() == "1")
                    {
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["select"].Value = true;
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Green;
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["Link"].Value = dt.Rows[i]["ClientMenuID"];

                    }
                    else
                    {
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["select"].Value = false;
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].DefaultCellStyle.BackColor = Color.White;
                    }
                    //if (i == 1)
                    //    dgvMenu.Columns[i].ReadOnly = false;
                }
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

        private void btnValidUserEntry_Click(object sender, EventArgs e)
        {
            DataTable dtComp = Operation.GetDataTable("Select * from ClientMaster where Clientid=" + lblid.Text, Operation.Conn);
            if (dtComp.Rows.Count > 0)
            {
                if (Convert.ToInt32(txtNoUser.Text) < Convert.ToInt32(dtComp.Rows[0]["TotalCreatedUser"]))
                {
                    picUser.Visible = true;
                    picUser.Image = ExpertMobileSystem.Properties.Resources.close;
                }
                else
                {
                    picUser.Visible = true;
                    picUser.Image = ExpertMobileSystem.Properties.Resources.accept;
                }
            }
            //txtNoUser.Text = dt.Rows[0]["NoOfAccessUsers"].ToString();
            //        txtNoComp.Text = dt.Rows[0]["NoOfCompanyPerUser"].ToString();

        }

        private void btnValidCompanyEntry_Click(object sender, EventArgs e)
        {
            DataTable dtComp = Operation.GetDataTable("Select * from ClientMaster where Clientid=" + lblid.Text, Operation.Conn);
            if (dtComp.Rows.Count > 0)
            {
                if (Convert.ToInt32(txtNoComp.Text) < Convert.ToInt32(dtComp.Rows[0]["TotalCreatedCompany"]))
                {
                    picCompany.Visible = true;
                    picCompany.Image = ExpertMobileSystem.Properties.Resources.close;
                }
                else
                {
                    picCompany.Visible = true;
                    picCompany.Image = ExpertMobileSystem.Properties.Resources.accept;
                }
            }
        }

        private void txtUsername_Validated(object sender, EventArgs e)
        {
            object name = Operation.ExecuteScalar("Select count(*) from ClientMaster where UserName = '" + txtUsername.Text + "'", Operation.Conn);
            if (name.ToString() != "0")
            {
                MessageBox.Show("User Name:'" + txtUsername.Text + "' Already Registered.Please Enter Another One.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsername.Focus();
                return;
            }
        }

        private void txtmobile_Validated(object sender, EventArgs e)
        {
            //object mobi = Operation.ExecuteScalar("Select count(*) from ClientMaster where MobileNo = '" + txtmobile.Text + "'", Operation.Conn);
            //if (mobi.ToString() != "0")
            //{
            //    MessageBox.Show("Mobile Number:'" + txtmobile.Text + "' Already Exist.Please Enter Another One.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    txtmobile.Focus();
            //    return;
            //}
        }

        private void txtemail_Validated(object sender, EventArgs e)
        {
            object email = Operation.ExecuteScalar("Select count(*) from ClientMaster where Email = '" + txtemail.Text + "'", Operation.Conn);
            if (email.ToString() != "0")
            {
                MessageBox.Show("EmailID:'" + txtemail.Text + "' Already Registered.Please Enter Another One.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtemail.Focus();
                return;
            }
        }

        private void txtCompanyname_Validated(object sender, EventArgs e)
        {
            object Com = Operation.ExecuteScalar("Select count(*) from ClientMaster where CompanyName = '" + txtCompanyname.Text + "'", Operation.Conn);
            if (Com != null && Com.ToString() != "0")
            {
                MessageBox.Show("CompanyName:'" + txtCompanyname.Text + "'  Already Registered.Please Enter Another One.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCompanyname.Focus();
                return;
            }
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }


    }
}
