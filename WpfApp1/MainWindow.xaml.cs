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

        private void NewClick(object sender, RoutedEventArgs e)
        {
            BuildData.Clear();
            MainFrame.Navigate(new Page1());
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page2());
        }

        private void ListClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page3());
        }
    }
}