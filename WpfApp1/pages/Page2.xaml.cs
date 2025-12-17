using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;

namespace WpfApp1.pages
{
    public partial class Page2 : Page
    {
        private List<Cvet> _cveta;
        private List<CheckBox> _checkboxi = new List<CheckBox>();

        public Page2()
        {
            InitializeComponent();
            Zagruzit();
            VosstanovitVibor();  
        }

        private void Zagruzit()
        {
            
            _cveta = new List<Cvet>
            {
                new Cvet { nazvanie = "Белый", cenaDop = 0, kodCveta = "#FFFFFF" },
                new Cvet { nazvanie = "Черный", cenaDop = 30000, kodCveta = "#000000" },
                new Cvet { nazvanie = "Серый металлик", cenaDop = 40000, kodCveta = "#808080" },
                new Cvet { nazvanie = "Синий", cenaDop = 45000, kodCveta = "#0000FF" }
            };
            spisokCvet.ItemsSource = _cveta;

            if (Dannye.tecushaa.VseOpcii.Count == 0)
            {
                Dannye.tecushaa.VseOpcii = new List<Opcia>
                {
                    new Opcia { nazvanie = "Кожаный салон", cena = 150000 },
                    new Opcia { nazvanie = "Панорамная крыша", cena = 120000 },
                    new Opcia { nazvanie = "Круиз-контроль", cena = 50000 },
                    new Opcia { nazvanie = "Парктроники", cena = 40000 }
                };
            }

            foreach (var opcia in Dannye.tecushaa.VseOpcii)
            {
                var chk = new CheckBox
                {
                    Content = $"{opcia.nazvanie} (+{opcia.cena:N0} руб.)",
                    IsChecked = opcia.vibrano,
                    FontSize = 14,
                    Margin = new Thickness(0, 5, 0, 5),
                    Tag = opcia  
                };

                chk.Checked += (s, e) =>
                {
                    if (chk.Tag is Opcia opt)
                        opt.vibrano = true;
                    ObnovitCenu();
                };

                chk.Unchecked += (s, e) =>
                {
                    if (chk.Tag is Opcia opt)
                        opt.vibrano = false;
                    ObnovitCenu();
                };

                panelOpcii.Children.Add(chk);
                _checkboxi.Add(chk);
            }

            ObnovitCenu();
            Proverit();
        }

        private void VosstanovitVibor()
        {
            if (Dannye.tecushaa.PoleCvet != null)
            {
                foreach (Cvet cvet in _cveta)
                {
                    if (cvet.nazvanie == Dannye.tecushaa.PoleCvet.nazvanie)
                    {
                        spisokCvet.SelectedItem = cvet;
                        break;
                    }
                }
            }

            foreach (var chk in _checkboxi)
            {
                if (chk.Tag is Opcia opt)
                {
                    chk.IsChecked = opt.vibrano;
                }
            }
        }

        private void cvet_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (spisokCvet.SelectedItem != null)
            {
                Dannye.tecushaa.PoleCvet = spisokCvet.SelectedItem as Cvet;
            }
            ObnovitCenu();
            Proverit();
        }

        private void ObnovitCenu()
        {
            tekyshayaCena.Text = $"Текущая стоимость: {Dannye.tecushaa.CenaItog:N0} руб.";
        }

        private void Proverit()
        {
            knopkaDalee.IsEnabled = Dannye.tecushaa.PoleCvet != null;
        }

        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void dalee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page3());
        }
    }
}