using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace mainForm
{
    public partial class identityCheck : Form
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;Initial Catalog='poscan';username=root;password=");
        MySqlCommand command;
        MySqlDataReader reader;
    

        private bool dragging = false;
        private Point offset;
        private Point startPoint = new Point(0, 0);
        

        public identityCheck()
        {
            InitializeComponent();
        }

        private void identityCheck_Load(object sender, EventArgs e)
        {
            testConnection();
        }


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

        private void btnVerify_Click(object sender, EventArgs e)
        {
            MySqlDataAdapter ad;
            openConnection();
            command = new MySqlCommand();
            command.CommandText = "select * from tbldummyaccounts where accountNumber = '" + textBox1.Text + "'";
            command.Connection = connection;
            command.ExecuteNonQuery();
            try {
                ad = new MySqlDataAdapter(command);

                DataTable table = new DataTable();
                ad.Fill(table);
                byte[] img = (byte[])table.Rows[0][3];
                MemoryStream ms = new MemoryStream(img);
                pictureBox1.Image = Image.FromStream(ms);

                ad.Dispose();
            }
            catch(Exception ex)
            {
                pictureBox1.Image = null;
                MessageBox.Show("No Available Data");
            }

        }

        private void lblExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bar_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);
        }

        private void bar_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.startPoint.X, p.Y - this.startPoint.Y);
            }
        }

        private void bar_MouseUp(object sender, MouseEventArgs e)
        {
           dragging = false;
        }

        private void bar_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
