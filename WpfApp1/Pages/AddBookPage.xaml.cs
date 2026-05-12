using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class AddBookPage : Page
    {
        public AddBookPage()
        {
            InitializeComponent();
        }

        private void SaveBook_Click(object sender, RoutedEventArgs e)
        {
            Books newBook = new Books()
            {
                Title = TitleBox.Text,
                Description = DescBox.Text,
                Content = ContentBox.Text,
                AuthorId = Core.CurrentUser.UserId,
                IsFrozen = false
            };

            Core.Context.Books.Add(newBook);
            Core.Context.SaveChanges();

            MessageBox.Show("книга успешно добавлена!");
            this.NavigationService.GoBack();
        }
    }
}