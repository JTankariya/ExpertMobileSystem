using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ExpertMobileSystem_Client_.enums;

namespace ExpertMobileSystem_Client_.Classes
{
    public class InsertUpdateQueries
    {
        public static string GetQueries(TableNames tableName, OperationTypes operationType, DataRow drNew, DataRow drOld, int ClientCompanyId, List<string> columns)
        {
            #region Insert
            if (operationType == OperationTypes.INSERT)
            {
                string columnNames = "";
                string columnValues = "";
                for (var i = 0; i < columns.Count; i++)
                {
                    string cName = columns[i].Split('(')[0];
                    string cType = columns[i].Split('(')[1].TrimEnd(')');
                    if (cName.ToUpper() != "CLIENTCOMPANYID")
                    {
                        columnNames += "[" + cName + "],";
                        if (cType == "smallint" || cType == "bit")
                        {
                            columnValues += (drNew[cName] != null && drNew[cName] != DBNull.Value ? "'" + (Convert.ToBoolean(drNew[cName]) ? "1" : "0") + "'," : "NULL,");
                        }
                        else if (cType == "datetime")
                        {
                            columnValues += (drNew[cName] != null && drNew[cName] != DBNull.Value ? "'" + Convert.ToDateTime(drNew[cName]).ToString("yyyy-MM-dd") + "'," : "NULL,");
                        }
                        else if (cType == "decimal")
                        {
                            columnValues += (drNew[cName] != null && drNew[cName] != DBNull.Value ? Convert.ToDouble(drNew[cName]) + "," : "NULL,");
                        }
                        else if (cType == "varchar")
                        {
                            columnValues += "'" + Operation.EscapeLikeValue(Convert.ToString(drNew[cName])) + "',";
                        }
                        else
                        {
                            columnValues += "'" + Operation.EscapeLikeValue(Convert.ToString(drNew[cName])) + "',";
                        }
                    }
                    else
                    {
                        columnNames += "[ClientCompanyID],";
                        columnValues += ClientCompanyId + ",";
                    }
                }
                columnNames = columnNames.TrimEnd(',');
                columnValues = columnValues.TrimEnd(',');

                return "INSERT INTO [dbo].[" + tableName + "](" + columnNames + ") VALUES (" + columnValues + ")";
            }
            #endregion
            else
            {
                switch (tableName)
                {
                    case TableNames.BATCH:
                    case TableNames.AGENT:
                    case TableNames.ACT:
                    case TableNames.CASHCUST:
                    case TableNames.FORMMAST:
                    case TableNames.GROUP:
                    case TableNames.PGROUP:
                    case TableNames.PRODUCT:
                    case TableNames.STAX:
                        if (operationType == OperationTypes.UPDATE)
                        {
                            var strQuery = GetUpdateQuery(columns, drNew, ClientCompanyId);
                            return "UPDATE [dbo].[" + tableName + "] SET " + strQuery + " WHERE ClientCompanyId=" + ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''");
                        }
                        else if (operationType == OperationTypes.DELETE)
                        {
                            if (drOld != null)
                            {
                                return "Delete from [" + tableName + "] where ClientCompanyId=" + ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''");
                            }
                            else
                            {
                                return "Delete from [" + tableName + "] where ClientCompanyId=" + ClientCompanyId;
                            }
                        }
                        break;
                    case TableNames.SP:
                        if (operationType == OperationTypes.UPDATE)
                        {
                            var strQuery = GetUpdateQuery(columns, drNew, ClientCompanyId);
                            return "UPDATE [dbo].[SP] SET " + strQuery + " WHERE ClientCompanyId=" + ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Date"])) ? "Date='" + Convert.ToDateTime(drOld["Date"]).ToString("yyyy/MM/dd") + "'" : "IsNull(Date,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DocNo"])) ? "DocNo='" + Convert.ToString(drOld["DocNo"]) + "'" : "IsNull(DocNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Division"])) ? "Division='" + Convert.ToString(drOld["Division"]) + "'" : "IsNull(Division,'')=''")+
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Bill_amt"])) ? "Bill_amt='" + Convert.ToString(drOld["Bill_amt"]) + "'" : "IsNull(Bill_amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["PSCode"])) ? "PSCode='" + Convert.ToString(drOld["PSCode"]) + "'" : "IsNull(PSCode,'')=''");
                        }
                        else if (operationType == OperationTypes.DELETE)
                        {
                            if (drOld != null)
                            {
                                return "Delete from SP where ClientCompanyId=" + ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Date"])) ? "Date='" + Convert.ToDateTime(drOld["Date"]).ToString("yyyy/MM/dd") + "'" : "IsNull(Date,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DocNo"])) ? "DocNo='" + Convert.ToString(drOld["DocNo"]) + "'" : "IsNull(DocNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Division"])) ? "Division='" + Convert.ToString(drOld["Division"]) + "'" : "IsNull(Division,'')=''")+
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Bill_amt"])) ? "Bill_amt='" + Convert.ToString(drOld["Bill_amt"]) + "'" : "IsNull(Bill_amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["PSCode"])) ? "PSCode='" + Convert.ToString(drOld["PSCode"]) + "'" : "IsNull(PSCode,'')=''");
                            }
                            else
                            {
                                return "Delete from SP where ClientCompanyId=" + ClientCompanyId;
                            }
                        }
                        break;
                    case TableNames.STOCK:
                        if (operationType == OperationTypes.UPDATE)
                        {
                            var strQuery = GetUpdateQuery(columns, drNew, ClientCompanyId);
                            return "UPDATE [dbo].[STOCK] SET " + strQuery + " WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["BatchNo"])) ? "BatchNo='" + Convert.ToString(drOld["BatchNo"]) + "'" : "IsNull(BatchNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Qty"])) ? "Qty=" + Convert.ToString(drOld["Qty"]) : "IsNull(Qty,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DocNo"])) ? "DocNo='" + Convert.ToString(drOld["DocNo"]) + "'" : "IsNull(DocNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Division"])) ? "Division='" + Convert.ToString(drOld["Division"]) + "'" : "IsNull(Division,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Narr1"])) ? "Narr1='" + Convert.ToString(drOld["Narr1"]) + "'" : "IsNull(Narr1,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Narr1"])) ? "Narr2='" + Convert.ToString(drOld["Narr2"]) + "'" : "IsNull(Narr2,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Value"])) ? "Value=" + Convert.ToString(drOld["Value"]) : "IsNull(Value,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["PSCode"])) ? "PSCode='" + Convert.ToString(drOld["PSCode"]) + "'" : "IsNull(PSCode,'')=''");
                        }
                        else if (operationType == OperationTypes.DELETE)
                        {
                            if (drOld != null)
                            {
                                return "Delete from STOCK WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["BatchNo"])) ? "BatchNo='" + Convert.ToString(drOld["BatchNo"]) + "'" : "IsNull(BatchNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Qty"])) ? "Qty=" + Convert.ToString(drOld["Qty"]) : "IsNull(Qty,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DocNo"])) ? "DocNo='" + Convert.ToString(drOld["DocNo"]) + "'" : "IsNull(DocNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Division"])) ? "Division='" + Convert.ToString(drOld["Division"]) + "'" : "IsNull(Division,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Narr1"])) ? "Narr1='" + Convert.ToString(drOld["Narr1"]) + "'" : "IsNull(Narr1,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Narr1"])) ? "Narr2='" + Convert.ToString(drOld["Narr2"]) + "'" : "IsNull(Narr2,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Value"])) ? "Value=" + Convert.ToString(drOld["Value"]) : "IsNull(Value,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["PSCode"])) ? "PSCode='" + Convert.ToString(drOld["PSCode"]) + "'" : "IsNull(PSCode,'')=''");
                            }
                            else
                            {
                                return "Delete from STOCK WHERE ClientCompanyId=" + ClientCompanyId;
                            }

                        }
                        break;
                    case TableNames.SALE_ADJ:
                        if (operationType == OperationTypes.UPDATE)
                        {
                            var strQuery = GetUpdateQuery(columns, drNew, ClientCompanyId);
                            return "UPDATE [dbo].[SALE_ADJ] SET " + strQuery + " WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["SP_Link"])) ? "SP_Link='" + Convert.ToString(drOld["SP_Link"]) + "'" : "IsNull(SP_Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["OT_Link"])) ? "OT_Link='" + Convert.ToString(drOld["OT_Link"]) + "'" : "IsNull(OT_Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Amt"])) ? "Amt='" + Convert.ToString(drOld["Amt"]) + "'" : "IsNull(Amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DC"])) ? "DC='" + Convert.ToString(drOld["DC"]) + "'" : "IsNull(DC,'')='')") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Disc"])) ? "Disc='" + Convert.ToString(drOld["Disc"]) + "'" : "IsNull(Disc,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''");
                        }
                        else if (operationType == OperationTypes.DELETE)
                        {
                            if (drOld != null)
                            {
                                return "Delete from SALE_ADJ WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["SP_Link"])) ? "SP_Link='" + Convert.ToString(drOld["SP_Link"]) + "'" : "IsNull(SP_Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["OT_Link"])) ? "OT_Link='" + Convert.ToString(drOld["OT_Link"]) + "'" : "IsNull(OT_Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Amt"])) ? "Amt='" + Convert.ToString(drOld["Amt"]) + "'" : "IsNull(Amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DC"])) ? "DC='" + Convert.ToString(drOld["DC"]) + "'" : "IsNull(DC,'')='')") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Disc"])) ? "Disc='" + Convert.ToString(drOld["Disc"]) + "'" : "IsNull(Disc,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''");
                            }
                            else
                            {
                                return "Delete from SALE_ADJ WHERE ClientCompanyId=" + ClientCompanyId;
                            }
                        }
                        break;
                    case TableNames.ORDER:
                        if (operationType == OperationTypes.UPDATE)
                        {
                            var strQuery = GetUpdateQuery(columns, drNew, ClientCompanyId);
                            return "UPDATE [dbo].[ORDER] SET " + strQuery + " WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Ord_No"])) ? "Ord_No='" + Convert.ToString(drOld["Ord_No"]) + "'" : "IsNull(Ord_No,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Ord_Dt"])) ? "[Ord_Dt]='" + Convert.ToDateTime(drOld["Ord_Dt"]).ToString("yyyy/MM/dd") + "'" : "IsNull(Ord_Dt,'')=''");
                        }
                        else if (operationType == OperationTypes.DELETE)
                        {
                            if (drOld != null)
                            {
                                return "Delete from [ORDER] WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Ord_No"])) ? "Ord_No='" + Convert.ToString(drOld["Ord_No"]) + "'" : "IsNull(Ord_No,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Ord_Dt"])) ? "[Ord_Dt]='" + Convert.ToDateTime(drOld["Ord_Dt"]).ToString("yyyy/MM/dd") + "'" : "IsNull(Ord_Dt,'')=''");
                            }
                            else
                            {
                                return "Delete from [ORDER] WHERE ClientCompanyId=" + ClientCompanyId;
                            }
                        }
                        break;
                    case TableNames.ORDER2:
                        if (operationType == OperationTypes.UPDATE)
                        {
                            var strQuery = GetUpdateQuery(columns, drNew, ClientCompanyId);
                            return "UPDATE [dbo].[ORDER2] SET " + strQuery + " WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Ord_No"])) ? "Ord_No='" + Convert.ToString(drOld["Ord_No"]) + "'" : "IsNull(Ord_No,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Ord_Dt"])) ? "[Ord_Dt]='" + Convert.ToDateTime(drOld["Ord_Dt"]).ToString("yyyy/MM/dd") + "'" : "IsNull(Ord_Dt,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Dly_Dt"])) ? "[Dly_Dt]='" + Convert.ToDateTime(drOld["Dly_Dt"]).ToString("yyyy/MM/dd") + "'" : "IsNull(Dly_Dt,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Qty"])) ? "Qty='" + Convert.ToString(drOld["Qty"]) + "'" : "IsNull(Qty,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Stk_Qty"])) ? "Stk_Qty='" + Convert.ToString(drOld["Stk_Qty"]) + "'" : "IsNull(Stk_Qty,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Division"])) ? "Division='" + Convert.ToString(drOld["Division"]) + "'" : "IsNull(Division,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Cv_Code"])) ? "Cv_Code='" + Convert.ToString(drOld["Cv_Code"]) + "'" : "IsNull(Cv_Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Batch_No"])) ? "Batch_No='" + Convert.ToString(drOld["Batch_No"]) + "'" : "IsNull(Batch_No,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Rate"])) ? "Rate='" + Convert.ToString(drOld["Rate"]) + "'" : "IsNull(Rate,0)=0");
                        }
                        else if (operationType == OperationTypes.DELETE)
                        {
                            if (drOld != null)
                            {
                                return "Delete from ORDER2 WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Ord_No"])) ? "Ord_No='" + Convert.ToString(drOld["Ord_No"]) + "'" : "IsNull(Ord_No,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Ord_Dt"])) ? "[Ord_Dt]='" + Convert.ToDateTime(drOld["Ord_Dt"]).ToString("yyyy/MM/dd") + "'" : "IsNull(Ord_Dt,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Dly_Dt"])) ? "[Dly_Dt]='" + Convert.ToDateTime(drOld["Dly_Dt"]).ToString("yyyy/MM/dd") + "'" : "IsNull(Dly_Dt,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Qty"])) ? "Qty='" + Convert.ToString(drOld["Qty"]) + "'" : "IsNull(Qty,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Stk_Qty"])) ? "Stk_Qty='" + Convert.ToString(drOld["Stk_Qty"]) + "'" : "IsNull(Stk_Qty,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Division"])) ? "Division='" + Convert.ToString(drOld["Division"]) + "'" : "IsNull(Division,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Cv_Code"])) ? "Cv_Code='" + Convert.ToString(drOld["Cv_Code"]) + "'" : "IsNull(Cv_Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Batch_No"])) ? "Batch_No='" + Convert.ToString(drOld["Batch_No"]) + "'" : "IsNull(Batch_No,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Rate"])) ? "Rate='" + Convert.ToString(drOld["Rate"]) + "'" : "IsNull(Rate,0)=0");
                            }
                            else
                            {
                                return "Delete from ORDER2 WHERE ClientCompanyId=" + ClientCompanyId;
                            }
                        }
                        break;
                    case TableNames.LEDMAST:
                        if (operationType == OperationTypes.UPDATE)
                        {
                            var strQuery = GetUpdateQuery(columns, drNew, ClientCompanyId);
                            return "UPDATE [dbo].[LEDMAST] SET " + strQuery + " WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Date"])) ? "[Date]='" + Convert.ToDateTime(drOld["Date"]).ToString("yyyy/MM/dd") + "'" : "IsNull(Date,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DocNo"])) ? "DocNo='" + Convert.ToString(drOld["DocNo"]) + "'" : "IsNull(DocNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Amt"])) ? "Amt='" + Convert.ToString(drOld["Amt"]) + "'" : "IsNull(Amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["PSCode"])) ? "PSCode='" + Convert.ToString(drOld["PSCode"]) + "'" : "IsNull(PSCode,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Division"])) ? "Division='" + Convert.ToString(drOld["Division"]) + "'" : "IsNull(Division,'')=''");
                        }
                        else if (operationType == OperationTypes.DELETE)
                        {
                            if (drOld != null)
                            {
                                return "Delete from LEDMAST WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Date"])) ? "[Date]='" + Convert.ToDateTime(drOld["Date"]).ToString("yyyy/MM/dd") + "'" : "IsNull(Date,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DocNo"])) ? "DocNo='" + Convert.ToString(drOld["DocNo"]) + "'" : "IsNull(DocNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Amt"])) ? "Amt='" + Convert.ToString(drOld["Amt"]) + "'" : "IsNull(Amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["PSCode"])) ? "PSCode='" + Convert.ToString(drOld["PSCode"]) + "'" : "IsNull(PSCode,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Division"])) ? "Division='" + Convert.ToString(drOld["Division"]) + "'" : "IsNull(Division,'')=''");
                            }
                            else
                            {
                                return "Delete from LEDMAST WHERE ClientCompanyId=" + ClientCompanyId;
                            }
                        }
                        break;
                    case TableNames.LEDGER:
                        if (operationType == OperationTypes.UPDATE)
                        {
                            var strQuery = GetUpdateQuery(columns, drNew, ClientCompanyId);
                            return "UPDATE [dbo].[LEDGER] SET " + strQuery + " WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Date"])) ? "[Date]='" + Convert.ToDateTime(drOld["Date"]).ToString("yyyy/MM/dd") + "'" : "IsNull(Date,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["ChDt"])) ? "[ChDt]='" + Convert.ToDateTime(drOld["ChDt"]).ToString("yyyy/MM/dd") + "'" : "IsNull(ChDt,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DocNo"])) ? "DocNo='" + Convert.ToString(drOld["DocNo"]) + "'" : "IsNull(DocNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["VatCode"])) ? "VatCode='" + Convert.ToString(drOld["VatCode"]) + "'" : "IsNull(VatCode,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DC"])) ? "DC='" + Convert.ToString(drOld["DC"]) + "'" : "IsNull(DC,'')='')") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["VType"])) ? "VType='" + Convert.ToString(drOld["VType"]) + "'" : "IsNull(VType,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Narr1"])) ? "Narr1='" + Convert.ToString(drOld["Narr1"]) + "'" : "IsNull(Narr1,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Narr2"])) ? "Narr2='" + Convert.ToString(drOld["Narr2"]) + "'" : "IsNull(Narr2,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Narr3"])) ? "Narr3='" + Convert.ToString(drOld["Narr3"]) + "'" : "IsNull(Narr3,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Narr4"])) ? "Narr4='" + Convert.ToString(drOld["Narr4"]) + "'" : "IsNull(Narr4,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Contra"])) ? "Contra='" + Convert.ToString(drOld["Contra"]) + "'" : "IsNull(Contra,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Adj_Type"])) ? "Adj_Type='" + Convert.ToString(drOld["Adj_Type"]) + "'" : "IsNull(Adj_Type,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Adj_Amt"])) ? "Adj_Amt='" + Convert.ToString(drOld["Adj_Amt"]) + "'" : "IsNull(Adj_Amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Amt"])) ? "Amt='" + Convert.ToString(drOld["Amt"]) + "'" : "IsNull(Amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["RCode"])) ? "RCode='" + Convert.ToString(drOld["RCode"]) + "'" : "IsNull(RCode,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["CHNO"])) ? "CHNO='" + Convert.ToString(drOld["CHNO"]) + "'" : "IsNull(CHNO,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["ChBank"])) ? "ChBank='" + Convert.ToString(drOld["ChBank"]) + "'" : "IsNull(ChBank,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Division"])) ? "Division='" + Convert.ToString(drOld["Division"]) + "'" : "IsNull(Division,'')=''");
                        }
                        else if (operationType == OperationTypes.DELETE)
                        {
                            if (drOld != null)
                            {
                                return "Delete from LEDGER WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Date"])) ? "[Date]='" + Convert.ToDateTime(drOld["Date"]).ToString("yyyy/MM/dd") + "'" : "IsNull(Date,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["ChDt"])) ? "[ChDt]='" + Convert.ToDateTime(drOld["ChDt"]).ToString("yyyy/MM/dd") + "'" : "IsNull(ChDt,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DocNo"])) ? "DocNo='" + Convert.ToString(drOld["DocNo"]) + "'" : "IsNull(DocNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["VatCode"])) ? "VatCode='" + Convert.ToString(drOld["VatCode"]) + "'" : "IsNull(VatCode,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DC"])) ? "DC='" + Convert.ToString(drOld["DC"]) + "'" : "IsNull(DC,'')='')") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["VType"])) ? "VType='" + Convert.ToString(drOld["VType"]) + "'" : "IsNull(VType,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Narr1"])) ? "Narr1='" + Convert.ToString(drOld["Narr1"]) + "'" : "IsNull(Narr1,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Narr2"])) ? "Narr2='" + Convert.ToString(drOld["Narr2"]) + "'" : "IsNull(Narr2,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Narr3"])) ? "Narr3='" + Convert.ToString(drOld["Narr3"]) + "'" : "IsNull(Narr3,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Narr4"])) ? "Narr4='" + Convert.ToString(drOld["Narr4"]) + "'" : "IsNull(Narr4,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Contra"])) ? "Contra='" + Convert.ToString(drOld["Contra"]) + "'" : "IsNull(Contra,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Adj_Type"])) ? "Adj_Type='" + Convert.ToString(drOld["Adj_Type"]) + "'" : "IsNull(Adj_Type,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Adj_Amt"])) ? "Adj_Amt='" + Convert.ToString(drOld["Adj_Amt"]) + "'" : "IsNull(Adj_Amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Amt"])) ? "Amt='" + Convert.ToString(drOld["Amt"]) + "'" : "IsNull(Amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["RCode"])) ? "RCode='" + Convert.ToString(drOld["RCode"]) + "'" : "IsNull(RCode,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["CHNO"])) ? "CHNO='" + Convert.ToString(drOld["CHNO"]) + "'" : "IsNull(CHNO,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["ChBank"])) ? "ChBank='" + Convert.ToString(drOld["ChBank"]) + "'" : "IsNull(ChBank,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Division"])) ? "Division='" + Convert.ToString(drOld["Division"]) + "'" : "IsNull(Division,'')=''");
                            }
                            else
                            {
                                return "Delete from LEDGER WHERE ClientCompanyId=" + ClientCompanyId;
                            }
                        }
                        break;
                    case TableNames.ADVANCE:
                        if (operationType == OperationTypes.UPDATE)
                        {
                            var strQuery = GetUpdateQuery(columns, drNew, ClientCompanyId);
                            return "UPDATE [dbo].[ADVANCE] SET " + strQuery + " WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Date"])) ? "[Date]='" + Convert.ToDateTime(drOld["Date"]).ToString("dd/MM/yyyy") + "'" : "IsNull(Date,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DocNo"])) ? "DocNo='" + Convert.ToString(drOld["DocNo"]) + "'" : "IsNUll(DocNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Amt"])) ? "Amt='" + Convert.ToString(drOld["Amt"]) + "'" : "IsNull(Amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DC"])) ? "DC='" + Convert.ToString(drOld["DC"]) + "'" : "IsNull(DC,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Division"])) ? "Division='" + Convert.ToString(drOld["Division"]) + "'" : "IsNull(Division,'')=''");
                        }
                        else if (operationType == OperationTypes.DELETE)
                        {
                            if (drOld != null)
                            {
                                return "Delete from ADVANCE WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Date"])) ? "[Date]='" + Convert.ToDateTime(drOld["Date"]).ToString("dd/MM/yyyy") + "'" : "IsNull(Date,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DocNo"])) ? "DocNo='" + Convert.ToString(drOld["DocNo"]) + "'" : "IsNUll(DocNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Amt"])) ? "Amt='" + Convert.ToString(drOld["Amt"]) + "'" : "IsNull(Amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["DC"])) ? "DC='" + Convert.ToString(drOld["DC"]) + "'" : "IsNull(DC,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Division"])) ? "Division='" + Convert.ToString(drOld["Division"]) + "'" : "IsNull(Division,'')=''");
                            }
                            else
                            {
                                return "Delete from ADVANCE WHERE ClientCompanyId=" + ClientCompanyId;
                            }
                        }
                        break;
                    case TableNames.ITVAT:
                        if (operationType == OperationTypes.UPDATE)
                        {
                            var strQuery = GetUpdateQuery(columns, drNew, ClientCompanyId);
                            return "UPDATE [dbo].[ITVAT] SET " + strQuery + " WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["VatCode"])) ? "VatCode='" + Convert.ToString(drOld["VatCode"]) + "'" : "IsNull(VatCode,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Amt"])) ? "Amt='" + Convert.ToString(drOld["Amt"]) + "'" : "IsNull(Amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Vat"])) ? "Vat='" + Convert.ToString(drOld["Vat"]) + "'" : "IsNull(Vat,'')=''");
                        }
                        else if (operationType == OperationTypes.DELETE)
                        {
                            if (drOld != null)
                            {
                                return "Delete from ITVAT WHERE ClientCompanyId=" +
ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["VatCode"])) ? "VatCode='" + Convert.ToString(drOld["VatCode"]) + "'" : "IsNull(VatCode,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Amt"])) ? "Amt='" + Convert.ToString(drOld["Amt"]) + "'" : "IsNull(Amt,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Vat"])) ? "Vat='" + Convert.ToString(drOld["Vat"]) + "'" : "IsNull(Vat,'')=''");
                            }
                            else
                            {
                                return "Delete from ITVAT WHERE ClientCompanyId=" + ClientCompanyId;
                            }
                        }
                        break;
                    case TableNames.SERIAL:
                        if (operationType == OperationTypes.UPDATE)
                        {
                            var strQuery = GetUpdateQuery(columns, drNew, ClientCompanyId);
                            return "UPDATE [dbo].[SERIAL] SET " + strQuery + " WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["BatchNo"])) ? "BatchNo='" + Convert.ToString(drOld["BatchNo"]) + "'" : "IsNull(BatchNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["SerialNo"])) ? "[SerialNo]='" + Convert.ToString(drOld["SerialNo"]) + "'" : "IsNull(SerialNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Qty"])) ? "Qty='" + Convert.ToString(drOld["Qty"]) + "'" : "IsNull(Qty,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Posifile"])) ? "Posifile='" + Convert.ToString(drOld["Posifile"]) + "'" : "IsNull(Posifile,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''");
                        }
                        else if (operationType == OperationTypes.DELETE)
                        {
                            if (drOld != null)
                            {
                                return "Delete from SERIAL WHERE ClientCompanyId=" +
                                ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["BatchNo"])) ? "BatchNo='" + Convert.ToString(drOld["BatchNo"]) + "'" : "IsNull(BatchNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["SerialNo"])) ? "[SerialNo]='" + Convert.ToString(drOld["SerialNo"]) + "'" : "IsNull(SerialNo,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Qty"])) ? "Qty='" + Convert.ToString(drOld["Qty"]) + "'" : "IsNull(Qty,0)=0") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Posifile"])) ? "Posifile='" + Convert.ToString(drOld["Posifile"]) + "'" : "IsNull(Posifile,'')=''") +
                            " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Link"])) ? "Link='" + Convert.ToString(drOld["Link"]) + "'" : "IsNull(Link,'')=''");
                            }
                            else
                            {
                                return "Delete from SERIAL WHERE ClientCompanyId=" + ClientCompanyId;
                            }
                        }
                        break;
                    default:
                        return "";
                }
                return "";
            }
        }

        private static string GetUpdateQuery(List<string> columns, DataRow drNew, int ClientCompanyId)
        {
            var strQuery = "";
            for (var i = 0; i < columns.Count; i++)
            {
                if (columns[i].Split('(')[0].ToString().ToUpper() != "CLIENTCOMPANYID")
                {
                    if (drNew[columns[i].Split('(')[0]].GetType() == typeof(Boolean))
                    {
                        strQuery += "[" + columns[i].Split('(')[0] + "]='" + (Convert.ToBoolean(drNew[columns[i].Split('(')[0]]) ? "1" : "0") + "',";
                    }
                    else if (drNew[columns[i].Split('(')[0]].GetType() == typeof(DateTime))
                    {
                        strQuery += "[" + columns[i].Split('(')[0] + "]='" + Convert.ToDateTime(drNew[columns[i].Split('(')[0]]).ToString("yyyy-MM-dd") + "',";
                    }
                    else
                    {
                        strQuery += "[" + columns[i].Split('(')[0] + "]='" + Operation.EscapeLikeValue(Convert.ToString(drNew[columns[i].Split('(')[0]])) + "',";
                    }
                }
                else
                {
                    strQuery += "[ClientCompanyID]=" + ClientCompanyId + ",";
                }
            }
            strQuery = strQuery.TrimEnd(',');
            return strQuery;
        }
    }
}
