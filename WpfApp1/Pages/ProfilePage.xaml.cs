using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class ProfilePage : Page
    {
        private MainWindow mainWindow;

        public ProfilePage(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.Content = new LoginPage(mainWindow);
        }
    }
}