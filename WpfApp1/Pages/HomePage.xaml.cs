using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp1.Pages
{
    public partial class HomePage : Page
    {
        private MainWindow mainWindow;
        private List<Movies> allMovies;

        public HomePage(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
            LoadMovies();
        }

        private void LoadMovies()
        {
            MoviesPanel.Children.Clear();
            allMovies = Core.Context.Movies.ToList();

            foreach (var movie in allMovies)
            {
                AddMovieToPanel(movie);
            }
        }

        private void AddMovieToPanel(Movies movie)
        {
            Border movieBorder = new Border();
            movieBorder.BorderBrush = Brushes.Black;
            movieBorder.BorderThickness = new Thickness(1);
            movieBorder.Margin = new Thickness(5);
            movieBorder.Width = 200;
            movieBorder.Height = 300;
            movieBorder.MouseLeftButtonDown += (s, e) => Movie_Click(movie);

            StackPanel moviePanel = new StackPanel();
            moviePanel.Margin = new Thickness(5);

            if (!string.IsNullOrEmpty(movie.image_path))
            {
                Image movieImage = new Image();
                movieImage.Source = new BitmapImage(new Uri(movie.image_path));
                movieImage.Width = 180;
                movieImage.Height = 200;
                moviePanel.Children.Add(movieImage);
            }

            TextBlock titleText = new TextBlock();
            titleText.Text = movie.tittle;
            titleText.FontWeight = FontWeights.Bold;
            titleText.Height = 40;
            moviePanel.Children.Add(titleText);

            TextBlock ratingText = new TextBlock();
            ratingText.Text = $"Рейтинг: {movie.rating}";
            moviePanel.Children.Add(ratingText);

            TextBlock dateText = new TextBlock();
            dateText.Text = $"Дата: {movie.release_date}";
            moviePanel.Children.Add(dateText);

            movieBorder.Child = moviePanel;
            MoviesPanel.Children.Add(movieBorder);
        }

        private void Movie_Click(Movies movie)
        {
            mainWindow.MainFrame.Content = new MoviePage(mainWindow, movie.movie_id);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchBox.Text.Trim().ToLower();
            MoviesPanel.Children.Clear();

            var filteredMovies = allMovies.Where(m =>
                m.tittle.ToLower().Contains(searchTerm)).ToList();

            foreach (var movie in filteredMovies)
            {
                AddMovieToPanel(movie);
            }
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            if (SortComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите критерий сортировки");
                return;
            }

            string selectedSort = (SortComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            MoviesPanel.Children.Clear();

            if (selectedSort == "По названию")
            {
                allMovies = allMovies.OrderBy(m => m.tittle).ToList();
            }
            else if (selectedSort == "По рейтингу")
            {
                allMovies = allMovies.OrderByDescending(m => m.rating).ToList();
            }

            foreach (var movie in allMovies)
            {
                AddMovieToPanel(movie);
            }
        }
    }
}