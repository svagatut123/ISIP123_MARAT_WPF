using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class RegisterPage : Page
    {
        private MainWindow mainWindow;

        public RegisterPage(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
        }

        // Метод для тестирования регистрации
        public bool Register(string email, string password, string firstname, string lastname, string phone)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Введите email");
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль");
                return false;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен быть не менее 6 символов");
                return false;
            }

            if (string.IsNullOrWhiteSpace(firstname))
            {
                MessageBox.Show("Введите имя");
                return false;
            }

            if (string.IsNullOrWhiteSpace(lastname))
            {
                MessageBox.Show("Введите фамилию");
                return false;
            }

            var existingUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким email уже существует");
                return false;
            }

            try
            {
                var newUser = new users
                {
                    email = email,
                    password = password,
                    firstname = firstname,
                    lastname = lastname,
                    phone_number = phone
                };

                Core.Context.users.Add(newUser);
                Core.Context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                MessageBox.Show("Ошибка при регистрации");
                return false;
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            string firstname = FirstNameBox.Text.Trim();
            string lastname = LastNameBox.Text.Trim();
            string phone = "";

            if (Register(email, password, firstname, lastname, phone))
            {
                MessageBox.Show("Регистрация успешна! Теперь войдите в аккаунт");
                mainWindow.MainFrame.Content = new LoginPage(mainWindow);
            }
        }
    }
}