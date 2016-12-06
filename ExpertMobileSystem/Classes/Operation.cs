using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;

using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;

namespace ExpertMobileSystem
{
    public class Operation
    {
        public static Boolean IsInternetExits = false;

        //public static clsSettings objSettings = new clsSettings();
        public static bool MasterFlag = false;
        public static DataTable ReportDt = new DataTable();
        public static string ReportName = "";
        public static string ReportFTDateHead = "";
        public static string ConnStr = @"server=208.91.198.59;user id=mehul;password=mitesh@8277_;database=ExpertMobile;persist security info=True";
      //  public static CompanyInfo objComp = new CompanyInfo();
        public static SqlConnection Conn = null;
        public static SqlConnection CompanyCon = null;
        public static string RightsStr = "";
        public static string reporttype = "";
        //public static string DatabaseName = "ReminderSoftware";
        public static string ServerName = "";
        public static string Password = "admin";
        public static string SqlUserName = "admin";
        public static bool CloseApp = false;
        public static string AdminUserId = "0";
        public static string AdminUserName = "";
        public static string CompanyId = "";
        public static bool IsSuperAdmin = false;
        public static string MultiViewId = "";
        public static string gViewQuery = "";
        public static string ViewID = "";
        public static string MsgTitle = "Expert Mobile System By Shah Infotech-9979866022";
        public static Form gform = new Form();
        public static Thread _splashThread;
        public static ExpertMobileSystem.frmLoading _splashForm;
        public static string EncryptionKey = "MAKV2SPBNI99212";
        public delegate void CloseDelegate();

        public static void ShowSplash()
        {
            if (_splashThread == null)
            {
                // show the form in a new thread
                _splashThread = new Thread(new ThreadStart(DoShowSplash));
                _splashThread.IsBackground = true;
                _splashThread.Start();
            }
        }

        // called by the thread
        private static void DoShowSplash()
        {
            //if (_splashForm == null)
            //{
            _splashForm = new frmLoading();
            //_splashForm.MdiParent=;
            //}

            // create a new message pump on this thread (started from ShowSplash)
            Application.Run(_splashForm);
        }

        /// <summary>
        /// Close the splash (Loading...) screen
        /// </summary>
        public static void CloseSplash()
        {
            //// need to call on the thread that launched this splash
            if (_splashForm.InvokeRequired)
            {
                _splashForm.Invoke(new MethodInvoker(CloseSplash));
            }
            else
            {
                Application.ExitThread();
                _splashForm = null;
                _splashThread = null;
            }
            ////_splashForm.Invoke(new CloseDelegate(frmLoading.CloseFormInternal));
            //_splashForm.Close();
        }
        public static void CloseFormInternal()
        {
            _splashForm.Close();
            _splashForm = null;
        }
       

        public static bool CheckReference(int value, string TableDetails)
        {
            for (int i = 0; i <= TableDetails.Split(',').Length - 1; i += 2)
            {
                if ((Convert.ToUInt32(Operation.ExecuteScalar("select count(" + TableDetails.Split(',')[i + 1] + ") from " + TableDetails.Split(',')[i] + " where " + TableDetails.Split(',')[i + 1] + " = " + value, Conn)) > 0))
                {
                    MessageBox.Show("Cannot delete, Reference exist in table " + TableDetails.Split(',')[i], MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            return true;
        }

        public static int ExecuteNonQuery(string Query, SqlConnection TempConn)
        {
            SqlCommand cmd = new SqlCommand(Query, TempConn);
            int Result = 0;
            try
            {
                if (TempConn.State != ConnectionState.Open)
                    TempConn.Open();
                Result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in ExecuteNonQuery.\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conn.Close();
            }
            return Result;
        }

        public static void BindComboBox(ComboBox cmb, string Query, string InitMsg, string DisplayMember, string ValueMember)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = GetDataTable(Query, Operation.Conn);
                dt.Rows.Add();
                dt.Rows[dt.Rows.Count - 1][0] = 0;
                dt.Rows[dt.Rows.Count - 1][1] = InitMsg;
                cmb.DataSource = dt;
                cmb.DisplayMember = DisplayMember;
                cmb.ValueMember = ValueMember;
                cmb.SelectedIndex = cmb.Items.Count - 1;
            }
            catch
            {
                //MessageBox.Show("Error Ocuured in Binding Combo Box", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public static bool ValidateNum(string Text)
        {
            bool ichar = true;
            for (int i = 0; i < Text.Length; i++)
            {
                if (!char.IsNumber(Text[i]) && Text[i] != '.')
                    ichar = false;
            }
            return ichar;
        }

        public static void UserLog(string Query, string FormName, string OprType)
        {
            Query = Query.Replace("'", "''");
            //string dasd = Operation.UserId;
            string MainQ = "Insert into UserLog(UserId,CompanyId,FormName,OperationType,Query,EditDate," +
                "ComputerName) values(" + AdminUserId.ToString() + "," + Operation.CompanyId + ",'" +
                FormName.ToString() + "','" + OprType.ToString() + "','" + Query.ToString() + "',getdate(),'" +
                System.Environment.MachineName.ToString() + "' )";
            ExecuteNonQuery(MainQ, Conn);
        }

        public static void InitData(SqlConnection TempConn)
        {
            SqlCommand cmd = new SqlCommand("InitData", TempConn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (TempConn.State == ConnectionState.Closed)
            {
                TempConn.Open();
            }
            cmd.ExecuteNonQuery();
            TempConn.Close();
        }

        //public static string Rightstring(Form CurrForm)
        //{
        //    string Query = "";
        //    RightsStr = "";
        //    if ((IsSuperAdmin))
        //    {
        //        RightsStr = "1,2,3,4,5,6,";
        //    }
        //    else
        //    {
        //        Query = "Select Rights from userrights left join menu on userrights.menuid=menu.menuid where userid=" + UserId.ToString() + " and CompanyId=" + objComp.CompId.ToString() + " and MenuName='" + CurrForm.Name + "' and IsChecked=1";
        //        DataTable RightsDt = GetDataTable(Query, Conn);
        //        if ((RightsDt.Rows.Count > 0))
        //        {
        //            for (int i = 0; i <= RightsDt.Rows.Count - 1; i++)
        //            {
        //                RightsStr = RightsStr + RightsDt.Rows[i][0].ToString() + ",";
        //            }
        //        }
        //    }
        //    return RightsStr;
        //}

        public static void BindGridComboBox(DataGridView GridName, int _rowindex, int colIndex, DataGridViewComboBoxCell currcell, string Query, string DisplayMember, string ValueMember, string InitMsg)
        {
            DataGridViewComboBoxCell dgvcombo = default(DataGridViewComboBoxCell);
            DataTable dtGrpBrand = new DataTable();
            dgvcombo = currcell;
            dtGrpBrand = GetDataTable(Query, Conn);
            dtGrpBrand.Rows.Add();
            dtGrpBrand.Rows[dtGrpBrand.Rows.Count - 1][DisplayMember] = InitMsg;
            dtGrpBrand.Rows[dtGrpBrand.Rows.Count - 1][ValueMember] = 0;
            dgvcombo.AutoComplete = true;
            ((DataGridViewComboBoxCell)GridName.Rows[_rowindex].Cells[colIndex]).DataSource = dtGrpBrand;
            ((DataGridViewComboBoxCell)GridName.Rows[_rowindex].Cells[colIndex]).DisplayMember = DisplayMember;
            ((DataGridViewComboBoxCell)GridName.Rows[_rowindex].Cells[colIndex]).ValueMember = ValueMember;
            ((DataGridViewComboBoxCell)GridName.Rows[_rowindex].Cells[colIndex]).Value = 0;
            //Call addserialno()
        }

        public static bool EmailValidation(string sString, TextBox ctl)
        {
            bool FoundMatch = false;
            try
            {
                if (sString != "")
                {
                    FoundMatch = Regex.IsMatch(sString, "\\b[A-Z0-9._%-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}\\b", RegexOptions.IgnoreCase);
                    //Syntax error in the regular expression

                }
                else
                    FoundMatch = true;
            }
            catch
            {
            }
            return FoundMatch;
        }

        public static Boolean ExecuteTransaction(ArrayList q, SqlConnection TempConn)
        {
            SqlCommand cmd;
            if (TempConn.State != ConnectionState.Open)
            { TempConn.Open(); }
            SqlTransaction Tran = TempConn.BeginTransaction();
            try
            {
                foreach (string query in q)
                {
                    cmd = new SqlCommand(query, TempConn, Tran);
                    cmd.ExecuteNonQuery();
                }
                Tran.Commit();
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Tran.Rollback();
                return false;
            }

        }

        public static bool CheckDependency(string TransactionTable, string ColumnName, string ColumnValue, string Msg, string ExtraCondition)
        {
            DataTable dt = Operation.GetDataTable("Select * from " + TransactionTable + " where " + ColumnName + "= '" + ColumnValue + "'" + ExtraCondition, Conn);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show(Msg, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Bindgrid(string selectcommond, DataGridView supplydatagrid)
        {
            DataTable dt = GetDataTable(selectcommond, Conn);
            supplydatagrid.DataSource = dt;
        }

        public static DataTable GetDataTable(string Query, SqlConnection TempConn)
        {
            if (TempConn.State == ConnectionState.Closed)
                TempConn.Open();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(Query, TempConn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);


                adp.Fill(dt);
                return dt;
            }
            catch
            {
                //MessageBox.Show("Error Ocuured", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return dt;
            }
            finally
            {

                TempConn.Close();
            
            }

        }

        public static object ExecuteScalar(string Query, SqlConnection TempConn)
        {
            try
            {

                SqlCommand cmd = new SqlCommand(Query, TempConn);
                if (TempConn.State != ConnectionState.Open)
                    TempConn.Open();
                object result = cmd.ExecuteScalar();
                TempConn.Close();
                return result;
            }
            catch
            {
                return null;
            
            }
        }

        public static string Encryptdata(string clearText)
        {
            //string strmsg = string.Empty;
            //byte[] encode = new byte[password.Length];
            //encode = Encoding.UTF8.GetBytes(password);
            //strmsg = Convert.ToBase64String(encode);
            //return strmsg;
            //string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decryptdata(string cipherText)
        {
            //  string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        internal static int SetVar(ComboBox newCombo, string ParamName)
        {
            int Var = 0;
            if (!(newCombo.SelectedIndex == newCombo.Items.Count - 1))
            {
                if (newCombo.SelectedValue == null)
                {
                    Var = CreateOnline(newCombo.Text, ParamName);
                }
                else
                {
                    Var = Convert.ToInt32(newCombo.SelectedValue.ToString());
                }
            }
            return Var;
        }

        public static int CreateOnline(string Name, string Parameter)
        {
            try
            {
                string ExistID = Convert.ToString(ExecuteScalar("select ISNULL(parametertransaction.link,0) as link from ParameterTransaction left join parametermaster on parametertransaction.masterlink=parametermaster.link where parametervalue = '" + Name + "' and parametermaster.parametername='" + Parameter + "' ", Conn));
                if (ExistID != "")
                {
                    return Convert.ToInt32(ExistID);
                }
                string ID = Convert.ToString(ExecuteScalar("select link from ParameterMaster where parametername = '" + Parameter + "'", Conn));
                int Result = Convert.ToInt32(ExecuteScalar("Insert into ParameterTransaction(masterlink,parametervalue,parameterabbrivation) values(" + ID + ",'" + Name + "','" + Name + "'); select scope_identity()", Conn).ToString());
                return Result;
            }
            catch
            {
                MessageBox.Show("Parameter " + Parameter + " is not found.", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }
        }

        public static void FixSrNo(DataGridView dgv)
        {
            int ctr = 0;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Visible)
                {
                    ctr++;
                    dgv.Rows[i].Cells[0].Value = ctr;
                }
            }
        }

        public static bool CheckLock(string TableName, bool close)
        {
            try
            {
                if (!close)
                {
                    DataTable TempDt = GetDataTable("Select isnull(InEdit,0) as InEdit,EditMsg from " + TableName.Split(',')[0] + " where " + TableName.Split(',')[1] + " = " + ViewID, Conn);
                    if (Convert.ToBoolean(TempDt.Rows[0]["InEdit"].ToString()) && TempDt.Rows[0]["EditMsg"].ToString().Split(' ')[2].ToString().ToUpper() != Environment.MachineName.ToString().ToUpper() )
                    {
                        MessageBox.Show("This record is used by " + TempDt.Rows[0]["EditMsg"].ToString() + " ,Please try after some time", Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        ExecuteNonQuery("Update " + TableName.Split(',')[0] + " set InEdit=1,EditMsg='" + AdminUserName.ToString() + " in " + Environment.MachineName.ToString() + "' where " + TableName.Split(',')[1] + " = " + ViewID.ToString(), Conn);
                        return true;
                    }

                }
                else
                {
                    ExecuteNonQuery("Update " + TableName.Split(',')[0] + " set InEdit=0 where " + TableName.Split(',')[1] + " = " + TableName.Split(',')[2],Conn);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error At CheckLock()\n" + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static DataTable datagrid_to_datatable(DataGridView dtg)
        {
            DataTable dt = new DataTable();
            // add the columns to the datatable            
            if (dtg.Columns != null)
            {
                for (int i = 0; i <= dtg.Columns.Count - 1; i++)
                {
                    dt.Columns.Add(dtg.Columns[i].Name);
                }
            }

            //  add each of the data rows to the table
            foreach (DataGridViewRow row in dtg.Rows)
            {
                DataRow dr = default(DataRow);
                dr = dt.NewRow();

                for (int i = 0; i <= row.Cells.Count - 1; i++)
                {
                    if ((row.Cells[i].Value == null))
                    {
                        continue;
                    }
                    dr[i] = row.Cells[i].Value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static DateTime GetNetworkTime()
        {
            const string ntpServer = "pool.ntp.org";
            var ntpData = new byte[48];
            ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();

            ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
            ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

            return networkDateTime.AddHours(5.5);
        }
        public static void SetSrNo(DataGridView dgv, string col)
        {
            int c = 1;

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells[col].Value = c;
                c++;
            }
        }
    }
    public class AutoClosingMessageBox
    {
        System.Threading.Timer _timeoutTimer;
        string _caption;
        AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            MessageBox.Show(text, caption);
        }

        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }

        void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow(null, _caption);
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}
