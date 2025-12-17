using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Models;

namespace WpfApp1.pages
{
    public partial class Page5 : Page
    {
        public Page5()
        {
            InitializeComponent();
            PostroitSvodku();
        }

        // построить сводку
        private void PostroitSvodku()
        {
            panelSvodka.Children.Clear();

            // заголовок
            DobavitZagolovok("Конфигурация автомобиля");

            // модель
            DobavitStroku("Модель:", Dannye.tecushaa.PoleModel?.nazvanie ?? "нет");

            // двигатель
            string dvigText = Dannye.tecushaa.PoleDvig?.tip ?? "нет";
            string dvigCena = Dannye.tecushaa.CenaDvig > 0 ? $"+{Dannye.tecushaa.CenaDvig:N0} руб." : "";
            DobavitStroku("Двигатель:", dvigText, dvigCena);

            // цвет
            string cvetText = Dannye.tecushaa.PoleCvet?.nazvanie ?? "нет";
            string cvetCena = Dannye.tecushaa.CenaCvet > 0 ? $"+{Dannye.tecushaa.CenaCvet:N0} руб." : "";
            DobavitStroku("Цвет:", cvetText, cvetCena);

            // опции
            DobavitZagolovok("Дополнительные опции");
            bool estOpcii = false;
            foreach (var opcia in Dannye.tecushaa.VseOpcii)
            {
                if (opcia.vibrano)
                {
                    DobavitStroku($"  {opcia.nazvanie}", "", $"+{opcia.cena:N0} руб.");
                    estOpcii = true;
                }
            }
            if (!estOpcii)
                DobavitStroku("Опции не выбраны", "", "");

            // финансы
            DobavitZagolovok("Финансовый расчет");
            DobavitStroku("Общая цена:", "", $"{Dannye.tecushaa.CenaItog:N0} руб.", true);

            DobavitStroku("Первый взнос:",
                $"{Dannye.tecushaa.procentVznos}%",
                $"{Dannye.tecushaa.SummaVznos:N0} руб.");

            DobavitStroku("Сумма кредита:",
                $"{Dannye.tecushaa.srokMes} мес.",
                $"{Dannye.tecushaa.SummaCredit:N0} руб.");

            DobavitStroku("Платеж в месяц:", "",
                $"{Dannye.tecushaa.PlatejVMes:N0} руб.", true);

            // контакты
            DobavitZagolovok("Контактные данные");
            DobavitStroku("ФИО:", Dannye.tecushaa.fio ?? "нет");
            DobavitStroku("Телефон:", Dannye.tecushaa.telefon ?? "нет");
            DobavitStroku("Email:", Dannye.tecushaa.email ?? "нет");
        }

        // добавить заголовок
        private void DobavitZagolovok(string text)
        {
            var zagolovok = new TextBlock
            {
                Text = text,
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 20, 0, 10),
                Foreground = Brushes.Navy
            };
            panelSvodka.Children.Add(zagolovok);
        }

        // добавить строку
        private void DobavitStroku(string label, string value, string cena = "", bool zhyr = false)
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 5, 0, 5)
            };

            // метка
            var lbl = new TextBlock
            {
                Text = label,
                Width = 200,
                FontWeight = FontWeights.SemiBold
            };
            panel.Children.Add(lbl);

            // значение
            if (!string.IsNullOrEmpty(value))
            {
                var val = new TextBlock
                {
                    Text = value,
                    Width = 200,
                    FontWeight = zhyr ? FontWeights.Bold : FontWeights.Normal
                };
                panel.Children.Add(val);
            }

            // цена
            if (!string.IsNullOrEmpty(cena))
            {
                var cen = new TextBlock
                {
                    Text = cena,
                    FontWeight = zhyr ? FontWeights.Bold : FontWeights.Normal,
                    Foreground = zhyr ? Brushes.DarkGreen : Brushes.Black
                };
                panel.Children.Add(cen);
            }

            panelSvodka.Children.Add(panel);
        }

        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        // оформление
        private void oformit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                $"Заявка оформлена!\n\n" +
                $"Автомобиль: {Dannye.tecushaa.PoleModel?.nazvanie}\n" +
                $"Цена: {Dannye.tecushaa.CenaItog:N0} руб.\n" +
                $"Телефон: {Dannye.tecushaa.telefon}\n\n" +
                $"С вами свяжется менеджер.",
                "Успех",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}