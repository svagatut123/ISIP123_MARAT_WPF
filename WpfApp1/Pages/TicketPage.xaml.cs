using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class TicketPage : Page
    {
        private MainWindow mainWindow;
        private int sessionId;
        private int seatId;
        private decimal price;

        public TicketPage(MainWindow window, int session_id, int seat_id)
        {
            InitializeComponent();
            mainWindow = window;
            sessionId = session_id;
            seatId = seat_id;
            LoadTicketInfo();
        }

        private void LoadTicketInfo()
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
                MovieText.Text = $"Фильм: {movie.tittle}";
            }

            var hall = Core.Context.halls.FirstOrDefault(h => h.hall_id == session.hall_id);
            if (hall != null)
            {
                HallText.Text = $"Зал: {hall.hallNumber}";
            }

            DateTimeText.Text = $"Дата и время: {session.session_datetime}";
            price = (decimal)session.price;
            PriceText.Text = $"Стоимость: {price} руб.";

            var seat = Core.Context.seats.FirstOrDefault(s => s.Seats_id == seatId);
            if (seat != null)
            {
                SeatText.Text = $"Место: {seat.Seats_number}";
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пожалуйста, войдите в аккаунт для оформления билета");
            mainWindow.MainFrame.Content = new LoginPage(mainWindow);
        }
    }
}