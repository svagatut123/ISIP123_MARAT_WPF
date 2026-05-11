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
            ComplaintsGrid.ItemsSource = Core.Context.Complaints.ToList();
            UsersGrid.ItemsSource = Core.Context.Users.ToList();
        }
        // метод для отклонения заявки на роль автора
        private void DeclineAuthor_Click(object sender, RoutedEventArgs e)
        {
            int reqId = (int)((Button)sender).Tag;
            var request = Core.Context.RoleRequests.FirstOrDefault(r => r.RequestId == reqId);

            if (request != null)
            {
                // вместо удаления можно просто поставить статус "Отклонено"
                request.Status = "Отклонено";
                Core.Context.SaveChanges();
                LoadData(); // обновляем таблицы на странице
                MessageBox.Show("заявка отклонена");
            }
        }

        // уточненный метод принятия заявки
        private void AcceptAuthor_Click(object sender, RoutedEventArgs e)
        {
            int reqId = (int)((Button)sender).Tag;
            var request = Core.Context.RoleRequests.FirstOrDefault(r => r.RequestId == reqId);

            if (request != null)
            {
                // меняем роль пользователю (обычно id автора = 2, проверь свою таблицу Roles)
                request.Users.RoleId = 2;
                request.Status = "Принято";

                Core.Context.SaveChanges();
                LoadData();
                MessageBox.Show("пользователь теперь автор!");
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
                LoadData(); // обновляем таблицу
            }
        }
    }
}