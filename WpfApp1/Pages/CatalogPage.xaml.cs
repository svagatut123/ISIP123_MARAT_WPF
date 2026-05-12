using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1.Pages
{
    public partial class CatalogPage : Page
    {
        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Core.Context == null) return;

            var books = Core.Context.Books.Where(b => b.IsFrozen == false).ToList();

            if (SearchTextBox != null && !string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                string search = SearchTextBox.Text.ToLower();
                books = books.Where(b => b.Title.ToLower().Contains(search) ||
                                         (b.Users != null && b.Users.DisplayName.ToLower().Contains(search))).ToList();
            }

            if (GenreFilterCombo != null && GenreFilterCombo.SelectedItem != null)
            {
                var selectedGenre = GenreFilterCombo.SelectedItem as Genres;
                books = books.Where(b => b.Genres.Any(g => g.GenreId == selectedGenre.GenreId)).ToList();
            }

            if (SortCombo != null)
            {
                if (SortCombo.SelectedIndex == 0)
                {
                    books = books.OrderBy(b => b.Title).ToList();
                }
                else if (SortCombo.SelectedIndex == 1) 
                {
                    books = books.OrderByDescending(b => b.BookId).ToList();
                }
            }

            if (BooksListView != null)
            {
                BooksListView.ItemsSource = books;
            }
        }

        public CatalogPage()
        {
            InitializeComponent();

            var genresList = Core.Context.Genres.ToList();

            GenreFilterCombo.ItemsSource = genresList;

            LoadBooks();
        }

        private void LoadBooks()
        {
            var books = Core.Context.Books.Where(b => b.IsFrozen == false).ToList();
            BooksListView.ItemsSource = books;
        }
        private void BooksListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedBook = BooksListView.SelectedItem as Books;
            if (selectedBook != null)
            {
                this.NavigationService.Navigate(new BookDetailsPage(selectedBook));
            }
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            var filteredBooks = Core.Context.Books
                .Where(b => b.IsFrozen == false && b.Title.ToLower().Contains(searchText))
                .ToList();

            BooksListView.ItemsSource = filteredBooks;
        }
    }
}