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
    public partial class frmDashboardMaster : Form
    {
        ArrayList Queries = new ArrayList();
        public frmDashboardMaster()
        {
            InitializeComponent();
        }
        private void frmPartymaster_Load(object sender, EventArgs e)
        {
            try
            {
                BindDashBoardGrid();
                BindUserMenuGrid();
                btnAdd_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BindUserMenuGrid()
        {
            string q = "SELECT  ClientMenuID, FirstName,LastName,MenuName,Query ,ClientMenuMaster.MenuID " +
                        " FROM  ClientMenuMaster    INNER JOIN   ClientMaster ON ClientMenuMaster.ClientID = ClientMaster.ClientID " +
                        "     INNER JOIN MenuMaster ON ClientMenuMaster.MenuID = MenuMaster.MenuID    " +
                        " where MenuMaster.IsDashboard = 1 and ClientMaster.CreatedAdminID = " + Operation.AdminUserId + "";
            DataTable dt = Operation.GetDataTable(q, Operation.Conn);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvClientMenu.Rows.Add();
                    dgvClientMenu.Rows[i].Cells["Select"].Value = false;
                    dgvClientMenu.Rows[i].Cells["ClientMenuID"].Value = dt.Rows[i]["ClientMenuID"];
                    dgvClientMenu.Rows[i].Cells["MenuID"].Value = dt.Rows[i]["MenuID"];
                    dgvClientMenu.Rows[i].Cells["FirstName"].Value = dt.Rows[i]["FirstName"];
                    dgvClientMenu.Rows[i].Cells["LastName"].Value = dt.Rows[i]["LastName"];
                    dgvClientMenu.Rows[i].Cells["MenuName"].Value = dt.Rows[i]["MenuName"];
                    dgvClientMenu.Rows[i].Cells["Query"].Value = dt.Rows[i]["Query"];
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


        private void btnAdd_Click(object sender, EventArgs e)
        {
            Queries.Clear();
            lblid.Text = "0";
            dgvDetail.Rows.Clear();
            FetchDetailGrid();
            // txtmobile.Text = "";
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (dgvDetail.Rows.Count == 0)
            //    return;
            try
            {
                for (int i = 0; i < dgvDetail.Rows.Count; i++)
                {
                    if (dgvDetail.Rows[i].Cells["DetailLink"].Value == null)
                        Queries.Add("Insert into ClientDashBoardMaster (ClientMenuID,DashBoardID) values (" + dgvDetail.Rows[i].Cells["DetailClientMenuID"].Value + "," + dgvDetail.Rows[i].Cells["DetailDashBoardID"].Value + "  )");
                    else
                    { } // Queries.Add("update ClientChildMenuMaster set  (ClientMenuID,ChildMenuID) values (" + dgvDetail.Rows[i].Cells["DetailClientMenuID"].Value + "," + dgvDetail.Rows[i].Cells["DetailChildMenuID"].Value + "  )");

                }
                if (Queries.Count == 0)
                {
                    MessageBox.Show("Nothing To Save..", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnAdd_Click(sender, e);
                    return;
                }
                if (Operation.ExecuteTransaction(Queries, Operation.Conn))
                {
                    MessageBox.Show("Record Saved Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    //btnAdd_Click(sender, e);
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
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private bool ChekForMenu()
        {
            for (int i = 0; i < dgvDash.Rows.Count; i++)
            {
                if (dgvDash.Rows[i].DefaultCellStyle.BackColor == Color.Green)
                {
                    return true;
                }
            }
            return false;
        }


        private void frmPartymaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Operation.ExecuteNonQuery("Update PartyMaster set inedit=False where link=" + lblid.Text, Operation.Conn);
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

        private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    if (dgvDash.Rows[dgvDash.CurrentRow.Index].DefaultCellStyle.BackColor == Color.White)
                    {
                        dgvDash.Rows[e.RowIndex].Cells["DashSelect"].Value = true;
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

        private void BindDashBoardGrid()
        {
            string Q;
            DataTable dt = new DataTable();
            //Q = "SELECT DashBoardID, MenuID, Type, DashboardName, Query from DashboardMaster";
            Q = "select  DashboardID, DashboardMaster.MenuID, Type, DashboardName, DashboardMaster.Query,ZoomMenuID,MenuMaster.MenuName from DashboardMaster inner join MenuMaster on MenuMaster.MenuID = DashboardMaster.ZoomMenuID";
            dt = Operation.GetDataTable(Q, Operation.Conn);
            dgvDash.ReadOnly = true;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    dgvDash.Rows.Add();
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashSelect"].Value = false;
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashBoardID"].Value = dt.Rows[i]["DashBoardID"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["MenuIDDash"].Value = dt.Rows[i]["MenuID"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashBoardName"].Value = dt.Rows[i]["DashboardName"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["DashBoardQuery"].Value = dt.Rows[i]["Query"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["Type"].Value = dt.Rows[i]["Type"];
                    dgvDash.Rows[dgvDash.Rows.Count - 1].Cells["ZoomMenu"].Value = dt.Rows[i]["MenuName"];
                
                }
            }
        }
        private void btndeselectuser_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvClientMenu.Rows.Count; i++)
            {
                dgvClientMenu.Rows[i].DefaultCellStyle.BackColor = Color.White;
                dgvClientMenu.Rows[i].Cells["Select"].Value = false;
            }
        }

        private void btnselectUser_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvClientMenu.Rows.Count; i++)
            {
                dgvClientMenu.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                dgvClientMenu.Rows[i].Cells["Select"].Value = true;
            }
        }

        private void btnAssignMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if ((!IsSelectedRows(dgvClientMenu)) || (!IsSelectedRows(dgvDash)))
                {
                    MessageBox.Show("Please Select Atleast One Client And SubMenu Item.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            for (int i = 0; i < dgvClientMenu.Rows.Count; i++)
            {
                for (int j = 0; j < dgvDash.Rows.Count; j++)
                {
                    if (dgvClientMenu.Rows[i].Cells["Select"].Value.ToString() == "True" && dgvDash.Rows[j].Cells["DashSelect"].Value.ToString() == "True")
                    {
                        for (int d = 0; d < dgvDetail.Rows.Count; d++)
                        {
                            if (dgvDetail.Rows[d].Cells["DetailClientMenuID"].Value.ToString() == dgvClientMenu.Rows[i].Cells["ClientMenuID"].Value.ToString() && dgvDetail.Rows[d].Cells["DetailDashBoardID"].Value.ToString() == dgvDash.Rows[j].Cells["DashBoardID"].Value.ToString())
                            {
                                isexist = true;
                                break;
                            }

                        }
                        if (isexist == false)
                        {
                            dgvDetail.Rows.Add();
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailClientMenuID"].Value = dgvClientMenu.Rows[i].Cells["ClientMenuID"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailFirstName"].Value = dgvClientMenu.Rows[i].Cells["FirstName"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailLastName"].Value = dgvClientMenu.Rows[i].Cells["LastName"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailMenuName"].Value = dgvClientMenu.Rows[i].Cells["MenuName"].Value;

                            // dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailToDate"].Value = dgvClientMenu.Rows[i].Cells["ToDate"].Value;
                            //Chidlmenu details
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailDashBoardID"].Value = dgvDash.Rows[j].Cells["DashBoardID"].Value;

                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailDashboardQuery"].Value = dgvDash.Rows[j].Cells["DashboardQuery"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailDashBoardName"].Value = dgvDash.Rows[j].Cells["DashBoardName"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailDashBoardType"].Value = dgvDash.Rows[j].Cells["Type"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailZoomMenu"].Value = dgvDash.Rows[j].Cells["ZoomMenu"].Value;

                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].DefaultCellStyle.BackColor = Color.SkyBlue;

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

        private void DisplaySubemnu(string MenuID)
        {
            if (MenuID != "0")
                for (int i = 0; i < dgvDash.Rows.Count; i++)
                {
                    if (dgvDash.Rows[i].Cells["MenuIDDash"].Value.ToString() == MenuID)
                        dgvDash.Rows[i].Visible = true;
                    else
                        dgvDash.Rows[i].Visible = false;

                }
        }
        private void dgvUserCompany_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.ColumnIndex == 2)
                {
                    if (dgvClientMenu.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.White)
                    {
                        dgvClientMenu.Rows[e.RowIndex].Cells["Select"].Value = true;
                        dgvClientMenu.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;

                    }
                    else
                    {
                        dgvClientMenu.Rows[e.RowIndex].Cells["Select"].Value = false;
                        dgvClientMenu.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
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
                if (e.ColumnIndex == 11 && e.RowIndex != -1)
                {
                    if (dgvDetail.Rows[e.RowIndex].Cells["DetailLink"].Value != null)
                    {
                        if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Queries.Add("delete from ClientDashBoardMaster Where ClientDashBoardID = " + dgvDetail.Rows[e.RowIndex].Cells["Detaillink"].Value + " ");
                            Queries.Add("delete from ClientUserDashBoardMaster Where ClientDashBoardID = " + dgvDetail.Rows[e.RowIndex].Cells["Detaillink"].Value + " ");
                     
                        }
                    }
                    dgvDetail.Rows.RemoveAt(e.RowIndex);// dgvDetail.Rows[e.RowIndex].
                    SetSrNo();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FetchDetailGrid()
        {
            string q = "SELECT     ClientDashBoardMaster.DashBoardID, ClientDashBoardMaster.ClientMenuID,  ClientDashBoardMaster.ClientDashBoardID,  ClientMaster.FirstName,  ClientMaster.LastName,  DashboardMaster.DashboardName,  DashboardMaster.Query,  DashboardMaster.Type,menuzoom.MenuName AS ZoomMenu,    MenuMaster.MenuName " +
                      " FROM ClientDashBoardMaster INNER JOIN " +
                      " ClientMenuMaster ON ClientDashBoardMaster.ClientMenuID = ClientMenuMaster.ClientMenuID " +
                      " INNER JOIN   MenuMaster ON ClientMenuMaster.MenuID = MenuMaster.MenuID " +
                      " INNER JOIN  ClientMaster ON ClientMenuMaster.ClientID = ClientMaster.ClientID " +
                       " INNER JOIN  DashboardMaster ON ClientDashBoardMaster.DashBoardID = DashboardMaster.DashboardID" +
                     " INNER JOIN    MenuMaster AS menuzoom ON DashboardMaster.ZoomMenuId = menuzoom.MenuID "+
                       " WHERE  ClientMaster.CreatedAdminID =  " + Operation.AdminUserId + " ";
            DataTable dt = Operation.GetDataTable(q, Operation.Conn);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvDetail.Rows.Add();
                    dgvDetail.Rows[i].Cells["DetailLink"].Value = dt.Rows[i]["ClientDashBoardID"];
                    //User And Menu Detail
                    dgvDetail.Rows[i].Cells["DetailClientMenuID"].Value = dt.Rows[i]["ClientMenuID"];
                    dgvDetail.Rows[i].Cells["DetailFirstName"].Value = dt.Rows[i]["FirstName"];
                    dgvDetail.Rows[i].Cells["DetailLastName"].Value = dt.Rows[i]["LastName"];
                    dgvDetail.Rows[i].Cells["DetailMenuName"].Value = dt.Rows[i]["MenuName"];
                    //menu details
                    dgvDetail.Rows[i].Cells["DetailDashboardID"].Value = dt.Rows[i]["DashboardID"];
                    dgvDetail.Rows[i].Cells["DetailDashboardName"].Value = dt.Rows[i]["DashboardName"];
                    dgvDetail.Rows[i].Cells["DetailDashboardQuery"].Value = dt.Rows[i]["Query"];
                    dgvDetail.Rows[i].Cells["DetailDashboardType"].Value = dt.Rows[i]["Type"];
                    dgvDetail.Rows[i].Cells["DetailZoomMenu"].Value = dt.Rows[i]["ZoomMenu"];

                }
                SetSrNo();
            }

        }

        private void dgvClientMenu_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            string mnuid = (dgvClientMenu.Rows[e.RowIndex].Cells["MenuID"].Value == null ? "0" : dgvClientMenu.Rows[e.RowIndex].Cells["MenuID"].Value.ToString());

            DisplaySubemnu(mnuid);
        }




    }
}
