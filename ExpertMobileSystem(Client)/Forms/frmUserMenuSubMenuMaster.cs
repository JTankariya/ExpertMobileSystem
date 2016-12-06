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
    public partial class frmUserMenuSubMenuMaster : Form
    {
        ArrayList Queries = new ArrayList();
        public frmUserMenuSubMenuMaster()
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
                Paint += draw;
                Invalidate();
                BindMenuGrid();
                BindDetailGrid();
                btnAdd_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BindDetailGrid()
        {
            string q = "SELECT    ClientUserMenuMaster.ClientUserMenuID,ClientUserMenuMaster.ClientMenuID,   ClientUserMenuMaster.ClientUserCompanyID,       ClientUserMaster.FirstName,    ClientUserMaster.LastName,ExpertPath,CompanyCode,    CompanyName,     MenuName    FROM " +
                     " ClientUserMenuMaster   INNER JOIN   ClientUserCompanyMaster ON ClientUserMenuMaster.ClientUserCompanyID = ClientUserCompanyMaster.ClientUserCompanyid     INNER JOIN " +
                     " ClientMenuMaster ON ClientUserMenuMaster.ClientMenuID = ClientMenuMaster.ClientMenuID   INNER JOIN " +
                     " MenuMaster ON ClientMenuMaster.MenuID = MenuMaster.MenuID   INNER JOIN " +
                     "   ClientCompanyMaster  ON ClientUserCompanyMaster.ClientCompanyId = ClientCompanyMaster.ClientCompanyId   " +
                     "   INNER JOIN    ClientUserMaster ON ClientUserCompanyMaster.ClientUserid = ClientUserMaster.ClientUserid " +
                     " where MenuMaster.HasChildMenu = 1  and ClientMenuMaster.ClientID = " + Operation.Clientid + "" +
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

                    //menu details
                    dgvDetail.Rows[i].Cells["DetailClientMenuID"].Value = dt.Rows[i]["ClientMenuID"];
                    dgvDetail.Rows[i].Cells["DetailMenuName"].Value = dt.Rows[i]["MenuName"];
                    dgvDetail.Rows[i].Cells["Connection"].Value = Operation.createCompanyConn(dt.Rows[i]["ExpertPath"].ToString(), dt.Rows[i]["CompanyCode"].ToString());
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
                dgvUserChild.Columns["DGVSubMenuQuery"].Visible = true;
                dgvUserChild.Columns["DGVQuery"].Visible = true;
                dgvUserChild.Columns["Execute"].Visible = true;
                dgvUserChild.Columns["DetailZoomQuery"].Visible = true;
                dgvUserChild.Columns["MasterZoom"].Visible = true;

                dgvSubMenu.Columns["SubMenuQuery"].Visible = true;
                dgvSubMenu.Columns["ZoomQuery"].Visible = true;


            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Queries.Clear();
                lblid.Text = "0";
                dgvUserChild.Rows.Clear();
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
            if (dgvUserChild.Rows.Count == 0 && Queries.Count == 0) 
                return;

            btnSave.Enabled = false;
            try
            {

                for (int i = 0; i < dgvUserChild.Rows.Count; i++)
                {
                    string zoomQ = "", FilterQuery = "";

                    zoomQ = (dgvUserChild.Rows[i].Cells["DetailZoomQuery"].Value == null ? "":dgvUserChild.Rows[i].Cells["DetailZoomQuery"].Value.ToString());
                    FilterQuery = (dgvUserChild.Rows[i].Cells["DGVQuery"].Value == null ? "" : dgvUserChild.Rows[i].Cells["DGVQuery"].Value.ToString());

                    // string q = txtQuery.Text, zoomq = txtZoomQuery.Text;
                    zoomQ = (zoomQ.Trim().Replace("\\", "\\\\")).Replace("'", "\\'");

                    FilterQuery = (FilterQuery.Trim().Replace("\\", "\\\\")).Replace("'", "\\'");

                    if (dgvUserChild.Rows[i].Cells["DGVLink"].Value == null)
                        Queries.Add("Insert into ClientUserChildMenuMaster (ClientChildMenuID,Query,ClientUserMenuID) Values (" + dgvUserChild.Rows[i].Cells["DGVClientChildMenuID"].Value + ",'" + FilterQuery + "'," + dgvUserChild.Rows[i].Cells["DGVClientUserMenuID"].Value + ")");
                    else if (dgvUserChild.Rows[i].Cells["DGVQuery"].Value != null && dgvUserChild.Rows[i].Cells["DGVLink"].Value != null)
                    {
                        Queries.Add("Update ClientUserChildMenuMaster set Query = '" + FilterQuery + "' where ClientUserChildMenuID = " + dgvUserChild.Rows[i].Cells["DGVLink"].Value + " ");
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
            for (int i = 0; i < dgvSubMenu.Rows.Count; i++)
            {
                dgvSubMenu.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                dgvSubMenu.Rows[i].Cells["SubMenuSelect"].Value = true;
            }
        }
        private void btnUnselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvSubMenu.Rows.Count; i++)
            {
                dgvSubMenu.Rows[i].DefaultCellStyle.BackColor = Color.White;
                dgvSubMenu.Rows[i].Cells["SubMenuSelect"].Value = false;
            }
        }
        private void BindMenuGrid()
        {

            string Q;
            DataTable dt = new DataTable();
            Q = "SELECT     ClientChildMenuMaster.ClientChildMenuID,   ClientMenuMaster.ClientMenuID,   ChildMenuMaster.ChildMenuName,  ChildMenuMaster.Query, ChildMenuMaster.ZoomQuery,    ChildMenuMaster.HasGraph,   ChildMenuMaster.HasChildData " +
                "   FROM   ClientChildMenuMaster   INNER JOIN  ChildMenuMaster ON ChildMenuMaster.ChildMenuID = ClientChildMenuMaster.ChildMenuID " +
                "    inner join    ClientMenuMaster on ClientMenuMaster.ClientMenuID = ClientChildMenuMaster.ClientMenuID " +
                " WHERE    ClientMenuMaster.ClientID = " + Operation.Clientid + "";
            dt = Operation.GetDataTable(Q, Operation.Conn);
            dgvSubMenu.ReadOnly = true;
            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = " ChildMenuName";
                dt = dt.DefaultView.ToTable(false);

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    dgvSubMenu.Rows.Add();
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuSelect"].Value = false;
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["ClientChildMenuID"].Value = dt.Rows[i]["ClientChildMenuID"];

                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["ClientMenuID"].Value = dt.Rows[i]["ClientMenuID"];
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuName"].Value = dt.Rows[i]["ChildMenuName"];
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuQuery"].Value = dt.Rows[i]["Query"];
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].DefaultCellStyle.BackColor = Color.White;
                    if (dt.Rows[i]["HasChildData"].ToString() == "1")
                        dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuHasChild"].Value = true;
                    else
                        dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuHasChild"].Value = false;
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuHasGraph"].Value = Convert.ToBoolean(dt.Rows[i]["HasGraph"]);
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["ZoomQuery"].Value = (dt.Rows[i]["ZoomQuery"]);

                    //if (i == 3)
                    //{
                    //    // dgvMenu.cou
                    //}
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
                if ((!IsSelectedRows(dgvDetail)) || (!IsSelectedRows(dgvSubMenu)))
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
                for (int j = 0; j < dgvSubMenu.Rows.Count; j++)
                {
                    if (dgvDetail.Rows[i].Cells["Select"].Value.ToString() == "True" && dgvSubMenu.Rows[j].Cells["SubMenuSelect"].Value.ToString() == "True")
                    {
                        for (int d = 0; d < dgvUserChild.Rows.Count; d++)
                        {
                            if (dgvUserChild.Rows[d].Cells["DGVClientUserMenuID"].Value.ToString() == dgvDetail.Rows[i].Cells["ClientUserMenuID"].Value.ToString() && dgvUserChild.Rows[d].Cells["DGVClientChildMenuID"].Value.ToString() == dgvSubMenu.Rows[j].Cells["ClientChildMenuID"].Value.ToString())
                            {
                                isexist = true;
                                break;

                            }

                        }
                        if (isexist == false)
                        {
                            dgvUserChild.Rows.Add();
                            dgvUserChild.Rows[dgvUserChild.Rows.Count - 1].Cells["DGVClientUserMenuID"].Value = dgvDetail.Rows[i].Cells["ClientUserMenuID"].Value;
                            dgvUserChild.Rows[dgvUserChild.Rows.Count - 1].Cells["DGVFirstName"].Value = dgvDetail.Rows[i].Cells["DetailFirstName"].Value;
                            dgvUserChild.Rows[dgvUserChild.Rows.Count - 1].Cells["DGVLastName"].Value = dgvDetail.Rows[i].Cells["DetailLastName"].Value;
                            dgvUserChild.Rows[dgvUserChild.Rows.Count - 1].Cells["DGVMenuName"].Value = dgvDetail.Rows[i].Cells["DetailMenuName"].Value;
                            dgvUserChild.Rows[dgvUserChild.Rows.Count - 1].Cells["DGVCompanyName"].Value = dgvDetail.Rows[i].Cells["DetailCompanyName"].Value;
                            dgvUserChild.Rows[dgvUserChild.Rows.Count - 1].Cells["DGVConn"].Value = dgvDetail.Rows[i].Cells["Connection"].Value;

                            //menu details
                            dgvUserChild.Rows[dgvUserChild.Rows.Count - 1].Cells["DGVClientChildMenuID"].Value = dgvSubMenu.Rows[j].Cells["ClientChildMenuID"].Value;
                            dgvUserChild.Rows[dgvUserChild.Rows.Count - 1].Cells["DGVSubMenuName"].Value = dgvSubMenu.Rows[j].Cells["SubMenuName"].Value;
                            dgvUserChild.Rows[dgvUserChild.Rows.Count - 1].Cells["DGVSubMenuQuery"].Value = dgvSubMenu.Rows[j].Cells["SubMenuQuery"].Value;
                            dgvUserChild.Rows[dgvUserChild.Rows.Count - 1].Cells["MasterZoom"].Value = dgvSubMenu.Rows[j].Cells["ZoomQuery"].Value;

                            dgvUserChild.Rows[dgvUserChild.Rows.Count - 1].DefaultCellStyle.BackColor = Color.SkyBlue;

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
            for (int i = 0; i < dgvUserChild.Rows.Count; i++)
            {
                dgvUserChild.Rows[i].Cells["DGVSrNo"].Value = c;
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
            string q = "  SELECT ClientUserChildMenuMaster.ClientUserChildMenuID,  ClientUserMenuMaster.ClientUserMenuID, ClientUserChildMenuMaster.ClientChildMenuID,   " +
                        "  ClientUserMaster.FirstName,  ClientUserMaster.LastName,   MenuName,ExpertPath,CompanyCode, " +
                        "   ChildMenuMaster.Query as ChildMenuQuery,ChildMenuMaster.ZoomQuery,ClientUserChildMenuMaster.Query as Query,MenuMaster.ZoomQuery as MasterZoom , " +
                        "  ChildMenuMaster.ChildMenuName, ClientCompanyMaster.CompanyName FROM    ClientUserChildMenuMaster " +
                        "  INNER JOIN   ClientUserMenuMaster ON ClientUserMenuMaster.ClientUserMenuID = ClientUserChildMenuMaster.ClientUserMenuID " +
                        "  INNER JOIN   ClientUserCompanyMaster ON ClientUserMenuMaster.ClientUserCompanyID = ClientUserCompanyMaster.ClientUserCompanyid " +
                        "  INNER JOIN   ClientMenuMaster ON ClientUserMenuMaster.ClientMenuID = ClientMenuMaster.ClientMenuID " +
                        "  INNER JOIN   MenuMaster ON ClientMenuMaster.MenuID = MenuMaster.MenuID " +
                        "  INNER JOIN   ClientUserMaster ON ClientUserCompanyMaster.ClientUserid = ClientUserMaster.ClientUserid " +
                        "  inner join   ClientChildMenuMaster on ClientChildMenuMaster.ClientChildMenuID = ClientUserChildMenuMaster.ClientChildMenuID " +
                        "  inner join	ChildMenuMaster on ChildMenuMaster.ChildMenuID =  ClientChildMenuMaster.ChildMenuID " +
                        "  inner join    ClientCompanyMaster on  ClientCompanyMaster.ClientCompanyId = ClientUserCompanyMaster.ClientCompanyid " +
                        "  where ClientUserMaster.ClientID = " + Operation.Clientid + " " +
                        "  ORDER BY ClientUserMenuID ASC ";
            DataTable dt = Operation.GetDataTable(q, Operation.Conn);
            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = " FirstName,LastName,CompanyName,MenuName";
                dt = dt.DefaultView.ToTable(false);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvUserChild.Rows.Add();
                    //User And Company Detail
                    dgvUserChild.Rows[i].Cells["DGVClientUserMenuID"].Value = dt.Rows[i]["ClientUserMenuID"];
                    dgvUserChild.Rows[i].Cells["DGVLink"].Value = dt.Rows[i]["ClientUserChildMenuID"];

                    // dgvUserChild.Rows[i].Cells["DetailClientUserCompanyID"].Value = dt.Rows[i]["ClientUserCompanyID"];
                    dgvUserChild.Rows[i].Cells["DGVFirstName"].Value = dt.Rows[i]["FirstName"];
                    dgvUserChild.Rows[i].Cells["DGVLastName"].Value = dt.Rows[i]["LastName"];
                    dgvUserChild.Rows[i].Cells["DGVMenuName"].Value = dt.Rows[i]["MenuName"];
                    //menu details
                    dgvUserChild.Rows[i].Cells["DGVClientChildMenuID"].Value = dt.Rows[i]["ClientChildMenuID"];
                    dgvUserChild.Rows[i].Cells["DGVSubMenuName"].Value = dt.Rows[i]["ChildMenuName"];
                    dgvUserChild.Rows[i].Cells["DGVSubMenuQuery"].Value = dt.Rows[i]["ChildMenuQuery"].ToString();
                    dgvUserChild.Rows[i].Cells["DGVQuery"].Value = dt.Rows[i]["Query"];

                    dgvUserChild.Rows[i].Cells["DGVCompanyName"].Value = dt.Rows[i]["CompanyName"];
                    dgvUserChild.Rows[i].Cells["DGVConn"].Value = Operation.createCompanyConn(dt.Rows[i]["ExpertPath"].ToString(), dt.Rows[i]["CompanyCode"].ToString());
                    dgvUserChild.Rows[i].Cells["DetailZoomQuery"].Value = dt.Rows[i]["ZoomQuery"];
                    dgvUserChild.Rows[i].Cells["MasterZoom"].Value = dt.Rows[i]["MasterZoom"];


                    dgvUserChild.Rows[i].DefaultCellStyle.BackColor = Color.White;
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
                for (int i = 0; i < dgvSubMenu.Rows.Count; i++)
                {
                    if (dgvSubMenu.Rows[i].Cells["ClientMenuID"].Value.ToString() == MenuID)
                        dgvSubMenu.Rows[i].Visible = true;
                    else
                        dgvSubMenu.Rows[i].Visible = false;

                }
        }

        private void dgvSubMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    if (dgvSubMenu.Rows[dgvSubMenu.CurrentRow.Index].DefaultCellStyle.BackColor == Color.White)
                    {
                        dgvSubMenu.Rows[e.RowIndex].Cells["SubMenuSelect"].Value = true;
                        dgvSubMenu.Rows[dgvSubMenu.CurrentRow.Index].DefaultCellStyle.BackColor = Color.Green;

                    }
                    else
                    {

                        dgvSubMenu.Rows[e.RowIndex].Cells["SubMenuSelect"].Value = false;
                        dgvSubMenu.Rows[dgvSubMenu.CurrentRow.Index].DefaultCellStyle.BackColor = Color.White;
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
                    if (dgvDetail.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.White)
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
                if (e.ColumnIndex == dgvUserChild.Columns["Delete"].Index)
                {
                    if (dgvUserChild.Rows[e.RowIndex].Cells["DGVLink"].Value != null)
                    {
                        if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            Queries.Add("delete from ClientUserChildMenuMaster Where ClientUserChildMenuid = " + dgvUserChild.Rows[e.RowIndex].Cells["DGVLink"].Value + " ");
                        else
                            return;
                    }
                    dgvUserChild.Rows.RemoveAt(e.RowIndex);// dgvDetail.Rows[e.RowIndex].
                    SetSrNo();

                }
                if (e.ColumnIndex == dgvUserChild.Columns["Execute"].Index)
                {
                    try
                    {
                        if (dgvUserChild.Rows[e.RowIndex].Cells["DGVQuery"].Value == null || dgvUserChild.Rows[e.RowIndex].Cells["DGVQuery"].Value.ToString() == "")
                        {
                            //  MessageBox.Show("No Query Found.\nExecuting Query from MENU.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LocalConnection.tempQuery = dgvUserChild.Rows[e.RowIndex].Cells["DGVSubMenuQuery"].Value.ToString();
                        }
                        else
                        {
                            LocalConnection.tempQuery = dgvUserChild.Rows[e.RowIndex].Cells["DGVQuery"].Value.ToString();
                        }
                        frmQueryExecutor obj = new frmQueryExecutor();
                        Operation.gViewQuery = "";
                        System.Data.OleDb.OleDbConnection tempconn = new System.Data.OleDb.OleDbConnection(dgvUserChild.Rows[e.RowIndex].Cells["DGVConn"].Value.ToString());
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
