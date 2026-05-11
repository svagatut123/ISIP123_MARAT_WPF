using System.Windows;
using WpfApp1.Pages;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CheckAccess();

            // открываем каталог по умолчанию
            MainFrame.Navigate(new CatalogPage());
        }

        // метод для проверки ролей и скрытия кнопок
        private void CheckAccess()
        {
            // если зашли как гость
            if (Core.CurrentUser == null)
            {
                ListsButton.Visibility = Visibility.Collapsed;
                ProfileButton.Visibility = Visibility.Collapsed;
                AuthorButton.Visibility = Visibility.Collapsed;
                AdminButton.Visibility = Visibility.Collapsed;
                return;
            }

            // показываем стандартные кнопки для вошедшего
            ListsButton.Visibility = Visibility.Visible;
            ProfileButton.Visibility = Visibility.Visible;

            // проверка на заморозку
            if (Core.CurrentUser.IsFrozen)
            {
                WarningButton.Visibility = Visibility.Visible;
            }
            else
            {
                WarningButton.Visibility = Visibility.Collapsed;
            }

            // ЛОГИКА РОЛЕЙ:
            // 3 — это Админ (по твоей БД)
            if (Core.CurrentUser.RoleId == 3)
            {
                AdminButton.Visibility = Visibility.Visible;
                AuthorButton.Visibility = Visibility.Collapsed; // админу обычно не нужна страница автора
            }
            // 2 — это Автор (проверь, какой ID у автора в твоей таблице Roles)
            else if (Core.CurrentUser.RoleId == 2)
            {
                AdminButton.Visibility = Visibility.Collapsed;
                AuthorButton.Visibility = Visibility.Visible;
            }
            // 1 — это Читатель
            else if (Core.CurrentUser.RoleId == 1)
            {
                AdminButton.Visibility = Visibility.Collapsed;
                AuthorButton.Visibility = Visibility.Collapsed;
            }
        }

        private void CatalogButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CatalogPage());
        }

        private void ListsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReadingListsPage());
        }

        private void AuthorButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AuthorPage());
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AdminPage());
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProfilePage());
        }
    }
}