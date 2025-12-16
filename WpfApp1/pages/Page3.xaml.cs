using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;

namespace WpfApp1.pages
{
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();

            // Подписываемся на событие загрузки слайдеров
            Loaded += Page3_Loaded;
        }

        // Загрузка страницы - безопасный способ
        private void Page3_Loaded(object sender, RoutedEventArgs e)
        {
            // Проверяем, что слайдеры существуют
            if (sliderProcent == null || sliderSrok == null)
            {
                MessageBox.Show("Ошибка загрузки элементов");
                return;
            }

            // Загружаем сохраненные значения
            sliderProcent.Value = (double)Dannye.tecushaa.procentVznos;
            sliderSrok.Value = (double)Dannye.tecushaa.srokMes;

            ObnovitRaschet();
        }

        // Слайдер изменен
        private void slider_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Проверяем, что слайдеры инициализированы
            if (sliderProcent == null || sliderSrok == null)
                return;

            // Сохраняем значения
            Dannye.tecushaa.procentVznos = (decimal)sliderProcent.Value;
            Dannye.tecushaa.srokMes = (int)sliderSrok.Value;

            ObnovitRaschet();
        }

        // Обновить расчет
        private void ObnovitRaschet()
        {
            // Проверяем, что элементы существуют
            if (cenaItog == null || textProcent == null || textSrok == null)
                return;

            // Общая цена
            decimal cena = Dannye.tecushaa.CenaItog;
            cenaItog.Text = $"Общая стоимость: {cena:N0} руб.";

            // Первый взнос
            decimal procent = Dannye.tecushaa.procentVznos;
            decimal summaVznosDec = Dannye.tecushaa.SummaVznos;
            textProcent.Text = $"{procent:0}%";

            if (summaVznos != null)
                summaVznos.Text = $"Сумма: {summaVznosDec:N0} руб.";

            // Срок
            int srok = Dannye.tecushaa.srokMes;
            textSrok.Text = $"{srok} мес.";

            // Расчет кредита
            decimal summaCred = Dannye.tecushaa.SummaCredit;
            decimal platej = Dannye.tecushaa.PlatejVMes;
            decimal pereplataDec = (platej * srok) - summaCred;

            if (summaCredit != null)
                summaCredit.Text = $"{summaCred:N0} руб.";

            if (pereplata != null)
                pereplata.Text = $"{pereplataDec:N0} руб.";

            if (platejMes != null)
                platejMes.Text = $"{platej:N0} руб.";
        }

        // Назад
        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        // Дальше
        private void dalee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page4());
        }
    }
}