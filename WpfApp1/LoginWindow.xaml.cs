using System.Linq;
using System.Windows;

namespace WpfApp1
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            var user = Core.Context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                if (user.IsFrozen)
                {
                    MessageBox.Show("ваш аккаунт заморожен.");
                }

                Core.CurrentUser = user;

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("неверный логин или пароль");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("тут должна быть регистрация по тз");
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            Core.CurrentUser = null; 
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}