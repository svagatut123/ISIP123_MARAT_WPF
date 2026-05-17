using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class AuthorPage : Page
    {
        private Books _editingBook;

        public AuthorPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            if (Core.Context == null || Core.CurrentUser == null) return;

            var allAuthorBooks = Core.Context.Books.Include("Genres").Where(b => b.AuthorId == Core.CurrentUser.UserId).ToList();

            var activeBooks = allAuthorBooks.Where(b => b.IsFrozen != true).Select(b => new {
                b.BookId,
                b.Title,
                b.Content,
                b.GenreId,
                GenresText = b.Genres != null ? b.GetType().GetProperty("Genres")?.GetValue(b)?.GetType().GetProperties().FirstOrDefault(p => p.PropertyType == typeof(string))?.GetValue(b.Genres)?.ToString() : "Без жанра"
            }).ToList();

            ActiveBooksGrid.ItemsSource = activeBooks;
            FrozenBooksGrid.ItemsSource = allAuthorBooks.Where(b => b.IsFrozen == true).ToList();

            var rawGenres = Core.Context.Genres.ToList();
            var genreItems = rawGenres.Select(g => new {
                Id = g.GenreId,
                Text = g.GetType().GetProperties().FirstOrDefault(p => p.PropertyType == typeof(string))?.GetValue(g)?.ToString() ?? "Жанр"
            }).ToList();

            EditGenreCombo.SelectedValuePath = "Id";
            EditGenreCombo.DisplayMemberPath = "Text";
            EditGenreCombo.ItemsSource = genreItems;
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            int bookId = (int)((Button)sender).Tag;
            _editingBook = Core.Context.Books.FirstOrDefault(b => b.BookId == bookId);

            if (_editingBook != null)
            {
                PlaceholderTxt.Visibility = Visibility.Collapsed;
                EditBlock.Visibility = Visibility.Visible;

                EditTitleBox.Text = _editingBook.Title;
                EditContentBox.Text = _editingBook.Content;
                EditGenreCombo.SelectedValue = _editingBook.GenreId;
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_editingBook == null) return;

            if (string.IsNullOrWhiteSpace(EditTitleBox.Text))
            {
                MessageBox.Show("Название произведения не может быть пустым!");
                return;
            }

            _editingBook.Title = EditTitleBox.Text.Trim();
            _editingBook.Content = EditContentBox.Text.Trim();
            if (EditGenreCombo.SelectedValue != null) _editingBook.GenreId = (int)EditGenreCombo.SelectedValue;

            Core.Context.SaveChanges();
            MessageBox.Show("Изменения сохранены.");
            EditBlock.Visibility = Visibility.Collapsed;
            PlaceholderTxt.Visibility = Visibility.Visible;
            LoadData();
        }

        private void Appeal_Click(object sender, RoutedEventArgs e)
        {
            int bookId = (int)((Button)sender).Tag;
            var existingAppeal = Core.Context.Complaints.FirstOrDefault(c => c.TargetType == "Appeal" && c.TargetId == bookId);

            if (existingAppeal != null)
            {
                MessageBox.Show("Запрос уже находится на рассмотрении.");
                return;
            }

            Complaints appeal = new Complaints()
            {
                UserId = Core.CurrentUser.UserId,
                TargetType = "Appeal",
                TargetId = bookId,
                Reason = "Автор оспаривает блокировку книги."
            };

            Core.Context.Complaints.Add(appeal);
            Core.Context.SaveChanges();
            MessageBox.Show("Заявка на апелляцию отправлена.");
        }
    }
}