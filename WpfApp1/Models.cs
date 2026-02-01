using System.Collections.Generic;
using System.ComponentModel;

namespace WpfApp1
{
    // Класс для товара
    public class Product
    {
        public int ProductId { get; set; }
        public string NazvanieTovara { get; set; }
        public decimal Cena { get; set; }
        public string IzobrazhenieTovara { get; set; }
    }

    // Класс для заказа
    public class Order
    {
        public int OrderId { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public string AdresDostavki { get; set; }
        public decimal ObshayaSumma { get; set; }
    }

    // Класс для товара в заказе
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Kolichestvo { get; set; }
    }

    // Класс для товара в корзине
    public class CartItems : INotifyPropertyChanged
    {
        private int _kolichestvo;

        public int ProductId { get; set; }
        public string NazvanieTovara { get; set; }
        public decimal Cena { get; set; }
        public string IzobrazhenieTovara { get; set; }

        public int Kolichestvo
        {
            get => _kolichestvo;
            set
            {
                _kolichestvo = value;
                OnPropertyChanged(nameof(Kolichestvo));
                OnPropertyChanged(nameof(Summa));
            }
        }

        public decimal Summa => Cena * Kolichestvo;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // Статический класс для корзины
    public static class Cart
    {
        public static List<CartItems> Tovary { get; set; } = new List<CartItems>();
    }
}