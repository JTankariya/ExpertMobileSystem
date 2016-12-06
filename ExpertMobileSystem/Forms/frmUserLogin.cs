using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

using System.Collections;

namespace ExpertMobileSystem
{
    public partial class frmUserLogin : Form
    {
        public string[] setting;
        public frmUserLogin()
        {
            InitializeComponent();
            Create_connection();
            // this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\Images\\Reminder.ico");
            //LoadCode();
        }

        private void LoadCode()
        {
            ////Program.CheckConn();
            //if (Operation.CloseApp)
            //{
            //    try
            //    {
            ////        SqlCommand cmd = new SqlCommand("Update UserLogIn set IsLoggedIn='False' where UserId=" + Operation.UserId, Operation.Conn);
            //        //cmd.ExecuteNonQuery();
            //        //executeQuery("Update UserLogIn set IsLoggedIn='False' where UserId=" & Decryptdata(AppSettings.UserId), con)

            //    }
            //    catch
            //    {
            //    }

            //    //Program.AppSettings.UserId = "";
            //    //Program.AppSettings.Save();
            //    this.Close();
            //}
            ////con.Close()
            //string Query = "Select * from UserLogIn where IsSuperAdmin=1";

            //DataTable dt = Operation.GetDataTable(Query,Operation.Conn);

            //if ((dt.Rows.Count == 0))
            //{
            //    Query = "Insert Into UserLogIn(UserName,Password,IsSuperAdmin) Values('Admin','" + Operation.Encryptdata("Admin") + "',1)";
            //    Operation.ExecuteNonQuery(Query, Operation.Conn);
            //}
            //this.KeyPreview = true;
            //this.Cursor = Cursors.Default;
            //this.BringToFront();
            //this.Focus();
        }

        private void lnkResetConn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (System.IO.File.Exists(Application.StartupPath + "\\servername.ini"))
            {
                Operation.ServerName = File.ReadAllText(Application.StartupPath + "\\servername.ini");
                File.Delete(Application.StartupPath + "\\connectionstring.txt");
            }
            else
            {
                //Program.AppSettings.ServerName = "";
                Operation.ServerName = "";
                System.IO.File.Create(Application.StartupPath + "\\servername.txt");
            }
            //AppSettings.ServerName = "COMPUTER3-PC"
            //Program.AppSettings.Save();
            //Application.Restart()
            // RegistraionProcess();
            try
            {
                //    SqlCommand cmd = new SqlCommand("Update UserLogIn set IsLoggedIn='False' where UserId=" + Operation.UserId, Operation.Conn);
                //       cmd.ExecuteNonQuery();

            }
            catch
            {
            }
            //executeQuery("Update UserLogIn set IsLoggedIn='False' where UserId=" & Decryptdata(AppSettings.UserId), con)
            //Program.AppSettings.UserId = "";
            //Program.AppSettings.Save();
            MessageBox.Show("Application is about to close.\nYou need to restart the application again.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void lnklblChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmChagePassword child = new frmChagePassword(0, "");
            this.SendToBack();
               child.ShowDialog();
            this.BringToFront();
        }
        private void Create_connection()
        {
            Operation.Conn = new SqlConnection(Operation.ConnStr);
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (Operation.IsInternetExits == false)
                {
                    AutoClosingMessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", 5000);
                    return;
                }
                Operation.ShowSplash();             
             
                string strAppPath = null;
                strAppPath = Application.StartupPath;
                //txtUserName.Text = "dharmesh shah";
                //txtpassword.Text = "admin";
              
              

                string str = "select * from AdminMaster where UserName = '" + txtUserName.Text.Trim() + "' and password = '" + Operation.Encryptdata(txtpassword.Text.Trim()) + "' ";
                //MessageBox.Show(Operation.Decryptdata("DJ0ZZa6P3tj+wMLJ+R4ZHA9xOknJZJzwe8336599Qsg="));
                //string str = "select * from AdminMaster where AdminName = '" + txtUserName.Text.Trim() + "' and password = '" + CryptorEngine.Encrypt(txtpassword.Text.Trim(),true) + "' ";
                bool flag = false;
                DataTable dt = Operation.GetDataTable(str, Operation.Conn);
                Operation.CloseSplash();
                if (dt.Rows.Count > 0)
                {
                        Operation.IsSuperAdmin = Convert.ToBoolean(dt.Rows[0]["IsSuperAdmin"]);
                        Operation.AdminUserId = dt.Rows[0]["Adminid"].ToString();
                        Operation.AdminUserName = dt.Rows[0]["UserName"].ToString();
                        this.Hide();
                        flag = true;
                        if (Operation.CloseApp)
                        {
                            this.Close();
                        }
                        else
                        {
                            frmMDI objMDI1 = new frmMDI();
                          //  objMDI1.BindMenu();
                            objMDI1.Show();
                        }
                }
                if (!flag)
                {
                    //string Authentication;//TO SOLVE:authentication with old password no longer supported, use 4.1 style passwords.
                    //Authentication = "SET SESSION old_passwords=0; \n SET PASSWORD=PASSWORD('dms@1001_');";
                    //Operation.ExecuteNonQuery(Authentication, Operation.Conn);

                    MessageBox.Show("UserName or Password is not Correct, Please Try Again", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtUserName.Focus();
                    return;
                }
                
            }
            catch (Exception ez)
            {
                MessageBox.Show("Error in user login" + Environment.NewLine + ez.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();

        }

        private void frmUserLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                btnOk_Click(sender, e);
            }

        }

        private void frmUserLogin_Shown(object sender, EventArgs e)
        {
            LoadCode();
        }

        private void frmUserLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
