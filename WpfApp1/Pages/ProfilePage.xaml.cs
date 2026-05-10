using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using WpfApp1;

namespace WpfApp1.Pages
{
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            LoadProfileData();
        }

        private void LoadProfileData()
        {
            try
            {
                // 1. Проверка, что пользователь вошел в систему
                if (Core.CurrentUser == null)
                {
                    MessageBox.Show("Ошибка: вы не вошли в систему.");
                    // Возвращаем на страницу входа
                    var login = new LoginWindow();
                    login.Show();
                    Application.Current.MainWindow?.Close();
                    return;
                }

                // 2. Проверка, что роль загружена
                if (Core.CurrentUser.Roles == null)
                {
                    MessageBox.Show("Ошибка: не удалось загрузить роль пользователя.");
                    return;
                }

                // 3. Заполняем поля профиля
                TxtName.Text = "Имя: " + Core.CurrentUser.DisplayName;
                TxtLogin.Text = "Логин: " + Core.CurrentUser.Login;
                TxtEmail.Text = "Email: " + Core.CurrentUser.Email;
                TxtRole.Text = "Роль: " + Core.CurrentUser.Roles.RoleName;

                // 4. Загружаем отзывы пользователя
                var reviews = Core.Context.Reviews
                    .Where(r => r.UserId == Core.CurrentUser.UserId)
                    .ToList();
                GridMyReviews.ItemsSource = reviews;

                // 5. Проверяем заморозку
                if (Core.CurrentUser.IsFrozen == true)
                {
                    var existingRequest = Core.Context.UnfreezeRequests
                        .FirstOrDefault(u => u.UserId == Core.CurrentUser.UserId && u.Status == "На рассмотрении");

                    if (existingRequest == null)
                    {
                        var result = MessageBox.Show(
                            "Ваш аккаунт заморожен. Хотите оспорить заморозку?",
                            "Заморозка",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            Core.Context.UnfreezeRequests.Add(new UnfreezeRequests
                            {
                                UserId = Core.CurrentUser.UserId,
                                TargetType = "Аккаунт",
                                Reason = "Прошу разморозить",
                                Status = "На рассмотрении",
                                RequestDate = DateTime.Now
                            });
                            Core.Context.SaveChanges();
                            MessageBox.Show("Заявка отправлена");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки профиля: " + ex.Message);
            }
        }

        private void BtnRequestAuthor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Core.CurrentUser == null)
                {
                    MessageBox.Show("Пользователь не авторизован");
                    return;
                }

                var exists = Core.Context.RoleRequests
                    .Any(r => r.UserId == Core.CurrentUser.UserId && r.Status == "На рассмотрении");

                if (exists)
                {
                    MessageBox.Show("Заявка уже отправлена");
                    return;
                }

                Core.Context.RoleRequests.Add(new RoleRequests
                {
                    UserId = Core.CurrentUser.UserId,
                    RequestedRole = "Автор",
                    Status = "На рассмотрении",
                    RequestDate = DateTime.Now
                });
                Core.Context.SaveChanges();
                MessageBox.Show("Заявка на роль отправлена");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Core.CurrentUser = null;
                var login = new LoginWindow();
                login.Show();
                Application.Current.MainWindow?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выходе: " + ex.Message);
            }
        }
    }
}