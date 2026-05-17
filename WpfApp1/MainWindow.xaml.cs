using System.Windows;
using WpfApp1.Pages;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateNavigationMenu(); 

            MainFrame.Navigate(new CatalogPage());
        }

        private void UpdateNavigationMenu()
        {
            if (Core.CurrentUser == null)
            {
                ListsButton.Visibility = Visibility.Collapsed;
                ProfileButton.Visibility = Visibility.Collapsed;
                AuthorButton.Visibility = Visibility.Collapsed;
                AdminButton.Visibility = Visibility.Collapsed;
                return;
            }

            ListsButton.Visibility = Visibility.Visible;
            ProfileButton.Visibility = Visibility.Visible;

            if (Core.CurrentUser.IsFrozen == true)
            {
                WarningButton.Visibility = Visibility.Visible;
            }
            else
            {
                WarningButton.Visibility = Visibility.Collapsed;
            }

            if (Core.CurrentUser.RoleId == 3)
            {
                AdminButton.Visibility = Visibility.Visible;
                AuthorButton.Visibility = Visibility.Collapsed;
            }
            else if (Core.CurrentUser.RoleId == 2)
            {
                AdminButton.Visibility = Visibility.Collapsed;
                AuthorButton.Visibility = Visibility.Visible;
            }
            else if (Core.CurrentUser.RoleId == 1)
            {
                AdminButton.Visibility = Visibility.Collapsed;
                AuthorButton.Visibility = Visibility.Collapsed;
            }
        }

        private void CatalogButton_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new CatalogPage());
        private void ListsButton_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new ReadingListsPage());
        private void AuthorButton_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new AuthorPage());
        private void AdminButton_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new AdminPage());
        private void ProfileButton_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new ProfilePage());
    }
}