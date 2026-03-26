using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WpfApp1.Pages
{
    public partial class SessionPage : Page
    {
        private MainWindow mainWindow;
        private int sessionId;
        private int selectedSeatId = -1;
        private WrapPanel seatsPanel;

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
                MovieTitleText.Text = $"Фильм: {movie.title}";
            }

            SessionInfoText.Text = $"Дата: {session.session_datetime} Цена: {session.price} руб.";
            LoadSeats(session.hall_id);
        }

        private void LoadSeats(int hallId)
        {
            var seats = Core.Context.seats.Where(s => s.hall_id == hallId).OrderBy(s => s.Seats_number).ToList();

            var seatsWithOccupancy = new System.Collections.ObjectModel.ObservableCollection<SeatInfo>();

            foreach (var seat in seats)
            {
                bool isOccupied = IsSeatOccupied(seat.Seats_id);
                seatsWithOccupancy.Add(new SeatInfo
                {
                    Seats_id = seat.Seats_id,
                    Seats_number = (int)seat.Seats_number,
                    IsOccupied = isOccupied
                });
            }

            SeatsList.ItemsSource = seatsWithOccupancy;
            seatsPanel = FindVisualChild<WrapPanel>(SeatsList);
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

            // Обновляем цвет всех кнопок
            if (seatsPanel != null)
            {
                foreach (var child in seatsPanel.Children)
                {
                    var button = child as Button;
                    if (button != null)
                    {
                        var seatInfo = button.DataContext as SeatInfo;
                        if (seatInfo != null)
                        {
                            if (seatInfo.IsOccupied)
                            {
                                button.Background = Brushes.Red;
                                button.IsEnabled = false;
                            }
                            else if (seatInfo.Seats_id == seatId)
                            {
                                button.Background = Brushes.Green;
                            }
                            else
                            {
                                button.Background = Brushes.LightGray;
                            }
                        }
                    }
                }
            }

            selectedSeatId = seatId;
        }

        private Button FindSeatButton(int seatId)
        {
            if (seatsPanel != null)
            {
                foreach (var child in seatsPanel.Children)
                {
                    var button = child as Button;
                    if (button != null && (int)button.Tag == seatId)
                    {
                        return button;
                    }
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

        // Вспомогательный метод дляка дочернего элемента в визуальном дереве
        private T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                        return (T)child;

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null)
                        return childItem;
                }
            }
            return null;
        }
    }

    public class SeatInfo
    {
        public int Seats_id { get; set; }
        public int Seats_number { get; set; }
        public bool IsOccupied { get; set; }
    }
}