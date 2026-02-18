using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1.Pages
{
    public partial class SessionPage : Page
    {
        private MainWindow mainWindow;
        private int sessionId;
        private int selectedSeatId = -1;

        public SessionPage(MainWindow window, int id)
        {
            InitializeComponent();
            mainWindow = window;
            sessionId = id;
            LoadSession();
        }

        private void LoadSession()
        {
            var session = Core.Context.sessions.FirstOrDefault(s => s.session_id == sessionId);
            if (session == null)
            {
                MessageBox.Show("Сеанс не найден");
                mainWindow.MainFrame.Content = new HomePage(mainWindow);
                return;
            }

            var movie = Core.Context.Movies.FirstOrDefault(m => m.movie_id == session.movie_id);
            if (movie != null)
            {
                MovieTitleText.Text = $"Фильм: {movie.tittle}";
            }

            SessionInfoText.Text = $"Дата: {session.session_datetime} Цена: {session.price} руб.";
            LoadSeats(session.hall_id);
        }

        private void LoadSeats(int hallId)
        {
            SeatsPanel.Children.Clear();
            var seats = Core.Context.seats.Where(s => s.hall_id == hallId).OrderBy(s => s.Seats_number).ToList();

            foreach (var seat in seats)
            {
                Button seatButton = new Button();
                seatButton.Content = seat.Seats_number;
                seatButton.Width = 50;
                seatButton.Height = 50;
                seatButton.Margin = new Thickness(2);
                seatButton.Tag = seat.Seats_id;

                if (IsSeatOccupied(seat.Seats_id))
                {
                    seatButton.Background = Brushes.Red;
                    seatButton.IsEnabled = false;
                }
                else
                {
                    seatButton.Background = Brushes.LightGray;
                    seatButton.Click += SeatButton_Click;
                }

                SeatsPanel.Children.Add(seatButton);
            }
        }

        private bool IsSeatOccupied(int seatId)
        {
            var ticket = Core.Context.tickets.FirstOrDefault(t =>
                t.session_id == sessionId && t.seats_id == seatId);
            return ticket != null;
        }

        private void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            int seatId = (int)clickedButton.Tag;

            if (selectedSeatId == seatId)
            {
                clickedButton.Background = Brushes.LightGray;
                selectedSeatId = -1;
            }
            else
            {
                if (selectedSeatId != -1)
                {
                    var previousButton = FindSeatButton(selectedSeatId);
                    if (previousButton != null)
                    {
                        previousButton.Background = Brushes.LightGray;
                    }
                }

                clickedButton.Background = Brushes.Green;
                selectedSeatId = seatId;
            }
        }

        private Button FindSeatButton(int seatId)
        {
            foreach (var child in SeatsPanel.Children)
            {
                var button = child as Button;
                if (button != null && (int)button.Tag == seatId)
                {
                    return button;
                }
            }
            return null;
        }

        private void ClearSelection_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSeatId != -1)
            {
                var button = FindSeatButton(selectedSeatId);
                if (button != null)
                {
                    button.Background = Brushes.LightGray;
                }
                selectedSeatId = -1;
            }
        }

        private void BookTicket_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSeatId == -1)
            {
                MessageBox.Show("Выберите место");
                return;
            }

            mainWindow.MainFrame.Content = new TicketPage(mainWindow, sessionId, selectedSeatId);
        }
    }
}