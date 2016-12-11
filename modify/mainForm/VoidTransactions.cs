using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace mainForm
{
    public partial class VoidTransactions : Form
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;Initial Catalog='finalproj';username=root;password=root");
        MySqlCommand command;
        public VoidTransactions()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {

            connection.Open();
            command.CommandText = "SELECT pincode FROM tbladmin WHERE pincode='" + txtPin.Text + "';";

            MySqlDataReader productsSqlDataReader = command.ExecuteReader();

            int productIDColPos = productsSqlDataReader.GetOrdinal("pincode");


            if (productsSqlDataReader.Read())
            {
                
                MessageBox.Show("FUCK! Pincode Updated!");

                connection.Close();
            }

            else
            {
                MessageBox.Show("invalid pincode");
            }
        }
    }
}
