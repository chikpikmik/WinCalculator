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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinCalculator
{
    /// <summary>
    /// Логика взаимодействия для Weight.xaml
    /// </summary>
    public partial class Weight : Page
    {
        public Weight()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonFunctions.TextBox_TextChanged(sender as TextBox);
        }
    }
}
