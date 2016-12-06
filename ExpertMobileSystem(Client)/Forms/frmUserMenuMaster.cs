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
    public partial class frmUserMenuMaster : Form
    {
        ArrayList Queries = new ArrayList();
        public frmUserMenuMaster()
        {
            InitializeComponent();
            Paint += draw;
            Invalidate();
            try
            { this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\MOBILE.ico"); }
            catch { }
        }
        private void txtmobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)8))
                e.Handled = true;
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
            try
            {
                BindMenuGrid();
                BindUserCompanyGrid();
                btnAdd_Click(sender, e);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void BindUserCompanyGrid()
        {
            DataTable dt = Operation.GetDataTable("SELECT ClientUserCompanyMaster.ClientUserCompanyid,  FirstName, LastName,CompanyName, CompanyFromDate, CompanyToDate, ExpertPath,  CompanyCode " +
                                                    " FROM ClientUserCompanyMaster INNER JOIN  ClientCompanyMaster ON ClientUserCompanyMaster.ClientCompanyid = ClientCompanyMaster.ClientCompanyid " +
                                                    " inner join ClientUserMaster on ClientUserCompanyMaster.ClientUserid = ClientUserMaster.ClientUserid where ClientUserCompanyMaster.Clientid  = " + Operation.Clientid + "", Operation.Conn);
          
            
            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = " FirstName, LastName,CompanyName, CompanyFromDate, CompanyToDate";
                dt = dt.DefaultView.ToTable(false);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvUserCompany.Rows.Add();
                    dgvUserCompany.Rows[i].Cells["UserSelect"].Value = false;

                    dgvUserCompany.Rows[i].Cells["ClientUserCompanyid"].Value = dt.Rows[i]["ClientUserCompanyid"];
                    dgvUserCompany.Rows[i].Cells["FirstName"].Value = dt.Rows[i]["FirstName"];
                    dgvUserCompany.Rows[i].Cells["LastName"].Value = dt.Rows[i]["LastName"];
                    dgvUserCompany.Rows[i].Cells["CompanyName"].Value = dt.Rows[i]["CompanyName"];
                    dgvUserCompany.Rows[i].Cells["FromDate"].Value = dt.Rows[i]["CompanyFromDate"];
                    dgvUserCompany.Rows[i].Cells["FromDate"].Value = dt.Rows[i]["CompanyFromDate"];
                    dgvUserCompany.Rows[i].Cells["ToDate"].Value = dt.Rows[i]["CompanyToDate"];
                    dgvUserCompany.Rows[i].Cells["Connection"].Value = Operation.createCompanyConn(dt.Rows[i]["ExpertPath"].ToString(), dt.Rows[i]["CompanyCode"].ToString());

                }
              // dgvUserCompany.Sort("FirstName", ListSortDirection.Ascending); 
            }

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
        private void DisableQuery()
        {
            if (Operation.objComp.QueryRights == true)
            {
                dgvDetail.Columns["DetailMenuQuery"].Visible = true;
                dgvDetail.Columns["DetailQuery"].Visible = true;
                dgvDetail.Columns["Execute"].Visible = true;
                dgvDetail.Columns["DetailZoomQuery"].Visible = true;
                dgvDetail.Columns["MasterZoom"].Visible = true;


                dgvMenu.Columns["Query"].Visible = true;
                dgvMenu.Columns["ZoomQuery"].Visible = true;

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            Queries.Clear();
            lblid.Text = "0";
            dgvDetail.Rows.Clear();
            FetchDetailGrid();
            DisableQuery();


            // txtmobile.Text = "";
        }
        private bool CheckForDashboard()
        {
            int userindex = 0, detailindex = 0;
            for (int i = 0; i < dgvUserCompany.Rows.Count; i++)
            {
                bool isexist = false;
                for (int j = 0; j < dgvDetail.Rows.Count; j++)
                {
                    if (dgvUserCompany.Rows[i].Cells["ClientUserCompanyID"].Value.ToString() == dgvDetail.Rows[j].Cells["DetailClientUserCompanyID"].Value.ToString() && dgvDetail.Rows[j].Cells["IsDash"].Value.ToString() == "True")
                    {
                        isexist = true;

                    }
                    else
                    {

                        userindex = i;
                    }
                }
                if (isexist == false)
                {
                    MessageBox.Show("Dashboard Is Not Assigned For '" + dgvUserCompany.Rows[userindex].Cells["FirstName"].Value + "' and '" + dgvUserCompany.Rows[userindex].Cells["CompanyName"].Value + "'.\nAssign Dashboard First.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvDetail.Rows.Count == 0 && Queries.Count == 0)
                return;
            else
            {
                btnSave.Enabled = false;
                try
                {

                    if (!CheckForDashboard())
                        return;

                    for (int i = 0; i < dgvDetail.Rows.Count; i++)
                    {
                        string zoomQ = "", FilterQuery = "";

                        // zoomQ = dgvDetail.Rows[i].Cells["DetailZoomQuery"].Value.ToString();
                        FilterQuery = (dgvDetail.Rows[i].Cells["DetailQuery"].Value == null ? "" : dgvDetail.Rows[i].Cells["DetailQuery"].Value.ToString());

                        // string q = txtQuery.Text, zoomq = txtZoomQuery.Text;
                        // zoomQ = (zoomQ.Trim().Replace("\\", "\\\\")).Replace("'", "\\'");

                        FilterQuery = (FilterQuery.Trim().Replace("\\", "\\\\")).Replace("'", "\\'");

                        if (dgvDetail.Rows[i].Cells["DetailLink"].Value == null)
                        {
                            Queries.Add("Insert into ClientUserMenuMaster (ClientUserCompanyID,ClientMenuID,FilterQuery,FilterZoomQuery) Values (" + dgvDetail.Rows[i].Cells["DetailClientUserCompanyID"].Value + "," + dgvDetail.Rows[i].Cells["DetailClientMenuID"].Value + ",'" + FilterQuery + "','" + zoomQ + "')");
                        }
                        else if (dgvDetail.Rows[i].Cells["DetailQuery"].Value != null && dgvDetail.Rows[i].Cells["DetailLink"].Value != null)
                        {
                            Queries.Add("Update ClientUserMenuMaster set FilterQuery = '" + FilterQuery + "',FilterZoomQuery='" + zoomQ + "' where ClientUserMenuID = " + dgvDetail.Rows[i].Cells["DetailLink"].Value + " ");
                        }
                    }
                    if (Operation.ExecuteTransaction(Queries, Operation.Conn))
                    {
                        MessageBox.Show("Record Saved Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        btnAdd_Click(sender, e);


                    }
                    else
                    {
                        MessageBox.Show("Error while Saving, Please Try after Some Time.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnAdd_Click(null, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Queries.Clear();
                    btnSave.Enabled = true;
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    btnSave.Enabled = true;
                }
            }
        }
        private bool InsertDashMenu(ArrayList ar, string ClientID)
        {
            bool ans = false;

            //for (int i = 0; i < dgvDash.Rows.Count; i++)
            //{
            //    if (dgvDash.Rows[i].DefaultCellStyle.BackColor == Color.Blue)
            //    {
            //        if (dgvDash.Rows[i].Cells["DashLink"].Value == null)
            //            ar.Add("Insert into ClientDashBoardMaster (ClientID,Query,DashBoardID) values (" + ClientID + ",'" + dgvDash.Rows[i].Cells["DashQuery"].Value + "'," + dgvDash.Rows[i].Cells["DashBoardID"].Value + "  )");
            //        ans = true;
            //    }
            //}
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
                    //txtCompanyname.Text = dt.Rows[0]["CompanyName"].ToString();
                    //txtadress.Text = dt.Rows[0]["CompanyAddress"].ToString();
                    //txtmobile.Text = dt.Rows[0]["MobileNo"].ToString();
                    //txtemail.Text = dt.Rows[0]["Email"].ToString();
                    //txtFirstname.Text = dt.Rows[0]["FirstName"].ToString();
                    //txtLastname.Text = dt.Rows[0]["LastName"].ToString();
                    //txtUsername.Text = dt.Rows[0]["UserName"].ToString();
                    //txtPassword.Text = Operation.Decryptdata(dt.Rows[0]["Password"].ToString());
                    //txtrepass.Text = txtPassword.Text;
                    //txtNoUser.Text = dt.Rows[0]["NoOfAccessUsers"].ToString();
                    //txtNoComp.Text = dt.Rows[0]["NoOfCompanyPerUser"].ToString();
                    //dtpdate.Value = Convert.ToDateTime(dt.Rows[0]["AccountExpiredOn"]);
                    //BindMenuGrid();
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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                if (e.ColumnIndex == 1)
                {
                    if (dgvMenu.Rows[e.RowIndex].Cells["Select"].Value.ToString() == "False")
                    {
                        dgvMenu.Rows[e.RowIndex].Cells["Select"].Value = true;
                        dgvMenu.Rows[dgvMenu.CurrentRow.Index].DefaultCellStyle.BackColor = Color.Green;

                    }
                    else
                    {

                        dgvMenu.Rows[e.RowIndex].Cells["Select"].Value = false;
                        dgvMenu.Rows[dgvMenu.CurrentRow.Index].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //private void BindSubMenu(string index)
        //{
        //    dgvSubMenu.DataSource = null;
        //    dgvSubMenu.Rows.Clear();

        //    //string q = "select DashboardID, MenuID, DashboardName, Type, Query from  DashboardMaster where MenuID = " + dgvMenu.Rows[index].Cells["MenuID"].Value + " ";
        //    string q = "SELECT  Userchild.ClientUserChildMenuID,    ClientChildMenuMaster.*,  ChildMenuMaster.HasChildData,ChildMenuMaster.HasGraph,ChildMenuMaster.ChildMenuName,    " +
        //                " IF(ISNULL(Userchild.ClientUserChildMenuID),  0,   1) AS AllocatedChild,  ChildMenuMaster.Query FROM " +
        //                "    (SELECT    *   FROM     ClientUserChildMenuMaster  )" +
        //                "    AS Userchild   RIGHT JOIN   ClientChildMenuMaster ON Userchild.ClientChildMenuID = ClientChildMenuMaster.ClientChildMenuID" +
        //                "  inner join ChildMenuMaster on  ClientChildMenuMaster.ChildMenuID = ChildMenuMaster.ChildMenuID " +
        //                "  where ClientChildMenuMaster.ClientID = "+index+"";
        //    DataTable dt = Operation.GetDataTable(q, Operation.Conn);
        //  //  dgvSubMenu.ReadOnly = true;
        //    dgvSubMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            dgvSubMenu.Rows.Add();

        //            //   dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["DashLink"].Value = dt.Rows[i]["DashboardID"];
        //            dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["ClientChildMenuID"].Value = dt.Rows[i]["ClientChildMenuID"];
        //            dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuName"].Value = dt.Rows[i]["ChildMenuName"];
        //            dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuQuery"].Value = dt.Rows[i]["Query"];
        //            dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuHasChild"].Value = Convert.ToBoolean(dt.Rows[i]["HasChildData"]);
        //            dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuHasGraph"].Value = Convert.ToBoolean(dt.Rows[i]["HasGraph"]);


        //            if (dt.Rows[i]["AllocatedChild"].ToString() == "1")
        //            {
        //                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuSelect"].Value = true;
        //                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].DefaultCellStyle.BackColor = Color.SandyBrown;
        //                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuLink"].Value = dt.Rows[i]["ClientUserChildMenuID"];
        //            }
        //            else
        //            {
        //                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuSelect"].Value = false;
        //                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].DefaultCellStyle.BackColor = Color.White;
        //            }
        //        }
        //    }

        //}
        private void BindMenuGrid()
        {

            string Q;
            DataTable dt = new DataTable();
            Q = "SELECT  ClientMenuID,  MenuMaster.MenuName,MenuMaster.ZoomQuery,MenuMaster.SrNo, MenuMaster.HasGraph,MenuMaster.HasChildMenu, MenuMaster.Query,MenuMaster.HasChildData, MenuMaster.IsDashboard " +
                " FROM   ClientMenuMaster    INNER JOIN MenuMaster ON ClientMenuMaster.MenuID = MenuMaster.MenuID " +
                " WHERE    ClientMenuMaster.ClientID = " + Operation.Clientid + "";
            dt = Operation.GetDataTable(Q, Operation.Conn);
            //dgvMenu.ReadOnly = true;
            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = " SrNo";
                dt = dt.DefaultView.ToTable(false);


                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    dgvMenu.Rows.Add();
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["Select"].Value = false;

                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["ClientMenuID"].Value = dt.Rows[i]["ClientMenuID"];
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["MenuName"].Value = dt.Rows[i]["MenuName"];
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["Query"].Value = dt.Rows[i]["Query"];
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].DefaultCellStyle.BackColor = Color.White;
                    if (dt.Rows[i]["HasChildData"].ToString() == "1")
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["HasChildData"].Value = true;
                    else
                        dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["HasChildData"].Value = false;
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["HasDashboard"].Value = Convert.ToBoolean(dt.Rows[i]["IsDashboard"]);
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["HasGraph"].Value = Convert.ToBoolean(dt.Rows[i]["HasGraph"]);
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["HasSubMenu"].Value = Convert.ToBoolean(dt.Rows[i]["HasChildMenu"]);
                    dgvMenu.Rows[dgvMenu.Rows.Count - 1].Cells["ZoomQuery"].Value = dt.Rows[i]["ZoomQuery"];

                }
            }
        }


        private void btndeselectuser_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvUserCompany.Rows.Count; i++)
            {
                dgvUserCompany.Rows[i].DefaultCellStyle.BackColor = Color.White;
                dgvUserCompany.Rows[i].Cells["UserSelect"].Value = false;
            }
        }

        private void btnselectUser_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvUserCompany.Rows.Count; i++)
            {
                dgvUserCompany.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                dgvUserCompany.Rows[i].Cells["UserSelect"].Value = true;
            }
        }

        private void btnAssignMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if ((!IsSelectedRows(dgvUserCompany)) || (!IsSelectedRows(dgvMenu)))
                {
                    MessageBox.Show("Please Select Atleast One UserCopany And Menu Item.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                FillDetailGrid();
                btndeselectuser_Click(null, null);
                btnUnselect_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FillDetailGrid()
        {
            bool isexist = false;
            for (int i = 0; i < dgvUserCompany.Rows.Count; i++)
            {
                for (int j = 0; j < dgvMenu.Rows.Count; j++)
                {
                    if (dgvUserCompany.Rows[i].Cells["UserSelect"].Value.ToString() == "True" && dgvMenu.Rows[j].Cells["Select"].Value.ToString() == "True")
                    {
                        for (int d = 0; d < dgvDetail.Rows.Count; d++)
                        {
                            if (dgvDetail.Rows[d].Cells["DetailClientUserCompanyID"].Value.ToString() == dgvUserCompany.Rows[i].Cells["ClientUserCompanyID"].Value.ToString() && dgvDetail.Rows[d].Cells["DetailClientMenuID"].Value.ToString() == dgvMenu.Rows[j].Cells["ClientMenuID"].Value.ToString())
                            {
                                isexist = true;
                                break;

                            }

                        }
                        if (isexist == false)
                        {
                            dgvDetail.Rows.Add();
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailClientUserCompanyID"].Value = dgvUserCompany.Rows[i].Cells["ClientUserCompanyID"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailFirstName"].Value = dgvUserCompany.Rows[i].Cells["FirstName"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailLastName"].Value = dgvUserCompany.Rows[i].Cells["LastName"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailCompanyName"].Value = dgvUserCompany.Rows[i].Cells["CompanyName"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailToDate"].Value = dgvUserCompany.Rows[i].Cells["ToDate"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailConn"].Value = dgvUserCompany.Rows[i].Cells["Connection"].Value;

                            //menu details
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailClientMenuID"].Value = dgvMenu.Rows[j].Cells["ClientMenuID"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailMenuName"].Value = dgvMenu.Rows[j].Cells["MenuName"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailMenuQuery"].Value = dgvMenu.Rows[j].Cells["Query"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["IsDash"].Value = dgvMenu.Rows[j].Cells["HasDashBoard"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["MasterZoom"].Value = dgvMenu.Rows[j].Cells["ZoomQuery"].Value;

                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].DefaultCellStyle.BackColor = Color.SkyBlue;
                            //dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["IsExecuted"].Value = false;
                        }
                        else
                        {
                            //dgvMenu.Rows[j].Cells["Select"].Value = false;
                            //dgvMenu.Rows[j].DefaultCellStyle.BackColor = Color.White;
                            isexist = false;
                        }


                    }
                }
            }
            SetSrNo();



        }
        private void SetSrNo()
        {
            int c = 1;
            for (int i = 0; i < dgvDetail.Rows.Count; i++)
            {
                dgvDetail.Rows[i].Cells["DetailSrNo"].Value = c;
                c++;

            }

        }

        private bool IsSelectedRows(DataGridView dgv)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].DefaultCellStyle.BackColor == Color.Green)
                {
                    return true;
                }

            }
            return false;
        }

        private void dgvUserCompany_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.ColumnIndex == 1)
                {
                    if (dgvUserCompany.Rows[e.RowIndex].Cells["UserSelect"].Value.ToString() == "False")
                    {
                        dgvUserCompany.Rows[e.RowIndex].Cells["UserSelect"].Value = true;
                        dgvUserCompany.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
                    }
                    else
                    {
                        dgvUserCompany.Rows[e.RowIndex].Cells["UserSelect"].Value = false;
                        dgvUserCompany.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvDetail.Columns["Delete"].Index)
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (dgvDetail.Rows[e.RowIndex].Cells["DetailLink"].Value != null)
                            Queries.Add("delete from ClientUserMenuMaster Where ClientUserMenuid = " + dgvDetail.Rows[e.RowIndex].Cells["Detaillink"].Value + " ");
                        dgvDetail.Rows.RemoveAt(e.RowIndex);// dgvDetail.Rows[e.RowIndex].
                        SetSrNo();

                    }
                }
                else if (e.ColumnIndex == dgvDetail.Columns["Execute"].Index)
                {
                    try
                    {
                        if (dgvDetail.Rows[e.RowIndex].Cells["DetailQuery"].Value == null || dgvDetail.Rows[e.RowIndex].Cells["DetailQuery"].Value.ToString() == "")
                        {
                            //  MessageBox.Show("No Query Found.\nExecuting Query from MENU.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LocalConnection.tempQuery = dgvDetail.Rows[e.RowIndex].Cells["DetailMenuQuery"].Value.ToString();
                        }
                        else
                        {
                            LocalConnection.tempQuery = dgvDetail.Rows[e.RowIndex].Cells["DetailQuery"].Value.ToString();
                        }
                        frmQueryExecutor obj = new frmQueryExecutor();
                        Operation.gViewQuery = "";
                        System.Data.OleDb.OleDbConnection tempconn = new System.Data.OleDb.OleDbConnection(dgvDetail.Rows[e.RowIndex].Cells["DetailConn"].Value.ToString());
                        obj.dgvresult.DataSource = null;
                        DataTable dt = LocalConnection.GetDataTable(LocalConnection.tempQuery, tempconn);
                        obj.dgvresult.DataSource = dt;
                        obj.dgvresult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                        obj.ShowDialog();
                    }
                    catch
                    {
                        MessageBox.Show("Error Occured, Please Check Query you Have Entered.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FetchDetailGrid()
        {
            string q = "SELECT    ClientUserMenuMaster.ClientUserMenuID,ClientUserMenuMaster.ClientMenuID,ClientUserMenuMaster.FilterZoomQuery,   ClientUserMenuMaster.ClientUserCompanyID,ClientCompanyMaster.CompanyCode,ClientCompanyMaster.ExpertPath,       ClientUserMaster.FirstName,    ClientUserMaster.LastName,    CompanyName,    CompanyToDate,    MenuName,   ClientUserMenuMaster.FilterQuery ,MenuMaster.IsDashboard ,MenuMaster.Query,    MenuMaster.ZoomQuery as MasterZoom ,  ClientUserMenuMaster.FilterZoomQuery as ZoomQuery FROM " +
                        " ClientUserMenuMaster   INNER JOIN   ClientUserCompanyMaster ON ClientUserMenuMaster.ClientUserCompanyID = ClientUserCompanyMaster.ClientUserCompanyid     INNER JOIN " +
                        " ClientMenuMaster ON ClientUserMenuMaster.ClientMenuID = ClientMenuMaster.ClientMenuID   INNER JOIN " +
                        " MenuMaster ON ClientMenuMaster.MenuID = MenuMaster.MenuID   INNER JOIN " +
                        "   ClientCompanyMaster  ON ClientUserCompanyMaster.ClientCompanyId = ClientCompanyMaster.ClientCompanyId   " +
                        "   INNER JOIN    ClientUserMaster ON ClientUserCompanyMaster.ClientUserid = ClientUserMaster.ClientUserid " +
                        "   where ClientCompanyMaster.ClientId = " + Operation.Clientid + "" +
                        "	ORDER BY ClientUserMenuID ASC";
            DataTable dt = Operation.GetDataTable(q, Operation.Conn);
            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = " FirstName, LastName,CompanyName, CompanyToDate,MenuName";
                dt = dt.DefaultView.ToTable(false);


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvDetail.Rows.Add();
                    dgvDetail.Rows[i].Cells["DetailLink"].Value = dt.Rows[i]["ClientUserMenuID"];
                    //User And Company Detail
                    dgvDetail.Rows[i].Cells["DetailClientUserCompanyID"].Value = dt.Rows[i]["ClientUserCompanyID"];
                    dgvDetail.Rows[i].Cells["DetailFirstName"].Value = dt.Rows[i]["FirstName"];
                    dgvDetail.Rows[i].Cells["DetailLastName"].Value = dt.Rows[i]["LastName"];
                    dgvDetail.Rows[i].Cells["DetailCompanyName"].Value = dt.Rows[i]["CompanyName"];
                    dgvDetail.Rows[i].Cells["DetailToDate"].Value = dt.Rows[i]["CompanyToDate"];
                    //menu details
                    dgvDetail.Rows[i].Cells["DetailClientMenuID"].Value = dt.Rows[i]["ClientMenuID"];
                    dgvDetail.Rows[i].Cells["IsDash"].Value = Convert.ToBoolean(dt.Rows[i]["IsDashboard"]);
                    dgvDetail.Rows[i].Cells["DetailMenuName"].Value = dt.Rows[i]["MenuName"];
                    dgvDetail.Rows[i].Cells["DetailMenuQuery"].Value = dt.Rows[i]["Query"];
                    dgvDetail.Rows[i].Cells["DetailQuery"].Value = dt.Rows[i]["FilterQuery"];
                    dgvDetail.Rows[i].Cells["DetailZoomQuery"].Value = dt.Rows[i]["ZoomQuery"];
                    dgvDetail.Rows[i].Cells["MasterZoom"].Value = dt.Rows[i]["MasterZoom"];

                    dgvDetail.Rows[i].Cells["DetailConn"].Value = Operation.createCompanyConn(dt.Rows[i]["ExpertPath"].ToString(), dt.Rows[i]["CompanyCode"].ToString());


                }
                SetSrNo();
            }

        }

        private void brnDashSelectAll_Click(object sender, EventArgs e)
        {

        }

        //private void dgvSubMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == 2)
        //    {
        //        if (dgvSubMenu.Rows[dgvSubMenu.CurrentRow.Index].DefaultCellStyle.BackColor == Color.White)
        //        {
        //            dgvSubMenu.Rows[e.RowIndex].Cells["SubMenuSelect"].Value = true;
        //            dgvSubMenu.Rows[dgvSubMenu.CurrentRow.Index].DefaultCellStyle.BackColor = Color.SandyBrown;
        //        }
        //        else
        //        {
        //            if (dgvSubMenu.Rows[e.RowIndex].Cells["SubMenuLink"].Value != null)
        //            {
        //                if (MessageBox.Show("Are you sure you want to remove this menu?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //                {
        //                    Queries.Add("delete from ClientUserChildMenuMaster Where ClientUserChildMenuID = " + dgvSubMenu.Rows[e.RowIndex].Cells["SubMenuLink"].Value + " ");
        //                    //  Queries.Add("delete from ClientUserMenuMaster Where ClientMenuID = " + dgvSubMenu.Rows[e.RowIndex].Cells["Link"].Value + " ");
        //                }
        //            }
        //            dgvSubMenu.Rows[e.RowIndex].Cells["SubMenuSelect"].Value = false;
        //            dgvSubMenu.Rows[dgvSubMenu.CurrentRow.Index].DefaultCellStyle.BackColor = Color.White;
        //        }
        //    }
        //}



    }
}
