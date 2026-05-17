using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class ReadingListsPage : Page
    {
        public ReadingListsPage()
        {
            InitializeComponent();
            LoadGenres();
            ApplyFilter();
        }

        private void LoadGenres()
        {
            if (Core.Context == null || GenreCombo == null) return;

            try
            {
                var rawGenres = Core.Context.Genres.ToList();
                var comboItems = rawGenres.Select(g => new {
                    Id = g.GenreId,
                    Text = g.GetType().GetProperties().FirstOrDefault(p => p.PropertyType == typeof(string))?.GetValue(g)?.ToString() ?? "Жанр"
                }).ToList();

                comboItems.Insert(0, new { Id = 0, Text = "Все жанры" });

                GenreCombo.SelectedValuePath = "Id";
                GenreCombo.DisplayMemberPath = "Text";
                GenreCombo.ItemsSource = comboItems;
                GenreCombo.SelectedIndex = 0;
            }
            catch { }
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();
        private void FilterChanged(object sender, TextChangedEventArgs e) => ApplyFilter();

        private void ApplyFilter()
        {
            if (Core.Context == null || ListsDataGrid == null || Core.CurrentUser == null) return;

            var query = Core.Context.ReadingLists
                .Include("Books")
                .Include("Books.Users")
                .Include("Books.Reviews")
                .Where(r => r.UserId == Core.CurrentUser.UserId).ToList();

            string search = SearchBox.Text.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r => (r.Books?.Title != null && r.Books.Title.ToLower().Contains(search)) ||
                                         (r.Books?.Users?.DisplayName != null && r.Books.Users.DisplayName.ToLower().Contains(search))).ToList();
            }

            if (GenreCombo.SelectedValue != null && (int)GenreCombo.SelectedValue != 0)
            {
                int currentGenreId = (int)GenreCombo.SelectedValue;
                query = query.Where(r => r.Books?.GenreId == currentGenreId).ToList();
            }

            if (SortCombo.SelectedIndex == 0)
            {
                query = query.OrderBy(r => r.Books?.Title).ToList();
            }
            else if (SortCombo.SelectedIndex == 1) 
            {
                query = query.OrderBy(r => r.Books?.Users?.DisplayName).ToList();
            }
            else if (SortCombo.SelectedIndex == 2) 
            {
                query = query.OrderByDescending(r => r.Books?.Reviews?.Count ?? 0).ToList();
            }

            ListsDataGrid.ItemsSource = query;
        }
    }
}