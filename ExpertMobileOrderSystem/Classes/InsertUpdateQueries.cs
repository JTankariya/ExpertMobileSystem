using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ExpertMobileOrderSystem.enums;

namespace ExpertMobileOrderSystem.Classes
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
                    if (cName.ToUpper() != "CLIENTCOMPANYID" && cName.ToUpper() != "OPERATIONFLAG" && cName.ToUpper() != "REFID")
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
                        if (cName.ToUpper() == "CLIENTCOMPANYID")
                        {
                            columnNames += "[ClientCompanyID],";
                            columnValues += ClientCompanyId + ",";
                        }
                    }
                }
                columnNames += "RefId,";
                columnValues += "NEWID(),";
                columnNames = columnNames.TrimEnd(',');
                columnValues = columnValues.TrimEnd(',');

                return "INSERT INTO [dbo].[" + tableName + "](" + columnNames + ") VALUES (" + columnValues + ")";
            }
            #endregion
            else
            {
                switch (tableName)
                {
                    case TableNames.OSPGROUP:
                    case TableNames.OSPRODUCT:
                    case TableNames.OSRATE:
                    case TableNames.OSRATE2:
                    case TableNames.OSACT:
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
                    default:
                        return "";
                }
                return "";
            }
        }

        public static string GetQueriesForExpert(TableNames tableName, OperationTypes operationType, DataRow drNew, DataRow drOld, int ClientCompanyId, List<string> columns)
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
                    if (cName.ToUpper() != "CLIENTCOMPANYID" && cName.ToUpper() != "OPERATIONFLAG" && cName.ToUpper() != "REFID")
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
                }
                columnNames = columnNames.TrimEnd(',');
                columnValues = columnValues.TrimEnd(',');

                return "INSERT INTO " + tableName.ToString().Substring(2, tableName.ToString().Length - 2) + "(" + columnNames + ") VALUES (" + columnValues + ")";
            }
            #endregion
            else
            {
                switch (tableName)
                {
                    case TableNames.OSPGROUP:
                    case TableNames.OSPRODUCT:
                    case TableNames.OSRATE:
                    case TableNames.OSRATE2:
                    case TableNames.OSACT:
                        if (operationType == OperationTypes.UPDATE)
                        {
                            var strQuery = GetUpdateQuery(columns, drNew, ClientCompanyId);
                            return "UPDATE [dbo].[" + tableName.ToString().Substring(2, tableName.ToString().Length - 2) + "] SET " + strQuery + " WHERE ClientCompanyId=" + ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''");
                        }
                        else if (operationType == OperationTypes.DELETE)
                        {
                            if (drOld != null)
                            {
                                return "Delete from [" + tableName.ToString().Substring(2, tableName.ToString().Length - 2) + "] where ClientCompanyId=" + ClientCompanyId + " and " + (!string.IsNullOrEmpty(Convert.ToString(drOld["Code"])) ? "Code='" + Convert.ToString(drOld["Code"]) + "'" : "IsNull(Code,'')=''");
                            }
                            else
                            {
                                return "Delete from [" + tableName.ToString().Substring(2, tableName.ToString().Length - 2) + "] where ClientCompanyId=" + ClientCompanyId;
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
                if (columns[i].Split('(')[0].ToString().ToUpper() == "CLIENTCOMPANYID")
                {
                    strQuery += "[ClientCompanyID]=" + ClientCompanyId + ",";
                }
                else if (columns[i].Split('(')[0].ToString().ToUpper() != "REFID")
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
            }
            strQuery = strQuery.TrimEnd(',');
            return strQuery;
        }
    }
}
