using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            OrderItemsList.ItemsSource = Cart.Items;
            TotalOrderText.Text = $"Итого к оплате: {Cart.Total:#,##0.00} ₽";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ShowCart_Click(this, new RoutedEventArgs());
            }
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            string fio = FioBox.Text.Trim();
            string email = EmailBox.Text.Trim();
            string address = AddressBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(fio))
            {
                MessageBox.Show("Введите ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Введите корректный email", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Введите адрес доставки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var order = new Orders // Orders с "s"!
                {
                    FIO = fio,
                    Email = email,
                    AdresDostavki = address,
                    ObshayaSumma = Cart.Total
                };
                Core.Context.Orders.Add(order);
                Core.Context.SaveChanges();

                foreach (var item in Cart.Items)
                {
                    Core.Context.OrderItems.Add(new OrderItems // OrderItems с "s"!
                    {
                        OrderId = order.OrderId,
                        ProductId = item.Product.ProductId,
                        Kolichestvo = item.Quantity
                    });
                }
                Core.Context.SaveChanges();

                Cart.Clear();
                MessageBox.Show($"Заказ №{order.OrderId} успешно оформлен!\nСпасибо за покупку!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                // Возвращаемся на страницу товаров
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.ShowProducts_Click(this, new RoutedEventArgs());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заказа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}