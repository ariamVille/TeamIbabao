using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace mainForm
{
    public partial class Form1 : Form
    {
        private bool dragging = false;
        private Point offset;
        private Point startPoint = new Point(0, 0);

        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;Initial Catalog='poscan';username=root;password=");
        MySqlCommand command;
        MySqlDataReader reader;
        int seconds = 6;
        string name = "", currentAccountType = "";
 
        int ctr2 = 0;
        public Form1()
        {
            InitializeComponent();
        
            //adminForm a = new adminForm();
            //a.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            testConnection();
        }

        //testing the connection
        void testConnection()
        {
            try
            {
                timer1.Stop();
                connection.Open();
                lblConnection.Text = "Connection State: Connected";
                checkIfSystemIsLocked();
                connection.Close();
            }
            catch (Exception)
            {
                lblConnection.Text = "Connection State: Not Connected";
                timer1.Start();
            }
        }

        //checking if system is locked
        void checkIfSystemIsLocked()
        {
            try
            {
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT systemislocked from systemsettings where id=1;";
                string result = command.ExecuteScalar().ToString();
                if (result.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    panel2.Show();
                }
                else
                {
                    panel1.Visible = false;
                }
                closeConnection();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to check system state");
                closeConnection();
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
/*
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

*/
/*
        //calling the on screen keyboard for user inputs
        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("osk.exe");
        
       
            button2.FlatAppearance.MouseOverBackColor = button2.BackColor;
        }*/

        //user log in
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                if (!txtUser.Text.Trim().Equals("") && !txtPass.Text.Trim().Equals(""))
                {
                    if (lblConnection.Text.Equals("Connection State: Connected"))
                    {
                        string log = "";
                        int ctr = 0;
                        string id = "", pass = "", empID = "";
                        openConnection();

                        string qry1 = "SELECT * FROM tblaccounts WHERE username='" + txtUser.Text + "' and password='" + txtPass.Text + "';";
                        command = new MySqlCommand();
                        command.Connection = connection;
                        command.CommandText = qry1;
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ctr++;
                            empID = reader["employeeID"].ToString();
                            log = reader["username"].ToString();
                            name = reader["firstname"].ToString() + " " + reader["lastname"].ToString();
                            id = reader["username"].ToString();
                            pass = reader["password"].ToString();
                            currentAccountType = reader["accountype"].ToString();
                        }
                        closeConnection();

                        if (ctr == 1) 
                        {
                            if (doublecheck(txtUser.Text, txtPass.Text, id, pass) == true)
                            {
                                openConnection();
                                command = new MySqlCommand();
                                command.CommandText = "INSERT INTO tblaudit(accountType,username,date,time,action) values('" + currentAccountType + "','" + log + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','log-in');";
                                command.Connection = connection;
                                command.ExecuteNonQuery();
                                closeConnection();

                                //EmployeeForm emp = new EmployeeForm(txtUser.Text);
                                //emp.loggedUserID = log;
                                //emp.loggedUserName = name;
                                //emp.Show();
                                adminForm m = new adminForm(txtUser.Text);
                                m.loggedUserID = log;
                                m.loggedUserName = name;
                                m.loggedUserAccountType = currentAccountType;
                                m.loggedUserEMployeeId = empID;
                                m.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Wrong userID or password");
                                lblTries.Text = (Convert.ToInt32(lblTries.Text) - 1).ToString();

                                if (lblTries.Text.Equals("1"))
                                {
                                    MessageBox.Show("You only have 1 try left, system will lockdown if you failed again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else if (lblTries.Text.Equals("0"))
                                {
                                    openConnection();
                                    command = new MySqlCommand();
                                    command.Connection = connection;
                                    command.CommandText = "UPDATE systemsettings SET systemislocked ='true' where id=1;";
                                    command.ExecuteNonQuery();
                                    closeConnection();
                                    MessageBox.Show("System will now lockdown", "Oops");
                                    panel2.Visible = true;
                                    panel1.Visible = true;
                                    panel1.Dock = DockStyle.Fill;
                                }
                            }
                        }
                        else if (ctr > 1)
                        {
                            MessageBox.Show("Your account have a duplicate");
                        }
                        else
                        {
                            ctr = 0;
                            openConnection();
                            command = new MySqlCommand();
                            command.CommandText = "SELECT * FROM tbladmin WHERE username='" + txtUser.Text + "' and password='" + txtPass.Text + "';";
                            command.Connection = connection;
                            MySqlDataReader reader2 = command.ExecuteReader();

                            while (reader2.Read())
                            {
                                ctr++;
                                empID = reader2["adminID"].ToString();
                                id = reader2["username"].ToString();
                                pass = reader2["password"].ToString();
                                log = reader2["username"].ToString();
                                name = reader2["firstname"].ToString() + " " + reader2["lastname"].ToString();
                                currentAccountType = reader2["accountype"].ToString();

                            }

                            if (ctr == 1)
                            {
                                //  MessageBox.Show("Original: " + id + " " + pass + "\nEntered: " + txtuserid.Text + " " + txtpassword.Text,"Admin");
                                if (doublecheck(txtUser.Text, txtPass.Text, id, pass) == true)
                                {
                                    openConnection();
                                    command = new MySqlCommand();
                                    command.CommandText = "INSERT INTO tblaudit(accountType,username,date,time,action) values('"+currentAccountType+"','" + log + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','log-in');";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    closeConnection();

                                    adminForm m = new adminForm(txtUser.Text);
                                    m.loggedUserID = log;
                                    m.loggedUserName = name;
                                    m.loggedUserAccountType = currentAccountType;
                                    m.loggedUserEMployeeId = empID;
                                    m.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("Wrong userID or password");
                                    lblTries.Text = (Convert.ToInt32(lblTries.Text) - 1).ToString();

                                    if (lblTries.Text.Equals("1"))
                                    {
                                        MessageBox.Show("You only have 1 try left, system will lockdown if you failed again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else if (lblTries.Text.Equals("0"))
                                    {
                                        openConnection();
                                        command = new MySqlCommand();
                                        command.Connection = connection;
                                        command.CommandText = "UPDATE systemsettings SET systemislocked ='true' where id=1;";
                                        command.ExecuteNonQuery();
                                        closeConnection();
                                        MessageBox.Show("System will now lockdown", "Oops");
                                        panel2.Visible = true;

                                        panel1.Dock = DockStyle.Fill;
                                        panel1.Visible = true;
                                    }
                                }

                            }
                            else if (ctr > 1)
                            {
                                MessageBox.Show("Your account have a duplicate");
                            }
                            else
                            {
                                MessageBox.Show("Wrong userID or password");
                                lblTries.Text = (Convert.ToInt32(lblTries.Text) - 1).ToString();

                                if (lblTries.Text.Equals("1"))
                                {
                                    MessageBox.Show("You only have 1 try left, system will lockdown if you failed again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else if (lblTries.Text.Equals("0"))
                                {
                                    openConnection();
                                    command = new MySqlCommand();
                                    command.Connection = connection;
                                    command.CommandText = "UPDATE systemsettings SET systemislocked ='true' where id=1;";
                                    command.ExecuteNonQuery();
                                    closeConnection();
                                    MessageBox.Show("System will now lockdown", "Oops");
                                    panel2.Visible = true;
                                    panel1.Dock = DockStyle.Fill;
                                    panel1.Visible = true;
                                }
                            }
                            closeConnection();
                        }
                    }


                    else
                    {
                        MessageBox.Show("System is not connected to database", "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please fill all the fields");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Failed");
            }
        }

        //double check log in credentials
        bool doublecheck(string inputid, string inputpass, string id, string pass)
        {
            bool test = false;
            if (inputid.Equals(id) && inputpass.Equals(pass))
            {
                test = true;
            }
            return test;
        }


        //number of attempts for log in
        public void attempts()
        {
            if (ctr2 == 3)
            {
                MessageBox.Show("You Have reach the maximum number of attempts");
                this.Close();
            }

        }

        //unmask password/show password
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtPass.UseSystemPasswordChar = false ;
            }
            else
            {
                txtPass.UseSystemPasswordChar=true;
            }
        }

    /*    private void checkBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                button1.PerformClick();
            }
        }*/

        //keypress insted of using mouse 
        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        //unlocking the system
        private void btnEnter_unlock_Click(object sender, EventArgs e)
        {
            try
            {
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select unlockKey from systemsettings where Id=1;";
                string result = command.ExecuteScalar().ToString();
                if (result.Equals(txtUnlock.Text))
                {
                    txtPass.Text = "";
                    txtUser.Text = "";
                   // txtuserid.BackColor = Color.White;
                   // txtpassword.BackColor = Color.White;
                    button1.Enabled = true;
                    panel1.Visible = false;
                    lblTries.Text = "3";
                    txtUnlock.Text = "";
                    command.CommandText = "UPDATE systemsettings SET systemislocked='false' where id=1;";
                    command.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("Wrong unlock Key");
                }
                closeConnection();
            }
            catch (Exception)
            {
                
            }
        }

        //timer for connection
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            --seconds;
            lblConnection.Text = "Connecting in " + seconds;

            if (seconds == 0)
            {
                testConnection();
                seconds = 6;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Do you really want to exit?", "Dialog Title", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            if (checkForSpecialChar(txtUser.Text) == false)
            {
                txtUser.BackColor = Color.LightCoral;
                MessageBox.Show("Oops, some special symbols are not allowed", "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.Enabled = false;
                txtUser.Text = "";
            }
            else
            {
                txtUser.BackColor = Color.White;
                button1.Enabled = true;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.startPoint.X, p.Y - this.startPoint.Y);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        bool checkForSpecialChar(string text)
        {
            bool result = true;
            var regex = new Regex("^[a-zA-Z0-9_ -]*$");
            if (!regex.IsMatch(text))
            {
                result = false;
            }
            return result;
        }

    }
}
