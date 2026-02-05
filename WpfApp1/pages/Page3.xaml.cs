using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.pages;

namespace WpfApp1
{
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
            LoadOrderItems();
            LoadSavedData();
        }

        private void LoadOrderItems()
        {
            ListOrderItems.ItemsSource = Cart.Tovary;
            decimal total = Cart.Tovary.Sum(item => item.Summa);
            TextOrderTotal.Text = total.ToString("C");
        }

        private void LoadSavedData()
        {
            if (Cart.OrderData != null)
            {
                TextFIO.Text = Cart.OrderData.FIO;
                TextEmail.Text = Cart.OrderData.Email;
                TextAddress.Text = Cart.OrderData.AdresDostavki;
            }
        }

        private void BackToCart_Click(object sender, RoutedEventArgs e)
        {
            SaveFormData();
            NavigationService.Navigate(new Page2());
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextFIO.Text))
            {
                MessageBox.Show("введите ФИО");
                return;
            }

            if (string.IsNullOrWhiteSpace(TextEmail.Text))
            {
                MessageBox.Show("введите email");
                return;
            }

            if (string.IsNullOrWhiteSpace(TextAddress.Text))
            {
                MessageBox.Show("введите адрес доставки");
                return;
            }

            if (Cart.Tovary.Count == 0)
            {
                MessageBox.Show("корзина пуста");
                return;
            }

            try
            {
                var order = new Order
                {
                    FIO = TextFIO.Text,
                    Email = TextEmail.Text,
                    AdresDostavki = TextAddress.Text,
                    ObshayaSumma = Cart.Tovary.Sum(item => item.Summa)
                };

                Core.context.Orders.Add(order);
                Core.context.SaveChanges();

                foreach (var item in Cart.Tovary)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        ProductId = item.ProductId,
                        Kolichestvo = item.Kolichestvo
                    };
                    Core.context.OrderItems.Add(orderItem);
                }

                Core.context.SaveChanges();

                Cart.Tovary.Clear();
                Cart.OrderData = new OrderFormData();

                MessageBox.Show($"заказ №{order.OrderId} оформлен!");
                NavigationService.Navigate(new Page1());
            }
            catch (Exception ex)
            {
                MessageBox.Show("ошибка: " + ex.Message);
            }
        }

        private void SaveFormData()
        {
            Cart.OrderData.FIO = TextFIO.Text;
            Cart.OrderData.Email = TextEmail.Text;
            Cart.OrderData.AdresDostavki = TextAddress.Text;
        }

        private void TextFIO_TextChanged(object sender, TextChangedEventArgs e)
        {
            Cart.OrderData.FIO = TextFIO.Text;
        }

        private void TextEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            Cart.OrderData.Email = TextEmail.Text;
        }

        private void TextAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            Cart.OrderData.AdresDostavki = TextAddress.Text;
        }
    }
}