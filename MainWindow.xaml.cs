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
using System.Windows.Media.Animation;

namespace WinCalculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new Calculator();
        }


        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }
        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);

            if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Normal; // Restore window
            }
            else
            {
                window.WindowState = WindowState.Maximized; // Maximize window
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void AnimationCompleted(object sender, EventArgs e)
        {
            Menu.Visibility = Visibility.Hidden;
        }
        private void OpenMenu() 
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.15));
            TranslateTransform trans = new TranslateTransform();
            Menu.RenderTransform = trans;
            animation.From = -256;
            animation.To = 0;
            Menu.Visibility = Visibility.Visible;
            trans.BeginAnimation(TranslateTransform.XProperty, animation);
        }
        private void CloseMenu() 
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.15));
            TranslateTransform trans = new TranslateTransform();
            Menu.RenderTransform = trans;
            animation.From = 0;
            animation.To = -256;
            animation.Completed += AnimationCompleted;
            trans.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private void Menu_button_Click(object sender, RoutedEventArgs e)
        {

            if (Menu.Visibility == Visibility.Visible)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }

        }

        private void Converter_click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            MainFrame.Navigate(new Converter(clickedButton.Name));
            CloseMenu();
        }
        private void Calculator_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Calculator());
            CloseMenu();
        }

        
    }
}
