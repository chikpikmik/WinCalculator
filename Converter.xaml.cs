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
    /// Логика взаимодействия для Converter.xaml
    /// </summary>
    public partial class Converter : Page
    {
        public List<QueryElement> Items { get; set; }
        
        public Converter(string pageName)
        {
            InitializeComponent();

            this.Items = CommonFunctions.GetListFromDB(pageName);
            DataContext = this;
            First_ComboBox.SelectedIndex = 0;
            Second_ComboBox.SelectedIndex = 1;

            foreach (var el in Everything.Children)
            {
                if (el is Button button && button.Content != null)
                {
                    button.Click += Button_click;
                }
            }

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (First_ComboBox.SelectedIndex < 0 || Second_ComboBox.SelectedIndex < 0)
                return;


            ComboBox ChangedComboBox = sender as ComboBox;
            ComboBox UnchangedComboBox;
            TextBox UnchangedTextBox;
            TextBox ChangedTextBox;
            if (ChangedComboBox == First_ComboBox) 
            {
                UnchangedComboBox = Second_ComboBox;
                UnchangedTextBox = Second_TextBox;
                ChangedTextBox = First_TextBox;
            }
            else
            {
                UnchangedComboBox = First_ComboBox;
                UnchangedTextBox = First_TextBox;
                ChangedTextBox = Second_TextBox;
            }
            double A, B;
            A = Items[ChangedComboBox.SelectedIndex].Value;
            B = Items[UnchangedComboBox.SelectedIndex].Value;

            CommonFunctions.ComboBox_SelectionChanged(
                ChangedComboBox, UnchangedComboBox, 
                ChangedTextBox, UnchangedTextBox, 
                A,B);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox ChangedTextBox = sender as TextBox;
            
            if (ChangedTextBox.IsFocused)
            {
                double A, B;
                TextBox UnchangedTextBox;
                if (ChangedTextBox != First_TextBox) 
                {
                    UnchangedTextBox = First_TextBox;
                    A = Items[Second_ComboBox.SelectedIndex].Value;
                    B = Items[First_ComboBox.SelectedIndex].Value;
                }
                else
                {
                    UnchangedTextBox = Second_TextBox;
                    A = Items[First_ComboBox.SelectedIndex].Value;
                    B = Items[Second_ComboBox.SelectedIndex].Value;
                }

                CommonFunctions.TextBox_TextChanged(ChangedTextBox, UnchangedTextBox, A,B);
            }
        }

        private void gotfocustextbox(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
        }

        private void Button_click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            var Calculator_TextBox = First_TextBox.IsFocused ? First_TextBox : Second_TextBox;
            CommonFunctions.Button_click(Calculator_TextBox, b);

        }

    }
}
