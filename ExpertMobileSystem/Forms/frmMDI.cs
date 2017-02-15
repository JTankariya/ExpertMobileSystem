using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;



namespace ExpertMobileSystem
{
    public partial class frmMDI : Form
    {
        List<String> Images = new List<string>();
        int ImageCount = 0;
        bool CloseFlag = false;
        public static int stmt_flag = 0;
        public static int inward_flag = 0;
        public frmMDI()
        {
            InitializeComponent();
            try
            {
                this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\Images\\Reminder.ico");
                Images = GetImagesPath();
             //   this.BackgroundImageLayout = (ImageLayout)Enum.Parse(typeof(ImageLayout), obj.PicturePosition);
                //this.tBackGround.Interval = Convert.ToInt32(obj.PictureChangeTime);
                this.BackgroundImage = Image.FromFile(Images[0]);
                ImageCount++;

            }
            catch
            {

            }


        }
        public frmMDI(bool flag)
        {
            InitializeComponent();
            try
            {
                this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\Images\\Reminder.ico");
                Images = GetImagesPath();
              //  this.BackgroundImageLayout = (ImageLayout)Enum.Parse(typeof(ImageLayout), obj.PicturePosition);
                //this.tBackGround.Interval = Convert.ToInt32(obj.PictureChangeTime);
                this.BackgroundImage = Image.FromFile(Images[0]);
                ImageCount++;

            }
            catch
            {

            }
            //Operation.IsAdmin = Convert.ToBoolean(Program.AppSettings.IsAdmin);
            //Operation.UserId = Operation.Decryptdata(Program.AppSettings.UserId);
          //  BindMenu();
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
         // else
            {
               // return null;
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
                    dtChild = Operation.GetDataTable("Select DisplayName,ImageFile from Menu left join UserRights on UserRights.MenuId=Menu.MenuId where Menu.ParentMenuId=" + dt.Rows[0][0].ToString() + " and IsDeployed=1 and IsChecked=1 and rights=6 and userid=" + Operation.AdminUserId + "  and menu.MenuId not in (26,23) order by SrNo", Operation.Conn);
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
                    Operation.ExecuteNonQuery("Update UserLogIn set IsLoggedIn='False' where UserId=" + Operation.AdminUserId, Operation.Conn);
                    Operation.AdminUserId = "0";
                    Operation.AdminUserName = "";
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
                string Query = "";
                Operation.RightsStr = "";
                if (Operation.IsSuperAdmin)
                {
                    Operation.RightsStr = "1,2,3,4,5,";
                }
                else
                {
                    Query = "Select Rights from userrights left join menu on userrights.menuid=menu.menuid where userid=" + Operation.AdminUserId + " and MenuName='" + ((ToolStripMenuItem)Sender).Tag + "' and Rights <> 6";
                    DataTable RightsDt = Operation.GetDataTable(Query, Operation.Conn);
                    if ((RightsDt.Rows.Count > 0))
                    {
                        for (int i = 0; i <= RightsDt.Rows.Count - 1; i++)
                        {
                            Operation.RightsStr = Operation.RightsStr + RightsDt.Rows[i][0].ToString() + ",";
                        }
                    }
                }
                if (((ToolStripMenuItem)Sender).Text.ToString() == "Branch")
                    Operation.reporttype = "1";
                else if (((ToolStripMenuItem)Sender).Text.ToString() == "Department")
                    Operation.reporttype = "2";
                else if (((ToolStripMenuItem)Sender).Text.ToString() == "Designation")
                    Operation.reporttype = "3";
                else
                    Operation.reporttype = "0";
                Form frm = new Form();

                string FormName = ((ToolStripMenuItem)Sender).Tag.ToString();
                FormName = Assembly.GetEntryAssembly().GetName().Name + "." + FormName;
                frm = (Form)Assembly.GetEntryAssembly().CreateInstance(FormName);

                FormSetting(frm);

                if (File.Exists(Application.StartupPath + "\\Images\\Reminder.ico"))
                {
                    frm.Icon = new System.Drawing.Icon(Application.StartupPath + "\\Images\\Reminder.ico");
                }
            }
            else if ((((ToolStripMenuItem)Sender).Text == "Backup"))
            {
                //SaveFileDialog sfdbackup = new SaveFileDialog();
                //sfdbackup.InitialDirectory = "C:";
                //sfdbackup.Title = "Save Backup";
                //sfdbackup.Filter = "Backup File(*.bak)|*.bak";
                //if (sfdbackup.ShowDialog() == DialogResult.OK)
                //{
                //    if (Operation.ExecuteNonQuery("backup database [" + Operation.Conn.Database + "] to disk = '" + sfdbackup.FileName + "'", Operation.Conn) == -1)
                //    {
                //        string txtfilename = sfdbackup.FileName.Substring(0, sfdbackup.FileName.LastIndexOf('\\')) + "\\" + DateTime.Now.Millisecond + ".txt";
                //        File.Create(txtfilename).Close();
                //        File.WriteAllText(txtfilename, "Backup Date : " +
                //            Convert.ToString(Operation.Encryptdata(DateTime.Now.ToString("dd/MM/yyyy"))) +
                //            "\nBackup Time : " + Convert.ToString(Operation.Encryptdata(DateTime.Now.ToString("hh:mm:ss"))) +
                //            "\nCompany Name : " + Convert.ToString(Operation.Encryptdata(Operation.objComp.CompName)) +
                //            "\nFinancial Year From : " + Convert.ToString(Operation.Encryptdata(Operation.objComp.CompFromDate.ToString("dd/MM/yyyy"))) +
                //            "\nFinancial Year To : " + Convert.ToString(Operation.Encryptdata(Operation.objComp.CompToDate.ToString("dd/MM/yyyy"))));
                //        using (ZipFile zip = new ZipFile())
                //        {
                //            zip.AddFile(sfdbackup.FileName);
                //            zip.AddFile(txtfilename);
                //            zip.Save(sfdbackup.FileName.Split('.')[0] + ".zip");
                //            File.Delete(txtfilename);
                //            File.Delete(sfdbackup.FileName);
                //        }
                //        //Compress(sfdbackup.FileName);
                //        MessageBox.Show("Backup process successfully completed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        File.Delete(sfdbackup.FileName);
                //    }
                //    else
                //        MessageBox.Show("Error taking backup, Please try later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
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
                //OpenFileDialog ofdrestore = new OpenFileDialog();
                //ofdrestore.InitialDirectory = "C:";
                //ofdrestore.Title = "Restore";
                //ofdrestore.DefaultExt = "bak";
                //ofdrestore.Filter = "Compressed File (*.zip)|*.zip";
                //ofdrestore.FilterIndex = 1;
                //if (ofdrestore.ShowDialog() == DialogResult.OK)
                //{
                //    string bakfile = "", textfile = "", fromyear = "", toyear = "", companyname = "", backupdate = "", backuptime = "";
                //    string ZipToUnpack = ofdrestore.FileName;
                //    //string UnpackDirectory = ofdrestore.FileName.Substring(0, ofdrestore.FileName.ToString().LastIndexOf("\\"));
                //    string UnpackDirectory = Application.StartupPath;
                //    using (ZipFile zip1 = ZipFile.Read(ZipToUnpack))
                //    {
                //        // e2 = default(ZipEntry);
                //        // here, we extract every entry, but we could extract conditionally,
                //        // based on entry name, size, date, checkbox status, etc.   
                //        foreach (ZipEntry e2 in zip1)
                //        {
                //            e2.Extract(UnpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                //            if (e2.FileName.ToString().ToUpper().Contains(".BAK"))
                //            {
                //                bakfile = e2.FileName;
                //            }
                //            if (e2.FileName.ToString().ToUpper().Contains(".TXT"))
                //            {
                //                textfile = e2.FileName;
                //            }
                //        }
                //        try
                //        {

                //            string[] lines = File.ReadAllText(textfile).Split('\n');
                //            backupdate = lines[0].Split(':')[1].Trim();
                //            backuptime = lines[1].Split(':')[1].Trim();
                //            companyname = lines[2].Split(':')[1].Trim();
                //            fromyear = lines[3].Split(':')[1].Trim();
                //            toyear = lines[4].Split(':')[1].Trim();
                //            if (Operation.Decryptdata(companyname) != Operation.objComp.CompName)
                //            {
                //                if (MessageBox.Show("You are restoring data of " + Operation.Decryptdata(companyname) + " to current company (" + Operation.objComp.CompName.ToString() + "),\nWould you like to continue?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                //                {
                //                    return;
                //                }
                //            }
                //            if (Operation.Decryptdata(fromyear) != Operation.objComp.CompFromDate.ToString("dd/MM/yyyy") || Operation.Decryptdata(toyear) != Operation.objComp.CompToDate.ToString("dd/MM/yyyy"))
                //            {
                //                MessageBox.Show("Your current companies financial year and backup data financial year is not matching, The restore operation can not be completed.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //                return;
                //            }
                //            if (MessageBox.Show("You have taken this backup dated " + Operation.Decryptdata(backupdate) + " and " + Operation.Decryptdata(backuptime) + " time,\nWould you like to continue?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                //            {
                //                return;
                //            }
                //        }
                //        catch
                //        {
                //            MessageBox.Show("Your database is currepted, Please select original file.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //            return;
                //        }
                //    }
                //    if ((Operation.ExecuteNonQuery("Alter database [" + Operation.Conn.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", Operation.Conn) == -1) && (Operation.ExecuteNonQuery("restore database [" + Operation.Conn.Database + "] from disk = '" + (UnpackDirectory + "\\" + bakfile) + "' with replace", Operation.Conn) == -1) && (Operation.ExecuteNonQuery("alter database [" + Operation.Conn.Database + "] set online", Operation.Conn) == -1))
                //        //if (Operation.ExecuteNonQuery("restore database [" + Operation.CompanyCon.Database + "] from disk = '" + (UnpackDirectory + "\\" + bakfile) + "' with FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 10", Operation.Conn) == -1)
                //        MessageBox.Show("Database Restored Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    else
                //    {
                //        Operation.ExecuteNonQuery("alter database [" + Operation.Conn.Database + "] set online", Operation.Conn);
                //        MessageBox.Show("Error Restoring database, Please try later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    }
                //    File.Delete(ofdrestore.FileName.Substring(0, ofdrestore.FileName.ToString().LastIndexOf("\\")) + bakfile);
                //    File.Delete(ofdrestore.FileName.Substring(0, ofdrestore.FileName.ToString().LastIndexOf("\\")) + textfile);
                //}
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

            // The memory stream now contains the Zipped data

            // Create the output stream for the new file.

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

        //public void BindMenu()
        //{

        //    MDIMenu.Items.Clear();
        //    string Query = "";
        //    Operation.CompanyId = Operation.ExecuteScalar("select Comp_id from Company_Master", Operation.Conn).ToString();
        //    //Program.AppSettings.CompanyId = Operation.Encryptdata(company_id);
        //    //if (!Convert.ToBoolean(Program.AppSettings.IsAdmin))
        //    if (!Operation.IsSuperAdmin)
        //    {
        //        Query = "Select * from Menu left join userrights on userrights.menuid=menu.menuid where userid=" + Operation.AdminUserId + " and CompanyId=" + Operation.CompanyId + " and rights=6 and ParentMenuId=0 and IsDeployed=1 order by SrNo";
        //    }
        //    else
        //    {
        //        Query = "Select * from Menu where ParentMenuId=0 and IsDeployed=1 order by SrNo";
        //    }

        //    DataTable ParentDt = Operation.GetDataTable(Query, Operation.Conn);
        //    for (int i = 0; i <= ParentDt.Rows.Count - 1; i++)
        //    {
        //        //add the root item and check if it has any children
        //        MenuId = Convert.ToInt32(ParentDt.Rows[i]["MenuId"].ToString());
        //        ToolTip = ParentDt.Rows[i]["ToolTip"].ToString();
        //        Image img = null;
        //        try
        //        {
        //            img = Image.FromFile(Application.StartupPath + "\\Design\\" + ParentDt.Rows[i]["ImageFile"].ToString());
        //        }
        //        catch
        //        {
        //            img = null;
        //        }
        //        AddChildMenuItems(MDIMenu.Items.Add(ParentDt.Rows[i]["DisplayName"].ToString(), img, MenuItem_Click));
        //    }
        //}
        //public void BindMenu(string companyid, bool isadmin)
        //{

        //    MDIMenu.Items.Clear();
        //    string Query = "";

        //    if (!Convert.ToBoolean(isadmin))
        //    {
        //        Query = "Select * from Menu left join userrights on userrights.menuid=menu.menuid where userid=" + Operation.AdminUserId + " and CompanyId=" + Operation.CompanyId + " and rights=6 and ParentMenuId=0 and IsDeployed=1 order by SrNo";
        //    }
        //    else
        //    {
        //        Query = "Select * from Menu where ParentMenuId=0 and IsDeployed=1 order by SrNo";
        //    }

        //    DataTable ParentDt = Operation.GetDataTable(Query, Operation.Conn);
        //    for (int i = 0; i <= ParentDt.Rows.Count - 1; i++)
        //    {
        //        //add the root item and check if it has any children
        //        MenuId = Convert.ToInt32(ParentDt.Rows[i]["MenuId"].ToString());
        //        ToolTip = ParentDt.Rows[i]["ToolTip"].ToString();
        //        Image img = null;
        //        try
        //        {
        //            img = Image.FromFile(Application.StartupPath + "\\Design\\" + ParentDt.Rows[i]["ImageFile"].ToString());
        //        }
        //        catch
        //        {
        //            img = null;
        //        }
        //        AddChildMenuItems(MDIMenu.Items.Add(ParentDt.Rows[i]["DisplayName"].ToString(), img, MenuItem_Click));
        //    }
        //}

        private void tBackGround_Tick(object sender, EventArgs e)
        {

            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    Operation.IsInternetExits = true;
                }
            }
            catch
            {
                Operation.IsInternetExits = false;
               
            }
            //if (Images != null)
            //{
            //    if (Images.Count > 0)
            //    {
            //        if (Images.Count == ImageCount)
            //        {
            //            try
            //            {
            //                this.BackgroundImage = Image.FromFile(Images[0]);
            //                ImageCount = 0;
            //            }
            //            catch { ImageCount = 0; }
            //        }
            //        else
            //        {
            //            try
            //            {
            //                this.BackgroundImage = Image.FromFile(Images[ImageCount]);
            //                ImageCount++;
            //            }
            //            catch { ImageCount++; }
            //        }
            //    }
            //}
            //else
            //    tBackGround.Enabled = false;
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
            if (!CloseFlag && !Operation.CloseApp)
            {
                if ((MessageBox.Show("Are you sure to Exit?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                {
                    //Operation.ExecuteNonQuery("update UserLogin set IsLoggedIn = 'False' where UserId = " + Operation.UserId, Operation.Conn);
                    ////executeQuery("Update UserLogIn set IsLoggedIn='False' where UserId=" & Decryptdata(AppSettings.UserId), con)
                    //Program.AppSettings.UserId = "";
                    //Program.AppSettings.Save();
                    CloseFlag = true;
                    //Operation.ExecuteNonQuery("Update UserLogIn set IsLoggedIn='False' where UserId=" + Operation.UserId, Operation.Conn);
                    Operation.AdminUserId = "0";
                    Operation.AdminUserName = "";
                    Operation.IsSuperAdmin = false;
                    //Program.AppSettings.UserId = "";
                    //Program.AppSettings.Save();
                  //  frmUserLogin LogIn = new frmUserLogin();
                    Application.Exit();
                }
                else
                {

                    e.Cancel = true;
                }
            }
        }

        private void frmMDI_Load(object sender, EventArgs e)
        {
            try
            {
                string isSuperadmin = "";
               // string user = Operation.ExecuteScalar("select UserName from UserLogIn where UserId='" + Operation.UserId.ToString() + "'", Operation.Conn).ToString();
                if (Operation.IsSuperAdmin == true)
                {
                    isSuperadmin = "(Super_Admin)";
                }
                
                ControlMessage.Text = Operation.AdminUserName+isSuperadmin;
            }
            catch
            {
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
   
        }

        private void btnClientMenu_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        
        }

        private void menuMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            MenuMaster frm = new MenuMaster();
            frm.MdiParent = this;

            frm.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmAdminMaster frm = new frmAdminMaster();
           // frm.MdiParent = this;
            frm.ShowDialog();

        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmChagePassword frm = new frmChagePassword();
            frm.ShowDialog();
        }

        private void clientMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmClientMaster frm = new frmClientMaster();
            frm.MdiParent = this;
            frm.Show();
        }

        private void clientMenuAllocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmSubMenuMaster frm = new frmSubMenuMaster();
            frm.MdiParent = this;
            frm.Show();
        }

        private void clientDashboardMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            frmDashboardMaster frm = new frmDashboardMaster();
            frm.MdiParent = this;
            frm.Show();
        }      

        private void clientLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Operation.IsInternetExits == false)
            {
                AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                return;
            }
            Process[] p = Process.GetProcessesByName("ExpertMobileSystem(Client)");

            if (p.Length > 0)
            {
                MessageBox.Show("ExpertMobileSystem is already running.....", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            Process myProcess = new Process();

            try
            {
                if (File.Exists(Application.StartupPath + "\\ExpertMobileSystem(Client).exe"))
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    // You can start any process, HelloWorld is a do-nothing example.

                    myProcess.StartInfo.FileName = Application.StartupPath + "\\ExpertMobileSystem(Client).exe";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                    // This code assumes the process you are starting will terminate itself.
                    // Given that is is started without a window so you cannot terminate it
                    // on the desktop, it must terminate itself or you can do it programmatically
                    // from this application using the Kill method.
                }
            }
            catch// (Exception e)
            {
               // Console.WriteLine(e.Message);
            }
            //ExpertMobileSystem_Client_.frmUserLogin frm = new ExpertMobileSystem_Client_.frmUserLogin();
            //frm.MdiParent = this;
            //frm.Show();
        }


    }
}
