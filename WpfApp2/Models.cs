using System.Collections.Generic;
using System.ComponentModel;

namespace WpfApp2
{
    public class Product
    {
        public int ProductId { get; set; }
        public string NazvanieTovara { get; set; }
        public decimal Cena { get; set; }
        public string IzobrazhenieTovara { get; set; }
    }

    public class CartItem : INotifyPropertyChanged
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

    public static class Cart
    {
        public static List<CartItem> Tovary { get; set; } = new List<CartItem>();
    }
}