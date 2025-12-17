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

        // Загрузка страницы
        private void stranica_Zagruzena(object sender, RoutedEventArgs e)
        {
            // Загружаем сохраненные данные из памяти
            poleFio.Text = Dannye.tecushaa.fio ?? "";
            poleTel.Text = Dannye.tecushaa.telefon ?? "";
            poleEmail.Text = Dannye.tecushaa.email ?? "";

            UpdateInfo();
            CheckAll();
        }

        // Поле изменено
        private void pole_Change(object sender, TextChangedEventArgs e)
        {
            CheckAll();
            UpdateInfo();
        }

        // Проверить все поля
        private void CheckAll()
        {
            bool fioOk = CheckFio();
            bool telOk = CheckTel();
            bool emailOk = CheckEmail();

            knopkaDalee.IsEnabled = fioOk && telOk && emailOk;
        }

        // Проверить ФИО
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

            // Проверяем что есть хотя бы 2 слова
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

        // Проверить телефон
        private bool CheckTel()
        {
            string text = poleTel.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                ShowError(oshibkaTel, "Введите телефон");
                return false;
            }

            // Проверяем что все символы - цифры
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

            // Проверяем длину (от 10 до 11 цифр)
            if (text.Length < 10 || text.Length > 11)
            {
                ShowError(oshibkaTel, "10 или 11 цифр");
                return false;
            }

            HideError(oshibkaTel);
            return true;
        }

        // Проверить email
        private bool CheckEmail()
        {
            string text = poleEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                ShowError(oshibkaEmail, "Введите email");
                return false;
            }

            // Простая проверка - есть @
            if (!text.Contains("@"))
            {
                ShowError(oshibkaEmail, "Должен быть @");
                return false;
            }

            HideError(oshibkaEmail);
            return true;
        }

        // Показать ошибку
        private void ShowError(TextBlock field, string message)
        {
            field.Text = message;
            field.Visibility = Visibility.Visible;
        }

        // Скрыть ошибку
        private void HideError(TextBlock field)
        {
            field.Visibility = Visibility.Collapsed;
        }

        // Обновить информацию о заказе
        private void UpdateInfo()
        {
            string text = $"Автомобиль: {Dannye.tecushaa.PoleModel?.nazvanie ?? "не выбран"}\n";
            text += $"Цена: {Dannye.tecushaa.CenaItog:N0} руб.\n";
            text += $"Платеж в месяц: {Dannye.tecushaa.PlatejVMes:N0} руб.";

            info.Text = text;
        }

        // Назад - просто переходим
        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        // Дальше - сохраняем и переходим
        private void dalee_Click(object sender, RoutedEventArgs e)
        {
            // Сохраняем введенные данные в память
            Dannye.tecushaa.fio = poleFio.Text.Trim();
            Dannye.tecushaa.telefon = poleTel.Text.Trim();
            Dannye.tecushaa.email = poleEmail.Text.Trim();

            // Проверяем еще раз перед переходом
            if (!CheckFio() || !CheckTel() || !CheckEmail())
            {
                MessageBox.Show("Исправьте ошибки", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NavigationService.Navigate(new Page5());
        }
    }
}