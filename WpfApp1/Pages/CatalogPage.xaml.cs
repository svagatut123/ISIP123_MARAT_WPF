using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1.Pages
{
    public partial class CatalogPage : Page
    {
        // вызываем этот метод при любом изменении фильтров
        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            // если контекст еще не создан, выходим (защита от ошибок при инициализации)
            if (Core.Context == null) return;

            // берем базовый список книг (не замороженных)
            var books = Core.Context.Books.Where(b => b.IsFrozen == false).ToList();

            // 1. фильтр по названию или автору (из текстбокса)
            if (SearchTextBox != null && !string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                string search = SearchTextBox.Text.ToLower();
                books = books.Where(b => b.Title.ToLower().Contains(search) ||
                                         (b.Users != null && b.Users.DisplayName.ToLower().Contains(search))).ToList();
            }

            // 2. фильтр по жанру
            if (GenreFilterCombo != null && GenreFilterCombo.SelectedItem != null)
            {
                var selectedGenre = GenreFilterCombo.SelectedItem as Genres;
                // учитываем связь многие-ко-многим через .Any()
                books = books.Where(b => b.Genres.Any(g => g.GenreId == selectedGenre.GenreId)).ToList();
            }

            // 3. сортировка
            if (SortCombo != null)
            {
                if (SortCombo.SelectedIndex == 0) // по названию
                {
                    books = books.OrderBy(b => b.Title).ToList();
                }
                else if (SortCombo.SelectedIndex == 1) // по рейтингу (если нет поля в БД, можно по ID для теста)
                {
                    books = books.OrderByDescending(b => b.BookId).ToList();
                }
            }

            // обновляем список на экране
            if (BooksListView != null)
            {
                BooksListView.ItemsSource = books;
            }
        }

        // в конструкторе страницы не забудь загрузить жанры
        public CatalogPage()
        {
            InitializeComponent();

            // загружаем жанры из БД
            var genresList = Core.Context.Genres.ToList();

            // привязываем список к комбобоксу
            GenreFilterCombo.ItemsSource = genresList;

            // загружаем книги при открытии страницы
            LoadBooks();
        }

        // загружаем не замороженные книги
        private void LoadBooks()
        {
            var books = Core.Context.Books.Where(b => b.IsFrozen == false).ToList();
            BooksListView.ItemsSource = books;
        }
        private void BooksListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // проверяем, что нажали именно на книгу, а не просто на пустое место в списке
            var selectedBook = BooksListView.SelectedItem as Books;
            if (selectedBook != null)
            {
                // открываем страницу книги и передаем туда объект выбранной книги
                this.NavigationService.Navigate(new BookDetailsPage(selectedBook));
            }
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            // ищем по названию
            var filteredBooks = Core.Context.Books
                .Where(b => b.IsFrozen == false && b.Title.ToLower().Contains(searchText))
                .ToList();

            BooksListView.ItemsSource = filteredBooks;
        }
    }
}