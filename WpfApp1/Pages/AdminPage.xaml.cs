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
            int reqId = (int)((Button)sender).Tag;
            var request = Core.Context.RoleRequests.FirstOrDefault(r => r.RequestId == reqId);

            if (request != null)
            {
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
                LoadData(); 
            }
        }
    }
}