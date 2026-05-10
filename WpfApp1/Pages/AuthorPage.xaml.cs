using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class AuthorPage : Page
    {
        public AuthorPage()
        {
            InitializeComponent();
            var books = Core.Context.Books.Where(b => b.AuthorId == Core.CurrentUser.UserId).ToList();
            GridMyBooks.ItemsSource = books;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Открывается страница добавления книги (заглушка)");
        }
    }
}