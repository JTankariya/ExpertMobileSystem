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
using System.Threading;

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
        DataTable dtToInsertInExpert = new DataTable();

        private void frmMain_Load(object sender, EventArgs e)
        {
            toolUserInfo.Text = "Welcome " + Operation.objComp.UserName;
            //try
            //{
            //    if (Operation.IsInternetOnorOff())
            //    {
            //        Operation.IsInternetExits = true;
            //    }
            //    else
            //    {
            //        Application.DoEvents();
            //        while (!Operation.IsInternetExits == true)
            //        {
            //            Application.DoEvents();
            //            if (Operation.IsInternetOnorOff())
            //                Operation.IsInternetExits = true;
            //            else
            //                Operation.IsInternetExits = false;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
            //}

            //Application.EnableVisualStyles();
            //Operation.Conn = new SqlConnection(Operation.ConnStr);
            //try
            //{
            //    string path = Application.StartupPath + "\\UserDetail.ini";
            //    if (File.Exists(path))
            //    {
            //        string fileUserDetail = File.ReadAllText(path);
            //        string fileUserDetailDecrypt = Operation.Decryptdata(fileUserDetail);
            //        string[] lines = fileUserDetailDecrypt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            //        string[] hdd = lines[0].Split(':');
            //        string[] mac = lines[1].Split(':');
            //        string[] myuser = lines[2].Split(':');
            //        string[] mypass = lines[3].Split(':');
            //        string username = Operation.Decryptdata(myuser[1].ToString());
            //        string pass = mypass[1].ToString();
            //        string userHDD = HardwareInfo.GetHDDSerialNo().ToString();
            //        string userMAC = HardwareInfo.GetMACAddress().ToString();
            //        if (Operation.Decryptdata(hdd[1].ToString()).Contains(userHDD))
            //        {
            //            string str = "select * from [Order.ClientMaster] where UserName = '" + username + "' and Password = '" + pass.Trim() + "' ";
            //            try
            //            {
            //                DataTable dt = Operation.GetDataTable(str, Operation.Conn);
            //                if (dt.Rows.Count > 0)
            //                {
            //                    Operation.LogFile = Application.StartupPath + "\\LogFile.txt";
            //                    Operation.Clientid = dt.Rows[0]["Clientid"].ToString();
            //                    Operation.ClientUserName = dt.Rows[0]["Username"].ToString();
            //                    new frmUserLogin().SetCompanyInfo(dt);
            //                    Operation.CurrentDate = Operation.GetNetworkTime();
            //                }
            //                else
            //                {
            //                    this.Hide();
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                Operation.writeLog("====================================================================" + Environment.NewLine + "Error Msg: " + ex.Message + Environment.NewLine + Environment.NewLine + "--------------------------------------------------------------------" + Environment.NewLine + "Error Stack : " + ex.StackTrace + Environment.NewLine + "====================================================================" + Environment.NewLine, Operation.ErrorLog);
            //                return;
            //            }
            //        }
            //    }

            //}
            //catch
            //{

            //}
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want Upload Data Now?", Operation.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Operation.PromptBeforeData == true)
                {
                    AutoClosingMessageBox.Show("Gentle Reminder......" + Environment.NewLine + Environment.NewLine + "Your Expert Data will be uploaded on " + (DateTime.Now.AddMinutes(Operation.PromptMins)).ToString("hh:mm tt") + "." + Environment.NewLine + "Please Close All Expert,If You Are Using in More Than One Computer.", "Upload Information", 10000);
                }
                Application.DoEvents();
                Thread MyThread = new Thread(new ThreadStart(NewDataUpload));
                MyThread.Start();
            }
            else
            {
                return;
            }

        }
        private int ClientCompanyId = 0;
        private void NewDataUpload()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                toolUploadProgress.Visible = true;
                toolUploadProgress.Value = 0;
            }));
            try
            {
                Operation.writeLog("Upload Start Time : " + DateTime.Now.ToLongTimeString(), Operation.LogFile);
                DataTable dtComp = Operation.GetDataTable("Select * From [Order.ClientCompanyMaster] Where ClientId = " + Operation.Clientid, Operation.Conn);
                foreach (DataRow dr in dtComp.Rows)
                {
                    ClientCompanyId = Convert.ToInt32(dr["ClientCompanyId"]);
                    List<string> strQueries = new List<string>();
                    List<string> expertQueries = new List<string>();
                    string UploadingExpertPath = dr["ExpertPath"].ToString();
                    string UploadingExpertDir = Operation.RemoveSpecialCharacters(UploadingExpertPath) + "\\" + dr["CompanyCode"].ToString().Trim().PadLeft(3, '0');

                    OleDbConnection localconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\" + UploadingExpertDir + ";Mode=ReadWrite;Extended Properties=dBASE IV;Persist Security Info=False");
                    OleDbConnection expconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\" + UploadingExpertDir + "\\TMP;Mode=ReadWrite;Extended Properties=dBASE IV;Persist Security Info=False");
                    OleDbConnection expliveconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + UploadingExpertPath + "\\" + dr["CompanyCode"].ToString().Trim().PadLeft(3, '0') + ";Mode=ReadWrite;Extended Properties=dBASE IV;Persist Security Info=False");
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
                        var tableName = tName.Substring(2, tName.Length - 2);
                        Operation.writeLog("====================================================================" + Environment.NewLine + tName + " Process Started: " + startTime.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.LogFile);

                        if (File.Exists(Application.StartupPath + "\\" + UploadingExpertDir + "\\" + tableName + ".DBF") && !Operation.ForceSync)
                        {
                            Operation.writeLog("File : " + tName + " execution Start at: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), Operation.LogFile);
                            CheckExpertTmpFile(dr, tableName + ".DBF", UploadingExpertDir);
                            dtLocalAct = GetDataFromExpert(localconn, "select * from [" + tableName + "]");
                            dtExpertAct = GetDataFromExpert(expconn, "select * from [" + tableName + "]");
                            this.Invoke(new MethodInvoker(delegate
                            {
                                toolUploadProgress.Value = 0;
                                toolUploadStatus.Text = "Processing Existing " + tableName + ".dbf : ";
                            }));
                            CompareTwoDataTable(dtLocalAct, dtExpertAct, (TableNames)Enum.Parse(typeof(TableNames), tName));
                            DataTable localTable = new DataTable();
                            List<string> tableColumns = GetTableColumns((TableNames)Enum.Parse(typeof(TableNames), tName), false);
                            this.Invoke(new MethodInvoker(delegate
                            {
                                toolUploadProgress.Maximum = dtModifiedRecord.Rows.Count + dtDeleteFromLocal.Rows.Count + dtNewlyInsertInLocal.Rows.Count + dtUnModifiedRecord.Rows.Count + dtToInsertInExpert.Rows.Count;
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
                            if (File.Exists(UploadingExpertPath + "\\" + dr["CompanyCode"].ToString().Trim().PadLeft(3, '0') + "\\" + tableName + ".DBF"))
                            {
                                List<string> tableColumns = GetTableColumns((TableNames)Enum.Parse(typeof(TableNames), tName), true);
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    toolUploadProgress.Value = 0;
                                    toolUploadStatus.Text = "Processing " + tName + ".dbf : ";
                                }));
                                CheckExpertTmpFile(dr, tableName + ".DBF", UploadingExpertDir);
                                var columnName = GetExperColumnNamesForInsert(tableColumns);
                                strQueries.Add(InsertUpdateQueries.GetQueries((TableNames)Enum.Parse(typeof(TableNames), tName), OperationTypes.DELETE, null, null, ClientCompanyId, tableColumns));
                                var dt = GetDataFromExpert(expconn, "select " + string.Join(",", columnName.ToArray()) + " from [" + tableName + "]");

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
                        Operation.writeLog("====================================================================" + Environment.NewLine + tableName + " Process Ends in: " + endTime.Hours + ":" + endTime.Minutes + ":" + endTime.Seconds, Operation.LogFile);
                    }
                    strQueries.Add("Update [Order.ClientCompanyMaster] Set DataUploadDateTime='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' Where ClientCompanyId=" + ClientCompanyId + ";");

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
                        strQueries = new List<string>();
                        foreach (string tName in tableNames)
                        {
                            var tableName = tName.Substring(2, tName.Length - 2);
                            List<string> tableColumns = GetTableColumns((TableNames)Enum.Parse(typeof(TableNames), tName), false);
                            dtToInsertInExpert = Operation.GetDataTable("select * from OS" + tableName + " where clientcompanyid=" + ClientCompanyId + " and OperationFlag is not null", Operation.Conn);

                            for (var i = 0; i < dtToInsertInExpert.Rows.Count; i++)
                            {
                                if (dtToInsertInExpert.Rows[i]["OperationFlag"].ToString() == "I")
                                {
                                    var dtcheckCode = GetDataFromExpert(expconn, "select * from " + tableName + " where Code='" + dtToInsertInExpert.Rows[i]["Code"] + "' and Name <> '" + dtToInsertInExpert.Rows[i]["Name"] + "'");
                                    if (dtcheckCode.Rows.Count > 0)
                                    {
                                        //This is a case where new entry has been done in expert and in web with same code but with different name
                                        var newCode = 100001;
                                        var oldCode = 0;
                                        var dtMax = GetDataFromExpert(expconn, "select * from PGroup order by Code desc");
                                        if (dtMax != null && dtMax.Rows.Count > 0)
                                        {
                                            newCode = Convert.ToInt32(dtMax.Rows[0]["Code"]) + 1;
                                        }
                                        else
                                        {
                                            newCode = 100001;
                                        }
                                        oldCode = Convert.ToInt32(dtToInsertInExpert.Rows[i]["Code"]);
                                        dtToInsertInExpert.Rows[i]["Code"] = newCode;
                                        strQueries.Add("Update " + tName + " set Code = '" + newCode + "',OperationFlag=NULL where ClientCompanyId=" + ClientCompanyId + " and Code='" + oldCode + "' and OperationFlag is not null");
                                    }
                                }
                                expertQueries.Add(InsertUpdateQueries.GetQueriesForExpert((TableNames)Enum.Parse(typeof(TableNames), tName), OperationTypes.INSERT, dtToInsertInExpert.Rows[i], null, ClientCompanyId, tableColumns) + "~" + tName + "~Code~" + dtToInsertInExpert.Rows[i]["Code"]);
                            }

                        }
                        if (expertQueries.Count > 0)
                        {
                            for (var i = 0; i < expertQueries.Count; i++)
                            {
                                if (Operation.ExecuteNonQuery(expertQueries[i].Split('~')[0], expliveconn) > 0)
                                {
                                    Operation.ExecuteNonQuery("Update " + expertQueries[i].Split('~')[1] + " set OperationFlag=NULL where ClientCompanyId=" + ClientCompanyId + " and " + expertQueries[i].Split('~')[2] + "='" + expertQueries[i].Split('~')[3] + "'", Operation.Conn);
                                }
                            }
                        }
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
                                var tableName = r.ToString().Substring(2, r.ToString().Length - 2);
                                tables.Add(tableName.ToString() + ".DBF");
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
                    if (!item.Contains("ClientCompanyId") && !item.Contains("OperationFlag") && !item.Contains("RefId"))
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
                case TableNames.OSACT:
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

                #region PGROUP
                case TableNames.OSPGROUP:

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
                case TableNames.OSPRODUCT:
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

                #region RATE
                case TableNames.OSRATE:
                    changed = (from table1 in dtLocal.AsEnumerable()
                               join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Link") equals table2.Field<string>("Link")
                               where table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<DateTime>("FDate") != table2.Field<DateTime>("FDate") || table1.Field<DateTime>("TDate") != table2.Field<DateTime>("TDate")
                               select table1);

                    if (changed != null && changed.Count() > 0)
                        dtModifiedRecord = changed.CopyToDataTable();

                    unchanged = (from table2 in dtExpert.AsEnumerable()
                                 join table1 in dtLocal.AsEnumerable() on table2.Field<string>("Link") equals table1.Field<string>("Link")
                                 where table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("Name") != table2.Field<string>("Name") || table1.Field<DateTime>("FDate") != table2.Field<DateTime>("FDate") || table1.Field<DateTime>("TDate") != table2.Field<DateTime>("TDate")
                                 select table2);

                    if (unchanged != null && unchanged.Count() > 0)
                        dtUnModifiedRecord = unchanged.CopyToDataTable();


                    newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                       join table1 in dtLocal.AsEnumerable()
                                       on table2.Field<string>("Link") equals table1.Field<string>("Link") into tg
                                       from tcheck in tg.DefaultIfEmpty()
                                       where tcheck == null
                                       select table2;

                    if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                        dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                    deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                      join table2 in dtExpert.AsEnumerable()
                                      on table1.Field<string>("Link") equals table2.Field<string>("Link") into tg
                                      from tcheck in tg.DefaultIfEmpty()
                                      where tcheck == null
                                      select table1;
                    if (deletefromlocal != null && deletefromlocal.Count() > 0)
                        dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                    break;
                #endregion

                #region RATE2
                case TableNames.OSRATE2:
                    changed = (from table1 in dtLocal.AsEnumerable()
                               join table2 in dtExpert.AsEnumerable() on table1.Field<string>("Link") equals table2.Field<string>("Link")
                               where table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("BatchNo") != table2.Field<string>("BatchNo") || table1.Field<string>("SL_Rate") != table2.Field<string>("SL_Rate") || table1.Field<string>("Remarks") != table2.Field<string>("Remarks") || table1.Field<decimal>("IT_Desc") != table2.Field<decimal>("IT_Desc") || table1.Field<decimal>("IT_Tax") != table2.Field<decimal>("IT_Tax") || table1.Field<decimal>("IT_OC") != table2.Field<decimal>("IT_OC") || table1.Field<string>("DSLab") != table2.Field<string>("DSLab")
                               select table1);

                    if (changed != null && changed.Count() > 0)
                        dtModifiedRecord = changed.CopyToDataTable();

                    unchanged = (from table2 in dtExpert.AsEnumerable()
                                 join table1 in dtLocal.AsEnumerable() on table2.Field<string>("Link") equals table1.Field<string>("Link")
                                 where table1.Field<string>("Link") != table2.Field<string>("Link") || table1.Field<string>("Code") != table2.Field<string>("Code") || table1.Field<string>("BatchNo") != table2.Field<string>("BatchNo") || table1.Field<string>("SL_Rate") != table2.Field<string>("SL_Rate") || table1.Field<string>("Remarks") != table2.Field<string>("Remarks") || table1.Field<decimal>("IT_Desc") != table2.Field<decimal>("IT_Desc") || table1.Field<decimal>("IT_Tax") != table2.Field<decimal>("IT_Tax") || table1.Field<decimal>("IT_OC") != table2.Field<decimal>("IT_OC") || table1.Field<string>("DSLab") != table2.Field<string>("DSLab")
                                 select table2);

                    if (unchanged != null && unchanged.Count() > 0)
                        dtUnModifiedRecord = unchanged.CopyToDataTable();


                    newinsertinlocal = from table2 in dtExpert.AsEnumerable()
                                       join table1 in dtLocal.AsEnumerable()
                                       on table2.Field<string>("Link") equals table1.Field<string>("Link") into tg
                                       from tcheck in tg.DefaultIfEmpty()
                                       where tcheck == null
                                       select table2;

                    if (newinsertinlocal != null && newinsertinlocal.Count() > 0)
                        dtNewlyInsertInLocal = newinsertinlocal.CopyToDataTable();

                    deletefromlocal = from table1 in dtLocal.AsEnumerable()
                                      join table2 in dtExpert.AsEnumerable()
                                      on table1.Field<string>("Link") equals table2.Field<string>("Link") into tg
                                      from tcheck in tg.DefaultIfEmpty()
                                      where tcheck == null
                                      select table1;
                    if (deletefromlocal != null && deletefromlocal.Count() > 0)
                        dtDeleteFromLocal = deletefromlocal.CopyToDataTable();
                    break;
                #endregion
            }
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            frmUserMaster user = new frmUserMaster();
            user.ShowDialog();
        }

        private void btnCompanies_Click(object sender, EventArgs e)
        {
            frmCompanyMaster company = new frmCompanyMaster();
            company.ShowDialog();
        }

        private void btnUserCompanyAllocation_Click(object sender, EventArgs e)
        {
            frmCompanyAllocation allocation = new frmCompanyAllocation();
            allocation.ShowDialog();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            frmChangePassword pw = new frmChangePassword();
            pw.ShowDialog();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            frmSettings settings = new frmSettings();
            settings.ShowDialog();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
                e.Cancel = true;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(100);
                this.Hide();
                return;
            }
            if (this.WindowState == FormWindowState.Minimized)
            {
                Application.Exit();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
