using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new Page1();
        }

        public void ShowProducts_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page1();
        }

        public void ShowCart_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page2();
        }

        public void ShowCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Items.Count == 0)
            {
                MessageBox.Show("Корзина пуста. Добавьте товары перед оформлением заказа.", "Пустая корзина", MessageBoxButton.OK, MessageBoxImage.Warning);
                MainFrame.Content = new Page2();
                return;
            }
            MainFrame.Content = new Page3();
        }
    }
}