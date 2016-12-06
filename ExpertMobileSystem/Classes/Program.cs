using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Net;
using System.Diagnostics;

namespace ExpertMobileSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           // Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            //Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C %windir%\\microsoft.net\\framework\\v2.0.50727\\Caspol.exe -pp off -m -cg LocalIntranet_Zone FullTrust";
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error in Caspol.exe : " + Environment.NewLine + e.Message);
                return;
            }

            Process[] p = Process.GetProcessesByName("ExpertMobileSystem");

            if (p.Length > 1)
            {
                MessageBox.Show("ExpertMobileSystem is already running.....", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }

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
                Application.Exit();
                return;
            }
            Application.Run(new frmUserLogin());
        }
    }
}
