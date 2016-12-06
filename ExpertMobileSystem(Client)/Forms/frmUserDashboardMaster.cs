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
    public partial class frmUserDashboardMaster : Form
    {
        ArrayList Queries = new ArrayList();
        public frmUserDashboardMaster()
        {
            InitializeComponent();
            try
            { this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\MOBILE.ico"); }
            catch { }
        }
        private void txtmobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)8))
                e.Handled = true;
        }
        private void frmPartymaster_Load(object sender, EventArgs e)
        {
            try
            {
                Paint += draw;
                Invalidate();
                BindDashGrid();
                BindDetailGrid();
                btnAdd_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

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
        private void BindDetailGrid()
        {
            string q = "SELECT    ClientUserMenuMaster.ClientUserMenuID,ClientUserMenuMaster.ClientMenuID,   ClientUserMenuMaster.ClientUserCompanyID,ExpertPath,CompanyCode,       ClientUserMaster.FirstName,    ClientUserMaster.LastName,    CompanyName,     MenuName    FROM " +
                     " ClientUserMenuMaster   INNER JOIN   ClientUserCompanyMaster ON ClientUserMenuMaster.ClientUserCompanyID = ClientUserCompanyMaster.ClientUserCompanyid     INNER JOIN " +
                     " ClientMenuMaster ON ClientUserMenuMaster.ClientMenuID = ClientMenuMaster.ClientMenuID   INNER JOIN " +
                     " MenuMaster ON ClientMenuMaster.MenuID = MenuMaster.MenuID   INNER JOIN " +
                     "   ClientCompanyMaster  ON ClientUserCompanyMaster.ClientCompanyId = ClientCompanyMaster.ClientCompanyId   " +
                     "   INNER JOIN    ClientUserMaster ON ClientUserCompanyMaster.ClientUserid = ClientUserMaster.ClientUserid " +
                     " where MenuMaster.IsDashboard = 1 and ClientMenuMaster.ClientID = " + Operation.Clientid + "" +
                     "	ORDER BY ClientUserMenuID ASC";
            DataTable dt = Operation.GetDataTable(q, Operation.Conn);
            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = " FirstName,LastName,CompanyName,MenuName";
                dt = dt.DefaultView.ToTable(false);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvDetail.Rows.Add();
                    //User And Company Detail
                    dgvDetail.Rows[i].Cells["ClientUserMenuID"].Value = dt.Rows[i]["ClientUserMenuID"];

                    dgvDetail.Rows[i].Cells["Select"].Value = false;
                    dgvDetail.Rows[i].Cells["DetailClientUserCompanyID"].Value = dt.Rows[i]["ClientUserCompanyID"];
                    dgvDetail.Rows[i].Cells["DetailFirstName"].Value = dt.Rows[i]["FirstName"];
                    dgvDetail.Rows[i].Cells["DetailLastName"].Value = dt.Rows[i]["LastName"];
                    dgvDetail.Rows[i].Cells["DetailCompanyName"].Value = dt.Rows[i]["CompanyName"];
                    dgvDetail.Rows[i].Cells["Connection"].Value = Operation.createCompanyConn(dt.Rows[i]["ExpertPath"].ToString(), dt.Rows[i]["CompanyCode"].ToString());

                    //menu details
                    dgvDetail.Rows[i].Cells["DetailClientMenuID"].Value = dt.Rows[i]["ClientMenuID"];
                    dgvDetail.Rows[i].Cells["DetailMenuName"].Value = dt.Rows[i]["MenuName"];
                    dgvDetail.Rows[i].DefaultCellStyle.BackColor = Color.White;
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
                dgvImformation.Columns["DGVDashQuery"].Visible = true;
                dgvImformation.Columns["DGVQuery"].Visible = true;
                dgvImformation.Columns["Execute"].Visible = true;
                dgvDash.Columns["DashQuery"].Visible = true;

            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Queries.Clear();
                lblid.Text = "0";
                dgvImformation.Rows.Clear();
                FetchUserChildGrid();
                DisableQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvImformation.Rows.Count == 0 && Queries.Count == 0)
                return;

            btnSave.Enabled = false;
            try
            {

                for (int i = 0; i < dgvImformation.Rows.Count; i++)
                {
                    string FilterQuery = "";

                    // zoomQ = dgvDetail.Rows[i].Cells["DetailZoomQuery"].Value.ToString();
                    FilterQuery = (dgvImformation.Rows[i].Cells["DGVQuery"].Value == null ? "" : dgvImformation.Rows[i].Cells["DGVQuery"].Value.ToString());

                    // string q = txtQuery.Text, zoomq = txtZoomQuery.Text;
                    //  zoomQ = (zoomQ.Trim().Replace("\\", "\\\\")).Replace("'", "\\'");

                    FilterQuery = (FilterQuery.Trim().Replace("\\", "\\\\")).Replace("'", "\\'");

                    if (dgvImformation.Rows[i].Cells["DGVLink"].Value == null)
                        Queries.Add("Insert into ClientUserDashBoardMaster (ClientDashBoardID,Query,ClientUserMenuID) Values (" + dgvImformation.Rows[i].Cells["DGVClientDashBoardID"].Value + ",'" + FilterQuery + "'," + dgvImformation.Rows[i].Cells["DGVClientUserMenuID"].Value + ")");
                    else if (dgvImformation.Rows[i].Cells["DGVQuery"].Value != null && dgvImformation.Rows[i].Cells["DGVLink"].Value != null)
                    {
                        Queries.Add("Update ClientUserDashBoardMaster set Query = '" + FilterQuery + "' where ClientUserDashBoardID = " + dgvImformation.Rows[i].Cells["DGVLink"].Value + " ");
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
                    Queries.Clear();
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

        private void btnselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvDash.Rows.Count; i++)
            {
                dgvDash.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                dgvDash.Rows[i].Cells["DashSelect"].Value = true;
            }
        }
        private void btnUnselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvDash.Rows.Count; i++)
            {
                dgvDash.Rows[i].DefaultCellStyle.BackColor = Color.White;
                dgvDash.Rows[i].Cells["DashSelect"].Value = false;
            }
        }
        private void BindDashGrid()
        {

            string Q;
            DataTable dt = new DataTable();
            Q = "SELECT ClientDashBoardMaster.ClientDashBoardID,ClientDashBoardMaster.ClientMenuID,DashboardMaster.type, " +
                    "    DashboardMaster.DashboardName,   DashboardMaster.Query,  MenuMaster.MenuName as ZoomMenu  FROM " +
                    "    ClientDashBoardMaster   INNER JOIN " +
                    "    DashboardMaster ON DashboardMaster.DashboardID = ClientDashBoardMaster.DashBoardID " +
                    "    inner join    ClientMenuMaster on ClientMenuMaster.ClientMenuID = ClientDashBoardMaster.ClientMenuID   inner join " +
                    "    MenuMaster on MenuMaster.MenuID = DashboardMaster.ZoomMenuId      " +
                    "    WHERE    ClientMenuMaster.ClientID = " + Operation.Clientid + "";
            dt = Operation.GetDataTable(Q, Operation.Conn);
            dgvDash.ReadOnly = true;
            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = " DashboardName,ZoomMenu,type";
                dt = dt.DefaultView.ToTable(false);
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    dgvDash.Rows.Add();
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashSelect"].Value = false;
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["ClientdashBoardID"].Value = dt.Rows[i]["ClientDashBoardID"];

                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["ClientMenuID"].Value = dt.Rows[i]["ClientMenuID"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashBoardName"].Value = dt.Rows[i]["DashBoardName"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashQuery"].Value = dt.Rows[i]["Query"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["Type"].Value = dt.Rows[i]["Type"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["ZoomMenu"].Value = dt.Rows[i]["ZoomMenu"];

                    dgvDash.Rows[dgvDash.Rows.Count - 1].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
        private void btndeselectuser_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvDetail.Rows.Count; i++)
            {
                dgvDetail.Rows[i].DefaultCellStyle.BackColor = Color.White;
                dgvDetail.Rows[i].Cells["Select"].Value = false;
            }
        }
        private void btnselectUser_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvDetail.Rows.Count; i++)
            {
                dgvDetail.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                dgvDetail.Rows[i].Cells["Select"].Value = true;
            }
        }
        private void btnAssignMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if ((!IsSelectedRows(dgvDetail)) || (!IsSelectedRows(dgvDash)))
                {
                    MessageBox.Show("Please Select Atleast One UserCopany And SubMenu Item.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            for (int i = 0; i < dgvDetail.Rows.Count; i++)
            {
                for (int j = 0; j < dgvDash.Rows.Count; j++)
                {
                    if (dgvDetail.Rows[i].Cells["Select"].Value.ToString() == "True" && dgvDash.Rows[j].Cells["dashSelect"].Value.ToString() == "True")
                    {
                        for (int d = 0; d < dgvImformation.Rows.Count; d++)
                        {
                            if (dgvImformation.Rows[d].Cells["DGVClientUserMenuID"].Value.ToString() == dgvDetail.Rows[i].Cells["ClientUserMenuID"].Value.ToString() && dgvImformation.Rows[d].Cells["DGVClientDashBoardID"].Value.ToString() == dgvDash.Rows[j].Cells["ClientDashBoardID"].Value.ToString())
                            {
                                isexist = true;
                                break;
                            }
                        }
                        if (isexist == false)
                        {
                            dgvImformation.Rows.Add();
                            dgvImformation.Rows[dgvImformation.Rows.Count - 1].Cells["DGVClientUserMenuID"].Value = dgvDetail.Rows[i].Cells["ClientUserMenuID"].Value;
                            dgvImformation.Rows[dgvImformation.Rows.Count - 1].Cells["DGVFirstName"].Value = dgvDetail.Rows[i].Cells["DetailFirstName"].Value;
                            dgvImformation.Rows[dgvImformation.Rows.Count - 1].Cells["DGVLastName"].Value = dgvDetail.Rows[i].Cells["DetailLastName"].Value;
                            dgvImformation.Rows[dgvImformation.Rows.Count - 1].Cells["DGVMenuName"].Value = dgvDetail.Rows[i].Cells["DetailMenuName"].Value;
                            dgvImformation.Rows[dgvImformation.Rows.Count - 1].Cells["DGVCompanyName"].Value = dgvDetail.Rows[i].Cells["DetailCompanyName"].Value;
                            dgvImformation.Rows[dgvImformation.Rows.Count - 1].Cells["DGVConn"].Value = dgvDetail.Rows[i].Cells["Connection"].Value;

                            //menu details
                            dgvImformation.Rows[dgvImformation.Rows.Count - 1].Cells["DGVClientDashBoardID"].Value = dgvDash.Rows[j].Cells["ClientDashboardID"].Value;
                            dgvImformation.Rows[dgvImformation.Rows.Count - 1].Cells["DGVDashBoardName"].Value = dgvDash.Rows[j].Cells["DashBoardName"].Value;
                            dgvImformation.Rows[dgvImformation.Rows.Count - 1].Cells["DGVDashQuery"].Value = dgvDash.Rows[j].Cells["DashQuery"].Value;
                            dgvImformation.Rows[dgvImformation.Rows.Count - 1].Cells["DGVType"].Value = dgvDash.Rows[j].Cells["Type"].Value;

                            dgvImformation.Rows[dgvImformation.Rows.Count - 1].DefaultCellStyle.BackColor = Color.SkyBlue;

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
            for (int i = 0; i < dgvImformation.Rows.Count; i++)
            {
                dgvImformation.Rows[i].Cells["DGVSrNo"].Value = c;
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
        private void FetchUserChildGrid()
        {
            string q = "SELECT   ClientUserDashBoardMaster.ClientUserDashBoardID, ClientUserDashBoardMaster.ClientUserMenuID, " +
                        "    ClientUserDashBoardMaster.ClientDashBoardID,   ClientUserMaster.FirstName, ClientUserMaster.LastName,ExpertPath,CompanyCode," +
                        "    MenuMaster.MenuName,       DashboardMaster.Query as dashQuery,    ClientUserDashBoardMaster.Query  AS Query," +
                        "    ClientCompanyMaster.CompanyName,   DashboardMaster.DashboardName,   DashboardMaster.Type, mnu.MenuName as ZoomMenu " +
                        "    FROM    ClientUserDashBoardMaster       INNER JOIN " +
                        "    ClientUserMenuMaster ON ClientUserMenuMaster.ClientUserMenuID = ClientUserDashBoardMaster.ClientUserMenuID " +
                        "    INNER JOIN    ClientUserCompanyMaster ON ClientUserMenuMaster.ClientUserCompanyID = ClientUserCompanyMaster.ClientUserCompanyid " +
                        "    INNER JOIN    ClientMenuMaster ON ClientUserMenuMaster.ClientMenuID = ClientMenuMaster.ClientMenuID " +
                        "    INNER JOIN    MenuMaster ON ClientMenuMaster.MenuID = MenuMaster.MenuID " +
                        "    INNER JOIN    ClientUserMaster ON ClientUserCompanyMaster.ClientUserid = ClientUserMaster.ClientUserid " +
                        "    INNER JOIN    ClientDashBoardMaster ON ClientDashBoardMaster.ClientDashBoardID = ClientUserDashBoardMaster.ClientDashBoardID " +
                        "    INNER JOIN    DashboardMaster ON DashboardMaster.DashboardID = ClientDashBoardMaster.DashBoardID " +
                        "    INNER JOIN    ClientCompanyMaster ON ClientCompanyMaster.ClientCompanyId = ClientUserCompanyMaster.ClientCompanyid " +
                        " inner join  MenuMaster as mnu on mnu.MenuID = DashboardMaster.ZoomMenuId     " +
                        "    where ClientUserMaster.ClientID = " + Operation.Clientid + " " +
                        "    ORDER BY ClientUserMenuID ASC ";
            DataTable dt = Operation.GetDataTable(q, Operation.Conn);
            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = " FirstName,LastName,MenuName,CompanyName,DashboardName";
                dt = dt.DefaultView.ToTable(false);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvImformation.Rows.Add();
                    //User And Company Detail
                    dgvImformation.Rows[i].Cells["DGVClientUserMenuID"].Value = dt.Rows[i]["ClientUserMenuID"];
                    dgvImformation.Rows[i].Cells["DGVLink"].Value = dt.Rows[i]["ClientUserDashBoardID"];

                    // dgvUserChild.Rows[i].Cells["DetailClientUserCompanyID"].Value = dt.Rows[i]["ClientUserCompanyID"];
                    dgvImformation.Rows[i].Cells["DGVFirstName"].Value = dt.Rows[i]["FirstName"];
                    dgvImformation.Rows[i].Cells["DGVLastName"].Value = dt.Rows[i]["LastName"];
                    dgvImformation.Rows[i].Cells["DGVMenuName"].Value = dt.Rows[i]["MenuName"];
                    dgvImformation.Rows[i].Cells["DGVCompanyName"].Value = dt.Rows[i]["CompanyName"];
                    //dashboard details
                    dgvImformation.Rows[i].Cells["DGVClientDashBoardID"].Value = dt.Rows[i]["ClientDashBoardID"];
                    dgvImformation.Rows[i].Cells["DGVDashBoardName"].Value = dt.Rows[i]["DashBoardName"];
                    dgvImformation.Rows[i].Cells["DGVDashQuery"].Value = dt.Rows[i]["dashQuery"];
                    dgvImformation.Rows[i].Cells["DGVQuery"].Value = dt.Rows[i]["Query"];

                    dgvImformation.Rows[i].Cells["DGVType"].Value = dt.Rows[i]["Type"];
                    dgvImformation.Rows[i].Cells["DGVZoomMenu"].Value = dt.Rows[i]["ZoomMenu"];

                    dgvImformation.Rows[i].Cells["DGVConn"].Value = Operation.createCompanyConn(dt.Rows[i]["ExpertPath"].ToString(), dt.Rows[i]["CompanyCode"].ToString());

                    dgvImformation.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
            SetSrNo();
        }
        private void dgvDetail_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string mnuid = (dgvDetail.Rows[e.RowIndex].Cells["DetailClientMenuID"].Value == null ? "0" : dgvDetail.Rows[e.RowIndex].Cells["DetailClientMenuID"].Value.ToString());
                DisplaySubemnu(mnuid);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DisplaySubemnu(string MenuID)
        {
            if (MenuID != "0")
                for (int i = 0; i < dgvDash.Rows.Count; i++)
                {
                    if (dgvDash.Rows[i].Cells["ClientMenuID"].Value.ToString() == MenuID)
                        dgvDash.Rows[i].Visible = true;
                    else
                        dgvDash.Rows[i].Visible = false;
                }
        }
        private void dgvSubMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    if (dgvDash.Rows[e.RowIndex].Cells["DashSelect"].Value.ToString() == "False")
                    {
                        dgvDash.Rows[e.RowIndex].Cells["dashSelect"].Value = true;
                        dgvDash.Rows[dgvDash.CurrentRow.Index].DefaultCellStyle.BackColor = Color.Green;

                    }
                    else
                    {

                        dgvDash.Rows[e.RowIndex].Cells["DashSelect"].Value = false;
                        dgvDash.Rows[dgvDash.CurrentRow.Index].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDetail_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.ColumnIndex == 1)
                {
                    if (dgvDetail.Rows[e.RowIndex].Cells["Select"].Value.ToString() == "False")
                    {
                        dgvDetail.Rows[e.RowIndex].Cells["Select"].Value = true;
                        dgvDetail.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
                    }
                    else
                    {
                        dgvDetail.Rows[e.RowIndex].Cells["Select"].Value = false;
                        dgvDetail.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvUserChild_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex ==  dgvImformation.Columns["Delete"].Index)
                {
                    if (dgvImformation.Rows[e.RowIndex].Cells["DGVLink"].Value != null)
                    {
                        if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            Queries.Add("delete from ClientUserDashBoardMaster Where ClientUserDashboardid = " + dgvImformation.Rows[e.RowIndex].Cells["DGVLink"].Value + " ");
                        else
                            return;
                    }
                    dgvImformation.Rows.RemoveAt(e.RowIndex);// dgvDetail.Rows[e.RowIndex].
                    SetSrNo();
                }
                else if (e.ColumnIndex ==  dgvImformation.Columns["Delete"].Index)                
                {
                    if (dgvImformation.Rows[e.RowIndex].Cells["DGVLink"].Value != null)
                    {
                        if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            Queries.Add("delete from ClientUserChildMenuMaster Where ClientUserChildMenuid = " + dgvImformation.Rows[e.RowIndex].Cells["DGVLink"].Value + " ");
                        else
                            return;
                    }
                    dgvImformation.Rows.RemoveAt(e.RowIndex);// dgvDetail.Rows[e.RowIndex].
                    SetSrNo();

                }
                if (e.ColumnIndex == dgvImformation.Columns["Execute"].Index)
                {
                    try
                    {
                        if (dgvImformation.Rows[e.RowIndex].Cells["DGVQuery"].Value == null || dgvImformation.Rows[e.RowIndex].Cells["DGVQuery"].Value.ToString() == "")
                        {
                            //  MessageBox.Show("No Query Found.\nExecuting Query from MENU.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LocalConnection.tempQuery = dgvImformation.Rows[e.RowIndex].Cells["DGVDashQuery"].Value.ToString();
                        }
                        else
                        {
                            LocalConnection.tempQuery = dgvImformation.Rows[e.RowIndex].Cells["DGVQuery"].Value.ToString();
                        }
                        frmQueryExecutor obj = new frmQueryExecutor();
                        Operation.gViewQuery = "";
                        System.Data.OleDb.OleDbConnection tempconn = new System.Data.OleDb.OleDbConnection(dgvImformation.Rows[e.RowIndex].Cells["DGVConn"].Value.ToString());
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




    }
}
