using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) || string.IsNullOrWhiteSpace(AuthorBox.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (BuildData.SaveAssembly(NameBox.Text, AuthorBox.Text))
            {
                MessageBox.Show("Сохранено");
                BuildData.Clear();
                CancelClick(sender, e);
            }
            else
            {
                MessageBox.Show("Ошибка сохранения");
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            var wnd = Window.GetWindow(this) as MainWindow;
            wnd?.MainFrame.Navigate(new Page1());
        }
    }
}