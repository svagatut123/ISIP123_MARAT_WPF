using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;

namespace WpfApp1.pages
{
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Модели
            CmbModel.ItemsSource = new List<CarModel>
            {
                new CarModel { Name = "Седан X", BasePrice = 1500000 },
                new CarModel { Name = "Внедорожник Y", BasePrice = 2500000 },
                new CarModel { Name = "Хэтчбек Z", BasePrice = 1200000 }
            };

            // Двигатели
            CmbEngine.ItemsSource = new List<Engine>
            {
                new Engine { Type = "1.6L Бензин (110 л.с.)", Price = 0 },
                new Engine { Type = "2.0L Turbo (190 л.с.)", Price = 150000 },
                new Engine { Type = "2.0L Дизель (170 л.с.)", Price = 180000 }
            };

            CheckCompletion();
        }

        private void CmbModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppConfig.Current.SelectedModel = CmbModel.SelectedItem as CarModel;
            CheckCompletion();
        }

        private void CmbEngine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppConfig.Current.SelectedEngine = CmbEngine.SelectedItem as Engine;
            CheckCompletion();
        }

        private void CheckCompletion()
        {
            BtnNext.IsEnabled = AppConfig.Current.SelectedModel != null &&
                               AppConfig.Current.SelectedEngine != null;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }
    }
}