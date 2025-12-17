using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;

namespace WpfApp1.pages
{
    public partial class Page4 : Page
    {
        public Page4()
        {
            InitializeComponent();
        }

        private void stranica_Zagruzena(object sender, RoutedEventArgs e)
        {
            // Загружаем данные из памяти
            poleFio.Text = Dannye.tecushaa.fio ?? "";
            poleTel.Text = Dannye.tecushaa.telefon ?? "";
            poleEmail.Text = Dannye.tecushaa.email ?? "";

            UpdateInfo();
            CheckAll();
        }

        private void pole_Change(object sender, TextChangedEventArgs e)
        {
            CheckAll();
            UpdateInfo();
        }

        private void CheckAll()
        {
            bool fioOk = CheckFio();
            bool telOk = CheckTel();
            bool emailOk = CheckEmail();

            knopkaDalee.IsEnabled = fioOk && telOk && emailOk;
        }

        private bool CheckFio()
        {
            string text = poleFio.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                ShowError(oshibkaFio, "Введите ФИО");
                return false;
            }

            if (text.Length < 5)
            {
                ShowError(oshibkaFio, "Минимум 5 символов");
                return false;
            }

            string[] words = text.Split(' ');
            int wordCount = 0;
            foreach (string word in words)
            {
                if (word.Length > 0) wordCount++;
            }

            if (wordCount < 2)
            {
                ShowError(oshibkaFio, "Введите фамилию и имя");
                return false;
            }

            HideError(oshibkaFio);
            return true;
        }

        private bool CheckTel()
        {
            string text = poleTel.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                ShowError(oshibkaTel, "Введите телефон");
                return false;
            }

            bool hasLetters = false;
            foreach (char c in text)
            {
                if (!char.IsDigit(c))
                {
                    hasLetters = true;
                    break;
                }
            }

            if (hasLetters)
            {
                ShowError(oshibkaTel, "Только цифры");
                return false;
            }

            if (text.Length < 10 || text.Length > 11)
            {
                ShowError(oshibkaTel, "10 или 11 цифр");
                return false;
            }

            HideError(oshibkaTel);
            return true;
        }

        private bool CheckEmail()
        {
            string text = poleEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                ShowError(oshibkaEmail, "Введите email");
                return false;
            }

            if (!text.Contains("@"))
            {
                ShowError(oshibkaEmail, "Должен быть @");
                return false;
            }

            HideError(oshibkaEmail);
            return true;
        }

        private void ShowError(TextBlock field, string message)
        {
            field.Text = message;
            field.Visibility = Visibility.Visible;
        }

        private void HideError(TextBlock field)
        {
            field.Visibility = Visibility.Collapsed;
        }

        private void UpdateInfo()
        {
            string text = $"Автомобиль: {Dannye.tecushaa.PoleModel?.nazvanie ?? "не выбран"}\n";
            text += $"Цена: {Dannye.tecushaa.CenaItog:N0} руб.\n";
            text += $"Платеж в месяц: {Dannye.tecushaa.PlatejVMes:N0} руб.";

            info.Text = text;
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
            if (!CheckFio() || !CheckTel() || !CheckEmail())
            {
                MessageBox.Show("Исправьте ошибки", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Dannye.tecushaa.fio = poleFio.Text.Trim();
            Dannye.tecushaa.telefon = poleTel.Text.Trim();
            Dannye.tecushaa.email = poleEmail.Text.Trim();

            NavigationService.Navigate(new Page5());
        }
    }
}