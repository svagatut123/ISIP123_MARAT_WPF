using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Data.Entity;

namespace WpfApp1.Pages
{
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            GridComplaints.ItemsSource = Core.Context.Complaints.ToList();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            GridComplaints.ItemsSource = Core.Context.Complaints.ToList();
            MessageBox.Show("Списки обновлены");
        }
    }
}