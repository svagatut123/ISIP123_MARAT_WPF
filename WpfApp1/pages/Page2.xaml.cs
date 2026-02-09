using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            UpdateCartDisplay();
        }

        private void UpdateCartDisplay()
        {
            CartItemsList.ItemsSource = null;
            CartItemsList.ItemsSource = Cart.Items;
            TotalText.Text = $"Итого: {Cart.Total:#,##0.00} ₽";

            if (Cart.Items.Count == 0)
            {
                TotalText.Text = "Корзина пуста";
            }
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Items.Count == 0)
            {
                MessageBox.Show("Корзина пуста", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ShowCheckout_Click(this, new RoutedEventArgs());
            }
        }
    }
}