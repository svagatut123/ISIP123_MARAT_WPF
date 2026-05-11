using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (Core.CurrentUser != null)
            {
                NameTextBlock.Text = "имя: " + Core.CurrentUser.DisplayName;
                LoginTextBlock.Text = "логин: " + Core.CurrentUser.Login;
                EmailTextBlock.Text = "почта: " + Core.CurrentUser.Email;

                // подгружаем отзывы текущего пользователя
                var userReviews = Core.Context.Reviews
                    .Where(r => r.UserId == Core.CurrentUser.UserId)
                    .ToList();

                ReviewsDataGrid.ItemsSource = userReviews;
            }
        }

        private void ApplyAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            // создаем новую заявку на роль
            RoleRequests newRequest = new RoleRequests()
            {
                UserId = Core.CurrentUser.UserId,
                RequestedRole = "Автор",
                RequestDate = DateTime.Now,
                Status = "На рассмотрении"
            };

            Core.Context.RoleRequests.Add(newRequest);
            Core.Context.SaveChanges();

            MessageBox.Show("заявка успешно отправлена");
        }
    }
}