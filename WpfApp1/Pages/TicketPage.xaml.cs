using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class TicketPage : Page
    {
        private int sessionId;
        private int seatId;

        public TicketPage(int session_id, int seat_id)
        {
            InitializeComponent();
            sessionId = session_id;
            seatId = seat_id;
            LoadTicketInfo();
        }

        private void LoadTicketInfo()
        {
            var session = Core.Context.sessions.FirstOrDefault(s => s.session_id == sessionId);
            if (session == null)
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.MainFrame.Content = new HomePage();
                }
                return;
            }

            var movie = Core.Context.Movies.FirstOrDefault(m => m.movie_id == session.movie_id);
            if (movie != null)
            {
                MovieText.Text = $"Фильм: {movie.title}";
            }

            DateTimeText.Text = $"Дата: {session.session_datetime}";
            PriceText.Text = $"Цена: {session.price}";
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            var ticket = new tickets
            {
                user_id = MainWindow.CurrentUserId,
                session_id = sessionId,
                hall_id = 1,
                seats_id = seatId
            };

            Core.Context.tickets.Add(ticket);
            Core.Context.SaveChanges();

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Content = new HomePage();
            }
        }
    }
}