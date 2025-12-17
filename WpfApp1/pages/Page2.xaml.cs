using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Models;

namespace WpfApp1.pages
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            Zagruzit();
        }

        private void Zagruzit()
        {
            // цвета
            spisokCvet.ItemsSource = new List<Cvet>
            {
                new Cvet { nazvanie = "Белый", cenaDop = 0, kodCveta = "#FFFFFF" },
                new Cvet { nazvanie = "Черный", cenaDop = 30000, kodCveta = "#000000" },
                new Cvet { nazvanie = "Синий", cenaDop = 45000, kodCveta = "#0000FF" }
            };

            // инициализация опций
            if (Dannye.tecushaa.VseOpcii.Count == 0)
            {
                Dannye.tecushaa.VseOpcii = new List<Opcia>
                {
                    new Opcia { nazvanie = "Кожаный салон", cena = 150000 },
                    new Opcia { nazvanie = "Панорамная крыша", cena = 120000 },
                    new Opcia { nazvanie = "Парктроники", cena = 40000 }
                };
            }

            // создаем чекбоксы для опций
            foreach (var opcia in Dannye.tecushaa.VseOpcii)
            {
                var chk = new CheckBox
                {
                    Content = $"{opcia.nazvanie} (+{opcia.cena:N0} руб.)",
                    IsChecked = opcia.vibrano,
                    FontSize = 14,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                // привязка состояния
                chk.Checked += (s, e) =>
                {
                    opcia.vibrano = true;
                    ObnovitCenu();
                };
                chk.Unchecked += (s, e) =>
                {
                    opcia.vibrano = false;
                    ObnovitCenu();
                };

                panelOpcii.Children.Add(chk);
            }

            ObnovitCenu();
        }

        // выбран цвет
        private void cvet_Selected(object sender, SelectionChangedEventArgs e)
        {
            Dannye.tecushaa.PoleCvet = spisokCvet.SelectedItem as Cvet;
            ObnovitCenu();
            Proverit();
        }

        // обновить цену
        private void ObnovitCenu()
        {
            tekyshayaCena.Text = $"Текущая стоимость: {Dannye.tecushaa.CenaItog:N0} руб.";
        }

        // проверка готовности
        private void Proverit()
        {
            knopkaDalee.IsEnabled = Dannye.tecushaa.PoleCvet != null;
        }

        // назад
        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        // дальше
        private void dalee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page3());
        }
    }
}