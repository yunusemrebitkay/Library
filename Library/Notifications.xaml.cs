﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Library
{

    public partial class Notifications : Window
    {
        public Notifications()
        {
            InitializeComponent();
            GlobalMethods.GetNotifications(this);
        }
    }
}
