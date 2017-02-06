using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;


namespace ExpertMobileOrderSystem
{
    public class LocalConnection
    {
        public static OleDbConnection ExpertCompanyConn = null;
        public static OleDbConnection TempCompanyConn = null;
        public static string tempQuery = "";

       // public static Company

        public static void Bindgrid(string selectcommond, DataGridView supplydatagrid)
        {
            DataTable dt = GetDataTable(selectcommond, ExpertCompanyConn);
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
            catch (Exception ex)
            {
                //MessageBox.Show("Error Ocuured", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                throw ex;
                //return dt;
            }
            finally
            {
                if (TempConn.State == ConnectionState.Open)
                    TempConn.Close();
            }
        }

        public static DataTable GetDataTable(string Query, OleDbConnection TempConn)
        {
            if (TempConn.State == ConnectionState.Closed)
                TempConn.Open();
            DataTable dt = new DataTable();
            try
            {
                OleDbCommand cmd = new OleDbCommand(Query, TempConn);
                OleDbDataAdapter adp = new OleDbDataAdapter(cmd);
                adp.Fill(dt);
                return dt;
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Error Ocuured", MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                throw ex;
                //return dt;
            }
            finally
            {
                if (TempConn.State == ConnectionState.Open)
                    TempConn.Close();
            }
        }


    }
}
