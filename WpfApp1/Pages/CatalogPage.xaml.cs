using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Pages;
using WpfApp1;

namespace WpfApp1.Pages
{
    public partial class CatalogPage : Page
    {
        public CatalogPage()
        {
            InitializeComponent();
            LoadGenres();
            LoadBooks();
        }

        private void LoadGenres()
        {
            try
            {
                var genres = Core.Context.Genres.ToList();
                CmbGenre.ItemsSource = genres;
                CmbGenre.DisplayMemberPath = "GenreName";
                CmbGenre.SelectedValuePath = "GenreId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки жанров: " + ex.Message);
            }
        }

        private void LoadBooks()
        {
            try
            {
                var books = Core.Context.Books.ToList();
                int? genreFilter = CmbGenre.SelectedValue as int?;
                string search = TxtSearch.Text.ToLower();

                var filtered = books.AsQueryable();

                // Поиск по названию или автору
                if (!string.IsNullOrEmpty(search))
                {
                    filtered = filtered.Where(b =>
                        b.Title.ToLower().Contains(search) ||
                        b.Users.DisplayName.ToLower().Contains(search));
                }

                // Фильтрация по жанру
                if (genreFilter.HasValue)
                {
                    filtered = filtered.Where(b =>
                        Core.Context.BookGenres.Any(bg =>
                            bg.BookId == b.BookId && bg.GenreId == genreFilter.Value));
                }

                // Сортировка
                if (CmbSort.SelectedIndex == 1)
                {
                    // Сортировка по среднему рейтингу
                    filtered = filtered.OrderByDescending(b =>
                        Core.Context.Reviews
                            .Where(r => r.BookId == b.BookId)
                            .DefaultIfEmpty()
                            .Average(r => (int?)r.Rating) ?? 0);
                }
                else
                {
                    // Сортировка по названию
                    filtered = filtered.OrderBy(b => b.Title);
                }

                GridBooks.ItemsSource = filtered.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки книг: " + ex.Message);
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadBooks();
        }

        private void GridBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GridBooks.SelectedItem is Books book)
            {
                NavigationService.Navigate(new BookPage(book));
            }
        }
    }
}