using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            string firstname = FirstNameBox.Text.Trim();
            string lastname = LastNameBox.Text.Trim();

            var user = new users
            {
                email = email,
                password = password,
                firstname = firstname,
                lastname = lastname
            };

            Core.Context.users.Add(user);
            Core.Context.SaveChanges();

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Content = new LoginPage();
            }
        }
    }
}