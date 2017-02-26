using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace ExpertMobileOrderSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Process[] p = Process.GetProcessesByName("ExpertMobileOrderSystem");

                if (p.Length > 1)
                {
                    MessageBox.Show("ExpertMobileOrderSystem is already running.....", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Application.Exit();
                }
                if (Operation.IsInternetOnorOff())
                {
                    Operation.IsInternetExits = true;
                }
                else
                {
                    Application.DoEvents();
                    //while (!Operation.IsInternetExits == true)
                    //{
                    //    Application.DoEvents();
                    //    if (Operation.IsInternetOnorOff())
                    //        Operation.IsInternetExits = true;
                    //    else
                    //        Operation.IsInternetExits = false;
                    //}
                }
                Operation.Conn = new SqlConnection(Operation.ConnStr);

                string path = Application.StartupPath + "\\UserDetail.ini";
                if (File.Exists(path))
                {
                    string fileUserDetail = File.ReadAllText(path);
                    string fileUserDetailDecrypt = Operation.Decryptdata(fileUserDetail);
                    string[] lines = fileUserDetailDecrypt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    string[] hdd = lines[0].Split(':');
                    string[] mac = lines[1].Split(':');
                    string[] myuser = lines[2].Split(':');
                    string[] mypass = lines[3].Split(':');
                    string username = Operation.Decryptdata(myuser[1].ToString());
                    string pass = mypass[1].ToString();
                    string userHDD = HardwareInfo.GetHDDSerialNo().ToString();
                    string userMAC = HardwareInfo.GetMACAddress().ToString();
                    if (Operation.Decryptdata(hdd[1].ToString()).Contains(userHDD))
                    {
                        string str = "select * from [Order.ClientMaster] where UserName = '" + username + "' and Password = '" + pass.Trim() + "' ";

                        DataTable dt = Operation.GetDataTable(str, Operation.Conn);
                        if (dt.Rows.Count > 0)
                        {
                            Operation.LogFile = Application.StartupPath + "\\LogFile.txt";
                            Operation.currClient = new Client(dt.Rows[0]);
                            Operation.CurrentDate = Operation.GetNetworkTime();
                            Application.Run(new frmMain());
                        }
                        else
                            Application.Run(new frmUserLogin());
                    }
                    else
                        Application.Run(new frmUserLogin());
                }
                else
                    Application.Run(new frmUserLogin());

            }
            catch (Exception ex)
            {
                Application.Run(new frmUserLogin());
            }
            //Application.Run(new frmUserLogin());
        }
    }
}
