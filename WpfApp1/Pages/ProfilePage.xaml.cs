using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            LoadProfile();
        }

        private void LoadProfile()
        {
            var user = Core.Context.users.FirstOrDefault(u => u.user_id == MainWindow.CurrentUserId);
            if (user != null)
            {
                UserInfoText.Text = $"Имя: {user.firstname} {user.lastname}\nEmail: {user.email}";
            }

            LoadTickets();
        }

        private void LoadTickets()
        {
            TicketsPanel.Children.Clear();
            var tickets = Core.Context.tickets.Where(t => t.user_id == MainWindow.CurrentUserId).ToList();

            foreach (var ticket in tickets)
            {
                var session = Core.Context.sessions.FirstOrDefault(s => s.session_id == ticket.session_id);
                if (session != null)
                {
                    var movie = Core.Context.Movies.FirstOrDefault(m => m.movie_id == session.movie_id);
                    if (movie != null)
                    {
                        TextBlock ticketText = new TextBlock();
                        ticketText.Text = $"Фильм: {movie.title}, Дата: {session.session_datetime}";
                        TicketsPanel.Children.Add(ticketText);
                    }
                }
            }
        }
    }
}