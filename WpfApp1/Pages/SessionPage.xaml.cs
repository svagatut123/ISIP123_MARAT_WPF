using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class SessionPage : Page
    {
        private int sessionId;
        private int selectedSeatId = -1;

        public SessionPage(int id)
        {
            InitializeComponent();
            sessionId = id;
            LoadSession();
        }

        private void LoadSession()
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
                MovieTitleText.Text = $"Фильм: {movie.title}";
            }

            SessionInfoText.Text = $"Дата: {session.session_datetime} Цена: {session.price}";
            LoadSeats();
        }

        private void LoadSeats()
        {
            SeatsPanel.Children.Clear();
            var seats = Core.Context.seats.Where(s => s.hall_id == 1).ToList();

            foreach (var seat in seats)
            {
                Button seatButton = new Button();
                seatButton.Content = seat.Seats_number;
                seatButton.Width = 40;
                seatButton.Height = 40;
                seatButton.Margin = new Thickness(2);
                seatButton.Click += (s, e) => Seat_Click(seat.Seats_id);
                SeatsPanel.Children.Add(seatButton);
            }
        }

        private void Seat_Click(int seatId)
        {
            selectedSeatId = seatId;
        }

        private void BookTicket_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSeatId == -1)
            {
                MessageBox.Show("Выберите место");
                return;
            }

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Content = new TicketPage(sessionId, selectedSeatId);
            }
        }
    }
}