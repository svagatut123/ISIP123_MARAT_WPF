using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.pages;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ramka.Navigate(new Page1());
        }

        private void ramka_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // определяем текущий шаг
            int shag = 1;
            string text = "Шаг 1: Выбор модели";

            if (e.Content is Page1)
            {
                shag = 1;
                text = "Шаг 1: Выбор модели";
            }
            else if (e.Content is Page2)
            {
                shag = 2;
                text = "Шаг 2: Цвет и опции";
            }
            else if (e.Content is Page3)
            {
                shag = 3;
                text = "Шаг 3: Кредит";
            }
            else if (e.Content is Page4)
            {
                shag = 4;
                text = "Шаг 4: Контакты";
            }
            else if (e.Content is Page5)
            {
                shag = 5;
                text = "Шаг 5: Итог";
            }

            // обновляем прогресс
            progress.Value = shag * 20;
            zagolovok.Text = text;
        }
    }
}