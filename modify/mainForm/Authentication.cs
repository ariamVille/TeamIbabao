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
    public partial class Authentication : Form
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;Initial Catalog='finalproj';username=root;password=root");
        MySqlCommand command;
        MySqlDataReader reader;
        int seconds = 6;
       // string name = "", currentAccountType = "";
 

        public Authentication()
        {
            InitializeComponent();
        }

        //testing the connection
        void testConnection()
        {
            try
            {
                timer1.Stop();
                connection.Open();
                lblConnection.Text = "Connection State: Connected";
                connection.Close();
            }
            catch (Exception)
            {
                lblConnection.Text = "Connection State: Not Connected";
                timer1.Start();
            }
        }

      

        //opening the connection
        void openConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                else
                {
                    connection.Close();
                    connection.Open();
                }
            }
            catch (Exception)
            {
                connection.Close();
                MessageBox.Show("Error in opening connection", "openConnection");
            }
        }

        //closing the connection
        void closeConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in closing connection", "closeConnection");
            }
        }

        //timer for connection
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblConnection.Text = "Connecting in " + seconds;

            if (seconds == 0)
            {
                testConnection();
                seconds = 6;
            }
            --seconds;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openConnection();
            command.CommandText = "SELECT pincode FROM tbladmin WHERE pincode='" + textBox1.Text + "'";
            MySqlDataReader dReader = command.ExecuteReader();
            while (dReader.Read())
            {
                label1.Text = dReader["username"].ToString();
           
             
            }
                 if (!label1.Text.Equals(textBox1.Text))
                    {
                       
                           VoidTransactions v = new VoidTransactions();
                                    v.Show();
                                    this.Hide();

                                    closeConnection();
                    }
                  
                        else
                        {
                            MessageBox.Show("invalid pincode");
                        }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            //txtBoxChangePinOld.Text = "";

            openConnection();
            command.CommandText = "SELECT pincode FROM tbladmin WHERE pincode='" + txtBoxChangePinOld.Text + "';";

            MySqlDataReader productsSqlDataReader = command.ExecuteReader();

            int productIDColPos = productsSqlDataReader.GetOrdinal("pincode");


            if (productsSqlDataReader.Read())
            {
               
                MessageBox.Show("FUCK! Pincode Updated!");

                closeConnection();
            }

            else
            {
                MessageBox.Show("invalid pincode");
            }
        }





    }
}
