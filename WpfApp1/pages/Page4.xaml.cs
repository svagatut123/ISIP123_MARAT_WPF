using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Models;

namespace WpfApp1.pages
{
    public partial class Page4 : Page
    {
        private bool _hasChanges = false;

        public Page4()
        {
            InitializeComponent();
        }

        // Загрузка страницы
        private void stranica_Zagruzena(object sender, RoutedEventArgs e)
        {
            // Загружаем сохраненные данные
            poleFio.Text = Dannye.tecushaa.fio ?? "";
            poleTel.Text = Dannye.tecushaa.telefon ?? "";
            poleEmail.Text = Dannye.tecushaa.email ?? "";

            UpdateInfo();
            CheckAll();
        }

        // Выгрузка страницы
        private void stranica_Vigruzhena(object sender, RoutedEventArgs e)
        {
            Save();
        }

        // Поле изменено
        private void pole_Change(object sender, TextChangedEventArgs e)
        {
            _hasChanges = true;
            CheckAll();
            UpdateInfo();
        }

        // Проверка ввода телефона - нельзя писать буквы
        private void poleTel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем каждый символ
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;  // Блокируем ввод

                    // Показываем ошибку
                    oshibkaTel.Text = "Нельзя писать буквы";
                    oshibkaTel.Visibility = Visibility.Visible;
                    return;
                }
            }

            // Если все цифры - скрываем ошибку
            oshibkaTel.Visibility = Visibility.Collapsed;
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
            foreach (char c in text)
            {
                if (!char.IsDigit(c))
                {
                    ShowError(oshibkaTel, "Только цифры");
                    return false;
                }
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

        // Сохранить данные
        private void Save()
        {
            Dannye.tecushaa.fio = poleFio.Text.Trim();
            Dannye.tecushaa.telefon = poleTel.Text.Trim();
            Dannye.tecushaa.email = poleEmail.Text.Trim();
            _hasChanges = false;
        }

        // Обновить информацию
        private void UpdateInfo()
        {
            string text = $"Автомобиль: {Dannye.tecushaa.PoleModel?.nazvanie ?? "не выбран"}\n";
            text += $"Цена: {Dannye.tecushaa.CenaItog:N0} руб.\n";
            text += $"Платеж в месяц: {Dannye.tecushaa.PlatejVMes:N0} руб.";

            info.Text = text;
        }

        // Назад
        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            if (_hasChanges)
            {
                var result = MessageBox.Show(
                    "Есть несохраненные изменения. Продолжить?",
                    "Вопрос",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    return;
            }

            NavigationService.GoBack();
        }

        // Дальше
        private void dalee_Click(object sender, RoutedEventArgs e)
        {
            Save();

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