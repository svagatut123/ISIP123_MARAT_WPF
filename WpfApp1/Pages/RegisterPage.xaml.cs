using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // 1. Валидация заполнения полей
            if (string.IsNullOrWhiteSpace(LoginBox.Text) ||
                string.IsNullOrWhiteSpace(NameBox.Text) ||
                string.IsNullOrWhiteSpace(EmailBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Заполните все текстовые поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 2. Проверка уникальности логина
                string cleanedLogin = LoginBox.Text.Trim();
                bool loginExists = Core.Context.Users.Any(u => u.Login.ToLower() == cleanedLogin.ToLower());

                if (loginExists)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // 3. Создание нового пользователя
                var newUser = new Users()
                {
                    Login = cleanedLogin,
                    DisplayName = NameBox.Text.Trim(),
                    Email = EmailBox.Text.Trim(),
                    Password = PasswordBox.Password,
                    IsFrozen = false
                };

                // Пытаемся записать дату регистрации, если такое поле есть в бд
                var dateProp = newUser.GetType().GetProperty("RegistrationDate");
                if (dateProp != null && dateProp.CanWrite) dateProp.SetValue(newUser, DateTime.Now);

                // 4. Динамическое присвоение базовой роли "Пользователь" (ID = 1)
                var roleProp = newUser.GetType().GetProperties().FirstOrDefault(p =>
                    (p.Name.Contains("Role") || p.Name.Contains("Status") || p.Name.Contains("Type")) &&
                    (p.PropertyType == typeof(int) || p.PropertyType == typeof(int?) || p.PropertyType == typeof(string)) &&
                    !p.PropertyType.IsGenericType);

                if (roleProp != null && roleProp.CanWrite)
                {
                    if (roleProp.PropertyType == typeof(int) || roleProp.PropertyType == typeof(int?))
                    {
                        roleProp.SetValue(newUser, 1); // 1 — Обычный пользователь/читатель по умолчанию
                    }
                    else
                    {
                        roleProp.SetValue(newUser, "Пользователь");
                    }
                }

                // 5. Сохранение в БД
                Core.Context.Users.Add(newUser);
                Core.Context.SaveChanges();

                MessageBox.Show("Регистрация успешно завершена! Теперь вы можете войти.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Возвращаем на страницу входа
                this.NavigationService.Navigate(new LoginWindow());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критическая ошибка при регистрации: " + ex.Message, "Ошибка БД");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginWindow());
        }
    }
}