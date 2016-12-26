using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;
using ExpertMobileOrderSystem.enums;
using ExpertMobileOrderSystem.Classes;

namespace ExpertMobileOrderSystem
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        DataTable dtModifiedRecord = new DataTable();
        DataTable dtUnModifiedRecord = new DataTable();
        DataTable dtNewlyInsertInLocal = new DataTable();
        DataTable dtDeleteFromLocal = new DataTable();

        private void frmMain_Load(object sender, EventArgs e)
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
            try
            {
                if (Operation.IsInternetOnorOff())
                {
                    Operation.IsInternetExits = true;
                }
                else
                {
                    Application.DoEvents();
                    while (!Operation.IsInternetExits == true)
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
                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
            }

            Application.EnableVisualStyles();
            frmUserLogin loginScreen = new frmUserLogin();
            Operation.lastScreen = this;
            Operation.Conn = new SqlConnection(Operation.ConnStr);
            try
            {
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
                        string str = "select * from ClientMaster where UserName = '" + username + "' and Password = '" + pass.Trim() + "' ";
                        try
                        {
                            DataTable dt = Operation.GetDataTable(str, Operation.Conn);
                            if (dt.Rows.Count > 0)
                            {
                                Operation.LogFile = Application.StartupPath + "\\LogFile.txt";
                                Operation.Clientid = dt.Rows[0]["Clientid"].ToString();
                                Operation.ClientUserName = dt.Rows[0]["Username"].ToString();
                                new frmUserLogin().SetCompanyInfo(dt);
                                Operation.CurrentDate = Operation.GetNetworkTime();
                            }
                            else
                                loginScreen.Show();
                        }
                        catch (Exception ex)
                        {
                            Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
                            return;
                        }
                    }
                    else
                        loginScreen.Show();
                }
                else
                    loginScreen.Show();
            }
            catch
            {
                loginScreen.Show();
            }
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            NewDataUpload();
        }
        private int ClientCompanyId = 0;
        private void NewDataUpload()
        {
            try
            {
                Operation.writeLog("Upload Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                DataTable dtComp = Operation.GetDataTable("Select * From ClientCompanyMaster Where ClientId = " + Operation.Clientid, Operation.Conn);
                foreach (DataRow dr in dtComp.Rows)
                {
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

                                this.Invoke(new MethodInvoker(delegate
                                {
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

                    if (strQueries.Count > 0)
                    {
                        Operation.writeLog("====================================================================" + Environment.NewLine + "Before query starting: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ", Total Queries : " + strQueries.Count, Operation.LogFile);
                        this.Invoke(new MethodInvoker(delegate
                        {
                            toolUploadStatus.Text = "Uploading data to server : ";
                        }));
                        if (strQueries.Count > 5000)
                        {
                            Application.DoEvents();
                            this.Invoke(new MethodInvoker(delegate
                            {
                                toolUploadProgress.Maximum = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(strQueries.Count / 5000))) + 1;
                                toolUploadProgress.Minimum = 0;
                                toolUploadProgress.Value = 0;
                            }));
                            for (var i = 0; i < strQueries.Count; i += 5000)
                            {
                                var strChunk = strQueries.GetRange(i, (i + 5000 < strQueries.Count ? 5000 : strQueries.Count - i));
                                Application.DoEvents();
                                Operation.ExecuteNonQuery(string.Join(";", strChunk.ToArray()), Operation.Conn);
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    toolUploadProgress.Value++;
                                }));
                            }
                        }
                        else
                        {
                            Operation.ExecuteNonQuery(string.Join(";", strQueries.ToArray()), Operation.Conn);
                        }
                        if (Operation.gotError == false)
                        {
                            Operation.writeLog("====================================================================" + Environment.NewLine + "Query execution Ends at: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.LogFile);
                            List<string> tables = new List<string>();
                            foreach (TableNames r in Enum.GetValues(typeof(TableNames)))
                            {
                                tables.Add(r.ToString() + ".DBF");
                            }
                            CopyExpertToLocal(dr, string.Join(",", tables.ToArray()), UploadingExpertDir);
                        }
                        else
                        {
                            Operation.gotError = false;
                        }
                    }
                }
                Operation.writeLog("Upload End Time : " + DateTime.Now.ToLongTimeString() + Environment.NewLine + "--------------------=====", Operation.LogFile);

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
                this.Invoke(new MethodInvoker(delegate
                {
                    toolUploadStatus.Visible = false;
                    toolUploadProgress.Visible = false;
                }));
            }
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

        private List<string> GetTableColumns(TableNames tableName, bool isFirstTime)
        {
            Operation.GetIniValue();
            List<string> columns = new List<string>();
            if (Operation.ForceUpdateColumns || isFirstTime)
            {
                columns = GetColumnListFromServer(tableName);
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
                                File.Copy(dr["ExpertPath"].ToString() + "\\" + dr["CompanyCode"].ToString().Trim().PadLeft(3, '0') + "\\" + files[i], Application.StartupPath + "\\" + UploadingExpertDir + "\\" + files[i], true);
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
            File.Copy(dr["ExpertPath"].ToString() + "\\" + dr["CompanyCode"].ToString().Trim().PadLeft(3, '0') + "\\" + fileName, Application.StartupPath + "\\" + UploadingExpertDir + "\\TMP\\" + fileName, true);
        }
        private DataTable GetDataFromExpert(OleDbConnection SupplyConn, String SupplyQuery)
        {
            OleDbCommand cmd = new OleDbCommand(SupplyQuery, SupplyConn);
            OleDbDataAdapter adp = new OleDbDataAdapter(cmd);
            DataTable dtData = new DataTable();
            adp.Fill(dtData);
            return dtData;
        }

        private void CompareTwoDataTable(DataTable dtLocal, DataTable dtExpert, TableNames tName)
        {

            dtModifiedRecord = new DataTable();
            dtUnModifiedRecord = new DataTable();
            dtNewlyInsertInLocal = new DataTable();
            dtDeleteFromLocal = new DataTable();
            switch (tName)
            {
                #region ACT
                case TableNames.PRODUCT:
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
            }
        }
    }
}
