using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1;

namespace WpfApp1.Pages
{
    public partial class ListsPage : Page
    {
        public ListsPage()
        {
            InitializeComponent();
            // Добавляем элементы в ComboBox
            CmbListStatus.Items.Add("Заброшено");
            CmbListStatus.Items.Add("В планах");
            CmbListStatus.Items.Add("Читаю");
            CmbListStatus.Items.Add("Прочитано");
            CmbListStatus.SelectedIndex = 2; // "Читаю" по умолчанию
        }

        private void CmbListStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Проверяем, что SelectedItem не null
                if (CmbListStatus.SelectedItem == null)
                    return;

                var status = CmbListStatus.SelectedItem.ToString();

                // Проверяем, что пользователь авторизован
                if (Core.CurrentUser == null)
                {
                    MessageBox.Show("Пользователь не авторизован");
                    return;
                }

                var items = Core.Context.ReadingLists
                    .Where(r => r.UserId == Core.CurrentUser.UserId && r.Status == status)
                    .Select(r => r.Books)
                    .ToList();

                GridMyBooks.ItemsSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки списков: " + ex.Message);
            }
        }
    }
}