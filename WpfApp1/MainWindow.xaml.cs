using System.Windows;
using System.Windows.Controls;
using WpfApp1.Pages;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Page1());
        }

        private void GoToBuilder_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page1());
        }

        private void GoToHistory_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page2());
        }
    }
}