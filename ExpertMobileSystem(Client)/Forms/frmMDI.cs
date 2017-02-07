using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Net;
using System.Data.SqlClient;
using System.Threading;
using System.Linq;
using ExpertMobileSystem_Client_.Classes;
using ExpertMobileSystem_Client_.enums;

namespace ExpertMobileSystem_Client_
{
    public partial class frmMDI : Form
    {
        ArrayList Queries = new ArrayList();
        List<String> Images = new List<string>();
        int ImageCount = 0;
        bool CloseFlag = false;
        public static int stmt_flag = 0;
        public static int inward_flag = 0;
        public static Int32 ClientCompanyId = 0;
        public static DataTable dtACT = new DataTable();
        public static DataTable dtADVANCE = new DataTable();
        public static DataTable dtAGENT = new DataTable();
        public static StringBuilder builder = new StringBuilder();
        public static DataTable dtBATCH = new DataTable();
        public static DataTable dtCASHCUST = new DataTable();
        public static DataTable dtFORMMAST = new DataTable();
        public static DataTable dtGROUP = new DataTable();
        public static DataTable dtLEDGER = new DataTable();
        public static DataTable dtLEDMAST = new DataTable();
        public static DataTable dtORDER = new DataTable();
        public static DataTable dtORDER2 = new DataTable();
        public static DataTable dtPGROUP = new DataTable();
        public static DataTable dtPRODUCT = new DataTable();
        public static DataTable dtSALE_ADJ = new DataTable();
        public static DataTable dtSP = new DataTable();
        public static DataTable dtSTAX = new DataTable();
        public static DataTable dtSTOCK = new DataTable();
        public static DataTable dtSERIAL = new DataTable();
        public static DataTable dtITVAT = new DataTable();
        public static DataTable dtModifiedRecord = new DataTable();
        public static DataTable dtUnModifiedRecord = new DataTable();
        public static DataTable dtNewlyInsertInLocal = new DataTable();
        public static DataTable dtDeleteFromLocal = new DataTable();

        public static Boolean ExitThread = false;
        //ArrayList Queries = new ArrayList();

        public frmMDI()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            try
            {
                this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\MOBILE.ico");
            }
            catch (Exception ex)
            {
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
            }


        }
        public frmMDI(bool flag)
        {
            InitializeComponent();
            try
            {
            }
            catch (Exception ex)
            {
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
            }
        }


        public List<String> GetImagesPath()
        {
            //  if (obj.IsSet)
            {
                List<String> imagesList = new List<String>();
                try
                {
                    DirectoryInfo Folder;
                    FileInfo[] Images;
                    string PictureLocation = Application.StartupPath + " \\BackImages";
                    Folder = new DirectoryInfo(PictureLocation);
                    Images = Folder.GetFiles();
                    for (int i = 0; i < Images.Length; i++)
                    {
                        imagesList.Add(String.Format(@"{0}/{1}", PictureLocation, Images[i].Name));
                        // Console.WriteLine(String.Format(@"{0}/{1}", folderName, Images[i].Name));
                    }

                    return imagesList;
                }
                catch
                {
                    MessageBox.Show("There is no folder and image files for Background Images in you directory, Please create Folder named 'BackGroundImages' and put image files in it which you want to set as a backgrond of this software", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return imagesList;
                }
            }
        }

        private void AddChildMenuItems(ToolStripItem parent)
        {
            // Cast the Parent to a ToolStripMenuItem
            ToolStripMenuItem ParentItem = (ToolStripMenuItem)parent;
            // Get the parents ID
            DataTable dt = new DataTable();

            dt = Operation.GetDataTable("select MenuId,MenuName from Menu where Menu.DisplayName='" + ParentItem.Text + "' and IsDeployed=1 order by Menu.SrNo ", Operation.Conn);
            DataTable dtChild = new DataTable();
            if (dt.Rows.Count > 0)
            {
                ParentItem.Tag = dt.Rows[0]["MenuName"].ToString();
                if (Operation.IsSuperAdmin)
                {
                    dtChild = Operation.GetDataTable("Select * from Menu where ParentMenuId=" + dt.Rows[0][0].ToString() + " and IsDeployed=1 and menu.MenuId not in (37) order By SrNo", Operation.Conn);
                }
                else
                    dtChild = Operation.GetDataTable("Select DisplayName,ImageFile from Menu left join UserRights on UserRights.MenuId=Menu.MenuId where Menu.ParentMenuId=" + dt.Rows[0][0].ToString() + " and IsDeployed=1 and IsChecked=1 and rights=6 and userid=" + Operation.Clientid + "  and menu.MenuId not in (26,23) order by SrNo", Operation.Conn);
            }
            // Get a list of the parents children


            if (dtChild.Rows.Count > 0)
            //if there are any children
            {
                for (int i = 0; i < dtChild.Rows.Count; i++)
                {
                    if (dtChild.Rows[i]["DisplayName"].ToString() == "-")
                    {
                        // add a seperator
                        ParentItem.DropDownItems.Add(dtChild.Rows[i]["DisplayName"].ToString());
                    }
                    else
                    {
                        Image img;
                        try
                        {
                            img = Image.FromFile(Application.StartupPath + "\\Design\\" + dtChild.Rows[i]["ImageFile"].ToString());
                        }
                        catch
                        {
                            img = null;
                        }
                        //if (dtChild.Rows[i]["DisplayName"].ToString() == "Employee Master" || dtChild.Rows[i]["DisplayName"].ToString() == "User Master")
                        //// add child and check if it has any children{
                        //{
                        //   if(Operation.IsAdmin)
                        //   {
                        AddChildMenuItems(ParentItem.DropDownItems.Add(dtChild.Rows[i]["DisplayName"].ToString(),
                                          img, new EventHandler(MenuItem_Click)));
                        // }

                        //}
                    }
                }
            }
        }

        private void MenuItem_Click(object Sender, EventArgs e)
        {
            if (((ToolStripMenuItem)Sender).Text.ToString() == "Exit")
            {
                this.Close();
            }

            if ((((ToolStripMenuItem)Sender).Text == "Log out"))
            {

                if (MessageBox.Show("Are you sure to Log Out?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Operation.ExecuteNonQuery("Update UserLogIn set IsLoggedIn='False' where UserId=" + Operation.Clientid, Operation.Conn);
                    Operation.Clientid = "0";
                    Operation.ClientUserName = "";
                    Operation.IsSuperAdmin = false;
                    //Program.AppSettings.UserId = "";
                    //Program.AppSettings.Save();
                    this.Hide();
                    frmUserLogin LogIn = new frmUserLogin();

                    LogIn.Show();


                }
                //executeQuery("update UserLogin set IsLoggedIn = 'False' where UserId = " & UserId, con)
            }
            if ((((ToolStripMenuItem)Sender).Text == "Statement Entry"))
            {

                stmt_flag = 1;
            }
            if ((((ToolStripMenuItem)Sender).Text == "Payment Entry"))
            {

                stmt_flag = 0;
            }

            if ((((ToolStripMenuItem)Sender).Text == "Inward Register"))
            {
                inward_flag = 1;
            }
            if ((((ToolStripMenuItem)Sender).Text == "Outward Register"))
            {
                inward_flag = 0;
            }

            if (((ToolStripMenuItem)Sender).Tag.ToString() != "")
            {
            }
            else if ((((ToolStripMenuItem)Sender).Text == "Backup"))
            {
                SaveFileDialog sfdbackup = new SaveFileDialog();
                sfdbackup.InitialDirectory = "C:";
                sfdbackup.Title = "Save Backup";
                sfdbackup.Filter = "Backup File(*.bak)|*.bak";
                if (sfdbackup.ShowDialog() == DialogResult.OK)
                {
                    if (Operation.ExecuteNonQuery("backup database [" + Operation.Conn.Database + "] to disk = '" + sfdbackup.FileName + "'", Operation.Conn) == -1)
                        MessageBox.Show("Backup process successfully completed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Error taking backup, Please try later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if ((((ToolStripMenuItem)Sender).Text == "Restore"))
            {
                OpenFileDialog ofdrestore = new OpenFileDialog();
                ofdrestore.InitialDirectory = "C:";
                ofdrestore.Title = "Restore";
                ofdrestore.DefaultExt = "bak";
                ofdrestore.Filter = "SQL Backup Files (*.bak)|*.bak";
                ofdrestore.FilterIndex = 1;
                if (ofdrestore.ShowDialog() == DialogResult.OK)
                {
                    if (Operation.ExecuteNonQuery("use master " + Environment.NewLine + " restore database [" + Operation.Conn.Database + "] from disk = '" + ofdrestore.FileName + "' with FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 10", Operation.Conn) == -1)
                        MessageBox.Show("Database Restored Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        //Operation.ExecuteNonQuery("alter database [" + Operation.CompanyCon.Database + "] set online", Operation.Conn);
                        MessageBox.Show("Error Restoring database, Please try later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }



        private void Compress(string filename)
        {
            // Open the file as a FileStream object.
            FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            int count = 0;

            byte[] buffer = new byte[infile.Length];
            // Read the file to ensure it is readable.
            count = infile.Read(buffer, 0, buffer.Length);

            if (count != buffer.Length)
            {
                infile.Close();
                throw new Exception("Compression failed. Number of bytes read not equal to buffer size");
            }

            infile.Close();

            // Create the memory stream to accept the compressed data
            MemoryStream ms = new MemoryStream();
            // Use the newly created memory stream for the compressed data.
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);

            compressedzipStream.Write(buffer, 0, buffer.Length);

            // Close the Zip stream.

            compressedzipStream.Close();
            File.Delete(filename.Split('.')[0] + ".zip");

            FileStream outfile = new FileStream(filename.Split('.')[0] + ".zip", FileMode.Create, FileAccess.Write, FileShare.Write);

            byte[] outbuffer = new byte[ms.Length];
            // Position to the start of the memory stream.
            ms.Position = 0;

            count = ms.Read(outbuffer, 0, Convert.ToInt32(ms.Length));

            if (count != outbuffer.Length)
            {
                throw new Exception("Error reading memory stream. Count(" + count.ToString() + " not equal to buffer size(" + outbuffer.Length.ToString() + ")");
            }

            outfile.Write(outbuffer, 0, outbuffer.Length);

            outfile.Close();

        }

        private void FormSetting(Form ChildForm)
        {
            ChildForm.MdiParent = this;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMDI));
            ChildForm.Icon = this.Icon;
            ChildForm.Show();
        }

        private void frmMDI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
            //if (e.KeyCode == Keys.OemQuotes)
            //{
            //    e.SuppressKeyPress = true;
            //}

            if ((e.KeyCode == Keys.Escape) | (e.Alt == true & e.KeyCode == Keys.X))
            {
                if (Application.OpenForms.Count <= 3)
                    this.Close();
            }
        }
        public bool IsFormOpen(Type formType)
        {
            foreach (Form form in Application.OpenForms)
                if (form.GetType().Name == form.Name)
                    return true;
            return false;
        }



        private void frmMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Minimized;
                e.Cancel = true;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(100);
                // formhide = true;
                this.Hide();
                return;
                //MessageBox.Show("normal");
            }
            if (this.WindowState == FormWindowState.Minimized)
            {
                Application.Exit();
            }
        }

        private void frmMDI_Load(object sender, EventArgs e)
        {
            try
            {
                Application.DoEvents();
                string isSuperadmin = "";
                if (Operation.IsSuperAdmin == true)
                {
                    isSuperadmin = "(Super_Admin)";
                }
                ControlMessage.Text = Operation.ClientUserName + isSuperadmin;
                this.WindowState = FormWindowState.Minimized;
                
            }
            catch (Exception ex)
            {
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
        private void btnClient_Click(object sender, EventArgs e)
        {
        }

        private void btnUserCreate_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void btnMenuMaster_Click(object sender, EventArgs e)
        {
        }

        private void companyMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmCompanyMaster frm = new frmCompanyMaster();
            // frm.MdiParent = this;

            frm.ShowDialog();

        }

        private void userCreationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmUserMaster obj = new frmUserMaster();
            // obj.MdiParent = this;

            obj.ShowDialog();
        }

        private void userwiseCompanyAllocationMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmCompanyAllocation frm = new frmCompanyAllocation();
            frm.MdiParent = this;
            frm.Show();
        }

        private void userCompanyMenuAllocationMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmUserMenuMaster frm = new frmUserMenuMaster();
            frm.MdiParent = this;
            frm.Show();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmChagePassword frm = new frmChagePassword();
            frm.MdiParent = this;
            frm.Show();
        }

        private void dashboardMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmUserMenuSubMenuMaster frm = new frmUserMenuSubMenuMaster();
            frm.MdiParent = this;
            frm.Show();
        }

        private void userDashboardMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmUserDashboardMaster frm = new frmUserDashboardMaster();
            frm.MdiParent = this;
            frm.Show();
        }

        private void settingsForDataUploadingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmSettings frm = new frmSettings();
            frm.MdiParent = this;
            frm.Show();
        }

        private void SyncProcess()
        {
            Operation.GetIniValue();

            string[] TimeArray = Operation.TimeInterval.Split(',');
            //MessageBox.Show(DateTime.Now.ToString("hh:mm tt"));
            int pos = Array.IndexOf(TimeArray, DateTime.Now.ToString("hh:mm tt"));
            //string[] newtime = new string[TimeArray.Count()];
            //for (int i = 0; i < TimeArray.Count(); i++)
            string[] newtime = new string[TimeArray.Length];
            for (int i = 0; i < TimeArray.Length; i++)
            {
                newtime[i] = (Convert.ToDateTime(TimeArray[i]).AddMinutes(-Convert.ToInt32(Operation.PromptMins))).ToString("hh:mm tt");
            }
            int posPromp = Array.IndexOf(newtime, DateTime.Now.ToString("hh:mm tt"));

            if (Operation.PromptBeforeData == true)
            {
                if (posPromp > -1)
                {
                    AutoClosingMessageBox.Show("Gentle Reminder......" + Environment.NewLine + Environment.NewLine + "Your Expert Data will be uploaded on " + (DateTime.Now.AddMinutes(Operation.PromptMins)).ToString("hh:mm tt") + "." + Environment.NewLine + "Please Close All Expert,If You Are Using in More Than One Computer.", "Upload Information", 10000);
                    //MessageBox.Show("Gentle Reminder......"+Environment.NewLine+Environment.NewLine+"Your Expert Data will be uploaded on " + (DateTime.Now.AddMinutes(Operation.PromptMins)).ToShortTimeString() +"." + Environment.NewLine + "Please Close All Expert,If You Are Using in More Than One Computer.", "Upload Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            if (pos > -1)
            {
                //Do What to do
                //Application.DoEvents();
                DataGathering();
            }
            ExitThread = true;
        }

        private void tGetExpertData_Tick(object sender, EventArgs e)
        {
            try
            {
                tGetExpertData.Enabled = false;
                Application.DoEvents();
                //Thread MyThread = new Thread(new ThreadStart(SyncProcess));
                //MyThread.Start();
                //while (ExitThread == false)
                //{
                //}
                //DataGathering();
                SyncProcess();
                tGetExpertData.Enabled = true;
                ExitThread = false;

            }
            catch (Exception ex)
            {
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
                tGetExpertData.Enabled = true;
            }
        }
        private void DataGathering2()
        {
            try
            {
                tGetExpertData.Enabled = false;
                if (Operation.IsInternetExits == false)
                {
                    Operation.writeLog("Internet Connection Doesn't Exist : " + DateTime.Now.ToLongTimeString() + Environment.NewLine, Operation.ErrorLog);

                    AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                    tGetExpertData.Enabled = true;
                    return;
                }

                Operation.writeLog("Upload Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                DataTable dtComp = Operation.GetDataTable("Select * From ClientCompanyMaster Where ClientId = " + Operation.Clientid, Operation.Conn);
                if (dtComp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtComp.Rows.Count; i++)
                    {
                        Application.DoEvents();
                        ClientCompanyId = Convert.ToInt32(dtComp.Rows[i]["ClientCompanyId"]);
                        string UploadingExpertPath = dtComp.Rows[i]["ExpertPath"].ToString();
                        string UploadingExpertDir = dtComp.Rows[i]["CompanyCode"].ToString().Trim().PadLeft(3, '0');
                        OleDbConnection expconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + UploadingExpertPath + "\\" + UploadingExpertDir + ";Mode=ReadWrite;Extended Properties=dBASE IV;Persist Security Info=False");
                        Queries.Clear();
                        builder = new StringBuilder();
                        Operation.writeLog("Upload From :- " + UploadingExpertPath + "\\" + UploadingExpertDir + Environment.NewLine + "Start Time : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        Operation.writeLog("Uploading ACT.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        dtACT = new DataTable();
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,ACT.* From ACT", "ACT") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtADVANCE = new DataTable();
                        Operation.writeLog("Uploading ADVANCE.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,ADVANCE.* From ADVANCE", "Advance") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtAGENT = new DataTable();
                        Operation.writeLog("Uploading AGENT.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,AGENT.* From AGENT", "Agent") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtBATCH = new DataTable();
                        Operation.writeLog("Uploading BATCH.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,BATCH.* From BATCH", "Batch") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtCASHCUST = new DataTable();
                        Operation.writeLog("Uploading CASHCUST.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,CASHCUST.* From CASHCUST", "CashCust") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtFORMMAST = new DataTable();
                        Operation.writeLog("Uploading FORMMAST.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,FORMMAST.* From FORMMAST", "FormMast") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtGROUP = new DataTable();
                        Operation.writeLog("Uploading GROUP.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,GROUP.* From [GROUP]", "[Group]") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtLEDGER = new DataTable();
                        Operation.writeLog("Uploading LEDGER.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,LEDGER.* From LEDGER", "Ledger") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtLEDMAST = new DataTable();
                        Operation.writeLog("Uploading LEDMAST.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,LEDMAST.* From LEDMAST", "LedMast") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtORDER = new DataTable();
                        Operation.writeLog("Uploading ORDER.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,ORDER.* From [ORDER]", "[Order]") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtORDER2 = new DataTable();
                        Operation.writeLog("Uploading ORDER2.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,ORDER2.* From ORDER2", "Order2") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtPGROUP = new DataTable();
                        Operation.writeLog("Uploading PGROUP.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,PGROUP.* From PGROUP", "PGroup") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtPRODUCT = new DataTable();
                        Operation.writeLog("Uploading PRODUCT.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,PRODUCT.* From PRODUCT", "Product") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtSALE_ADJ = new DataTable();
                        Operation.writeLog("Uploading SALE_ADJ.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,SALE_ADJ.* From [SALE_ADJ]", "Sale_Adj") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtSP = new DataTable();
                        Operation.writeLog("Uploading SP.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,SP.* From SP", "SP") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtSTAX = new DataTable();
                        Operation.writeLog("Uploading STAX.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,STAX.* From STAX", "Stax") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        dtSTOCK = new DataTable();
                        Operation.writeLog("Uploading STOCK.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,STOCK.* From STOCK", "Stock") == false)
                        {
                            tGetExpertData.Enabled = true;
                            return;
                        }
                        if (File.Exists(UploadingExpertPath + "\\" + UploadingExpertDir + "\\" + "Serial.dbf"))
                        {
                            dtSERIAL = new DataTable();
                            Operation.writeLog("Uploading SERIAL.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                            if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,SERIAL.* From SERIAL", "Serial") == false)
                            {
                                tGetExpertData.Enabled = true;
                                return;
                            }
                        }
                        //Update ClientCompanyMaster Set DataUploadDateTime=STR_TO_DATE('30/03/2016 8:51:30 PM','%d/%m/%Y %h:%i:%s %p') Where ClientCompanyId=14
                        Queries.Add("Update ClientCompanyMaster Set DataUploadDateTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss") + "' Where ClientCompanyId=" + ClientCompanyId + ";");
                        //MessageBox.Show(builder.ToString());
                        Operation.writeLog("Uploading.....Ended and Executing Query Start Time : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        string myQuery = "";
                        for (int j = 0; j < Queries.Count; j++)
                        {
                            myQuery += Queries[j] + ";";
                        }

                        SqlConnection myConn = new SqlConnection(Operation.ConnStr);
                        try
                        {
                            SqlCommand cmd = new SqlCommand("ExecuteMyQuery", myConn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter param = new SqlParameter();
                            param.ParameterName = "@ExecQuery";
                            //param.SqlDbType = SqlDbType.Structured;
                            param.Value = myQuery;
                            cmd.Parameters.Add(param);


                            if (myConn.State == ConnectionState.Closed)
                                myConn.Open();
                            cmd.ExecuteNonQuery();
                            myConn.Close();
                            Operation.writeLog((i + 1).ToString() + " of " + dtComp.Rows.Count.ToString() + " Successfully Execuated.", Operation.LogFile);
                            Operation.writeLog("Execution Completed at : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                        }
                        catch (Exception ex)
                        {
                            myConn.Close();
                            Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
                            //Operation.writeLog("Error while Executing Whole Query.....", Operation.LogFile);
                        }
                        finally
                        {
                            myConn.Close();
                        }
                    }//End of For Loop
                    Operation.writeLog("======================================================================" + Environment.NewLine, Operation.LogFile);
                    if (Operation.PromptAfterData == true)
                    {
                        AutoClosingMessageBox.Show("Gentle Notification......" + Environment.NewLine + Environment.NewLine + "Your Expert Data has been uploaded successfully....." + Environment.NewLine + "You may do your work with EXPERT.....", "After Upload Information", 10000);
                    }
                    tGetExpertData.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
            }

        }
        private void DataGathering()
        {
            try
            {
                tGetExpertData.Enabled = false;
                if (Operation.IsInternetExits == false)
                {
                    Operation.writeLog("Internet Connection Doesn't Exist : " + DateTime.Now.ToLongTimeString() + Environment.NewLine, Operation.ErrorLog);

                    AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                    tGetExpertData.Enabled = true;
                    return;
                }

                Operation.writeLog("Upload Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                NewDataUpload();
                Operation.writeLog("Upload End Time : " + DateTime.Now.ToLongTimeString() + Environment.NewLine + "--------------------=====", Operation.LogFile);
                #region commented
                //DataTable dtComp = Operation.GetDataTable("Select * From ClientCompanyMaster Where ClientId = " + Operation.Clientid, Operation.Conn);
                //if (dtComp.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtComp.Rows.Count; i++)
                //    {
                //        Application.DoEvents();
                //        ClientCompanyId = Convert.ToInt32(dtComp.Rows[i]["ClientCompanyId"]);
                //        string UploadingExpertPath = dtComp.Rows[i]["ExpertPath"].ToString();
                //        string UploadingExpertDir = dtComp.Rows[i]["CompanyCode"].ToString().Trim().PadLeft(3, '0');
                //        OleDbConnection expconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + UploadingExpertPath + "\\" + UploadingExpertDir + ";Mode=ReadWrite;Extended Properties=dBASE IV;Persist Security Info=False");
                //        Queries.Clear();
                //        builder = new StringBuilder();
                //        Operation.writeLog("Upload From :- " + UploadingExpertPath + "\\" + UploadingExpertDir + Environment.NewLine + "Start Time : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        Operation.writeLog("Uploading ACT.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        dtACT = new DataTable();
                //        //if(CopyTable(expconn,Operation.Conn,"Select " + ClientCompanyId + " as ClientCompanyId,ACT.* From ACT","ACT"))
                //        //{
                //        //}
                //        //if (CopyTable(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,LEDGER.* From LEDGER", "Ledger") == false)
                //        //{
                //        //}
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,ACT.* From ACT", "ACT", dtACT) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtADVANCE = new DataTable();
                //        Operation.writeLog("Uploading ADVANCE.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,ADVANCE.* From ADVANCE", "Advance", dtADVANCE) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtAGENT = new DataTable();
                //        Operation.writeLog("Uploading AGENT.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,AGENT.* From AGENT", "Agent", dtAGENT) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtBATCH = new DataTable();
                //        Operation.writeLog("Uploading BATCH.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,BATCH.* From BATCH", "Batch", dtBATCH) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtCASHCUST = new DataTable();
                //        Operation.writeLog("Uploading CASHCUST.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,CASHCUST.* From CASHCUST", "CashCust", dtCASHCUST) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtFORMMAST = new DataTable();
                //        Operation.writeLog("Uploading FORMMAST.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,FORMMAST.* From FORMMAST", "FormMast", dtFORMMAST) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtGROUP = new DataTable();
                //        Operation.writeLog("Uploading GROUP.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,GROUP.* From [GROUP]", "Group", dtGROUP) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtLEDGER = new DataTable();
                //        Operation.writeLog("Uploading LEDGER.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,LEDGER.* From LEDGER", "Ledger", dtLEDGER) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtLEDMAST = new DataTable();
                //        Operation.writeLog("Uploading LEDMAST.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,LEDMAST.* From LEDMAST", "LedMast", dtLEDMAST) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtORDER = new DataTable();
                //        Operation.writeLog("Uploading ORDER.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,ORDER.* From [ORDER]", "Order", dtORDER) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtORDER2 = new DataTable();
                //        Operation.writeLog("Uploading ORDER2.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,ORDER2.* From ORDER2", "Order2", dtORDER2) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtPGROUP = new DataTable();
                //        Operation.writeLog("Uploading PGROUP.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,PGROUP.* From PGROUP", "PGroup", dtPGROUP) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtPRODUCT = new DataTable();
                //        Operation.writeLog("Uploading PRODUCT.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,PRODUCT.* From PRODUCT", "Product", dtPRODUCT) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtSALE_ADJ = new DataTable();
                //        Operation.writeLog("Uploading SALE_ADJ.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,SALE_ADJ.* From [SALE_ADJ]", "Sale_Adj", dtSALE_ADJ) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtSP = new DataTable();
                //        Operation.writeLog("Uploading SP.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,SP.* From SP", "SP", dtSP) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtSTAX = new DataTable();
                //        Operation.writeLog("Uploading STAX.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,STAX.* From STAX", "Stax", dtSTAX) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        dtSTOCK = new DataTable();
                //        Operation.writeLog("Uploading STOCK.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,STOCK.* From STOCK", "Stock", dtSTOCK) == false)
                //        {
                //            tGetExpertData.Enabled = true;
                //            return;
                //        }
                //        if (File.Exists(UploadingExpertPath + "\\" + UploadingExpertDir + "\\" + "Serial.dbf"))
                //        {
                //            dtSERIAL = new DataTable();
                //            Operation.writeLog("Uploading SERIAL.....Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //            if (CopyTableNew(expconn, Operation.Conn, "Select " + ClientCompanyId + " as ClientCompanyId,SERIAL.* From SERIAL", "Serial", dtSERIAL) == false)
                //            {
                //                tGetExpertData.Enabled = true;
                //                return;
                //            }
                //        }
                //        //Update ClientCompanyMaster Set DataUploadDateTime=STR_TO_DATE('30/03/2016 8:51:30 PM','%d/%m/%Y %h:%i:%s %p') Where ClientCompanyId=14
                //        //Queries.Add("Update ClientCompanyMaster Set DataUploadDateTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss")+ "' Where ClientCompanyId=" + ClientCompanyId + ";");
                //        builder.Append("Update ClientCompanyMaster Set DataUploadDateTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss") + "' Where ClientCompanyId=" + ClientCompanyId + ";");
                //        //MessageBox.Show(builder.ToString());
                //        Operation.writeLog("Uploading.....Ended and Executing Query Start Time : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        //Operation.writeLog("Act Start Time : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString()+Environment.NewLine, Operation.LogFile);
                //        //if (BulkInsertDataTable("Stock", dtSTOCK))
                //        //{
                //        //    //MessageBox.Show("Successfully Inserted ACT");
                //        //}
                //        //if (BulkInsertDataTable("Serial", dtSERIAL))
                //        //{
                //        //    //MessageBox.Show("Successfully Inserted ACT");
                //        //}
                //        //Operation.writeLog("Serial End Time : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + Environment.NewLine, Operation.LogFile);
                //        StringBuilder builder1 = new StringBuilder();
                //        for (int j = 0; j < Queries.Count; j++)
                //        {
                //            builder1.Append(Queries[j].ToString() + ";" + Environment.NewLine);
                //        }
                //        SqlConnection myConn = new SqlConnection(Operation.ConnStr);
                //        try
                //        {
                //            SqlCommand cmd = new SqlCommand("InsertRecord", myConn);
                //            //SqlCommand cmd = new SqlCommand("InsertFinalRecord", myConn);
                //            cmd.CommandType = CommandType.StoredProcedure;
                //            SqlParameter param = new SqlParameter();
                //            param.ParameterName = "@ExecQuery";
                //            param.Value = builder.ToString();
                //            cmd.Parameters.Add(param);

                //            SqlParameter param1 = new SqlParameter();
                //            param1.ParameterName = "@dtAct";
                //            param1.Value = dtACT;
                //            cmd.Parameters.Add(param1);

                //            SqlParameter param2 = new SqlParameter();
                //            param2.ParameterName = "@dtAdvance";
                //            param2.Value = dtADVANCE;
                //            cmd.Parameters.Add(param2);

                //            SqlParameter param3 = new SqlParameter();
                //            param3.ParameterName = "@dtAgent";
                //            param3.Value = dtAGENT;
                //            cmd.Parameters.Add(param3);

                //            SqlParameter param4 = new SqlParameter();
                //            param4.ParameterName = "@dtBatch";
                //            param4.Value = dtBATCH;
                //            cmd.Parameters.Add(param4);

                //            SqlParameter param5 = new SqlParameter();
                //            param5.ParameterName = "@dtCashCust";
                //            param5.Value = dtCASHCUST;
                //            cmd.Parameters.Add(param5);

                //            SqlParameter param6 = new SqlParameter();
                //            param6.ParameterName = "@dtFormMast";
                //            param6.Value = dtFORMMAST;
                //            cmd.Parameters.Add(param6);

                //            SqlParameter param7 = new SqlParameter();
                //            param7.ParameterName = "@dtGroup";
                //            param7.Value = dtGROUP;
                //            cmd.Parameters.Add(param7);

                //            SqlParameter param8 = new SqlParameter();
                //            param8.ParameterName = "@dtLedger";
                //            param8.Value = dtLEDGER;
                //            cmd.Parameters.Add(param8);

                //            SqlParameter param9 = new SqlParameter();
                //            param9.ParameterName = "@dtLedMast";
                //            param9.Value = dtLEDMAST;
                //            cmd.Parameters.Add(param9);

                //            SqlParameter param10 = new SqlParameter();
                //            param10.ParameterName = "@dtOrder";
                //            param10.Value = dtORDER;
                //            cmd.Parameters.Add(param10);

                //            SqlParameter param11 = new SqlParameter();
                //            param11.ParameterName = "@dtOrder2";
                //            param11.Value = dtORDER2;
                //            cmd.Parameters.Add(param11);

                //            SqlParameter param12 = new SqlParameter();
                //            param12.ParameterName = "@dtPGroup";
                //            param12.Value = dtPGROUP;
                //            cmd.Parameters.Add(param12);

                //            SqlParameter param13 = new SqlParameter();
                //            param13.ParameterName = "@dtProduct";
                //            param13.Value = dtPRODUCT;
                //            cmd.Parameters.Add(param13);

                //            SqlParameter param14 = new SqlParameter();
                //            param14.ParameterName = "@dtSale_adj";
                //            param14.Value = dtSALE_ADJ;
                //            cmd.Parameters.Add(param14);

                //            SqlParameter param15 = new SqlParameter();
                //            param15.ParameterName = "@dtSp";
                //            param15.Value = dtSP;
                //            cmd.Parameters.Add(param15);

                //            SqlParameter param16 = new SqlParameter();
                //            param16.ParameterName = "@dtStax";
                //            param16.Value = dtSTAX;
                //            cmd.Parameters.Add(param16);

                //            SqlParameter param17 = new SqlParameter();
                //            param17.ParameterName = "@dtStock";
                //            param17.Value = dtSTOCK;
                //            cmd.Parameters.Add(param17);

                //            //SqlParameter param18 = new SqlParameter();
                //            //param18.ParameterName = "@dtStock";

                //            //if (File.Exists(UploadingExpertPath + "\\" + UploadingExpertDir + "\\" + "Serial.dbf"))
                //            //{
                //            //    param18.Value = dtSERIAL;
                //            //}
                //            //else
                //            //{
                //            //    param18.Value = null;
                //            //}
                //            //cmd.Parameters.Add(param18);

                //            cmd.CommandTimeout = 0;
                //            if (myConn.State == ConnectionState.Closed)
                //                myConn.Open();
                //            cmd.ExecuteNonQuery();
                //            myConn.Close();
                //            Operation.writeLog((i + 1).ToString() + " of " + dtComp.Rows.Count.ToString() + " Successfully Execuated.", Operation.LogFile);
                //            Operation.writeLog("Execution Completed at : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                //        }
                //        catch (Exception ex)
                //        {
                //            myConn.Close();
                //            Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
                //            //Operation.writeLog("Error while Executing Whole Query.....", Operation.LogFile);
                //        }
                //        finally
                //        {
                //            myConn.Close();
                //        }
                //    }//End of For Loop
                //    Operation.writeLog("======================================================================" + Environment.NewLine, Operation.LogFile);
                //    if (Operation.PromptAfterData == true)
                //    {
                //        AutoClosingMessageBox.Show("Gentle Notification......" + Environment.NewLine + Environment.NewLine + "Your Expert Data has been uploaded successfully....." + Environment.NewLine + "You may do your work with EXPERT.....", "After Upload Information", 10000);
                //    }
                //    tGetExpertData.Enabled = true;
                //}
                #endregion
                if (Operation.PromptAfterData == true)
                {
                    AutoClosingMessageBox.Show("Gentle Notification......" + Environment.NewLine + Environment.NewLine + "Your Expert Data has been uploaded successfully....." + Environment.NewLine + "You may do your work with EXPERT.....", "After Upload Information", 10000);
                }
            }
            catch (Exception ex)
            {
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
            }
            finally
            {
                tGetExpertData.Enabled = true;
                this.Invoke(new MethodInvoker(delegate
                {
                    toolUploadStatus.Visible = false;
                    toolUploadProgress.Visible = false;
                }));
            }
        }

        private void NewDataUpload()
        {
            DataTable dtComp = Operation.GetDataTable("Select * From ClientCompanyMaster Where ClientId = " + Operation.Clientid, Operation.Conn);
            foreach (DataRow dr in dtComp.Rows)
            {
                //bool isFreshInsert = false;
                ClientCompanyId = Convert.ToInt32(dr["ClientCompanyId"]);
                List<string> strQueries = new List<string>();
                string UploadingExpertPath = dr["ExpertPath"].ToString();
                string UploadingExpertDir = Operation.RemoveSpecialCharacters(UploadingExpertPath) + "\\" + dr["CompanyCode"].ToString().Trim().PadLeft(3, '0');

                OleDbConnection localconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\" + UploadingExpertDir + ";Mode=ReadWrite;Extended Properties=dBASE IV;Persist Security Info=False");
                OleDbConnection expconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\" + UploadingExpertDir + "\\TMP;Mode=ReadWrite;Extended Properties=dBASE IV;Persist Security Info=False");
                DataTable dtLocalAct, dtExpertAct;
                Application.DoEvents();
                List<string> tableNames = new List<string>();
                foreach (TableNames r in Enum.GetValues(typeof(TableNames)))
                {
                    tableNames.Add(r.ToString());
                }
                var startTime = DateTime.Now;
                Operation.GetIniValue();
                foreach (string tName in tableNames)
                {
                    Operation.writeLog("====================================================================" + Environment.NewLine + tName + " Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.LogFile);

                    if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\" + tName + ".DBF") && !Operation.ForceSync)
                    {
                        Operation.writeLog("File : " + tName + " execution Start at: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.LogFile);
                        CheckExpertTmpFile(dr, tName + ".DBF", UploadingExpertDir);
                        dtLocalAct = GetDataFromExpert(localconn, "select * from [" + tName + "]");
                        dtExpertAct = GetDataFromExpert(expconn, "select * from [" + tName + "]");
                        this.Invoke(new MethodInvoker(delegate
                        {
                            toolUploadStatus.Visible = true;
                            toolUploadProgress.Visible = true;
                            toolUploadProgress.Value = 0;
                            toolUploadStatus.Text = "Processing Existing " + tName + ".dbf : ";
                        }));
                        CompareTwoDataTable(dtLocalAct, dtExpertAct, (TableNames)Enum.Parse(typeof(TableNames), tName));
                        DataTable localTable = new DataTable();
                        List<string> tableColumns = GetTableColumns((TableNames)Enum.Parse(typeof(TableNames), tName), false);
                        this.Invoke(new MethodInvoker(delegate
                        {
                            toolUploadProgress.Maximum = dtModifiedRecord.Rows.Count + dtDeleteFromLocal.Rows.Count + dtNewlyInsertInLocal.Rows.Count + dtUnModifiedRecord.Rows.Count;
                            toolUploadProgress.Minimum = 0;
                            toolUploadProgress.Value = 0;
                        }));
                        for (var i = 0; i < dtDeleteFromLocal.Rows.Count; i++)
                        {
                            strQueries.Add(InsertUpdateQueries.GetQueries((TableNames)Enum.Parse(typeof(TableNames), tName), OperationTypes.DELETE, null, dtDeleteFromLocal.Rows[i], ClientCompanyId, tableColumns));
                            //if (tName.ToString() == "ACT" || tName.ToString().ToString() == "PRODUCT" || tName.ToString() == "BATCH" || tName.ToString() == "GROUP" || tName.ToString() == "PGROUP" || tName.ToString() == "AGENT" || tName.ToString() == "CASHCUST" || tName.ToString() == "FORMMAST" || tName.ToString() == "STAX")
                            //{
                            //    if (dtUnModifiedRecord.Rows.Count > 0)
                            //        strQueries.Add(InsertUpdateQueries.GetQueries((TableNames)Enum.Parse(typeof(TableNames), tName), OperationTypes.DELETE, null, dtUnModifiedRecord.Rows[i], ClientCompanyId, tableColumns));
                            //}
                            //else
                            //{
                            //    if (dtNewlyInsertInLocal.Rows.Count > 0)
                            //        strQueries.Add(InsertUpdateQueries.GetQueries((TableNames)Enum.Parse(typeof(TableNames), tName), OperationTypes.DELETE, null, dtNewlyInsertInLocal.Rows[i], ClientCompanyId, tableColumns));
                            //}
                            //this.Invoke(new MethodInvoker(delegate
                            //{
                            //    toolUploadProgress.Value = toolUploadProgress.Value + 1;
                            //}));
                        }
                        foreach (DataRow row in dtModifiedRecord.Rows)
                        {
                            strQueries.Add(InsertUpdateQueries.GetQueries((TableNames)Enum.Parse(typeof(TableNames), tName), OperationTypes.DELETE, null, row, ClientCompanyId, tableColumns));
                            this.Invoke(new MethodInvoker(delegate
                            {
                                toolUploadProgress.Value = toolUploadProgress.Value + 1;
                            }));
                        }
                        foreach (DataRow row in dtUnModifiedRecord.Rows)
                        {
                            strQueries.Add(InsertUpdateQueries.GetQueries((TableNames)Enum.Parse(typeof(TableNames), tName), OperationTypes.INSERT, row, null, ClientCompanyId, tableColumns));
                            this.Invoke(new MethodInvoker(delegate
                            {
                                toolUploadProgress.Value = toolUploadProgress.Value + 1;
                            }));
                        }

                        foreach (DataRow row in dtNewlyInsertInLocal.Rows)
                        {
                            strQueries.Add(InsertUpdateQueries.GetQueries((TableNames)Enum.Parse(typeof(TableNames), tName), OperationTypes.INSERT, row, null, ClientCompanyId, tableColumns));
                            this.Invoke(new MethodInvoker(delegate
                            {
                                toolUploadProgress.Value = toolUploadProgress.Value + 1;
                            }));
                        }
                        //for (var i = 0; i < dtModifiedRecord.Rows.Count; i++)
                        //{
                        //    strQueries.Add(InsertUpdateQueries.GetQueries((TableNames)Enum.Parse(typeof(TableNames), tName), OperationTypes.UPDATE, dtUnModifiedRecord.Rows[i], dtModifiedRecord.Rows[i], ClientCompanyId, tableColumns));
                        //    this.Invoke(new MethodInvoker(delegate
                        //    {
                        //        toolUploadProgress.Value = toolUploadProgress.Value + 1;
                        //    }));
                        //}                        

                    }
                    else
                    {
                        if (File.Exists(UploadingExpertPath + "\\" + dr["CompanyCode"].ToString().Trim().PadLeft(3, '0') + "\\" + tName + ".DBF"))
                        {
                            List<string> tableColumns = GetTableColumns((TableNames)Enum.Parse(typeof(TableNames), tName), true);
                            this.Invoke(new MethodInvoker(delegate
                            {
                                toolUploadStatus.Visible = true;
                                toolUploadProgress.Visible = true;
                                toolUploadProgress.Value = 0;
                                toolUploadStatus.Text = "Processing " + tName + ".dbf : ";
                            }));
                            CheckExpertTmpFile(dr, tName + ".DBF", UploadingExpertDir);
                            var columnName = GetExperColumnNamesForInsert(tableColumns);
                            strQueries.Add(InsertUpdateQueries.GetQueries((TableNames)Enum.Parse(typeof(TableNames), tName), OperationTypes.DELETE, null, null, ClientCompanyId, tableColumns));
                            var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from [" + tName + "]");
                            //dtACT = dt;

                            this.Invoke(new MethodInvoker(delegate
                            {
                                //toolUploadStatus.Text = "Processing " + tName + ".dbf : ";
                                toolUploadProgress.Maximum = dt.Rows.Count;
                                toolUploadProgress.Minimum = 0;
                            }));
                            for (var i = 0; i < dt.Rows.Count; i++)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    toolUploadProgress.Value = i;
                                }));
                                strQueries.Add(InsertUpdateQueries.GetQueries((TableNames)Enum.Parse(typeof(TableNames), tName), OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, tableColumns));
                            }
                        }
                    }
                    var endTime = DateTime.Now - startTime;
                    Operation.writeLog("====================================================================" + Environment.NewLine + tName + " Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.LogFile);
                }
                strQueries.Add("Update ClientCompanyMaster Set DataUploadDateTime='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' Where ClientCompanyId=" + ClientCompanyId + ";");
                #region Commented
                //#region ADVANCE TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "ADVANCE Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\ADVANCE.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "ADVANCE.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from ADVANCE");
                //    dtExpertAct = GetDataFromExpert(expconn, "select * from ADVANCE");
                //    List<string> ADVANCEColumns = GetTableColumns(TableNames.ADVANCE, false);
                //    DataTable localTable = new DataTable();
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {

                //        DataView dtExpert = new DataView(dtExpertAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtLocalAct.Rows[i]["Link"]) + "'" : "Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Date"])) ? "[Date]='" + Convert.ToDateTime(dtLocalAct.Rows[i]["Date"]).ToString("dd/MM/yyyy") + "'" : "Date is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["DocNo"])) ? "DocNo='" + Convert.ToString(dtLocalAct.Rows[i]["DocNo"]) + "'" : "DocNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Amt"])) ? "Amt='" + Convert.ToString(dtLocalAct.Rows[i]["Amt"]) + "'" : "Amt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["DC"])) ? "DC='" + Convert.ToString(dtLocalAct.Rows[i]["DC"]) + "'" : "DC is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Division"])) ? "Division='" + Convert.ToString(dtLocalAct.Rows[i]["Division"]) + "'" : "Division is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < ADVANCEColumns.Count; j++)
                //            {
                //                if (localTable.Columns[ADVANCEColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(localTable.Rows[0][ADVANCEColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][ADVANCEColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ADVANCE, OperationTypes.UPDATE, localTable.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, ADVANCEColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ADVANCE, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, ADVANCEColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from ADVANCE");

                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        DataView dtExpert = new DataView(dtLocalAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtExpertAct.Rows[i]["Link"]) + "'" : "Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Date"])) ? "[Date]='" + Convert.ToDateTime(dtExpertAct.Rows[i]["Date"]).ToString("dd/MM/yyyy") + "'" : "Date is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["DocNo"])) ? "DocNo='" + Convert.ToString(dtExpertAct.Rows[i]["DocNo"]) + "'" : "DocNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Amt"])) ? "Amt='" + Convert.ToString(dtExpertAct.Rows[i]["Amt"]) + "'" : "Amt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["DC"])) ? "DC='" + Convert.ToString(dtExpertAct.Rows[i]["DC"]) + "'" : "DC is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Division"])) ? "Division='" + Convert.ToString(dtExpertAct.Rows[i]["Division"]) + "'" : "Division is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < ADVANCEColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[ADVANCEColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][ADVANCEColumns[j].Split('(')[0]]).Equals(Convert.ToString(localTable.Rows[0][ADVANCEColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(localTable.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ADVANCE, OperationTypes.INSERT, localTable.Rows[0], null, ClientCompanyId, ADVANCEColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ADVANCE, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, ADVANCEColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> ADVANCEColumns = GetTableColumns(TableNames.ADVANCE, true);
                //    CheckExpertTmpFile(dr, "ADVANCE.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(ADVANCEColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from ADVANCE");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ADVANCE, OperationTypes.DELETE, null, null, ClientCompanyId, ADVANCEColumns));
                //    //dtADVANCE = dt;
                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing Advance.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ADVANCE, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, ADVANCEColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "ADVANCE Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region AGENT TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "AGENT Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\AGENT.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "AGENT.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from AGENT");
                //    List<string> AGENTColumns = GetTableColumns(TableNames.AGENT, false);
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {
                //        dtExpertAct = GetDataFromExpert(expconn, "select * from AGENT where " +
                //            (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtExpertAct != null && dtExpertAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < AGENTColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[AGENTColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[0][AGENTColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][AGENTColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.AGENT, OperationTypes.UPDATE, dtExpertAct.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, AGENTColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.AGENT, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, AGENTColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from AGENT");
                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        dtLocalAct = GetDataFromExpert(localconn, "select * from AGENT where " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtLocalAct != null && dtLocalAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < AGENTColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[AGENTColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][AGENTColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[0][AGENTColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(dtLocalAct.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.AGENT, OperationTypes.INSERT, dtLocalAct.Rows[0], null, ClientCompanyId, AGENTColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.AGENT, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, AGENTColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> AGENTColumns = GetTableColumns(TableNames.AGENT, true);
                //    CheckExpertTmpFile(dr, "AGENT.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(AGENTColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from AGENT");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.AGENT, OperationTypes.DELETE, null, null, ClientCompanyId, AGENTColumns));
                //    //dtAGENT = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing Agent.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.AGENT, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, AGENTColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "AGENT Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region BATCH TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "BATCH Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\BATCH.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "BATCH.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from BATCH");
                //    List<string> BATCHColumns = GetTableColumns(TableNames.AGENT, false);
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {
                //        dtExpertAct = GetDataFromExpert(expconn, "select * from BATCH where " +
                //            (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtExpertAct != null && dtExpertAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < BATCHColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[BATCHColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[0][BATCHColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][BATCHColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.BATCH, OperationTypes.UPDATE, dtExpertAct.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, BATCHColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.BATCH, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, BATCHColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from BATCH");
                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        dtLocalAct = GetDataFromExpert(localconn, "select * from BATCH where " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtLocalAct != null && dtLocalAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < BATCHColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[BATCHColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][BATCHColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[0][BATCHColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(dtLocalAct.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.BATCH, OperationTypes.INSERT, dtLocalAct.Rows[0], null, ClientCompanyId, BATCHColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.BATCH, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, BATCHColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> BATCHColumns = GetTableColumns(TableNames.BATCH, true);
                //    CheckExpertTmpFile(dr, "BATCH.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(BATCHColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from BATCH");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.BATCH, OperationTypes.DELETE, null, null, ClientCompanyId, BATCHColumns));
                //    //dtBATCH = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing Batch.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.BATCH, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, BATCHColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "BATCH Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region CASHCUST TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "CASHCUST Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\CASHCUST.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "CASHCUST.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from CASHCUST");
                //    List<string> CASHCUSTColumns = GetTableColumns(TableNames.AGENT, false);
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {
                //        dtExpertAct = GetDataFromExpert(expconn, "select * from CASHCUST where " +
                //            (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtExpertAct != null && dtExpertAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < CASHCUSTColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[CASHCUSTColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[0][CASHCUSTColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][CASHCUSTColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.CASHCUST, OperationTypes.UPDATE, dtExpertAct.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, CASHCUSTColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.CASHCUST, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, CASHCUSTColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from CASHCUST");
                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        dtLocalAct = GetDataFromExpert(localconn, "select * from CASHCUST where " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtLocalAct != null && dtLocalAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < CASHCUSTColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[CASHCUSTColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][CASHCUSTColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[0][CASHCUSTColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(dtLocalAct.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.CASHCUST, OperationTypes.INSERT, dtLocalAct.Rows[0], null, ClientCompanyId, CASHCUSTColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.CASHCUST, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, CASHCUSTColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> CASHCUSTColumns = GetTableColumns(TableNames.CASHCUST, true);
                //    CheckExpertTmpFile(dr, "CASHCUST.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(CASHCUSTColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from CASHCUST");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.CASHCUST, OperationTypes.DELETE, null, null, ClientCompanyId, CASHCUSTColumns));
                //    //dtCASHCUST = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing CashCust.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.CASHCUST, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, CASHCUSTColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "CASHCUST Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region FORMMAST TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "FORMMAST Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\FORMMAST.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "FORMMAST.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from FORMMAST");
                //    List<string> FORMMASTColumns = GetTableColumns(TableNames.AGENT, false);
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {
                //        dtExpertAct = GetDataFromExpert(expconn, "select * from FORMMAST where " +
                //            (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtExpertAct != null && dtExpertAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < FORMMASTColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[FORMMASTColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[0][FORMMASTColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][FORMMASTColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.FORMMAST, OperationTypes.UPDATE, dtExpertAct.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, FORMMASTColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.FORMMAST, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, FORMMASTColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from FORMMAST");
                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        dtLocalAct = GetDataFromExpert(localconn, "select * from FORMMAST where " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtLocalAct != null && dtLocalAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < FORMMASTColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[FORMMASTColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][FORMMASTColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[0][FORMMASTColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(dtLocalAct.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.FORMMAST, OperationTypes.INSERT, dtLocalAct.Rows[0], null, ClientCompanyId, FORMMASTColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.FORMMAST, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, FORMMASTColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> FORMMASTColumns = GetTableColumns(TableNames.FORMMAST, true);
                //    CheckExpertTmpFile(dr, "FORMMAST.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(FORMMASTColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from FORMMAST");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.FORMMAST, OperationTypes.DELETE, null, null, ClientCompanyId, FORMMASTColumns));
                //    //dtFORMMAST = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing FormMast.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.FORMMAST, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, FORMMASTColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "FORMMAST Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region GROUP TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "GROUP Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\GROUP.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "GROUP.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from [GROUP]");
                //    List<string> GROUPColumns = GetTableColumns(TableNames.AGENT, false);
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {
                //        dtExpertAct = GetDataFromExpert(expconn, "select * from [GROUP] where " +
                //            (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtExpertAct != null && dtExpertAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < GROUPColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[GROUPColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[0][GROUPColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][GROUPColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.GROUP, OperationTypes.UPDATE, dtExpertAct.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, GROUPColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.GROUP, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, GROUPColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from [GROUP]");
                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        dtLocalAct = GetDataFromExpert(localconn, "select * from [GROUP] where " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtLocalAct != null && dtLocalAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < GROUPColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[GROUPColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][GROUPColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[0][GROUPColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(dtLocalAct.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.GROUP, OperationTypes.INSERT, dtLocalAct.Rows[0], null, ClientCompanyId, GROUPColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.GROUP, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, GROUPColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> GROUPColumns = GetTableColumns(TableNames.GROUP, true);
                //    CheckExpertTmpFile(dr, "GROUP.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(GROUPColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from [GROUP]");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.GROUP, OperationTypes.DELETE, null, null, ClientCompanyId, GROUPColumns));
                //    //dtGROUP = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing Group.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.GROUP, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, GROUPColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "GROUP Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region ITVAT TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "ITVAT Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\ITVAT.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "ITVAT.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from ITVAT");
                //    dtExpertAct = GetDataFromExpert(expconn, "select * from ITVAT");
                //    List<string> ITVATColumns = GetTableColumns(TableNames.ITVAT, false);
                //    DataTable localTable = new DataTable();
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {

                //        DataView dtExpert = new DataView(dtExpertAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["VatCode"])) ? "VatCode='" + Convert.ToString(dtLocalAct.Rows[i]["VatCode"]) + "'" : "VatCode is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtLocalAct.Rows[i]["Link"]) + "'" : "Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Amt"])) ? "Amt='" + Convert.ToString(dtLocalAct.Rows[i]["Amt"]) + "'" : "Amt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Vat"])) ? "Vat='" + Convert.ToString(dtLocalAct.Rows[i]["Vat"]) + "'" : "Vat is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < ITVATColumns.Count; j++)
                //            {
                //                if (localTable.Columns[ITVATColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(localTable.Rows[0][ITVATColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][ITVATColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ITVAT, OperationTypes.UPDATE, localTable.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, ITVATColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ITVAT, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, ITVATColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from ITVAT");

                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        DataView dtExpert = new DataView(dtLocalAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["VatCode"])) ? "VatCode='" + Convert.ToString(dtExpertAct.Rows[i]["VatCode"]) + "'" : "VatCode is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtExpertAct.Rows[i]["Link"]) + "'" : "Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Amt"])) ? "Amt='" + Convert.ToString(dtExpertAct.Rows[i]["Amt"]) + "'" : "Amt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Vat"])) ? "Vat='" + Convert.ToString(dtExpertAct.Rows[i]["Vat"]) + "'" : "Vat is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < ITVATColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[ITVATColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][ITVATColumns[j].Split('(')[0]]).Equals(Convert.ToString(localTable.Rows[0][ITVATColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(localTable.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ITVAT, OperationTypes.INSERT, localTable.Rows[0], null, ClientCompanyId, ITVATColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ITVAT, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, ITVATColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> ITVATColumns = GetTableColumns(TableNames.ITVAT, true);
                //    CheckExpertTmpFile(dr, "ITVAT.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(ITVATColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from ITVAT");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ITVAT, OperationTypes.DELETE, null, null, ClientCompanyId, ITVATColumns));
                //    //dtITVAT = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing ITVAT.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ITVAT, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, ITVATColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "ITVAT Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region LEDGER TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "LEDGER Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\LEDGER.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "LEDGER.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from LEDGER");
                //    dtExpertAct = GetDataFromExpert(expconn, "select * from LEDGER");
                //    List<string> LEDGERColumns = GetTableColumns(TableNames.LEDGER, false);
                //    DataTable localTable = new DataTable();
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {

                //        DataView dtExpert = new DataView(dtExpertAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtLocalAct.Rows[i]["Link"]) + "'" : "Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Date"])) ? "[Date]='" + Convert.ToDateTime(dtLocalAct.Rows[i]["Date"]).ToString("dd/MM/yyyy") + "'" : "Date is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["ChDt"])) ? "[ChDt]='" + Convert.ToDateTime(dtLocalAct.Rows[i]["ChDt"]).ToString("dd/MM/yyyy") + "'" : "ChDt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["DocNo"])) ? "DocNo='" + Convert.ToString(dtLocalAct.Rows[i]["DocNo"]) + "'" : "DocNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["ClrDt"])) ? "ClrDt='" + Convert.ToString(dtLocalAct.Rows[i]["ClrDt"]) + "'" : "ClrDt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["VatCode"])) ? "VatCode='" + Convert.ToString(dtLocalAct.Rows[i]["VatCode"]) + "'" : "VatCode is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["DC"])) ? "DC='" + Convert.ToString(dtLocalAct.Rows[i]["DC"]) + "'" : "DC is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["VType"])) ? "VType='" + Convert.ToString(dtLocalAct.Rows[i]["VType"]) + "'" : "VType is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Narr1"])) ? "Narr1='" + Convert.ToString(dtLocalAct.Rows[i]["Narr1"]) + "'" : "Narr1 is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Narr2"])) ? "Narr2='" + Convert.ToString(dtLocalAct.Rows[i]["Narr2"]) + "'" : "Narr2 is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Narr3"])) ? "Narr3='" + Convert.ToString(dtLocalAct.Rows[i]["Narr3"]) + "'" : "Narr3 is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Narr4"])) ? "Narr4='" + Convert.ToString(dtLocalAct.Rows[i]["Narr4"]) + "'" : "Narr4 is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Contra"])) ? "Contra='" + Convert.ToString(dtLocalAct.Rows[i]["Contra"]) + "'" : "Contra is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Adj_Type"])) ? "Adj_Type='" + Convert.ToString(dtLocalAct.Rows[i]["Adj_Type"]) + "'" : "Adj_Type is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Adj_Amt"])) ? "Adj_Amt='" + Convert.ToString(dtLocalAct.Rows[i]["Adj_Amt"]) + "'" : "Adj_Amt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Amt"])) ? "Amt='" + Convert.ToString(dtLocalAct.Rows[i]["Amt"]) + "'" : "Amt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["RCode"])) ? "RCode='" + Convert.ToString(dtLocalAct.Rows[i]["RCode"]) + "'" : "RCode is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["CHNO"])) ? "CHNO='" + Convert.ToString(dtLocalAct.Rows[i]["CHNO"]) + "'" : "CHNO is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["ChBank"])) ? "ChBank='" + Convert.ToString(dtLocalAct.Rows[i]["ChBank"]) + "'" : "ChBank is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Division"])) ? "Division='" + Convert.ToString(dtLocalAct.Rows[i]["Division"]) + "'" : "Division is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < LEDGERColumns.Count; j++)
                //            {
                //                if (localTable.Columns[LEDGERColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(localTable.Rows[0][LEDGERColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][LEDGERColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.LEDGER, OperationTypes.UPDATE, localTable.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, LEDGERColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.LEDGER, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, LEDGERColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from LEDGER");

                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        DataView dtExpert = new DataView(dtLocalAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtExpertAct.Rows[i]["Link"]) + "'" : "Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Date"])) ? "[Date]='" + Convert.ToDateTime(dtExpertAct.Rows[i]["Date"]).ToString("dd/MM/yyyy") + "'" : "Date is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["ChDt"])) ? "[ChDt]='" + Convert.ToDateTime(dtExpertAct.Rows[i]["ChDt"]).ToString("dd/MM/yyyy") + "'" : "ChDt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["DocNo"])) ? "DocNo='" + Convert.ToString(dtExpertAct.Rows[i]["DocNo"]) + "'" : "DocNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["ClrDt"])) ? "ClrDt='" + Convert.ToString(dtExpertAct.Rows[i]["ClrDt"]) + "'" : "ClrDt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["VatCode"])) ? "VatCode='" + Convert.ToString(dtExpertAct.Rows[i]["VatCode"]) + "'" : "VatCode is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["DC"])) ? "DC='" + Convert.ToString(dtExpertAct.Rows[i]["DC"]) + "'" : "DC is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["VType"])) ? "VType='" + Convert.ToString(dtExpertAct.Rows[i]["VType"]) + "'" : "VType is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Narr1"])) ? "Narr1='" + Convert.ToString(dtExpertAct.Rows[i]["Narr1"]) + "'" : "Narr1 is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Narr2"])) ? "Narr2='" + Convert.ToString(dtExpertAct.Rows[i]["Narr2"]) + "'" : "Narr2 is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Narr3"])) ? "Narr3='" + Convert.ToString(dtExpertAct.Rows[i]["Narr3"]) + "'" : "Narr3 is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Narr4"])) ? "Narr4='" + Convert.ToString(dtExpertAct.Rows[i]["Narr4"]) + "'" : "Narr4 is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Contra"])) ? "Contra='" + Convert.ToString(dtExpertAct.Rows[i]["Contra"]) + "'" : "Contra is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Adj_Type"])) ? "Adj_Type='" + Convert.ToString(dtExpertAct.Rows[i]["Adj_Type"]) + "'" : "Adj_Type is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Adj_Amt"])) ? "Adj_Amt='" + Convert.ToString(dtExpertAct.Rows[i]["Adj_Amt"]) + "'" : "Adj_Amt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Amt"])) ? "Amt='" + Convert.ToString(dtExpertAct.Rows[i]["Amt"]) + "'" : "Amt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["RCode"])) ? "RCode='" + Convert.ToString(dtExpertAct.Rows[i]["RCode"]) + "'" : "RCode is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["CHNO"])) ? "CHNO='" + Convert.ToString(dtExpertAct.Rows[i]["CHNO"]) + "'" : "CHNO is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["ChBank"])) ? "ChBank='" + Convert.ToString(dtExpertAct.Rows[i]["ChBank"]) + "'" : "ChBank is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Division"])) ? "Division='" + Convert.ToString(dtExpertAct.Rows[i]["Division"]) + "'" : "Division is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < LEDGERColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[LEDGERColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][LEDGERColumns[j].Split('(')[0]]).Equals(Convert.ToString(localTable.Rows[0][LEDGERColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(localTable.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.LEDGER, OperationTypes.INSERT, localTable.Rows[0], null, ClientCompanyId, LEDGERColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.LEDGER, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, LEDGERColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    isFreshInsert = true;
                //    List<string> LEDGERColumns = GetTableColumns(TableNames.LEDGER, true);
                //    CheckExpertTmpFile(dr, "LEDGER.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(LEDGERColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from LEDGER");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.LEDGER, OperationTypes.DELETE, null, null, ClientCompanyId, LEDGERColumns));
                //    //dtLEDGER = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing Ledger.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.LEDGER, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, LEDGERColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "LEDGER Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region LEDMAST TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "LEDMAST Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\LEDMAST.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "LEDMAST.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from LEDMAST");
                //    dtExpertAct = GetDataFromExpert(expconn, "select * from LEDMAST");
                //    List<string> LEDMASTColumns = GetTableColumns(TableNames.LEDMAST, false);
                //    DataTable localTable = new DataTable();
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {

                //        DataView dtExpert = new DataView(dtExpertAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtLocalAct.Rows[i]["Link"]) + "'" : "Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Date"])) ? "[Date]='" + Convert.ToDateTime(dtLocalAct.Rows[i]["Date"]).ToString("dd/MM/yyyy") + "'" : "Date is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["DocNo"])) ? "DocNo='" + Convert.ToString(dtLocalAct.Rows[i]["DocNo"]) + "'" : "DocNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Amt"])) ? "Amt='" + Convert.ToString(dtLocalAct.Rows[i]["Amt"]) + "'" : "Amt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["PSCode"])) ? "PSCode='" + Convert.ToString(dtLocalAct.Rows[i]["PSCode"]) + "'" : "PSCode is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Division"])) ? "Division='" + Convert.ToString(dtLocalAct.Rows[i]["Division"]) + "'" : "Division is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < LEDMASTColumns.Count; j++)
                //            {
                //                if (localTable.Columns[LEDMASTColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(localTable.Rows[0][LEDMASTColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][LEDMASTColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.LEDMAST, OperationTypes.UPDATE, localTable.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, LEDMASTColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.LEDMAST, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, LEDMASTColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from LEDMAST");

                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        DataView dtExpert = new DataView(dtLocalAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtExpertAct.Rows[i]["Link"]) + "'" : "Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Date"])) ? "[Date]='" + Convert.ToDateTime(dtExpertAct.Rows[i]["Date"]).ToString("dd/MM/yyyy") + "'" : "Date is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["DocNo"])) ? "DocNo='" + Convert.ToString(dtExpertAct.Rows[i]["DocNo"]) + "'" : "DocNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Amt"])) ? "Amt='" + Convert.ToString(dtExpertAct.Rows[i]["Amt"]) + "'" : "Amt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["PSCode"])) ? "PSCode='" + Convert.ToString(dtExpertAct.Rows[i]["PSCode"]) + "'" : "PSCode is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Division"])) ? "Division='" + Convert.ToString(dtExpertAct.Rows[i]["Division"]) + "'" : "Division is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < LEDMASTColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[LEDMASTColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][LEDMASTColumns[j].Split('(')[0]]).Equals(Convert.ToString(localTable.Rows[0][LEDMASTColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(localTable.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.LEDMAST, OperationTypes.INSERT, localTable.Rows[0], null, ClientCompanyId, LEDMASTColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.LEDMAST, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, LEDMASTColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> LEDMASTColumns = GetTableColumns(TableNames.LEDMAST, true);
                //    CheckExpertTmpFile(dr, "LEDMAST.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(LEDMASTColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from LEDMAST");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.LEDMAST, OperationTypes.DELETE, null, null, ClientCompanyId, LEDMASTColumns));
                //    //dtLEDMAST = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing LedMast.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.LEDMAST, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, LEDMASTColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "LEDMAST Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region ORDER TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "ORDER Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\ORDER.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "ORDER.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from [ORDER]");
                //    dtExpertAct = GetDataFromExpert(expconn, "select * from [ORDER]");
                //    List<string> ORDERColumns = GetTableColumns(TableNames.ORDER, false);
                //    DataTable localTable = new DataTable();
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {

                //        DataView dtExpert = new DataView(dtExpertAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Ord_No"])) ? "Ord_No='" + Convert.ToString(dtLocalAct.Rows[i]["Ord_No"]) + "'" : "Ord_No is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Ord_Dt"])) ? "[Ord_Dt]='" + Convert.ToDateTime(dtLocalAct.Rows[i]["Ord_Dt"]).ToString("dd/MM/yyyy") + "'" : "Ord_Dt is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < ORDERColumns.Count; j++)
                //            {
                //                if (localTable.Columns[ORDERColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(localTable.Rows[0][ORDERColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][ORDERColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ORDER, OperationTypes.UPDATE, localTable.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, ORDERColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ORDER, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, ORDERColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from [ORDER]");

                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        DataView dtExpert = new DataView(dtLocalAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Ord_No"])) ? "Ord_No='" + Convert.ToString(dtExpertAct.Rows[i]["Ord_No"]) + "'" : "Ord_No is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Ord_Dt"])) ? "[Ord_Dt]='" + Convert.ToDateTime(dtExpertAct.Rows[i]["Ord_Dt"]).ToString("dd/MM/yyyy") + "'" : "Ord_Dt is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < ORDERColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[ORDERColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][ORDERColumns[j].Split('(')[0]]).Equals(Convert.ToString(localTable.Rows[0][ORDERColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(localTable.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ORDER, OperationTypes.INSERT, localTable.Rows[0], null, ClientCompanyId, ORDERColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ORDER, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, ORDERColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> ORDERColumns = GetTableColumns(TableNames.ORDER, true);
                //    CheckExpertTmpFile(dr, "ORDER.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(ORDERColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from [ORDER]");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ORDER, OperationTypes.DELETE, null, null, ClientCompanyId, ORDERColumns));
                //    //dtORDER = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing Order.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ORDER, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, ORDERColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "ORDER Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region ORDER2 TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "ORDER2 Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\ORDER2.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "ORDER2.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from ORDER2");
                //    dtExpertAct = GetDataFromExpert(expconn, "select * from ORDER2");
                //    List<string> ORDER2Columns = GetTableColumns(TableNames.ORDER2, false);
                //    DataTable localTable = new DataTable();
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {

                //        DataView dtExpert = new DataView(dtExpertAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Ord_No"])) ? "Ord_No='" + Convert.ToString(dtLocalAct.Rows[i]["Ord_No"]) + "'" : "Ord_No is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Ord_Dt"])) ? "[Ord_Dt]='" + Convert.ToDateTime(dtLocalAct.Rows[i]["Ord_Dt"]).ToString("dd/MM/yyyy") + "'" : "Ord_Dt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Dly_Dt"])) ? "[Dly_Dt]='" + Convert.ToDateTime(dtLocalAct.Rows[i]["Dly_Dt"]).ToString("dd/MM/yyyy") + "'" : "Dly_Dt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Qty"])) ? "Qty='" + Convert.ToString(dtLocalAct.Rows[i]["Qty"]) + "'" : "Qty is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Stk_Qty"])) ? "Stk_Qty='" + Convert.ToString(dtLocalAct.Rows[i]["Stk_Qty"]) + "'" : "Stk_Qty is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Division"])) ? "Division='" + Convert.ToString(dtLocalAct.Rows[i]["Division"]) + "'" : "Division is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Cv_Code"])) ? "Cv_Code='" + Convert.ToString(dtLocalAct.Rows[i]["Cv_Code"]) + "'" : "Cv_Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Batch_No"])) ? "Batch_No='" + Convert.ToString(dtLocalAct.Rows[i]["Batch_No"]) + "'" : "Batch_No is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Rate"])) ? "Rate='" + Convert.ToString(dtLocalAct.Rows[i]["Rate"]) + "'" : "Rate is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < ORDER2Columns.Count; j++)
                //            {
                //                if (localTable.Columns[ORDER2Columns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(localTable.Rows[0][ORDER2Columns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][ORDER2Columns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ORDER2, OperationTypes.UPDATE, localTable.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, ORDER2Columns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ORDER2, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, ORDER2Columns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from ORDER2");

                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        DataView dtExpert = new DataView(dtLocalAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Ord_No"])) ? "Ord_No='" + Convert.ToString(dtExpertAct.Rows[i]["Ord_No"]) + "'" : "Ord_No is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Ord_Dt"])) ? "[Ord_Dt]='" + Convert.ToDateTime(dtExpertAct.Rows[i]["Ord_Dt"]).ToString("dd/MM/yyyy") + "'" : "Ord_Dt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Dly_Dt"])) ? "[Dly_Dt]='" + Convert.ToDateTime(dtExpertAct.Rows[i]["Dly_Dt"]).ToString("dd/MM/yyyy") + "'" : "Dly_Dt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Qty"])) ? "Qty='" + Convert.ToString(dtExpertAct.Rows[i]["Qty"]) + "'" : "Qty is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Stk_Qty"])) ? "Stk_Qty='" + Convert.ToString(dtExpertAct.Rows[i]["Stk_Qty"]) + "'" : "Stk_Qty is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Division"])) ? "Division='" + Convert.ToString(dtExpertAct.Rows[i]["Division"]) + "'" : "Division is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Cv_Code"])) ? "Cv_Code='" + Convert.ToString(dtExpertAct.Rows[i]["Cv_Code"]) + "'" : "Cv_Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Batch_No"])) ? "Batch_No='" + Convert.ToString(dtExpertAct.Rows[i]["Batch_No"]) + "'" : "Batch_No is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Rate"])) ? "Rate='" + Convert.ToString(dtExpertAct.Rows[i]["Rate"]) + "'" : "Rate is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < ORDER2Columns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[ORDER2Columns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][ORDER2Columns[j].Split('(')[0]]).Equals(Convert.ToString(localTable.Rows[0][ORDER2Columns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(localTable.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ORDER2, OperationTypes.INSERT, localTable.Rows[0], null, ClientCompanyId, ORDER2Columns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ORDER2, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, ORDER2Columns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> ORDER2Columns = GetTableColumns(TableNames.ORDER2, true);
                //    CheckExpertTmpFile(dr, "ORDER2.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(ORDER2Columns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from ORDER2");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ORDER2, OperationTypes.DELETE, null, null, ClientCompanyId, ORDER2Columns));
                //    //dtORDER2 = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing Order2.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.ORDER2, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, ORDER2Columns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "ORDER2 Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region PGROUP TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "PGROUP Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\PGROUP.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "PGROUP.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from PGROUP");
                //    List<string> PGROUPColumns = GetTableColumns(TableNames.AGENT, false);
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {
                //        dtExpertAct = GetDataFromExpert(expconn, "select * from PGROUP where " +
                //            (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtExpertAct != null && dtExpertAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < PGROUPColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[PGROUPColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[0][PGROUPColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][PGROUPColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.PGROUP, OperationTypes.UPDATE, dtExpertAct.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, PGROUPColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.PGROUP, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, PGROUPColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from PGROUP");
                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        dtLocalAct = GetDataFromExpert(localconn, "select * from PGROUP where " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtLocalAct != null && dtLocalAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < PGROUPColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[PGROUPColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][PGROUPColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[0][PGROUPColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(dtLocalAct.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.PGROUP, OperationTypes.INSERT, dtLocalAct.Rows[0], null, ClientCompanyId, PGROUPColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.PGROUP, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, PGROUPColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> PGROUPColumns = GetTableColumns(TableNames.PGROUP, true);
                //    CheckExpertTmpFile(dr, "PGROUP.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(PGROUPColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from PGROUP");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.PGROUP, OperationTypes.DELETE, null, null, ClientCompanyId, PGROUPColumns));
                //    //dtPGROUP = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing Pgroup.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.PGROUP, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, PGROUPColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "PGROUP Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region PRODUCT TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "PRODUCT Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\PRODUCT.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "PRODUCT.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from [PRODUCT]");
                //    List<string> PRODUCTColumns = GetTableColumns(TableNames.PRODUCT, false);
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {
                //        dtExpertAct = GetDataFromExpert(expconn, "select * from [PRODUCT] where " +
                //            (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtExpertAct != null && dtExpertAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < PRODUCTColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[PRODUCTColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[0][PRODUCTColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][PRODUCTColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.PRODUCT, OperationTypes.UPDATE, dtExpertAct.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, PRODUCTColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.PRODUCT, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, PRODUCTColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from [PRODUCT]");
                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        dtLocalAct = GetDataFromExpert(localconn, "select * from [PRODUCT] where " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtLocalAct != null && dtLocalAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < PRODUCTColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[PRODUCTColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][PRODUCTColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[0][PRODUCTColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(dtLocalAct.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.PRODUCT, OperationTypes.INSERT, dtLocalAct.Rows[0], null, ClientCompanyId, PRODUCTColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.PRODUCT, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, PRODUCTColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> PRODUCTColumns = GetTableColumns(TableNames.PRODUCT, true);
                //    CheckExpertTmpFile(dr, "PRODUCT.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(PRODUCTColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from PRODUCT");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.PRODUCT, OperationTypes.DELETE, null, null, ClientCompanyId, PRODUCTColumns));
                //    //dtPRODUCT = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing Product.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.PRODUCT, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, PRODUCTColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "PRODUCT Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region SALE_ADJ TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "SALE_ADJ Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\SALE_ADJ.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "SALE_ADJ.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from SALE_ADJ");
                //    dtExpertAct = GetDataFromExpert(expconn, "select * from SALE_ADJ");
                //    List<string> SALE_ADJColumns = GetTableColumns(TableNames.SALE_ADJ, false);
                //    DataTable localTable = new DataTable();
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {

                //        DataView dtExpert = new DataView(dtExpertAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["SP_Link"])) ? "SP_Link='" + Convert.ToString(dtLocalAct.Rows[i]["SP_Link"]) + "'" : "SP_Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["OT_Link"])) ? "OT_Link='" + Convert.ToString(dtLocalAct.Rows[i]["OT_Link"]) + "'" : "OT_Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Amt"])) ? "Amt='" + Convert.ToString(dtLocalAct.Rows[i]["Amt"]) + "'" : "Amt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["DC"])) ? "DC='" + Convert.ToString(dtLocalAct.Rows[i]["DC"]) + "'" : "DC is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Disc"])) ? "Disc='" + Convert.ToString(dtLocalAct.Rows[i]["Disc"]) + "'" : "Disc is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < SALE_ADJColumns.Count; j++)
                //            {
                //                if (localTable.Columns[SALE_ADJColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(localTable.Rows[0][SALE_ADJColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][SALE_ADJColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SALE_ADJ, OperationTypes.UPDATE, localTable.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, SALE_ADJColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SALE_ADJ, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, SALE_ADJColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from SALE_ADJ");

                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        DataView dtExpert = new DataView(dtLocalAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["SP_Link"])) ? "SP_Link='" + Convert.ToString(dtExpertAct.Rows[i]["SP_Link"]) + "'" : "SP_Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["OT_Link"])) ? "OT_Link='" + Convert.ToString(dtExpertAct.Rows[i]["OT_Link"]) + "'" : "OT_Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Amt"])) ? "Amt='" + Convert.ToString(dtExpertAct.Rows[i]["Amt"]) + "'" : "Amt is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["DC"])) ? "DC='" + Convert.ToString(dtExpertAct.Rows[i]["DC"]) + "'" : "DC is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Disc"])) ? "Disc='" + Convert.ToString(dtExpertAct.Rows[i]["Disc"]) + "'" : "Disc is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < SALE_ADJColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[SALE_ADJColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][SALE_ADJColumns[j].Split('(')[0]]).Equals(Convert.ToString(localTable.Rows[0][SALE_ADJColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(localTable.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SALE_ADJ, OperationTypes.INSERT, localTable.Rows[0], null, ClientCompanyId, SALE_ADJColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SALE_ADJ, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, SALE_ADJColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> SALE_ADJColumns = GetTableColumns(TableNames.SALE_ADJ, true);
                //    CheckExpertTmpFile(dr, "SALE_ADJ.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(SALE_ADJColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from SALE_ADJ");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SALE_ADJ, OperationTypes.DELETE, null, null, ClientCompanyId, SALE_ADJColumns));
                //    //dtSALE_ADJ = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing Sale_Adj.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SALE_ADJ, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, SALE_ADJColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "SALE_ADJ Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region SERIAL TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "SERIAL Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\SERIAL.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "SERIAL.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from SERIAL");
                //    dtExpertAct = GetDataFromExpert(expconn, "select * from SERIAL");
                //    List<string> SERIALColumns = GetTableColumns(TableNames.SERIAL, false);
                //    DataTable localTable = new DataTable();
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {
                //        DataView dtExpert = new DataView(dtExpertAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["BatchNo"])) ? "BatchNo='" + Convert.ToString(dtLocalAct.Rows[i]["BatchNo"]) + "'" : "BatchNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["SerialNo"])) ? "[SerialNo]='" + Convert.ToString(dtLocalAct.Rows[i]["SerialNo"]) + "'" : "SerialNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Qty"])) ? "Qty='" + Convert.ToString(dtLocalAct.Rows[i]["Qty"]) + "'" : "Qty is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Posifile"])) ? "Posifile='" + Convert.ToString(dtLocalAct.Rows[i]["Posifile"]) + "'" : "Posifile is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtLocalAct.Rows[i]["Link"]) + "'" : "Link is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < SERIALColumns.Count; j++)
                //            {
                //                if (localTable.Columns[SERIALColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(localTable.Rows[0][SERIALColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][SERIALColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SERIAL, OperationTypes.UPDATE, localTable.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, SERIALColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SERIAL, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, SERIALColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from SERIAL");

                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        DataView dtExpert = new DataView(dtLocalAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["BatchNo"])) ? "BatchNo='" + Convert.ToString(dtExpertAct.Rows[i]["BatchNo"]) + "'" : "BatchNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["SerialNo"])) ? "[SerialNo]='" + Convert.ToString(dtExpertAct.Rows[i]["SerialNo"]) + "'" : "SerialNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Qty"])) ? "Qty='" + Convert.ToString(dtExpertAct.Rows[i]["Qty"]) + "'" : "Qty is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Posifile"])) ? "Posifile='" + Convert.ToString(dtExpertAct.Rows[i]["Posifile"]) + "'" : "Posifile is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtExpertAct.Rows[i]["Link"]) + "'" : "Link is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < SERIALColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[SERIALColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][SERIALColumns[j].Split('(')[0]]).Equals(Convert.ToString(localTable.Rows[0][SERIALColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(localTable.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SERIAL, OperationTypes.INSERT, localTable.Rows[0], null, ClientCompanyId, SERIALColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SERIAL, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, SERIALColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    if (File.Exists(UploadingExpertPath + "\\" + dr["CompanyCode"].ToString().Trim().PadLeft(3, '0') + "\\Serial.DBF"))
                //    {
                //        List<string> SERIALColumns = GetTableColumns(TableNames.SERIAL, true);
                //        CheckExpertTmpFile(dr, "SERIAL.DBF", UploadingExpertDir);
                //        var columnName = GetExperColumnNamesForInsert(SERIALColumns);
                //        var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from SERIAL");
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SERIAL, OperationTypes.DELETE, null, null, ClientCompanyId, SERIALColumns));
                //        //dtSERIAL = dt;

                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadStatus.Text = "Processing Serial.dbf : ";
                //            toolUploadProgress.Maximum = dt.Rows.Count;
                //            toolUploadProgress.Minimum = 0;
                //        }));

                //        for (var i = 0; i < dt.Rows.Count; i++)
                //        {
                //            this.Invoke(new MethodInvoker(delegate
                //            {
                //                toolUploadProgress.Value = i;
                //            }));
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SERIAL, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, SERIALColumns));
                //        }
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "SERIAL Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region STAX TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "STAX Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\STAX.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "STAX.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from STAX");
                //    List<string> STAXColumns = GetTableColumns(TableNames.AGENT, false);
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {
                //        dtExpertAct = GetDataFromExpert(expconn, "select * from STAX where " +
                //            (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtExpertAct != null && dtExpertAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < STAXColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[STAXColumns[j].Split('(')[0]] != null && !Convert.ToString(dtExpertAct.Rows[0][STAXColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][STAXColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STAX, OperationTypes.UPDATE, dtExpertAct.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, STAXColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STAX, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, STAXColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from STAX");
                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        dtLocalAct = GetDataFromExpert(localconn, "select * from STAX where " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null"));
                //        if (dtLocalAct != null && dtLocalAct.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < STAXColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[STAXColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][STAXColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[0][STAXColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(dtLocalAct.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STAX, OperationTypes.INSERT, dtLocalAct.Rows[0], null, ClientCompanyId, STAXColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STAX, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, STAXColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> STAXColumns = GetTableColumns(TableNames.STAX, true);
                //    CheckExpertTmpFile(dr, "STAX.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(STAXColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from [STAX]");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STAX, OperationTypes.DELETE, null, null, ClientCompanyId, STAXColumns));
                //    //dtSTAX = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing STAX.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STAX, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, STAXColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "STAX Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region SP TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "SP Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\SP.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "SP.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from SP");
                //    dtExpertAct = GetDataFromExpert(expconn, "select * from SP");
                //    List<string> SPColumns = GetTableColumns(TableNames.SP, false);
                //    DataTable localTable = new DataTable();
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {

                //        DataView dtExpert = new DataView(dtExpertAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtLocalAct.Rows[i]["Link"]) + "'" : "Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Date"])) ? "[Date]='" + Convert.ToDateTime(dtLocalAct.Rows[i]["Date"]).ToString("dd/MM/yyyy") + "'" : "Date is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["DocNo"])) ? "DocNo='" + Convert.ToString(dtLocalAct.Rows[i]["DocNo"]) + "'" : "DocNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Division"])) ? "Division='" + Convert.ToString(dtLocalAct.Rows[i]["Division"]) + "'" : "Division is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < SPColumns.Count; j++)
                //            {
                //                if (localTable.Columns[SPColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(localTable.Rows[0][SPColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][SPColumns[j].Split('(')[0]])))
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SP, OperationTypes.UPDATE, localTable.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, SPColumns));
                //                    break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SP, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, SPColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from SP");

                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        DataView dtExpert = new DataView(dtLocalAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtExpertAct.Rows[i]["Link"]) + "'" : "Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Date"])) ? "Date='" + Convert.ToString(dtExpertAct.Rows[i]["Date"]) + "'" : "Date is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["DocNo"])) ? "DocNo='" + Convert.ToString(dtExpertAct.Rows[i]["DocNo"]) + "'" : "DocNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Division"])) ? "Division='" + Convert.ToString(dtExpertAct.Rows[i]["Division"]) + "'" : "Division is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            for (var j = 0; j < SPColumns.Count; j++)
                //            {
                //                if (dtExpertAct.Columns[SPColumns[j].Split('(')[0]] != null &&
                //                    !Convert.ToString(dtExpertAct.Rows[i][SPColumns[j].Split('(')[0]]).Equals(Convert.ToString(localTable.Rows[0][SPColumns[j].Split('(')[0]])))
                //                {
                //                    dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(localTable.Rows[0]["Code"] + "'");
                //                    if (dtExpertAct.DefaultView.Count == 0)
                //                    {
                //                        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SP, OperationTypes.INSERT, localTable.Rows[0], null, ClientCompanyId, SPColumns));
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SP, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, SPColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> SPColumns = GetTableColumns(TableNames.SP, true);
                //    CheckExpertTmpFile(dr, "SP.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(SPColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from SP");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SP, OperationTypes.DELETE, null, null, ClientCompanyId, SPColumns));
                //    //dtSP = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing SP.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.SP, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, SPColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "SP Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion

                //#region STOCK TABLE SYNC PROCESS
                //startTime = DateTime.Now;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "STOCK Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.ErrorLog);
                //if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\STOCK.DBF"))
                //{
                //    CheckExpertTmpFile(dr, "STOCK.DBF", UploadingExpertDir);
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from STOCK");
                //    dtExpertAct = GetDataFromExpert(expconn, "select * from STOCK");
                //    DataTable localTable = new DataTable();
                //    List<string> STOCKColumns = GetTableColumns(TableNames.STOCK, false);
                //    for (var i = 0; i < dtLocalAct.Rows.Count; i++)
                //    {
                //        DataView dtExpert = new DataView(dtExpertAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtLocalAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtLocalAct.Rows[i]["Link"]) + "'" : "Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["BatchNo"])) ? "BatchNo='" + Convert.ToString(dtLocalAct.Rows[i]["BatchNo"]) + "'" : "BatchNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Qty"])) ? "Qty=" + Convert.ToString(dtLocalAct.Rows[i]["Qty"]) : "Qty is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["DocNo"])) ? "DocNo='" + Convert.ToString(dtLocalAct.Rows[i]["DocNo"]) + "'" : "DocNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Division"])) ? "Division='" + Convert.ToString(dtLocalAct.Rows[i]["Division"]) + "'" : "Division is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["Value"])) ? "Value=" + Convert.ToString(dtLocalAct.Rows[i]["Value"]) : "Value is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtLocalAct.Rows[i]["PSCode"])) ? "PSCode='" + Convert.ToString(dtLocalAct.Rows[i]["PSCode"]) + "'" : "PSCode is null");
                //        localTable = dtExpert.ToTable();
                //        if (localTable != null && localTable.Rows.Count > 0)
                //        {
                //            if (!Convert.ToString(localTable.Rows[0]["Link"]).Equals(Convert.ToString(dtLocalAct.Rows[i]["Link"])) ||
                //                !Convert.ToString(localTable.Rows[0]["Code"]).Equals(Convert.ToString(dtLocalAct.Rows[i]["Code"])) ||
                //                !Convert.ToString(localTable.Rows[0]["BatchNo"]).Equals(Convert.ToString(dtLocalAct.Rows[i]["BatchNo"])) ||
                //                !Convert.ToString(localTable.Rows[0]["Qty"]).Equals(Convert.ToString(dtLocalAct.Rows[i]["Qty"])) ||
                //                !Convert.ToString(localTable.Rows[0]["DocNo"]).Equals(Convert.ToString(dtLocalAct.Rows[i]["DocNo"])) ||
                //                !Convert.ToString(localTable.Rows[0]["Division"]).Equals(Convert.ToString(dtLocalAct.Rows[i]["Division"])) ||
                //                !Convert.ToString(localTable.Rows[0]["Value"]).Equals(Convert.ToString(dtLocalAct.Rows[i]["Value"])) ||
                //                !Convert.ToString(localTable.Rows[0]["PSCode"]).Equals(Convert.ToString(dtLocalAct.Rows[i]["PSCode"])))
                //            {
                //                strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STOCK, OperationTypes.UPDATE, localTable.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, STOCKColumns));
                //                //break;
                //            }
                //            //for (var j = 0; j < STOCKColumns.Count; j++)
                //            //{
                //            //    if (dtExpertAct.Columns[STOCKColumns[j].Split('(')[0]] != null &&
                //            //        !Convert.ToString(dtExpertAct.Rows[0][STOCKColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[i][STOCKColumns[j].Split('(')[0]])))
                //            //    {
                //            //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STOCK, OperationTypes.UPDATE, dtExpertAct.Rows[0], dtLocalAct.Rows[i], ClientCompanyId, STOCKColumns));
                //            //        break;
                //            //    }
                //            //}
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STOCK, OperationTypes.DELETE, dtLocalAct.Rows[i], dtLocalAct.Rows[i], ClientCompanyId, STOCKColumns));
                //        }
                //    }

                //    dtExpertAct = GetDataFromExpert(expconn, "select * from STOCK");
                //    dtLocalAct = GetDataFromExpert(localconn, "select * from STOCK");
                //    for (var i = 0; i < dtExpertAct.Rows.Count; i++)
                //    {
                //        DataView dtExpert = new DataView(dtExpertAct);
                //        dtExpert.RowFilter = (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Code"])) ? "Code='" + Convert.ToString(dtExpertAct.Rows[i]["Code"]) + "'" : "Code is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Link"])) ? "Link='" + Convert.ToString(dtExpertAct.Rows[i]["Link"]) + "'" : "Link is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["BatchNo"])) ? "BatchNo='" + Convert.ToString(dtExpertAct.Rows[i]["BatchNo"]) + "'" : "BatchNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Qty"])) ? "Qty=" + Convert.ToString(dtExpertAct.Rows[i]["Qty"]) : "Qty is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["DocNo"])) ? "DocNo='" + Convert.ToString(dtExpertAct.Rows[i]["DocNo"]) + "'" : "DocNo is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Division"])) ? "Division='" + Convert.ToString(dtExpertAct.Rows[i]["Division"]) + "'" : "Division is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["Value"])) ? "Value=" + Convert.ToString(dtExpertAct.Rows[i]["Value"]) : "Value is null") +
                //            " and " + (!string.IsNullOrEmpty(Convert.ToString(dtExpertAct.Rows[i]["PSCode"])) ? "PSCode='" + Convert.ToString(dtExpertAct.Rows[i]["PSCode"]) + "'" : "PSCode is null");
                //        localTable = dtExpert.ToTable();

                //        if (dtLocalAct != null && dtLocalAct.Rows.Count > 0)
                //        {
                //            if (!Convert.ToString(dtExpertAct.Rows[i]["Link"]).Equals(Convert.ToString(localTable.Rows[0]["Link"])) ||
                //                !Convert.ToString(dtExpertAct.Rows[i]["Code"]).Equals(Convert.ToString(localTable.Rows[0]["Code"])) ||
                //                !Convert.ToString(dtExpertAct.Rows[i]["BatchNo"]).Equals(Convert.ToString(localTable.Rows[0]["BatchNo"])) ||
                //                !Convert.ToString(dtExpertAct.Rows[i]["Qty"]).Equals(Convert.ToString(localTable.Rows[0]["Qty"])) ||
                //                !Convert.ToString(dtExpertAct.Rows[i]["DocNo"]).Equals(Convert.ToString(localTable.Rows[0]["DocNo"])) ||
                //                !Convert.ToString(dtExpertAct.Rows[i]["Division"]).Equals(Convert.ToString(localTable.Rows[0]["Division"])) ||
                //                !Convert.ToString(dtExpertAct.Rows[i]["Value"]).Equals(Convert.ToString(localTable.Rows[0]["Value"])) ||
                //                !Convert.ToString(dtExpertAct.Rows[i]["PSCode"]).Equals(Convert.ToString(localTable.Rows[0]["PSCode"])))
                //            {
                //                dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(localTable.Rows[0]["Code"] + "'");
                //                if (dtExpertAct.DefaultView.Count == 0)
                //                {
                //                    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STOCK, OperationTypes.INSERT, localTable.Rows[0], null, ClientCompanyId, STOCKColumns));
                //                    //break;
                //                }
                //            }
                //            //for (var j = 0; j < STOCKColumns.Count; j++)
                //            //{
                //            //    if (dtExpertAct.Columns[STOCKColumns[j].Split('(')[0]] != null &&
                //            //        !Convert.ToString(dtExpertAct.Rows[i][STOCKColumns[j].Split('(')[0]]).Equals(Convert.ToString(dtLocalAct.Rows[0][STOCKColumns[j].Split('(')[0]])))
                //            //    {
                //            //        dtExpertAct.DefaultView.RowFilter = "Code='" + Convert.ToString(dtLocalAct.Rows[0]["Code"] + "'");
                //            //        if (dtExpertAct.DefaultView.Count == 0)
                //            //        {
                //            //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STOCK, OperationTypes.INSERT, dtLocalAct.Rows[0], null, ClientCompanyId, STOCKColumns));
                //            //            break;
                //            //        }
                //            //    }
                //            //}
                //        }
                //        else
                //        {
                //            strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STOCK, OperationTypes.INSERT, dtExpertAct.Rows[i], null, ClientCompanyId, STOCKColumns));
                //        }
                //    }
                //}
                //else
                //{
                //    List<string> STOCKColumns = GetTableColumns(TableNames.STOCK, true);
                //    CheckExpertTmpFile(dr, "STOCK.DBF", UploadingExpertDir);
                //    var columnName = GetExperColumnNamesForInsert(STOCKColumns);
                //    var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from STOCK");
                //    strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STOCK, OperationTypes.DELETE, null, null, ClientCompanyId, STOCKColumns));
                //    //dtSTOCK = dt;

                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        toolUploadStatus.Text = "Processing Stock.dbf : ";
                //        toolUploadProgress.Maximum = dt.Rows.Count;
                //        toolUploadProgress.Minimum = 0;
                //    }));
                //    for (var i = 0; i < dt.Rows.Count; i++)
                //    {
                //        this.Invoke(new MethodInvoker(delegate
                //        {
                //            toolUploadProgress.Value = i;
                //        }));
                //        strQueries.Add(InsertUpdateQueries.GetQueries(TableNames.STOCK, OperationTypes.INSERT, dt.Rows[i], null, ClientCompanyId, STOCKColumns));
                //    }
                //}
                //endTime = DateTime.Now - startTime;
                //Operation.writeLog("====================================================================" + Environment.NewLine + "STOCK Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.ErrorLog);
                //#endregion
                #endregion
                if (strQueries.Count > 0)
                {
                    Operation.writeLog("====================================================================" + Environment.NewLine + "Before query starting: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ", Total Queries : " + strQueries.Count, Operation.LogFile);
                    //if (isFreshInsert)
                    //{
                    //    PerformFreshInsert(UploadingExpertPath, UploadingExpertDir, strQueries);
                    //    isFreshInsert = false;
                    //}
                    this.Invoke(new MethodInvoker(delegate
                    {
                        toolUploadStatus.Text = "Uploading data to server : ";
                    }));
                    if (strQueries.Count > 10000)
                    {
                        try
                        {
                            Application.DoEvents();
                            this.Invoke(new MethodInvoker(delegate
                            {
                                toolUploadProgress.Maximum = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(strQueries.Count / 10000))) + 1;
                                toolUploadProgress.Value = 0;
                            }));
                            for (var i = 0; i < strQueries.Count; i += 10000)
                            {
                                var strChunk = strQueries.GetRange(i, (i + 10000 < strQueries.Count ? 10000 : strQueries.Count - i));
                                Operation.ExecuteNonQuery(string.Join(";", strChunk.ToArray()), Operation.Conn);
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    toolUploadProgress.Value++;
                                }));
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        Operation.ExecuteNonQuery(string.Join(";", strQueries.ToArray()), Operation.Conn);
                    }

                    Operation.writeLog("====================================================================" + Environment.NewLine + "Query execution Ends at: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.LogFile);
                    List<string> tables = new List<string>();
                    foreach (TableNames r in Enum.GetValues(typeof(TableNames)))
                    {
                        tables.Add(r.ToString() + ".DBF");
                    }
                    CopyExpertToLocal(dr, string.Join(",", tables.ToArray()), UploadingExpertDir);
                    Operation.ForceSync = false;
                    Operation.SetIniValue();
                }
            }

            //throw new Exception("Done");
        }

        private List<string> GetExperColumnNamesForInsert(List<string> arr)
        {
            var temp = new List<string>();
            if (arr.Count > 0)
            {
                foreach (var item in arr)
                {
                    if (!item.Contains("ClientCompanyId"))
                    {
                        temp.Add("[" + item.Split('(')[0] + "]");
                    }

                }
            }
            return temp;
        }

        private void PerformFreshInsert(string UploadingExpertPath, string UploadingExpertDir, List<string> strQueries)
        {
            SqlCommand cmd = new SqlCommand("SyncRecord", Operation.Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@ExecQuery";
            param.Value = string.Join(";", strQueries.ToArray());
            cmd.Parameters.Add(param);

            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@dtAct";
            param1.Value = dtACT;
            cmd.Parameters.Add(param1);

            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@dtAdvance";
            param2.Value = dtADVANCE;
            cmd.Parameters.Add(param2);

            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@dtAgent";
            param3.Value = dtAGENT;
            cmd.Parameters.Add(param3);

            SqlParameter param4 = new SqlParameter();
            param4.ParameterName = "@dtBatch";
            param4.Value = dtBATCH;
            cmd.Parameters.Add(param4);

            SqlParameter param5 = new SqlParameter();
            param5.ParameterName = "@dtCashCust";
            param5.Value = dtCASHCUST;
            cmd.Parameters.Add(param5);

            SqlParameter param6 = new SqlParameter();
            param6.ParameterName = "@dtFormMast";
            param6.Value = dtFORMMAST;
            cmd.Parameters.Add(param6);

            SqlParameter param7 = new SqlParameter();
            param7.ParameterName = "@dtGroup";
            param7.Value = dtGROUP;
            cmd.Parameters.Add(param7);

            SqlParameter param8 = new SqlParameter();
            param8.ParameterName = "@dtLedger";
            param8.Value = dtLEDGER;
            cmd.Parameters.Add(param8);

            SqlParameter param9 = new SqlParameter();
            param9.ParameterName = "@dtLedMast";
            param9.Value = dtLEDMAST;
            cmd.Parameters.Add(param9);

            SqlParameter param10 = new SqlParameter();
            param10.ParameterName = "@dtOrder";
            param10.Value = dtORDER;
            cmd.Parameters.Add(param10);

            SqlParameter param11 = new SqlParameter();
            param11.ParameterName = "@dtOrder2";
            param11.Value = dtORDER2;
            cmd.Parameters.Add(param11);

            SqlParameter param12 = new SqlParameter();
            param12.ParameterName = "@dtPGroup";
            param12.Value = dtPGROUP;
            cmd.Parameters.Add(param12);

            SqlParameter param13 = new SqlParameter();
            param13.ParameterName = "@dtProduct";
            param13.Value = dtPRODUCT;
            cmd.Parameters.Add(param13);

            SqlParameter param14 = new SqlParameter();
            param14.ParameterName = "@dtSale_adj";
            param14.Value = dtSALE_ADJ;
            cmd.Parameters.Add(param14);

            SqlParameter param15 = new SqlParameter();
            param15.ParameterName = "@dtSp";
            param15.Value = dtSP;
            cmd.Parameters.Add(param15);

            SqlParameter param16 = new SqlParameter();
            param16.ParameterName = "@dtStax";
            param16.Value = dtSTAX;
            cmd.Parameters.Add(param16);

            SqlParameter param17 = new SqlParameter();
            param17.ParameterName = "@dtStock";
            param17.Value = dtSTOCK;
            cmd.Parameters.Add(param17);

            SqlParameter param18 = new SqlParameter();
            param18.ParameterName = "@dtITVat";
            param18.Value = dtITVAT;
            cmd.Parameters.Add(param18);

            SqlParameter param19 = new SqlParameter();
            param19.ParameterName = "@dtSerial";
            if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\SERIAL.DBF"))
            {
                param19.Value = dtSERIAL;
            }
            else
            {
                param19.Value = null;
            }
            cmd.Parameters.Add(param19);

            cmd.CommandTimeout = 0;
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private List<string> GetTableColumns(TableNames tableName, bool isFirstTime)
        {
            Operation.GetIniValue();
            List<string> columns = new List<string>();
            if (Operation.ForceUpdateColumns || isFirstTime)
            {
                columns = GetColumnListFromServer(tableName);
                //var insertCol = new List<string>();
                //foreach (var item in columns)
                //{
                //    if (Convert.ToString(item.Split('(')[0]).ToUpper() == "CLIENTCOMPANYID")
                //    {
                //        insertCol.Add(ClientCompanyId + " as [" + Convert.ToString(item.Split('(')[0]) + "]");
                //    }
                //    else
                //    {
                //    insertCol.Add("[" + Convert.ToString(item.Split('(')[0]) + "]");
                //    }

                //}
                //return insertCol;
            }
            else
            {
                columns = ReadColumnListFromFile(tableName);
            }
            Operation.ForceUpdateColumns = false;
            Operation.SetIniValue();
            return columns;
        }

        private List<string> ReadColumnListFromFile(TableNames tableNames)
        {
            List<string> columns = new List<string>();
            var tableColumns = File.ReadAllText(Application.StartupPath + "\\Columns.txt").Split(';');
            if (tableColumns.Length > 0)
            {
                foreach (var tables in tableColumns)
                {
                    if (!string.IsNullOrEmpty(tables))
                    {
                        var tName = tables.Split('=')[0];
                        var tColumns = tables.Split('=')[1];
                        if (!string.IsNullOrEmpty(tName))
                        {
                            if (tableNames.ToString() == tName)
                            {
                                columns.AddRange(tColumns.Split(','));
                                break;
                            }
                        }
                    }
                }
            }
            return columns;
        }

        private List<string> GetColumnListFromServer(TableNames tableName)
        {
            List<string> columns = new List<string>();
            var dt = Operation.GetDataTable("SELECT *, Column_Name as Name,data_type as Type" +
" FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + tableName.ToString() + "'", Operation.Conn);
            if (dt != null)
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    columns.Add(Convert.ToString(dt.Rows[i]["Name"]) + "(" + Convert.ToString(dt.Rows[i]["Type"]) + ")");
                }
            }
            FileStream fs = new FileStream(Application.StartupPath + "\\Columns.txt", FileMode.Append);
            StreamWriter str = new StreamWriter(fs);
            str.Write(tableName + "=" + string.Join(",", columns.ToArray()) + ";");
            str.Flush();
            str.Close();
            fs.Close();
            return columns;
        }

        private void CopyExpertToLocal(DataRow dr, string fileName, string UploadingExpertDir)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                if (fileName.Contains(","))
                {
                    var files = fileName.Split(',');
                    if (files != null)
                    {
                        for (var i = 0; i < files.Length; i++)
                        {
                            if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\" + files[i]))
                            {
                                File.Delete(Application.StartupPath + "\\" + UploadingExpertDir + "\\" + files[i]);
                            }
                            if (File.Exists(dr["ExpertPath"].ToString() + "\\" + dr["CompanyCode"].ToString().Trim().PadLeft(3, '0') + "\\" + files[i]))
                            {
                                File.Copy(dr["ExpertPath"].ToString() + "\\" + dr["CompanyCode"].ToString().Trim().PadLeft(3, '0') + "\\" + files[i], Application.StartupPath + "\\" + UploadingExpertDir + "\\" + files[i]);
                            }
                        }
                    }
                }
                else
                {
                    if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\" + fileName))
                    {
                        File.Delete(Application.StartupPath + "\\" + UploadingExpertDir + "\\" + fileName);
                    }
                    File.Copy(dr["ExpertPath"].ToString() + "\\" + dr["CompanyCode"].ToString().Trim().PadLeft(3, '0') + "\\" + fileName, Application.StartupPath + "\\" + UploadingExpertDir + "\\" + fileName);
                }
            }

        }

        private void CheckExpertTmpFile(DataRow dr, string fileName, string UploadingExpertDir)
        {
            if (!Directory.Exists(Application.StartupPath + "\\" + UploadingExpertDir))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\" + UploadingExpertDir);
            }
            if (!Directory.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\TMP"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\" + UploadingExpertDir + "\\TMP");
            }
            if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\TMP\\" + fileName))
            {
                File.Delete(Application.StartupPath + "\\" + UploadingExpertDir + "\\TMP\\" + fileName);
            }

            File.Copy(dr["ExpertPath"].ToString() + "\\" + dr["CompanyCode"].ToString().Trim().PadLeft(3, '0') + "\\" + fileName, Application.StartupPath + "\\" + UploadingExpertDir + "\\TMP\\" + fileName);
        }
        private DataTable GetDataFromExpert(OleDbConnection SupplyConn, String SupplyQuery)
        {
            OleDbCommand cmd = new OleDbCommand(SupplyQuery, SupplyConn);
            OleDbDataAdapter adp = new OleDbDataAdapter(cmd);
            DataTable dtData = new DataTable();
            adp.Fill(dtData);
            return dtData;
        }
        private bool CopyTableNew(OleDbConnection source, SqlConnection destination, String sourceSQL, String destinationTableName, DataTable dtSupply)
        {
            bool myReturnValue = true;
            Application.DoEvents();

            builder.Append("delete from  [" + destinationTableName + "] where ClientCompanyId = " + ClientCompanyId + "; ");
            System.Diagnostics.Debug.WriteLine(System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + " " + destinationTableName + " load started");
            SqlCommand cmd = destination.CreateCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandText = "Select Top 1 * From [" + destinationTableName.ToString() + "]";
            System.Diagnostics.Debug.WriteLine("\tSource SQL: " + sourceSQL);
            try
            {
                if (destination.State == ConnectionState.Closed)
                    destination.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable schemaTable = rdr.GetSchemaTable();
                if (destination.State == ConnectionState.Open)
                    destination.Close();
                string paramsSQL = String.Empty;
                //build the insert statement
                foreach (DataRow row in schemaTable.Rows)
                {
                    Application.DoEvents();
                    if (paramsSQL.Length > 0)
                        paramsSQL += " , ";
                    paramsSQL += "@[" + row["ColumnName"].ToString() + "]";
                }
                string myValue =
                    String.Format("Select {0} as {1} From [{2}]",
                    ClientCompanyId, paramsSQL.Replace("@", String.Empty), destinationTableName);
                if (destinationTableName.ToUpper() == "ACT")
                    dtACT = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "ADVANCE")
                    dtADVANCE = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "AGENT")
                    dtAGENT = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "BATCH")
                    dtBATCH = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "CASHCUST")
                    dtCASHCUST = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "FORMMAST")
                    dtFORMMAST = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "GROUP")
                    dtGROUP = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "LEDGER")
                    dtLEDGER = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "LEDMAST")
                    dtLEDMAST = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "ORDER")
                    dtORDER = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "ORDER2")
                    dtORDER2 = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "PGROUP")
                    dtPGROUP = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "PRODUCT")
                    dtPRODUCT = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "SALE_ADJ")
                    dtSALE_ADJ = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "SP")
                    dtSP = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "STAX")
                    dtSTAX = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "STOCK")
                    dtSTOCK = LocalConnection.GetDataTable(myValue, source);
                else if (destinationTableName.ToUpper() == "SERIAL")
                    dtSERIAL = LocalConnection.GetDataTable(myValue, source);

                myReturnValue = true;
            }
            catch (Exception ex)
            {
                //Operation.writeLog("CopyTable Outer Error : " + ex.Message, Operation.LogFile);
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
                myReturnValue = false;
            }
            finally
            {
                if (source.State == ConnectionState.Open)
                    source.Close();
                if (destination.State == ConnectionState.Open)
                    destination.Close();
            }
            return myReturnValue;
        }
        private bool CopyTable(OleDbConnection source, SqlConnection destination, String sourceSQL, String destinationTableName)
        {
            bool myReturnValue = true;
            Application.DoEvents();
            Queries.Add("delete from  " + destinationTableName + " where ClientCompanyId = " + ClientCompanyId + "");
            System.Diagnostics.Debug.WriteLine(System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + " " + destinationTableName + " load started");
            SqlCommand cmd = destination.CreateCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandText = "Select Top 1 * From " + destinationTableName.ToString();
            System.Diagnostics.Debug.WriteLine("\tSource SQL: " + sourceSQL);

            try
            {
                if (source.State == ConnectionState.Closed)

                    source.Open();
                if (destination.State == ConnectionState.Closed)
                    destination.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable schemaTable = rdr.GetSchemaTable();
                SqlCommand insertCmd = destination.CreateCommand();
                DataTable srcData = LocalConnection.GetDataTable(sourceSQL, source);
                if (source.State == ConnectionState.Open)
                    source.Close();
                if (destination.State == ConnectionState.Open)
                    destination.Close();
                if (srcData.Rows.Count == 0)
                {
                    return true;
                }
                string paramsSQL = String.Empty;
                //build the insert statement
                foreach (DataRow row in schemaTable.Rows)
                {
                    Application.DoEvents();
                    if (paramsSQL.Length > 0)
                        paramsSQL += " , ";
                    paramsSQL += "@" + row["ColumnName"].ToString();

                    IDbDataParameter param = insertCmd.CreateParameter();
                    param.ParameterName = "@" + row["ColumnName"].ToString();
                    param.SourceColumn = row["ColumnName"].ToString();

                    if (row["DataType"] == typeof(System.DateTime))
                    {
                        param.DbType = DbType.DateTime;
                    }
                    else if (row["DataType"] == typeof(System.String) || row["DataType"] == typeof(System.Byte))
                    {
                        param.DbType = DbType.String;
                    }
                    else if (row["DataType"] == typeof(System.Boolean))
                    {
                        param.DbType = DbType.Boolean;
                    }
                    else if (row["DataType"] == typeof(System.Double))
                    {
                        param.DbType = DbType.Double;
                    }
                    else if (row["DataType"] == typeof(System.Int32) || row["DataType"] == typeof(System.Int16) || row["DataType"] == typeof(System.Int64))
                    {
                        param.DbType = DbType.Int32;
                    }
                    else if (row["DataType"] == typeof(System.Decimal))
                    {
                        param.DbType = DbType.Double;
                    }
                    else
                    {
                        param.DbType = DbType.String;
                    }
                    insertCmd.Parameters.Add(param);
                }
                StringBuilder builder = new StringBuilder();
                string tableparameter = "";
                insertCmd.CommandText =
                    String.Format("insert into {0} ( ClientCompanyId , {1} ) values (" + ClientCompanyId + " , {2} )",
                    destinationTableName, paramsSQL.Replace("@", String.Empty), paramsSQL);
                tableparameter = String.Format("insert into {0} ( {1} ) values ", destinationTableName, paramsSQL.Replace("@", String.Empty));
                builder.Append(String.Format("insert into {0} ( {1} ) values ",
                    destinationTableName, paramsSQL.Replace("@", String.Empty)));
                int counter = 0;
                int errors = 0;
                string newq;
                string myQuery = paramsSQL;
                for (int i = 0; i < srcData.Rows.Count; i++)
                {
                    Application.DoEvents();
                    newq = myQuery;
                    try
                    {
                        foreach (SqlParameter param in insertCmd.Parameters)
                        {
                            Application.DoEvents();
                            object col = srcData.Rows[i][param.SourceColumn];
                            string newchar = "";
                            string oldchar = "@" + param.SourceColumn.ToString().Trim();
                            if (param.DbType == DbType.String)
                            {
                                newchar = "'" + (col.ToString().Trim().Replace("\\", "\\\\")).Replace("'", "\\'") + "'";
                                newq = newq.Replace(oldchar, newchar).ToString();
                            }
                            else if (param.DbType == DbType.Boolean)
                            {
                                newchar = (col.ToString().Trim() == "False" ? "0" : "1");
                                newq = newq.Replace(oldchar, newchar);
                            }
                            else if (param.DbType == DbType.DateTime)
                            {
                                if (col != DBNull.Value)
                                {
                                    newchar = "'" + Convert.ToDateTime(col).ToString("yyyy-MM-dd") + "'";
                                }
                                else
                                {
                                    newchar = "null";
                                }
                                newq = newq.Replace(oldchar, newchar);
                            }
                            else if (param.DbType == DbType.Double)
                            {
                                param.Value = Convert.ToDouble(col.ToString() == "" ? 0 : col);
                                newchar = param.Value.ToString().Trim();
                                newq = newq.Replace(oldchar, newchar);
                            }
                            else if (param.DbType == DbType.Int32)
                            {
                                param.Value = Convert.ToInt32(col.ToString() == "" ? 0 : col);
                                newchar = param.Value.ToString().Trim();
                                newq = newq.Replace(oldchar, newchar);
                            }
                            else if (param.DbType != DbType.AnsiString && param.DbType != DbType.String && param.DbType != DbType.Double && param.DbType != DbType.DateTime)
                            {
                                param.Value = col;
                                MessageBox.Show("Not Known DBType");
                            }
                            else
                            {
                                MessageBox.Show("ElsePart");
                            }
                        }

                        //MessageBox.Show(insertCmd.CommandText);
                        if (i == 0)
                        {
                            builder.Append(" (" + newq + ")");
                        }
                        else
                        {
                            //builder.Append(" , " + tableparameter + " (" + newq + ")");
                            builder.Append(" , (" + newq + ")");
                        }
                    }
                    catch (Exception ex)
                    {
                        Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
                        //Operation.writeLog("CopyTable Inner Error : " + ex.Message, Operation.LogFile);
                        myReturnValue = false;
                    }
                }
                Queries.Add(builder.ToString());
                System.Diagnostics.Debug.WriteLine(errors + " errors");
                System.Diagnostics.Debug.WriteLine(counter + " records copied");
                System.Diagnostics.Debug.WriteLine(System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") +
                " " + destinationTableName + " load completed");
            }
            catch (Exception ex)
            {
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
                //Operation.writeLog("CopyTable Outer Error : " + ex.Message, Operation.LogFile);
                myReturnValue = false;
            }
            finally
            {
                if (source.State == ConnectionState.Open)
                    source.Close();
                if (destination.State == ConnectionState.Open)
                    destination.Close();
            }
            return myReturnValue;
        }
        private bool CopyTableFinal(OleDbConnection source, SqlConnection destination, String sourceSQL, String destinationTableName)
        {
            bool myReturnValue = true;
            //Application.DoEvents();
            string sDBFType = "";
            try
            {
                if (!File.Exists(Application.StartupPath.ToString() + "\\" + Operation.DBFIniFile))
                {
                    string route = Application.StartupPath.ToString() + "\\" + Operation.DBFIniFile;
                    FileStream SysFile = new FileStream(@route, FileMode.Create);
                    SysFile.Close();
                }
                IniParser.IniParserMain(Application.StartupPath.ToString() + "\\" + Operation.DBFIniFile);
                sDBFType = IniParser.GetSetting(destinationTableName, destinationTableName) == null ? "" : IniParser.GetSetting(destinationTableName, destinationTableName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : While Creating Ini File....." + Environment.NewLine + "It may be issue of Permission" + Environment.NewLine + "Message : " + ex.Message);
            }
            SqlCommand insertCmd = destination.CreateCommand();
            string paramsSQL = String.Empty;
            if (string.IsNullOrEmpty(sDBFType))
            {
                SqlCommand cmd = destination.CreateCommand();
                cmd.CommandTimeout = 0;
                cmd.CommandText = "Select Top 1 * From [" + destinationTableName.ToString() + "]";
                try
                {
                    if (destination.State == ConnectionState.Closed)
                        destination.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    DataTable schemaTable = rdr.GetSchemaTable();
                    if (destination.State == ConnectionState.Open)
                        destination.Close();

                    ArrayList myDBFArray = new ArrayList();
                    //build the insert statement
                    foreach (DataRow row in schemaTable.Rows)
                    {
                        //Application.DoEvents();
                        if (paramsSQL.Length > 0)
                            paramsSQL += " , ";
                        paramsSQL += "@" + row["ColumnName"].ToString();
                        IDbDataParameter param = insertCmd.CreateParameter();
                        param.ParameterName = "@" + row["ColumnName"].ToString();
                        param.SourceColumn = row["ColumnName"].ToString();
                        myDBFArray.Add(row["ColumnName"].ToString());
                        if (row["DataType"] == typeof(System.DateTime))
                        {
                            param.DbType = DbType.DateTime;
                            myDBFArray.Add("Date");
                        }
                        else if (row["DataType"] == typeof(System.String) || row["DataType"] == typeof(System.Byte))
                        {
                            param.DbType = DbType.String;
                            myDBFArray.Add("String");
                        }
                        else if (row["DataType"] == typeof(System.Boolean))
                        {
                            param.DbType = DbType.Boolean;
                            myDBFArray.Add("Boolean");
                        }
                        else if (row["DataType"] == typeof(System.Double) || row["DataType"] == typeof(System.Decimal))
                        {
                            param.DbType = DbType.Double;
                            myDBFArray.Add("Double");
                        }
                        else if (row["DataType"] == typeof(System.Int32) || row["DataType"] == typeof(System.Int16) || row["DataType"] == typeof(System.Int64))
                        {
                            param.DbType = DbType.Int32;
                            myDBFArray.Add("Int32");
                        }
                        else
                        {
                            param.DbType = DbType.String;
                            myDBFArray.Add("String");
                        }
                        insertCmd.Parameters.Add(param);
                    }
                    //MessageBox.Show(paramsSQL);
                    string xdbfstring = "";
                    for (int i = 0; i < myDBFArray.Count; i++)
                    {
                        xdbfstring += myDBFArray[i].ToString() + "#";
                        i++;
                        xdbfstring += myDBFArray[i].ToString() + "#";
                    }
                    IniParser.AddSetting(destinationTableName, destinationTableName, xdbfstring);
                    IniParser.SaveSettings();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else // if(ini file is not empty)
            {
                string[] mydbf = sDBFType.Split('#');
                for (int i = 0; i < mydbf.Length - 1; i++)
                {
                    if (paramsSQL.Length > 0)
                        paramsSQL += " , ";
                    paramsSQL += "@" + mydbf[i].ToString();
                    IDbDataParameter param = insertCmd.CreateParameter();
                    param.ParameterName = "@" + mydbf[i].ToString();
                    param.SourceColumn = mydbf[i].ToString();
                    i++;
                    if (mydbf[i].ToString().ToUpper() == "DATE")
                    {
                        param.DbType = DbType.DateTime;
                    }
                    else if (mydbf[i].ToString().ToUpper() == "STRING")
                    {
                        param.DbType = DbType.String;
                    }
                    else if (mydbf[i].ToString().ToUpper() == "BOOLEAN")
                    {
                        param.DbType = DbType.Boolean;
                    }
                    else if (mydbf[i].ToString().ToUpper() == "DOUBLE")
                    {
                        param.DbType = DbType.Double;
                    }
                    else if (mydbf[i].ToString().ToUpper() == "INT32")
                    {
                        param.DbType = DbType.Int32;
                    }
                    else
                    {
                        param.DbType = DbType.String;
                    }
                    insertCmd.Parameters.Add(param);
                }
            }
            Queries.Add("delete from  [" + destinationTableName + "] where ClientCompanyId = " + ClientCompanyId + "");
            System.Diagnostics.Debug.WriteLine(System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + " " + destinationTableName + " load started");
            try
            {
                if (source.State == ConnectionState.Closed)
                    source.Open();
                DataTable srcData = LocalConnection.GetDataTable(sourceSQL, source);
                if (source.State == ConnectionState.Open)
                    source.Close();
                if (srcData.Rows.Count == 0)
                {
                    return true;
                }
                //string paramsSQL = String.Empty;
                //build the insert statement

                StringBuilder builder = new StringBuilder();
                string tableparameter = "";
                insertCmd.CommandText =
                    String.Format("insert into [{0}] ( ClientCompanyId , {1} ) values (" + ClientCompanyId + " , {2} )",
                    destinationTableName, paramsSQL.Replace("@", String.Empty), paramsSQL);
                tableparameter = String.Format("insert into [{0}] ( {1} ) values ", destinationTableName, paramsSQL.Replace("@", String.Empty));
                builder.Append(String.Format("insert into [{0}] ( {1} ) values ",
                    destinationTableName, paramsSQL.Replace("@", String.Empty)));
                int counter = 0;
                int errors = 0;
                string newq;
                string myQuery = paramsSQL.ToUpper();
                for (int i = 0; i < srcData.Rows.Count; i++)
                {
                    // Application.DoEvents();
                    newq = myQuery;
                    try
                    {
                        foreach (SqlParameter param in insertCmd.Parameters)
                        {
                            Application.DoEvents();
                            object col = srcData.Rows[i][param.SourceColumn];
                            string newchar = "";
                            string oldchar = "@" + param.SourceColumn.ToString().Trim().ToUpper();
                            if (param.DbType == DbType.String)
                            {
                                newchar = "'" + (col.ToString().Trim().Replace("\\", "\\\\")).Replace("'", "\\'") + "'";
                                newq = newq.Replace(oldchar, newchar).ToString();
                            }
                            else if (param.DbType == DbType.Boolean)
                            {
                                newchar = (col.ToString().Trim() == "False" ? "0" : "1");
                                newq = newq.Replace(oldchar, newchar);
                            }
                            else if (param.DbType == DbType.DateTime)
                            {
                                if (col != DBNull.Value)
                                {
                                    newchar = "'" + Convert.ToDateTime(col).ToString("yyyy-MM-dd") + "'";
                                }
                                else
                                {
                                    newchar = "null";
                                }
                                newq = newq.Replace(oldchar, newchar);
                            }
                            else if (param.DbType == DbType.Double)
                            {
                                param.Value = Convert.ToDouble(col.ToString() == "" ? 0 : col);
                                newchar = param.Value.ToString().Trim();
                                newq = newq.Replace(oldchar, newchar);
                            }
                            else if (param.DbType == DbType.Int32)
                            {
                                param.Value = Convert.ToInt32(col.ToString() == "" ? 0 : col);
                                newchar = param.Value.ToString().Trim();
                                newq = newq.Replace(oldchar, newchar);
                            }
                            else if (param.DbType != DbType.AnsiString && param.DbType != DbType.String && param.DbType != DbType.Double && param.DbType != DbType.DateTime)
                            {
                                param.Value = col;
                                MessageBox.Show("Not Known DBType");
                            }
                            else
                            {
                                MessageBox.Show("ElsePart");
                            }
                        }

                        //MessageBox.Show(insertCmd.CommandText);
                        if (i == 0)
                        {
                            builder.Append(" (" + newq + ")");
                        }
                        else
                        {
                            //builder.Append(" , " + tableparameter + " (" + newq + ")");
                            builder.Append(" , (" + newq + ")");
                        }
                    }
                    catch (Exception ex)
                    {
                        Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
                        //Operation.writeLog("CopyTable Inner Error : " + ex.Message, Operation.LogFile);
                        myReturnValue = false;
                    }
                }
                Queries.Add(builder.ToString());
                System.Diagnostics.Debug.WriteLine(errors + " errors");
                System.Diagnostics.Debug.WriteLine(counter + " records copied");
                System.Diagnostics.Debug.WriteLine(System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") +
                " " + destinationTableName + " load completed");
            }
            catch (Exception ex)
            {
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
                //Operation.writeLog("CopyTable Outer Error : " + ex.Message, Operation.LogFile);
                myReturnValue = false;
            }
            finally
            {
                if (source.State == ConnectionState.Open)
                    source.Close();
                if (destination.State == ConnectionState.Open)
                    destination.Close();
            }
            return myReturnValue;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Application.DoEvents();
            Show();
            this.WindowState = FormWindowState.Maximized;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;

        }

        private void frmMDI_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(100);
                this.Hide();
            }
        }

        private void tBackGround_Tick(object sender, EventArgs e)
        {
            tBackGround.Enabled = false;
            Application.DoEvents();

            Operation.IsInternetExits = Operation.IsInternetOnorOff();
            tBackGround.Enabled = true;
        }

        private void masterToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void uploadDataNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want Upload Data Now?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Operation.PromptBeforeData == true)
                {
                    AutoClosingMessageBox.Show("Gentle Reminder......" + Environment.NewLine + Environment.NewLine + "Your Expert Data will be uploaded on " + (DateTime.Now.AddMinutes(Operation.PromptMins)).ToString("hh:mm tt") + "." + Environment.NewLine + "Please Close All Expert,If You Are Using in More Than One Computer.", "Upload Information", 10000);
                }
                Application.DoEvents();
                Thread MyThread = new Thread(new ThreadStart(DataGathering));
                MyThread.Start();
            }
            else
            {
                return;
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public bool BulkInsertDataTable(string tableName, DataTable dataTable)
        {
            bool isSuccuss;
            try
            {
                SqlConnection myConn = new SqlConnection(Operation.ConnStr);
                if (myConn.State == ConnectionState.Closed)
                    myConn.Open();
                //SqlConnection SqlConnectionObj = GetSQLConnection();
                SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.WriteToServer(dataTable);

                if (myConn.State == ConnectionState.Open)
                    myConn.Close();
                isSuccuss = true;
            }
            catch (Exception ex)
            {
                isSuccuss = false;
            }
            return isSuccuss;
        }
        private void CompareTwoDataTable(DataTable dtLocal, DataTable dtExpert, TableNames tName)
        {
            try
            {
                dtModifiedRecord = new DataTable();
                dtUnModifiedRecord = new DataTable();
                dtNewlyInsertInLocal = new DataTable();
                dtDeleteFromLocal = new DataTable();
                switch (tName)
                {
                    #region OLDACT
                    //case TableNames.ACT:
                    //    var ids = dtLocal.AsEnumerable().Select(x => new { Code = x.Field<string>("Code"), Name = x.Field<string>("Name") }).ToList();
                    //    var changed = (from table1 in dtLocal.AsEnumerable()
                    //                   join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Code") equals table2.Field<string>("Code")
                    //                   where (table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("Group") != table2.Field<string>("Group") || table1.Field<double?>("Bal_Op") != table2.Field<double?>("Bal_Op") || table1.Field<string>("Bal_Dc") != table2.Field<string>("Bal_Dc") || table1.Field<double?>("OpBal") != table2.Field<double?>("OpBal") || table1.Field<double?>("TotDr") != table2.Field<double?>("TotDr") || table1.Field<double?>("TotCr") != table2.Field<double?>("TotCr") || table1.Field<double?>("ClBal") != table2.Field<double?>("ClBal") || table1.Field<string>("Add1") != table2.Field<string>("Add1") || table1.Field<string>("Add2") != table2.Field<string>("Add2") || table1.Field<string>("Add3") != table2.Field<string>("Add3") || table1.Field<string>("StNo") != table2.Field<string>("StNo") || table1.Field<string>("Cstno") != table2.Field<string>("Cstno") || table1.Field<string>("Phone") != table2.Field<string>("Phone") || table1.Field<string>("Mobile") != table2.Field<string>("Mobile") || table1.Field<string>("Email") != table2.Field<string>("Email") || table1.Field<string>("Zone") != table2.Field<string>("Zone") || table1.Field<string>("Category") != table2.Field<string>("Category") || table1.Field<string>("State") != table2.Field<string>("State") || table1.Field<string>("ItNo") != table2.Field<string>("ItNo") || table1.Field<string>("LicNo") != table2.Field<string>("LicNo") || table1.Field<string>("VatNo") != table2.Field<string>("VatNo") || table1.Field<string>("FaxNo") != table2.Field<string>("FaxNo") || table1.Field<double?>("Cr_Days") != table2.Field<double?>("Cr_Days") || table1.Field<double?>("Act_Type") != table2.Field<double?>("Act_Type") || table1.Field<string>("BSGroup") != table2.Field<string>("BSGroup") || table1.Field<string>("Remarks") != table2.Field<string>("Remarks"))
                    //                   //&& !(table2.Field<string>("Code").Contains(id))
                    //                   select table1);

                    //    if (changed != null && changed.Count() > 0)
                    //    {
                    //        dtModifiedRecord = changed.CopyToDataTable();
                    //    }

                    //    var unchanged = (from table2 in dtExpert.AsEnumerable()
                    //                     join table1 in dtLocal.AsEnumerable() on table2.Field<string>("Code") equals table1.Field<string>("Code")
                    //                     where table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("Group") != table2.Field<string>("Group") || table1.Field<double?>("Bal_Op") != table2.Field<double?>("Bal_Op") || table1.Field<string>("Bal_Dc") != table2.Field<string>("Bal_Dc") || table1.Field<double?>("OpBal") != table2.Field<double?>("OpBal") || table1.Field<double?>("TotDr") != table2.Field<double?>("TotDr") || table1.Field<double?>("TotCr") != table2.Field<double?>("TotCr") || table1.Field<double?>("ClBal") != table2.Field<double?>("ClBal") || table1.Field<string>("Add1") != table2.Field<string>("Add1") || table1.Field<string>("Add2") != table2.Field<string>("Add2") || table1.Field<string>("Add3") != table2.Field<string>("Add3") || table1.Field<string>("StNo") != table2.Field<string>("StNo") || table1.Field<string>("Cstno") != table2.Field<string>("Cstno") || table1.Field<string>("Phone") != table2.Field<string>("Phone") || table1.Field<string>("Mobile") != table2.Field<string>("Mobile") || table1.Field<string>("Email") != table2.Field<string>("Email") || table1.Field<string>("Zone") != table2.Field<string>("Zone") || table1.Field<string>("Category") != table2.Field<string>("Category") || table1.Field<string>("State") != table2.Field<string>("State") || table1.Field<string>("ItNo") != table2.Field<string>("ItNo") || table1.Field<string>("LicNo") != table2.Field<string>("LicNo") || table1.Field<string>("VatNo") != table2.Field<string>("VatNo") || table1.Field<string>("FaxNo") != table2.Field<string>("FaxNo") || table1.Field<double?>("Cr_Days") != table2.Field<double?>("Cr_Days") || table1.Field<double?>("Act_Type") != table2.Field<double?>("Act_Type") || table1.Field<string>("BSGroup") != table2.Field<string>("BSGroup") || table1.Field<string>("Remarks") != table2.Field<string>("Remarks")
                    //                     select table2);

                    //    if (unchanged != null && unchanged.Count() > 0)
                    //        dtUnModifiedRecord = unchanged.CopyToDataTable();


                    //    var newinsertinlocal = (from table1 in dtLocal.AsEnumerable()
                    //                            let id = table1.Field<string>("Code")
                    //                            join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Code") equals table2.Field<string>("Code")
                    //                            where (table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("Group") != table2.Field<string>("Group") || table1.Field<double?>("Bal_Op") != table2.Field<double?>("Bal_Op") || table1.Field<string>("Bal_Dc") != table2.Field<string>("Bal_Dc") || table1.Field<double?>("OpBal") != table2.Field<double?>("OpBal") || table1.Field<double?>("TotDr") != table2.Field<double?>("TotDr") || table1.Field<double?>("TotCr") != table2.Field<double?>("TotCr") || table1.Field<double?>("ClBal") != table2.Field<double?>("ClBal") || table1.Field<string>("Add1") != table2.Field<string>("Add1") || table1.Field<string>("Add2") != table2.Field<string>("Add2") || table1.Field<string>("Add3") != table2.Field<string>("Add3") || table1.Field<string>("StNo") != table2.Field<string>("StNo") || table1.Field<string>("Cstno") != table2.Field<string>("Cstno") || table1.Field<string>("Phone") != table2.Field<string>("Phone") || table1.Field<string>("Mobile") != table2.Field<string>("Mobile") || table1.Field<string>("Email") != table2.Field<string>("Email") || table1.Field<string>("Zone") != table2.Field<string>("Zone") || table1.Field<string>("Category") != table2.Field<string>("Category") || table1.Field<string>("State") != table2.Field<string>("State") || table1.Field<string>("ItNo") != table2.Field<string>("ItNo") || table1.Field<string>("LicNo") != table2.Field<string>("LicNo") || table1.Field<string>("VatNo") != table2.Field<string>("VatNo") || table1.Field<string>("FaxNo") != table2.Field<string>("FaxNo") || table1.Field<double?>("Cr_Days") != table2.Field<double?>("Cr_Days") || table1.Field<double?>("Act_Type") != table2.Field<double?>("Act_Type") || table1.Field<string>("BSGroup") != table2.Field<string>("BSGroup") || table1.Field<string>("Remarks") != table2.Field<string>("Remarks"))
                    //                            && (table2.Field<string>("Code").Contains(id))
                    //                            select table2);
                    //    if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                    //    {
                    //        dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();
                    //    }

                    //    var newinsertinlocalold = from t2 in dtExpert.AsEnumerable()
                    //                           join t1 in dtLocal.AsEnumerable()
                    //                           on t2.Field<string>("Code") equals t1.Field<string>("Code") into tg
                    //                           from tcheck in tg.DefaultIfEmpty()
                    //                           where tcheck == null
                    //                           select t2;
                    //    if (newinsertinlocalold != null && newinsertinlocalold.Count() > 0)
                    //    {
                    //        DataTable dtNewlyInsertInLocalOld = newinsertinlocalold.CopyToDataTable();
                    //    }
                    //    var deletefromlocal = (from table1 in dtLocal.AsEnumerable()
                    //                           let id = table1.Field<string>("Code")
                    //                           join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Code") equals table2.Field<string>("Code")
                    //                           where (table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("Group") != table2.Field<string>("Group") || table1.Field<double?>("Bal_Op") != table2.Field<double?>("Bal_Op") || table1.Field<string>("Bal_Dc") != table2.Field<string>("Bal_Dc") || table1.Field<double?>("OpBal") != table2.Field<double?>("OpBal") || table1.Field<double?>("TotDr") != table2.Field<double?>("TotDr") || table1.Field<double?>("TotCr") != table2.Field<double?>("TotCr") || table1.Field<double?>("ClBal") != table2.Field<double?>("ClBal") || table1.Field<string>("Add1") != table2.Field<string>("Add1") || table1.Field<string>("Add2") != table2.Field<string>("Add2") || table1.Field<string>("Add3") != table2.Field<string>("Add3") || table1.Field<string>("StNo") != table2.Field<string>("StNo") || table1.Field<string>("Cstno") != table2.Field<string>("Cstno") || table1.Field<string>("Phone") != table2.Field<string>("Phone") || table1.Field<string>("Mobile") != table2.Field<string>("Mobile") || table1.Field<string>("Email") != table2.Field<string>("Email") || table1.Field<string>("Zone") != table2.Field<string>("Zone") || table1.Field<string>("Category") != table2.Field<string>("Category") || table1.Field<string>("State") != table2.Field<string>("State") || table1.Field<string>("ItNo") != table2.Field<string>("ItNo") || table1.Field<string>("LicNo") != table2.Field<string>("LicNo") || table1.Field<string>("VatNo") != table2.Field<string>("VatNo") || table1.Field<string>("FaxNo") != table2.Field<string>("FaxNo") || table1.Field<double?>("Cr_Days") != table2.Field<double?>("Cr_Days") || table1.Field<double?>("Act_Type") != table2.Field<double?>("Act_Type") || table1.Field<string>("BSGroup") != table2.Field<string>("BSGroup") || table1.Field<string>("Remarks") != table2.Field<string>("Remarks"))
                    //                           && (table2.Field<string>("Code").Contains(id))
                    //                           select table1);
                    //    //var deletefromlocal = from t1 in dtLocal.AsEnumerable()
                    //    //                      join t2 in dtExpert.AsEnumerable()
                    //    //                      on t1.Field<string>("Code") equals t2.Field<string>("Code") into tg
                    //    //                      from tcheck in tg.DefaultIfEmpty()
                    //    //                      where tcheck == null
                    //    //                      select t1;
                    //    if (deletefromlocal != null && deletefromlocal.Count() > 0)
                    //        dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                    //    break;
                    #endregion
                    #region ACT
                    case TableNames.ACT:
                        var changed = (from table1 in dtLocal.AsEnumerable()
                                       join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Code") equals table2.Field<string>("Code")
                                       where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("Group") != table2.Field<string>("Group") || table1.Field<double?>("Bal_Op") != table2.Field<double?>("Bal_Op") || table1.Field<string>("Bal_Dc") != table2.Field<string>("Bal_Dc") || table1.Field<double?>("OpBal") != table2.Field<double?>("OpBal") || table1.Field<double?>("TotDr") != table2.Field<double?>("TotDr") || table1.Field<double?>("TotCr") != table2.Field<double?>("TotCr") || table1.Field<double?>("ClBal") != table2.Field<double?>("ClBal") || table1.Field<string>("Add1") != table2.Field<string>("Add1") || table1.Field<string>("Add2") != table2.Field<string>("Add2") || table1.Field<string>("Add3") != table2.Field<string>("Add3") || table1.Field<string>("StNo") != table2.Field<string>("StNo") || table1.Field<string>("Cstno") != table2.Field<string>("Cstno") || table1.Field<string>("Phone") != table2.Field<string>("Phone") || table1.Field<string>("Mobile") != table2.Field<string>("Mobile") || table1.Field<string>("Email") != table2.Field<string>("Email") || table1.Field<string>("Zone") != table2.Field<string>("Zone") || table1.Field<string>("Category") != table2.Field<string>("Category") || table1.Field<string>("State") != table2.Field<string>("State") || table1.Field<string>("ItNo") != table2.Field<string>("ItNo") || table1.Field<string>("LicNo") != table2.Field<string>("LicNo") || table1.Field<string>("VatNo") != table2.Field<string>("VatNo") || table1.Field<string>("FaxNo") != table2.Field<string>("FaxNo") || table1.Field<double?>("Cr_Days") != table2.Field<double?>("Cr_Days") || table1.Field<double?>("Act_Type") != table2.Field<double?>("Act_Type") || table1.Field<string>("BSGroup") != table2.Field<string>("BSGroup") || table1.Field<string>("Remarks") != table2.Field<string>("Remarks")
                                       select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        var unchanged = (from table2 in dtExpert.AsEnumerable()
                                         join table1 in dtLocal.AsEnumerable() on table2.Field<string>("Code") equals table1.Field<string>("Code")
                                         where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("Group") != table2.Field<string>("Group") || table1.Field<double?>("Bal_Op") != table2.Field<double?>("Bal_Op") || table1.Field<string>("Bal_Dc") != table2.Field<string>("Bal_Dc") || table1.Field<double?>("OpBal") != table2.Field<double?>("OpBal") || table1.Field<double?>("TotDr") != table2.Field<double?>("TotDr") || table1.Field<double?>("TotCr") != table2.Field<double?>("TotCr") || table1.Field<double?>("ClBal") != table2.Field<double?>("ClBal") || table1.Field<string>("Add1") != table2.Field<string>("Add1") || table1.Field<string>("Add2") != table2.Field<string>("Add2") || table1.Field<string>("Add3") != table2.Field<string>("Add3") || table1.Field<string>("StNo") != table2.Field<string>("StNo") || table1.Field<string>("Cstno") != table2.Field<string>("Cstno") || table1.Field<string>("Phone") != table2.Field<string>("Phone") || table1.Field<string>("Mobile") != table2.Field<string>("Mobile") || table1.Field<string>("Email") != table2.Field<string>("Email") || table1.Field<string>("Zone") != table2.Field<string>("Zone") || table1.Field<string>("Category") != table2.Field<string>("Category") || table1.Field<string>("State") != table2.Field<string>("State") || table1.Field<string>("ItNo") != table2.Field<string>("ItNo") || table1.Field<string>("LicNo") != table2.Field<string>("LicNo") || table1.Field<string>("VatNo") != table2.Field<string>("VatNo") || table1.Field<string>("FaxNo") != table2.Field<string>("FaxNo") || table1.Field<double?>("Cr_Days") != table2.Field<double?>("Cr_Days") || table1.Field<double?>("Act_Type") != table2.Field<double?>("Act_Type") || table1.Field<string>("BSGroup") != table2.Field<string>("BSGroup") || table1.Field<string>("Remarks") != table2.Field<string>("Remarks")
                                         select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        var newinsertinlocal = from t2 in dtExpert.AsEnumerable()
                                               join t1 in dtLocal.AsEnumerable()
                                               on t2.Field<string>("Code") equals t1.Field<string>("Code") into tg
                                               from tcheck in tg.DefaultIfEmpty()
                                               where tcheck == null
                                               select t2;
                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        var deletefromlocal = from t1 in dtLocal.AsEnumerable()
                                              join t2 in dtExpert.AsEnumerable()
                                              on t1.Field<string>("Code") equals t2.Field<string>("Code") into tg
                                              from tcheck in tg.DefaultIfEmpty()
                                              where tcheck == null
                                              select t1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion
                    #region ADVANCE
                    case TableNames.ADVANCE:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable()
                                        on new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Code"), ColC = table1.Field<string>("DocNo"), ColD = table1.Field<DateTime>("Date"), ColE = table1.Field<double?>("Amt"), ColF = table1.Field<string>("Division"), ColG = table1.Field<string>("DC") } equals new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Code"), ColC = table2.Field<string>("DocNo"), ColD = table2.Field<DateTime>("Date"), ColE = table2.Field<double?>("Amt"), ColF = table2.Field<string>("Division"), ColG = table2.Field<string>("DC") }
                                   where table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("DocNo") != table2.Field<string>("DocNo") || table1.Field<DateTime>("Date") != table2.Field<DateTime>("Date") || table1.Field<double?>("Amt") != table2.Field<double?>("Amt") || table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<string>("DC") != table1.Field<string>("DC")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable()
                                          on new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Code"), ColC = table2.Field<string>("DocNo"), ColD = table2.Field<DateTime>("Date"), ColE = table2.Field<double?>("Amt"), ColF = table2.Field<string>("Division"), ColG = table2.Field<string>("DC") } equals new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Code"), ColC = table1.Field<string>("DocNo"), ColD = table1.Field<DateTime>("Date"), ColE = table1.Field<double?>("Amt"), ColF = table1.Field<string>("Division"), ColG = table1.Field<string>("DC") }
                                     where table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("DocNo") != table2.Field<string>("DocNo") || table1.Field<DateTime>("Date") != table2.Field<DateTime>("Date") || table1.Field<double?>("Amt") != table2.Field<double?>("Amt") || table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<string>("DC") != table1.Field<string>("DC")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Code"), ColC = table2.Field<string>("DocNo"), ColD = table2.Field<DateTime>("Date"), ColE = table2.Field<double?>("Amt"), ColF = table2.Field<string>("Division"), ColG = table2.Field<string>("DC") } equals new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Code"), ColC = table1.Field<string>("DocNo"), ColD = table1.Field<DateTime>("Date"), ColE = table1.Field<double?>("Amt"), ColF = table1.Field<string>("Division"), ColG = table1.Field<string>("DC") } into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Code"), ColC = table1.Field<string>("DocNo"), ColD = table1.Field<DateTime>("Date"), ColE = table1.Field<double?>("Amt"), ColF = table1.Field<string>("Division"), ColG = table1.Field<string>("DC") } equals new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Code"), ColC = table2.Field<string>("DocNo"), ColD = table2.Field<DateTime>("Date"), ColE = table2.Field<double?>("Amt"), ColF = table2.Field<string>("Division"), ColG = table2.Field<string>("DC") } into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region AGENT
                    case TableNames.AGENT:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Code") equals table2.Field<string>("Code")
                                   where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<double?>("CM_PCN") != table2.Field<double?>("CM_PCN") || table1.Field<double?>("CM_ON") != table2.Field<double?>("CM_ON") || table1.Field<string>("Category1") != table2.Field<string>("Category1") || table1.Field<string>("Category2") != table2.Field<string>("Category2") || table1.Field<string>("Category3") != table2.Field<string>("Category3")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable() on table2.Field<string>("Code") equals table1.Field<string>("Code")
                                     where table2.Field<string>("Code") != table1.Field<string>("Code") || table2.Field<string>("Name") != table1.Field<string>("Name") || table2.Field<double?>("CM_PCN") != table1.Field<double?>("CM_PCN") || table2.Field<double?>("CM_ON") != table1.Field<double?>("CM_ON") || table2.Field<string>("Category1") != table1.Field<string>("Category1") || table2.Field<string>("Category2") != table1.Field<string>("Category2") || table2.Field<string>("Category3") != table1.Field<string>("Category3")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on table2.Field<string>("Code") equals table1.Field<string>("Code") into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on table1.Field<string>("Code") equals table2.Field<string>("Code") into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region BATCH
                    case TableNames.BATCH:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable()
                                   on new { ColA = table1.Field<string>("Code"), ColB = table1.Field<string>("BatchNo") } equals new { ColA = table2.Field<string>("Code"), ColB = table2.Field<string>("BatchNo") }
                                   where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("BatchNo") != table2.Field<string>("BatchNo") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<double?>("Op_Qty") != table2.Field<double?>("Op_Qty") || table1.Field<double?>("TotIn") != table2.Field<double?>("TotIn") || table1.Field<double?>("TotOut") != table2.Field<double?>("TotOut") || table1.Field<double?>("Cl_Qty") != table2.Field<double?>("Cl_Qty") || table1.Field<double?>("Op_Value") != table2.Field<double?>("Op_Value") || table1.Field<double?>("Op_Rate") != table2.Field<double?>("Op_Rate") || table1.Field<double?>("Sl_Rate") != table2.Field<double?>("Sl_Rate") || table1.Field<double?>("Pu_Rate") != table2.Field<double?>("Pu_Rate")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable()
                                     on new { ColA = table2.Field<string>("Code"), ColB = table2.Field<string>("BatchNo") } equals new { ColA = table1.Field<string>("Code"), ColB = table1.Field<string>("BatchNo") }
                                     where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("BatchNo") != table2.Field<string>("BatchNo") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<double?>("Op_Qty") != table2.Field<double?>("Op_Qty") || table1.Field<double?>("TotIn") != table2.Field<double?>("TotIn") || table1.Field<double?>("TotOut") != table2.Field<double?>("TotOut") || table1.Field<double?>("Cl_Qty") != table2.Field<double?>("Cl_Qty") || table1.Field<double?>("Op_Value") != table2.Field<double?>("Op_Value") || table1.Field<double?>("Op_Rate") != table2.Field<double?>("Op_Rate") || table1.Field<double?>("Sl_Rate") != table2.Field<double?>("Sl_Rate") || table1.Field<double?>("Pu_Rate") != table2.Field<double?>("Pu_Rate")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on new { ColA = table2.Field<string>("Code"), ColB = table2.Field<string>("BatchNo") } equals new { ColA = table1.Field<string>("Code"), ColB = table1.Field<string>("BatchNo") } into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;
                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on new { ColA = table1.Field<string>("Code"), ColB = table1.Field<string>("BatchNo") } equals new { ColA = table2.Field<string>("Code"), ColB = table2.Field<string>("BatchNo") } into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region CASHCUST
                    case TableNames.CASHCUST:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Code") equals table2.Field<string>("Code")
                                   where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("Add1") != table2.Field<string>("Add1") || table1.Field<string>("Add2") != table2.Field<string>("Add2") || table1.Field<string>("Add3") != table2.Field<string>("Add3") || table1.Field<string>("CSTNo") != table2.Field<string>("CSTNo") || table1.Field<string>("STNo") != table2.Field<string>("STNo") || table1.Field<string>("Phone") != table2.Field<string>("Phone") || table1.Field<string>("Mobile") != table2.Field<string>("Mobile") || table1.Field<string>("Zone") != table2.Field<string>("Zone") || table1.Field<string>("Category") != table2.Field<string>("Category") || table1.Field<string>("ITNo") != table2.Field<string>("ITNo") || table1.Field<string>("LicNo") != table2.Field<string>("LicNo") || table1.Field<string>("FaxNo") != table2.Field<string>("FaxNo") || table1.Field<string>("Email") != table2.Field<string>("Email") || table1.Field<string>("Remarks") != table2.Field<string>("Remarks")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable() on table2.Field<string>("Code") equals table1.Field<string>("Code")
                                     where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("Add1") != table2.Field<string>("Add1") || table1.Field<string>("Add2") != table2.Field<string>("Add2") || table1.Field<string>("Add3") != table2.Field<string>("Add3") || table1.Field<string>("CSTNo") != table2.Field<string>("CSTNo") || table1.Field<string>("STNo") != table2.Field<string>("STNo") || table1.Field<string>("Phone") != table2.Field<string>("Phone") || table1.Field<string>("Mobile") != table2.Field<string>("Mobile") || table1.Field<string>("Zone") != table2.Field<string>("Zone") || table1.Field<string>("Category") != table2.Field<string>("Category") || table1.Field<string>("ITNo") != table2.Field<string>("ITNo") || table1.Field<string>("LicNo") != table2.Field<string>("LicNo") || table1.Field<string>("FaxNo") != table2.Field<string>("FaxNo") || table1.Field<string>("Email") != table2.Field<string>("Email") || table1.Field<string>("Remarks") != table2.Field<string>("Remarks")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on table2.Field<string>("Code") equals table1.Field<string>("Code") into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on table1.Field<string>("Code") equals table2.Field<string>("Code") into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region FORMMAST
                    case TableNames.FORMMAST:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Code") equals table2.Field<string>("Code")
                                   where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable() on table2.Field<string>("Code") equals table1.Field<string>("Code")
                                     where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on table2.Field<string>("Code") equals table1.Field<string>("Code") into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on table1.Field<string>("Code") equals table2.Field<string>("Code") into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region GROUP
                    case TableNames.GROUP:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Code") equals table2.Field<string>("Code")
                                   where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("OrigName") != table2.Field<string>("OrigName") || table1.Field<string>("ALIE") != table2.Field<string>("ALIE") || table1.Field<string>("Order") != table2.Field<string>("Order") || table1.Field<Boolean>("Schedule") != table2.Field<Boolean>("Schedule") || table1.Field<double?>("Type") != table2.Field<double?>("Type") || table1.Field<string>("Flag") != table2.Field<string>("Flag") || table1.Field<string>("Under") != table2.Field<string>("Under") || table1.Field<string>("Parent") != table2.Field<string>("Parent")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable() on table2.Field<string>("Code") equals table1.Field<string>("Code")
                                     where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("OrigName") != table2.Field<string>("OrigName") || table1.Field<string>("ALIE") != table2.Field<string>("ALIE") || table1.Field<string>("Order") != table2.Field<string>("Order") || table1.Field<Boolean>("Schedule") != table2.Field<Boolean>("Schedule") || table1.Field<double?>("Type") != table2.Field<double?>("Type") || table1.Field<string>("Flag") != table2.Field<string>("Flag") || table1.Field<string>("Under") != table2.Field<string>("Under") || table1.Field<string>("Parent") != table2.Field<string>("Parent")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on table2.Field<string>("Code") equals table1.Field<string>("Code") into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on table1.Field<string>("Code") equals table2.Field<string>("Code") into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region ITVAT
                    case TableNames.ITVAT:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable()
                                        on new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("VatCode"), ColC = table1.Field<double?>("Amt"), ColD = table1.Field<double?>("Vat") } equals new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("VatCode"), ColC = table2.Field<double?>("Amt"), ColD = table2.Field<double?>("Vat") }
                                   where table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("VatCode") != table2.Field<string>("VatCode") || table1.Field<double?>("Amt") != table2.Field<double?>("Amt") || table1.Field<double?>("Vat") != table2.Field<double?>("Vat")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable()
                                        on new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("VatCode"), ColC = table2.Field<double?>("Amt"), ColD = table2.Field<double?>("Vat") } equals new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("VatCode"), ColC = table1.Field<double?>("Amt"), ColD = table1.Field<double?>("Vat") }
                                     where table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("VatCode") != table2.Field<string>("VatCode") || table1.Field<double?>("Amt") != table2.Field<double?>("Amt") || table1.Field<double?>("Vat") != table2.Field<double?>("Vat")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("VatCode"), ColC = table2.Field<double?>("Amt"), ColD = table2.Field<double?>("Vat") } equals new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("VatCode"), ColC = table1.Field<double?>("Amt"), ColD = table1.Field<double?>("Vat") } into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("VatCode"), ColC = table1.Field<double?>("Amt"), ColD = table1.Field<double?>("Vat") } equals new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("VatCode"), ColC = table2.Field<double?>("Amt"), ColD = table2.Field<double?>("Vat") } into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region LEDGER
                    case TableNames.LEDGER:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable()
                                        on new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<double?>("Amt"), ColG = table1.Field<string>("DC"), ColH = table1.Field<string>("Vtype"), ColI = table1.Field<string>("RCode"), ColJ = table1.Field<string>("Narr1"), ColK = table1.Field<string>("Narr2"), ColL = table1.Field<string>("Narr3"), ColM = table1.Field<string>("Narr4"), ColN = table1.Field<string>("Contra"), ColO = table1.Field<string>("Adj_Type"), ColP = table1.Field<double?>("Adj_Amt"), ColQ = table1.Field<string>("ChNo"), ColS = table1.Field<string>("ChBank"), ColU = table1.Field<string>("VatCode") } equals new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<double?>("Amt"), ColG = table2.Field<string>("DC"), ColH = table2.Field<string>("Vtype"), ColI = table2.Field<string>("RCode"), ColJ = table2.Field<string>("Narr1"), ColK = table2.Field<string>("Narr2"), ColL = table2.Field<string>("Narr3"), ColM = table2.Field<string>("Narr4"), ColN = table2.Field<string>("Contra"), ColO = table2.Field<string>("Adj_Type"), ColP = table2.Field<double?>("Adj_Amt"), ColQ = table2.Field<string>("ChNo"), ColS = table2.Field<string>("ChBank"), ColU = table2.Field<string>("VatCode") }
                                   where table1.Field<string>("Link") != table1.Field<string>("Link") || table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<DateTime>("Date") != table2.Field<DateTime>("Date") || table1.Field<string>("DocNo") != table2.Field<string>("DocNo") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<double?>("Amt") != table2.Field<double?>("Amt") || table1.Field<string>("DC") != table2.Field<string>("DC") || table1.Field<string>("Vtype") != table2.Field<string>("Vtype") || table1.Field<string>("RCode") != table2.Field<string>("RCode") || table1.Field<string>("Narr1") != table2.Field<string>("Narr1") || table1.Field<string>("Narr2") != table2.Field<string>("Narr2") || table1.Field<string>("Narr3") != table2.Field<string>("Narr3") || table1.Field<string>("Narr4") != table2.Field<string>("Narr4") || table1.Field<string>("Contra") != table2.Field<string>("Contra") || table1.Field<string>("Adj_Type") != table2.Field<string>("Adj_Type") || table1.Field<double?>("Adj_Amt") != table2.Field<double?>("Adj_Amt") || table1.Field<string>("ChNo") != table2.Field<string>("ChNo") || table1.Field<string>("ChBank") != table2.Field<string>("ChBank") || table1.Field<string>("VatCode") != table2.Field<string>("VatCode")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable()
                                        on new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<double?>("Amt"), ColG = table2.Field<string>("DC"), ColH = table2.Field<string>("Vtype"), ColI = table2.Field<string>("RCode"), ColJ = table2.Field<string>("Narr1"), ColK = table2.Field<string>("Narr2"), ColL = table2.Field<string>("Narr3"), ColM = table2.Field<string>("Narr4"), ColN = table2.Field<string>("Contra"), ColO = table2.Field<string>("Adj_Type"), ColP = table2.Field<double?>("Adj_Amt"), ColQ = table2.Field<string>("ChNo"), ColS = table2.Field<string>("ChBank"), ColU = table2.Field<string>("VatCode") } equals new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<double?>("Amt"), ColG = table1.Field<string>("DC"), ColH = table1.Field<string>("Vtype"), ColI = table1.Field<string>("RCode"), ColJ = table1.Field<string>("Narr1"), ColK = table1.Field<string>("Narr2"), ColL = table1.Field<string>("Narr3"), ColM = table1.Field<string>("Narr4"), ColN = table1.Field<string>("Contra"), ColO = table1.Field<string>("Adj_Type"), ColP = table1.Field<double?>("Adj_Amt"), ColQ = table1.Field<string>("ChNo"), ColS = table1.Field<string>("ChBank"), ColU = table1.Field<string>("VatCode") }
                                     where table1.Field<string>("Link") != table1.Field<string>("Link") || table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<DateTime>("Date") != table2.Field<DateTime>("Date") || table1.Field<string>("DocNo") != table2.Field<string>("DocNo") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<double?>("Amt") != table2.Field<double?>("Amt") || table1.Field<string>("DC") != table2.Field<string>("DC") || table1.Field<string>("Vtype") != table2.Field<string>("Vtype") || table1.Field<string>("RCode") != table2.Field<string>("RCode") || table1.Field<string>("Narr1") != table2.Field<string>("Narr1") || table1.Field<string>("Narr2") != table2.Field<string>("Narr2") || table1.Field<string>("Narr3") != table2.Field<string>("Narr3") || table1.Field<string>("Narr4") != table2.Field<string>("Narr4") || table1.Field<string>("Contra") != table2.Field<string>("Contra") || table1.Field<string>("Adj_Type") != table2.Field<string>("Adj_Type") || table1.Field<double?>("Adj_Amt") != table2.Field<double?>("Adj_Amt") || table1.Field<string>("ChNo") != table2.Field<string>("ChNo") || table1.Field<string>("ChBank") != table2.Field<string>("ChBank") || table1.Field<string>("VatCode") != table2.Field<string>("VatCode")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<double?>("Amt"), ColG = table2.Field<string>("DC"), ColH = table2.Field<string>("Vtype"), ColI = table2.Field<string>("RCode"), ColJ = table2.Field<string>("Narr1"), ColK = table2.Field<string>("Narr2"), ColL = table2.Field<string>("Narr3"), ColM = table2.Field<string>("Narr4"), ColN = table2.Field<string>("Contra"), ColO = table2.Field<string>("Adj_Type"), ColP = table2.Field<double?>("Adj_Amt"), ColQ = table2.Field<string>("ChNo"), ColS = table2.Field<string>("ChBank"), ColU = table2.Field<string>("VatCode") } equals new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<double?>("Amt"), ColG = table1.Field<string>("DC"), ColH = table1.Field<string>("Vtype"), ColI = table1.Field<string>("RCode"), ColJ = table1.Field<string>("Narr1"), ColK = table1.Field<string>("Narr2"), ColL = table1.Field<string>("Narr3"), ColM = table1.Field<string>("Narr4"), ColN = table1.Field<string>("Contra"), ColO = table1.Field<string>("Adj_Type"), ColP = table1.Field<double?>("Adj_Amt"), ColQ = table1.Field<string>("ChNo"), ColS = table1.Field<string>("ChBank"), ColU = table1.Field<string>("VatCode") } into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<double?>("Amt"), ColG = table1.Field<string>("DC"), ColH = table1.Field<string>("Vtype"), ColI = table1.Field<string>("RCode"), ColJ = table1.Field<string>("Narr1"), ColK = table1.Field<string>("Narr2"), ColL = table1.Field<string>("Narr3"), ColM = table1.Field<string>("Narr4"), ColN = table1.Field<string>("Contra"), ColO = table1.Field<string>("Adj_Type"), ColP = table1.Field<double?>("Adj_Amt"), ColQ = table1.Field<string>("ChNo"), ColS = table1.Field<string>("ChBank"), ColU = table1.Field<string>("VatCode") } equals new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<double?>("Amt"), ColG = table2.Field<string>("DC"), ColH = table2.Field<string>("Vtype"), ColI = table2.Field<string>("RCode"), ColJ = table2.Field<string>("Narr1"), ColK = table2.Field<string>("Narr2"), ColL = table2.Field<string>("Narr3"), ColM = table2.Field<string>("Narr4"), ColN = table2.Field<string>("Contra"), ColO = table2.Field<string>("Adj_Type"), ColP = table2.Field<double?>("Adj_Amt"), ColQ = table2.Field<string>("ChNo"), ColS = table2.Field<string>("ChBank"), ColU = table2.Field<string>("VatCode") } into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region LEDMAST
                    case TableNames.LEDMAST:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable()
                                        on new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<double?>("Amt"), ColG = table1.Field<string>("Vtype"), ColI = table1.Field<string>("PSCode"), ColJ = table1.Field<string>("Ord_No") } equals new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<double?>("Amt"), ColG = table2.Field<string>("Vtype"), ColI = table2.Field<string>("PSCode"), ColJ = table2.Field<string>("Ord_No") }
                                   where table1.Field<string>("Link") != table1.Field<string>("Link") || table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<DateTime>("Date") != table2.Field<DateTime>("Date") || table1.Field<string>("DocNo") != table2.Field<string>("DocNo") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<double?>("Amt") != table2.Field<double?>("Amt") || table1.Field<string>("Vtype") != table2.Field<string>("Vtype") || table1.Field<string>("PSCode") != table2.Field<string>("PSCode") || table1.Field<string>("Ord_no") != table2.Field<string>("Ord_no")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable()
                                        on new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<double?>("Amt"), ColG = table2.Field<string>("Vtype"), ColI = table2.Field<string>("PSCode"), ColJ = table2.Field<string>("Ord_No") } equals new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<double?>("Amt"), ColG = table1.Field<string>("Vtype"), ColI = table1.Field<string>("PSCode"), ColJ = table1.Field<string>("Ord_No") }
                                     where table1.Field<string>("Link") != table1.Field<string>("Link") || table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<DateTime>("Date") != table2.Field<DateTime>("Date") || table1.Field<string>("DocNo") != table2.Field<string>("DocNo") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<double?>("Amt") != table2.Field<double?>("Amt") || table1.Field<string>("Vtype") != table2.Field<string>("Vtype") || table1.Field<string>("PSCode") != table2.Field<string>("PSCode") || table1.Field<string>("Ord_no") != table2.Field<string>("Ord_no")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<double?>("Amt"), ColG = table2.Field<string>("Vtype"), ColI = table2.Field<string>("PSCode"), ColJ = table2.Field<string>("Ord_No") } equals new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<double?>("Amt"), ColG = table1.Field<string>("Vtype"), ColI = table1.Field<string>("PSCode"), ColJ = table1.Field<string>("Ord_No") } into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<double?>("Amt"), ColG = table1.Field<string>("Vtype"), ColI = table1.Field<string>("PSCode"), ColJ = table1.Field<string>("Ord_No") } equals new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<double?>("Amt"), ColG = table2.Field<string>("Vtype"), ColI = table2.Field<string>("PSCode"), ColJ = table2.Field<string>("Ord_No") } into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region ORDER
                    case TableNames.ORDER:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable()
                                        on new { ColA = table1.Field<string>("Division"), ColB = table1.Field<string>("Ord_No"), ColC = table1.Field<DateTime>("Ord_Dt"), ColD = table1.Field<string>("Code"), ColE = table1.Field<double?>("Ord_value"), ColF = table1.Field<string>("Bill_Ins"), ColG = table1.Field<string>("Pay_Terms"), ColI = table1.Field<string>("Del_Ins"), ColJ = table1.Field<Boolean>("Adjusted"), ColK = table1.Field<string>("Ship"), ColL = table1.Field<string>("TRPT"), ColM = table1.Field<Boolean>("Closed") } equals new { ColA = table2.Field<string>("Division"), ColB = table2.Field<string>("Ord_No"), ColC = table2.Field<DateTime>("Ord_Dt"), ColD = table2.Field<string>("Code"), ColE = table2.Field<double?>("Ord_value"), ColF = table2.Field<string>("Bill_Ins"), ColG = table2.Field<string>("Pay_Terms"), ColI = table2.Field<string>("Del_Ins"), ColJ = table2.Field<Boolean>("Adjusted"), ColK = table2.Field<string>("Ship"), ColL = table2.Field<string>("TRPT"), ColM = table2.Field<Boolean>("Closed") }
                                   where table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<string>("Ord_No") != table2.Field<string>("Ord_No") || table1.Field<DateTime>("Ord_Dt") != table2.Field<DateTime>("Ord_Dt") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<double?>("Ord_value") != table2.Field<double?>("Ord_value") || table1.Field<string>("Bill_Ins") != table2.Field<string>("Bill_Ins") || table1.Field<string>("Pay_Terms") != table2.Field<string>("Pay_Terms") || table1.Field<string>("Del_Ins") != table2.Field<string>("Del_Ins") || table1.Field<Boolean>("Adjusted") != table2.Field<Boolean>("Adjusted") || table1.Field<string>("Ship") != table2.Field<string>("Ship") || table1.Field<string>("TRPT") != table2.Field<string>("TRPT") || table1.Field<Boolean>("Closed") != table2.Field<Boolean>("Closed")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable()
                                        on new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<double?>("Amt"), ColG = table2.Field<string>("Vtype"), ColI = table2.Field<string>("PSCode"), ColJ = table2.Field<string>("Ord_No") } equals new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<double?>("Amt"), ColG = table1.Field<string>("Vtype"), ColI = table1.Field<string>("PSCode"), ColJ = table1.Field<string>("Ord_No") }
                                     where table1.Field<string>("Link") != table1.Field<string>("Link") || table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<DateTime>("Date") != table2.Field<DateTime>("Date") || table1.Field<string>("DocNo") != table2.Field<string>("DocNo") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<double?>("Amt") != table2.Field<double?>("Amt") || table1.Field<string>("Vtype") != table2.Field<string>("Vtype") || table1.Field<string>("PSCode") != table2.Field<string>("PSCode") || table1.Field<string>("Ord_no") != table2.Field<string>("Ord_no")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<double?>("Amt"), ColG = table2.Field<string>("Vtype"), ColI = table2.Field<string>("PSCode"), ColJ = table2.Field<string>("Ord_No") } equals new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<double?>("Amt"), ColG = table1.Field<string>("Vtype"), ColI = table1.Field<string>("PSCode"), ColJ = table1.Field<string>("Ord_No") } into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<double?>("Amt"), ColG = table1.Field<string>("Vtype"), ColI = table1.Field<string>("PSCode"), ColJ = table1.Field<string>("Ord_No") } equals new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<double?>("Amt"), ColG = table2.Field<string>("Vtype"), ColI = table2.Field<string>("PSCode"), ColJ = table2.Field<string>("Ord_No") } into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region ORDER2
                    case TableNames.ORDER2:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable()
                                        on new { ColA = table1.Field<string>("Division"), ColB = table1.Field<string>("Ord_No"), ColC = table1.Field<DateTime>("Ord_Dt"), ColD = table1.Field<string>("Code"), ColE = table1.Field<double?>("Qty"), ColF = table1.Field<double?>("Rate"), ColG = table1.Field<double?>("Value"), ColI = table1.Field<string>("Unit"), ColJ = table1.Field<double?>("It_Disc"), ColK = table1.Field<double?>("It_tax"), ColL = table1.Field<double?>("It_Oc"), ColM = table1.Field<double?>("Stk_Qty"), ColN = table1.Field<DateTime>("Dly_Dt"), ColO = table1.Field<string>("Cv_Code"), ColP = table1.Field<string>("Type"), ColQ = table1.Field<string>("BatchNo"), ColR = table1.Field<string>("Narr1"), ColS = table1.Field<string>("Narr2"), ColT = table1.Field<double?>("Packages"), ColU = table1.Field<string>("Trackno"), ColV = table1.Field<string>("RtUnit") } equals new { ColA = table2.Field<string>("Division"), ColB = table2.Field<string>("Ord_No"), ColC = table2.Field<DateTime>("Ord_Dt"), ColD = table2.Field<string>("Code"), ColE = table2.Field<double?>("Qty"), ColF = table2.Field<double?>("Rate"), ColG = table2.Field<double?>("Value"), ColI = table2.Field<string>("Unit"), ColJ = table2.Field<double?>("It_Disc"), ColK = table2.Field<double?>("It_tax"), ColL = table2.Field<double?>("It_Oc"), ColM = table2.Field<double?>("Stk_Qty"), ColN = table2.Field<DateTime>("Dly_Dt"), ColO = table2.Field<string>("Cv_Code"), ColP = table2.Field<string>("Type"), ColQ = table2.Field<string>("BatchNo"), ColR = table2.Field<string>("Narr1"), ColS = table2.Field<string>("Narr2"), ColT = table2.Field<double?>("Packages"), ColU = table2.Field<string>("Trackno"), ColV = table2.Field<string>("RtUnit") }
                                   where table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<string>("Ord_No") != table2.Field<string>("Ord_No") || table1.Field<DateTime>("Ord_Dt") != table2.Field<DateTime>("Ord_Dt") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<double?>("Qty") != table2.Field<double?>("Qty") || table1.Field<double?>("Rate") != table2.Field<double?>("Rate") || table1.Field<double?>("Value") != table2.Field<double?>("Value") || table1.Field<string>("Unit") != table2.Field<string>("Unit") || table1.Field<double?>("It_Disc") != table2.Field<double?>("It_Disc") || table1.Field<double?>("It_tax") != table2.Field<double?>("It_tax") || table1.Field<double?>("It_Oc") != table2.Field<double?>("It_Oc") || table1.Field<double?>("Stk_Qty") != table2.Field<double?>("Stk_Qty") || table1.Field<DateTime>("Dly_Dt") != table2.Field<DateTime>("Dly_Dt") || table1.Field<string>("Cv_Code") != table2.Field<string>("Cv_Code") || table1.Field<string>("Type") != table2.Field<string>("Type") || table1.Field<string>("BatchNo") != table2.Field<string>("BatchNo") || table1.Field<string>("Narr1") != table2.Field<string>("Narr1") || table1.Field<string>("Narr2") != table2.Field<string>("Narr2") || table1.Field<double?>("Packages") != table2.Field<double?>("Packages") || table1.Field<string>("Trackno") != table2.Field<string>("Trackno") || table1.Field<string>("RtUnit") != table2.Field<string>("RtUnit")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable()
                                        on new { ColA = table2.Field<string>("Division"), ColB = table2.Field<string>("Ord_No"), ColC = table2.Field<DateTime>("Ord_Dt"), ColD = table2.Field<string>("Code"), ColE = table2.Field<double?>("Qty"), ColF = table2.Field<double?>("Rate"), ColG = table2.Field<double?>("Value"), ColI = table2.Field<string>("Unit"), ColJ = table2.Field<double?>("It_Disc"), ColK = table2.Field<double?>("It_tax"), ColL = table2.Field<double?>("It_Oc"), ColM = table2.Field<double?>("Stk_Qty"), ColN = table2.Field<DateTime>("Dly_Dt"), ColO = table2.Field<string>("Cv_Code"), ColP = table2.Field<string>("Type"), ColQ = table2.Field<string>("BatchNo"), ColR = table2.Field<string>("Narr1"), ColS = table2.Field<string>("Narr2"), ColT = table2.Field<double?>("Packages"), ColU = table2.Field<string>("Trackno"), ColV = table2.Field<string>("RtUnit") } equals new { ColA = table1.Field<string>("Division"), ColB = table1.Field<string>("Ord_No"), ColC = table1.Field<DateTime>("Ord_Dt"), ColD = table1.Field<string>("Code"), ColE = table1.Field<double?>("Qty"), ColF = table1.Field<double?>("Rate"), ColG = table1.Field<double?>("Value"), ColI = table1.Field<string>("Unit"), ColJ = table1.Field<double?>("It_Disc"), ColK = table1.Field<double?>("It_tax"), ColL = table1.Field<double?>("It_Oc"), ColM = table1.Field<double?>("Stk_Qty"), ColN = table1.Field<DateTime>("Dly_Dt"), ColO = table1.Field<string>("Cv_Code"), ColP = table1.Field<string>("Type"), ColQ = table1.Field<string>("BatchNo"), ColR = table1.Field<string>("Narr1"), ColS = table1.Field<string>("Narr2"), ColT = table1.Field<double?>("Packages"), ColU = table1.Field<string>("Trackno"), ColV = table1.Field<string>("RtUnit") }
                                     where table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<string>("Ord_No") != table2.Field<string>("Ord_No") || table1.Field<DateTime>("Ord_Dt") != table2.Field<DateTime>("Ord_Dt") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<double?>("Qty") != table2.Field<double?>("Qty") || table1.Field<double?>("Rate") != table2.Field<double?>("Rate") || table1.Field<double?>("Value") != table2.Field<double?>("Value") || table1.Field<string>("Unit") != table2.Field<string>("Unit") || table1.Field<double?>("It_Disc") != table2.Field<double?>("It_Disc") || table1.Field<double?>("It_tax") != table2.Field<double?>("It_tax") || table1.Field<double?>("It_Oc") != table2.Field<double?>("It_Oc") || table1.Field<double?>("Stk_Qty") != table2.Field<double?>("Stk_Qty") || table1.Field<DateTime>("Dly_Dt") != table2.Field<DateTime>("Dly_Dt") || table1.Field<string>("Cv_Code") != table2.Field<string>("Cv_Code") || table1.Field<string>("Type") != table2.Field<string>("Type") || table1.Field<string>("BatchNo") != table2.Field<string>("BatchNo") || table1.Field<string>("Narr1") != table2.Field<string>("Narr1") || table1.Field<string>("Narr2") != table2.Field<string>("Narr2") || table1.Field<double?>("Packages") != table2.Field<double?>("Packages") || table1.Field<string>("Trackno") != table2.Field<string>("Trackno") || table1.Field<string>("RtUnit") != table2.Field<string>("RtUnit")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on new { ColA = table2.Field<string>("Division"), ColB = table2.Field<string>("Ord_No"), ColC = table2.Field<DateTime>("Ord_Dt"), ColD = table2.Field<string>("Code"), ColE = table2.Field<double?>("Qty"), ColF = table2.Field<double?>("Rate"), ColG = table2.Field<double?>("Value"), ColI = table2.Field<string>("Unit"), ColJ = table2.Field<double?>("It_Disc"), ColK = table2.Field<double?>("It_tax"), ColL = table2.Field<double?>("It_Oc"), ColM = table2.Field<double?>("Stk_Qty"), ColN = table2.Field<DateTime>("Dly_Dt"), ColO = table2.Field<string>("Cv_Code"), ColP = table2.Field<string>("Type"), ColQ = table2.Field<string>("BatchNo"), ColR = table2.Field<string>("Narr1"), ColS = table2.Field<string>("Narr2"), ColT = table2.Field<double?>("Packages"), ColU = table2.Field<string>("Trackno"), ColV = table2.Field<string>("RtUnit") } equals new { ColA = table1.Field<string>("Division"), ColB = table1.Field<string>("Ord_No"), ColC = table1.Field<DateTime>("Ord_Dt"), ColD = table1.Field<string>("Code"), ColE = table1.Field<double?>("Qty"), ColF = table1.Field<double?>("Rate"), ColG = table1.Field<double?>("Value"), ColI = table1.Field<string>("Unit"), ColJ = table1.Field<double?>("It_Disc"), ColK = table1.Field<double?>("It_tax"), ColL = table1.Field<double?>("It_Oc"), ColM = table1.Field<double?>("Stk_Qty"), ColN = table1.Field<DateTime>("Dly_Dt"), ColO = table1.Field<string>("Cv_Code"), ColP = table1.Field<string>("Type"), ColQ = table1.Field<string>("BatchNo"), ColR = table1.Field<string>("Narr1"), ColS = table1.Field<string>("Narr2"), ColT = table1.Field<double?>("Packages"), ColU = table1.Field<string>("Trackno"), ColV = table1.Field<string>("RtUnit") } into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on new { ColA = table1.Field<string>("Division"), ColB = table1.Field<string>("Ord_No"), ColC = table1.Field<DateTime>("Ord_Dt"), ColD = table1.Field<string>("Code"), ColE = table1.Field<double?>("Qty"), ColF = table1.Field<double?>("Rate"), ColG = table1.Field<double?>("Value"), ColI = table1.Field<string>("Unit"), ColJ = table1.Field<double?>("It_Disc"), ColK = table1.Field<double?>("It_tax"), ColL = table1.Field<double?>("It_Oc"), ColM = table1.Field<double?>("Stk_Qty"), ColN = table1.Field<DateTime>("Dly_Dt"), ColO = table1.Field<string>("Cv_Code"), ColP = table1.Field<string>("Type"), ColQ = table1.Field<string>("BatchNo"), ColR = table1.Field<string>("Narr1"), ColS = table1.Field<string>("Narr2"), ColT = table1.Field<double?>("Packages"), ColU = table1.Field<string>("Trackno"), ColV = table1.Field<string>("RtUnit") } equals new { ColA = table2.Field<string>("Division"), ColB = table2.Field<string>("Ord_No"), ColC = table2.Field<DateTime>("Ord_Dt"), ColD = table2.Field<string>("Code"), ColE = table2.Field<double?>("Qty"), ColF = table2.Field<double?>("Rate"), ColG = table2.Field<double?>("Value"), ColI = table2.Field<string>("Unit"), ColJ = table2.Field<double?>("It_Disc"), ColK = table2.Field<double?>("It_tax"), ColL = table2.Field<double?>("It_Oc"), ColM = table2.Field<double?>("Stk_Qty"), ColN = table2.Field<DateTime>("Dly_Dt"), ColO = table2.Field<string>("Cv_Code"), ColP = table2.Field<string>("Type"), ColQ = table2.Field<string>("BatchNo"), ColR = table2.Field<string>("Narr1"), ColS = table2.Field<string>("Narr2"), ColT = table2.Field<double?>("Packages"), ColU = table2.Field<string>("Trackno"), ColV = table2.Field<string>("RtUnit") } into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region PGROUP
                    case TableNames.PGROUP:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Code") equals table2.Field<string>("Code")
                                   where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("Category1") != table2.Field<string>("Category1") || table1.Field<string>("Category2") != table2.Field<string>("Category2") || table1.Field<string>("Category3") != table2.Field<string>("Category3") || table1.Field<string>("Parent") != table2.Field<string>("Parent")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable() on table2.Field<string>("Code") equals table1.Field<string>("Code")
                                     where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("Category1") != table2.Field<string>("Category1") || table1.Field<string>("Category2") != table2.Field<string>("Category2") || table1.Field<string>("Category3") != table2.Field<string>("Category3") || table1.Field<string>("Parent") != table2.Field<string>("Parent")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on table2.Field<string>("Code") equals table1.Field<string>("Code") into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on table1.Field<string>("Code") equals table2.Field<string>("Code") into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region PRODUCT
                    case TableNames.PRODUCT:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Code") equals table2.Field<string>("Code")
                                   where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("Group") != table2.Field<string>("Group") || table1.Field<string>("Desc") != table2.Field<string>("Desc") || table1.Field<Boolean>("Batch") != table2.Field<Boolean>("Batch") || table1.Field<Boolean>("DUnit") != table2.Field<Boolean>("Dunit") || table1.Field<string>("Unit1") != table2.Field<string>("Unit1") || table1.Field<string>("Unit2") != table2.Field<string>("Unit2") || table1.Field<string>("Unit3") != table2.Field<string>("Unit3") || table1.Field<string>("Unit4") != table2.Field<string>("Unit4") || table1.Field<string>("Unit5") != table2.Field<string>("Unit5") || table1.Field<string>("Unit6") != table2.Field<string>("Unit6") || table1.Field<double?>("Ratio2") != table2.Field<double?>("Ratio2") || table1.Field<double?>("Ratio3") != table2.Field<double?>("Ratio3") || table1.Field<double?>("Ratio4") != table2.Field<double?>("Ratio4") || table1.Field<double?>("Ratio5") != table2.Field<double?>("Ratio5") || table1.Field<double?>("Ratio6") != table2.Field<double?>("Ratio6") || table1.Field<double?>("Op_Qty") != table2.Field<double?>("Op_Qty") || table1.Field<double?>("TotIn") != table2.Field<double?>("TotIn") || table1.Field<double?>("TotOut") != table2.Field<double?>("TotOut") || table1.Field<double?>("Cl_Qty") != table2.Field<double?>("Cl_Qty") || table1.Field<double?>("Op_Value") != table2.Field<double?>("Op_Value") || table1.Field<double?>("Op_Rate") != table2.Field<double?>("Op_Rate") || table1.Field<double?>("Sl_Rate") != table2.Field<double?>("Sl_Rate") || table1.Field<double?>("Pu_Rate") != table2.Field<double?>("Pu_Rate") || table1.Field<double?>("Op_Package") != table2.Field<double?>("Op_Package") || table1.Field<string>("I_VatCode") != table2.Field<string>("I_VatCode") || table1.Field<string>("O_VatCode") != table2.Field<string>("O_VatCode")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable() on table2.Field<string>("Code") equals table1.Field<string>("Code")
                                     where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<string>("Group") != table2.Field<string>("Group") || table1.Field<string>("Desc") != table2.Field<string>("Desc") || table1.Field<Boolean>("Batch") != table2.Field<Boolean>("Batch") || table1.Field<Boolean>("DUnit") != table2.Field<Boolean>("Dunit") || table1.Field<string>("Unit1") != table2.Field<string>("Unit1") || table1.Field<string>("Unit2") != table2.Field<string>("Unit2") || table1.Field<string>("Unit3") != table2.Field<string>("Unit3") || table1.Field<string>("Unit4") != table2.Field<string>("Unit4") || table1.Field<string>("Unit5") != table2.Field<string>("Unit5") || table1.Field<string>("Unit6") != table2.Field<string>("Unit6") || table1.Field<double?>("Ratio2") != table2.Field<double?>("Ratio2") || table1.Field<double?>("Ratio3") != table2.Field<double?>("Ratio3") || table1.Field<double?>("Ratio4") != table2.Field<double?>("Ratio4") || table1.Field<double?>("Ratio5") != table2.Field<double?>("Ratio5") || table1.Field<double?>("Ratio6") != table2.Field<double?>("Ratio6") || table1.Field<double?>("Op_Qty") != table2.Field<double?>("Op_Qty") || table1.Field<double?>("TotIn") != table2.Field<double?>("TotIn") || table1.Field<double?>("TotOut") != table2.Field<double?>("TotOut") || table1.Field<double?>("Cl_Qty") != table2.Field<double?>("Cl_Qty") || table1.Field<double?>("Op_Value") != table2.Field<double?>("Op_Value") || table1.Field<double?>("Op_Rate") != table2.Field<double?>("Op_Rate") || table1.Field<double?>("Sl_Rate") != table2.Field<double?>("Sl_Rate") || table1.Field<double?>("Pu_Rate") != table2.Field<double?>("Pu_Rate") || table1.Field<double?>("Op_Package") != table2.Field<double?>("Op_Package") || table1.Field<string>("I_VatCode") != table2.Field<string>("I_VatCode") || table1.Field<string>("O_VatCode") != table2.Field<string>("O_VatCode")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on table2.Field<string>("Code") equals table1.Field<string>("Code") into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on table1.Field<string>("Code") equals table2.Field<string>("Code") into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region SALE_ADJ
                    case TableNames.SALE_ADJ:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable()
                                        on new { ColA = table1.Field<string>("Sp_Link"), ColB = table1.Field<string>("OT_Link"), ColC = table1.Field<double?>("Amt"), ColD = table1.Field<string>("DC"), ColE = table1.Field<double?>("Disc"), ColF = table1.Field<string>("Code") } equals new { ColA = table2.Field<string>("Sp_Link"), ColB = table2.Field<string>("OT_Link"), ColC = table2.Field<double?>("Amt"), ColD = table2.Field<string>("DC"), ColE = table2.Field<double?>("Disc"), ColF = table2.Field<string>("Code") }
                                   where table1.Field<string>("Sp_Link") != table2.Field<string>("Sp_Link") || table1.Field<string>("OT_Link") != table2.Field<string>("OT_Link") || table1.Field<double?>("Amt") != table2.Field<double?>("Amt") || table1.Field<string>("DC") != table2.Field<string>("DC") || table1.Field<double?>("Disc") != table2.Field<double?>("Disc") || table1.Field<string>("Code") != table2.Field<string>("Code")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable()
                                        on new { ColA = table2.Field<string>("Sp_Link"), ColB = table2.Field<string>("OT_Link"), ColC = table2.Field<double?>("Amt"), ColD = table2.Field<string>("DC"), ColE = table2.Field<double?>("Disc"), ColF = table2.Field<string>("Code") } equals new { ColA = table1.Field<string>("Sp_Link"), ColB = table1.Field<string>("OT_Link"), ColC = table1.Field<double?>("Amt"), ColD = table1.Field<string>("DC"), ColE = table1.Field<double?>("Disc"), ColF = table1.Field<string>("Code") }
                                     where table1.Field<string>("Sp_Link") != table2.Field<string>("Sp_Link") || table1.Field<string>("OT_Link") != table2.Field<string>("OT_Link") || table1.Field<double?>("Amt") != table2.Field<double?>("Amt") || table1.Field<string>("DC") != table2.Field<string>("DC") || table1.Field<double?>("Disc") != table2.Field<double?>("Disc") || table1.Field<string>("Code") != table2.Field<string>("Code")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on new { ColA = table2.Field<string>("Sp_Link"), ColB = table2.Field<string>("OT_Link"), ColC = table2.Field<double?>("Amt"), ColD = table2.Field<string>("DC"), ColE = table2.Field<double?>("Disc"), ColF = table2.Field<string>("Code") } equals new { ColA = table1.Field<string>("Sp_Link"), ColB = table1.Field<string>("OT_Link"), ColC = table1.Field<double?>("Amt"), ColD = table1.Field<string>("DC"), ColE = table1.Field<double?>("Disc"), ColF = table1.Field<string>("Code") } into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on new { ColA = table1.Field<string>("Sp_Link"), ColB = table1.Field<string>("OT_Link"), ColC = table1.Field<double?>("Amt"), ColD = table1.Field<string>("DC"), ColE = table1.Field<double?>("Disc"), ColF = table1.Field<string>("Code") } equals new { ColA = table2.Field<string>("Sp_Link"), ColB = table2.Field<string>("OT_Link"), ColC = table2.Field<double?>("Amt"), ColD = table2.Field<string>("DC"), ColE = table2.Field<double?>("Disc"), ColF = table2.Field<string>("Code") } into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region SERIAL
                    case TableNames.SERIAL:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable()
                                        on new { ColA = table1.Field<string>("Code"), ColB = table1.Field<string>("BatchNo"), ColC = table1.Field<string>("SerialNo"), ColD = table1.Field<double?>("Qty"), ColE = table1.Field<string>("Narr"), ColF = table1.Field<string>("Link"), ColG = table1.Field<string>("PosiFile") } equals new { ColA = table2.Field<string>("Code"), ColB = table2.Field<string>("BatchNo"), ColC = table2.Field<string>("SerialNo"), ColD = table2.Field<double?>("Qty"), ColE = table2.Field<string>("Narr"), ColF = table2.Field<string>("Link"), ColG = table2.Field<string>("PosiFile") }
                                   where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("BatchNo") != table2.Field<string>("BatchNo") || table1.Field<string>("SerialNo") != table2.Field<string>("SerialNo") || table1.Field<double?>("Qty") != table2.Field<double?>("Qty") || table1.Field<string>("Narr") != table2.Field<string>("Narr") || table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("PosiFile") != table2.Field<string>("PosiFile")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable()
                                        on new { ColA = table2.Field<string>("Code"), ColB = table2.Field<string>("BatchNo"), ColC = table2.Field<string>("SerialNo"), ColD = table2.Field<double?>("Qty"), ColE = table2.Field<string>("Narr"), ColF = table2.Field<string>("Link"), ColG = table2.Field<string>("PosiFile") } equals new { ColA = table1.Field<string>("Code"), ColB = table1.Field<string>("BatchNo"), ColC = table1.Field<string>("SerialNo"), ColD = table1.Field<double?>("Qty"), ColE = table1.Field<string>("Narr"), ColF = table1.Field<string>("Link"), ColG = table1.Field<string>("PosiFile") }
                                     where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("BatchNo") != table2.Field<string>("BatchNo") || table1.Field<string>("SerialNo") != table2.Field<string>("SerialNo") || table1.Field<double?>("Qty") != table2.Field<double?>("Qty") || table1.Field<string>("Narr") != table2.Field<string>("Narr") || table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("PosiFile") != table2.Field<string>("PosiFile")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on new { ColA = table2.Field<string>("Code"), ColB = table2.Field<string>("BatchNo"), ColC = table2.Field<string>("SerialNo"), ColD = table2.Field<double?>("Qty"), ColE = table2.Field<string>("Narr"), ColF = table2.Field<string>("Link"), ColG = table2.Field<string>("PosiFile") } equals new { ColA = table1.Field<string>("Code"), ColB = table1.Field<string>("BatchNo"), ColC = table1.Field<string>("SerialNo"), ColD = table1.Field<double?>("Qty"), ColE = table1.Field<string>("Narr"), ColF = table1.Field<string>("Link"), ColG = table1.Field<string>("PosiFile") } into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on new { ColA = table1.Field<string>("Code"), ColB = table1.Field<string>("BatchNo"), ColC = table1.Field<string>("SerialNo"), ColD = table1.Field<double?>("Qty"), ColE = table1.Field<string>("Narr"), ColF = table1.Field<string>("Link"), ColG = table1.Field<string>("PosiFile") } equals new { ColA = table2.Field<string>("Code"), ColB = table2.Field<string>("BatchNo"), ColC = table2.Field<string>("SerialNo"), ColD = table2.Field<double?>("Qty"), ColE = table2.Field<string>("Narr"), ColF = table2.Field<string>("Link"), ColG = table2.Field<string>("PosiFile") } into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region STAX
                    case TableNames.STAX:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Code") equals table2.Field<string>("Code")
                                   where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<double?>("PCN") != table2.Field<double?>("PCN") || table1.Field<string>("Type") != table2.Field<string>("Type") || table1.Field<Boolean>("Form") != table2.Field<Boolean>("Form") || table1.Field<double?>("VatType") != table2.Field<double?>("VatType") || table1.Field<string>("VatDet") != table2.Field<string>("VatDet") || table1.Field<double?>("Bc_Tax") != table2.Field<double?>("Bc_Tax") || table1.Field<double?>("BC_Sc") != table2.Field<double?>("Bc_Sc") || table1.Field<Boolean>("TaxIncl") != table2.Field<Boolean>("TaxIncl") || table1.Field<Boolean>("Itemwise") != table2.Field<Boolean>("Itemwise")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable() on table2.Field<string>("Code") equals table1.Field<string>("Code")
                                     where table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<double?>("PCN") != table2.Field<double?>("PCN") || table1.Field<string>("Type") != table2.Field<string>("Type") || table1.Field<Boolean>("Form") != table2.Field<Boolean>("Form") || table1.Field<double?>("VatType") != table2.Field<double?>("VatType") || table1.Field<string>("VatDet") != table2.Field<string>("VatDet") || table1.Field<double?>("Bc_Tax") != table2.Field<double?>("Bc_Tax") || table1.Field<double?>("BC_Sc") != table2.Field<double?>("Bc_Sc") || table1.Field<Boolean>("TaxIncl") != table2.Field<Boolean>("TaxIncl") || table1.Field<Boolean>("Itemwise") != table2.Field<Boolean>("Itemwise")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on table2.Field<string>("Code") equals table1.Field<string>("Code") into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on table1.Field<string>("Code") equals table2.Field<string>("Code") into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region SP
                    case TableNames.SP:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable()
                                        on new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime?>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<string>("Ord_No"), ColG = table1.Field<double?>("It_Val"), ColH = table1.Field<double?>("Disc_per"), ColI = table1.Field<double?>("Disc_Amt"), ColJ = table1.Field<double?>("Sd_per"), ColK = table1.Field<double?>("Sd_Amt"), ColL = table1.Field<double?>("OC_Before"), ColM = table1.Field<double?>("Tax_Amt"), ColN = table1.Field<string>("St_Type"), ColO = table1.Field<double?>("St_Amt"), ColP = table1.Field<double?>("Oc1_Per"), ColQ = table1.Field<double?>("Oc1_amt"), ColR = table1.Field<double?>("Oc2_Per"), ColS = table1.Field<double?>("Oc2_amt"), ColT = table1.Field<double?>("Oc3_Per"), ColU = table1.Field<double?>("Oc3_amt"), ColV = table1.Field<double?>("Oc4_Per"), ColW = table1.Field<double?>("Oc4_amt"), ColX = table1.Field<double?>("Oc5_Per"), ColY = table1.Field<double?>("Oc5_amt"), ColZ = table1.Field<double?>("Rounded"), Col1 = table1.Field<double?>("Bill_amt"), Col2 = table1.Field<string>("Agent"), Col3 = table1.Field<double?>("Ag_Per"), Col4 = table1.Field<double?>("Ag_Comm"), Col5 = table1.Field<Boolean>("Form_Req"), Col6 = table1.Field<string>("Form_Type"), Col66 = table1.Field<string>("Form_No"), Col7 = table1.Field<DateTime?>("Form_Dt"), Col8 = table1.Field<double?>("Excise_Per"), Col9 = table1.Field<double?>("Excise_Amt"), Col10 = table1.Field<double?>("SplExc_Per"), Col11 = table1.Field<double?>("SplExc_Amt"), Col12 = table1.Field<double?>("At_Type"), Col122 = table1.Field<double?>("At_Amt"), Col13 = table1.Field<string>("Svt_Type"), Col14 = table1.Field<double?>("Svt_On"), Col15 = table1.Field<double?>("Svt_Amt"), Col16 = table1.Field<double?>("Tot_Per"), Col17 = table1.Field<double?>("Tot_Amt"), Col18 = table1.Field<double?>("Exempt"), Col19 = table1.Field<string>("PSCode"), Col20 = table1.Field<string>("Narr1"), Col21 = table1.Field<string>("Narr2"), Col22 = table1.Field<string>("Narr3"), Col23 = table1.Field<Boolean>("TaxIncl"), Col24 = table1.Field<string>("CBR"), Col25 = table1.Field<string>("CBAC"), Col26 = table1.Field<string>("ChNo"), Col27 = table1.Field<DateTime?>("ChDt"), Col28 = table1.Field<string>("ChBank"), Col29 = table1.Field<DateTime?>("Ord_Dt"), Col30 = table1.Field<double?>("DueDays"), Col31 = table1.Field<string>("ChlnLink"), Col32 = table1.Field<double?>("Bal_Amt"), Col33 = table1.Field<DateTime?>("StockDt"), Col34 = table1.Field<string>("WayBillNo"), Col35 = table1.Field<DateTime?>("WayBillDt") } equals new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime?>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<string>("Ord_No"), ColG = table2.Field<double?>("It_Val"), ColH = table2.Field<double?>("Disc_per"), ColI = table2.Field<double?>("Disc_Amt"), ColJ = table2.Field<double?>("Sd_per"), ColK = table2.Field<double?>("Sd_Amt"), ColL = table2.Field<double?>("OC_Before"), ColM = table2.Field<double?>("Tax_Amt"), ColN = table2.Field<string>("St_Type"), ColO = table2.Field<double?>("St_Amt"), ColP = table2.Field<double?>("Oc1_Per"), ColQ = table2.Field<double?>("Oc1_amt"), ColR = table2.Field<double?>("Oc2_Per"), ColS = table2.Field<double?>("Oc2_amt"), ColT = table2.Field<double?>("Oc3_Per"), ColU = table2.Field<double?>("Oc3_amt"), ColV = table2.Field<double?>("Oc4_Per"), ColW = table2.Field<double?>("Oc4_amt"), ColX = table2.Field<double?>("Oc5_Per"), ColY = table2.Field<double?>("Oc5_amt"), ColZ = table2.Field<double?>("Rounded"), Col1 = table2.Field<double?>("Bill_amt"), Col2 = table2.Field<string>("Agent"), Col3 = table2.Field<double?>("Ag_Per"), Col4 = table2.Field<double?>("Ag_Comm"), Col5 = table2.Field<Boolean>("Form_Req"), Col6 = table2.Field<string>("Form_Type"), Col66 = table2.Field<string>("Form_No"), Col7 = table2.Field<DateTime?>("Form_Dt"), Col8 = table2.Field<double?>("Excise_Per"), Col9 = table2.Field<double?>("Excise_Amt"), Col10 = table2.Field<double?>("SplExc_Per"), Col11 = table2.Field<double?>("SplExc_Amt"), Col12 = table2.Field<double?>("At_Type"), Col122 = table2.Field<double?>("At_Amt"), Col13 = table2.Field<string>("Svt_Type"), Col14 = table2.Field<double?>("Svt_On"), Col15 = table2.Field<double?>("Svt_Amt"), Col16 = table2.Field<double?>("Tot_Per"), Col17 = table2.Field<double?>("Tot_Amt"), Col18 = table2.Field<double?>("Exempt"), Col19 = table2.Field<string>("PSCode"), Col20 = table2.Field<string>("Narr1"), Col21 = table2.Field<string>("Narr2"), Col22 = table2.Field<string>("Narr3"), Col23 = table2.Field<Boolean>("TaxIncl"), Col24 = table2.Field<string>("CBR"), Col25 = table2.Field<string>("CBAC"), Col26 = table2.Field<string>("ChNo"), Col27 = table2.Field<DateTime?>("ChDt"), Col28 = table2.Field<string>("ChBank"), Col29 = table2.Field<DateTime?>("Ord_Dt"), Col30 = table2.Field<double?>("DueDays"), Col31 = table2.Field<string>("ChlnLink"), Col32 = table2.Field<double?>("Bal_Amt"), Col33 = table2.Field<DateTime?>("StockDt"), Col34 = table2.Field<string>("WayBillNo"), Col35 = table2.Field<DateTime?>("WayBillDt") }
                                   where table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<DateTime?>("Date") != table2.Field<DateTime?>("Date") || table1.Field<string>("DocNo") != table2.Field<string>("DocNo") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Ord_No") != table2.Field<string>("Ord_No") || table1.Field<double?>("It_Val") != table2.Field<double?>("It_Val") || table1.Field<double?>("Disc_per") != table2.Field<double?>("Disc_per") || table1.Field<double?>("Disc_Amt") != table2.Field<double?>("Disc_Amt") || table1.Field<double?>("Sd_per") != table2.Field<double?>("Sd_per") || table1.Field<double?>("Sd_Amt") != table2.Field<double?>("Sd_Amt") || table1.Field<double?>("OC_Before") != table2.Field<double?>("OC_Before") || table1.Field<double?>("Tax_Amt") != table2.Field<double?>("Tax_Amt") || table1.Field<string>("St_Type") != table2.Field<string>("St_Type") || table1.Field<double?>("St_Amt") != table2.Field<double?>("St_Amt") || table1.Field<double?>("Oc1_Per") != table2.Field<double?>("Oc1_Per") || table1.Field<double?>("Oc1_amt") != table2.Field<double?>("Oc1_amt") || table1.Field<double?>("Oc2_Per") != table2.Field<double?>("Oc2_Per") || table1.Field<double?>("Oc2_amt") != table2.Field<double?>("Oc2_amt") || table1.Field<double?>("Oc3_Per") != table2.Field<double?>("Oc3_Per") || table1.Field<double?>("Oc3_amt") != table2.Field<double?>("Oc3_amt") || table1.Field<double?>("Oc4_Per") != table2.Field<double?>("Oc4_Per") || table1.Field<double?>("Oc4_amt") != table2.Field<double?>("Oc4_amt") || table1.Field<double?>("Oc5_Per") != table2.Field<double?>("Oc5_Per") || table1.Field<double?>("Oc5_amt") != table2.Field<double?>("Oc5_amt") || table1.Field<double?>("Rounded") != table2.Field<double?>("Rounded") || table1.Field<double?>("Bill_amt") != table2.Field<double?>("Bill_amt") || table1.Field<string>("Agent") != table2.Field<string>("Agent") || table1.Field<double?>("Ag_Per") != table2.Field<double?>("Ag_Per") || table1.Field<double?>("Ag_Comm") != table2.Field<double?>("Ag_Comm") || table1.Field<Boolean>("Form_Req") != table2.Field<Boolean>("Form_Req") || table1.Field<string>("Form_Type") != table2.Field<string>("Form_Type") || table1.Field<string>("Form_No") != table2.Field<string>("Form_No") || table1.Field<DateTime?>("Form_Dt") != table2.Field<DateTime?>("Form_Dt") || table1.Field<double?>("Excise_Per") != table2.Field<double?>("Excise_Per") || table1.Field<double?>("Excise_Amt") != table2.Field<double?>("Excise_Amt") || table1.Field<double?>("SplExc_Per") != table2.Field<double?>("SplExc_Per") || table1.Field<double?>("SplExc_Amt") != table2.Field<double?>("SplExc_Amt") || table1.Field<double?>("At_Type") != table2.Field<double?>("At_Type") || table1.Field<double?>("At_Amt") != table2.Field<double?>("At_Amt") || table1.Field<string>("Svt_Type") != table2.Field<string>("Svt_Type") || table1.Field<double?>("Svt_On") != table2.Field<double?>("Svt_On") || table1.Field<double?>("Svt_Amt") != table2.Field<double?>("Svt_Amt") || table1.Field<double?>("Tot_Per") != table2.Field<double?>("Tot_Per") || table1.Field<double?>("Tot_Amt") != table2.Field<double?>("Tot_Amt") || table1.Field<double?>("Exempt") != table2.Field<double?>("Exempt") || table1.Field<string>("PSCode") != table2.Field<string>("PSCode") || table1.Field<string>("Narr1") != table2.Field<string>("Narr1") || table1.Field<string>("Narr2") != table2.Field<string>("Narr2") || table1.Field<string>("Narr3") != table2.Field<string>("Narr3") || table1.Field<Boolean>("TaxIncl") != table2.Field<Boolean>("TaxIncl") || table1.Field<string>("CBR") != table2.Field<string>("CBR") || table1.Field<string>("CBAC") != table2.Field<string>("CBAC") || table1.Field<string>("ChNo") != table2.Field<string>("ChNo") || table1.Field<DateTime?>("ChDt") != table2.Field<DateTime?>("ChDt") || table1.Field<string>("ChBank") != table2.Field<string>("ChBank") || table1.Field<DateTime?>("Ord_Dt") != table2.Field<DateTime?>("Ord_Dt") || table1.Field<double?>("DueDays") != table2.Field<double?>("DueDays") || table1.Field<string>("ChlnLink") != table2.Field<string>("ChlnLink") || table1.Field<double?>("Bal_Amt") != table2.Field<double?>("Bal_Amt") || table1.Field<DateTime?>("StockDt") != table2.Field<DateTime?>("StockDt") || table1.Field<string>("WayBillNo") != table2.Field<string>("WayBillNo") || table1.Field<DateTime?>("WayBillDt") != table2.Field<DateTime?>("WayBillDt")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable()
                                     on new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime?>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<string>("Ord_No"), ColG = table2.Field<double?>("It_Val"), ColH = table2.Field<double?>("Disc_per"), ColI = table2.Field<double?>("Disc_Amt"), ColJ = table2.Field<double?>("Sd_per"), ColK = table2.Field<double?>("Sd_Amt"), ColL = table2.Field<double?>("OC_Before"), ColM = table2.Field<double?>("Tax_Amt"), ColN = table2.Field<string>("St_Type"), ColO = table2.Field<double?>("St_Amt"), ColP = table2.Field<double?>("Oc1_Per"), ColQ = table2.Field<double?>("Oc1_amt"), ColR = table2.Field<double?>("Oc2_Per"), ColS = table2.Field<double?>("Oc2_amt"), ColT = table2.Field<double?>("Oc3_Per"), ColU = table2.Field<double?>("Oc3_amt"), ColV = table2.Field<double?>("Oc4_Per"), ColW = table2.Field<double?>("Oc4_amt"), ColX = table2.Field<double?>("Oc5_Per"), ColY = table2.Field<double?>("Oc5_amt"), ColZ = table2.Field<double?>("Rounded"), Col1 = table2.Field<double?>("Bill_amt"), Col2 = table2.Field<string>("Agent"), Col3 = table2.Field<double?>("Ag_Per"), Col4 = table2.Field<double?>("Ag_Comm"), Col5 = table2.Field<Boolean>("Form_Req"), Col6 = table2.Field<string>("Form_Type"), Col66 = table2.Field<string>("Form_No"), Col7 = table2.Field<DateTime?>("Form_Dt"), Col8 = table2.Field<double?>("Excise_Per"), Col9 = table2.Field<double?>("Excise_Amt"), Col10 = table2.Field<double?>("SplExc_Per"), Col11 = table2.Field<double?>("SplExc_Amt"), Col12 = table2.Field<double?>("At_Type"), Col122 = table2.Field<double?>("At_Amt"), Col13 = table2.Field<string>("Svt_Type"), Col14 = table2.Field<double?>("Svt_On"), Col15 = table2.Field<double?>("Svt_Amt"), Col16 = table2.Field<double?>("Tot_Per"), Col17 = table2.Field<double?>("Tot_Amt"), Col18 = table2.Field<double?>("Exempt"), Col19 = table2.Field<string>("PSCode"), Col20 = table2.Field<string>("Narr1"), Col21 = table2.Field<string>("Narr2"), Col22 = table2.Field<string>("Narr3"), Col23 = table2.Field<Boolean>("TaxIncl"), Col24 = table2.Field<string>("CBR"), Col25 = table2.Field<string>("CBAC"), Col26 = table2.Field<string>("ChNo"), Col27 = table2.Field<DateTime?>("ChDt"), Col28 = table2.Field<string>("ChBank"), Col29 = table2.Field<DateTime?>("Ord_Dt"), Col30 = table2.Field<double?>("DueDays"), Col31 = table2.Field<string>("ChlnLink"), Col32 = table2.Field<double?>("Bal_Amt"), Col33 = table2.Field<DateTime?>("StockDt"), Col34 = table2.Field<string>("WayBillNo"), Col35 = table2.Field<DateTime?>("WayBillDt") } equals new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime?>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<string>("Ord_No"), ColG = table1.Field<double?>("It_Val"), ColH = table1.Field<double?>("Disc_per"), ColI = table1.Field<double?>("Disc_Amt"), ColJ = table1.Field<double?>("Sd_per"), ColK = table1.Field<double?>("Sd_Amt"), ColL = table1.Field<double?>("OC_Before"), ColM = table1.Field<double?>("Tax_Amt"), ColN = table1.Field<string>("St_Type"), ColO = table1.Field<double?>("St_Amt"), ColP = table1.Field<double?>("Oc1_Per"), ColQ = table1.Field<double?>("Oc1_amt"), ColR = table1.Field<double?>("Oc2_Per"), ColS = table1.Field<double?>("Oc2_amt"), ColT = table1.Field<double?>("Oc3_Per"), ColU = table1.Field<double?>("Oc3_amt"), ColV = table1.Field<double?>("Oc4_Per"), ColW = table1.Field<double?>("Oc4_amt"), ColX = table1.Field<double?>("Oc5_Per"), ColY = table1.Field<double?>("Oc5_amt"), ColZ = table1.Field<double?>("Rounded"), Col1 = table1.Field<double?>("Bill_amt"), Col2 = table1.Field<string>("Agent"), Col3 = table1.Field<double?>("Ag_Per"), Col4 = table1.Field<double?>("Ag_Comm"), Col5 = table1.Field<Boolean>("Form_Req"), Col6 = table1.Field<string>("Form_Type"), Col66 = table1.Field<string>("Form_No"), Col7 = table1.Field<DateTime?>("Form_Dt"), Col8 = table1.Field<double?>("Excise_Per"), Col9 = table1.Field<double?>("Excise_Amt"), Col10 = table1.Field<double?>("SplExc_Per"), Col11 = table1.Field<double?>("SplExc_Amt"), Col12 = table1.Field<double?>("At_Type"), Col122 = table1.Field<double?>("At_Amt"), Col13 = table1.Field<string>("Svt_Type"), Col14 = table1.Field<double?>("Svt_On"), Col15 = table1.Field<double?>("Svt_Amt"), Col16 = table1.Field<double?>("Tot_Per"), Col17 = table1.Field<double?>("Tot_Amt"), Col18 = table1.Field<double?>("Exempt"), Col19 = table1.Field<string>("PSCode"), Col20 = table1.Field<string>("Narr1"), Col21 = table1.Field<string>("Narr2"), Col22 = table1.Field<string>("Narr3"), Col23 = table1.Field<Boolean>("TaxIncl"), Col24 = table1.Field<string>("CBR"), Col25 = table1.Field<string>("CBAC"), Col26 = table1.Field<string>("ChNo"), Col27 = table1.Field<DateTime?>("ChDt"), Col28 = table1.Field<string>("ChBank"), Col29 = table1.Field<DateTime?>("Ord_Dt"), Col30 = table1.Field<double?>("DueDays"), Col31 = table1.Field<string>("ChlnLink"), Col32 = table1.Field<double?>("Bal_Amt"), Col33 = table1.Field<DateTime?>("StockDt"), Col34 = table1.Field<string>("WayBillNo"), Col35 = table1.Field<DateTime?>("WayBillDt") }
                                     where table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<DateTime?>("Date") != table2.Field<DateTime?>("Date") || table1.Field<string>("DocNo") != table2.Field<string>("DocNo") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("Ord_No") != table2.Field<string>("Ord_No") || table1.Field<double?>("It_Val") != table2.Field<double?>("It_Val") || table1.Field<double?>("Disc_per") != table2.Field<double?>("Disc_per") || table1.Field<double?>("Disc_Amt") != table2.Field<double?>("Disc_Amt") || table1.Field<double?>("Sd_per") != table2.Field<double?>("Sd_per") || table1.Field<double?>("Sd_Amt") != table2.Field<double?>("Sd_Amt") || table1.Field<double?>("OC_Before") != table2.Field<double?>("OC_Before") || table1.Field<double?>("Tax_Amt") != table2.Field<double?>("Tax_Amt") || table1.Field<string>("St_Type") != table2.Field<string>("St_Type") || table1.Field<double?>("St_Amt") != table2.Field<double?>("St_Amt") || table1.Field<double?>("Oc1_Per") != table2.Field<double?>("Oc1_Per") || table1.Field<double?>("Oc1_amt") != table2.Field<double?>("Oc1_amt") || table1.Field<double?>("Oc2_Per") != table2.Field<double?>("Oc2_Per") || table1.Field<double?>("Oc2_amt") != table2.Field<double?>("Oc2_amt") || table1.Field<double?>("Oc3_Per") != table2.Field<double?>("Oc3_Per") || table1.Field<double?>("Oc3_amt") != table2.Field<double?>("Oc3_amt") || table1.Field<double?>("Oc4_Per") != table2.Field<double?>("Oc4_Per") || table1.Field<double?>("Oc4_amt") != table2.Field<double?>("Oc4_amt") || table1.Field<double?>("Oc5_Per") != table2.Field<double?>("Oc5_Per") || table1.Field<double?>("Oc5_amt") != table2.Field<double?>("Oc5_amt") || table1.Field<double?>("Rounded") != table2.Field<double?>("Rounded") || table1.Field<double?>("Bill_amt") != table2.Field<double?>("Bill_amt") || table1.Field<string>("Agent") != table2.Field<string>("Agent") || table1.Field<double?>("Ag_Per") != table2.Field<double?>("Ag_Per") || table1.Field<double?>("Ag_Comm") != table2.Field<double?>("Ag_Comm") || table1.Field<Boolean>("Form_Req") != table2.Field<Boolean>("Form_Req") || table1.Field<string>("Form_Type") != table2.Field<string>("Form_Type") || table1.Field<string>("Form_No") != table2.Field<string>("Form_No") || table1.Field<DateTime?>("Form_Dt") != table2.Field<DateTime?>("Form_Dt") || table1.Field<double?>("Excise_Per") != table2.Field<double?>("Excise_Per") || table1.Field<double?>("Excise_Amt") != table2.Field<double?>("Excise_Amt") || table1.Field<double?>("SplExc_Per") != table2.Field<double?>("SplExc_Per") || table1.Field<double?>("SplExc_Amt") != table2.Field<double?>("SplExc_Amt") || table1.Field<double?>("At_Type") != table2.Field<double?>("At_Type") || table1.Field<double?>("At_Amt") != table2.Field<double?>("At_Amt") || table1.Field<string>("Svt_Type") != table2.Field<string>("Svt_Type") || table1.Field<double?>("Svt_On") != table2.Field<double?>("Svt_On") || table1.Field<double?>("Svt_Amt") != table2.Field<double?>("Svt_Amt") || table1.Field<double?>("Tot_Per") != table2.Field<double?>("Tot_Per") || table1.Field<double?>("Tot_Amt") != table2.Field<double?>("Tot_Amt") || table1.Field<double?>("Exempt") != table2.Field<double?>("Exempt") || table1.Field<string>("PSCode") != table2.Field<string>("PSCode") || table1.Field<string>("Narr1") != table2.Field<string>("Narr1") || table1.Field<string>("Narr2") != table2.Field<string>("Narr2") || table1.Field<string>("Narr3") != table2.Field<string>("Narr3") || table1.Field<Boolean>("TaxIncl") != table2.Field<Boolean>("TaxIncl") || table1.Field<string>("CBR") != table2.Field<string>("CBR") || table1.Field<string>("CBAC") != table2.Field<string>("CBAC") || table1.Field<string>("ChNo") != table2.Field<string>("ChNo") || table1.Field<DateTime?>("ChDt") != table2.Field<DateTime?>("ChDt") || table1.Field<string>("ChBank") != table2.Field<string>("ChBank") || table1.Field<DateTime?>("Ord_Dt") != table2.Field<DateTime?>("Ord_Dt") || table1.Field<double?>("DueDays") != table2.Field<double?>("DueDays") || table1.Field<string>("ChlnLink") != table2.Field<string>("ChlnLink") || table1.Field<double?>("Bal_Amt") != table2.Field<double?>("Bal_Amt") || table1.Field<DateTime?>("StockDt") != table2.Field<DateTime?>("StockDt") || table1.Field<string>("WayBillNo") != table2.Field<string>("WayBillNo") || table1.Field<DateTime?>("WayBillDt") != table2.Field<DateTime?>("WayBillDt")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime?>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<string>("Ord_No"), ColG = table2.Field<double?>("It_Val"), ColH = table2.Field<double?>("Disc_per"), ColI = table2.Field<double?>("Disc_Amt"), ColJ = table2.Field<double?>("Sd_per"), ColK = table2.Field<double?>("Sd_Amt"), ColL = table2.Field<double?>("OC_Before"), ColM = table2.Field<double?>("Tax_Amt"), ColN = table2.Field<string>("St_Type"), ColO = table2.Field<double?>("St_Amt"), ColP = table2.Field<double?>("Oc1_Per"), ColQ = table2.Field<double?>("Oc1_amt"), ColR = table2.Field<double?>("Oc2_Per"), ColS = table2.Field<double?>("Oc2_amt"), ColT = table2.Field<double?>("Oc3_Per"), ColU = table2.Field<double?>("Oc3_amt"), ColV = table2.Field<double?>("Oc4_Per"), ColW = table2.Field<double?>("Oc4_amt"), ColX = table2.Field<double?>("Oc5_Per"), ColY = table2.Field<double?>("Oc5_amt"), ColZ = table2.Field<double?>("Rounded"), Col1 = table2.Field<double?>("Bill_amt"), Col2 = table2.Field<string>("Agent"), Col3 = table2.Field<double?>("Ag_Per"), Col4 = table2.Field<double?>("Ag_Comm"), Col5 = table2.Field<Boolean>("Form_Req"), Col6 = table2.Field<string>("Form_Type"), Col66 = table2.Field<string>("Form_No"), Col7 = table2.Field<DateTime?>("Form_Dt"), Col8 = table2.Field<double?>("Excise_Per"), Col9 = table2.Field<double?>("Excise_Amt"), Col10 = table2.Field<double?>("SplExc_Per"), Col11 = table2.Field<double?>("SplExc_Amt"), Col12 = table2.Field<double?>("At_Type"), Col122 = table2.Field<double?>("At_Amt"), Col13 = table2.Field<string>("Svt_Type"), Col14 = table2.Field<double?>("Svt_On"), Col15 = table2.Field<double?>("Svt_Amt"), Col16 = table2.Field<double?>("Tot_Per"), Col17 = table2.Field<double?>("Tot_Amt"), Col18 = table2.Field<double?>("Exempt"), Col19 = table2.Field<string>("PSCode"), Col20 = table2.Field<string>("Narr1"), Col21 = table2.Field<string>("Narr2"), Col22 = table2.Field<string>("Narr3"), Col23 = table2.Field<Boolean>("TaxIncl"), Col24 = table2.Field<string>("CBR"), Col25 = table2.Field<string>("CBAC"), Col26 = table2.Field<string>("ChNo"), Col27 = table2.Field<DateTime?>("ChDt"), Col28 = table2.Field<string>("ChBank"), Col29 = table2.Field<DateTime?>("Ord_Dt"), Col30 = table2.Field<double?>("DueDays"), Col31 = table2.Field<string>("ChlnLink"), Col32 = table2.Field<double?>("Bal_Amt"), Col33 = table2.Field<DateTime?>("StockDt"), Col34 = table2.Field<string>("WayBillNo"), Col35 = table2.Field<DateTime?>("WayBillDt") } equals new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime?>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<string>("Ord_No"), ColG = table1.Field<double?>("It_Val"), ColH = table1.Field<double?>("Disc_per"), ColI = table1.Field<double?>("Disc_Amt"), ColJ = table1.Field<double?>("Sd_per"), ColK = table1.Field<double?>("Sd_Amt"), ColL = table1.Field<double?>("OC_Before"), ColM = table1.Field<double?>("Tax_Amt"), ColN = table1.Field<string>("St_Type"), ColO = table1.Field<double?>("St_Amt"), ColP = table1.Field<double?>("Oc1_Per"), ColQ = table1.Field<double?>("Oc1_amt"), ColR = table1.Field<double?>("Oc2_Per"), ColS = table1.Field<double?>("Oc2_amt"), ColT = table1.Field<double?>("Oc3_Per"), ColU = table1.Field<double?>("Oc3_amt"), ColV = table1.Field<double?>("Oc4_Per"), ColW = table1.Field<double?>("Oc4_amt"), ColX = table1.Field<double?>("Oc5_Per"), ColY = table1.Field<double?>("Oc5_amt"), ColZ = table1.Field<double?>("Rounded"), Col1 = table1.Field<double?>("Bill_amt"), Col2 = table1.Field<string>("Agent"), Col3 = table1.Field<double?>("Ag_Per"), Col4 = table1.Field<double?>("Ag_Comm"), Col5 = table1.Field<Boolean>("Form_Req"), Col6 = table1.Field<string>("Form_Type"), Col66 = table1.Field<string>("Form_No"), Col7 = table1.Field<DateTime?>("Form_Dt"), Col8 = table1.Field<double?>("Excise_Per"), Col9 = table1.Field<double?>("Excise_Amt"), Col10 = table1.Field<double?>("SplExc_Per"), Col11 = table1.Field<double?>("SplExc_Amt"), Col12 = table1.Field<double?>("At_Type"), Col122 = table1.Field<double?>("At_Amt"), Col13 = table1.Field<string>("Svt_Type"), Col14 = table1.Field<double?>("Svt_On"), Col15 = table1.Field<double?>("Svt_Amt"), Col16 = table1.Field<double?>("Tot_Per"), Col17 = table1.Field<double?>("Tot_Amt"), Col18 = table1.Field<double?>("Exempt"), Col19 = table1.Field<string>("PSCode"), Col20 = table1.Field<string>("Narr1"), Col21 = table1.Field<string>("Narr2"), Col22 = table1.Field<string>("Narr3"), Col23 = table1.Field<Boolean>("TaxIncl"), Col24 = table1.Field<string>("CBR"), Col25 = table1.Field<string>("CBAC"), Col26 = table1.Field<string>("ChNo"), Col27 = table1.Field<DateTime?>("ChDt"), Col28 = table1.Field<string>("ChBank"), Col29 = table1.Field<DateTime?>("Ord_Dt"), Col30 = table1.Field<double?>("DueDays"), Col31 = table1.Field<string>("ChlnLink"), Col32 = table1.Field<double?>("Bal_Amt"), Col33 = table1.Field<DateTime?>("StockDt"), Col34 = table1.Field<string>("WayBillNo"), Col35 = table1.Field<DateTime?>("WayBillDt") } into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on new { ColA = table1.Field<string>("Link"), ColB = table1.Field<string>("Division"), ColC = table1.Field<DateTime?>("Date"), ColD = table1.Field<string>("DocNo"), ColE = table1.Field<string>("Code"), ColF = table1.Field<string>("Ord_No"), ColG = table1.Field<double?>("It_Val"), ColH = table1.Field<double?>("Disc_per"), ColI = table1.Field<double?>("Disc_Amt"), ColJ = table1.Field<double?>("Sd_per"), ColK = table1.Field<double?>("Sd_Amt"), ColL = table1.Field<double?>("OC_Before"), ColM = table1.Field<double?>("Tax_Amt"), ColN = table1.Field<string>("St_Type"), ColO = table1.Field<double?>("St_Amt"), ColP = table1.Field<double?>("Oc1_Per"), ColQ = table1.Field<double?>("Oc1_amt"), ColR = table1.Field<double?>("Oc2_Per"), ColS = table1.Field<double?>("Oc2_amt"), ColT = table1.Field<double?>("Oc3_Per"), ColU = table1.Field<double?>("Oc3_amt"), ColV = table1.Field<double?>("Oc4_Per"), ColW = table1.Field<double?>("Oc4_amt"), ColX = table1.Field<double?>("Oc5_Per"), ColY = table1.Field<double?>("Oc5_amt"), ColZ = table1.Field<double?>("Rounded"), Col1 = table1.Field<double?>("Bill_amt"), Col2 = table1.Field<string>("Agent"), Col3 = table1.Field<double?>("Ag_Per"), Col4 = table1.Field<double?>("Ag_Comm"), Col5 = table1.Field<Boolean>("Form_Req"), Col6 = table1.Field<string>("Form_Type"), Col66 = table1.Field<string>("Form_No"), Col7 = table1.Field<DateTime?>("Form_Dt"), Col8 = table1.Field<double?>("Excise_Per"), Col9 = table1.Field<double?>("Excise_Amt"), Col10 = table1.Field<double?>("SplExc_Per"), Col11 = table1.Field<double?>("SplExc_Amt"), Col12 = table1.Field<double?>("At_Type"), Col122 = table1.Field<double?>("At_Amt"), Col13 = table1.Field<string>("Svt_Type"), Col14 = table1.Field<double?>("Svt_On"), Col15 = table1.Field<double?>("Svt_Amt"), Col16 = table1.Field<double?>("Tot_Per"), Col17 = table1.Field<double?>("Tot_Amt"), Col18 = table1.Field<double?>("Exempt"), Col19 = table1.Field<string>("PSCode"), Col20 = table1.Field<string>("Narr1"), Col21 = table1.Field<string>("Narr2"), Col22 = table1.Field<string>("Narr3"), Col23 = table1.Field<Boolean>("TaxIncl"), Col24 = table1.Field<string>("CBR"), Col25 = table1.Field<string>("CBAC"), Col26 = table1.Field<string>("ChNo"), Col27 = table1.Field<DateTime?>("ChDt"), Col28 = table1.Field<string>("ChBank"), Col29 = table1.Field<DateTime?>("Ord_Dt"), Col30 = table1.Field<double?>("DueDays"), Col31 = table1.Field<string>("ChlnLink"), Col32 = table1.Field<double?>("Bal_Amt"), Col33 = table1.Field<DateTime?>("StockDt"), Col34 = table1.Field<string>("WayBillNo"), Col35 = table1.Field<DateTime?>("WayBillDt") } equals new { ColA = table2.Field<string>("Link"), ColB = table2.Field<string>("Division"), ColC = table2.Field<DateTime?>("Date"), ColD = table2.Field<string>("DocNo"), ColE = table2.Field<string>("Code"), ColF = table2.Field<string>("Ord_No"), ColG = table2.Field<double?>("It_Val"), ColH = table2.Field<double?>("Disc_per"), ColI = table2.Field<double?>("Disc_Amt"), ColJ = table2.Field<double?>("Sd_per"), ColK = table2.Field<double?>("Sd_Amt"), ColL = table2.Field<double?>("OC_Before"), ColM = table2.Field<double?>("Tax_Amt"), ColN = table2.Field<string>("St_Type"), ColO = table2.Field<double?>("St_Amt"), ColP = table2.Field<double?>("Oc1_Per"), ColQ = table2.Field<double?>("Oc1_amt"), ColR = table2.Field<double?>("Oc2_Per"), ColS = table2.Field<double?>("Oc2_amt"), ColT = table2.Field<double?>("Oc3_Per"), ColU = table2.Field<double?>("Oc3_amt"), ColV = table2.Field<double?>("Oc4_Per"), ColW = table2.Field<double?>("Oc4_amt"), ColX = table2.Field<double?>("Oc5_Per"), ColY = table2.Field<double?>("Oc5_amt"), ColZ = table2.Field<double?>("Rounded"), Col1 = table2.Field<double?>("Bill_amt"), Col2 = table2.Field<string>("Agent"), Col3 = table2.Field<double?>("Ag_Per"), Col4 = table2.Field<double?>("Ag_Comm"), Col5 = table2.Field<Boolean>("Form_Req"), Col6 = table2.Field<string>("Form_Type"), Col66 = table2.Field<string>("Form_No"), Col7 = table2.Field<DateTime?>("Form_Dt"), Col8 = table2.Field<double?>("Excise_Per"), Col9 = table2.Field<double?>("Excise_Amt"), Col10 = table2.Field<double?>("SplExc_Per"), Col11 = table2.Field<double?>("SplExc_Amt"), Col12 = table2.Field<double?>("At_Type"), Col122 = table2.Field<double?>("At_Amt"), Col13 = table2.Field<string>("Svt_Type"), Col14 = table2.Field<double?>("Svt_On"), Col15 = table2.Field<double?>("Svt_Amt"), Col16 = table2.Field<double?>("Tot_Per"), Col17 = table2.Field<double?>("Tot_Amt"), Col18 = table2.Field<double?>("Exempt"), Col19 = table2.Field<string>("PSCode"), Col20 = table2.Field<string>("Narr1"), Col21 = table2.Field<string>("Narr2"), Col22 = table2.Field<string>("Narr3"), Col23 = table2.Field<Boolean>("TaxIncl"), Col24 = table2.Field<string>("CBR"), Col25 = table2.Field<string>("CBAC"), Col26 = table2.Field<string>("ChNo"), Col27 = table2.Field<DateTime?>("ChDt"), Col28 = table2.Field<string>("ChBank"), Col29 = table2.Field<DateTime?>("Ord_Dt"), Col30 = table2.Field<double?>("DueDays"), Col31 = table2.Field<string>("ChlnLink"), Col32 = table2.Field<double?>("Bal_Amt"), Col33 = table2.Field<DateTime?>("StockDt"), Col34 = table2.Field<string>("WayBillNo"), Col35 = table2.Field<DateTime?>("WayBillDt") } into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                    #region STOCK
                    case TableNames.STOCK:
                        changed = (from table1 in dtLocal.AsEnumerable()
                                   join table2 in dtExpert.AsEnumerable()
                                        on new { Col1 = table1.Field<string>("Link"), Col2 = table1.Field<string>("Vtype"), Col3 = table1.Field<string>("Code"), Col4 = table1.Field<string>("BatchNo"), Col5 = table1.Field<double?>("Qty"), Col6 = table1.Field<double?>("Stk_Qty"), Col7 = table1.Field<string>("Unit"), Col8 = table1.Field<string>("DocNo"), Col9 = table1.Field<DateTime?>("Date"), Col10 = table1.Field<string>("Cv_Code"), Col11 = table1.Field<string>("Division"), Col12 = table1.Field<string>("Godown"), Col13 = table1.Field<string>("Ord_no"), Col14 = table1.Field<double?>("Rate"), Col15 = table1.Field<double?>("Value"), Col16 = table1.Field<string>("Narr1"), Col17 = table1.Field<string>("Narr2"), Col18 = table1.Field<string>("PSCode"), Col19 = table1.Field<string>("AL"), Col20 = table1.Field<string>("TrackNo"), Col21 = table1.Field<string>("ChlnLink"), Col22 = table1.Field<double?>("It_Disc"), Col23 = table1.Field<double?>("It_Tax"), Col24 = table1.Field<double?>("It_Oc"), Col25 = table1.Field<string>("RtUnit"), Col26 = table1.Field<Boolean>("Free"), Col27 = table1.Field<double?>("Packages"), Col28 = table1.Field<double?>("QFree"), Col29 = table1.Field<string>("PosiFile"), Col30 = table1.Field<string>("VatCode") } equals new { Col1 = table2.Field<string>("Link"), Col2 = table2.Field<string>("Vtype"), Col3 = table2.Field<string>("Code"), Col4 = table2.Field<string>("BatchNo"), Col5 = table2.Field<double?>("Qty"), Col6 = table2.Field<double?>("Stk_Qty"), Col7 = table2.Field<string>("Unit"), Col8 = table2.Field<string>("DocNo"), Col9 = table2.Field<DateTime?>("Date"), Col10 = table2.Field<string>("Cv_Code"), Col11 = table2.Field<string>("Division"), Col12 = table2.Field<string>("Godown"), Col13 = table2.Field<string>("Ord_no"), Col14 = table2.Field<double?>("Rate"), Col15 = table2.Field<double?>("Value"), Col16 = table2.Field<string>("Narr1"), Col17 = table2.Field<string>("Narr2"), Col18 = table2.Field<string>("PSCode"), Col19 = table2.Field<string>("AL"), Col20 = table2.Field<string>("TrackNo"), Col21 = table2.Field<string>("ChlnLink"), Col22 = table2.Field<double?>("It_Disc"), Col23 = table2.Field<double?>("It_Tax"), Col24 = table2.Field<double?>("It_Oc"), Col25 = table2.Field<string>("RtUnit"), Col26 = table2.Field<Boolean>("Free"), Col27 = table2.Field<double?>("Packages"), Col28 = table2.Field<double?>("QFree"), Col29 = table2.Field<string>("PosiFile"), Col30 = table2.Field<string>("VatCode") }
                                   where table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("Vtype") != table2.Field<string>("Vtype") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("BatchNo") != table2.Field<string>("BatchNo") || table1.Field<double?>("Qty") != table2.Field<double?>("Qty") || table1.Field<double?>("Stk_Qty") != table2.Field<double?>("Stk_Qty") || table1.Field<string>("Unit") != table2.Field<string>("Unit") || table1.Field<string>("DocNo") != table2.Field<string>("DocNo") || table1.Field<DateTime?>("Date") != table2.Field<DateTime?>("Date") || table1.Field<string>("Cv_Code") != table2.Field<string>("Cv_Code") || table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<string>("Godown") != table2.Field<string>("Godown") || table1.Field<string>("Ord_no") != table2.Field<string>("Ord_no") || table1.Field<double?>("Rate") != table2.Field<double?>("Rate") || table1.Field<double?>("Value") != table2.Field<double?>("Value") || table1.Field<string>("Narr1") != table2.Field<string>("Narr1") || table1.Field<string>("Narr2") != table2.Field<string>("Narr2") || table1.Field<string>("PSCode") != table2.Field<string>("PSCode") || table1.Field<string>("AL") != table2.Field<string>("AL") || table1.Field<string>("TrackNo") != table2.Field<string>("TrackNo") || table1.Field<string>("ChlnLink") != table2.Field<string>("ChlnLink") || table1.Field<double?>("It_Disc") != table2.Field<double?>("It_Disc") || table1.Field<double?>("It_Tax") != table2.Field<double?>("It_Tax") || table1.Field<double?>("It_Oc") != table2.Field<double?>("It_Oc") || table1.Field<string>("RtUnit") != table2.Field<string>("RtUnit") || table1.Field<Boolean>("Free") != table2.Field<Boolean>("Free") || table1.Field<double?>("Packages") != table2.Field<double?>("Packages") || table1.Field<double?>("QFree") != table2.Field<double?>("QFree") || table1.Field<string>("PosiFile") != table2.Field<string>("PosiFile") || table1.Field<string>("VatCode") != table2.Field<string>("VatCode")
                                   select table1);

                        if (changed != null && changed.Count() > 0)
                            dtModifiedRecord = changed.CopyToDataTable();

                        unchanged = (from table2 in dtExpert.AsEnumerable()
                                     join table1 in dtLocal.AsEnumerable()
                                        on new { Col1 = table2.Field<string>("Link"), Col2 = table2.Field<string>("Vtype"), Col3 = table2.Field<string>("Code"), Col4 = table2.Field<string>("BatchNo"), Col5 = table2.Field<double?>("Qty"), Col6 = table2.Field<double?>("Stk_Qty"), Col7 = table2.Field<string>("Unit"), Col8 = table2.Field<string>("DocNo"), Col9 = table2.Field<DateTime?>("Date"), Col10 = table2.Field<string>("Cv_Code"), Col11 = table2.Field<string>("Division"), Col12 = table2.Field<string>("Godown"), Col13 = table2.Field<string>("Ord_no"), Col14 = table2.Field<double?>("Rate"), Col15 = table2.Field<double?>("Value"), Col16 = table2.Field<string>("Narr1"), Col17 = table2.Field<string>("Narr2"), Col18 = table2.Field<string>("PSCode"), Col19 = table2.Field<string>("AL"), Col20 = table2.Field<string>("TrackNo"), Col21 = table2.Field<string>("ChlnLink"), Col22 = table2.Field<double?>("It_Disc"), Col23 = table2.Field<double?>("It_Tax"), Col24 = table2.Field<double?>("It_Oc"), Col25 = table2.Field<string>("RtUnit"), Col26 = table2.Field<Boolean>("Free"), Col27 = table2.Field<double?>("Packages"), Col28 = table2.Field<double?>("QFree"), Col29 = table2.Field<string>("PosiFile"), Col30 = table2.Field<string>("VatCode") } equals new { Col1 = table1.Field<string>("Link"), Col2 = table1.Field<string>("Vtype"), Col3 = table1.Field<string>("Code"), Col4 = table1.Field<string>("BatchNo"), Col5 = table1.Field<double?>("Qty"), Col6 = table1.Field<double?>("Stk_Qty"), Col7 = table1.Field<string>("Unit"), Col8 = table1.Field<string>("DocNo"), Col9 = table1.Field<DateTime?>("Date"), Col10 = table1.Field<string>("Cv_Code"), Col11 = table1.Field<string>("Division"), Col12 = table1.Field<string>("Godown"), Col13 = table1.Field<string>("Ord_no"), Col14 = table1.Field<double?>("Rate"), Col15 = table1.Field<double?>("Value"), Col16 = table1.Field<string>("Narr1"), Col17 = table1.Field<string>("Narr2"), Col18 = table1.Field<string>("PSCode"), Col19 = table1.Field<string>("AL"), Col20 = table1.Field<string>("TrackNo"), Col21 = table1.Field<string>("ChlnLink"), Col22 = table1.Field<double?>("It_Disc"), Col23 = table1.Field<double?>("It_Tax"), Col24 = table1.Field<double?>("It_Oc"), Col25 = table1.Field<string>("RtUnit"), Col26 = table1.Field<Boolean>("Free"), Col27 = table1.Field<double?>("Packages"), Col28 = table1.Field<double?>("QFree"), Col29 = table1.Field<string>("PosiFile"), Col30 = table1.Field<string>("VatCode") }
                                     where table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("Vtype") != table2.Field<string>("Vtype") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("BatchNo") != table2.Field<string>("BatchNo") || table1.Field<double?>("Qty") != table2.Field<double?>("Qty") || table1.Field<double?>("Stk_Qty") != table2.Field<double?>("Stk_Qty") || table1.Field<string>("Unit") != table2.Field<string>("Unit") || table1.Field<string>("DocNo") != table2.Field<string>("DocNo") || table1.Field<DateTime?>("Date") != table2.Field<DateTime?>("Date") || table1.Field<string>("Cv_Code") != table2.Field<string>("Cv_Code") || table1.Field<string>("Division") != table2.Field<string>("Division") || table1.Field<string>("Godown") != table2.Field<string>("Godown") || table1.Field<string>("Ord_no") != table2.Field<string>("Ord_no") || table1.Field<double?>("Rate") != table2.Field<double?>("Rate") || table1.Field<double?>("Value") != table2.Field<double?>("Value") || table1.Field<string>("Narr1") != table2.Field<string>("Narr1") || table1.Field<string>("Narr2") != table2.Field<string>("Narr2") || table1.Field<string>("PSCode") != table2.Field<string>("PSCode") || table1.Field<string>("AL") != table2.Field<string>("AL") || table1.Field<string>("TrackNo") != table2.Field<string>("TrackNo") || table1.Field<string>("ChlnLink") != table2.Field<string>("ChlnLink") || table1.Field<double?>("It_Disc") != table2.Field<double?>("It_Disc") || table1.Field<double?>("It_Tax") != table2.Field<double?>("It_Tax") || table1.Field<double?>("It_Oc") != table2.Field<double?>("It_Oc") || table1.Field<string>("RtUnit") != table2.Field<string>("RtUnit") || table1.Field<Boolean>("Free") != table2.Field<Boolean>("Free") || table1.Field<double?>("Packages") != table2.Field<double?>("Packages") || table1.Field<double?>("QFree") != table2.Field<double?>("QFree") || table1.Field<string>("PosiFile") != table2.Field<string>("PosiFile") || table1.Field<string>("VatCode") != table2.Field<string>("VatCode")
                                     select table2);

                        if (unchanged != null && unchanged.Count() > 0)
                            dtUnModifiedRecord = unchanged.CopyToDataTable();


                        newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                           join table1 in dtLocal.AsEnumerable()
                                           on new { Col1 = table2.Field<string>("Link"), Col2 = table2.Field<string>("Vtype"), Col3 = table2.Field<string>("Code"), Col4 = table2.Field<string>("BatchNo"), Col5 = table2.Field<double?>("Qty"), Col6 = table2.Field<double?>("Stk_Qty"), Col7 = table2.Field<string>("Unit"), Col8 = table2.Field<string>("DocNo"), Col9 = table2.Field<DateTime?>("Date"), Col10 = table2.Field<string>("Cv_Code"), Col11 = table2.Field<string>("Division"), Col12 = table2.Field<string>("Godown"), Col13 = table2.Field<string>("Ord_no"), Col14 = table2.Field<double?>("Rate"), Col15 = table2.Field<double?>("Value"), Col16 = table2.Field<string>("Narr1"), Col17 = table2.Field<string>("Narr2"), Col18 = table2.Field<string>("PSCode"), Col19 = table2.Field<string>("AL"), Col20 = table2.Field<string>("TrackNo"), Col21 = table2.Field<string>("ChlnLink"), Col22 = table2.Field<double?>("It_Disc"), Col23 = table2.Field<double?>("It_Tax"), Col24 = table2.Field<double?>("It_Oc"), Col25 = table2.Field<string>("RtUnit"), Col26 = table2.Field<Boolean>("Free"), Col27 = table2.Field<double?>("Packages"), Col28 = table2.Field<double?>("QFree"), Col29 = table2.Field<string>("PosiFile"), Col30 = table2.Field<string>("VatCode") } equals new { Col1 = table1.Field<string>("Link"), Col2 = table1.Field<string>("Vtype"), Col3 = table1.Field<string>("Code"), Col4 = table1.Field<string>("BatchNo"), Col5 = table1.Field<double?>("Qty"), Col6 = table1.Field<double?>("Stk_Qty"), Col7 = table1.Field<string>("Unit"), Col8 = table1.Field<string>("DocNo"), Col9 = table1.Field<DateTime?>("Date"), Col10 = table1.Field<string>("Cv_Code"), Col11 = table1.Field<string>("Division"), Col12 = table1.Field<string>("Godown"), Col13 = table1.Field<string>("Ord_no"), Col14 = table1.Field<double?>("Rate"), Col15 = table1.Field<double?>("Value"), Col16 = table1.Field<string>("Narr1"), Col17 = table1.Field<string>("Narr2"), Col18 = table1.Field<string>("PSCode"), Col19 = table1.Field<string>("AL"), Col20 = table1.Field<string>("TrackNo"), Col21 = table1.Field<string>("ChlnLink"), Col22 = table1.Field<double?>("It_Disc"), Col23 = table1.Field<double?>("It_Tax"), Col24 = table1.Field<double?>("It_Oc"), Col25 = table1.Field<string>("RtUnit"), Col26 = table1.Field<Boolean>("Free"), Col27 = table1.Field<double?>("Packages"), Col28 = table1.Field<double?>("QFree"), Col29 = table1.Field<string>("PosiFile"), Col30 = table1.Field<string>("VatCode") } into tg
                                           from tcheck in tg.DefaultIfEmpty()
                                           where tcheck == null
                                           select table2;

                        if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                            dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                        deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                          join table2 in dtExpert.AsEnumerable()
                                          on new { Col1 = table1.Field<string>("Link"), Col2 = table1.Field<string>("Vtype"), Col3 = table1.Field<string>("Code"), Col4 = table1.Field<string>("BatchNo"), Col5 = table1.Field<double?>("Qty"), Col6 = table1.Field<double?>("Stk_Qty"), Col7 = table1.Field<string>("Unit"), Col8 = table1.Field<string>("DocNo"), Col9 = table1.Field<DateTime?>("Date"), Col10 = table1.Field<string>("Cv_Code"), Col11 = table1.Field<string>("Division"), Col12 = table1.Field<string>("Godown"), Col13 = table1.Field<string>("Ord_no"), Col14 = table1.Field<double?>("Rate"), Col15 = table1.Field<double?>("Value"), Col16 = table1.Field<string>("Narr1"), Col17 = table1.Field<string>("Narr2"), Col18 = table1.Field<string>("PSCode"), Col19 = table1.Field<string>("AL"), Col20 = table1.Field<string>("TrackNo"), Col21 = table1.Field<string>("ChlnLink"), Col22 = table1.Field<double?>("It_Disc"), Col23 = table1.Field<double?>("It_Tax"), Col24 = table1.Field<double?>("It_Oc"), Col25 = table1.Field<string>("RtUnit"), Col26 = table1.Field<Boolean>("Free"), Col27 = table1.Field<double?>("Packages"), Col28 = table1.Field<double?>("QFree"), Col29 = table1.Field<string>("PosiFile"), Col30 = table1.Field<string>("VatCode") } equals new { Col1 = table2.Field<string>("Link"), Col2 = table2.Field<string>("Vtype"), Col3 = table2.Field<string>("Code"), Col4 = table2.Field<string>("BatchNo"), Col5 = table2.Field<double?>("Qty"), Col6 = table2.Field<double?>("Stk_Qty"), Col7 = table2.Field<string>("Unit"), Col8 = table2.Field<string>("DocNo"), Col9 = table2.Field<DateTime?>("Date"), Col10 = table2.Field<string>("Cv_Code"), Col11 = table2.Field<string>("Division"), Col12 = table2.Field<string>("Godown"), Col13 = table2.Field<string>("Ord_no"), Col14 = table2.Field<double?>("Rate"), Col15 = table2.Field<double?>("Value"), Col16 = table2.Field<string>("Narr1"), Col17 = table2.Field<string>("Narr2"), Col18 = table2.Field<string>("PSCode"), Col19 = table2.Field<string>("AL"), Col20 = table2.Field<string>("TrackNo"), Col21 = table2.Field<string>("ChlnLink"), Col22 = table2.Field<double?>("It_Disc"), Col23 = table2.Field<double?>("It_Tax"), Col24 = table2.Field<double?>("It_Oc"), Col25 = table2.Field<string>("RtUnit"), Col26 = table2.Field<Boolean>("Free"), Col27 = table2.Field<double?>("Packages"), Col28 = table2.Field<double?>("QFree"), Col29 = table2.Field<string>("PosiFile"), Col30 = table2.Field<string>("VatCode") } into tg
                                          from tcheck in tg.DefaultIfEmpty()
                                          where tcheck == null
                                          select table1;
                        if (deletefromlocal != null && deletefromlocal.Count() > 0)
                            dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {

            }
        }
        private void tNextUploadStatus_Tick(object sender, EventArgs e)
        {
            tNextUploadStatus.Enabled = false;
            if (string.IsNullOrEmpty(Operation.TimeInterval))
                Operation.GetIniValue();
            string[] TimeArray = Operation.TimeInterval.Split(',');
            var currentTime = DateTime.Now;
            List<TimeSpan> greaterDatesList = new List<TimeSpan>();
            TimeSpan[] greaterDates;
            TimeSpan finalTime = new TimeSpan();
            foreach (var time in TimeArray)
            {
                var diff = DateTime.Now - Convert.ToDateTime(time);
                if (diff.TotalMilliseconds < 0)
                {
                    greaterDatesList.Add(diff);
                }
            }
            if (greaterDatesList.Count > 0)
            {
                greaterDates = greaterDatesList.ToArray();
                Array.Sort(greaterDates, new Comparison<TimeSpan>(
                            (i1, i2) => i2.CompareTo(i1)
                    ));
                finalTime = greaterDates[0];
            }

            if (finalTime.TotalMilliseconds < 0)
            {
                toolNextUpload.Text = "Next Upload will start in : " + (finalTime.Hours > 0 ? finalTime.Hours : (-1 * finalTime.Hours)) + ":" +
                    (finalTime.Minutes > 0 ? finalTime.Minutes : (-1 * finalTime.Minutes)) + ":" +
                    (finalTime.Seconds > 0 ? finalTime.Seconds : (-1 * finalTime.Seconds));
            }

            tNextUploadStatus.Enabled = true;
        }
    }
}
