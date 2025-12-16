using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;

namespace WpfApp1.pages
{
    public partial class Page4 : Page
    {
        // есть ли несохраненные изменения
        private bool _estIzmeneniya = false;

        public Page4()
        {
            InitializeComponent();
        }

        // загрузка страницы
        private void stranica_Zagruzena(object sender, RoutedEventArgs e)
        {
            // загружаем сохраненные данные
            poleFio.Text = Dannye.tecushaa.fio ?? "";
            poleTel.Text = Dannye.tecushaa.telefon ?? "";
            poleEmail.Text = Dannye.tecushaa.email ?? "";

            ObnovitInfo();
            ProveritVse();
        }

        // выгрузка страницы
        private void stranica_Vigruzhena(object sender, RoutedEventArgs e)
        {
            Sohranit();
        }

        // поле изменено
        private void pole_Izmenilos(object sender, TextChangedEventArgs e)
        {
            _estIzmeneniya = true;
            ProveritVse();
            ObnovitInfo();
        }

        // проверить все поля
        private void ProveritVse()
        {
            bool fioOk = ProveritFio();
            bool telOk = ProveritTel();
            bool emailOk = ProveritEmail();

            // кнопка активна если все ок
            knopkaDalee.IsEnabled = fioOk && telOk && emailOk;
        }

        // проверить фио
        private bool ProveritFio()
        {
            string text = poleFio.Text.Trim();

            // проверка на пустое
            if (string.IsNullOrWhiteSpace(text))
            {
                PokazatOshibku(oshibkaFio, "введите ФИО");
                return false;
            }

            // проверка длины
            if (text.Length < 5)
            {
                PokazatOshibku(oshibkaFio, "минимум 5 символов");
                return false;
            }

            // проверка на 2 слова
            int slov = text.Split(' ').Count(w => w.Length > 0);
            if (slov < 2)
            {
                PokazatOshibku(oshibkaFio, "введите фамилию и имя");
                return false;
            }

            SpryatatOshibku(oshibkaFio);
            return true;
        }

        // проверить телефон
        private bool ProveritTel()
        {
            string text = poleTel.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                PokazatOshibku(oshibkaTel, "введите телефон");
                return false;
            }

            // оставляем только цифры
            string cifry = new string(text.Where(char.IsDigit).ToArray());

            // проверка длины
            if (cifry.Length < 10 || cifry.Length > 11)
            {
                PokazatOshibku(oshibkaTel, "неверная длина");
                return false;
            }

            SpryatatOshibku(oshibkaTel);
            return true;
        }

        // проверить email
        private bool ProveritEmail()
        {
            string text = poleEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                PokazatOshibku(oshibkaEmail, "введите email");
                return false;
            }

            // проверяем @
            if (!text.Contains("@"))
            {
                PokazatOshibku(oshibkaEmail, "должен быть @");
                return false;
            }

            // проверяем точку
            if (!text.Contains("."))
            {
                PokazatOshibku(oshibkaEmail, "должна быть точка");
                return false;
            }

            SpryatatOshibku(oshibkaEmail);
            return true;
        }

        // показать ошибку
        private void PokazatOshibku(TextBlock pole, string text)
        {
            pole.Text = $"✗ {text}";
            pole.Visibility = Visibility.Visible;
        }

        // спрятать ошибку
        private void SpryatatOshibku(TextBlock pole)
        {
            pole.Visibility = Visibility.Collapsed;
        }

        // сохранить данные
        private void Sohranit()
        {
            Dannye.tecushaa.fio = poleFio.Text.Trim();
            Dannye.tecushaa.telefon = poleTel.Text.Trim();
            Dannye.tecushaa.email = poleEmail.Text.Trim();
            _estIzmeneniya = false;
        }

        // обновить информацию
        private void ObnovitInfo()
        {
            string text = $"Выбран автомобиль: {Dannye.tecushaa.PoleModel?.nazvanie ?? "нет"}\n";
            text += $"Цена: {Dannye.tecushaa.CenaItog:N0} руб.\n";
            text += $"Платеж в месяц: {Dannye.tecushaa.PlatejVMes:N0} руб.";

            info.Text = text;
        }

        // назад
        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            if (_estIzmeneniya)
            {
                // спрашиваем подтверждение
                var otvet = MessageBox.Show(
                    "Есть несохраненные изменения. Продолжить?",
                    "Вопрос",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (otvet == MessageBoxResult.No)
                    return;
            }

            NavigationService.GoBack();
        }

        // дальше
        private void dalee_Click(object sender, RoutedEventArgs e)
        {
            // сохраняем
            Sohranit();

            // проверяем
            if (!ProveritFio() || !ProveritTel() || !ProveritEmail())
            {
                MessageBox.Show("Исправьте ошибки", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NavigationService.Navigate(new Page5());
        }
    }
}