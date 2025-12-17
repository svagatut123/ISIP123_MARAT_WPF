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
            Zagruzit();
        }

        // загрузка данных
        private void Zagruzit()
        {
            // модели авто
            spisokModel.ItemsSource = new List<ModelAvto>
            {
                new ModelAvto { nazvanie = "AUDI TT", cenaOsn = 1600000 },
                new ModelAvto { nazvanie = "ВАЗ 2106", cenaOsn = 2800000 },
                new ModelAvto { nazvanie = "митсубиси лансер 10 эволюшн", cenaOsn = 1200000 }
            };

            // двигатели
            spisokDvig.ItemsSource = new List<Dvigatel>
            {
                new Dvigatel { tip = "v.8", cenaDop = 0 },
                new Dvigatel { tip = "v.7", cenaDop = 180000 },
                new Dvigatel { tip = "v.6", cenaDop = 300000 }
            };

            Proverit();
        }

        // выбрана модель
        private void model_Selected(object sender, SelectionChangedEventArgs e)
        {
            Dannye.tecushaa.PoleModel = spisokModel.SelectedItem as ModelAvto;
            Proverit();
        }

        // выбран двигатель
        private void dvig_Selected(object sender, SelectionChangedEventArgs e)
        {
            Dannye.tecushaa.PoleDvig = spisokDvig.SelectedItem as Dvigatel;
            Proverit();
        }

        // проверка готовности
        private void Proverit()
        {
            bool modelVybrana = Dannye.tecushaa.PoleModel != null;
            bool dvigVybran = Dannye.tecushaa.PoleDvig != null;
            knopkaDalee.IsEnabled = modelVybrana && dvigVybran;
        }

        // переход дальше
        private void dalee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }
    }
}