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

        public bool Auth(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false; // Просто возвращаем false
            }

            // Удаляем пробелы в начале и конце
            email = email.Trim();
            password = password.Trim();

            var user = Core.Context.users.FirstOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                Core.CurrentUser = user;
                return true;
            }
            else
            {
                return false; // Без MessageBox!
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (Auth(email, password))
            {
                MessageBox.Show($"Добро пожаловать, {Core.CurrentUser.firstname} {Core.CurrentUser.lastname}!");
                mainWindow.MainFrame.Content = new HomePage(mainWindow);
            }
            else
            {
                MessageBox.Show("Неверный email или пароль");
            }
        }
    }
}