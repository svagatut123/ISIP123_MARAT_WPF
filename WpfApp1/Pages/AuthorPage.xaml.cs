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

        private void LoadAuthorBooks()
        {
            if (Core.CurrentUser == null) return;

            var allAuthorBooks = Core.Context.Books
                .Where(b => b.AuthorId == Core.CurrentUser.UserId)
                .ToList();

            ActiveBooksGrid.ItemsSource = allAuthorBooks.Where(b => b.IsFrozen == false).ToList();
            FrozenBooksGrid.ItemsSource = allAuthorBooks.Where(b => b.IsFrozen == true).ToList();
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddBookPage());
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            int bookId = (int)((Button)sender).Tag;
            MessageBox.Show("переход к редактированию книги с id: " + bookId);
        }

        private void Appeal_Click(object sender, RoutedEventArgs e)
        {
            int bookId = (int)((Button)sender).Tag;

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