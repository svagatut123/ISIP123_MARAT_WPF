using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Pages;
using WpfApp1;

namespace WpfApp1

{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ApplyRoleVisibility();

            // Безопасный переход на первую страницу
            SafeNavigate(new CatalogPage());
        }

        private void ApplyRoleVisibility()
        {
            try
            {
                if (Core.CurrentUser == null || Core.CurrentUser.Roles == null)
                    return;

                var roleName = Core.CurrentUser.Roles.RoleName;

                if (roleName == "Администратор")
                    BtnAdmin.Visibility = Visibility.Visible;

                if (roleName == "Автор")
                    BtnAuthor.Visibility = Visibility.Visible;

                if (Core.CurrentUser.IsFrozen == true)
                    MessageBox.Show("⚠️ Ваш аккаунт заморожен. Доступ ограничен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке профиля: " + ex.Message);
            }
        }

        private void SafeNavigate(Page page)
        {
            try
            {
                MainFrame.Navigate(page);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка открытия страницы:\n" + ex.Message);
            }
        }

        private void BtnCatalog_Click(object sender, RoutedEventArgs e) => SafeNavigate(new CatalogPage());
        private void BtnLists_Click(object sender, RoutedEventArgs e) => SafeNavigate(new ListsPage());
        private void BtnAdmin_Click(object sender, RoutedEventArgs e) => SafeNavigate(new AdminPage());
        private void BtnAuthor_Click(object sender, RoutedEventArgs e) => SafeNavigate(new AuthorPage());
        private void BtnProfile_Click(object sender, RoutedEventArgs e) => SafeNavigate(new ProfilePage());
    }
}