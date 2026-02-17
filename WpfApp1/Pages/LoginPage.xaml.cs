using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            var user = Core.Context.users.FirstOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                MainWindow.CurrentUserId = user.user_id;

                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.MainFrame.Content = new HomePage();
                }
            }
            else
            {
                MessageBox.Show("Неверный email или пароль");
            }
        }
    }
}