﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mainForm
{
    public partial class customerOrderForm : Form
    {
        public customerOrderForm(List<customer> tempList)
        {
            InitializeComponent();
            dataGridView2.DataSource = tempList;
        }
    }
}
