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
    public partial class frmSubMenuMaster : Form
    {
        ArrayList Queries = new ArrayList();
        public frmSubMenuMaster()
        {
            InitializeComponent();
        }
      
        private void frmPartymaster_Load(object sender, EventArgs e)
        {
            try
            {

                BindSubMenuGrid();
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
                        "where MenuMaster.HasChildMenu = 1 and ClientMaster.CreatedAdminID = " + Operation.AdminUserId + " ";
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
                        Queries.Add("Insert into ClientChildMenuMaster (ClientMenuID,ChildMenuID) values (" + dgvDetail.Rows[i].Cells["DetailClientMenuID"].Value + "," + dgvDetail.Rows[i].Cells["DetailChildMenuID"].Value + "  )");
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
            for (int i = 0; i < dgvSubMenu.Rows.Count; i++)
            {
                if (dgvSubMenu.Rows[i].DefaultCellStyle.BackColor == Color.Green)
                {
                    return true;
                }
            }
            return false;
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

        private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3)
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
    
        private void BindSubMenuGrid()
        {

            string Q;
            DataTable dt = new DataTable();
            Q = "SELECT ChildMenuID, ChildMenuName, MenuID, Query, HasChildData,ZoomQuery, HasGraph FROM ChildMenuMaster";
            dt = Operation.GetDataTable(Q, Operation.Conn);
            dgvSubMenu.ReadOnly = true;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    dgvSubMenu.Rows.Add();
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuSelect"].Value = false;

                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["ChildMenuID"].Value = dt.Rows[i]["ChildMenuID"];
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["MenuIDsub"].Value = dt.Rows[i]["MenuID"];

                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuName"].Value = dt.Rows[i]["ChildMenuName"];
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuQuery"].Value = dt.Rows[i]["Query"];
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["ZoomQuery"].Value = dt.Rows[i]["ZoomQuery"];
                    
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].DefaultCellStyle.BackColor = Color.White;
                    if (dt.Rows[i]["HasChildData"].ToString() == "1")
                        dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["HasChildData"].Value = true;
                    else
                        dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["HasChildData"].Value = false;
                    dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["HasGraph"].Value = Convert.ToBoolean(dt.Rows[i]["HasGraph"]);
                    //if (i == 3)
                    //{ 
                    //dgvSubMenu.cou
                    //}
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
                if ((!IsSelectedRows(dgvClientMenu)) || (!IsSelectedRows(dgvSubMenu)))
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
                for (int j = 0; j < dgvSubMenu.Rows.Count; j++)
                {
                    if (dgvClientMenu.Rows[i].Cells["Select"].Value.ToString() == "True" && dgvSubMenu.Rows[j].Cells["SubMenuSelect"].Value.ToString() == "True")
                    {
                        for (int d = 0; d < dgvDetail.Rows.Count; d++)
                        {
                            if (dgvDetail.Rows[d].Cells["DetailClientMenuID"].Value.ToString() == dgvClientMenu.Rows[i].Cells["ClientMenuID"].Value.ToString() && dgvDetail.Rows[d].Cells["DetailChildMenuID"].Value.ToString() == dgvSubMenu.Rows[j].Cells["ChildMenuID"].Value.ToString())
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
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailChildMenuID"].Value = dgvSubMenu.Rows[j].Cells["ChildMenuID"].Value;
                           
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailSubMenuQuery"].Value = dgvSubMenu.Rows[j].Cells["SubMenuQuery"].Value;
                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailSubMenuName"].Value = dgvSubMenu.Rows[j].Cells["SubMenuName"].Value;

                            dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["DetailZoomQuery"].Value = dgvSubMenu.Rows[j].Cells["ZoomQuery"].Value;
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
            if(MenuID!="0")
            for (int i = 0; i < dgvSubMenu.Rows.Count; i++)
            {
                if (dgvSubMenu.Rows[i].Cells["MenuIDsub"].Value.ToString() == MenuID)
                    dgvSubMenu.Rows[i].Visible = true;
                else
                    dgvSubMenu.Rows[i].Visible = false ;
            
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
                if (e.ColumnIndex == 9 && e.RowIndex != -1)
                {
                    if (dgvDetail.Rows[e.RowIndex].Cells["DetailLink"].Value != null)
                    {
                        if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Queries.Add("delete from ClientChildMenuMaster Where ClientChildMenuID = " + dgvDetail.Rows[e.RowIndex].Cells["Detaillink"].Value + " ");
                            Queries.Add("delete from ClientUserChildMenuMaster Where ClientChildMenuID = " + dgvDetail.Rows[e.RowIndex].Cells["Detaillink"].Value + " ");
                     
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
            string q = "SELECT     ClientChildMenuMaster.ChildMenuID,     ClientChildMenuMaster.ClientMenuID,  ClientChildMenuMaster.ClientChildMenuID,   ClientMaster.FirstName,ClientMaster.LastName,ChildMenuMaster.ChildMenuName,ChildMenuMaster.Query,  ChildMenuMaster.ZoomQuery,  MenuName " +
                        "FROM    ClientChildMenuMaster        INNER JOIN   ClientMenuMaster ON ClientChildMenuMaster.ClientMenuID = ClientMenuMaster.ClientMenuID " +
                        " INNER JOIN   MenuMaster ON ClientMenuMaster.MenuID = MenuMaster.MenuID    inner join " +
                        " ClientMaster on ClientMenuMaster.ClientID = ClientMaster.ClientID    inner join " +
                        " ChildMenuMaster on ClientChildMenuMaster.ChildMenuID = ChildMenuMaster.ChildMenuID    " +
                        "  where ClientMaster.CreatedAdminID = "+Operation.AdminUserId+"";
            DataTable dt = Operation.GetDataTable(q, Operation.Conn);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvDetail.Rows.Add();
                    dgvDetail.Rows[i].Cells["DetailLink"].Value = dt.Rows[i]["ClientChildMenuID"];
                    //User And Menu Detail
                    dgvDetail.Rows[i].Cells["DetailClientMenuID"].Value = dt.Rows[i]["ClientMenuID"];
                    dgvDetail.Rows[i].Cells["DetailFirstName"].Value = dt.Rows[i]["FirstName"];
                    dgvDetail.Rows[i].Cells["DetailLastName"].Value = dt.Rows[i]["LastName"];
                    dgvDetail.Rows[i].Cells["DetailMenuName"].Value = dt.Rows[i]["MenuName"];
                    //menu details
                    dgvDetail.Rows[i].Cells["DetailChildMenuID"].Value = dt.Rows[i]["ChildMenuID"];
                    dgvDetail.Rows[i].Cells["DetailSubMenuName"].Value = dt.Rows[i]["ChildMenuName"];
                    dgvDetail.Rows[i].Cells["DetailSubMenuQuery"].Value = dt.Rows[i]["Query"];
                    dgvDetail.Rows[i].Cells["DetailZoomQuery"].Value = dt.Rows[i]["ZoomQuery"];
                
                }
                SetSrNo();
            }

        }

        private void brnDashSelectAll_Click(object sender, EventArgs e)
        {

        }


        private void dgvClientMenu_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            string mnuid = (dgvClientMenu.Rows[e.RowIndex].Cells["MenuID"].Value == null ? "0" : dgvClientMenu.Rows[e.RowIndex].Cells["MenuID"].Value.ToString());

            DisplaySubemnu(mnuid);
        }

        private void dgvDetail_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //SelectLinkUp(e.RowIndex);
            //dgvDetail.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
        }
        //private void SelectLinkUp(int index)
        //{


        //    for (int i = 0; i < dgvClientMenu.Rows.Count; i++)
        //    {
        //        if (dgvDetail.Rows[index].Cells["DetailClientMenuID"].Value.ToString() == dgvClientMenu.Rows[i].Cells["ClientMenuID"].Value.ToString())
        //        {
                  
        //            dgvClientMenu.Rows[i].DefaultCellStyle.BackColor = Color.Green;
        //        }


        //    }
        //    for (int j = 0; j < dgvSubMenu.Rows.Count; j++)
        //    {
        //        if (dgvDetail.Rows[index].Cells["DetailChildMenuID"].Value.ToString() == dgvSubMenu.Rows[j].Cells["ChildMenuID"].Value.ToString())
        //        {
        //            dgvSubMenu.Rows[j].DefaultCellStyle.BackColor = Color.Green;
        //            dgvSubMenu.Rows[j].Cells["SubMenuselect"].Value = true;
        //        }
        //    }
       
        
        //}
        


    }
}
