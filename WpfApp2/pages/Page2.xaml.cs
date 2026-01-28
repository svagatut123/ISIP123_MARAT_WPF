using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp2.pages;

namespace WpfApp2.pages
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            LoadCart();
        }

        private void LoadCart()
        {
            ListCart.ItemsSource = Cart.Tovary;
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = Cart.Tovary.Sum(item => item.Summa);
            TextTotal.Text = total.ToString("C");
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button.DataContext as CartItem;

            if (item != null)
            {
                Cart.Tovary.Remove(item);
                ListCart.Items.Refresh();
                UpdateTotal();
            }
        }

        private void ContinueShopping_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1());
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Tovary.Count == 0)
            {
                MessageBox.Show("корзина пуста");
                return;
            }

            NavigationService.Navigate(new Page3());
        }
    }
}