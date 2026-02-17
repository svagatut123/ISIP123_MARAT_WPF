using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class MoviePage : Page
    {
        private int movieId;

        public MoviePage(int id)
        {
            InitializeComponent();
            movieId = id;
            LoadMovie();
        }

        private void LoadMovie()
        {
            var movie = Core.Context.Movies.FirstOrDefault(m => m.movie_id == movieId);
            if (movie == null)
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.MainFrame.Content = new HomePage();
                }
                return;
            }

            TitleText.Text = movie.title;
            DescriptionText.Text = movie.description;
            RatingText.Text = $"Рейтинг: {movie.rating}";
            ReleaseDateText.Text = $"Дата: {movie.release_date}";

            LoadSessions();
        }

        private void LoadSessions()
        {
            SessionsPanel.Children.Clear();
            var sessions = Core.Context.sessions.Where(s => s.movie_id == movieId).ToList();

            foreach (var session in sessions)
            {
                AddSessionToPanel(session);
            }
        }

        private void AddSessionToPanel(sessions session)
        {
            Button sessionButton = new Button();
            sessionButton.Content = $"Дата: {session.session_datetime} Зал: {session.hall_id}";
            sessionButton.Margin = new Thickness(0, 5, 0, 5);
            sessionButton.Click += (s, e) => Session_Click(session);
            SessionsPanel.Children.Add(sessionButton);
        }

        private void Session_Click(sessions session)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Content = new SessionPage(session.session_id);
            }
        }
    }
}