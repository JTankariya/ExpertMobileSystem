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
using System.IO;
using System.Data.OleDb;
using System.Drawing.Drawing2D;

namespace ExpertMobileSystem_Client_
{
    public partial class frmCompanyMaster : Form
    {
        public string filename = Guid.NewGuid().ToString();
        //bool blEventExit = false;
        //string Reporthead = "Party";
        //int partyType = 0;
        //bool Loaded = false;
        //private Thread WaitingThread;

        public frmCompanyMaster()
        {
           
            InitializeComponent();
            Load += frmPartymaster_Load;
            try
            { this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\MOBILE.ico"); }
            catch { }
           
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
        private void txtpostalcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void txtphone1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void txtphone2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void txtmobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)8))
                e.Handled = true;
        }


        private void frmPartymaster_Load(object sender, EventArgs e)
        {
            Application.DoEvents();
            Paint += draw;
            Invalidate();
            // Operation.SetEventHandlers(groupBox5,ControlMessage);
            this.Location = new Point(this.Location.X + 15, this.Location.Y);
            btnAdd_Click(sender, e);
            // Operation.SetEventHandlers(groupBox5,ControlMessage);
            this.Location = new Point(this.Location.X + 15, this.Location.Y);

            //if (frmMDI.partyMaster == 1)
            //{
            //    this.Text = "Party Master";
            //    lblPArtyNAme.Text = "Party Name";
            //    partyType = Convert.ToInt32(Operation.PartyType.Party);
            //    lblAgent.Visible = true;
            //    cmbAgentName.Visible = true;
            //}
            //else if (frmMDI.partyMaster == 2)
            //{
            //    this.Text = "Vendor Master";
            //    lblPArtyNAme.Text = "Vendor Name";
            //    partyType = Convert.ToInt32(Operation.PartyType.Vendor);
            //    lblAgent.Visible = true;
            //    cmbAgentName.Visible = true;
            //}
            //else if (frmMDI.partyMaster == 3)
            //{
            //    this.Text = "Agent Master";
            //    lblPArtyNAme.Text = "Agent Name";
            //    partyType = Convert.ToInt32(Operation.PartyType.Agent);
            //    lblAgent.Visible = false;
            //    cmbAgentName.Visible = false;
            //}
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

            if (!File.Exists(txtpath.Text + "\\Company.dbf"))
            {
                MessageBox.Show("Company.dbf file was not found in Path :" + txtpath.Text + ". \n please Select Path First");
                txtpath.Focus();
                return false;
            }
            if (txtcompCode.Text == "0")
            {
                MessageBox.Show("Please Select Company First..", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectedIndex = 0;
                return false;
            }
            if (lblid.Text == "0")
            {
                if (!CheckMaximumCompany())
                {
                    return false;

                }
            }

            //if (!Regex.IsMatch(txtemail.Text, "\\b[A-Z0-9._%-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}\\b", RegexOptions.IgnoreCase) && txtemail.Text != "")
            //{
            //    MessageBox.Show("Please Enter Valid Email Address.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    txtemail.Focus();
            //    return false;
            //}

            return true;
        }
        private bool CheckMaximumCompany()
        {
            if (Operation.objComp.NoOfCompanyPerUser <= Operation.objComp.TotalCreatedCompany)
            {
                MessageBox.Show("You have Created Maximum Number Of Alloted Company.\n To Add More user Please Contact Administrator.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            // bindcombo();
            dgvCompany.DataSource = null;
            lblid.Text = "0";
            txtcompCode.Text = "0";
            txtcompname.Text = "";
            dtpfromdate.Value = Operation.CurrentDate;
            dtptodate.Value = Operation.CurrentDate;
            txtpath.Text = "";
            txtadd1.Text = "";
            txtadd2.Text = "";
            txtadd3.Text = "";
            txtadd4.Text = "";
            txtcompmobile.Text = "";
            txtcompphone.Text = "";
            txtcompVAT.Text = "";
            txtcompemail.Text = "";
            txtwebsite.Text = "";
            txtcompCST.Text = "";
            txtcompITNO.Text = "";
            txtcompLICNO.Text = "";
            txtTANNO.Text = "";
            txtCompAutho.Text = "";
            txtremark.Text = "";

            btnDelete.Enabled = false;

        }

        private bool isAlreadyExistCompany()
        {
            string Record = Operation.ExecuteScalar("select clientcompanyid from ClientCompanyMaster where companyCode = " + txtcompCode.Text + " and ClientId=" + Operation.Clientid + " and ExpertPath='" + txtpath.Text.Replace("\\", "\\\\") + "'", Operation.Conn) != null ? Operation.ExecuteScalar("select clientcompanyid from ClientCompanyMaster where companyCode = " + txtcompCode.Text + " and ClientId=" + Operation.Clientid + " and ExpertPath='" + txtpath.Text.Replace("\\", "\\\\") + "'", Operation.Conn).ToString() : "0";
            if (lblid.Text == "0")
            {
                if (Record == "0")
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Comapny Already Exist,Please Select Another Company.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                if (Record == "0")
                {
                    return true;
                }
                else
                {
                    if (lblid.Text != Record.ToString())
                    {
                        MessageBox.Show("Comapny Already Exist,Please Select Another Company.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!Validate_form())
                return;
//            string pathmysql = txtpath.Text.Replace("\\", "_");
            string pathmysql = txtpath.Text.Replace("\\","\\\\");
            btnSave.Enabled = false;
            try
            {
                ArrayList Queries = new ArrayList();
                //  int NoOfDays  = Convert.ToInt32( ((dtpdate.Value - DateTime.Today).Days));
                //int city, state, country;
                //city = Operation.SetVar(cmbcity, "City");
                //state = Operation.SetVar(cmbstate, "State");
                //country = Operation.SetVar(cmbcountry, "Country");

                if (lblid.Text == "0")
                {
                    if (!isAlreadyExistCompany())
                        return;
                    Queries.Add("insert into ClientCompanyMaster ( ClientID,CompanyName,CompanyFromDate,CompanyToDate,CompanyAdd1,CompanyAdd2,CompanyAdd3,CompanyAdd4 " +
                        ",CompanyPhone, CompanyMobileNo, CompanyEmail, CompanyWebsite, CompanyVAT, CompanyCST, CompanyITNO, CompanyLICNO, CompanyTANNO, CompanyAuthorised, CompanyRemarks, ExpertPath, CompanyCode) " +
                       " values(" + Operation.objComp.ClientId + ",'" + txtcompname.Text + "','" + dtpfromdate.Value.ToString("yyyy-MM-dd") + "','" + dtptodate.Value.ToString("yyyy-MM-dd") + "'" +
                       ",'" + txtadd1.Text + "','" + txtadd2.Text + "','" + txtadd3.Text + "','" + txtadd4.Text + "','" + txtcompphone.Text + "','" + txtcompmobile.Text + "'" +
                        ",'" + txtcompemail.Text + "','" + txtwebsite.Text + "','" + txtcompVAT.Text + "','" + txtcompCST.Text + "','" + txtcompITNO.Text + "','" + txtcompLICNO.Text + "','" + txtTANNO.Text + "','" + txtCompAutho.Text + "','" + txtremark.Text + "','" + pathmysql + "'," + txtcompCode.Text + ")");
                }
                else
                {
                    if (!isAlreadyExistCompany())
                        return;
                    string Update = "update  ClientCompanyMaster set  CompanyName='" + txtcompname.Text + "',CompanyFromDate='" + dtpfromdate.Value.ToString("yyyy-MM-dd") + "',CompanyToDate='" + dtptodate.Value.ToString("yyyy-MM-dd") + "'" +
                    ",CompanyAdd1='" + txtadd1.Text + "',CompanyAdd2='" + txtadd2.Text + "',CompanyAdd3='" + txtadd3.Text + "',CompanyAdd4='" + txtadd4.Text + "' " +
                        ",CompanyPhone='" + txtcompphone.Text + "', CompanyMobileNo='" + txtcompmobile.Text + "', CompanyEmail='" + txtcompemail.Text + "', CompanyWebsite='" + txtwebsite.Text + "', CompanyVAT='" + txtcompVAT.Text + "', CompanyCST='" + txtcompCST.Text + "', CompanyITNO='" + txtcompITNO.Text + "', CompanyLICNO='" + txtcompLICNO.Text + "'" +
                        ", CompanyTANNO='" + txtTANNO.Text + "', CompanyAuthorised='" + txtCompAutho.Text + "', CompanyRemarks='" + txtremark.Text + "', ExpertPath='" + pathmysql + "',CompanyCode='" + txtcompCode.Text + "' Where ClientCompanyID = " + lblid.Text + "";
                    Queries.Add(Update);
                }
                if (Operation.ExecuteTransaction(Queries, Operation.Conn))
                {
                    MessageBox.Show("Record Saved Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (lblid.Text == "0")
                        IncreaseCreatedCompanyCount();
                    btnAdd_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Error while Saving, Please Try after Some Time.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //if ((this.Modal == true))
                    //    this.Dispose();
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
            //if ((this.Modal == true))
            //    this.Dispose();
        }
        private bool IncreaseCreatedCompanyCount()
        {
            ArrayList IncreaseQ = new ArrayList();
            Operation.objComp.TotalCreatedCompany += 1;

            IncreaseQ.Add("update ClientMaster set TotalCreatedCompany = " + Operation.objComp.TotalCreatedCompany + " where ClientID = " + Operation.objComp.ClientId + "");
            if (!Operation.ExecuteTransaction(IncreaseQ, Operation.Conn))
                return false;
            else
                return true;
        }
        private bool DecreaseCreatedCompanyCount()
        {

            ArrayList DecreaseQ = new ArrayList();
            Operation.objComp.TotalCreatedCompany -= 1;

            DecreaseQ.Add("update ClientMaster set TotalCreatedCompany = " + Operation.objComp.TotalCreatedCompany + " where ClientID = " + Operation.objComp.ClientId + "");
            if (!Operation.ExecuteTransaction(DecreaseQ, Operation.Conn))
                return false;
            else
                return true;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            //if (!(Operation.RightsStr.Contains(Convert.ToInt32(Operation.Rights.VIEW).ToString())))
            //{
            //    MessageBox.Show("You don't have rights, Please contact your Administrator.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //if (!(lblid.Text == "0"))
            //{
            //    Operation.CheckLock("partymaster,link," + lblid.Text, true);
            //}
            //else
            //{
            //    Operation.ExecuteNonQuery("Update partymaster set InEdit=0 where link=" + lblid.Text, Operation.Conn);
            //}
            fillgrid();

            tabControl1.SelectedIndex = 1;

        }
        private void fillgrid()
        {
            frmSearch view = new frmSearch();
            Operation.gViewQuery = "SELECT ClientCompanyId, CompanyName, CompanyFromDate, CompanyToDate, ExpertPath, CompanyAdd1, CompanyAdd2, CompanyAdd3, CompanyAdd4, CompanyPhone, CompanyVAT, CompanyMobileNo, CompanyEmail, CompanyWebsite, CompanyCST, CompanyITNO, CompanyLICNO, CompanyTANNO,CompanyAuthorised, CompanyRemarks FROM ClientCompanyMaster where ClientId = " + Operation.Clientid + "";//

            ////            view.dgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            Operation.Bindgrid(Operation.gViewQuery, view.dgvSearch);
            view.dgvSearch.Columns[0].Visible = false;
            view.OrderByColoumn = "CompanyFromDate";
            view.ShowDialog();

            view.dgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            if (Operation.ViewID != null && Operation.ViewID != string.Empty)
            {
                //if (!Operation.CheckLock("Partymaster,link", false))
                //    return;
                filldata();
                Operation.ViewID = "";
            }
        }
        public void filldata()
        {
            filldata(Operation.GetDataTable("Select * from ClientCompanyMaster where ClientCompanyId=" + Operation.ViewID.ToString(), Operation.Conn));
        }
        private void filldata(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {

                    lblid.Text = dt.Rows[0]["ClientCompanyId"].ToString();
                    txtcompname.Text = dt.Rows[0]["CompanyName"].ToString();
                    dtpfromdate.Value = Convert.ToDateTime(dt.Rows[0]["CompanyFromDate"]);
                    dtptodate.Value = Convert.ToDateTime(dt.Rows[0]["CompanyToDate"]);
                    txtpath.Text = dt.Rows[0]["ExpertPath"].ToString();
                    string newpath = txtpath.Text.Replace("_", "\\");
                    txtpath.Text = newpath;
                    txtadd1.Text = dt.Rows[0]["CompanyAdd1"].ToString();
                    txtadd2.Text = dt.Rows[0]["CompanyAdd2"].ToString();
                    txtadd3.Text = dt.Rows[0]["CompanyAdd3"].ToString();
                    txtadd4.Text = dt.Rows[0]["CompanyAdd4"].ToString();
                    txtcompmobile.Text = dt.Rows[0]["CompanyMobileNo"].ToString();
                    txtcompphone.Text = dt.Rows[0]["CompanyPhone"].ToString();
                    txtcompVAT.Text = dt.Rows[0]["CompanyVAT"].ToString();
                    txtcompemail.Text = dt.Rows[0]["CompanyEmail"].ToString();
                    txtwebsite.Text = dt.Rows[0]["CompanyWebsite"].ToString();
                    txtcompCST.Text = dt.Rows[0]["CompanyCST"].ToString();
                    txtcompITNO.Text = dt.Rows[0]["CompanyITNO"].ToString();
                    txtcompLICNO.Text = dt.Rows[0]["CompanyLICNO"].ToString();
                    txtTANNO.Text = dt.Rows[0]["CompanyTANNO"].ToString();
                    txtCompAutho.Text = dt.Rows[0]["CompanyAuthorised"].ToString();
                    txtremark.Text = dt.Rows[0]["CompanyRemarks"].ToString();
                    txtcompCode.Text = dt.Rows[0]["CompanyCode"].ToString();
                    btnDelete.Enabled = true;
                    //btnSave.Text = "Update"
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
                //ElseIf MsgBox("Are you sure you want to Delete the record?", MsgBoxStyle.YesNo, "Shah Software") = MsgBoxResult.Yes Then
            }
            //else if (!(Operation.RightsStr.Contains(Convert.ToInt32(Operation.Rights.DELETE).ToString())))
            //{
            //    MessageBox.Show("You don't have rights, Please contact your Administrator.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            else if (!Operation.CheckReference(Convert.ToInt32(lblid.Text), "ClientUserCompanyMaster,ClientCompanyID"))
                return;
            else if (MessageBox.Show("Are you sure you want to delete this record?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = null;
                query = "Delete from ClientCompanyMaster where ClientCompanyID= " + lblid.Text.Trim();
                Operation.ExecuteNonQuery(query, Operation.Conn);
                //  Operation.UserLog(query, this.Name, Operation.Rights.DELETE.ToString());
                MessageBox.Show("Record Deleted Succeessfully.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                DecreaseCreatedCompanyCount();

                lblid.Text = "0";
                btnAdd_Click(sender, e);
                btnDelete.Enabled = false;
            }
        }

        private void frmPartymaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Operation.ExecuteNonQuery("Update PartyMaster set inedit=False where link=" + lblid.Text, Operation.Conn);


        }

        private void cmbAgentName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                frmCompanyMaster p1 = new frmCompanyMaster();
                //  frmMDI.partyMaster = 3;
                p1.ShowDialog();
                //  Operation.BindComboBox(cmbAgentName, "SELECT max(PartyMaster.link) as link, iif(isnull(PartyMaster.partyName),'-- Select Agent Name--',PartyMaster.partyName) as partyName FROM PartyMaster   where (((PartyMaster.partyType)=" + Convert.ToInt32(Operation.PartyType.Agent) + ")) group by partyName order by partyName", "-- Select Agent Name--", "partyName", "link");

            }
        }




        private void txtNoUser_Validated(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text == "")
            {
                txt.Text = "0";


            }
        }


        private void chkviewPass_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkviewPass.Checked == true)
            //{

            //    txtPassword.PasswordChar = '\0';
            //    txtrepass.PasswordChar = '\0';

            //}
            //else
            //{
            //    txtPassword.PasswordChar = '*';
            //    txtrepass.PasswordChar = '*';


            //}
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();


            // OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Filter = "Excel Files (*.xls)|*.xls";
            //   dialog.InitialDirectory = "C:\\";
            //dialog.Title = "Select a Expert Folder";
            dialog.SelectedPath = (txtpath.Text == "" ? "C:\\" : txtpath.Text) ;
            //dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string ExpertFolder = dialog.SelectedPath;
                // txtpath.Text = System.IO.File.ReadAllText(fname);
                txtpath.Text = ExpertFolder;
                string dir = Path.GetDirectoryName(ExpertFolder);
                // && (ExpertFolder.Replace(dir, "").ToUpper() == "EXPERT" || ExpertFolder.Replace(dir, "").ToUpper() == "EXPERTLN" || ExpertFolder.Replace(dir, "").ToUpper() == "EXPHV" || ExpertFolder.Replace(dir, "").ToUpper() == "EXPHVLN")
                if (dir != null)
                {
                    if (!File.Exists(txtpath.Text + "\\Company.dbf"))
                    {
                        MessageBox.Show("Company.dbf file was not found in Path :" + txtpath.Text);
                        return;
                    }
                    else
                    {
                        CreateExpertComapanyConnection(txtpath.Text);
                        FillCompanyGrid();

                    }
                }
                else
                {
                    MessageBox.Show("Select Expert Software Only.");
                }
                //txtpath.Text = new DirectoryInfo(ExpertFolder).GetDirectories().ToString();

            }
        }
        private void CreateExpertComapanyConnection(string path)
        {

            LocalConnection.ExpertCompanyConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Mode=ReadWrite;Extended Properties=dBase IV;Persist Security Info=False");
        }
        private void FillCompanyGrid()
        {

            string Q = "Select Code,Name As [Company Name],Format(DFR,'dd/MM/yyyy') as [From Date],Format(DTO,'dd/MM/yyyy') as [To Date] from COMPANY order by Name,DFR,DTO";

            LocalConnection.Bindgrid(Q, dgvCompany);
            dgvCompany.Columns[0].Visible = false;

            dgvCompany.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;


        }

        private void dgvCompany_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgvCompany.CurrentRow.Index;
            DataTable dt = LocalConnection.GetDataTable("select * from company where code = " + dgvCompany.Rows[i].Cells["CODE"].Value + "", LocalConnection.ExpertCompanyConn);
            if (dt.Rows.Count > 0)
            {
                txtcompname.Text = dt.Rows[0]["NAME"].ToString();
                dtpfromdate.Value = Convert.ToDateTime(dt.Rows[0]["DFR"]);
                dtptodate.Value = Convert.ToDateTime(dt.Rows[0]["DTO"]);

                txtadd1.Text = dt.Rows[0]["ADD1"].ToString();
                txtadd2.Text = dt.Rows[0]["ADD2"].ToString();
                txtadd3.Text = dt.Rows[0]["ADD3"].ToString();
                txtcompmobile.Text = dt.Rows[0]["MOBILE"].ToString();
                txtcompphone.Text = dt.Rows[0]["PHONE"].ToString();
                txtcompemail.Text = dt.Rows[0]["EMAIL"].ToString();
                txtwebsite.Text = dt.Rows[0]["WEBSITE"].ToString();
                txtcompVAT.Text = dt.Rows[0]["VATNO"].ToString();
                txtcompCST.Text = dt.Rows[0]["CSTNO"].ToString();
                txtcompITNO.Text = dt.Rows[0]["ITNO"].ToString();
                txtcompLICNO.Text = dt.Rows[0]["LICNO"].ToString();
                txtTANNO.Text = dt.Rows[0]["TAN"].ToString();
                txtCompAutho.Text = dt.Rows[0]["AUTH_SIGN"].ToString();
                txtremark.Text = dt.Rows[0]["REMARKS"].ToString();
                txtcompCode.Text = dt.Rows[0]["CODE"].ToString();



            }
            tabControl1.SelectedIndex = 1;
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }
    }
}