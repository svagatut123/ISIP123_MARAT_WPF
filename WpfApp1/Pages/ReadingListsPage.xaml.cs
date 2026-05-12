using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class ReadingListsPage : Page
    {
        public ReadingListsPage()
        {
            InitializeComponent();
            StatusComboBox.SelectedIndex = 0;
            RefreshData();
        }

        private void RefreshData()
        {
            if (Core.CurrentUser == null || StatusComboBox.SelectedItem == null) return;

            string selectedStatus = (StatusComboBox.SelectedItem as ComboBoxItem).Content.ToString();

            var myBooks = Core.Context.ReadingLists
                .Where(rl => rl.UserId == Core.CurrentUser.UserId && rl.Status == selectedStatus)
                .ToList();

            ListsListView.ItemsSource = myBooks;
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshData();
        }

        private void MoveToRead_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var record = Core.Context.ReadingLists.FirstOrDefault(r => r.ReadingListId == id);

            if (record != null)
            {
                record.Status = "Прочитано";
                Core.Context.SaveChanges();
                RefreshData();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var record = Core.Context.ReadingLists.FirstOrDefault(r => r.ReadingListId == id);

            if (record != null)
            {
                Core.Context.ReadingLists.Remove(record);
                Core.Context.SaveChanges();
                RefreshData();
            }
        }
    }
}