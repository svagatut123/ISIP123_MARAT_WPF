using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using WpfApp1;

namespace WpfApp1

{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var login = TxtLogin.Text.Trim();
                var pass = TxtPassword.Text;

                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass))
                {
                    MessageBox.Show("Введите логин и пароль");
                    return;
                }

                // Обязательно включаем загрузку роли
                var user = Core.Context.Users
                    .Include(u => u.Roles)
                    .FirstOrDefault(u => u.Login == login && u.Password == pass);

                if (user != null)
                {
                    Core.CurrentUser = user;
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка входа: " + ex.Message);
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, что поля не пустые
                if (string.IsNullOrEmpty(TxtLogin.Text) || string.IsNullOrEmpty(TxtPassword.Text))
                {
                    MessageBox.Show("Введите логин и пароль");
                    return;
                }

                // Проверяем, что пользователь с таким логином ещё не существует
                var existingUser = Core.Context.Users
                    .FirstOrDefault(u => u.Login == TxtLogin.Text.Trim());

                if (existingUser != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                    return;
                }

                // Получаем роль "Читатель" (обычно RoleId = 1)
                var readerRole = Core.Context.Roles
                    .FirstOrDefault(r => r.RoleName == "Читатель");

                if (readerRole == null)
                {
                    MessageBox.Show("Ошибка: роль 'Читатель' не найдена в базе данных");
                    return;
                }

                // Создаём нового пользователя
                var newUser = new Users
                {
                    Login = TxtLogin.Text.Trim(),
                    Password = TxtPassword.Text, // В реальном приложении нужно хешировать!
                    Email = TxtLogin.Text.Trim() + "@example.com", // Заглушка для email
                    DisplayName = TxtLogin.Text.Trim(), // Заглушка для имени
                    RoleId = readerRole.RoleId,
                    IsFrozen = false,
                    RegistrationDate = DateTime.Now
                };

                Core.Context.Users.Add(newUser);
                Core.Context.SaveChanges();

                MessageBox.Show("Регистрация успешна! Теперь войдите в систему.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка регистрации: " + ex.Message);
            }
        }
    }
}