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
            MainFrame.Navigate(new Page1());
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // Обновляем прогресс и заголовок
            string[] stepTitles = {
                "Шаг 1: Выбор модели и двигателя",
                "Шаг 2: Выбор цвета и опций",
                "Шаг 3: Расчет кредита",
                "Шаг 4: Контактные данные",
                "Шаг 5: Итоговая сводка"
            };

            int stepIndex = 0;
            if (e.Content is Page1) stepIndex = 0;
            else if (e.Content is Page2) stepIndex = 1;
            else if (e.Content is Page3) stepIndex = 2;
            else if (e.Content is Page4) stepIndex = 3;
            else if (e.Content is Page5_Summary) stepIndex = 4;

            AppProgressBar.Value = (stepIndex + 1) * 20; // 20% за шаг
            StepTitle.Text = stepTitles[stepIndex];
        }
    }
}