using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;

namespace WpfApp1.pages
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Цвета
            CmbColor.ItemsSource = new List<CarColor>
            {
                new CarColor { Name = "Белый", Price = 0, HexCode = "#FFFFFF" },
                new CarColor { Name = "Черный металлик", Price = 35000, HexCode = "#000000" },
                new CarColor { Name = "Синий металлик", Price = 50000, HexCode = "#0000FF" },
                new CarColor { Name = "Красный", Price = 45000, HexCode = "#FF0000" }
            };

            // Инициализируем опции один раз
            if (AppConfig.Current.Options.Count == 0)
                AppConfig.InitializeOptions();

            OptionsList.ItemsSource = AppConfig.Current.Options;

            // Подписываемся на изменение опций
            foreach (var option in AppConfig.Current.Options)
                option.PropertyChanged += (s, e) => UpdatePrice();

            UpdatePrice();
        }

        private void CmbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppConfig.Current.SelectedColor = CmbColor.SelectedItem as CarColor;
            UpdatePrice();
            CheckCompletion();
        }

        private void UpdatePrice()
        {
            TxtPrice.Text = $"Текущая стоимость: {AppConfig.Current.TotalPrice:N0} руб.";
        }

        private void CheckCompletion()
        {
            BtnNext.IsEnabled = AppConfig.Current.SelectedColor != null;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
        private void BtnNext_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new Page3());
    }
}