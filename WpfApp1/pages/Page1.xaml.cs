using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1;
using WpfApp1.pages;

namespace WpfApp1.pages
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
            try
            {
                using (var db = new MaratPrac13Entities1())
                {
                    var products = db.Products.ToList();
                    ListProducts.ItemsSource = products;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ошибка: " + ex.Message);
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button.DataContext as Product;

            if (product != null)
            {
                var cartItem = Cart.Tovary.FirstOrDefault(t => t.ProductId == product.ProductId);

                if (cartItem != null)
                {
                    cartItem.Kolichestvo++;
                }
                else
                {
                    Cart.Tovary.Add(new CartItem
                    {
                        ProductId = product.ProductId,
                        NazvanieTovara = product.NazvanieTovara,
                        Cena = product.Cena,
                        Kolichestvo = 1
                    });
                }

                MessageBox.Show("товар добавлен в корзину");
            }
        }

        private void GoToCart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }
    }
}