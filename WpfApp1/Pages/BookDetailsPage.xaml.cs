using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class BookDetailsPage : Page
    {
        private Books _currentBook;

        public BookDetailsPage(Books selectedBook)
        {
            InitializeComponent();
            _currentBook = selectedBook;

            LoadBookData();
            LoadReviews();
            CheckGuestAccess();
        }

        private void LoadBookData()
        {
            if (_currentBook == null) return;

            TitleTxt.Text = _currentBook.Title;
            DescTxt.Text = string.IsNullOrWhiteSpace(_currentBook.Description) ? "Описание отсутствует." : _currentBook.Description;
            ContentTxt.Text = string.IsNullOrWhiteSpace(_currentBook.Content) ? "Текст произведения пуст." : _currentBook.Content;

            if (_currentBook.Users != null)
            {
                AuthorTxt.Text = $"Автор: {_currentBook.Users.DisplayName}";
            }
            else
            {
                AuthorTxt.Text = $"ID Автора: {_currentBook.AuthorId}";
            }
            if (_currentBook != null)
            {
                if (_currentBook.Genres != null)
                {
                    var genreProp = _currentBook.Genres.GetType().GetProperties()
                        .FirstOrDefault(p => p.PropertyType == typeof(string));

                    if (genreProp != null)
                    {
                        var genreValue = genreProp.GetValue(_currentBook.Genres);
                        BookGenreTxt.Text = genreValue?.ToString() ?? "Без жанра";
                    }
                }
                else
                {
                    BookGenreTxt.Text = "Не указан";
                }
            }
        }

        private void LoadReviews()
        {
            if (Core.Context == null || _currentBook == null) return;

            var reviews = Core.Context.Reviews
                .Include("Users")
                .Where(r => r.BookId == _currentBook.BookId)
                .ToList();

            ReviewsList.ItemsSource = reviews;
        }

        private void CheckGuestAccess()
        {
            if (Core.CurrentUser == null)
            {
                AddReviewBlock.Visibility = Visibility.Collapsed;
                StatusSelectCombo.IsEnabled = false;
                ReportBookBtn.IsEnabled = false;
                ReportAuthorBtn.IsEnabled = false;
            }
        }

        private void SendReview_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null) return;

            if (string.IsNullOrWhiteSpace(ReviewTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите текст отзыва.");
                return;
            }

            int ratingValue = Convert.ToInt32((RatingCombo.SelectedItem as ComboBoxItem).Content);

            Reviews newReview = new Reviews()
            {
                BookId = _currentBook.BookId,
                UserId = Core.CurrentUser.UserId,
                ReviewText = ReviewTextBox.Text.Trim(),
                Rating = ratingValue
            };

            try
            {
                Core.Context.Reviews.Add(newReview);
                Core.Context.SaveChanges();

                MessageBox.Show("Отзыв успешно добавлен!");
                ReviewTextBox.Text = string.Empty;
                RatingCombo.SelectedIndex = 4;

                LoadReviews();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении отзыва: " + ex.Message);
            }
        }

        private void AddToList_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null)
            {
                MessageBox.Show("Авторизуйтесь, чтобы использовать списки чтения!");
                return;
            }

            string selectedStatus = (StatusSelectCombo.SelectedItem as ComboBoxItem).Content.ToString();

            var existingRecord = Core.Context.ReadingLists
                .FirstOrDefault(r => r.UserId == Core.CurrentUser.UserId && r.BookId == _currentBook.BookId);

            if (existingRecord != null)
            {
                existingRecord.Status = selectedStatus;
                MessageBox.Show($"Статус книги обновлен на: \"{selectedStatus}\"");
            }
            else
            {
                ReadingLists newRecord = new ReadingLists()
                {
                    UserId = Core.CurrentUser.UserId,
                    BookId = _currentBook.BookId,
                    Status = selectedStatus
                };
                Core.Context.ReadingLists.Add(newRecord);
                MessageBox.Show($"Книга добавлена в список \"{selectedStatus}\"");
            }

            Core.Context.SaveChanges();
        }

        private void ReportBook_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null) return;

            string reason = "Нарушение правил публикации / Содержимое книги";

            Complaints newComplaint = new Complaints()
            {
                UserId = Core.CurrentUser.UserId,
                TargetType = "Book",
                TargetId = _currentBook.BookId,
                Reason = reason
            };

            try
            {
                Core.Context.Complaints.Add(newComplaint);
                Core.Context.SaveChanges();
                MessageBox.Show("Жалоба на произведение успешно отправлена модераторам.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка отправки жалобы: " + ex.Message);
            }
        }

        private void ReportAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null) return;

            if (_currentBook.AuthorId == Core.CurrentUser.UserId)
            {
                MessageBox.Show("Вы не можете отправить жалобу на самого себя!");
                return;
            }

            string reason = "Жалоба на поведение автора / Нарушение авторских прав";

            Complaints newComplaint = new Complaints()
            {
                UserId = Core.CurrentUser.UserId,
                TargetType = "User",
                TargetId = _currentBook.AuthorId,
                Reason = reason
            };

            try
            {
                Core.Context.Complaints.Add(newComplaint);
                Core.Context.SaveChanges();
                MessageBox.Show("Жалоба на автора успешно отправлена модераторам.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка отправки жалобы: " + ex.Message);
            }
        }
        private void AdminElement_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (Core.CurrentUser == null)
                {
                    element.Visibility = Visibility.Collapsed;
                    return;
                }

                var prop = Core.CurrentUser.GetType().GetProperties()
                    .FirstOrDefault(p => (p.Name.Contains("Role") || p.Name.Contains("Status") || p.Name.Contains("Type") || p.Name.Contains("Position")) &&
                                         (p.PropertyType == typeof(string) || p.PropertyType == typeof(int) || p.PropertyType == typeof(int?)) &&
                                         !p.PropertyType.IsGenericType);

                if (prop == null)
                {
                    element.Visibility = Visibility.Collapsed;
                    return;
                }

                var value = prop.GetValue(Core.CurrentUser);
                if (value == null)
                {
                    element.Visibility = Visibility.Collapsed;
                    return;
                }

                string roleValue = value.ToString().Trim().ToLower();

                bool isAdmin = roleValue.Contains("админ") || roleValue.Contains("admin") || roleValue == "3";

                element.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void ReportReview_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null) return;
            int reviewId = (int)((Button)sender).Tag;

            Complaints newComplaint = new Complaints()
            {
                UserId = Core.CurrentUser.UserId,
                TargetType = "Review",
                TargetId = reviewId,
                Reason = "Жалоба на содержание отзыва"
            };

            Core.Context.Complaints.Add(newComplaint);
            Core.Context.SaveChanges();
            MessageBox.Show("Жалоба на отзыв отправлена.");
        }

        private void FreezeReview_Click(object sender, RoutedEventArgs e)
        {
            int reviewId = (int)((Button)sender).Tag;
            var review = Core.Context.Reviews.FirstOrDefault(r => r.ReviewId == reviewId);

            if (review != null)
            {
                Core.Context.Reviews.Remove(review);
                Core.Context.SaveChanges();
                MessageBox.Show("Отзыв успешно удален (заморожен) модератором.");
                LoadReviews();
            }
        }

        private void FreezeBook_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBook == null) return;

            var dbBook = Core.Context.Books.FirstOrDefault(b => b.BookId == _currentBook.BookId);
            if (dbBook != null)
            {
                dbBook.IsFrozen = true;
                Core.Context.SaveChanges();
                MessageBox.Show("Книга успешно заморожена и скрыта из общего каталога.");
                this.NavigationService.GoBack();
            }
        }
    }
}