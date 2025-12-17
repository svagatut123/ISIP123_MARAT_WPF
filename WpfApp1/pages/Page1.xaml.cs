using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;

namespace WpfApp1.pages
{
    public partial class Page1 : Page
    {
        private List<ModelAvto> _modeli;
        private List<Dvigatel> _dvigateli;

        public Page1()
        {
            InitializeComponent();
            Zagruzit();
            VosstanovitVibor();  
        }

        private void Zagruzit()
        {
            
            _modeli = new List<ModelAvto>
            {
                new ModelAvto { nazvanie = "AUDI TT", cenaOsn = 1600000 },
                new ModelAvto { nazvanie = "ВАЗ 2106", cenaOsn = 2800000 },
                new ModelAvto { nazvanie = "митсубиси лансер 10 эволюшн", cenaOsn = 1200000 }
            };
            spisokModel.ItemsSource = _modeli;

            _dvigateli = new List<Dvigatel>
            {
                new Dvigatel { tip = "v.8", cenaDop = 0 },
                new Dvigatel { tip = "v.7", cenaDop = 180000 },
                new Dvigatel { tip = "v.6", cenaDop = 300000 }
            };
            spisokDvig.ItemsSource = _dvigateli;

            Proverit();
        }

        private void VosstanovitVibor()
        {
            if (Dannye.tecushaa.PoleModel != null)
            {
                foreach (ModelAvto model in _modeli)
                {
                    if (model.nazvanie == Dannye.tecushaa.PoleModel.nazvanie)
                    {
                        spisokModel.SelectedItem = model;
                        break;
                    }
                }
            }

            if (Dannye.tecushaa.PoleDvig != null)
            {
                foreach (Dvigatel dvig in _dvigateli)
                {
                    if (dvig.tip == Dannye.tecushaa.PoleDvig.tip)
                    {
                        spisokDvig.SelectedItem = dvig;
                        break;
                    }
                }
            }
        }

        private void model_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (spisokModel.SelectedItem != null)
            {
                Dannye.tecushaa.PoleModel = spisokModel.SelectedItem as ModelAvto;
            }
            Proverit();
        }

        private void dvig_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (spisokDvig.SelectedItem != null)
            {
                Dannye.tecushaa.PoleDvig = spisokDvig.SelectedItem as Dvigatel;
            }
            Proverit();
        }

        private void Proverit()
        {
            bool modelVybrana = Dannye.tecushaa.PoleModel != null;
            bool dvigVybran = Dannye.tecushaa.PoleDvig != null;
            knopkaDalee.IsEnabled = modelVybrana && dvigVybran;
        }

        private void dalee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }
    }
}