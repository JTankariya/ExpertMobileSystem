using System;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Data.SqlClient;

namespace ExpertMobileSystem_Client_
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //try
            //{
            //    using (var client = new WebClient())
            //    using (var stream = client.OpenRead("http://www.google.com"))
            //    {
            //        Operation.IsInternetExits = true;
            //    }
            //}
            //catch
            //{
            //    Operation.IsInternetExits = false;
            //    MessageBox.Show("Internet Connection does not Exist." + Environment.NewLine + "Please Check Internet connection.", "Internet Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    //Application.Exit();
            //    return;
            //}
            //try
            //{
            //    System.Diagnostics.Process process = new System.Diagnostics.Process();
            //    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //    startInfo.FileName = "cmd.exe";
            //    startInfo.Arguments = "/C %windir%\\microsoft.net\\framework\\v2.0.50727\\Caspol.exe -pp off -m -cg LocalIntranet_Zone FullTrust";
            //    process.StartInfo = startInfo;
            //    process.Start();
            //    //MessageBox.Show("Done-Caspol");
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Error in Caspol.exe : " + Environment.NewLine + e.Message);
            //    return;
            //}
            //Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Process[] p = Process.GetProcessesByName("ExpertMobileSystem(Client)");

                if (p.Length > 1)
                {
                    MessageBox.Show("ExpertMobileSystem is already running.....", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Error : While Checking Another exe." + ex.Message);
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
            }
            try
            {
                if (Operation.IsInternetOnorOff())
                {
                    Operation.IsInternetExits = true;
                }
                else
                {
                    Application.DoEvents();
                    while(!Operation.IsInternetExits==true)
                    {
                        Application.DoEvents();
                        if (Operation.IsInternetOnorOff())
                            Operation.IsInternetExits = true;
                        else
                            Operation.IsInternetExits = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error while checking Internet is On or Off" + ex.Message);
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
            }
             try
             {
                Application.EnableVisualStyles();
                // Application.SetCompatibleTextRenderingDefault(false);
                Operation.Conn = new SqlConnection(Operation.ConnStr);
                try
                {
                    string path = Application.StartupPath + "\\UserDetail.ini";
                    if (File.Exists(path))
                    {
                        //MessageBox.Show(HardwareInfo.GetBIOSmaker()+Environment.NewLine + HardwareInfo.GetBIOSserNo());
                        string fileUserDetail = File.ReadAllText(path);
                        
                        string fileUserDetailDecrypt = Operation.Decryptdata(fileUserDetail);
                        //string fileUserDetailDecrypt = CryptorEngine.Decrypt(fileUserDetail, true);
                        string[] lines = fileUserDetailDecrypt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        string[] hdd = lines[0].Split(':');
                        string[] mac = lines[1].Split(':');
                        string[] myuser = lines[2].Split(':');
                        string[] mypass = lines[3].Split(':');
                        //string username = Operation.ReadFromINI(path, Operation.Encryptdata("UserName"));
                        //string pass = Operation.ReadFromINI(path, Operation.Encryptdata("Password"));
                        string username = Operation.Decryptdata(myuser[1].ToString());
                        //string username = CryptorEngine.Decrypt(myuser[1].ToString(), true);
                        string pass = mypass[1].ToString();
                        string userHDD = HardwareInfo.GetHDDSerialNo().ToString();
                        string userMAC = HardwareInfo.GetMACAddress().ToString();
                        if (Operation.Decryptdata(hdd[1].ToString()).Contains(userHDD))
                        //if (userHDD == CryptorEngine.Decrypt(hdd[1].ToString(), true) && userMAC == CryptorEngine.Decrypt(mac[1].ToString(),true))
                        {
                            //string Query = "Select * from UserLogIn where userid='" + Operation.Decryptdata(setting[0]) + "'";
                            string str = "select * from ClientMaster where UserName = '" + username + "' and Password = '" + pass.Trim() + "' ";
                            try
                            {
                                DataTable dt = Operation.GetDataTable(str, Operation.Conn);
                                if (dt.Rows.Count > 0)
                                {
                                    Operation.LogFile = Application.StartupPath + "\\LogFile.txt";
                                    Operation.Clientid = dt.Rows[0]["Clientid"].ToString();
                                    Operation.ClientUserName = dt.Rows[0]["Username"].ToString();
                                    // frmUserLogin ob = new frmUserLogin();
                                    new frmUserLogin().SetCompanyInfo(dt);
                                    Operation.CurrentDate = Operation.GetNetworkTime();
                                    Application.Run(new frmMDI());
                                }
                                else
                                    Application.Run(new frmUserLogin());
                            }
                            catch (Exception ex)
                            {
                                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
                                //MessageBox.Show("Error in Login");
                                return;
                            }
                        }
                        else
                            Application.Run(new frmUserLogin());
                    }
                    else
                        Application.Run(new frmUserLogin());
                }
                catch
                {
                    Application.Run(new frmUserLogin());
                }
                //Application.Run(new frmUserLogin());
                
            }
            catch (Exception ex)
            {
                
            }

        }
    }
}
