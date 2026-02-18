using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class LoginPage : Page
    {
        private MainWindow mainWindow;

        public LoginPage(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Введите email");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            var user = Core.Context.users.FirstOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                Core.CurrentUser = user; 
                MessageBox.Show($"Добро пожаловать, {user.firstname} {user.lastname}!");
                mainWindow.MainFrame.Content = new HomePage(mainWindow);
            }
            else
            {
                MessageBox.Show("Неверный email или пароль");
            }
        }
    }
}