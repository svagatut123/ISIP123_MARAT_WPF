using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2.pages
{
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
            LoadOrderItems();
        }

        private void LoadOrderItems()
        {
            ListOrderItems.ItemsSource = Cart.Tovary;
            decimal total = Cart.Tovary.Sum(item => item.Summa);
            TextOrderTotal.Text = total.ToString("C");
        }

        private void BackToCart_Click(object sender, RoutedEventArgs e)
        {
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
                using (var db = new MaratPrac13Entities1())
                {
                    // Создаем заказ
                    var order = new Order
                    {
                        FIO = TextFIO.Text,
                        Email = TextEmail.Text,
                        AdresDostavki = TextAddress.Text,
                        ObshayaSumma = Cart.Tovary.Sum(item => item.Summa)
                    };

                    db.Orders.Add(order);
                    db.SaveChanges();

                    // Добавляем товары заказа
                    foreach (var item in Cart.Tovary)
                    {
                        var orderItem = new OrderItem
                        {
                            OrderId = order.OrderId,
                            ProductId = item.ProductId,
                            Kolichestvo = item.Kolichestvo
                        };
                        db.OrderItems.Add(orderItem);
                    }

                    db.SaveChanges();

                    // Очищаем корзину
                    Cart.Tovary.Clear();

                    MessageBox.Show($"заказ №{order.OrderId} оформлен!");

                    // Возвращаемся на страницу товаров
                    NavigationService.Navigate(new Page1());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ошибка: " + ex.Message);
            }
        }
    }
}