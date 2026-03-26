using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1.Pages
{
    public partial class MoviePage : Page
    {
        private MainWindow mainWindow;
        private int movieId;

        public MoviePage(MainWindow window, int id)
        {
            InitializeComponent();
            mainWindow = window;
            movieId = id;
            LoadMovie();
        }

        private void LoadMovie()
        {
            var movie = Core.Context.Movies.FirstOrDefault(m => m.movie_id == movieId);
            if (movie == null)
            {
                MessageBox.Show("Фильм не найден");
                mainWindow.MainFrame.Content = new HomePage(mainWindow);
                return;
            }

            TitleText.Text = movie.title;
            DescriptionText.Text = movie.description ?? "Описание отсутствует";
            RatingText.Text = $"Рейтинг: {movie.rating}";
            ReleaseDateText.Text = $"Дата выхода: {movie.release_date}";

            LoadGenres(movie);
            LoadAgeRating(movie);
            LoadSessions();
        }

        private void LoadGenres(Movies movie)
        {
            var genres = Core.Context.movie_genres.Where(mg => mg.movie_id == movie.movie_id).ToList();
            string genreNames = "";

            foreach (var mg in genres)
            {
                var genre = Core.Context.genres.FirstOrDefault(g => g.genre_id == mg.genre_id);
                if (genre != null)
                {
                    if (genreNames.Length > 0) genreNames += ", ";
                    genreNames += genre.genre_name;
                }
            }

            if (genreNames.Length > 0)
            {
                GenresText.Text = $"Жанры: {genreNames}";
            }
        }

        private void LoadAgeRating(Movies movie)
        {
            var ageRating = Core.Context.AgeRating.FirstOrDefault(a => a.Id == movie.age_ratingId);
            if (ageRating != null)
            {
                AgeRatingText.Text = $"Возрастное ограничение: {ageRating.limitation}";
            }
        }

        private void LoadSessions()
        {
            SessionsPanel.Children.Clear();
            var sessions = Core.Context.sessions.Where(s => s.movie_id == movieId).ToList();

            if (sessions.Count == 0)
            {
                TextBlock noSessionsText = new TextBlock();
                noSessionsText.Text = "Сеансы отсутствуют";
                SessionsPanel.Children.Add(noSessionsText);
                return;
            }

            foreach (var session in sessions)
            {
                AddSessionToPanel(session);
            }
        }

        private void AddSessionToPanel(sessions session)
        {
            Border sessionBorder = new Border();
            sessionBorder.BorderBrush = Brushes.Black;
            sessionBorder.BorderThickness = new Thickness(1);
            sessionBorder.Margin = new Thickness(0, 5, 0, 5);
            sessionBorder.Padding = new Thickness(10);
            sessionBorder.Cursor = System.Windows.Input.Cursors.Hand;
            sessionBorder.MouseLeftButtonDown += (s, e) => Session_Click(session);

            StackPanel sessionPanel = new StackPanel();

            TextBlock dateTimeText = new TextBlock();
            dateTimeText.Text = $"Дата и время: {session.session_datetime}";
            sessionPanel.Children.Add(dateTimeText);

            var hall = Core.Context.halls.FirstOrDefault(h => h.hall_id == session.hall_id);
            if (hall != null)
            {
                TextBlock hallText = new TextBlock();
                hallText.Text = $"Зал: {hall.hallNumber} ({hall.hall_type})";
                sessionPanel.Children.Add(hallText);
            }

            TextBlock priceText = new TextBlock();
            priceText.Text = $"Цена: {session.price} руб.";
            sessionPanel.Children.Add(priceText);

            sessionBorder.Child = sessionPanel;
            SessionsPanel.Children.Add(sessionBorder);
        }

        private void Session_Click(sessions session)
        {
            mainWindow.MainFrame.Content = new SessionPage(mainWindow, session.session_id);
        }
    }
}