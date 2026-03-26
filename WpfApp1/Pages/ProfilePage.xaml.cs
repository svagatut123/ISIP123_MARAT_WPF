using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class ProfilePage : Page
    {
        private MainWindow mainWindow;

        public ProfilePage(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
            LoadProfile();
        }

        private void LoadProfile()
        {
            if (Core.CurrentUser == null)
            {
                WelcomeText.Text = "Профиль";
                UserInfoText.Text = "Пожалуйста, войдите в аккаунт";
                LoginButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                WelcomeText.Text = "Добро пожаловать!";
                UserInfoText.Text = $"Имя: {Core.CurrentUser.firstname} {Core.CurrentUser.lastname}\n" +
                                   $"Email: {Core.CurrentUser.email}\n" +
                                   $"Телефон: {Core.CurrentUser.phone_number ?? "не указан"}";

                LoadTickets();
            }
        }

        private void LoadTickets()
        {
            var tickets = Core.Context.tickets.Where(t => t.user_id == Core.CurrentUser.user_id).ToList();

            if (tickets.Count == 0)
            {
                UserInfoText.Text += "\n\nУ вас нет купленных билетов";
                return;
            }

            string ticketsInfo = "\n\nВаши билеты:\n";
            foreach (var ticket in tickets)
            {
                var session = Core.Context.sessions.FirstOrDefault(s => s.session_id == ticket.session_id);
                if (session != null)
                {
                    var movie = Core.Context.Movies.FirstOrDefault(m => m.movie_id == session.movie_id);
                    if (movie != null)
                    {
                        ticketsInfo += $"\n• {movie.title}\n  Дата: {session.session_datetime}\n";
                    }
                }
            }

            UserInfoText.Text += ticketsInfo;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.Content = new LoginPage(mainWindow);
        }
    }
}