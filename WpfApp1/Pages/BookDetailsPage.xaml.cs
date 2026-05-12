using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class BookDetailsPage : Page
    {
        private Books _currentBook;

        public BookDetailsPage(Books book)
        {
            InitializeComponent();
            _currentBook = book;
            DataContext = _currentBook;

            TitleTxt.Text = book.Title;
            AuthorTxt.Text = "автор: " + book.Users.DisplayName;
            DescTxt.Text = book.Description;
            ContentTxt.Text = book.Content;

            LoadReviews();
        }

        private void LoadReviews()
        {
            ReviewsList.ItemsSource = Core.Context.Reviews.Where(r => r.BookId == _currentBook.BookId).ToList();
        }

        private void AddToList_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null)
            {
                MessageBox.Show("пожалуйста, войдите в систему, чтобы сохранять книги");
                return;
            }

            string selectedStatus = (StatusSelectCombo.SelectedItem as ComboBoxItem).Content.ToString();

            var existingRecord = Core.Context.ReadingLists.FirstOrDefault(rl =>
                rl.UserId == Core.CurrentUser.UserId &&
                rl.BookId == _currentBook.BookId);

            if (existingRecord != null)
            {
                existingRecord.Status = selectedStatus;
                MessageBox.Show("статус книги обновлен на: " + selectedStatus);
            }
            else
            {
                ReadingLists newItem = new ReadingLists()
                {
                    UserId = Core.CurrentUser.UserId,
                    BookId = _currentBook.BookId,
                    Status = selectedStatus
                };
                Core.Context.ReadingLists.Add(newItem);
                MessageBox.Show("книга добавлена в список: " + selectedStatus);
            }

            Core.Context.SaveChanges();
        }

        private void SendReview_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null) { MessageBox.Show("войдите, чтобы оставить отзыв"); return; }
            if (string.IsNullOrWhiteSpace(ReviewTextBox.Text)) { MessageBox.Show("напишите текст отзыва"); return; }

            int userRating = int.Parse((RatingCombo.SelectedItem as ComboBoxItem).Content.ToString());
            int dbRating = userRating * 2;

            Reviews newReview = new Reviews()
            {
                UserId = Core.CurrentUser.UserId,
                BookId = _currentBook.BookId,
                ReviewText = ReviewTextBox.Text,
                Rating = dbRating,
                ReviewDate = DateTime.Now 
            };

            Core.Context.Reviews.Add(newReview);
            Core.Context.SaveChanges();

            ReviewTextBox.Text = ""; 
            LoadReviews(); 
            MessageBox.Show("спасибо за отзыв!");
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null) return;
            if (ComplaintReasonCombo.SelectedItem == null) { MessageBox.Show("выберите причину"); return; }

            string reason = (ComplaintReasonCombo.SelectedItem as ComboBoxItem).Content.ToString();

            Complaints newComplaint = new Complaints()
            {
                UserId = Core.CurrentUser.UserId,
                TargetId = _currentBook.BookId,
                TargetType = "Книга",
                Reason = reason,
                ComplaintDate = DateTime.Now
            };

            Core.Context.Complaints.Add(newComplaint);
            Core.Context.SaveChanges();
            MessageBox.Show("жалоба на книгу отправлена");
        }
    }
}