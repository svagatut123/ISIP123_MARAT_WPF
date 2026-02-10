using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public static user CurrentUser = null;

        public MainWindow()
        {
            InitializeComponent();
            ShowMoviesPage();
            UpdateAccountButton();
        }

        private void ShowMoviesPage()
        {
            var page = new MoviesPage();
            MainFrame.Navigate(page);
        }

        private void UpdateAccountButton()
        {
            if (CurrentUser == null)
            {
                AccountButton.Content = "войти";
            }
            else
            {
                AccountButton.Content = "профиль (" + CurrentUser.first_name + ")";
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MainFrame.Content is MoviesPage)
            {
                var page = (MoviesPage)MainFrame.Content;
                page.ApplyFilter(SearchBox.Text);
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainFrame.Content is MoviesPage && SortComboBox.SelectedItem != null)
            {
                var page = (MoviesPage)MainFrame.Content;
                var item = (ComboBoxItem)SortComboBox.SelectedItem;
                page.ApplySort(item.Content.ToString());
            }
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null)
            {
                var page = new LoginPage();
                MainFrame.Navigate(page);
            }
            else
            {
                var page = new ProfilePage();
                MainFrame.Navigate(page);
            }
        }

        public void RefreshAccountButton()
        {
            UpdateAccountButton();
        }
    }
}