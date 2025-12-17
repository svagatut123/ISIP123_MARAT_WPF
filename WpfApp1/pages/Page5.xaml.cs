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
            BuildSummary();
        }

        // Построить сводку
        private void BuildSummary()
        {
            panelSvodka.Children.Clear();

            // Заголовок конфигурации
            AddSection("Конфигурация автомобиля");

            // Модель
            AddRow("Модель:", Dannye.tecushaa.PoleModel?.nazvanie ?? "нет", "", true);

            // Двигатель
            string dvigText = Dannye.tecushaa.PoleDvig?.tip ?? "нет";
            string dvigCena = Dannye.tecushaa.CenaDvig > 0 ? $"+{Dannye.tecushaa.CenaDvig:N0} руб." : "";
            AddRow("Двигатель:", dvigText, dvigCena);

            // Цвет
            string cvetText = Dannye.tecushaa.PoleCvet?.nazvanie ?? "нет";
            string cvetCena = Dannye.tecushaa.CenaCvet > 0 ? $"+{Dannye.tecushaa.CenaCvet:N0} руб." : "";
            AddRow("Цвет:", cvetText, cvetCena);

            // Опции
            AddSection("Дополнительные опции");
            bool estOpcii = false;
            foreach (var opcia in Dannye.tecushaa.VseOpcii)
            {
                if (opcia.vibrano)
                {
                    AddRow($"  {opcia.nazvanie}", "", $"+{opcia.cena:N0} руб.");
                    estOpcii = true;
                }
            }
            if (!estOpcii)
            {
                AddRow("Опции не выбраны", "", "");
            }

            // Финансы
            AddSection("Финансовый расчет");
            AddRow("Общая цена:", "", $"{Dannye.tecushaa.CenaItog:N0} руб.", true);

            AddRow("Первый взнос:",
                $"{Dannye.tecushaa.procentVznos}%",
                $"{Dannye.tecushaa.SummaVznos:N0} руб.");

            AddRow("Сумма кредита:",
                $"{Dannye.tecushaa.srokMes} мес.",
                $"{Dannye.tecushaa.SummaCredit:N0} руб.");

            AddRow("Платеж в месяц:", "",
                $"{Dannye.tecushaa.PlatejVMes:N0} руб.", true);

            // Контакты
            AddSection("Контактные данные");
            AddRow("ФИО:", Dannye.tecushaa.fio ?? "нет");
            AddRow("Телефон:", Dannye.tecushaa.telefon ?? "нет");
            AddRow("Email:", Dannye.tecushaa.email ?? "нет");
        }

        // Добавить заголовок
        private void AddSection(string title)
        {
            var zagolovok = new TextBlock
            {
                Text = title,
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 20, 0, 10),
                Foreground = Brushes.Navy
            };
            panelSvodka.Children.Add(zagolovok);
        }

        // Добавить строку
        private void AddRow(string label, string value, string cena = "", bool zhyr = false)
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 5, 0, 5)
            };

            // Метка
            var lbl = new TextBlock
            {
                Text = label,
                Width = 200,
                FontWeight = FontWeights.SemiBold
            };
            panel.Children.Add(lbl);

            // Значение
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

            // Цена
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

        // Назад - очищаем контакты и возвращаемся на 4
        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            // Очищаем контакты
            Dannye.tecushaa.fio = "";
            Dannye.tecushaa.telefon = "";
            Dannye.tecushaa.email = "";

            // Возвращаемся на страницу 4
            NavigationService.GoBack();
        }

        // Оформление заказа
        private void oformit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                $"Заявка оформлена\n\n" +
                $"Автомобиль: {Dannye.tecushaa.PoleModel?.nazvanie}\n" +
                $"Цена: {Dannye.tecushaa.CenaItog:N0} руб.\n" +
                $"Телефон: {Dannye.tecushaa.telefon}");
        }
    }
}