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
        bool FirstFocus=true;
        public Calculator()
        {
            InitializeComponent();

            foreach (var el in Everything.Children) 
            {
                if (el is Button button && button.Content!=null)
                {
                    button.Click += Button_click;
                }
            }

        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonFunctions.TextBox_TextChanged(sender as TextBox);
        }

        private void Button_click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            string content = b.Content.ToString();
            string name = b.Name;

            string selectedtext = Calculator_TextBox.SelectedText;
            int selectionStart = Calculator_TextBox.SelectionStart;
            int selectionLength = Calculator_TextBox.SelectionLength;

            int oldpos = Calculator_TextBox.SelectionStart;
            string oldstr = Calculator_TextBox.Text;

            if (content == "=")
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
            else if (name == "Backspace")
            {
                

                if (oldstr.Length == 1)
                    Calculator_TextBox.Text = "0";
                else if (!string.IsNullOrEmpty(selectedtext))
                {
                    Calculator_TextBox.Text = Calculator_TextBox.Text.Remove(selectionStart, selectionLength);
                    Calculator_TextBox.SelectionStart = selectionStart;
                }
                else
                {
                    Calculator_TextBox.Text = oldstr.Remove(oldpos-1,1);
                    Calculator_TextBox.SelectionStart = oldpos - 1;
                }
            }
            else if (content == "C")
                Calculator_TextBox.Text = "0";

            else if (!string.IsNullOrEmpty(selectedtext))
            {
                Calculator_TextBox.Text = Calculator_TextBox.Text.Remove(selectionStart, selectionLength).Insert(selectionStart, content);
                Calculator_TextBox.SelectionStart = selectionStart;
            }

            else
            {
                Calculator_TextBox.Text = oldstr.Insert(oldpos, content); ;
                Calculator_TextBox.SelectionStart = oldpos + 1;
            }
            
            
 
        }

        private void gotfocustextbox(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
        }

    }
}
