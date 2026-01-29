using System.Windows;
using WpfApp1.pages;


namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BtnPage1_Click(null, null);
        }
        private void BtnPage1_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page1());

        }
        private void BtnPage2_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page2());

        }
        private void BtnPage3_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page3());

        }
    }
}