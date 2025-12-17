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
            Loaded += Page3_Loaded;
        }

        private void Page3_Loaded(object sender, RoutedEventArgs e)
        {
            if (sliderProcent == null || sliderSrok == null)
                return;

            sliderProcent.Value = (double)Dannye.tecushaa.procentVznos;
            sliderSrok.Value = (double)Dannye.tecushaa.srokMes;

            ObnovitRaschet();
        }

        private void slider_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sliderProcent == null || sliderSrok == null)
                return;

            Dannye.tecushaa.procentVznos = (decimal)sliderProcent.Value;
            Dannye.tecushaa.srokMes = (int)sliderSrok.Value;

            ObnovitRaschet();
        }

        private void ObnovitRaschet()
        {
            if (cenaItog == null || textProcent == null || textSrok == null)
                return;

            decimal cena = Dannye.tecushaa.CenaItog;
            cenaItog.Text = $"Общая стоимость: {cena:N0} руб.";

            decimal procent = Dannye.tecushaa.procentVznos;
            decimal summaVznosDec = Dannye.tecushaa.SummaVznos;
            textProcent.Text = $"{procent:0}%";

            if (summaVznos != null)
                summaVznos.Text = $"Сумма: {summaVznosDec:N0} руб.";

            int srok = Dannye.tecushaa.srokMes;
            textSrok.Text = $"{srok} мес.";

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

        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            Dannye.tecushaa.fio = "";
            Dannye.tecushaa.telefon = "";
            Dannye.tecushaa.email = "";

            NavigationService.GoBack();
        }

        private void dalee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page4());
        }
    }
}