using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1.Pages
{
    public partial class CatalogPage : Page
    {
        public CatalogPage()
        {
            InitializeComponent();
            LoadGenres();

            if (SortCombo != null) SortCombo.SelectedIndex = 0;
            ApplyFilter();
        }

        private void LoadGenres()
        {
            if (Core.Context == null || GenreFilterCombo == null) return;

            try
            {
                var rawGenres = Core.Context.Genres.ToList();

                var comboItems = rawGenres.Select(g => new
                {
                    Id = g.GenreId,
                    Text = g.GetType().GetProperties()
                            .FirstOrDefault(p => p.PropertyType == typeof(string))?
                            .GetValue(g)?.ToString() ?? "Жанр"
                }).ToList();

                comboItems.Insert(0, new { Id = 0, Text = "Все жанры" });

                GenreFilterCombo.SelectedValuePath = "Id";
                GenreFilterCombo.DisplayMemberPath = "Text";
                GenreFilterCombo.ItemsSource = comboItems;
                GenreFilterCombo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка инициализации жанров: " + ex.Message);
            }
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();
        private void FilterChanged(object sender, TextChangedEventArgs e) => ApplyFilter();

        private void SearchButton_Click(object sender, RoutedEventArgs e) => ApplyFilter();

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox != null) SearchTextBox.Text = string.Empty;
            if (GenreFilterCombo != null) GenreFilterCombo.SelectedIndex = 0;
            if (SortCombo != null) SortCombo.SelectedIndex = 0;
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (Core.Context == null || BooksListView == null || SearchTextBox == null || SortCombo == null)
                return;

            var query = Core.Context.Books.Include("Reviews").Include("Users").Where(b => b.IsFrozen != true).ToList();

            string searchText = SearchTextBox.Text.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(b => b.Title != null && b.Title.ToLower().Contains(searchText)).ToList();
            }

            if (GenreFilterCombo != null && GenreFilterCombo.SelectedValue != null)
            {
                int selectedGenreId = (int)GenreFilterCombo.SelectedValue;
                if (selectedGenreId != 0)
                {
                    query = query.Where(b => b.GenreId == selectedGenreId).ToList();
                }
            }

            if (SortCombo.SelectedIndex == 0)
            {
                query = query.OrderBy(b => b.Title).ToList();
            }
            else if (SortCombo.SelectedIndex == 1) 
            {
                query = query.OrderByDescending(b => b.Reviews.Count).ToList();
            }

            BooksListView.ItemsSource = query;
        }

        private void BooksListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BooksListView.SelectedItem is Books selectedBook)
            {
                var detailedBook = Core.Context.Books
            .Include("Genres")
            .Include("Users")
            .Include("Reviews")
            .FirstOrDefault(b => b.BookId == selectedBook.BookId);
                this.NavigationService.Navigate(new BookDetailsPage(selectedBook));
            }
        }
    }
}