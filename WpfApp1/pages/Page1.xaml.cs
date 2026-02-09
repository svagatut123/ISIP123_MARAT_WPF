using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace WpfApp1
{
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            var products = Core.Context.Products.ToList();
            ListProducts.ItemsSource = products;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var product = (Products)button.Tag; 
            Cart.AddProduct(product);
            MessageBox.Show($"товар \"{product.NazvanieTovara}\" добавлен в корзину",
                          "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}