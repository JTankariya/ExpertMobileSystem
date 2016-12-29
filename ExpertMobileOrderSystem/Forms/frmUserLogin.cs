using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Net;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ExpertMobileOrderSystem
{

    public partial class frmUserLogin : Form
    {
        public string[] setting;
        ClientInfo obj = new ClientInfo();
        public frmUserLogin()
        {
            InitializeComponent();
            Create_connection();
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
                Operation.ServerName = "";
                System.IO.File.Create(Application.StartupPath + "\\servername.txt");
            }
            MessageBox.Show("Application is about to close.\nYou need to restart the application again.", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void lnklblChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmChangePassword child = new frmChangePassword(0, "");
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
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    Operation.IsInternetExits = true;
                }
            }
            catch
            {
                Operation.IsInternetExits = false;
                MessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            

            try
            {
                ExpertMobileOrderSystem.Operation.ShowSplash();
                string strAppPath = null;
                strAppPath = Application.StartupPath;
                string str = "select * from ClientMaster where UserName = '" + txtUserName.Text.Trim() + "' and Password = '" + Operation.Encryptdata(txtpassword.Text.Trim()) + "' ";
                bool flag = false;
                DataTable dt = Operation.GetDataTable(str, Operation.Conn);
                if (dt.Rows.Count > 0)
                {
                    Operation.LogFile = Application.StartupPath + "\\LogFile.txt";
                    Operation.Clientid = dt.Rows[0]["Clientid"].ToString();
                    Operation.ClientUserName = dt.Rows[0]["Username"].ToString();
                    SetCompanyInfo(dt);
                    Operation.CurrentDate = DateTime.Now;
                    flag = true;
                    if (Operation.CloseApp)
                    {
                        this.Close();
                    }
                    else
                    {
                        File.Delete(Application.StartupPath + "\\UserDetail.ini");
                        FileStream SysFile = File.Create(Application.StartupPath + "\\UserDetail.ini");
                        SysFile.Close();
                        string MyUserDetail = "";
                        MyUserDetail = Operation.Encryptdata(Operation.Encryptdata("GetHDDNo")+ ":" + Operation.Encryptdata(HardwareInfo.GetHDDSerialNo().ToString()) + "" + Environment.NewLine + Operation.Encryptdata("MACAddress") + ":" + Operation.Encryptdata(HardwareInfo.GetMACAddress().ToString()) + "" + Environment.NewLine + Operation.Encryptdata("UserName") + ":" + Operation.Encryptdata(txtUserName.Text) + "" + Environment.NewLine + Operation.Encryptdata("Password") + ":" + Operation.Encryptdata(txtpassword.Text) + "");
                        File.AppendAllText(Application.StartupPath + "\\UserDetail.ini", MyUserDetail );
                        Operation.GetIniValue();
                        ExpertMobileOrderSystem.Operation.CloseSplash();
                        this.Hide();
                        frmMain objMain = new frmMain();
                        objMain.Show();
                    }
                }
                if (!flag)
                {
                    ExpertMobileOrderSystem.Operation.CloseSplash();
                    MessageBox.Show("UserName or Password is not Correct, Please Try Again", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    string Authentication;//TO SOLVE:authentication with old password no longer supported, use 4.1 style passwords.
                    Authentication = "SET SESSION old_passwords=0; \n SET PASSWORD=PASSWORD('dms@1001_');";
                    Operation.ExecuteNonQuery(Authentication, Operation.Conn);
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
            //  LoadCode();
        }

        private void frmUserLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        public void SetCompanyInfo(DataTable dt)
        {


            Operation.objComp.AccountExpiredOn = Convert.ToDateTime(dt.Rows[0]["AccountExpiredOn"]);
            Operation.objComp.ClientId = Convert.ToInt32(dt.Rows[0]["ClientId"]);
            Operation.objComp.ClientCreatedDate = Convert.ToDateTime(dt.Rows[0]["ClientCreatedDate"]);
            Operation.objComp.CompanyAddress = dt.Rows[0]["CompanyAddress"].ToString();
            Operation.objComp.CompanyName = dt.Rows[0]["CompanyName"].ToString();
            Operation.objComp.CreatedAdminID = Convert.ToInt32(dt.Rows[0]["CreatedAdminID"]);
            Operation.objComp.Email = dt.Rows[0]["Email"].ToString();
            Operation.objComp.FirstName = dt.Rows[0]["FirstName"].ToString();
            Operation.objComp.LastName = dt.Rows[0]["LastName"].ToString();
            Operation.objComp.MobileNo = dt.Rows[0]["MobileNo"].ToString();
            Operation.objComp.NoOfAccessUser = Convert.ToInt32(dt.Rows[0]["NoOfAccessUsers"]);
            Operation.objComp.NoOfCompanyPerUser = Convert.ToInt32(dt.Rows[0]["NoOfCompanyPerUser"]);
            Operation.objComp.NoOfDays = Convert.ToInt32(dt.Rows[0]["NoOfDays"]);
            Operation.objComp.Password = dt.Rows[0]["Password"].ToString();
            Operation.objComp.UserName = dt.Rows[0]["UserName"].ToString();
            Operation.objComp.TotalCreatedUser = Convert.ToInt32(dt.Rows[0]["TotalCreatedUser"]);
            Operation.objComp.TotalCreatedCompany = Convert.ToInt32(dt.Rows[0]["TotalCreatedCompany"]);
            Operation.objComp.QueryRights = Convert.ToBoolean(dt.Rows[0]["QueryRights"]);

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

        private void frmUserLogin_Load(object sender, EventArgs e)
        {
            try
            {
                Process[] p = Process.GetProcessesByName("ExpertMobileOrderSystem");

                if (p.Length > 1)
                {
                    MessageBox.Show("ExpertMobileOrderSystem is already running.....", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
            }
            Paint += draw;
            Invalidate();
        }
    }
}
