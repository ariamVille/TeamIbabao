using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
//using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

using System.Security.Cryptography;


namespace mainForm
{
    public partial class adminForm : Form
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;Initial Catalog='poscan';username=root;password=");
        MySqlCommand command;
        MySqlDataReader reader;
        int seconds = 6;
        public string loggedUserID = "";
        public string loggedUserName = "";
        public string loggedUserAccountType = "";
        public string loggedUserEMployeeId = "";
        int index, counter = 0, counter2 = 0;
        int blink = 0; bool up = true;
        string fileName;
        string theBDateEditProf;
        List<tblaccount> list;

        string Name = "", currentAccountType = "";
        DataTable dt;
        DataView dv;
        public adminForm(string username)
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

        //check logged in user
        private void adminForm_Load(object sender, EventArgs e)
        {
            lblrestrictor.Text = loggedUserAccountType;

            if (lblrestrictor.Text.Equals("Employee"))
            {
                button2.Enabled = false;
                button5.Enabled = false;
                button18.Enabled = false;
                button28.Enabled = false;
                button27.Enabled = false;
                button3.Enabled = false;
                lblForAdmin.Hide();
                lblrestrictor.Hide();
                button18.Hide();
                button28.Hide();
                button27.Hide();
                button2.Hide();
                button5.Hide();
                button3.Hide();
                button6.Hide();
            }
            else{
                timerWelcome.Start();
                lblrestrictor.Hide();
                button2.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button18.Show();
                button28.Show();
                button27.Show();
                button2.Show();
                button5.Show();
                button3.Show();
                button6.Show();
            }

            populateProducts();
            timerBlink.Start();
            TimeDate.Start();
            lblTime.Text = DateTime.Now.ToLongTimeString();
           
            testConnection();
            AutoCompleteText();
            gbRegistration.Visible = false;
            gBdashboard.Show();
            gbVtbl.Visible = false;
            gbEditProf.Visible = false;
            gbCpass.Visible = false;
            txtPCode.Enabled = false;
            grpBoxForReports.Visible = false;
           // gbInventory.Enabled = false;

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

        //buttons for group boxes
        private void button1_Click(object sender, EventArgs e)
        {
            gBdashboard.Visible = true;
            gbRegistration.Visible = false;
            gbVtbl.Visible = false;
            gbEditProf.Visible = false;
            gbCpass.Visible = false;
            gbInventory.Visible = false;
            grpBoxForReports.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gbRegistration.Visible = true;
            gBdashboard.Visible = false;
            gbVtbl.Visible = false;
            gbEditProf.Visible = false;
            gbCpass.Visible = false;
            gbInventory.Visible = false;
           
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            gbVtbl.Visible = true;
            gbRegistration.Visible = false;
            gBdashboard.Visible = false;
            gbEditProf.Visible = false;
            gbCpass.Visible = false;
            gbInventory.Visible = false;
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            gbEditProf.Visible = true;
            gbVtbl.Visible = false;
            gbRegistration.Visible = false;
            gBdashboard.Visible = false;
            gbCpass.Visible = false;
            gbInventory.Visible = false;
            grpBoxForReports.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            gbCpass.Visible = true;
            gbEditProf.Visible = false;
            gbVtbl.Visible = false;
            gbRegistration.Visible = false;
            gBdashboard.Visible = false;
            gbInventory.Visible = false;
            grpBoxForReports.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            populateProducts();
            gbInventory.Visible = true;
            gbCpass.Visible = false;
            gbEditProf.Visible = false;
            gbVtbl.Visible = false;
            gbRegistration.Visible = false;
            gBdashboard.Visible = false;
            grpBoxForReports.Visible = false;
             

        }

      

        //uploading pictures
        private void btnPic_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|All Files(*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK) {
                string picLoc = dlg.FileName.ToString();
                txtxPhotoPath.Text = picLoc;
                pictureBox1.ImageLocation = picLoc;
            }

            

        }
             

        //save button from registration
        private void button17_Click(object sender, EventArgs e)
        {

            string valueNull = "No Data";
          

            try
            {
                string selectedItem = cbbType.Text;
                string theBDateReg = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            if (selectedItem.Equals("Employee"))
            {

                if (!txtxPhotoPath.Text.Equals(""))
                {
                    openConnection();
                    command.CommandText = "SELECT username FROM tblaccounts WHERE username='" + txtUsername.Text + "'";
                    MySqlDataReader dReader = command.ExecuteReader();
                    if (dReader.Read())
                    {
                        userHolder.Text = dReader["username"].ToString();
                    }

                    if (!userHolder.Text.Equals(txtUsername.Text))
                    {
                        byte[] imageBt = null;
                        FileStream fstream = new FileStream(this.txtxPhotoPath.Text, FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fstream);
                        imageBt = br.ReadBytes((int)fstream.Length);

                        closeConnection();
                        openConnection();
                        command = new MySqlCommand();
                        command.CommandText = "INSERT INTO tblaccounts(EmployeeID,AccounType,FirstName,MiddleName,LastName,Gender,Birthday,Age,ContactNumber,EmailAddress,UserName,Password,HomeAddress,RegistrationDate,RegistrationTime,PhotoPath,image) VALUES('" + txtEmpID.Text + "', '" +
                                          cbbType.Text + "', '" + txtFName.Text + "','" + txtMName.Text + "','" + txtLName.Text + "','" + cbbGender.Text + "','" + theBDateReg + "','" + txtAge.Text + "','" + textBox1.Text + "','" + txtEAdd.Text + "','" + txtUsername.Text + "','" + txtPassword.Text + "','" +
                                          txtHAdd.Text + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + txtxPhotoPath.Text + "',@IMG)";
                        command.Connection = connection;
                        command.Parameters.Add(new MySqlParameter("@IMG", imageBt));
                        command.ExecuteNonQuery();
                        MessageBox.Show("Details Saved!");
                        closeConnection();
                    }
                    else
                    {
                        MessageBox.Show("UserName already exists!");
                    }
                       
                }
                else
                {
                    openConnection();
                    command.CommandText = "SELECT username FROM tblaccounts WHERE username='" + txtUsername.Text + "'";
                    MySqlDataReader dReader = command.ExecuteReader();
                    if (dReader.Read())
                    {
                        userHolder.Text = dReader["username"].ToString();
                    }

                    if (!userHolder.Text.Equals(txtUsername.Text))
                    {
                       
                        closeConnection();
                        openConnection();
                        command = new MySqlCommand();
                        command.CommandText = "INSERT INTO tblaccounts(EmployeeID,AccounType,FirstName,MiddleName,LastName,Gender,Birthday,Age,ContactNumber,EmailAddress,UserName,Password,HomeAddress,RegistrationDate,RegistrationTime,PhotoPath,image) VALUES('" + txtEmpID.Text + "', '" +
                                          cbbType.Text + "', '" + txtFName.Text + "','" + txtMName.Text + "','" + txtLName.Text + "','" + cbbGender.Text + "','" + theBDateReg + "','" + txtAge.Text + "','" + textBox1.Text + "','" + txtEAdd.Text + "','" + txtUsername.Text + "','" + txtPassword.Text + "','" +
                                          txtHAdd.Text + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + valueNull + "','"+valueNull+"')";
                        command.Connection = connection;
                        command.ExecuteNonQuery();
                        MessageBox.Show("Details Saved!");
                        closeConnection();
                    }
                    else
                    {
                        MessageBox.Show("UserName already exists!");
                    }
                       
                }
               
               
            }
            else if(selectedItem.Equals("Admin")) 
            {
                if (!txtxPhotoPath.Text.Equals(""))
                {
                    openConnection();
                    command.CommandText = "SELECT username FROM tbladmin WHERE username='" + txtUsername.Text + "'";
                    MySqlDataReader dReader = command.ExecuteReader();
                    if (dReader.Read())
                    {
                        userHolder.Text = dReader["username"].ToString();
                    }

                    if (!userHolder.Text.Equals(txtUsername.Text))
                    {
                        byte[] imageBt = null;
                        FileStream fstream = new FileStream(this.txtxPhotoPath.Text, FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fstream);
                        imageBt = br.ReadBytes((int)fstream.Length);

                        //closeConnection();
                        openConnection();
                        command = new MySqlCommand();
                        command.CommandText = "INSERT INTO tbladmin(AccounType,FirstName,MiddleName,LastName,Gender,Birthday,Age,ContactNumber,EmailAddress,UserName,Password,PinCode,HomeAddress,RegistrationDate,RegistrationTime,PhotoPath,image) VALUES( '" +
                                               cbbType.Text + "', '" + txtFName.Text + "','" + txtMName.Text + "','" + txtLName.Text + "','" + cbbGender.Text + "','" + dateTimePicker1.Text + "','" + txtAge.Text + "','" + textBox1.Text + "','" + txtEAdd.Text + "','" + txtUsername.Text + "','" + txtPassword.Text + "','" + txtPCode.Text + "','" +
                                               txtHAdd.Text + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + txtxPhotoPath.Text + "',@IMG)";
                        command.Connection = connection;
                        command.Parameters.Add(new MySqlParameter("@IMG", imageBt));
                        command.ExecuteNonQuery();
                        MessageBox.Show("Details Saved!");
                        closeConnection();
                    }
                    else
                    {
                        MessageBox.Show("UserName already exists!");
                    }
                }
                else {

                    openConnection();
                    command.CommandText = "SELECT username FROM tbladmin WHERE username='" + txtUsername.Text + "'";
                    MySqlDataReader dReader = command.ExecuteReader();
                    if (dReader.Read())
                    {
                        userHolder.Text = dReader["username"].ToString();
                    }

                    if (!userHolder.Text.Equals(txtUsername.Text))
                    {
                       

                        //closeConnection();
                        openConnection();
                        command = new MySqlCommand();
                        command.CommandText = "INSERT INTO tbladmin(AccounType,FirstName,MiddleName,LastName,Gender,Birthday,Age,ContactNumber,EmailAddress,UserName,Password,PinCode,HomeAddress,RegistrationDate,RegistrationTime,PhotoPath,image) VALUES( '" +
                                               cbbType.Text + "', '" + txtFName.Text + "','" + txtMName.Text + "','" + txtLName.Text + "','" + cbbGender.Text + "','" + dateTimePicker1.Text + "','" + txtAge.Text + "','" + textBox1.Text + "','" + txtEAdd.Text + "','" + txtUsername.Text + "','" + txtPassword.Text + "','" + txtPCode.Text + "','" +
                                               txtHAdd.Text + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + valueNull + "','"+valueNull+"')";
                        command.Connection = connection;
                        command.ExecuteNonQuery();
                        MessageBox.Show("Details Saved!");
                        closeConnection();
                    }
                    else
                    {
                        MessageBox.Show("UserName already exists!");
                    }
                }
                
               

            }
           

            }

            catch (Exception ee) {
                MessageBox.Show(ee.Message);
            }

           
        }

     
        //removing image(edit profile module)
        private void btnRemovePic_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            txtxPhotoPath.Text = "";
        }

        //setting everything to nothing(edit profile module)
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtAge.Text = "";
            txtPCode.Text="";
            textBox1.Text="";
            txtEAdd.Text = "";
            txtEmpID.Text = "";
            txtFName.Text = "";
            cbbGender.SelectedIndex = -1;
            txtHAdd.Text = "";
            txtLName.Text = "";
            txtMName.Text = "";
            txtPassword.Text = "";
            //txtSAns.Text = "";
            txtUsername.Text = "";
            txtxPhotoPath.Text="";
            //cbbSQuestion.SelectedIndex = -1;
            cbbType.SelectedIndex = -1;
            pictureBox1.Image = null;

        }

        //VIEW TABLE ==> select table (admin form)
        private void cbbSelectTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                openConnection();
                string selectedItem = cbbSelectTable.Text;
                if (selectedItem.Equals("Employee Accounts"))
                {
                    cbbAdminAccounts.Hide();
                    cbbEmpAccounts.Show();
                    cbbProductList.Hide();

                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "select * from tblaccounts";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        dt = new DataTable();
                        dt.Load(reader);
                        gridViewtbl.DataSource = dt;
                        closeConnection();
                    }



                }

                if (selectedItem.Equals("Admin Accounts"))
                {
                    cbbProductList.Hide();
                    cbbEmpAccounts.Hide();
                    cbbAdminAccounts.Show();
                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "select * from tbladmin";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        dt = new DataTable();
                        dt.Load(reader);
                        gridViewtbl.DataSource = dt;
                        closeConnection();
                    }


                }
                if (selectedItem.Equals("Frappes"))
                {
                    cbbProductList.Show();
                    cbbEmpAccounts.Hide();
                    cbbAdminAccounts.Hide();
                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * from tblprodfrappes";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        dt = new DataTable();
                        dt.Load(reader);
                        gridViewtbl.DataSource = dt;
                        closeConnection();
                    }
                }
                if (selectedItem.Equals("Hot Drinks"))
                {
                    cbbProductList.Show();
                    cbbEmpAccounts.Hide();
                    cbbAdminAccounts.Hide();
                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * from tblprodhotbvrgs";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        dt = new DataTable();
                        dt.Load(reader);
                        gridViewtbl.DataSource = dt;
                        closeConnection();
                    }
                }
                if (selectedItem.Equals("Snacks"))
                {
                    cbbEmpAccounts.Hide();
                    cbbAdminAccounts.Hide();
                    cbbProductList.Show();
                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * from tblprodsnacks";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        dt = new DataTable();
                        dt.Load(reader);
                        gridViewtbl.DataSource = dt;
                        closeConnection();
                    }
                }
                if (selectedItem.Equals("ProductSales"))
                {
                    cbbEmpAccounts.Hide();
                    cbbAdminAccounts.Hide();
                   // cbbProductList.Show();
                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * from tblproductSales";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        dt = new DataTable();
                        dt.Load(reader);
                        gridViewtbl.DataSource = dt;
                        closeConnection();
                    }
                }
                if (selectedItem.Equals("Transactions"))
                {
                    cbbEmpAccounts.Hide();
                    cbbAdminAccounts.Hide();
                    //cbbProductList.Hide();
                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * from tbltransactionsrecords";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        dt = new DataTable();
                        dt.Load(reader);
                        gridViewtbl.DataSource = dt;
                        closeConnection();
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }

        //VIEW TABLE ==> search by (admin form)
        private void cbbSearchEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                openConnection();
                string selectedItem2 = cbbSearchEdit.Text;
                if (selectedItem2.Equals("Employee Accounts"))
                {
                   

                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "select *  from tblaccounts";
                    //  command.CommandText = "SELECT DATE_FORMAT(Birthday, '%Y/%m/%d') FROM tblaccounts";
                    MySqlDataReader reader2 = command.ExecuteReader();
                    if (reader2.HasRows)
                    {

                        dt = new DataTable();
                        dt.Load(reader2);
                        dataGridView2.DataSource = dt;
                        closeConnection();
                    }



                }

                else if (selectedItem2.Equals("Admin Accounts"))
                {

                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "select * from tbladmin";
                    MySqlDataReader reader2 = command.ExecuteReader();
                    if (reader2.HasRows)
                    {
                   

                        dt = new DataTable();
                        dt.Load(reader2);
                        dataGridView2.DataSource = dt;
                        closeConnection();
                    }


                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        //displaying data from gridview to textboxes[EDiT PROFiLE]
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                //openConnection();
                //connection = new MySqlConnection();
                //command.Connection = connection;
                //command.CommandText = "select * from tblaccounts";
                //MySqlDataReader mreader = command.ExecuteReader();
                //while(){
                //}
              
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
                txtEditEmpID.Text = row.Cells[0].Value.ToString();//employee or admin id
                txtEditActype.Text = row.Cells[1].Value.ToString();//account type
                txtEditFname.Text = row.Cells[2].Value.ToString();//firstname
                txtEditMName.Text = row.Cells[3].Value.ToString();//middlename
                txtEditLName.Text = row.Cells[4].Value.ToString();//lastname
                txtEditGender.Text = row.Cells[5].Value.ToString();//gender
               // txtEditBDay.Text = row.Cells[6].Value.ToString();//birthday
                txtEditAge.Text = row.Cells[7].Value.ToString();//age
                txtEditCpNum.Text = row.Cells[8].Value.ToString();//phone number
                txtEditEAdd.Text = row.Cells[9].Value.ToString();//email address
                txtEditUser.Text = row.Cells[10].Value.ToString();//username
                txtEditPass.Text = row.Cells[11].Value.ToString();//password
                txtEditHAdd.Text = row.Cells[13].Value.ToString();//homeaddress
                txtEditPic.Text = row.Cells[18].Value.ToString();//picture

                 dateTimePicker2.Text = row.Cells[6].Value.ToString();//birthday

                if (dataGridView2.CurrentRow.Cells[19].Value != DBNull.Value)
                {
                    byte[] img = (byte[])dataGridView2.CurrentRow.Cells[19].Value;
                    MemoryStream ms = new MemoryStream(img);
                    pictureBoxEdit.Image = Image.FromStream(ms);
                }

                else
                {
                    pictureBoxEdit.Image = null;
                }
            }

           
        }

        //updating picture in edit profile 
        private void btnEditPic_Click(object sender, EventArgs e)
        {
             OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|All Files(*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK) {
                string picLoc = dlg.FileName.ToString();
                txtEditPic.Text = picLoc;
                pictureBoxEdit.ImageLocation = picLoc;
            }
        }


        private void txtVTblSearch_Click(object sender, EventArgs e)
        {
            search.Text = "";
        }

        //autocomplete text in search box
        void AutoCompleteText() {

            search.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            search.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();

            openConnection();
           
                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "select * from tblaccounts";
                    MySqlDataReader reader;
                    int i = 0;
                    try
                    {
                       
                        reader = command.ExecuteReader();

                        while(reader.Read()){


                            string fname = reader.GetString("FirstName");
                            coll.Add(fname);
                            i++;

                        }
                    }   
                    catch(Exception ex) { 
                        MessageBox.Show(ex.Message);
                    }

                    search.AutoCompleteCustomSource = coll;
                    connection.Close();

        }

        //auto suggesstions 
        private void search_TextChanged(object sender, EventArgs e)
        {
            string selectedItem2 = cbbEmpAccounts.Text;
            string selectedItemAdminAccount = cbbAdminAccounts.Text;
            string selectedItemProducts= cbbProductList.Text;

            if(selectedItem2.Equals("FirstName")){
                dv = new DataView(dt);
                dv.RowFilter = string.Format("FirstName LIKE '%{0}%'", search.Text);
                gridViewtbl.DataSource = dv;
            }
            if (selectedItem2.Equals("UserName"))
            {
                dv = new DataView(dt);
                dv.RowFilter = string.Format("UserName LIKE '%{0}%'", search.Text);
                gridViewtbl.DataSource = dv;
            }
            if (selectedItem2.Equals("EmployeeID"))
            {
                dv = new DataView(dt);
                dv.RowFilter = string.Format("convert(EmployeeID,'System.String') LIKE '%{0}%'", search.Text);
                gridViewtbl.DataSource = dv;
            }
            if (selectedItem2.Equals("MiddleName"))
            {
                dv = new DataView(dt);
                dv.RowFilter = string.Format("MiddleName LIKE '%{0}%'", search.Text);
                gridViewtbl.DataSource = dv;
            }
            if (selectedItem2.Equals("LastName"))
            {
                dv = new DataView(dt);
                dv.RowFilter = string.Format("LastName LIKE '%{0}%'", search.Text);
                gridViewtbl.DataSource = dv;
            }
            if (selectedItemAdminAccount.Equals("UserName"))
            {
                dv = new DataView(dt);
                dv.RowFilter = string.Format("UserName LIKE '%{0}%'", search.Text);
                gridViewtbl.DataSource = dv;
            }
            if (selectedItemAdminAccount.Equals("AdminId"))
            {
                dv = new DataView(dt);
                dv.RowFilter = string.Format("convert(AdminId,'System.String') LIKE '%{0}%'", search.Text);
                gridViewtbl.DataSource = dv;
            }
            if (selectedItemAdminAccount.Equals("MiddleName"))
            {
                dv = new DataView(dt);
                dv.RowFilter = string.Format("MiddleName LIKE '%{0}%'", search.Text);
                gridViewtbl.DataSource = dv;
            }
            if (selectedItemAdminAccount.Equals("LastName"))
            {
                dv = new DataView(dt);
                dv.RowFilter = string.Format("LastName LIKE '%{0}%'", search.Text);
                gridViewtbl.DataSource = dv;
            }
            if (selectedItemProducts.Equals("ProductID"))
            {
                dv = new DataView(dt);
                dv.RowFilter = string.Format("convert(ProductID,'System.String') LIKE '%{0}%'", search.Text);
                gridViewtbl.DataSource = dv;
            }
            if (selectedItemProducts.Equals("ProductName"))
            {
                dv = new DataView(dt);
                dv.RowFilter = string.Format("ProductName LIKE '%{0}%'", search.Text);
                gridViewtbl.DataSource = dv;
            }
             
        }

        //view tables via combobox
        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
           // string selectedItem = cbbSelectTable.Text;
            //string select = cbbEmpAccounts.Text;
            //openConnection();
            //if (select.Equals("FirstName"))
            //{

            //    command = new MySqlCommand();
            //    command.Connection = connection;
            //    command.CommandText = "select firstname from tblaccounts";
            //    MySqlDataReader reader = command.ExecuteReader();
            //    if (reader.HasRows)
            //    {
            //        dt = new DataTable();
            //        dt.Load(reader);
            //        gridViewtbl.DataSource = dt;
            //        closeConnection();
            //    }
            //}
            //else if (select.Equals("EmployeeID"))
            //{

            //    command = new MySqlCommand();
            //    command.Connection = connection;
            //    command.CommandText = "select employeeId from tblaccounts";
            //    MySqlDataReader reader = command.ExecuteReader();
            //    if (reader.HasRows)
            //    {
            //        dt = new DataTable();
            //        dt.Load(reader);
            //        gridViewtbl.DataSource = dt;
            //        closeConnection();
            //    }
            //}
        }

      

        //setting pincode availability according to user type registration 
        private void cbbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem2 = cbbType.Text;
            int employeeIdDefValue = 0;
            int adminIdDefValue=0;
            if (selectedItem2.Equals("Admin"))
            {
                txtPCode.Enabled = true;
                txtEmpID.Enabled = false;

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * FROM tbladmin WHERE adminid=(SELECT MAX(adminid) FROM tbladmin)";
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    adminIdDefValue = reader.GetInt32("adminId");
                    adminIdDefValue = adminIdDefValue + 1;
                }



                txtEmpID.Text = adminIdDefValue.ToString();


            }
            else
            {
                txtPCode.Enabled = false;
                txtEmpID.Enabled = false;

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * FROM tblaccounts WHERE employeeid=(SELECT MAX(employeeid) FROM tblaccounts)";
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    employeeIdDefValue = reader.GetInt32("employeeid");
                   // userHolder.Text = reader["username"].ToString();

                    employeeIdDefValue = employeeIdDefValue + 1;
                }



                txtEmpID.Text = employeeIdDefValue.ToString();


                //openConnection();
                //command.CommandText = "select username from tblaccounts";
                //MySqlDataReader readerEmp = command.ExecuteReader();

                //while (readerEmp.Read())
                //{

                //    userHolder.Text = readerEmp["username"].ToString();

                //}
                //closeConnection();
            }
            closeConnection();
        }

        //datetimepicker
        private void dateTimePicker1_SizeChanged(object sender, EventArgs e)
        {
            TimeSpan age = DateTime.Now - dateTimePicker1.Value;
            int years = DateTime.Now.Year - dateTimePicker1.Value.Year;
            if (dateTimePicker1.Value.AddYears(years) > DateTime.Now) years--;
            txtAge.Text = years.ToString();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            TimeSpan age = DateTime.Now - dateTimePicker1.Value;
            int years = DateTime.Now.Year - dateTimePicker1.Value.Year;
            if (dateTimePicker1.Value.AddYears(years) > DateTime.Now) years--;
            txtAge.Text = years.ToString();
        }

        //warning message for special characters for user input
        private void txtEmpID_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
                MessageBox.Show("Please input integer only");
            }
        }

        //array labels for blinking username
        private void timerBlink_Tick(object sender, EventArgs e)
        {
           
            Label[] lbls;
            lbls = new Label[ 6]{ lbl1, lbl2, lbl3,lbl4,lbl6,lbl7};
            
            lbl1.Visible = false;
            lbl2.Visible = false;
            lbl3.Visible = false;
            lbl4.Visible = false;
            lbl6.Visible = false;
            lbl7.Visible = false;

            if (blink < lbls.Length && up == true)
            {
                lbls[blink].Visible = true;
                blink++;
            }
            else
            {
                blink--;
                up = false;

                if (blink != -1)
                {
                    lbls[blink].Visible = true;

                }
                else
                {
                    up = true;
                    blink = 0;
                   
                    lblEwan.Visible = true;
                }
            }
        }

       

        //calling orderform
        private void button16_Click(object sender, EventArgs e)
        {
            //string logOrder = "";
            //string id = "", pass = "", empID = "";

            //openConnection();
            //command = new MySqlCommand();
            //command.CommandText = "SELECT * FROM tbladmin ";
            //command.Connection = connection;
            //MySqlDataReader reader2 = command.ExecuteReader();

            //while (reader2.Read())
            //{
            //  //  empID = reader2["adminID"].ToString();
            //   // id = reader2["username"].ToString();
            //   // pass = reader2["password"].ToString();
            //    logOrder = reader2["username"].ToString();
            //    //name = reader2["firstname"].ToString() + " " + reader2["lastname"].ToString();
            //    currentAccountType = reader2["accountype"].ToString();

            //}
            string log = "";

            openConnection();
            command = new MySqlCommand();
            command.CommandText = "INSERT INTO tblaudit(accountType,username,date,time,action) values('" + lblrestrictor.Text + "','" + lbl3 + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','button for order form was clicked');";
            command.Connection = connection;
            command.ExecuteNonQuery();
            closeConnection();
           
            orderForm order = new orderForm(Name);
            order.loggedUserID = lbl1.Text;
            //order.loggedUserName = name;
            order.loggedUserAccountType = lblrestrictor.Text;
            //m.loggedUserEMployeeId = empID;
            

            order.Show();
            this.Hide();
        }

        //displaying current time 
        private void TimeDate_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            TimeDate.Start();
        }

        //button for changing pin code
        private void button23_Click(object sender, EventArgs e)
        {



            openConnection();
            command.CommandText = "SELECT pincode FROM tbladmin WHERE pincode='" + txtBoxChangePinOld.Text + "';";
         
            MySqlDataReader productsSqlDataReader = command.ExecuteReader();

            int productIDColPos = productsSqlDataReader.GetOrdinal("pincode");


            if (productsSqlDataReader.Read())
            {
                connection.Close();
                openConnection();
                command.CommandText = "UPDATE tbladmin SET pincode = '" + txtBoxChangePinNew.Text + "' WHERE pincode = '" + txtBoxChangePinOld.Text + "';";
                command.Connection = connection;
                command.ExecuteNonQuery();
                MessageBox.Show("FUCK! Pincode Updated!");

                closeConnection();
            }

            else
            {
                MessageBox.Show("invalid pincode");
            }
        }
        //change password
        private void button26_Click(object sender, EventArgs e)
        {

            openConnection();
            command.CommandText = "SELECT password FROM tbladmin WHERE password='" + textBox31.Text + "';";

            MySqlDataReader productsSqlDataReader = command.ExecuteReader();

            int productIDColPos = productsSqlDataReader.GetOrdinal("password");


            if (productsSqlDataReader.Read())
            {
                connection.Close();
                openConnection();
                command.CommandText = "UPDATE tbladmin SET password = '" + textBox30.Text + "' WHERE password = '" + textBox31.Text + "';";
                command.Connection = connection;
                command.ExecuteNonQuery();
                MessageBox.Show("FUCK! Password Updated!");

                closeConnection();
            }

            else
            {
                MessageBox.Show("invalid pincode");
            }

        }

        //displayiong the current number of stocks 
        void populateProducts() {

            MySqlDataReader productsSqlDataReader;
            openConnection();
            command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * from tblprodfrappes where productid=10001;";
            productsSqlDataReader = command.ExecuteReader();

           

           if(productsSqlDataReader.Read())
            {
                textBox2.Text = productsSqlDataReader["pricep"].ToString();
                textBox13.Text = productsSqlDataReader["stocksp"].ToString();
                textBox19.Text = productsSqlDataReader["priceg"].ToString();
                textBox25.Text = productsSqlDataReader["stocksg"].ToString();
       
         
                closeConnection();
            }

           openConnection();
           command = new MySqlCommand();
           command.Connection = connection;
           command.CommandText = "SELECT * from tblprodfrappes where productid=10002;";
           productsSqlDataReader = command.ExecuteReader();


           if (productsSqlDataReader.Read())
           {
               textBox3.Text = productsSqlDataReader["pricep"].ToString();
               textBox12.Text = productsSqlDataReader["stocksp"].ToString();
               textBox18.Text = productsSqlDataReader["priceg"].ToString();
               textBox24.Text = productsSqlDataReader["stocksg"].ToString();
           
               closeConnection();
            }


           openConnection();
           command = new MySqlCommand();
           command.Connection = connection;
           command.CommandText = "SELECT * from tblprodfrappes where productid=10003;";
           productsSqlDataReader = command.ExecuteReader();


           if (productsSqlDataReader.Read())
           {
              
                textBox4.Text = productsSqlDataReader["pricep"].ToString();
                textBox11.Text = productsSqlDataReader["stocksp"].ToString();
                textBox17.Text = productsSqlDataReader["priceg"].ToString();
                textBox23.Text = productsSqlDataReader["stocksg"].ToString();
               closeConnection();
           }

         

           openConnection();
           command = new MySqlCommand();
           command.Connection = connection;
           command.CommandText = "SELECT * from tblprodhotbvrgs where productid=10013;";
           productsSqlDataReader = command.ExecuteReader();

           if (productsSqlDataReader.Read())
           {

               textBox90.Text = productsSqlDataReader["price"].ToString();
               textBox84.Text = productsSqlDataReader["stocks"].ToString();
               closeConnection();
           }
           openConnection();
           command = new MySqlCommand();
           command.Connection = connection;
           command.CommandText = "SELECT * from tblprodhotbvrgs where productid=10014;";
           productsSqlDataReader = command.ExecuteReader();

           if (productsSqlDataReader.Read())
           {

               textBox89.Text = productsSqlDataReader["price"].ToString();
               textBox83.Text = productsSqlDataReader["stocks"].ToString();
               closeConnection();
           }

           openConnection();
           command = new MySqlCommand();
           command.Connection = connection;
           command.CommandText = "SELECT * from tblprodhotbvrgs where productid=10015;";
           productsSqlDataReader = command.ExecuteReader();

           if (productsSqlDataReader.Read())
           {

               textBox88.Text = productsSqlDataReader["price"].ToString();
               textBox82.Text = productsSqlDataReader["stocks"].ToString();
               closeConnection();
           }

        

           openConnection();
           command = new MySqlCommand();
           command.Connection = connection;
           command.CommandText = "SELECT * from tblprodsnacks where productid=10019;";
           productsSqlDataReader = command.ExecuteReader();

           if (productsSqlDataReader.Read())
           {

               textBox78.Text = productsSqlDataReader["price"].ToString();
               textBox72.Text = productsSqlDataReader["stocks"].ToString();
               closeConnection();
           }

           openConnection();
           command = new MySqlCommand();
           command.Connection = connection;
           command.CommandText = "SELECT * from tblprodsnacks where productid=10020;";
           productsSqlDataReader = command.ExecuteReader();

           if (productsSqlDataReader.Read())
           {

               textBox77.Text = productsSqlDataReader["price"].ToString();
               textBox71.Text = productsSqlDataReader["stocks"].ToString();
               closeConnection();
           }

           openConnection();
           command = new MySqlCommand();
           command.Connection = connection;
           command.CommandText = "SELECT * from tblprodsnacks where productid=10023;";
           productsSqlDataReader = command.ExecuteReader();

           if (productsSqlDataReader.Read())
           {

               textBox74.Text = productsSqlDataReader["price"].ToString();
               textBox68.Text = productsSqlDataReader["stocks"].ToString();
               closeConnection();
           }

          

            closeConnection();
        }

        //default stocks
        private void button28_Click(object sender, EventArgs e)
        {
            openConnection();
            command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "select stocksP from tblprodfrappes";
            command.CommandText = "select stocks from tblprodhotbvrgs";
            command.CommandText = "select stocks from tblprodsnacks";
            MySqlDataReader dataReader = command.ExecuteReader();

            if (dataReader.Read())
            {
                connection.Close();
                openConnection();
                command.CommandText = "UPDATE tblprodfrappes set stocksp = '25'";
                command.Connection = connection;
                command.ExecuteNonQuery();
                command.CommandText = "UPDATE tblprodfrappes set stocksg = '25'";
                command.Connection = connection;
                command.ExecuteNonQuery();
                command.CommandText = "UPDATE tblprodhotbvrgs set stocks = '25'";
                command.Connection = connection;
                command.ExecuteNonQuery();
                command.CommandText = "UPDATE tblprodsnacks set stocks = '25'";
                command.Connection = connection;
                command.ExecuteNonQuery();
                
            }
            closeConnection();
            this.populateProducts();
            MessageBox.Show("Updated Stocks");
        }

        //editing stock and prices 
        private void button18_Click(object sender, EventArgs e)
        {
            //price petite
            textBox4.Enabled = true; //frap
            textBox2.Enabled = true; //mocha
            textBox3.Enabled = true; //dchoco
            //petite stocks
            textBox11.Enabled = true; //frap
            textBox12.Enabled = true; //dchoco
            textBox13.Enabled = true; //mocha
            //price grande
            textBox17.Enabled = true; //frap
            textBox18.Enabled = true; //choco
            textBox19.Enabled = true; //mocha
            //stocks granda
            textBox23.Enabled = true; //frap
            textBox24.Enabled = true; //choco
            textBox25.Enabled = true; //mocha
            //price snacks
            textBox74.Enabled = true; //carbo
            textBox77.Enabled = true; //hnc
            textBox78.Enabled = true; //tacos
            //stocks snacks
            textBox68.Enabled = true; //carbo
            textBox71.Enabled = true; //hnc
            textBox72.Enabled = true; //tacos 
            //price hot beverages
            textBox88.Enabled = true; //fancywhite
            textBox89.Enabled = true; //caffe latte
            textBox90.Enabled = true; //flavored syrup
            //stocks hot beverages
            textBox82.Enabled = true; //fancywhite
            textBox83.Enabled = true; //caffe latte
            textBox84.Enabled = true; //flavored syrup

           
        }

        //saving inventory
        private void button27_Click(object sender, EventArgs e)
        {
            openConnection();
            command = new MySqlCommand();
            command.Connection = connection;

            if ((!textBox2.Text.Equals("")) && (!textBox13.Text.Equals("")) && (!textBox19.Text.Equals("")) && (!textBox25.Text.Equals("")))
            {
                command.CommandText = "update tblprodfrappes set pricep = '" + textBox2.Text + "', stocksp = '" + textBox13.Text + "', priceg = '" + textBox19.Text + "',stocksg = '" + textBox25.Text + "' where productid = 10001";
                command.ExecuteNonQuery();
            }
            if ((!textBox3.Text.Equals("")) && (!textBox12.Text.Equals("")) && (!textBox18.Text.Equals("")) && (!textBox24.Text.Equals("")))
            {
                command.CommandText = "update tblprodfrappes set pricep = '" + textBox3.Text + "', stocksp = '" + textBox12.Text + "', priceg = '" + textBox18.Text + "',stocksg = '" + textBox24.Text + "' where productid = 10002";
                command.ExecuteNonQuery();
            }
                 else
                    {
                        MessageBox.Show("input values for products");
                    }
            closeConnection();
            this.populateProducts();
        }


        //closing/hiding the module adminform/logout
        private void button19_Click(object sender, EventArgs e)
        {
          
              
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Dialog Title", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    
                    try
                    {//code for logging out logged user
                        openConnection();
                        command = new MySqlCommand();
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO tblaudit(accountType,username,date,time,action) values('" + loggedUserAccountType + "','" + loggedUserID + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','log-out')";
                        command.ExecuteNonQuery();
                        closeConnection();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Failed to record the action in audit trail", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    Form1 f1 = new Form1();
                    f1.Show();
                    this.Hide();
                }
                else
                {
                   this.Activate();
                }


           
        }

        private void adminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to go to log in form?", "Dialog Title", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    
                    try
                    {//code for logging out logged user
                        openConnection();
                        command = new MySqlCommand();
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO tblaudit(accountType,username,date,time,action) values('"+loggedUserAccountType+"','" + loggedUserID + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','log-out')";
                        command.ExecuteNonQuery();
                        closeConnection();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Failed to record the action in audit trail", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    Form1 f1 = new Form1();
                    f1.Show();
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

        private void timerWelcome_Tick(object sender, EventArgs e)
        {
            if (lblForAdmin.Visible == true)
            {
                lblForAdmin.Visible = false;
            }
            else
            {
                lblForAdmin.Visible = true;
            }
        }

        //editing profile button
        private void button20_Click(object sender, EventArgs e)
        {
          if(txtEditEmpID.Text.Equals(loggedUserEMployeeId)){

              txtEditFname.Enabled = true;
              txtEditGender.Enabled = true;
              txtEditMName.Enabled = true;
              txtEditLName.Enabled = true;
              txtEditGender.Enabled = true;
            //  txtEditBDay.Enabled = true;
              txtEditAge.Enabled = true;
              txtEditHAdd.Enabled = true;
              txtEditUser.Enabled = true;
              txtEditPass.Enabled = true;
              txtEditCpNum.Enabled = true;
              txtEditEAdd.Enabled = true;
              btnEditPic.Enabled = true;
              button21.Enabled = true;
              dateTimePicker2.Enabled = true;

          }else{

              MessageBox.Show("Access is denied!");
          }
        }

        //saving edited profile
        private void button21_Click(object sender, EventArgs e)
        {
            string current = txtEditPic.Text;

            try
            {
               
                   
                    if (!txtEditFname.Text.Trim().Equals("") && !txtEditMName.Text.Trim().Equals("") && !txtEditLName.Text.Trim().Equals("")
                     && !txtEditGender.Text.Trim().Equals("")&& !txtEditHAdd.Text.Trim().Equals("") && !txtEditUser.Text.Trim().Equals("") && !txtEditPass.Text.Trim().Equals("")
                     && !txtEditCpNum.Text.Trim().Equals("") && !txtEditEAdd.Text.Trim().Equals(""))
                    {
                        if (lblrestrictor.Text.Equals("Employee"))
                        {

                        openConnection();
                        command = new MySqlCommand();
                        command.Connection = connection;
                        command.CommandText = "select * from tblaccounts";
                        MySqlDataReader rd = command.ExecuteReader();

                        while(rd.Read()){
                        
                               current = rd["photopath"].ToString();
                        }

                        closeConnection();


                        if (txtEditPic.Text.Equals(current))
                        {
                           
                                openConnection();
                                command = new MySqlCommand();
                                command.Connection = connection;
                                command.CommandText = "update tblaccounts set FirstName = '" + txtEditFname.Text + "', MiddleName = '" + txtEditMName.Text + "', LastName = '" + txtEditLName.Text
                                + "',Gender = '" + txtEditGender.Text + "',Birthday = '" + dateTimePicker2.Text + "', Age = '" + txtEditAge.Text + "', ContactNumber = '" + txtEditCpNum.Text
                                + "', EmailAddress='" + txtEditEAdd.Text + "',username = '" + txtEditUser.Text + "',password = '" + txtEditPass.Text + "',HomeAddress = '" + txtEditHAdd.Text + "' where employeeid = '" + txtEditEmpID.Text + "'";
                                command.Connection = connection;
                                command.ExecuteNonQuery();
                                MessageBox.Show("SAVED!");
                         
                            
                        }
                        else
                        {

                            byte[] imageBtEdit = null;
                            FileStream fstreamEdit = new FileStream(this.txtEditPic.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstreamEdit);
                            imageBtEdit = br.ReadBytes((int)fstreamEdit.Length);
                            openConnection();
                            command = new MySqlCommand();
                            command.Connection = connection;
                            command.CommandText = "update tblaccounts set FirstName = '" + txtEditFname.Text + "', MiddleName = '" + txtEditMName.Text + "', LastName = '" + txtEditLName.Text
                                + "',Gender = '" + txtEditGender.Text + "',Birthday = '" + dateTimePicker2.Text + "', Age = '" + txtEditAge.Text + "', ContactNumber = '" + txtEditCpNum.Text
                                + "', EmailAddress='" + txtEditEAdd.Text + "',username = '" + txtEditUser.Text + "',password = '" + txtEditPass.Text + "',HomeAddress = '" + txtEditHAdd.Text + "',image = @IMG where employeeid = '" + txtEditEmpID.Text + "'";
                            command.Connection = connection;
                            command.Parameters.Add(new MySqlParameter("@IMG", imageBtEdit));
                            command.ExecuteNonQuery();
                            MessageBox.Show("UPDATED!");
                        }
                       

                    }
                    else
                    {
                        openConnection();
                        command = new MySqlCommand();
                        command.Connection = connection;
                        command.CommandText = "select * from tbladmin";
                        MySqlDataReader rd = command.ExecuteReader();

                        while (rd.Read())
                        {

                            current = rd["photopath"].ToString();
                        }

                        closeConnection();


                        if (txtEditPic.Text.Equals(current))
                        {

                            openConnection();
                            command = new MySqlCommand();
                            command.Connection = connection;
                            command.CommandText = "update tbladmin set FirstName = '" + txtEditFname.Text + "', MiddleName = '" + txtEditMName.Text + "', LastName = '" + txtEditLName.Text
                                + "',Gender = '" + txtEditGender.Text + "',Birthday = '" + dateTimePicker2.Text + "', Age = '" + txtEditAge.Text + "', ContactNumber = '" + txtEditCpNum.Text
                                + "', EmailAddress='" + txtEditEAdd.Text + "',username = '" + txtEditUser.Text + "',password = '" + txtEditPass.Text + "',HomeAddress = '" + txtEditHAdd.Text + "' where adminid= '" + txtEditEmpID.Text + "'";
                            command.Connection = connection;
                            command.ExecuteNonQuery();
                            MessageBox.Show("SAVED!");
                        }
                        else
                        {

                            byte[] imageBtEdit = null;
                            FileStream fstreamEdit = new FileStream(this.txtEditPic.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstreamEdit);
                            imageBtEdit = br.ReadBytes((int)fstreamEdit.Length);
                            openConnection();
                            command = new MySqlCommand();
                            command.Connection = connection;
                            command.CommandText = "update tbladmin set FirstName = '" + txtEditFname.Text + "', MiddleName = '" + txtEditMName.Text + "', LastName = '" + txtEditLName.Text
                                + "',Gender = '" + txtEditGender.Text + "',Birthday = '" + dateTimePicker2.Text + "', Age = '" + txtEditAge.Text + "', ContactNumber = '" + txtEditCpNum.Text
                                + "', EmailAddress='" + txtEditEAdd.Text + "',username = '" + txtEditUser.Text + "',password = '" + txtEditPass.Text + "',HomeAddress = '" + txtEditHAdd.Text + "',image = @IMG where adminid = '" + txtEditEmpID.Text + "'";
                            command.Connection = connection;
                            command.Parameters.Add(new MySqlParameter("@IMG", imageBtEdit));
                            command.ExecuteNonQuery();
                            MessageBox.Show("UPDATED!");
                        }
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
            closeConnection();
        }

       

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            //txtEditBDay.Text = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            TimeSpan age = DateTime.Now - dateTimePicker2.Value;
            int years = DateTime.Now.Year - dateTimePicker2.Value.Year;
            if (dateTimePicker2.Value.AddYears(years) > DateTime.Now) years--;
            txtEditAge.Text = years.ToString();
         
        }


        bool checkForSpecialChar(int checkingcase, string text)
        {

            bool result = true;
            var regex = new Regex("^[a-zA-Z0-9-]*$");

            switch (checkingcase)
            {
                case 1://for ID
                    regex = new Regex("^[a-zA-Z0-9-_ .]*$");
                    break;
                case 2://numbers only
                    regex = new Regex("^[0-9]*$");
                    break;
                case 3://letters only (names)
                    regex = new Regex("^[a-zA-Z -]*$");
                    break;
                case 4://passwords
                    regex = new Regex("^[a-zA-Z0-9-_ @!?.]*$");
                    break;
            }

            if (!regex.IsMatch(text))
            {
                result = false;
            }
            return result;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            grpBoxForReports.Visible = true;
            gbInventory.Visible = false;
            gbCpass.Visible = false;
            gbEditProf.Visible = false;
            gbVtbl.Visible = false;
            gbRegistration.Visible = false;
            gBdashboard.Visible = false;
           
             
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ForReports rep = new ForReports();
            rep.Show();
        }

    
        

    }
}
