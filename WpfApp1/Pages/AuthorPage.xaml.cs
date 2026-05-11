using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class AuthorPage : Page
    {
        public AuthorPage()
        {
            InitializeComponent();
            LoadAuthorBooks();
        }

        // загружаем книги текущего автора
        private void LoadAuthorBooks()
        {
            if (Core.CurrentUser == null) return;

            // получаем все книги автора
            var allAuthorBooks = Core.Context.Books
                .Where(b => b.AuthorId == Core.CurrentUser.UserId)
                .ToList();

            // разделяем на активные и замороженные
            ActiveBooksGrid.ItemsSource = allAuthorBooks.Where(b => b.IsFrozen == false).ToList();
            FrozenBooksGrid.ItemsSource = allAuthorBooks.Where(b => b.IsFrozen == true).ToList();
        }

        // переход на страницу добавления
        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddBookPage());
        }

        // кнопка редактирования
        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            int bookId = (int)((Button)sender).Tag;
            // здесь можно открыть ту же AddBookPage, но передать туда id книги для загрузки данных
            MessageBox.Show("переход к редактированию книги с id: " + bookId);
        }

        // кнопка подачи апелляции на заморозку книги
        private void Appeal_Click(object sender, RoutedEventArgs e)
        {
            int bookId = (int)((Button)sender).Tag;

            // создаем заявку на разморозку (согласно таблице UnfreezeRequests)
            UnfreezeRequests newAppeal = new UnfreezeRequests()
            {
                UserId = Core.CurrentUser.UserId,
                TargetId = bookId,
                TargetType = "Книга",
                Reason = "прошу пересмотреть решение, книга не нарушает правила",
                RequestDate = DateTime.Now,
                Status = "На рассмотрении"
            };

            Core.Context.UnfreezeRequests.Add(newAppeal);
            Core.Context.SaveChanges();

            MessageBox.Show("заявка на снятие заморозки отправлена администрации");
        }
    }
}