using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WpfApp1.Models
{
    public class CarConfig : INotifyPropertyChanged
    {
        // Модель
        private CarModel _selectedModel;
        public CarModel SelectedModel
        {
            get => _selectedModel;
            set { _selectedModel = value; OnPropertyChanged(); }
        }

        // Двигатель
        private Engine _selectedEngine;
        public Engine SelectedEngine
        {
            get => _selectedEngine;
            set { _selectedEngine = value; OnPropertyChanged(); }
        }

        // Цвет
        private CarColor _selectedColor;
        public CarColor SelectedColor
        {
            get => _selectedColor;
            set { _selectedColor = value; OnPropertyChanged(); }
        }

        // Опции
        public List<CarOption> Options { get; set; } = new List<CarOption>();

        // Кредит
        private decimal _initialPaymentPercent = 20;
        public decimal InitialPaymentPercent
        {
            get => _initialPaymentPercent;
            set { _initialPaymentPercent = value; OnPropertyChanged(); }
        }

        private int _loanTermMonths = 24;
        public int LoanTermMonths
        {
            get => _loanTermMonths;
            set { _loanTermMonths = value; OnPropertyChanged(); }
        }

        // Контакты
        private string _clientName;
        public string ClientName
        {
            get => _clientName;
            set { _clientName = value; OnPropertyChanged(); }
        }

        private string _clientPhone;
        public string ClientPhone
        {
            get => _clientPhone;
            set { _clientPhone = value; OnPropertyChanged(); }
        }

        private string _clientEmail;
        public string ClientEmail
        {
            get => _clientEmail;
            set { _clientEmail = value; OnPropertyChanged(); }
        }

        // Расчеты
        public decimal BasePrice => SelectedModel?.BasePrice ?? 0;
        public decimal EnginePrice => SelectedEngine?.Price ?? 0;
        public decimal ColorPrice => SelectedColor?.Price ?? 0;
        public decimal OptionsPrice => Options.Where(o => o.IsSelected).Sum(o => o.Price);
        public decimal TotalPrice => BasePrice + EnginePrice + ColorPrice + OptionsPrice;

        public decimal InitialPaymentAmount => TotalPrice * (InitialPaymentPercent / 100m);
        public decimal LoanAmount => TotalPrice - InitialPaymentAmount;

        public decimal MonthlyPayment
        {
            get
            {
                if (LoanAmount <= 0 || LoanTermMonths == 0) return 0;

                const decimal annualRate = 0.15m;
                decimal monthlyRate = annualRate / 12;

                // Формула аннуитетного платежа
                decimal factor = (decimal)System.Math.Pow((double)(1 + monthlyRate), LoanTermMonths);
                return LoanAmount * monthlyRate * factor / (factor - 1);
            }
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // Простые модели данных
    public class CarModel
    {
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
    }

    public class Engine
    {
        public string Type { get; set; }
        public decimal Price { get; set; }
    }

    public class CarColor
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string HexCode { get; set; }
    }

    public class CarOption : INotifyPropertyChanged
    {
        private bool _isSelected;
        public string Name { get; set; }
        public decimal Price { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    // Статический класс для хранения конфигурации
    public static class AppConfig
    {
        public static CarConfig Current { get; } = new CarConfig();

        public static void InitializeOptions()
        {
            Current.Options = new List<CarOption>
            {
                new CarOption { Name = "Пакет 'Зима'", Price = 45000 },
                new CarOption { Name = "Премиум аудиосистема", Price = 90000 },
                new CarOption { Name = "Панорамная крыша", Price = 120000 }
            };
        }
    }

}