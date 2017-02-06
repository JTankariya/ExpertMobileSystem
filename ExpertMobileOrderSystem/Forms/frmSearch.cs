using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ExpertMobileOrderSystem
{
    public partial class frmSearch : Form
    {
        int[] w =new int[0];
        int notFoundIncolumns = 0;
        public  string OrderByColoumn = "";

        public frmSearch()
        {
            InitializeComponent();
            try
            { this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\MOBILE.ico"); }
            catch { }
        }
        public frmSearch(string Query)
        {
             
            InitializeComponent();
            DataTable dt = Operation.GetDataTable(Query,Operation.Conn);        

            DataGridViewCheckBoxColumn chkSelect = new DataGridViewCheckBoxColumn();
            chkSelect.Name = "chkSelect";
            chkSelect.HeaderText = "Select";
            dgvSearch.ReadOnly = false;
            dgvSearch.Columns.Insert(0, chkSelect);
            
            dgvSearch.DataSource = dt;
            

            for (int i = 0; i < dgvSearch.Columns.Count; i++)
            {
                if (dgvSearch.Columns[i].HeaderText.ToUpper().Contains("GROUP") || dgvSearch.Columns[i].HeaderText.ToUpper().Contains("CODE"))
                    dgvSearch.Columns[i].Visible = false;
            }
            dgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //Shown += new EventHandler(Form1_Shown);
            //dgvSalesAcc.Columns["NAME"].Width = 310;
        }

        private void LoadCode()
        {
            this.Cursor = Cursors.WaitCursor;
            this.KeyPreview = true;
            fillsearchoncombo();
            this.Cursor = Cursors.Default;
        }

        private void fillsearchoncombo()
        {
            DataTable dt = default(DataTable);
            if (Operation.gViewQuery.ToLower().Contains("where"))
            {
                dt = Operation.GetDataTable(Operation.gViewQuery, Operation.Conn);
            }
            else
            {
                if (!Operation.gViewQuery.Contains("Company_Master"))
                {
                    if (!Operation.gViewQuery.Contains("[Process Priority]"))
                    {
                        dt = Operation.GetDataTable(Operation.gViewQuery + " where 1 = 2", Operation.Conn);
                    }
                    else
                    {
                        dt = Operation.GetDataTable(Operation.gViewQuery + " where 1=2 Order by a.[Process Priority]", Operation.Conn);
                    }

                }
                else
                {
                    dt = Operation.GetDataTable(Operation.gViewQuery + " where 1 = 2", Operation.Conn);
                }

            }
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ToString().ToUpper().Contains("LINK"))
                {
                    continue;
                }
                cmbsearchon.Items.Add(column.Caption);
            }
            cmbsearchon.Items.Insert(0, "All");
            if (cmbsearchon.Items.Count > 2)
            {
                cmbsearchon.SelectedIndex = 0;
                //if (!Operation.gViewQuery.Contains("Priority"))
                //{
                //    dgvSearch.Sort(dgvSearch.Columns[cmbsearchon.Text], System.ComponentModel.ListSortDirection.Descending);
                //    //dgvSearch.Sort(dgvSearch.Columns["Doc Date"], System.ComponentModel.ListSortDirection.Descending);

                //}
                if (cmbsearchon.Text != "All")
                {
                    dgvSearch.Sort(dgvSearch.Columns[cmbsearchon.Text], System.ComponentModel.ListSortDirection.Ascending);
                }
                else
                {
                    dgvSearch.Sort(dgvSearch.Columns[OrderByColoumn], System.ComponentModel.ListSortDirection.Descending);


                }
                cmbsearchon.SelectedIndex = 0;
            }
            else
            {
                cmbsearchon.SelectedIndex = 0;
            }
        }

        private void frmSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (dgvSearch.Rows.Count > 0 & e.KeyCode == Keys.Enter)
                {
                    this.Cursor = Cursors.WaitCursor;
                    Operation.ViewID = dgvSearch.CurrentRow.Cells[0].Value.ToString();
                    this.Cursor = Cursors.Default;
                    this.Dispose();
                }
                else if (!dgvSearch.Focused && dgvSearch.Rows.Count > 0 && e.KeyCode == Keys.Up && dgvSearch.CurrentRow.Index > 0)
                {
                    dgvSearch.CurrentCell = dgvSearch.Rows[dgvSearch.CurrentRow.Index - 1].Cells[dgvSearch.CurrentCell.ColumnIndex];
                }
                else if (!dgvSearch.Focused && dgvSearch.Rows.Count > 0 && e.KeyCode == Keys.Down && dgvSearch.CurrentRow.Index < dgvSearch.Rows.Count)
                {
                    dgvSearch.CurrentCell = dgvSearch.Rows[dgvSearch.CurrentRow.Index + 1].Cells[dgvSearch.CurrentCell.ColumnIndex];
                }
            }
            catch
            {
            }
            finally
            {
                this.Focus();
            }
        }

        private void frmSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Escape))
            {
                this.Dispose();
            }
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void search()
        {
            if (txtsearch.Text != string.Empty)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    SearchGridValue(dgvSearch, cmbsearchon.Text, txtsearch.Text, chkisglobal.Checked);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                Operation.Bindgrid(Operation.gViewQuery, dgvSearch);
                dgvSearch.Columns[0].Visible = false;
            }
            if (cmbsearchon.Text != "All")
            {
                dgvSearch.Sort(dgvSearch.Columns[cmbsearchon.Text], System.ComponentModel.ListSortDirection.Ascending);
            }
            //else
            //{
            //    dgvSearch.Sort(dgvSearch.Columns[OrderByColoumn], System.ComponentModel.ListSortDirection.Descending);
            
            
            //}
        }

        private bool SearchGridValue(DataGridView dtg, string ColumnName, string ValueToSearch, bool isglobal)
        {
            bool Found = false;
            bool FoundOnce = false;
            string StringToSearch = "";
            string ValueToSearchFor = ValueToSearch.Trim().ToLower();
            try
            {
                for (int i = 0; i <= dtg.RowCount - 1; i++)
                {
                    //if (Found == true)
                    //    break;
                    Found = false;
                    notFoundIncolumns = 0;
                    if (isglobal)
                    {
                        if (ColumnName == "All")
                        {
                            for (int colindex = 1; colindex <= dtg.ColumnCount - 1; colindex++)
                            {
                                
                                    StringToSearch = dtg.Rows[i].Cells[colindex].Value.ToString().Trim().ToLower();
                                    if (StringToSearch.Contains(ValueToSearchFor))
                                    {
                                        if (!FoundOnce)
                                        {
                                            if(dtg.Rows[i].Cells[1].Visible==true)
                                                dtg.CurrentCell = dtg.Rows[i].Cells[1];
                                           
                                        }
                                        //  dtg.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                                        Found = true;
                                        FoundOnce = true;
                                    }
                                    else
                                    {
                                        dtg.Rows[i].DefaultCellStyle.BackColor = Color.White;
                                        notFoundIncolumns++;
                                    }
                                    if (Found)
                                    {
                                        break; // TODO: might not be correct. Was : Exit For
                                    }
                                }                            
                        }
                        else
                        {
                            StringToSearch = dtg.Rows[i].Cells[ColumnName].Value.ToString().Trim().ToLower();
                            if (StringToSearch.Contains(ValueToSearchFor))
                            {
                                if (!FoundOnce)
                                {
                                    dtg.CurrentCell = dtg.Rows[i].Cells[1];
                                }
                            //    dtg.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                                Found = true;
                                FoundOnce = true;
                            }
                            else
                            {
                                dtg.Rows[i].DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                    }
                    else
                    {
                        Found = false;
                        if (ColumnName == "All")
                        {
                            for (int colindex = 1; colindex <= dtg.ColumnCount - 1; colindex++)
                            {
                                StringToSearch = dtg.Rows[i].Cells[colindex].Value.ToString().Trim().ToLower();
                                if (StringToSearch.Length >= ValueToSearchFor.Length)
                                {
                                    StringToSearch = StringToSearch.Substring(0, ValueToSearchFor.Length);
                                }
                                if (StringToSearch == ValueToSearchFor)
                                {
                                    if (!FoundOnce)
                                    {
                                        if(dtg.Rows[i].Cells[1].Visible==true)
                                            dtg.CurrentCell = dtg.Rows[i].Cells[1];
                                    }
                                 //   dtg.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                                    Found = true;
                                    FoundOnce = true;
                                }
                                else
                                {
                                    dtg.Rows[i].DefaultCellStyle.BackColor = Color.White;
                                    notFoundIncolumns++;
                                    //New added to display only searched record.
                                  
                                }
                                if (Found)
                                {
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }

                        }
                        else
                        {
                            StringToSearch = dtg.Rows[i].Cells[ColumnName].Value.ToString().Trim().ToLower();
                            if (StringToSearch.Length >= ValueToSearchFor.Length)
                            {
                                StringToSearch = StringToSearch.Substring(0, ValueToSearchFor.Length);
                            }
                            if (StringToSearch == ValueToSearchFor)
                            {
                                if (!FoundOnce)
                                {
                                    dtg.CurrentCell = dtg.Rows[i].Cells[ColumnName];
                                }
                           //     dtg.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                               
                              


                                dtg.Rows[i].Visible = true;

                                Found = true;
                                FoundOnce = true;
                            }
                            else
                            {
                                dtg.Rows[i].DefaultCellStyle.BackColor = Color.White;

                                //New added to display only searched record.

                                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dtg.DataSource];
                                currencyManager1.SuspendBinding();
                                dtg.Rows[i].Visible = false;
                                currencyManager1.ResumeBinding();
                              
                            }
                        }
                    }

                    if (ColumnName == "All" && notFoundIncolumns == dtg.ColumnCount - 1)
                    {
                        CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dtg.DataSource];
                        currencyManager1.SuspendBinding();
                        dtg.Rows[i].Visible = false;
                        currencyManager1.ResumeBinding();
                    }
                    else
                    {
                        CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dtg.DataSource];
                        currencyManager1.SuspendBinding();
                        dtg.Rows[i].Visible = true;
                        currencyManager1.ResumeBinding();
                    }
                }
                if (!FoundOnce)
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    for (int i = 0; i <= dtg.Rows.Count - 1; i++)
                    {
                        dtg.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return Found;
         }

        private void cmbsearchon_SelectionChangeCommitted(object sender, EventArgs e)
        {
            search();
        }

        private void dgvSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if ((dgvSearch.Columns[0].HeaderText != "Select"))
                {
                    this.Cursor = Cursors.WaitCursor;
                    Operation.ViewID = dgvSearch.CurrentRow.Cells[0].Value.ToString();
                    //gForm.Activate()
                    this.Cursor = Cursors.Default;
                    this.Dispose();
                }
                else
                {
                    for (int i = 0; i <= dgvSearch.Rows.Count - 1; i++)
                    {
                        if (Convert.ToBoolean(dgvSearch.Rows[i].Cells["chkSelect"].EditedFormattedValue))
                        {
                            if ((string.IsNullOrEmpty(Operation.MultiViewId)))
                            {
                                Operation.MultiViewId = dgvSearch.Rows[i].Cells[1].Value.ToString() + ",";
                            }
                            else
                            {
                                Operation.MultiViewId = Operation.MultiViewId + dgvSearch.Rows[i].Cells[1].Value.ToString() + ",";
                            }
                        }
                    }
                    this.Dispose();
                }
            }
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            LoadCode();
            w = new int[dgvSearch.Columns.Count];
            dgvSearch.ColumnWidthChanged += dgvSearch_ColumnWidthChanged;

            //to know what WERE the widths.
            for (int i = 0; i < dgvSearch.Columns.Count; i++)
            {
                
                w[i] = dgvSearch.Columns[i].Width;
            }

            //Won't work without this.
            dgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        }

        private void dgvSearch_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            //int column = e.Column.Index;

            ////unsubscribe so changing will not call itself.
            //dgvSearch.ColumnWidthChanged -= dgvSearch_ColumnWidthChanged;

            ////updating width.
            //dgvSearch.Columns[column + 1].Width = (w[column + 1] + w[column]) - dgvSearch.Columns[column].Width;
            //w[column + 1] = dgvSearch.Columns[column + 1].Width;
            //w[column] = dgvSearch.Columns[column].Width;

            ////re-subscribe
            //dgvSearch.ColumnWidthChanged += dgvSearch_ColumnWidthChanged;
            //for (int i = 0; i < dgvSearch.Columns.Count; i++)
            //{
            //    dgvSearch.Columns[e.Column.Index].FillWeight = e.Column.FillWeight;
            //}
        }

        private void frmSearch_Resize(object sender, EventArgs e)
        {
            dgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
    }
}
