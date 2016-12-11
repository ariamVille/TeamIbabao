    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using MySql.Data.MySqlClient;
    using System.Collections;
    namespace mainForm
    {
        public partial class orderForm : Form
        {

            private bool dragging = false;
            private Point offset;
            private Point startPoint = new Point(0, 0);

            MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;Initial Catalog='poscan';username=root;password=");
            MySqlCommand command;
            int blink = 0; bool up = true;
            int seconds = 6;
            int order = 0;
            int order2;
            int FrappePorder;
            int priceP, priceG, stocksP, stocksG, totalStocks, totalPrice, Stocks;
            string convertedPrice, convertedOrder,product;
            ListViewItem item;
            public string loggedUserID = "";
            public string loggedUserAccountType = "";
            bool exists = false;

            double SellingPrice = 0;
            double amtOfVat;
            double netOfVaT;
            double amtOfDiscount;
            double discountedAmt;
            double change;
            double cash;
            double totalAmt;
            double currentCharge;
            double totalQty;
            double salesFlavoredSyrup;
            double salesAmazingMochaP;
            double salesAmazingMochaG;
            double salesDoubleChocoP;
            double salesDoubleChocoG;
            double salesFrapuccinoP, salesFrapuccinoG;
            double salesCaffeLatte, salesFancyWhite;
            double salesTacos, salesHamAndVeggies, salesCarbonara;

            int stockspFrappe, stocksgFrappe, stockspDchoco, stocksgDchoco, stockspFrapuccino, stocksgFrapuccino, StocksFlavoredSyrup,
                StocksCaffelatte, StocksfancyWhite, StocksTacos, StocksHamAndVeggies, StocksCarbonara;
            DataTable dt;

            int Tno = 0;

            string timeIn;
            public orderForm(string username)
            {

               


                InitializeComponent();
                testConnection();
                stocks();
                lblLog.Text = username;

                timer1Blink2.Start();
                groupBoxFrappes.Show();
                groupBoxHotBvrgs.Visible = false;
                groupBoxSnacks.Visible = false;
                panelFrappe.Visible = false;
                panelHotBeverages.Visible = false;
                panelCategory.Visible = false;
                panelForSnacks.Visible = false;
                lblTimeOrderForm.Text = DateTime.Now.ToLongTimeString();
                lblDateOrderForm.Text = DateTime.Now.ToLongDateString();
                openConnection();
                command = new MySqlCommand();
                command.CommandText = "SELECT * FROM tblaudit";
                command.Connection = connection;
                MySqlDataReader reader2 = command.ExecuteReader();

                while (reader2.Read())
                {
                   
                    timeIn = reader2["time"].ToString();

                }

                lblLogInTime.Text = timeIn;

            }
       
            //testing connection
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
            //opening connection
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
            //closing connection
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
            //connection timer
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

            //displaying number of stocks
            void stocks()
            {

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from tblprodfrappes where productid=10001";
                MySqlDataReader dataReader;

                dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    txtStockPFrappe.Text = dataReader.GetInt32("stocksP").ToString();
                    txtStockGFrappe.Text = dataReader.GetInt32("stocksG").ToString();
                    closeConnection();
                }

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from tblprodfrappes where productid=10002";

                dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    txtPstocksDoubleChoco.Text = dataReader.GetInt32("stocksP").ToString();
                    txtGstocksDoubleChoco.Text = dataReader.GetInt32("stocksG").ToString();
                    closeConnection();
                }

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from tblprodfrappes where productid=10003";

                dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    txtPstocksFrapuccino.Text = dataReader.GetInt32("stocksP").ToString();
                    txtGstocksFrapuccino.Text = dataReader.GetInt32("stocksG").ToString();
                    closeConnection();
                }

           
         

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from tblprodhotbvrgs where productid=10013";

                dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    txtStocksFlavoredSyrup.Text = dataReader.GetInt32("stocks").ToString();
                    closeConnection();
                }

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from tblprodhotbvrgs where productid=10014";

                dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    txtStocksCaffeeLatte.Text = dataReader.GetInt32("stocks").ToString();
                    closeConnection();
                }

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from tblprodhotbvrgs where productid=10015";

                dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    txtStocksfancyWhite.Text = dataReader.GetInt32("stocks").ToString();
                    closeConnection();
                }


          

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from tblprodsnacks where productid=10019";

                dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    txtStocksTacos.Text = dataReader.GetInt32("stocks").ToString();
                    closeConnection();
                }

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from tblprodsnacks where productid=10020";

                dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    txtStocksHamAndVeggies.Text = dataReader.GetInt32("stocks").ToString();
                    closeConnection();
                }

           

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from tblprodsnacks where productid=10023";

                dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    txtStocksCarbonara.Text = dataReader.GetInt32("stocks").ToString();
                    closeConnection();
                }

            }

            //buttons for group boxes
            private void btnFrappes_Click(object sender, EventArgs e)
            {
                panelHotBeverages.Visible = false;
                panelFrappe.Visible = true;
                panelCategory.Visible = false;
                panelForSnacks.Visible = false;
                timerBlinkFrappe.Start();
                groupBoxFrappes.Visible = true;
                groupBoxHotBvrgs.Visible = false;
                groupBoxSnacks.Visible = false;
            }

      

            private void btnHotBvrgs_Click(object sender, EventArgs e)
            {
                panelHotBeverages.Visible = true;
                panelCategory.Visible = false;
                panelFrappe.Visible = false;
                panelForSnacks.Visible = false;
                timerBlinkHotBeverages.Start();
                groupBoxHotBvrgs.Visible = true;
                groupBoxFrappes.Visible = false;
                groupBoxSnacks.Visible = false;
            }

            private void btnSnacks_Click(object sender, EventArgs e)
            {
                panelForSnacks.Visible = true;
                panelHotBeverages.Visible = false;
                panelCategory.Visible = false;
                panelFrappe.Visible = false;
                timerBlinkSnacks.Start();
                groupBoxSnacks.Visible = true;
                groupBoxHotBvrgs.Visible = false;
                groupBoxFrappes.Visible = false;
            }

       
            private void timer1Blink2_Tick(object sender, EventArgs e)
            {
                Label[] lbls;
                lbls = new Label[5] { lblsc1, lblsc2, lblsc3, lblsc4, lblsc5};
                lblsc1.Visible = false;
                lblsc2.Visible = false;
                lblsc3.Visible = false;
                lblsc4.Visible = false;
                lblsc5.Visible = false;
           

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
                        lblsc6.Visible = true;
                    }
                }
            }





            //frappe amazing mocha+
            private void button11_Click(object sender, EventArgs e)
            {
        
                string select = cbbFrappe.Text;
                bool found = false;
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;

                MySqlDataReader dataReader;


                if (select.Equals("Amazing Mocha (P)"))
                {
                    command.CommandText = "select * from tblprodfrappes where productid=10001";
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("PriceP").ToString();
                        stockspFrappe = Int32.Parse(txtStockPFrappe.Text);
                        int totalp = stockspFrappe - 1;

                        if (totalp < 0)
                        {
                            MessageBox.Show("out of stocks");
                        }
                        else
                        {

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {

                                    if (Convert.ToString(row.Cells[0].Value) == cbbFrappe.Text && Convert.ToString(row.Cells[1].Value) == label71.Text)
                                    {
                                        row.Cells[2].Value = Convert.ToString(1 + Convert.ToInt16(row.Cells[2].Value));
                                        stockspFrappe = Int32.Parse(txtStockPFrappe.Text);
                                        txtStockPFrappe.Text = Convert.ToString(stockspFrappe - 1);
                                        found = true;
                                        label74.Text = Convert.ToString(Convert.ToInt16(row.Cells[3].Value));
                                        row.Cells[dataGridView1.Columns["Amount"].Index].Value = (Convert.ToDouble(row.Cells[dataGridView1.Columns["price"].Index].Value) * Convert.ToDouble(row.Cells[dataGridView1.Columns["qty"].Index].Value));

                                
                                
                                    }

                                }
                                if (!found)
                                {
                               
                                
                                    stockspFrappe = Int32.Parse(txtStockPFrappe.Text);
                                    txtStockPFrappe.Text = Convert.ToString(stockspFrappe - 1);
                                    dataGridView1.Rows.Add(cbbFrappe.Text, label71.Text, 1,label71.Text);
                                }
                            } 
                            else
                            {
                            
                                
                                stockspFrappe = Int32.Parse(txtStockPFrappe.Text);
                                txtStockPFrappe.Text = Convert.ToString(stockspFrappe - 1);
                                dataGridView1.Rows.Add(cbbFrappe.Text, label71.Text, 1, label71.Text);

                            }
                       }

                    }
                }
                else if (select.Equals("Amazing Mocha (G)"))
                {

                    command.CommandText = "select priceg from tblprodfrappes where productid=10001";
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("Priceg").ToString();
                        stocksgFrappe = Int32.Parse(txtStockGFrappe.Text);
                        int totalg = stocksgFrappe - 1;

                        if (totalg < 0)
                        {
                            MessageBox.Show("out of stocks");
                        }
                        else
                        {
                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {

                                    if (Convert.ToString(row.Cells[0].Value) == cbbFrappe.Text && Convert.ToString(row.Cells[1].Value) == label71.Text)
                                    {
                                        row.Cells[2].Value = Convert.ToString(1 + Convert.ToInt16(row.Cells[2].Value));
                                        stocksgFrappe = Int32.Parse(txtStockGFrappe.Text);
                                        txtStockGFrappe.Text = Convert.ToString(stocksgFrappe - 1);
                                        found = true;
                                        label74.Text = Convert.ToString(Convert.ToInt16(row.Cells[3].Value));
                                        row.Cells[dataGridView1.Columns["Amount"].Index].Value = (Convert.ToDouble(row.Cells[dataGridView1.Columns["price"].Index].Value) * Convert.ToDouble(row.Cells[dataGridView1.Columns["qty"].Index].Value));

                               
                                
                                    }

                                }
                                if (!found)
                                {
                               
                                    stocksgFrappe = Int32.Parse(txtStockGFrappe.Text);
                                    txtStockGFrappe.Text = Convert.ToString(stocksgFrappe - 1);
                                    dataGridView1.Rows.Add(cbbFrappe.Text, label71.Text, 1, label71.Text);
                                }
                            }
                            else
                            {
                           
                                stocksgFrappe = Int32.Parse(txtStockGFrappe.Text);
                                txtStockGFrappe.Text = Convert.ToString(stocksgFrappe - 1);
                                dataGridView1.Rows.Add(cbbFrappe.Text, label71.Text, 1, label71.Text);
                            }

                        }
                    }
                }

                closeConnection();
            }

            //amazing mocha-
            private void button10_Click(object sender, EventArgs e)
            {
                string select = cbbFrappe.Text;
                bool found = false;
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;

                MySqlDataReader dataReader;


                if (select.Equals("Amazing Mocha (P)")){

               command.CommandText = "select * from tblprodfrappes where productid=10001";


                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("PriceP").ToString();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (Convert.ToString(row.Cells[0].Value) == cbbFrappe.Text && Convert.ToString(row.Cells[1].Value) == label71.Text)
                            {
                                if (Convert.ToInt16(row.Cells[2].Value) == 0)
                                {
                                    MessageBox.Show("no remaining orders");
                                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                                    dataGridView1.Rows.RemoveAt(rowIndex);
                                }
                                else
                                {
                                    row.Cells[2].Value = Convert.ToString(-1 + Convert.ToInt16(row.Cells[2].Value));
                                    label74.Text = Convert.ToString(-Convert.ToInt16(label71.Text) + Convert.ToInt16(row.Cells[3].Value));
                                    row.Cells[3].Value = Convert.ToString(Convert.ToInt16(label74.Text));
                                
                                    stockspFrappe = Int32.Parse(txtStockPFrappe.Text);

                                    int sum = stockspFrappe + 1;

                                    txtStockPFrappe.Text = sum.ToString();

                                }
                            
                            }

                            }
                        }

                }
                else if (select.Equals("Amazing Mocha (G)"))
                {

                    command.CommandText = "select priceg from tblprodfrappes where productid=10001";


                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("Priceg").ToString();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (Convert.ToString(row.Cells[0].Value) == cbbFrappe.Text && Convert.ToString(row.Cells[1].Value) == label71.Text)
                            {
                                if (Convert.ToInt16(row.Cells[2].Value) == 0)
                                {
                                    MessageBox.Show("no remaining orders");
                                }
                                else
                                {
                                    row.Cells[2].Value = Convert.ToString(-1 + Convert.ToInt16(row.Cells[2].Value));
                                    label74.Text = Convert.ToString(-Convert.ToInt16(label71.Text) + Convert.ToInt16(row.Cells[3].Value));
                                    row.Cells[3].Value = Convert.ToString(Convert.ToInt16(label74.Text));

                                    stocksgFrappe = Int32.Parse(txtStockGFrappe.Text);

                                    int sum = stocksgFrappe + 1;

                                    txtStockGFrappe.Text = sum.ToString();

                                }

                            }

                        }
                   
                    }
                }
                closeConnection();
                }


       

            //reseting textboxes in orders
            private void button51_Click(object sender, EventArgs e)
            {
                SellingPrice = 0;
                amtOfVat = 0;
                discountedAmt = 0;
                totalQty = 0;

                textBox14.Text = SellingPrice.ToString("F2");
                textBox18.Text = amtOfVat.ToString("F2");
                textBox15.Text = discountedAmt.ToString("F2");
                label70.Text = totalQty.ToString();

                dataGridView1.Rows.Clear();

                //openConnection();
                //command = new MySqlCommand();
                //command.Connection = connection;
                //command.CommandText = "select * FROM tbltransactionsrecords WHERE TransactionNumber=(SELECT MAX(TransactionNumber) FROM tbltransactionsrecords)";
                //MySqlDataReader reader = command.ExecuteReader();
                //if (reader.Read())
                //{
                //    Tno = reader.GetInt32("TransactionNumber");
                //    Tno = Tno + 1;
                //}

                //closeConnection();

                //label78.Text = Tno.ToString();


            }

       


      

            //double choco+
            private void button17_Click(object sender, EventArgs e)
            {
                string select = cbbDoubleChoco.Text;
                bool found = false;
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;

                MySqlDataReader dataReader;


                if (select.Equals("Double Choco (P)"))
                {
                    command.CommandText = "select * from tblprodfrappes where productid=10002";
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("PriceP").ToString();
                        stockspDchoco = Int32.Parse(txtPstocksDoubleChoco.Text);
                        int totalp = stockspDchoco - 1;

                        if (totalp < 0)
                        {
                            MessageBox.Show("out of stocks");
                        }
                        else
                        {

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {

                                    if (Convert.ToString(row.Cells[0].Value) == cbbDoubleChoco.Text && Convert.ToString(row.Cells[1].Value) == label71.Text)
                                    {
                                        row.Cells[2].Value = Convert.ToString(1 + Convert.ToInt16(row.Cells[2].Value));
                                        stockspDchoco = Int32.Parse(txtPstocksDoubleChoco.Text);
                                        txtPstocksDoubleChoco.Text = Convert.ToString(stockspDchoco - 1);
                                        found = true;
                                        label74.Text = Convert.ToString(Convert.ToInt16(row.Cells[3].Value));
                                        row.Cells[dataGridView1.Columns["Amount"].Index].Value = (Convert.ToDouble(row.Cells[dataGridView1.Columns["price"].Index].Value) * Convert.ToDouble(row.Cells[dataGridView1.Columns["qty"].Index].Value));

                                    }

                                }
                                if (!found)
                                {
                              
                                    stockspDchoco = Int32.Parse(txtPstocksDoubleChoco.Text);
                                    txtPstocksDoubleChoco.Text = Convert.ToString(stockspDchoco - 1);
                                    dataGridView1.Rows.Add(cbbDoubleChoco.Text, label71.Text, 1, label71.Text);
                                }
                            }
                            else
                            {
                           
                                stockspDchoco = Int32.Parse(txtPstocksDoubleChoco.Text);
                                txtPstocksDoubleChoco.Text = Convert.ToString(stockspDchoco - 1);
                                dataGridView1.Rows.Add(cbbDoubleChoco.Text, label71.Text, 1, label71.Text);
                            }
                        }

                    }
                }
                else if (select.Equals("Double Choco (G)"))
                {

                    command.CommandText = "select priceg from tblprodfrappes where productid=10002";
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("Priceg").ToString();
                        stocksgDchoco = Int32.Parse(txtGstocksDoubleChoco.Text);
                        int totalg = stocksgDchoco - 1;

                        if (totalg < 0)
                        {
                            MessageBox.Show("out of stocks");
                        }
                        else
                        {
                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {

                                    if (Convert.ToString(row.Cells[0].Value) == cbbDoubleChoco.Text && Convert.ToString(row.Cells[1].Value) == label71.Text)
                                    {
                                        row.Cells[2].Value = Convert.ToString(1 + Convert.ToInt16(row.Cells[2].Value));
                                        stocksgDchoco = Int32.Parse(txtGstocksDoubleChoco.Text);
                                        txtGstocksDoubleChoco.Text = Convert.ToString(stocksgDchoco - 1);
                                        found = true;
                                        label74.Text = Convert.ToString(Convert.ToInt16(row.Cells[3].Value));
                                        row.Cells[dataGridView1.Columns["Amount"].Index].Value = (Convert.ToDouble(row.Cells[dataGridView1.Columns["price"].Index].Value) * Convert.ToDouble(row.Cells[dataGridView1.Columns["qty"].Index].Value));

                                   
                                
                                    }

                                }
                                if (!found)
                                {
                              
                                    stocksgDchoco = Int32.Parse(txtGstocksDoubleChoco.Text);
                                    txtGstocksDoubleChoco.Text = Convert.ToString(stocksgDchoco - 1);
                                    dataGridView1.Rows.Add(cbbDoubleChoco.Text, label71.Text, 1, label71.Text);
                                }
                            }
                            else
                            {
                            
                                stocksgDchoco = Int32.Parse(txtGstocksDoubleChoco.Text);
                                txtGstocksDoubleChoco.Text = Convert.ToString(stocksgDchoco - 1);
                                dataGridView1.Rows.Add(cbbDoubleChoco.Text, label71.Text, 1, label71.Text);
                            }

                        }
                    }
                }

                closeConnection();
            }

            //double choco -
            private void button16_Click(object sender, EventArgs e)
            {
                string select = cbbDoubleChoco.Text;
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;

                MySqlDataReader dataReader;


                if (select.Equals("Double Choco (P)"))
                {

                    command.CommandText = "select * from tblprodfrappes where productid=10002";


                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("PriceP").ToString();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (Convert.ToString(row.Cells[0].Value) == cbbDoubleChoco.Text && Convert.ToString(row.Cells[1].Value) == label71.Text)
                            {
                                if (Convert.ToInt16(row.Cells[2].Value) == 0)
                                {
                                    MessageBox.Show("no remaining orders");
                                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                                    dataGridView1.Rows.RemoveAt(rowIndex);
                                }
                                else
                                {
                                    row.Cells[2].Value = Convert.ToString(-1 + Convert.ToInt16(row.Cells[2].Value));
                                    label74.Text = Convert.ToString(-Convert.ToInt16(label71.Text) + Convert.ToInt16(row.Cells[3].Value));
                                    row.Cells[3].Value = Convert.ToString(Convert.ToInt16(label74.Text));
                                    stockspDchoco = Int32.Parse(txtPstocksDoubleChoco.Text);
                                    int sum = stockspDchoco + 1;
                                    txtPstocksDoubleChoco.Text = sum.ToString();

                                }

                            }

                        }
                    }

                }
                else if (select.Equals("Double Choco (G)"))
                {

                    command.CommandText = "select priceg from tblprodfrappes where productid=10002";


                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("Priceg").ToString();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (Convert.ToString(row.Cells[0].Value) == cbbDoubleChoco.Text && Convert.ToString(row.Cells[1].Value) == label71.Text)
                            {
                                if (Convert.ToInt16(row.Cells[2].Value) == 0)
                                {
                                    MessageBox.Show("no remaining orders");
                                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                                    dataGridView1.Rows.RemoveAt(rowIndex);
                                }
                                else
                                {
                                    row.Cells[2].Value = Convert.ToString(-1 + Convert.ToInt16(row.Cells[2].Value));
                                    label74.Text = Convert.ToString(-Convert.ToInt16(label71.Text) + Convert.ToInt16(row.Cells[3].Value));
                                    row.Cells[3].Value = Convert.ToString(Convert.ToInt16(label74.Text));
                                    stocksgDchoco = Int32.Parse(txtGstocksDoubleChoco.Text);
                                    int sum = stocksgDchoco + 1;
                                    txtGstocksDoubleChoco.Text = sum.ToString();

                                }

                            }

                        }
               
                    }
                }
                closeConnection();
            }

        

            //frapuccino +
            private void button24_Click(object sender, EventArgs e)
            {
                string select = cbbFrapuccino.Text;
                bool found = false;
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;

                MySqlDataReader dataReader;


                if (select.Equals("Frapuccino (P)"))
                {
                    command.CommandText = "select * from tblprodfrappes where productid=10003";
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("PriceP").ToString();
                        stockspFrapuccino = Int32.Parse(txtPstocksFrapuccino.Text);
                        int totalp = stockspFrapuccino - 1;

                        if (totalp < 0)
                        {
                            MessageBox.Show("out of stocks");
                        }
                        else
                        {

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {

                                    if (Convert.ToString(row.Cells[0].Value) == cbbFrapuccino.Text && Convert.ToString(row.Cells[1].Value) == label71.Text)
                                    {
                                        row.Cells[2].Value = Convert.ToString(1 + Convert.ToInt16(row.Cells[2].Value));
                                        stockspFrapuccino = Int32.Parse(txtPstocksFrapuccino.Text);
                                        txtPstocksFrapuccino.Text = Convert.ToString(stockspFrapuccino - 1);
                                        found = true;
                                        label74.Text = Convert.ToString(Convert.ToInt16(row.Cells[3].Value));
                                        row.Cells[dataGridView1.Columns["Amount"].Index].Value = (Convert.ToDouble(row.Cells[dataGridView1.Columns["price"].Index].Value) * Convert.ToDouble(row.Cells[dataGridView1.Columns["qty"].Index].Value));
                                
                                    }

                                }
                                if (!found)
                                {
                               
                                    stockspFrapuccino = Int32.Parse(txtPstocksFrapuccino.Text);
                                    txtPstocksFrapuccino.Text = Convert.ToString(stockspFrapuccino - 1);
                                    dataGridView1.Rows.Add(cbbFrapuccino.Text, label71.Text, 1, label71.Text);
                                }
                            }
                            else
                            {
                           
                                stockspFrapuccino = Int32.Parse(txtPstocksFrapuccino.Text);
                                txtPstocksFrapuccino.Text = Convert.ToString(stockspFrapuccino - 1);
                                dataGridView1.Rows.Add(cbbFrapuccino.Text, label71.Text, 1, label71.Text);
                            }
                        }

                    }
                }
                else if (select.Equals("Frapuccino (G)"))
                {

                    command.CommandText = "select priceg from tblprodfrappes where productid=10002";
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("Priceg").ToString();
                        stocksgFrapuccino = Int32.Parse(txtGstocksFrapuccino.Text);
                        int totalg = stocksgFrapuccino - 1;

                        if (totalg < 0)
                        {
                            MessageBox.Show("out of stocks");
                        }
                        else
                        {
                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {

                                    if (Convert.ToString(row.Cells[0].Value) == cbbFrapuccino.Text && Convert.ToString(row.Cells[1].Value) == label71.Text)
                                    {
                                        row.Cells[2].Value = Convert.ToString(1 + Convert.ToInt16(row.Cells[2].Value));
                                        stocksgFrapuccino = Int32.Parse(txtGstocksFrapuccino.Text);
                                        txtGstocksFrapuccino.Text = Convert.ToString(stocksgFrapuccino - 1);
                                        found = true;
                                        label74.Text = Convert.ToString(Convert.ToInt16(row.Cells[3].Value));
                                        row.Cells[dataGridView1.Columns["Amount"].Index].Value = (Convert.ToDouble(row.Cells[dataGridView1.Columns["price"].Index].Value) * Convert.ToDouble(row.Cells[dataGridView1.Columns["qty"].Index].Value));

                                    }

                                }
                                if (!found)
                                {
                               
                                    stocksgFrapuccino = Int32.Parse(txtGstocksFrapuccino.Text);
                                    txtGstocksFrapuccino.Text = Convert.ToString(stocksgFrapuccino - 1);
                                    dataGridView1.Rows.Add(cbbFrapuccino.Text, label71.Text, 1, label71.Text);
                                }
                            }
                            else
                            {
                          
                                stocksgFrapuccino = Int32.Parse(txtGstocksFrapuccino.Text);
                                txtGstocksFrapuccino.Text = Convert.ToString(stocksgFrapuccino - 1);
                                dataGridView1.Rows.Add(cbbFrapuccino.Text, label71.Text, 1, label71.Text);
                            }

                        }
                    }
                }

                closeConnection();
            }

            //frapuccino-
            private void button23_Click(object sender, EventArgs e)
            {
                string select = cbbFrapuccino.Text;
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;

                MySqlDataReader dataReader;


                if (select.Equals("Frapuccino (P)"))
                {

                    command.CommandText = "select * from tblprodfrappes where productid=10003";


                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("PriceP").ToString();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (Convert.ToString(row.Cells[0].Value) == cbbFrapuccino.Text && Convert.ToString(row.Cells[1].Value) == label71.Text)
                            {
                                if (Convert.ToInt16(row.Cells[2].Value) == 0)
                                {
                                    MessageBox.Show("no remaining orders");
                                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                                    dataGridView1.Rows.RemoveAt(rowIndex);
                                }
                                else
                                {
                                    row.Cells[2].Value = Convert.ToString(-1 + Convert.ToInt16(row.Cells[2].Value));
                                    label74.Text = Convert.ToString(-Convert.ToInt16(label71.Text) + Convert.ToInt16(row.Cells[3].Value));
                                    row.Cells[3].Value = Convert.ToString(Convert.ToInt16(label74.Text));
                                    stockspFrapuccino= Int32.Parse(txtPstocksFrapuccino.Text);
                                    int sum = stockspFrapuccino+ 1;
                                    txtPstocksFrapuccino.Text = sum.ToString();

                                }

                            }

                        }
                    }

                }
                else if (select.Equals("Frapuccino (G)"))
                {

                    command.CommandText = "select priceg from tblprodfrappes where productid=10003";


                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("Priceg").ToString();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (Convert.ToString(row.Cells[0].Value) == cbbFrapuccino.Text && Convert.ToString(row.Cells[1].Value) == label71.Text)
                            {
                                if (Convert.ToInt16(row.Cells[2].Value) == 0)
                                {
                                    MessageBox.Show("no remaining orders");
                                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                                    dataGridView1.Rows.RemoveAt(rowIndex);
                                }
                                else
                                {
                                    row.Cells[2].Value = Convert.ToString(-1 + Convert.ToInt16(row.Cells[2].Value));
                                    label74.Text = Convert.ToString(-Convert.ToInt16(label71.Text) + Convert.ToInt16(row.Cells[3].Value));
                                    row.Cells[3].Value = Convert.ToString(Convert.ToInt16(label74.Text));
                                    stocksgFrapuccino= Int32.Parse(txtGstocksFrapuccino.Text);
                                    int sum = stocksgFrapuccino + 1;
                                    txtGstocksFrapuccino.Text = sum.ToString();

                                }

                            }

                        }

                    }
                }
                closeConnection();
            }

            //updating stocks in the database
            private void button7_Click(object sender, EventArgs e)
            {
                cash = Convert.ToDouble(textBox16.Text);

                if (cash.Equals(""))
                {
                    MessageBox.Show("Please input cash value!");
                }
                else if (cash < discountedAmt)
                {

                    MessageBox.Show("insufficient cash");

                }
                else
                {
                    cash = Convert.ToDouble(textBox16.Text);
                    change = cash - discountedAmt;
                    textBox17.Text = change.ToString("F2");
                    openConnection();
                    command.CommandText = "select stocksP from tblprodfrappes";
                    MySqlDataReader productsSqlDataReader = command.ExecuteReader();

                    if (productsSqlDataReader.Read())
                    {

                        connection.Close();
                        openConnection();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {


                            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                            {
                                String header = dataGridView1.Columns[i].HeaderText;
                                String cellText = Convert.ToString(row.Cells[i].Value);

                                if (cellText == "Amazing Mocha (P)")
                                {
                                    int qtyMochaP = 0;
                                    command.CommandText = "UPDATE tblprodfrappes set stocksp = '" + txtStockPFrappe.Text + "' where productId = 10001";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    salesAmazingMochaP += Convert.ToDouble(row.Cells[3].Value);
                                    qtyMochaP += Convert.ToInt32(row.Cells[2].Value);
                                    command.CommandText = "INSERT INTO tblproductsales(TransactionNo,ProductId,ProductName,QTY,TotalSales,Date) VALUES('"+label78.Text+"',10001,'Amazing Mocha (P)','"+qtyMochaP+"', '" + salesAmazingMochaP + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "' )";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();

                                }
                                if (cellText == "Amazing Mocha (G)")
                                {
                                    int qtyMocha = 0;
                                    command.CommandText = "UPDATE tblprodfrappes set stocksg = '" + txtStockGFrappe.Text + "' where productId = 10001";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    salesAmazingMochaG += Convert.ToDouble(row.Cells[3].Value);
                                    qtyMocha += Convert.ToInt32(row.Cells[2].Value);
                                    command.CommandText = "INSERT INTO tblproductsales(TransactionNo,ProductId,ProductName,QTY,TotalSales,Date) VALUES('" + label78.Text + "',10001,'Amazing Mocha (G)','"+qtyMocha+"', '" + salesAmazingMochaG + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "' )";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();

                                }
                                if (cellText == "Double Choco (P)")
                                {
                                    int qtyDchocoG = 0;
                                    command.CommandText = "UPDATE tblprodfrappes set stocksp = '" + txtPstocksDoubleChoco.Text + "' where productId = 10002";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    salesDoubleChocoP += Convert.ToDouble(row.Cells[3].Value);
                                    qtyDchocoG += Convert.ToInt32(row.Cells[2].Value);
                                    command.CommandText = "INSERT INTO tblproductsales(TransactionNo,ProductId,ProductName,QTY,TotalSales,Date) VALUES('" + label78.Text + "',10002,'Double Choco (P)','"+qtyDchocoG+"', '" + salesDoubleChocoP + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "' )";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();

                                }
                                if (cellText == "Double Choco (G)")
                                {
                                    int qtyDchoco = 0;
                                    command.CommandText = "UPDATE tblprodfrappes set stocksg = '" + txtGstocksDoubleChoco.Text + "' where productId = 10002";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    salesDoubleChocoG += Convert.ToDouble(row.Cells[3].Value);
                                    qtyDchoco += Convert.ToInt32(row.Cells[2].Value);
                                    command.CommandText = "INSERT INTO tblproductsales(TransactionNo,ProductId,ProductName,QTY,TotalSales,Date) VALUES('" + label78.Text + "',10002,'Double Choco (G)','"+qtyDchoco+"', '" + salesDoubleChocoG + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "' )";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();

                                }
                                if (cellText == "Frapuccino (P)")
                                {
                                    int qtyFrapuccinoP = 0;
                                    command.CommandText = "UPDATE tblprodfrappes set stocksp = '" + txtPstocksFrapuccino.Text + "' where productId = 10003";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    salesFrapuccinoP += Convert.ToDouble(row.Cells[3].Value);
                                    qtyFrapuccinoP += Convert.ToInt32(row.Cells[2].Value);
                                    command.CommandText = "INSERT INTO tblproductsales(TransactionNo,ProductId,ProductName,QTY,TotalSales,Date) VALUES('" + label78.Text + "',10003,'Frapuccino (P)','"+qtyFrapuccinoP+"', '" + salesFrapuccinoP + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "' )";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();

                                }
                                if (cellText == "Frapuccino (G)")
                                {
                                    int qtyFrapuccino = 0;
                                    command.CommandText = "UPDATE tblprodfrappes set stocksg = '" + txtGstocksFrapuccino.Text + "' where productId = 10003";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    salesFrapuccinoG += Convert.ToDouble(row.Cells[3].Value);
                                    qtyFrapuccino += Convert.ToInt32(row.Cells[2].Value);
                                    command.CommandText = "INSERT INTO tblproductsales(TransactionNo,ProductId,ProductName,QTY,TotalSales,Date) VALUES('" + label78.Text + "',10003,'Frapuccino (G)','"+qtyFrapuccino+"', '" + salesFrapuccinoG + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "' )";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();

                                }
                                if (cellText == "Flavored Syrup")
                                {
                                    int qtyFlavoredSyrup = 0;
                                    command.CommandText = "UPDATE tblprodhotbvrgs set stocks = '" + txtStocksFlavoredSyrup.Text + "' where productId = 10013";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    salesFlavoredSyrup += Convert.ToDouble(row.Cells[3].Value);
                                    qtyFlavoredSyrup += Convert.ToInt32(row.Cells[2].Value);
                                    command.CommandText = "INSERT INTO tblproductsales(TransactionNo,ProductId,ProductName,QTY,TotalSales,Date) VALUES('" + label78.Text + "',10013,'Flavored Syrup','"+qtyFlavoredSyrup+"', '" + salesFlavoredSyrup + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "' )";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();

                                }
                                if (cellText == "Caffe Latte")
                                {
                                    int qtyCaffeLatte = 0;
                                    command.CommandText = "UPDATE tblprodhotbvrgs set stocks = '" + txtStocksCaffeeLatte.Text + "' where productId = 10014";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    salesCaffeLatte += Convert.ToDouble(row.Cells[3].Value);
                                    qtyCaffeLatte += Convert.ToInt32(row.Cells[2].Value);
                                    command.CommandText = "INSERT INTO tblproductsales(TransactionNo,ProductId,ProductName,QTY,TotalSales,Date) VALUES('" + label78.Text + "',10014,'Caffe Latte','"+qtyCaffeLatte+"', '" + salesCaffeLatte + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "' )";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();

                                }
                                if (cellText == "Fancy White")
                                {
                                    int qtyFancyWhite = 0;
                                    command.CommandText = "UPDATE tblprodhotbvrgs set stocks = '" + txtStocksfancyWhite.Text + "' where productId = 10015";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    salesFancyWhite += Convert.ToDouble(row.Cells[3].Value);
                                    qtyFancyWhite += Convert.ToInt32(row.Cells[2].Value);
                                    command.CommandText = "INSERT INTO tblproductsales(TransactionNo,ProductId,ProductName,QTY,TotalSales,Date) VALUES('" + label78.Text + "',10015,'Fancy White','"+qtyFancyWhite+"', '" + salesFancyWhite + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "' )";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();

                                }
                                if (cellText == "Tacos")
                                {
                                    int qtyTacos = 0;
                                    command.CommandText = "UPDATE tblprodsnacks set stocks = '" + txtStocksTacos.Text + "' where productId = 10019";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    salesTacos += Convert.ToDouble(row.Cells[3].Value);
                                    qtyTacos += Convert.ToInt32(row.Cells[2].Value);
                                    command.CommandText = "INSERT INTO tblproductsales(TransactionNo,ProductId,ProductName,QTY,TotalSales,Date) VALUES('" + label78.Text + "',10019,'Tacos','"+qtyTacos+"', '" + salesTacos + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "' )";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();

                                }
                                if (cellText == "Ham And Veggies")
                                {
                                    int qtyHam = 0;
                                    command.CommandText = "UPDATE tblprodsnacks set stocks = '" + txtStocksHamAndVeggies.Text + "' where productId = 10020";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    salesHamAndVeggies += Convert.ToDouble(row.Cells[3].Value);
                                    qtyHam += Convert.ToInt32(row.Cells[2].Value);
                                    command.CommandText = "INSERT INTO tblproductsales(TransactionNo,ProductId,ProductName,QTY,TotalSales,Date) VALUES('" + label78.Text + "',10020,'Ham And Veggies','"+qtyHam+"', '" + salesHamAndVeggies + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "' )";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();

                                }
                                if (cellText == "Carbonara")
                                {
                                    int qtyCarbo=0;
                                    command.CommandText = "UPDATE tblprodsnacks set stocks = '" + txtStocksCarbonara.Text + "' where productId = 10023";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();
                                    salesCarbonara += Convert.ToDouble(row.Cells[3].Value);
                                    qtyCarbo += Convert.ToInt32(row.Cells[2].Value);
                                    command.CommandText = "INSERT INTO tblproductsales(TransactionNo,ProductId,ProductName,QTY,TotalSales,Date) VALUES('" + label78.Text + "',10023,'Carbonara','"+qtyCarbo+"', '" + salesCarbonara + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "' )";
                                    command.Connection = connection;
                                    command.ExecuteNonQuery();

                                }

                            }

                        }
                  
                    }
                    MessageBox.Show("Stocks updated!");
                    closeConnection();

                }




            }
            //computation of orders
            private void button48_Click(object sender, EventArgs e)
            {
                button49.Enabled = false;
                button51.Enabled = false;
                button7.Enabled = true;
                button52.Enabled = true;
                button53.Enabled = true;
                button48.Enabled = true;
                if (!textBox2.Text.Equals("0"))
                {

                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        SellingPrice += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);

                    }

                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        totalQty += Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);

                    }

                    label70.Text = totalQty.ToString();

                    double forDiscount = Convert.ToDouble(textBox2.Text);
                    double converted = forDiscount / 100;
                    amtOfVat = SellingPrice * 0.12;
                    currentCharge = SellingPrice + amtOfVat;
                    amtOfDiscount = SellingPrice  * converted;
                    discountedAmt = currentCharge - amtOfDiscount;

                    textBox14.Text = SellingPrice.ToString("F2");
                    textBox18.Text = amtOfVat.ToString("F2");
                    textBox15.Text = discountedAmt.ToString("F2");
                    textBox3.Text = amtOfDiscount.ToString("F2");
                }
                else
                {

                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        SellingPrice += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);

                    }

                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        totalQty += Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);

                    }

                    label70.Text = totalQty.ToString();

                    amtOfVat = SellingPrice * 0.12;
                    currentCharge = SellingPrice + amtOfVat;
                    amtOfDiscount = currentCharge * 0;
                    discountedAmt = currentCharge - amtOfDiscount;


                    textBox14.Text = SellingPrice.ToString("F2");
                    textBox18.Text = amtOfVat.ToString("F2");
                    textBox15.Text = discountedAmt.ToString("F2");
                    textBox3.Text = amtOfDiscount.ToString("F2");

                }


            }

            //inserting transactions in database
            private void button53_Click(object sender, EventArgs e)
            {
                button49.Enabled = false;
                button51.Enabled = false;
                button7.Enabled = false;
                button52.Enabled = false;
                button53.Enabled = false;

              


                openConnection();
                command = new MySqlCommand();
                if(!textBox1.Text.Equals(""))
                {
                command.CommandText = "INSERT INTO tbltransactionsrecords(TransactionNumber,EmployeeName,ClientName,SubTotal,Discount,VatAmount,TotalAmount,Qty,CashAmount,ChangeAmount,Date,Time) VALUES('"+label78.Text+"','" +
                                                lblLog.Text + "', '" + textBox1.Text + "','" + textBox14.Text + "','" + textBox3.Text + "','" + textBox18.Text + "','" + textBox15.Text + "','" + label70.Text + "','" + textBox16.Text + "','" + textBox17.Text + "','" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
         
                command.Connection = connection;
                command.ExecuteNonQuery();
                MessageBox.Show("Printing Transaction Receipt......");

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * FROM tbltransactionsrecords WHERE TransactionNumber=(SELECT MAX(TransactionNumber) FROM tbltransactionsrecords)";
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Tno = reader.GetInt32("TransactionNumber");
                    Tno = Tno + 1;
                }

                closeConnection();

                label78.Text = Tno.ToString();

                closeConnection();
                textBox1.Clear();
                textBox14.Clear();
                textBox15.Clear();
                textBox16.Clear();
                textBox17.Clear();
                textBox18.Clear();
                textBox2.Text = Convert.ToInt32(0).ToString();
                dataGridView1.Rows.Clear();
                }else{
                    MessageBox.Show("Please Input customer's name");
                }



            }

            //button for removing/cancelling orders in gridview
            private void button49_Click(object sender, EventArgs e)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                dataGridView1.Rows.RemoveAt(rowIndex);
            }

        private void button5_Click_1(object sender, EventArgs e)
        {
            identityCheck iD = new identityCheck();
            iD.Show();
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


        //flavored syrup +
        private void button130_Click(object sender, EventArgs e)
            {
                bool found = false;
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;

                MySqlDataReader dataReader;


                    command.CommandText = "select * from tblprodhotbvrgs where productid=10013";
                    dataReader = command.ExecuteReader();
                     if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("Price").ToString();
                        StocksFlavoredSyrup = Int32.Parse(txtStocksFlavoredSyrup.Text);
                        int totalp = StocksFlavoredSyrup - 1;

                        if (totalp < 0)
                        {
                            MessageBox.Show("out of stocks");
                        }
                        else
                        {

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {

                                    if (Convert.ToString(row.Cells[0].Value) == "Flavored Syrup" && Convert.ToString(row.Cells[1].Value) == label71.Text)
                                    {
                                        row.Cells[2].Value = Convert.ToString(1 + Convert.ToInt16(row.Cells[2].Value));
                                        StocksFlavoredSyrup = Int32.Parse(txtStocksFlavoredSyrup.Text);
                                        txtStocksFlavoredSyrup.Text = Convert.ToString(StocksFlavoredSyrup - 1);
                                        found = true;
                                        label74.Text = Convert.ToString(Convert.ToInt16(row.Cells[3].Value));
                                        row.Cells[dataGridView1.Columns["Amount"].Index].Value = (Convert.ToDouble(row.Cells[dataGridView1.Columns["price"].Index].Value) * Convert.ToDouble(row.Cells[dataGridView1.Columns["qty"].Index].Value));
                                    }

                                }
                                if (!found)
                                {
                                    StocksFlavoredSyrup = Int32.Parse(txtStocksFlavoredSyrup.Text);
                                    txtStocksFlavoredSyrup.Text = Convert.ToString(StocksFlavoredSyrup - 1);
                                    dataGridView1.Rows.Add("Flavored Syrup", label71.Text, 1, label71.Text);
                                }
                            }
                            else
                            {
                                StocksFlavoredSyrup = Int32.Parse(txtStocksFlavoredSyrup.Text);
                                txtStocksFlavoredSyrup.Text = Convert.ToString(StocksFlavoredSyrup - 1);
                                dataGridView1.Rows.Add("Flavored Syrup", label71.Text, 1, label71.Text);
                            }
                        }

                }
                    closeConnection();
            }

            private void orderForm_Load(object sender, EventArgs e)
            {
               // MessageBox.Show("Input Customer's Name");

                button49.Enabled = false;
                button51.Enabled = false;
                button7.Enabled = false;
                button52.Enabled = false;
                button53.Enabled = false;
                //button48.Enabled = false;

                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * FROM tbltransactionsrecords WHERE TransactionNumber=(SELECT MAX(TransactionNumber) FROM tbltransactionsrecords)";
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Tno = reader.GetInt32("TransactionNumber");
                    Tno = Tno + 1;
                }
                


               label78.Text = Tno.ToString();

            }

       
            //flavored syrup-
            private void button129_Click(object sender, EventArgs e)
            {
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                MySqlDataReader dataReader;
                command.CommandText = "select * from tblprodhotbvrgs where productid=10013";

                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        label71.Text = dataReader.GetInt32("Price").ToString();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (Convert.ToString(row.Cells[0].Value) == "Flavored Syrup" && Convert.ToString(row.Cells[1].Value) == label71.Text)
                            {
                                if (Convert.ToInt16(row.Cells[2].Value) == 0)
                                {
                                    MessageBox.Show("no remaining orders");
                                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                                    dataGridView1.Rows.RemoveAt(rowIndex);
                                }
                                else
                                {
                                    row.Cells[2].Value = Convert.ToString(-1 + Convert.ToInt16(row.Cells[2].Value));
                                    label74.Text = Convert.ToString(-Convert.ToInt16(label71.Text) + Convert.ToInt16(row.Cells[3].Value));
                                    row.Cells[3].Value = Convert.ToString(Convert.ToInt16(label74.Text));
                                    StocksFlavoredSyrup= Int32.Parse(txtStocksFlavoredSyrup.Text);
                                    int sum = StocksFlavoredSyrup + 1;
                                    txtStocksFlavoredSyrup.Text = sum.ToString();

                                }

                            }

                        }
                    }

                }
            //caffee latte+
            private void button123_Click(object sender, EventArgs e)
            {

                bool found = false;
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;

                MySqlDataReader dataReader;


                command.CommandText = "select * from tblprodhotbvrgs where productid=10014";
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    label71.Text = dataReader.GetInt32("Price").ToString();
                    StocksCaffelatte = Int32.Parse(txtStocksCaffeeLatte.Text);
                    int totalp = StocksCaffelatte - 1;

                    if (totalp < 0)
                    {
                        MessageBox.Show("out of stocks");
                    }
                    else
                    {

                        if (dataGridView1.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {

                                if (Convert.ToString(row.Cells[0].Value) == "Caffe Latte" && Convert.ToString(row.Cells[1].Value) == label71.Text)
                                {
                                    row.Cells[2].Value = Convert.ToString(1 + Convert.ToInt16(row.Cells[2].Value));
                                    StocksCaffelatte = Int32.Parse(txtStocksCaffeeLatte.Text);
                                    txtStocksCaffeeLatte.Text = Convert.ToString(StocksCaffelatte - 1);
                                    found = true;
                                    label74.Text = Convert.ToString(Convert.ToInt16(row.Cells[3].Value));
                                    row.Cells[dataGridView1.Columns["Amount"].Index].Value = (Convert.ToDouble(row.Cells[dataGridView1.Columns["price"].Index].Value) * Convert.ToDouble(row.Cells[dataGridView1.Columns["qty"].Index].Value));


                                }

                            }
                            if (!found)
                            {
                                StocksCaffelatte = Int32.Parse(txtStocksCaffeeLatte.Text);
                                txtStocksCaffeeLatte.Text = Convert.ToString(StocksCaffelatte - 1);
                                dataGridView1.Rows.Add("Caffe Latte", label71.Text, 1, label71.Text);
                            }
                        }
                        else
                        {
                            StocksCaffelatte = Int32.Parse(txtStocksCaffeeLatte.Text);
                            txtStocksCaffeeLatte.Text = Convert.ToString(StocksCaffelatte - 1);
                            dataGridView1.Rows.Add("Caffe Latte", label71.Text, 1, label71.Text);
                        }
                    }

                }
                closeConnection();
            }

            //caffe latte-
            private void button122_Click(object sender, EventArgs e)
            {
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                MySqlDataReader dataReader;
                command.CommandText = "select * from tblprodhotbvrgs where productid=10014";

                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    label71.Text = dataReader.GetInt32("Price").ToString();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (Convert.ToString(row.Cells[0].Value) == "Caffe Latte" && Convert.ToString(row.Cells[1].Value) == label71.Text)
                        {
                            if (Convert.ToInt16(row.Cells[2].Value) == 0)
                            {
                                MessageBox.Show("no remaining orders");
                                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                                dataGridView1.Rows.RemoveAt(rowIndex);
                            }
                            else
                            {
                                row.Cells[2].Value = Convert.ToString(-1 + Convert.ToInt16(row.Cells[2].Value));

                                label74.Text = Convert.ToString(-Convert.ToInt16(label71.Text) + Convert.ToInt16(row.Cells[3].Value));
                                row.Cells[3].Value = Convert.ToString(Convert.ToInt16(label74.Text));
                                StocksCaffelatte= Int32.Parse(txtStocksCaffeeLatte.Text);
                                int sum = StocksCaffelatte + 1;
                                txtStocksCaffeeLatte.Text = sum.ToString();

                            }

                        }

                    }
                }
            }
            //fancy white+
            private void button114_Click(object sender, EventArgs e)
            {
                bool found = false;
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;

                MySqlDataReader dataReader;


                command.CommandText = "select * from tblprodhotbvrgs where productid=10015";
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    label71.Text = dataReader.GetInt32("Price").ToString();
                    StocksfancyWhite = Int32.Parse(txtStocksfancyWhite.Text);
                    int totalp = StocksfancyWhite- 1;

                    if (totalp < 0)
                    {
                        MessageBox.Show("out of stocks");
                    }
                    else
                    {

                        if (dataGridView1.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {

                                if (Convert.ToString(row.Cells[0].Value) == "Fancy White" && Convert.ToString(row.Cells[1].Value) == label71.Text)
                                {
                                    row.Cells[2].Value = Convert.ToString(1 + Convert.ToInt16(row.Cells[2].Value));
                                    StocksfancyWhite = Int32.Parse(txtStocksfancyWhite.Text);
                                    txtStocksfancyWhite.Text = Convert.ToString(StocksfancyWhite - 1);
                                    found = true;
                                    label74.Text = Convert.ToString(Convert.ToInt16(row.Cells[3].Value));
                                    row.Cells[dataGridView1.Columns["Amount"].Index].Value = (Convert.ToDouble(row.Cells[dataGridView1.Columns["price"].Index].Value) * Convert.ToDouble(row.Cells[dataGridView1.Columns["qty"].Index].Value));


                                }

                            }
                            if (!found)
                            {
                                StocksfancyWhite = Int32.Parse(txtStocksfancyWhite.Text);
                                txtStocksfancyWhite.Text = Convert.ToString(StocksfancyWhite - 1);
                                dataGridView1.Rows.Add("Fancy White", label71.Text, 1, label71.Text);
                            }
                        }
                        else
                        {
                            StocksfancyWhite = Int32.Parse(txtStocksfancyWhite.Text);
                            txtStocksfancyWhite.Text = Convert.ToString(StocksfancyWhite - 1);
                            dataGridView1.Rows.Add("Fancy White", label71.Text, 1, label71.Text);
                        }
                    }

                }
                closeConnection();
            }

            //fancy white-
            private void button112_Click(object sender, EventArgs e)
            {
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                MySqlDataReader dataReader;
                command.CommandText = "select * from tblprodhotbvrgs where productid=10015";

                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    label71.Text = dataReader.GetInt32("Price").ToString();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (Convert.ToString(row.Cells[0].Value) == "Fancy White" && Convert.ToString(row.Cells[1].Value) == label71.Text)
                        {
                            if (Convert.ToInt16(row.Cells[2].Value) == 0)
                            {
                                MessageBox.Show("no remaining orders");
                                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                                dataGridView1.Rows.RemoveAt(rowIndex);
                            }
                            else
                            {
                                row.Cells[2].Value = Convert.ToString(-1 + Convert.ToInt16(row.Cells[2].Value));

                                label74.Text = Convert.ToString(-Convert.ToInt16(label71.Text) + Convert.ToInt16(row.Cells[3].Value));
                                row.Cells[3].Value = Convert.ToString(Convert.ToInt16(label74.Text));
                                StocksfancyWhite = Int32.Parse(txtStocksfancyWhite.Text);
                                int sum = StocksfancyWhite + 1;
                                txtStocksfancyWhite.Text = sum.ToString();

                            }

                        }

                    }
                }
            }
            //snacks+
            private void button172_Click(object sender, EventArgs e)
            {
                bool found = false;
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;

                MySqlDataReader dataReader;


                command.CommandText = "select * from tblprodsnacks where productid=10019";
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    label71.Text = dataReader.GetInt32("Price").ToString();
                    StocksTacos = Int32.Parse(txtStocksTacos.Text);
                    int totalp = StocksTacos - 1;

                    if (totalp < 0)
                    {
                        MessageBox.Show("out of stocks");
                    }
                    else
                    {

                        if (dataGridView1.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {

                                if (Convert.ToString(row.Cells[0].Value) == "Tacos" && Convert.ToString(row.Cells[1].Value) == label71.Text)
                                {
                                    row.Cells[2].Value = Convert.ToString(1 + Convert.ToInt16(row.Cells[2].Value));
                                    StocksTacos = Int32.Parse(txtStocksTacos.Text);
                                    txtStocksTacos.Text = Convert.ToString(StocksTacos - 1);
                                    found = true;
                                    label74.Text = Convert.ToString(Convert.ToInt16(row.Cells[3].Value));
                                    row.Cells[dataGridView1.Columns["Amount"].Index].Value = (Convert.ToDouble(row.Cells[dataGridView1.Columns["price"].Index].Value) * Convert.ToDouble(row.Cells[dataGridView1.Columns["qty"].Index].Value));


                                }

                            }
                            if (!found)
                            {
                                StocksTacos= Int32.Parse(txtStocksTacos.Text);
                                txtStocksTacos.Text = Convert.ToString(StocksTacos - 1);
                                dataGridView1.Rows.Add("Tacos", label71.Text, 1, label71.Text);
                            }
                        }
                        else
                        {
                            StocksTacos = Int32.Parse(txtStocksTacos.Text);
                            txtStocksTacos.Text = Convert.ToString(StocksTacos - 1);
                            dataGridView1.Rows.Add("Tacos", label71.Text, 1, label71.Text);
                        }
                    }

                }
                closeConnection();
            }
            //snacks-
            private void button171_Click(object sender, EventArgs e)
            {
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                MySqlDataReader dataReader;
                command.CommandText = "select * from tblprodsnacks where productid=10019";

                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    label71.Text = dataReader.GetInt32("Price").ToString();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (Convert.ToString(row.Cells[0].Value) == "Tacos" && Convert.ToString(row.Cells[1].Value) == label71.Text)
                        {
                            if (Convert.ToInt16(row.Cells[2].Value) == 0)
                            {
                                MessageBox.Show("no remaining orders");
                                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                                dataGridView1.Rows.RemoveAt(rowIndex);
                            }
                            else
                            {
                                row.Cells[2].Value = Convert.ToString(-1 + Convert.ToInt16(row.Cells[2].Value));

                                label74.Text = Convert.ToString(-Convert.ToInt16(label71.Text) + Convert.ToInt16(row.Cells[3].Value));
                                row.Cells[3].Value = Convert.ToString(Convert.ToInt16(label74.Text));
                                StocksTacos = Int32.Parse(txtStocksTacos.Text);
                                int sum = StocksTacos + 1;
                                txtStocksTacos.Text = sum.ToString();

                            }

                        }

                    }
                }
            }
            //ham and veggies+
            private void button165_Click(object sender, EventArgs e)
            {
                bool found = false;
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;

                MySqlDataReader dataReader;


                command.CommandText = "select * from tblprodsnacks where productid=10020";
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    label71.Text = dataReader.GetInt32("Price").ToString();
                    StocksHamAndVeggies = Int32.Parse(txtStocksHamAndVeggies.Text);
                    int totalp = StocksHamAndVeggies - 1;

                    if (totalp < 0)
                    {
                        MessageBox.Show("out of stocks");
                    }
                    else
                    {

                        if (dataGridView1.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {

                                if (Convert.ToString(row.Cells[0].Value) == "Ham And Veggies" && Convert.ToString(row.Cells[1].Value) == label71.Text)
                                {
                                    row.Cells[2].Value = Convert.ToString(1 + Convert.ToInt16(row.Cells[2].Value));
                                    StocksHamAndVeggies = Int32.Parse(txtStocksHamAndVeggies.Text);
                                    txtStocksHamAndVeggies.Text = Convert.ToString(StocksHamAndVeggies - 1);
                                    found = true;
                                    label74.Text = Convert.ToString(Convert.ToInt16(row.Cells[3].Value));
                                    row.Cells[dataGridView1.Columns["Amount"].Index].Value = (Convert.ToDouble(row.Cells[dataGridView1.Columns["price"].Index].Value) * Convert.ToDouble(row.Cells[dataGridView1.Columns["qty"].Index].Value));


                                }

                            }
                            if (!found)
                            {
                                StocksHamAndVeggies = Int32.Parse(txtStocksHamAndVeggies.Text);
                                txtStocksHamAndVeggies.Text = Convert.ToString(StocksHamAndVeggies - 1);
                                dataGridView1.Rows.Add("Ham And Veggies", label71.Text, 1, label71.Text);
                            }
                        }
                        else
                        {
                            StocksHamAndVeggies = Int32.Parse(txtStocksHamAndVeggies.Text);
                            txtStocksHamAndVeggies.Text = Convert.ToString(StocksHamAndVeggies - 1);
                            dataGridView1.Rows.Add("Ham And Veggies", label71.Text, 1, label71.Text);
                        }
                    }

                }
            }

            //ham and veggies-
            private void button164_Click(object sender, EventArgs e)
            {
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                MySqlDataReader dataReader;
                command.CommandText = "select * from tblprodsnacks where productid=10020";

                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    label71.Text = dataReader.GetInt32("Price").ToString();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (Convert.ToString(row.Cells[0].Value) == "Ham And Veggies" && Convert.ToString(row.Cells[1].Value) == label71.Text)
                        {
                            if (Convert.ToInt16(row.Cells[2].Value) == 0)
                            {
                                MessageBox.Show("no remaining orders");
                                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                                dataGridView1.Rows.RemoveAt(rowIndex);
                            }
                            else
                            {
                                row.Cells[2].Value = Convert.ToString(-1 + Convert.ToInt16(row.Cells[2].Value));

                                label74.Text = Convert.ToString(-Convert.ToInt16(label71.Text) + Convert.ToInt16(row.Cells[3].Value));
                                row.Cells[3].Value = Convert.ToString(Convert.ToInt16(label74.Text));
                                StocksHamAndVeggies = Int32.Parse(txtStocksHamAndVeggies.Text);
                                int sum = StocksHamAndVeggies + 1;
                                txtStocksHamAndVeggies.Text = sum.ToString();

                            }

                        }

                    }
                }
            }

      
            private void timerBlinkHotBeverages_Tick(object sender, EventArgs e)
            {
                Label[] lblsHotBeverages;
                lblsHotBeverages = new Label[5] { lblHb1, lblHb2, lblHb3, lblHb4, lblHb5};
                lblHb1.Visible = false;
                lblHb2.Visible = false;
                lblHb3.Visible = false;
                lblHb4.Visible = false;
                lblHb5.Visible = false;
           

                if (blink < lblsHotBeverages.Length && up == true)
                {
                    lblsHotBeverages[blink].Visible = true;
                    blink++;
                }
                else
                {
                    blink--;
                    up = false;

                    if (blink != -1)
                    {
                        lblsHotBeverages[blink].Visible = true;

                    }
                    else
                    {
                        up = true;
                        blink = 0;
                        lblHb6.Visible = true;
                    }
                }
            }

            private void timerBlinkFrappe_Tick(object sender, EventArgs e)
            {
                Label[] lblsFrappe;
                lblsFrappe = new Label[5] { lbls1, lbls2, lbls3, lbls4, lbls5 };
                lbls1.Visible = false;
                lbls2.Visible = false;
                lbls3.Visible = false;
                lbls4.Visible = false;
                lbls5.Visible = false;


                if (blink < lblsFrappe.Length && up == true)
                {
                    lblsFrappe[blink].Visible = true;
                    blink++;
                }
                else
                {
                    blink--;
                    up = false;

                    if (blink != -1)
                    {
                        lblsFrappe[blink].Visible = true;

                    }
                    else
                    {
                        up = true;
                        blink = 0;
                        lbls6.Visible = true;
                    }
                }
            }

            private void timerBlinkSnacks_Tick(object sender, EventArgs e)
            {
                Label[] lblsSnacks;
                lblsSnacks = new Label[5] { label22, label23, label24, label25, label26 };
                label22.Visible = false;
                label23.Visible = false;
                label24.Visible = false;
                label25.Visible = false;
                label26.Visible = false;


                if (blink < lblsSnacks.Length && up == true)
                {
                    lblsSnacks[blink].Visible = true;
                    blink++;
                }
                else
                {
                    blink--;
                    up = false;

                    if (blink != -1)
                    {
                        lblsSnacks[blink].Visible = true;

                    }
                    else
                    {
                        up = true;
                        blink = 0;
                        label27.Visible = true;
                    }
                }
            }
            //carbonara+
            private void button144_Click(object sender, EventArgs e)
            {
                bool found = false;
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;

                MySqlDataReader dataReader;


                command.CommandText = "select * from tblprodsnacks where productid=10023";
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    label71.Text = dataReader.GetInt32("Price").ToString();
                    StocksCarbonara = Int32.Parse(txtStocksCarbonara.Text);
                    int totalp = StocksCarbonara- 1;

                    if (totalp < 0)
                    {
                        MessageBox.Show("out of stocks");
                    }
                    else
                    {

                        if (dataGridView1.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {

                                if (Convert.ToString(row.Cells[0].Value) == "Carbonara" && Convert.ToString(row.Cells[1].Value) == label71.Text)
                                {
                                    row.Cells[2].Value = Convert.ToString(1 + Convert.ToInt16(row.Cells[2].Value));
                                    StocksCarbonara= Int32.Parse(txtStocksCarbonara.Text);
                                    txtStocksCarbonara.Text = Convert.ToString(StocksCarbonara - 1);
                                    found = true;
                                    label74.Text = Convert.ToString(Convert.ToInt16(row.Cells[3].Value));
                                    row.Cells[dataGridView1.Columns["Amount"].Index].Value = (Convert.ToDouble(row.Cells[dataGridView1.Columns["price"].Index].Value) * Convert.ToDouble(row.Cells[dataGridView1.Columns["qty"].Index].Value));


                                }

                            }
                            if (!found)
                            {
                                StocksCarbonara = Int32.Parse(txtStocksCarbonara.Text);
                                txtStocksHamAndVeggies.Text = Convert.ToString(StocksCarbonara - 1);
                                dataGridView1.Rows.Add("Carbonara", label71.Text, 1, label71.Text);
                            }
                        }
                        else
                        {
                            StocksCarbonara = Int32.Parse(txtStocksCarbonara.Text);
                            txtStocksCarbonara.Text = Convert.ToString(StocksCarbonara - 1);
                            dataGridView1.Rows.Add("Carbonara", label71.Text, 1, label71.Text);
                        }
                    }

                }
            }

            //carbonara-
            private void button143_Click(object sender, EventArgs e)
            {
                openConnection();
                command = new MySqlCommand();
                command.Connection = connection;
                MySqlDataReader dataReader;
                command.CommandText = "select * from tblprodsnacks where productid=10023";

                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    label71.Text = dataReader.GetInt32("Price").ToString();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (Convert.ToString(row.Cells[0].Value) == "Carbonara" && Convert.ToString(row.Cells[1].Value) == label71.Text)
                        {
                            if (Convert.ToInt16(row.Cells[2].Value) == 0)
                            {
                                MessageBox.Show("no remaining orders");
                                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                                dataGridView1.Rows.RemoveAt(rowIndex);
                            }
                            else
                            {
                                row.Cells[2].Value = Convert.ToString(-1 + Convert.ToInt16(row.Cells[2].Value));

                                label74.Text = Convert.ToString(-Convert.ToInt16(label71.Text) + Convert.ToInt16(row.Cells[3].Value));
                                row.Cells[3].Value = Convert.ToString(Convert.ToInt16(label74.Text));
                                StocksCarbonara = Int32.Parse(txtStocksCarbonara.Text);
                                int sum = StocksCarbonara+ 1;
                                txtStocksCarbonara.Text = sum.ToString();

                            }

                        }

                    }
                }
            }

            private void button1_Click(object sender, EventArgs e)
            {
                //adminForm admin = new adminForm(lblLog.Text);
                //admin.Show();
                //this.Hide();
            }

            private void orderForm_FormClosing(object sender, FormClosingEventArgs e)
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
                   // this.Hide();

                  //  Environment.Exit(0);
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

            private void button52_Click(object sender, EventArgs e)
            {
                pre.Document = printDocument1;
                pre.ShowDialog();
            }

            private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
            {
                string TotalQty = "";
                string productName = "";
                string qtyPerProd = "";
                string pricePerProd = "";
                string totalPrice = "";

                openConnection();
                command.CommandText = "select * from tblproductsales where TransactionNo = '"+label78.Text+"'";
               // MySqlDataReader receipt = command.ExecuteReader();

               /* while (receipt.Read())
                {

                    productName = receipt["productName"].ToString();
                    qtyPerProd = receipt["qty"].ToString();
                    totalPrice = receipt["TotalSales"].ToString();

                }*/


                try
                {
                    Graphics graph = e.Graphics;

                    Font fontWelcome = new Font("Courier New", 18);

                    Font fontProducts = new Font("Courier New", 12);

                    SolidBrush sb = new SolidBrush(Color.DarkGray);

                    String test = DateTime.Now.ToString("MM.dd.yyyy");
                    graph.DrawString("Welcome to Le Maleche Coffee Shop", fontWelcome, sb, 150, 15);
                    graph.DrawString("#7 Oaks St, San Isidro Cainta Rizal", fontProducts, sb, 250, 40);
                    graph.DrawString("Mobile: 0975-XXX-XXXX", fontProducts, sb, 300, 60);
                    graph.DrawString("E-Mail: LeChe@Example.com", fontProducts, sb, 280, 80);
                    graph.DrawString("TiN No: XXX-XXX-XXX-000", fontProducts, sb, 300, 100);
                    graph.DrawString("Permit No: XXX-XXX-XXX-000", fontProducts, sb, 280, 120);
                    graph.DrawString("Date:" + lblDateOrderForm.Text, fontProducts, sb, 300, 140);
                    graph.DrawString("Time:" + lblTimeOrderForm.Text, fontProducts, sb, 320, 160);
                    graph.DrawString("Transaction No:" +label78.Text, fontProducts, sb, 320, 140);

                    graph.DrawString("Cashier Name:"+lblLog.Text, fontProducts, sb, 10, 200);
                    graph.DrawString("Client Name:" +textBox1.Text, fontProducts, sb, 10, 220);

                    graph.DrawString("Date:" + test, fontProducts, sb, 660, 200);
                    graph.DrawString("Time:" + lblTimeOrderForm.Text, fontProducts, sb, 660, 220);

                    graph.DrawString("------------------------------------------------------------------------------", fontProducts, sb, 10, 240);
                    graph.DrawString("------------------------------------------------------------------------------", fontProducts, sb, 10, 260);
                    openConnection();
                  /*  command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "select * from tblproductsales where transactionno = 65";*/
                    MySqlDataReader reader = command.ExecuteReader();


                    string records = "";
                    string total = "";
                    List<string> myList = new List<string>();

                    while (reader.Read())
                    {
                      records = reader[2].ToString() + "," + reader[3].ToString() + "," + reader[4].ToString();
                       // records = reader[2].ToString();
                       // total = reader[3].ToString();
                        myList.Add(records);
                        myList.Add(total);
                    }
                    int spacing = 40;

                    for (int i = 0; i < myList.Count; i++)
                    {
                       // graph.DrawString(myList[i], fontProducts, sb, 10, 300 + spacing);
                        graph.DrawString(myList[i], fontProducts, sb, 100, 300 + spacing);
                        spacing = spacing + 20;
                    }

                    spacing = spacing + 20;
/*
                    List<string> myListQty = new List<string>();

                    while (reader.Read())
                    {
                        //  records = reader[2].ToString() + "," + reader[3].ToString() + "," + reader[4].ToString();
                        //records = reader[2].ToString();
                        total = reader[3].ToString();
                        //myList.Add(records);
                        myListQty.Add(total);
                    }
                    int spacing2 = 40;

                    for (int x = 0; x < myListQty.Count; x++)
                    {
                        //graph.DrawString(myList[i], fontProducts, sb, 10, 300 + spacing2);
                        graph.DrawString(myListQty[x], fontProducts, sb, 100, 300 + spacing2);
                        spacing2 = spacing2 + 20;
                    }

                    spacing2 = spacing2 + 20;*/

                    //Bitmap bm = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
                    //dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));
                    //e.Graphics.DrawImage(bm, 100, 300);


                    graph.DrawString("------------------------------------------------------------------------------", fontProducts, sb, 10, 600);
                    graph.DrawString("------------------------------------------------------------------------------", fontProducts, sb, 10, 630);
                    graph.DrawString("Sub-Total:", fontWelcome, sb, 10, 660);
                    graph.DrawString(textBox14.Text, fontWelcome, sb, 660, 660);
                    graph.DrawString("Discount:", fontWelcome, sb, 10, 680);
                    graph.DrawString(textBox3.Text, fontWelcome, sb, 610, 680);
                    graph.DrawString("("+textBox2.Text+")%", fontWelcome, sb, 700, 680);
                    graph.DrawString("Total Amount:", fontWelcome, sb, 10, 700);
                    graph.DrawString(textBox15.Text, fontWelcome, sb, 660, 700);
                    graph.DrawString("Cash:", fontWelcome, sb, 10, 720);
                    graph.DrawString(textBox16.Text, fontWelcome, sb, 660, 720);
                    graph.DrawString("Change:", fontWelcome, sb, 10, 740);
                    graph.DrawString(textBox17.Text, fontWelcome, sb, 660, 740);
                    
                }
                catch { }
            }

           
            private void button3_Click(object sender, EventArgs e)
            {
                adminForm admin = new adminForm(lblLog.Text);
                admin.Show();
                this.Hide();
            }

            private void button4_Click(object sender, EventArgs e)
            {
                printDialog1.Document = printDocument1;
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }

            private void button5_Click(object sender, EventArgs e)
            {
               // Authentication au = new Authentication();
                VoidTransactions au = new VoidTransactions();
                au.Show();
                
            }


          

           



            
     }

}  

