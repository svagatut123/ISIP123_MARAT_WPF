using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public static Client CurrentUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ShowMovies_Click(null, null);
        }

        private void ShowMovies_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new MoviesPage();
        }

        private void ShowLogin_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new LoginPage();
        }

        private void ShowRegister_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new RegisterPage();
        }

        private void ShowProfile_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser != null)
            {
                MainFrame.Content = new ProfilePage();
            }
        }

        public void UpdateAuthButtons()
        {
            if (CurrentUser != null)
            {
                LoginButton.Visibility = Visibility.Collapsed;
                RegisterButton.Visibility = Visibility.Collapsed;
                ProfileButton.Visibility = Visibility.Visible;
                ProfileButton.Content = "Личный кабинет: " + CurrentUser.FirstName;
            }
            else
            {
                LoginButton.Visibility = Visibility.Visible;
                RegisterButton.Visibility = Visibility.Visible;
                ProfileButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}