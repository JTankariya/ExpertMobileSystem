using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace ExpertMobileSystem_Client_
{
    public partial class ConnectToMySQL : Form
    {
        public ConnectToMySQL()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connection;
            SqlCommand command;
           
            connection = new SqlConnection(Operation.ConnStr);
            try
            {
                connection.Open();
             //   MessageBox.Show("Connection Established");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            try
            {
                DataTable dt = new DataTable();
                command = new SqlCommand("Select * From AdminMaster", connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
                connection.Close();
                da.Dispose();

                string test = GenSQL.BuildInsertSQL(dt);
                MessageBox.Show("Completed" + Environment.NewLine + test);

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
