using System.Windows;
using WpfApp1.Pages;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new HomePage(this);
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new HomePage(this);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new LoginPage(this);
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new RegisterPage(this);
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ProfilePage(this);
        }
    }
}