using System.Windows;
using WpfApp1.Pages;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public static int CurrentUserId = -1;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new HomePage();
        }
    }
}