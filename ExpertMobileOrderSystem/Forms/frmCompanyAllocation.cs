using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Collections;
using System.Drawing.Printing;
using System.Threading;
using System.Drawing.Drawing2D;

namespace ExpertMobileOrderSystem
{
    public partial class frmCompanyAllocation : Form
    {
        string User_ID = "0";
        int User_index;
        int SelectedCompany = 0;
        ArrayList Query = new ArrayList();
        public frmCompanyAllocation()
        {
            InitializeComponent();
            try
            { this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\MOBILE.ico"); }
            catch { }
        }
        private void Bind_UserGrid()
        {
            string Q = "select ClientUserid, FirstName, LastName, MobileNo, CreatedDate, AccountExpiredOn  from [Order.ClientUserMaster] where Clientid = " + Operation.objComp.ClientId + " order by FirstName, LastName, CreatedDate, AccountExpiredOn";
            Operation.Bindgrid(Q, dgvUser);
            dgvUser.Columns[0].Visible = false;
            DataGridViewCheckBoxColumn chkSelect = new DataGridViewCheckBoxColumn();
            chkSelect.Name = "chkSelect";
            chkSelect.HeaderText = "Select";
            chkSelect.CellTemplate = new DataGridViewCheckBoxCell(false);
            dgvUser.ReadOnly = false;
            dgvUser.Columns.Insert(1, chkSelect);
            for (int i = 0; i < dgvUser.Columns.Count; i++)
            {
                if (i == 1)
                    dgvUser.Columns[i].ReadOnly = false;
                else
                    dgvUser.Columns[i].ReadOnly = true;
            }
            dgvUser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private void Bind_CompanyGrid()
        {
            string Q = "select ClientCompanyId, CompanyName, CompanyFromDate, CompanyToDate,CompanyPhone, CompanyMobileNo  from [Order.ClientCompanyMaster] where Clientid = " + Operation.objComp.ClientId + " order by CompanyName, CompanyFromDate, CompanyToDate ";
            Operation.Bindgrid(Q, dgvCompany);
            dgvCompany.Columns[0].Visible = false;
            DataGridViewCheckBoxColumn chkSelect = new DataGridViewCheckBoxColumn();
            chkSelect.Name = "chkSelect";
            chkSelect.HeaderText = "Select";
            chkSelect.CellTemplate = new DataGridViewCheckBoxCell(false);
            dgvCompany.ReadOnly = false;
            dgvCompany.Columns.Insert(1, chkSelect);
            for (int i = 0; i < dgvCompany.Columns.Count; i++)
            {
                if (i == 1)
                    dgvCompany.Columns[i].ReadOnly = false;
                else
                    dgvCompany.Columns[i].ReadOnly = true;
            }
            dgvCompany.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private void Bind_DetailGrid()
        {
            string Q = "SELECT [Order.ClientUserCompanyMaster].ClientUserCompanyid,[Order.ClientUserCompanyMaster].ClientUserid," +
                "[Order.ClientUserCompanyMaster].ClientCompanyid,CompanyName, CompanyFromDate, CompanyToDate, CompanyAdd1, CompanyAdd2, " +
                "CompanyPhone ,FirstName, LastName, MobileNo FROM [Order.ClientUserCompanyMaster] " +
                "left join [Order.ClientUserMaster] on [Order.ClientUserCompanyMaster].ClientUserid = [Order.ClientUserMaster].ClientUserid " +
                "left join [Order.ClientCompanyMaster] on [Order.ClientUserCompanyMaster].ClientCompanyid = [Order.ClientCompanyMaster].ClientCompanyId" +
                " where [Order.ClientUserCompanyMaster].Clientid = " + Operation.objComp.ClientId;
            DataTable dt = Operation.GetDataTable(Q, Operation.Conn);
            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = " FirstName, LastName ,CompanyName, CompanyFromDate, CompanyToDate";
                dt = dt.DefaultView.ToTable(false);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvDetail.Rows.Add();
                    dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["Link"].Value = dt.Rows[i]["ClientUserCompanyid"];
                    dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["UserID"].Value = dt.Rows[i]["ClientUserid"];
                    dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["FirstName"].Value = dt.Rows[i]["FirstName"];
                    dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["LastName"].Value = dt.Rows[i]["LastName"];
                    dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["MobileNo"].Value = dt.Rows[i]["MobileNo"];
                    dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["CompanyID"].Value = dt.Rows[i]["ClientCompanyid"];
                    dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["CompanyName"].Value = dt.Rows[i]["CompanyName"];
                    dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["FromDate"].Value = dt.Rows[i]["CompanyFromDate"];
                    dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["ToDate"].Value = dt.Rows[i]["CompanyToDate"];
                }
            }
            Set_Srno(dgvDetail);
        }
        private void frmPartymaster_Load(object sender, EventArgs e)
        {
            Bind_UserGrid();
            Bind_CompanyGrid();
            Bind_DetailGrid();
            btnAdd_Click(sender, e);
            Paint += draw;
            Invalidate();
        }
        public void draw(object sender, PaintEventArgs e)
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


        private bool Validate_form()
        {
            if ((!IsSelectedRows(dgvUser)) || (!IsSelectedRows(dgvCompany)))
            {
                MessageBox.Show("Please Select Atleast One User And Company For Assign....", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                return true;
            }

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            lblid.Text = "0";
            User_ID = "0";
            btnUnselect_Click(null, null);
            btndeselectuser_Click(null, null);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvDetail.Rows.Count == 0)
            {
                MessageBox.Show("Please Assing Company To User.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            btnSave.Enabled = false;
            try
            {
                for (int i = 0; i < dgvDetail.Rows.Count; i++)
                {
                    if (dgvDetail.Rows[i].Cells["Link"].Value == null)
                    {
                        Query.Add("insert into [Order.ClientUserCompanyMaster] (Clientid,ClientUserID,ClientCompanyID ) values(" + 
                            Operation.objComp.ClientId + "," + dgvDetail.Rows[i].Cells["Userid"].Value + "," + dgvDetail.Rows[i].Cells["Companyid"].Value + ")");
                    }
                }
                if (Query.Count != 0)
                {
                    if (Operation.ExecuteTransaction(Query, Operation.Conn))
                    {
                        MessageBox.Show("Record Saved Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnAdd_Click(sender, e);
                        dgvDetail.Rows.Clear();
                        Query.Clear();
                        Bind_DetailGrid();
                    }
                    else
                    {
                        MessageBox.Show("Error while Saving, Please Try after Some Time.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    MessageBox.Show("Nothing to Save.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void SelectExistCompany(string index)
        {
        }
        private void dgvUser_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }
        private void RemoveColor(int Oldindex, DataGridView dgv, int newindex)
        {
        }
        private void btndeselectuser_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvUser.Rows.Count; i++)
            {
                dgvUser.Rows[i].DefaultCellStyle.BackColor = Color.White;
                dgvUser.Rows[i].Cells["chkSelect"].Value = false;
            }
            User_index = 0;
            User_ID = "0";
        }
        private void btnselectAllcomp_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                dgvCompany.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                dgvCompany.Rows[i].Cells["chkSelect"].Value = true;
                SelectedCompany += 1;
            }
        }
        private void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validate_form())
                    return;
                bool isExist = false;
                for (int k = 0; k < dgvUser.Rows.Count; k++)
                {
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        if ((dgvUser.Rows[k].Cells["chkSelect"].Value == null ? "" : dgvUser.Rows[k].Cells["chkSelect"].Value.ToString()) == "True" && (dgvCompany.Rows[i].Cells["chkSelect"].Value == null ? "" : dgvCompany.Rows[i].Cells["chkSelect"].Value.ToString()) == "True")
                        {
                            for (int j = 0; j < dgvDetail.Rows.Count; j++)
                            {
                                if ((dgvUser.Rows[k].Cells["ClientUserid"].Value.ToString() == dgvDetail.Rows[j].Cells["UserID"].Value.ToString()) && (dgvCompany.Rows[i].Cells["ClientCompanyid"].Value.ToString() == dgvDetail.Rows[j].Cells["CompanyID"].Value.ToString()))
                                {
                                    isExist = true;
                                    break;
                                }
                                else
                                    isExist = false;
                            }
                            if (isExist == false)
                            {
                                dgvDetail.Rows.Add();
                                dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["UserID"].Value = dgvUser.Rows[k].Cells["ClientUserid"].Value;
                                dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["FirstName"].Value = dgvUser.Rows[k].Cells["FirstName"].Value;
                                dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["LastName"].Value = dgvUser.Rows[k].Cells["LastName"].Value;
                                dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["MobileNo"].Value = dgvUser.Rows[k].Cells["MobileNo"].Value;
                                dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["CompanyID"].Value = dgvCompany.Rows[i].Cells["ClientCompanyid"].Value;
                                dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["CompanyName"].Value = dgvCompany.Rows[i].Cells["CompanyName"].Value;
                                dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["FromDate"].Value = dgvCompany.Rows[i].Cells["CompanyFromDate"].Value;
                                dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["ToDate"].Value = dgvCompany.Rows[i].Cells["CompanyToDate"].Value;

                            }
                            else
                            {
                                isExist = false;
                            }
                        }
                    }
                }
                Set_Srno(dgvDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void dgvCompany_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (dgvCompany.Rows[dgvCompany.CurrentRow.Index].DefaultCellStyle.BackColor == Color.Green)
                {
                    dgvCompany.Rows[e.RowIndex].Cells["chkselect"].Value = false;
                    dgvCompany.Rows[dgvCompany.CurrentRow.Index].DefaultCellStyle.BackColor = Color.White;
                    SelectedCompany -= 1;
                }
                else
                {
                    dgvCompany.Rows[e.RowIndex].Cells["chkselect"].Value = true;
                    dgvCompany.Rows[dgvCompany.CurrentRow.Index].DefaultCellStyle.BackColor = Color.Green;
                    SelectedCompany += 1;
                }
            }
        }
        private void btnUnselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                dgvCompany.Rows[i].DefaultCellStyle.BackColor = Color.White;
                dgvCompany.Rows[i].Cells["chkSelect"].Value = false;
                SelectedCompany -= 1;
            }
        }
        private void Set_Srno(DataGridView dgv)
        {
            int count = 1;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells["SrNo"].Value = count;
                count++;
            }
        }
        private void dgvDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvDetail.Columns["Delete"].Index)
            {
                if (!Operation.CheckReference(Convert.ToInt32(dgvDetail.Rows[e.RowIndex].Cells["link"].Value), "[Order.ClientUserMenuMaster],ClientUserCompanyID"))
                {
                    return;
                }
                else if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (dgvDetail.Rows[e.RowIndex].Cells["link"].Value != null)
                        Query.Add("delete from [Order.ClientUserCompanyMaster] Where ClientUserCompanyid = " + dgvDetail.Rows[e.RowIndex].Cells["link"].Value + " ");
                    dgvDetail.Rows.RemoveAt(e.RowIndex);
                    Set_Srno(dgvDetail);
                }
            }
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (dgvUser.Rows[dgvUser.CurrentRow.Index].DefaultCellStyle.BackColor == Color.Green)
                {
                    dgvUser.Rows[e.RowIndex].Cells["chkselect"].Value = false;
                    dgvUser.Rows[dgvUser.CurrentRow.Index].DefaultCellStyle.BackColor = Color.White;
                    SelectedCompany -= 1;
                }
                else
                {
                    dgvUser.Rows[e.RowIndex].Cells["chkselect"].Value = true;
                    dgvUser.Rows[dgvUser.CurrentRow.Index].DefaultCellStyle.BackColor = Color.Green;
                    SelectedCompany += 1;
                }
            }
        }

        private void btnSelectAllUser_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvUser.Rows.Count; i++)
            {
                dgvUser.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                dgvUser.Rows[i].Cells["chkSelect"].Value = true;
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

    }
}