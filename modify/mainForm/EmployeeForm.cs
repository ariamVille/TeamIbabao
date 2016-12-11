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
    public partial class EmployeeForm : Form{

        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;Initial Catalog='poscan';username=root;password=");
        MySqlCommand command;

        int seconds = 6;

        public string loggedUserID = "";
        public string loggedUserName = "";


        public EmployeeForm(string username)
        {
            InitializeComponent();
            lblEwan.Text = username;
            Name = username;
            lbl1.Text = username;
            lbl2.Text = username;
            lbl3.Text = username;
            lbl4.Text = username;
            lbl6.Text = username;
            lbl7.Text = username;
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
          //  populateProducts();
            timerBlink.Start();
            TimeDate.Start();
            lblTime.Text = DateTime.Now.ToLongTimeString();

            testConnection();
          //  AutoCompleteText();
            gBdashboard.Show();
            gbVtbl.Visible = false;
            gbEditProf.Visible = false;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void button1_Click(object sender, EventArgs e)
        {
            gBdashboard.Visible = true;
            gbVtbl.Visible = false;
            gbEditProf.Visible = false;
            gbInventory.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gbVtbl.Visible = true;
            gBdashboard.Visible = false;
            gbEditProf.Visible = false;
            gbInventory.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            gbEditProf.Visible = true;
            gbVtbl.Visible = false;
            gBdashboard.Visible = false;
            gbInventory.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
          //  populateProducts();
            gbInventory.Visible = true;
            gbEditProf.Visible = false;
            gbVtbl.Visible = false;
            gBdashboard.Visible = false;

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }
    }
}
