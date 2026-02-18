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

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            string firstname = FirstNameBox.Text.Trim();
            string lastname = LastNameBox.Text.Trim();

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

            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен быть не менее 6 символов");
                return;
            }

            if (string.IsNullOrWhiteSpace(firstname))
            {
                MessageBox.Show("Введите имя");
                return;
            }

            if (string.IsNullOrWhiteSpace(lastname))
            {
                MessageBox.Show("Введите фамилию");
                return;
            }

            var existingUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким email уже существует");
                return;
            }

            try
            {
                var newUser = new users
                {
                    email = email,
                    password = password,
                    firstname = firstname,
                    lastname = lastname
                };

                Core.Context.users.Add(newUser);
                Core.Context.SaveChanges();

                MessageBox.Show("Регистрация успешна! Теперь войдите в аккаунт");
                mainWindow.MainFrame.Content = new LoginPage(mainWindow);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}");
            }
        }
    }
}