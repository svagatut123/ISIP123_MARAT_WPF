using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            UsersGrid.ItemsSource = Core.Context.Users.Include("Roles").ToList();

            ComplaintsGrid.ItemsSource = Core.Context.Complaints.Include("Users").ToList();

            AuthorRequestsGrid.ItemsSource = Core.Context.RoleRequests
                .Include("Users")
                .Where(r => r.Status == "На рассмотрении")
                .ToList();
        }
        private void DeclineAuthor_Click(object sender, RoutedEventArgs e)
        {
            int reqId = (int)((Button)sender).Tag;
            var request = Core.Context.RoleRequests.FirstOrDefault(r => r.RequestId == reqId);

            if (request != null)
            {
                request.Status = "Отклонено";
                Core.Context.SaveChanges();
                LoadData(); 
                MessageBox.Show("заявка отклонена");
            }
        }

        private void AcceptAuthor_Click(object sender, RoutedEventArgs e)
        {
            var request = (sender as Button).DataContext as RoleRequests;
            if (request != null)
            {
                request.Users.RoleId = 2;
              
                request.Status = "Принята";

                Core.Context.SaveChanges();
                LoadData();
                MessageBox.Show($"Пользователь {request.Users.DisplayName} теперь автор!");
            }
        }

        private void RejectRequest_Click(object sender, RoutedEventArgs e)
        {
            var request = (sender as Button).DataContext as RoleRequests;
            if (request != null)
            {
                request.Status = "Отклонена";
                Core.Context.SaveChanges();
                LoadData();
            }
        }

        private void ToggleFreeze_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).DataContext as Users;

            if (user != null)
            {
                if (user.UserId == Core.CurrentUser.UserId)
                {
                    MessageBox.Show("Вы не можете заморозить свою учетную запись администратора!");
                    return;
                }

                user.IsFrozen = !user.IsFrozen;

                try
                {
                    Core.Context.SaveChanges();

                    LoadData();

                    string status = user.IsFrozen ? "заморожен" : "разморожен";
                    MessageBox.Show($"Пользователь {user.DisplayName} теперь {status}.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении: " + ex.Message);
                }
            }
        }
        private void DeleteComplaint_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var complaint = Core.Context.Complaints.FirstOrDefault(c => c.ComplaintId == id);

            if (complaint != null)
            {
                Core.Context.Complaints.Remove(complaint);
                Core.Context.SaveChanges();
                LoadData(); 
            }
        }
    }
}