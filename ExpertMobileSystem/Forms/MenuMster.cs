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
    public partial class MenuMaster : Form
    {
        ArrayList Queries = new ArrayList();
        public MenuMaster()
        {
            InitializeComponent();
            Operation.BindComboBox(cmbZoomMenu, "SELECT MenuID,MenuName FROM MenuMaster where HaschildMenu = 0 and IsDashboard = 0 order by MenuName", "-- Select Menu Name--", "MenuName", "MenuID");
            cmbZoomMenu.SelectedIndex = cmbZoomMenu.Items.Count - 1;
            btnAdd_Click(null, null);

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
            cmbZoomMenu.SelectedIndex = cmbZoomMenu.Items.Count - 1;

            txtMenuName.Text = "";
            lblid.Text = "0";
            txtQuery.Text = "";
            rbNoneOfAbove.Checked = true;
            clear_subMenu();
            clearDash();
            dgvSubMenu.Rows.Clear();
            dgvDashboard.Rows.Clear();
            tabControl1.SelectedIndex = 0;
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage3);
            lblIndexDash.Text = "";
            lblIndex.Text = "";
            AutoGenerateSrNo();
            txtZoomQuery.Text = "";
        }
        private void AutoGenerateSrNo()
        {
            string No;
            No = Operation.ExecuteScalar("SELECT max(MenuMaster.SrNo) as no from MenuMaster ", Operation.Conn).ToString();
            if (No == "")
                txtsrno.Text = "1";
            else
                txtsrno.Text = (Convert.ToInt16(No) + 1).ToString();
        }
        private bool Validate_form()
        {
            
            if (txtMenuName.Text == "")
            {
                MessageBox.Show("Please Enter Menu Name.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMenuName.Focus();
                return false;
            }
            if ((txtQuery.Text == "" && rbIsDashboard.Checked == false && rbHasSubMenu.Checked == false))
            {
                MessageBox.Show("Please Enter Query.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtQuery.Focus();
                return false;
            }
            //if ((txtQuery.Text == "" && rbHasSubMenu.Checked == false))
            //{
            //    MessageBox.Show("Please Enter Query.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    txtQuery.Focus();
            //    return false;
            //}
            if (rbIsDashboard.Checked == true)
            {
                if (dgvDashboard.Rows.Count <= 0)
                {
                    MessageBox.Show("Please Enter Atleast One Dashboard.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tabControl1.SelectedIndex = 1;
                    return false;
                }
            }
            if (rbHasSubMenu.Checked == true)
            {
                if (dgvSubMenu.Rows.Count <= 0)
                {
                    MessageBox.Show("Please Enter Atleast One SubMenu.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tabControl1.SelectedIndex = 2;
                    return false;
                }
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {   
            
            if (!Validate_form())
                return;
            object ans = null;
            string isDashboardExist = "";
            if (rbIsDashboard.Checked == true)
                ans = Operation.ExecuteScalar("SELECT MenuID from MenuMaster where IsDashboard = 1  ", Operation.Conn);
            isDashboardExist = (ans == null ? "" : ans.ToString());
            if (isDashboardExist != "")
            {
                if (lblid.Text != isDashboardExist)
                {
                    MessageBox.Show("Dashboard Menu Already Exist.\nOnly One Dashboard Can be Existed.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //rbIsDashboard.Checked = false;
                    rbNoneOfAbove.Checked = true;
                    return;
                }

            }
            string HasChild = (rbHasChildData.Checked == true ? "1" : "0");
            string isdashboard = (rbIsDashboard.Checked == true ? "1" : "0");
            string HasChildMenu = (rbHasSubMenu.Checked == true ? "1" : "0");
            string HasGraph = (rbHasGraph.Checked == true ? "1" : "0");
            string q = txtQuery.Text, zoomq = txtZoomQuery.Text;
            txtQuery.Text = (q.ToString().Trim().Replace("\\", "\\\\")).Replace("'", "\\'");
            txtZoomQuery.Text= (zoomq.ToString().Trim().Replace("\\", "\\\\")).Replace("'", "\\'");

            try
            {
                if (lblid.Text == "0")
                {
                    Queries.Add("insert into MenuMaster (MenuName, Query, HasChildData,SrNo,IsDashboard,HasChildMenu,HasGraph,ZoomQuery) values ('" + txtMenuName.Text + "','" + txtQuery.Text + "'," + HasChild + "," + txtsrno.Text + "," + isdashboard + "," + HasChildMenu + "," + HasGraph + ",'" + txtZoomQuery.Text + "')");
                
                
                }
                else
                {
                    Queries.Add("update MenuMaster set MenuName = '" + txtMenuName.Text + "',Query = '" + txtQuery.Text + "',HasChildData= " + HasChild + ",SrNo = " + txtsrno.Text + ",IsDashboard =" + isdashboard + ",HasChildMenu = " + HasChildMenu + ",HasGraph = " + HasGraph + ",ZoomQuery='" + txtZoomQuery.Text + "' where Menuid = " + lblid.Text + "");
                }
                if (Operation.ExecuteTransaction(Queries, Operation.Conn))
                {
                    if (rbIsDashboard.Checked == true || rbHasSubMenu.Checked == true)
                    {
                        ArrayList Menuq = new ArrayList();
                        string mnuid;
                        if (lblid.Text == "0")
                            mnuid = Operation.ExecuteScalar("SELECT max(MenuID) from MenuMaster ", Operation.Conn).ToString();
                        else
                            mnuid = lblid.Text;
                        if (rbIsDashboard.Checked == true)
                        {
                           
                            for (int i = 0; i < dgvDashboard.Rows.Count; i++)
                            {
                                string  FilterQuery = "";
                                
                                FilterQuery = (dgvDashboard.Rows[i].Cells["Query"].Value == null ? "" :dgvDashboard.Rows[i].Cells["Query"].Value.ToString());                    

                                FilterQuery = (FilterQuery.Trim().Replace("\\", "\\\\")).Replace("'", "\\'");

                                if (dgvDashboard.Rows[i].Cells["Link"].Value == null)
                                    Menuq.Add("Insert into DashboardMaster (MenuID,Type,DashboardName,Query,ZoomMenuId) values (" + mnuid + ",'" + dgvDashboard.Rows[i].Cells["Type"].Value + "','" + dgvDashboard.Rows[i].Cells["DashboardName"].Value + "','" + FilterQuery+ "'," + dgvDashboard.Rows[i].Cells["ZoomMenuID"].Value + "  )");
                                else
                                    Menuq.Add("update DashboardMaster set Type ='" + dgvDashboard.Rows[i].Cells["Type"].Value + "',DashboardName= '" + dgvDashboard.Rows[i].Cells["DashboardName"].Value + "',Query='" +FilterQuery + "',ZoomMenuId=" + dgvDashboard.Rows[i].Cells["ZoomMenuID"].Value + " where DashboardID = " + dgvDashboard.Rows[i].Cells["Link"].Value + " ");
                            }
                        }
                        if (rbHasSubMenu.Checked == true)
                        {
                            for (int i = 0; i < dgvSubMenu.Rows.Count; i++)
                            {
                                string zoomQ = "", FilterQuery = "";

                                zoomQ = (dgvSubMenu.Rows[i].Cells["ZoomQuery"].Value == null ?"":dgvSubMenu.Rows[i].Cells["ZoomQuery"].Value.ToString());
                                FilterQuery = (dgvSubMenu.Rows[i].Cells["SubMenuQuery"].Value == null ? "" :dgvSubMenu.Rows[i].Cells["SubMenuQuery"].Value.ToString());

                               
                                zoomQ = (zoomQ.Trim().Replace("\\", "\\\\")).Replace("'", "\\'");

                                FilterQuery = (FilterQuery.Trim().Replace("\\", "\\\\")).Replace("'", "\\'");
                                if (dgvSubMenu.Rows[i].Cells["SubMenuLink"].Value == null)
                                    Menuq.Add("Insert into ChildMenuMaster (MenuID,ChildMenuName,Query,HasChildData,HasGraph,ZoomQuery) values (" + mnuid + ",'" + dgvSubMenu.Rows[i].Cells["SubMenuName"].Value + "','" + FilterQuery + "'," + dgvSubMenu.Rows[i].Cells["HasChildData"].Value + "," + dgvSubMenu.Rows[i].Cells["HasGraph"].Value + ",'" + zoomQ + "'  )");
                                else
                                    Menuq.Add("Update ChildMenuMaster Set ChildMenuName= '" + dgvSubMenu.Rows[i].Cells["SubMenuName"].Value + "', Query='" + FilterQuery + "',HasChildData = " + dgvSubMenu.Rows[i].Cells["HasChildData"].Value + ",HasGraph = " + dgvSubMenu.Rows[i].Cells["HasGraph"].Value + ",ZoomQuery ='" + zoomQ + "'   where ChildMenuID = " + dgvSubMenu.Rows[i].Cells["SubMenuLink"].Value + " ");
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
                    Queries = new ArrayList();
                    txtQuery.Text = q;
                    txtZoomQuery.Text = zoomq;
                    //btnAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured, Please Try after Some Time.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Queries = new ArrayList();
                txtQuery.Text = q;
                txtZoomQuery.Text = zoomq;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            dgvDashboard.Rows.Clear();
            dgvSubMenu.Rows.Clear();
            fillgrid();
        }
        private void fillgrid()
        {
            frmSearch view = new frmSearch();
            Operation.gViewQuery = "select MenuID, SrNo, MenuName, Query,ZoomQuery, IF(MenuMaster.HasChildData ='1', 'TRUE', 'FALSE') as HasChidData,IF(MenuMaster.IsDashboard ='1', 'TRUE', 'FALSE') as IsDashboard,IF(MenuMaster.HasChildMenu ='1', 'TRUE', 'FALSE') as HasChidMenu,IF(MenuMaster.HasGraph ='1', 'TRUE', 'FALSE') as HasGraph from MenuMaster  ";
            Operation.Bindgrid(Operation.gViewQuery, view.dgvSearch);
            view.dgvSearch.Columns[0].Visible = false;
            view.OrderByColoumn = "SrNo";
            view.ShowDialog();
            view.dgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            if (Operation.ViewID != null && Operation.ViewID != string.Empty)
            {

                filldata();
                Operation.ViewID = "";
                btnDelete.Enabled = true;
                Queries = new ArrayList();
            }
        }
        public void filldata()
        {
            filldata(Operation.GetDataTable("Select * from MenuMaster where MenuID=" + Operation.ViewID.ToString() + "", Operation.Conn));//Select * from MenuMaster where MenuID=" + Operation.ViewID.ToString()
            //MenuMaster.*, DashboardID, Type, DashboardName,DashboardMaster.Query,ChildMenuID, ChildMenuName, ChildMenuMaster.Query "+
            //                                   " FROM    MenuMaster     LEFT JOIN    DashboardMaster ON MenuMaster.MenuID = DashboardMaster.MenuID "+
            //                                    " LEFT JOIN   ChildMenuMaster ON MenuMaster.MenuID = ChildMenuMaster.MenuID WHERE   MenuMaster.MenuID = " + Operation.ViewID.ToString()
        }
        private void filldata(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {

                    txtMenuName.Text = dt.Rows[0]["MenuName"].ToString();
                    lblid.Text = dt.Rows[0]["MenuID"].ToString();
                    txtsrno.Text = dt.Rows[0]["SrNo"].ToString();
                    txtQuery.Text = dt.Rows[0]["Query"].ToString();
                    txtZoomQuery.Text = dt.Rows[0]["ZoomQuery"].ToString();

                    rbHasChildData.Checked = Convert.ToBoolean(dt.Rows[0]["HasChildData"]); //(dt.Rows[0]["HasChildData"] == null ? 0 : 1);
                    rbIsDashboard.Checked = Convert.ToBoolean(dt.Rows[0]["IsDashboard"]); //(dt.Rows[0]["HasChildData"] == null ? 0 : 1);
                    rbHasSubMenu.Checked = Convert.ToBoolean(dt.Rows[0]["HasChildMenu"]);
                    rbHasGraph.Checked = Convert.ToBoolean(dt.Rows[0]["HasGraph"]);
                    if (rbHasChildData.Checked == false && rbIsDashboard.Checked == false && rbHasSubMenu.Checked == false && rbHasGraph.Checked == false)
                        rbNoneOfAbove.Checked = true;
                    if (rbIsDashboard.Checked == true)
                    {
                        DataTable dtdas = Operation.GetDataTable("select  DashboardID, DashboardMaster.MenuID, Type, DashboardName, DashboardMaster.Query,ZoomMenuID,MenuMaster.MenuName from DashboardMaster inner join MenuMaster on MenuMaster.MenuID = DashboardMaster.ZoomMenuID where DashboardMaster.MenuID = " + lblid.Text + "", Operation.Conn);
                        if (dtdas.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtdas.Rows.Count; i++)
                            {
                                dgvDashboard.Rows.Add();
                                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["MenuID"].Value = dtdas.Rows[i]["MenuID"];
                                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["Link"].Value = dtdas.Rows[i]["DashboardID"];
                                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["SrNo"].Value = dgvDashboard.Rows.Count;
                                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["Type"].Value = dtdas.Rows[i]["Type"];
                                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["DashboardName"].Value = dtdas.Rows[i]["DashboardName"];
                                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["Query"].Value = dtdas.Rows[i]["Query"];
                                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["ZoomMenuID"].Value = dtdas.Rows[i]["ZoomMenuID"];
                                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["ZoomMenu"].Value = dtdas.Rows[i]["MenuName"];

                            }
                        }
                    }
                    if (rbHasSubMenu.Checked == true)
                    {
                        DataTable dtSub = Operation.GetDataTable("select  ChildMenuID, ChildMenuName, Query,HasChildData,HasGraph,ZoomQuery from ChildMenuMaster where MenuID = " + lblid.Text + "", Operation.Conn);

                        if (dtSub.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtSub.Rows.Count; i++)
                            {
                                dgvSubMenu.Rows.Add();
                                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuName"].Value = dtSub.Rows[i]["ChildMenuName"];
                                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuQuery"].Value = dtSub.Rows[i]["Query"];
                                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuLink"].Value = dtSub.Rows[i]["ChildMenuID"];
                                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["HasChildData"].Value = Convert.ToBoolean(dtSub.Rows[i]["HasChildData"]);
                                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["HasGraph"].Value = Convert.ToBoolean(dtSub.Rows[i]["HasGraph"]);
                                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["ZoomQuery"].Value = dtSub.Rows[i]["ZoomQuery"];


                            }
                        }
                        Operation.SetSrNo(dgvSubMenu, "SubMenuSrNo");

                    }

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
                else if (!Operation.CheckReference(Convert.ToInt32(lblid.Text), "MenuID,ClientMenuMaster"))
                    return;
                else if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ArrayList qDelete = new ArrayList();
                    qDelete.Add("Delete from MenuMaster where MenuID= " + lblid.Text.Trim());
                    qDelete.Add("Delete from DashboardMaster where MenuID = " + lblid.Text + "");
                    qDelete.Add("Delete from ChidMenuMaster where MenuID = " + lblid.Text + "");

                    Operation.ExecuteTransaction(qDelete, Operation.Conn);
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
        private bool mthValidate_Dashboard()
        {
            if (txtdashboard.Text == "")
            {
                MessageBox.Show("Please Enter Dashboard Name.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtdashboard.Focus();
                return false;
            }
            if (cmbtype.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Dashboard Type.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbtype.Focus();
                return false;
            }
            if (txtQuerydash.Text == "")
            {
                MessageBox.Show("Please Enter Dashboard Query.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtQuerydash.Focus();
                return false;
            }
            if (cmbZoomMenu.SelectedIndex == cmbZoomMenu.Items.Count - 1 || cmbZoomMenu.SelectedValue == lblid.Text)
            {
                MessageBox.Show("Please Select Zoom MenuName.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbZoomMenu.Focus();
                return false;
            }

            return true;
        }
        private bool Duplicate_dash()
        {
            for (int i = 0; i < dgvDashboard.Rows.Count; i++)
            {
                if (dgvDashboard.Rows[i].Cells["Type"].Value.ToString() == cmbtype.Text && dgvDashboard.Rows[i].Cells["DashboardName"].Value.ToString() == txtdashboard.Text && lblIndexDash.Text != i.ToString())
                {
                    return false;
                }
            }
            return true;
        }
        private void clearDash()
        {
            txtdashboard.Text = "";
            cmbtype.SelectedIndex = 0;
            txtQuerydash.Text = "";
            lblIndexDash.Text = "";
            btnCancelDash.Visible = false;
            button1.Text = "Add Dashboard";
            cmbZoomMenu.SelectedIndex = cmbZoomMenu.Items.Count - 1;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!mthValidate_Dashboard())
                return;
            if (!Duplicate_dash())
            {
                MessageBox.Show("Dashboard Name: " + txtdashboard.Text + " with " + cmbtype.Text + " Link Already Exist.\nPlease Select Another One.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (button1.Text == "Update Dashboard")
            {
                button1.Text = "Add Dashboard";
            }
            if (lblIndexDash.Text != "")
            {
                int i = Convert.ToInt16(lblIndexDash.Text);
                dgvDashboard.Rows[i].Cells["SrNo"].Value = dgvDashboard.Rows.Count;
                dgvDashboard.Rows[i].Cells["Type"].Value = cmbtype.Text;
                dgvDashboard.Rows[i].Cells["DashboardName"].Value = txtdashboard.Text;
                dgvDashboard.Rows[i].Cells["Query"].Value = txtQuerydash.Text;
                dgvDashboard.Rows[i].Cells["ZoomMenu"].Value = cmbZoomMenu.Text;
                dgvDashboard.Rows[i].Cells["ZoomMenuID"].Value = cmbZoomMenu.SelectedValue;



            }
            else
            {

                dgvDashboard.Rows.Add();
                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["SrNo"].Value = dgvDashboard.Rows.Count;
                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["Type"].Value = cmbtype.Text;
                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["DashboardName"].Value = txtdashboard.Text;
                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["Query"].Value = txtQuerydash.Text;
                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["ZoomMenu"].Value = cmbZoomMenu.Text;
                dgvDashboard.Rows[dgvDashboard.Rows.Count - 1].Cells["ZoomMenuID"].Value = cmbZoomMenu.SelectedValue;
            }
            clearDash();
        }

        private void dgvDashboard_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 8)
            {
                if (dgvDashboard.Rows[e.RowIndex].Cells["Link"].Value != null)
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Queries.Add("Delete from DashboardMaster where DashboardID = " + dgvDashboard.Rows[e.RowIndex].Cells["Link"].Value + "");
                    }
                }
                dgvDashboard.Rows.RemoveAt(e.RowIndex);
                mthdSetSrNo(dgvDashboard, "SrNo");
            }
        }
        private void mthdSetSrNo(DataGridView dgv, string col)
        {
            int c = 1;

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells[col].Value = c;
                c++;
            }
        }
        private void dgvDashboard_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtdashboard.Text = dgvDashboard.Rows[e.RowIndex].Cells["DashboardName"].Value.ToString();
                if (dgvDashboard.Rows[e.RowIndex].Cells["Type"].Value.ToString() == "BALANCE")
                    cmbtype.SelectedIndex = 1;
                else
                    cmbtype.SelectedIndex = 2;
                txtQuerydash.Text = dgvDashboard.Rows[e.RowIndex].Cells["Query"].Value.ToString();
                cmbZoomMenu.SelectedValue = dgvDashboard.Rows[e.RowIndex].Cells["ZoomMenuID"].Value.ToString();

                lblIndexDash.Text = e.RowIndex.ToString();
                button1.Text = "Update Dashboard";
                btnCancelDash.Visible = true;
            }
        }
        private void cmbtype_Validated(object sender, EventArgs e)
        {
            if (cmbtype.SelectedIndex == -1)
            {
                MessageBox.Show("" + cmbtype.Text + " Does not Exist.\nPlease Select Another One.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbtype.SelectedIndex = 0;
            }
        }
        private bool Duplicate_SubMenu()
        {
            for (int i = 0; i < dgvSubMenu.Rows.Count; i++)
            {
                if (dgvSubMenu.Rows[i].Cells["SubMenuName"].Value.ToString() == txtSubMenuName.Text && dgvSubMenu.Rows[i].Cells["SubMenuQuery"].Value.ToString() == txtQuerySubMenu.Text && i.ToString() != lblIndex.Text)
                {
                    return false;
                }
            }
            return true;
        }
        private void clear_subMenu()
        {
            txtQuerySubMenu.Text = "";
            txtSubMenuName.Text = "";
            lblIndex.Text = "";
            rbNoneOfAboveSubMenu.Checked = true;
            btnCancelSubmenu.Visible = false;
            txtzoomQSub.Text = "";
            btnAddSubMenu.Text = "Add SubMenu";
        }
        private void btnAddSubMenu_Click(object sender, EventArgs e)
        {
            if (!Validate_Submenu())
                return;
            if (!Duplicate_SubMenu())
            {
                MessageBox.Show("SubMenu Name : " + txtSubMenuName.Text + " with " + txtQuerySubMenu.Text + " Query Already Exist.\nPlease Enter Another One.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lblIndex.Text != "")
            {
                int i = Convert.ToInt16(lblIndex.Text);
                dgvSubMenu.Rows[i].Cells["SubMenuName"].Value = txtSubMenuName.Text;
                dgvSubMenu.Rows[i].Cells["SubMenuQuery"].Value = txtQuerySubMenu.Text;
                dgvSubMenu.Rows[i].Cells["HasChildData"].Value = rbHasChildDataSubMenu.Checked;
                dgvSubMenu.Rows[i].Cells["HasGraph"].Value = rbHasGraphSubMenu.Checked;
                dgvSubMenu.Rows[i].Cells["ZoomQuery"].Value = txtzoomQSub.Text;

                btnAddSubMenu.Text = "Add SubMenu";
                clear_subMenu();
                return;
            }
            else
            {
                dgvSubMenu.Rows.Add();
                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuName"].Value = txtSubMenuName.Text;
                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["SubMenuQuery"].Value = txtQuerySubMenu.Text;
                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["HasChildData"].Value = rbHasChildDataSubMenu.Checked;
                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["HasGraph"].Value = rbHasGraphSubMenu.Checked;
                dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["ZoomQuery"].Value = txtzoomQSub.Text;
                
                mthdSetSrNo(dgvSubMenu, "SubMenuSrNo");
                clear_subMenu();
            }

        }
        private Boolean Validate_Submenu()
        {
            if (txtSubMenuName.Text == "")
            {
                MessageBox.Show("Please Enter Sub Menu Name.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSubMenuName.Focus();
                return false;
            }
            if (txtQuerySubMenu.Text == "")
            {
                MessageBox.Show("Please Enter Query.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtQuerySubMenu.Focus();
                return false;
            }
            return true;
        }

        private void dgvSubMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                if (dgvSubMenu.Rows[e.RowIndex].Cells["SubMenuLink"].Value != null)
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Queries.Add("Delete from ChildMenuMaster where ChildMenuID = " + dgvSubMenu.Rows[e.RowIndex].Cells["SubMenuLink"].Value + "");
                    }
                }
                dgvSubMenu.Rows.RemoveAt(e.RowIndex);
                Operation.SetSrNo(dgvSubMenu, "SubMenuSrNo");
                // mthdSetSrNo(dgvSubMenu,"SubMenuSrNo");
            }
        }
        private void dgvSubMenu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSubMenuName.Text = dgvSubMenu.Rows[e.RowIndex].Cells["SubMenuName"].Value.ToString();
            txtQuerySubMenu.Text = dgvSubMenu.Rows[e.RowIndex].Cells["SubMenuQuery"].Value.ToString();
            if (dgvSubMenu.Rows[e.RowIndex].Cells["HasChildData"].Value.ToString() == "True")
                rbHasChildDataSubMenu.Checked = true;
            else if (dgvSubMenu.Rows[e.RowIndex].Cells["HasGraph"].Value.ToString() == "True")
                rbHasGraphSubMenu.Checked = true;
            txtzoomQSub.Text = dgvSubMenu.Rows[dgvSubMenu.Rows.Count - 1].Cells["ZoomQuery"].Value.ToString();

            lblIndex.Text = e.RowIndex.ToString();
            btnAddSubMenu.Text = "Update SubMenu";
            btnCancelSubmenu.Visible = true;
        }
        private void rbHasChildData_CheckedChanged(object sender, EventArgs e)
        {
        }
        private void rbIsDashboard_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIsDashboard.Checked == true)
            {
                tabControl1.TabPages.Add(tabPage2);
                txtQuery.Text = "";
            }
            else
            {
                tabControl1.TabPages.Remove(tabPage2);
                if (lblid.Text != "0")
                {
                    Queries.Add("delete from DashboardMaster where MenuID = " + lblid.Text + "");
                }
            }
        }
        private void rbHasSubMenu_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHasSubMenu.Checked == true)
                tabControl1.TabPages.Add(tabPage3);
            else
            {
                tabControl1.TabPages.Remove(tabPage3);
                //txtQuery.Text = "";
                if (lblid.Text != "0")
                {
                    Queries.Add("delete from ChildMenuMaster where MenuID = " + lblid.Text + "");
                }
            }

        }

        private void rbHasGraph_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelSubmenu_Click(object sender, EventArgs e)
        {
            clear_subMenu();
            //  btnCancelSubmenu.Visible = false;

        }

        private void btnCancelDash_Click(object sender, EventArgs e)
        {
            clearDash();
        }

    }
}