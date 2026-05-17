using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class AddBookPage : Page
    {
        public AddBookPage()
        {
            InitializeComponent();
            if (Core.Context != null)
            {
                GenreCombo.ItemsSource = Core.Context.Genres.ToList();
            }
        }

        private void SaveBook_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Введите название книги!");
                return;
            }

            if (GenreCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите жанр книги из списка!");
                return;
            }

            var selectedGenre = GenreCombo.SelectedItem as Genres;

            Books newBook = new Books()
            {
                Title = TitleBox.Text,
                Description = DescBox.Text,
                Content = ContentBox.Text,
                AuthorId = Core.CurrentUser.UserId,
                GenreId = selectedGenre.GenreId,
                IsFrozen = false
            };

            Core.Context.Books.Add(newBook);
            Core.Context.SaveChanges();

            MessageBox.Show("Книга успешно сохранена и опубликована!");
            this.NavigationService.GoBack();
        }
    }
}