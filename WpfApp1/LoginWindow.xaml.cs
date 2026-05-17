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
                if (user.IsFrozen == true)
                {
                    MessageBox.Show("Ваш аккаунт заморожен.");
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

    }
}