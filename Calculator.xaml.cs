using System;
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
    /// Логика взаимодействия для Calculator.xaml
    /// </summary>
    public partial class Calculator : Page
    {
        public Calculator()
        {
            InitializeComponent();
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonFunctions.TextBox_TextChanged(sender as TextBox);
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            double ComputedExpression = CommonFunctions.ComputeExpression(Calculator_TextBox.Text);
            if (double.IsInfinity(ComputedExpression))
            {
                Calculator_TextBox.Text = "∞";
            }
            else if (!ComputedExpression.Equals(double.NaN))
            {
                Calculator_TextBox.Text = ComputedExpression.ToString();
            }
 
        }


    }
}
