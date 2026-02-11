using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    public partial class MoviesPage : Page
    {
        private List<Movie> allMovies;

        public MoviesPage()
        {
            InitializeComponent();
            LoadMovies();
            SortBox.SelectedIndex = 0;
        }

        private void LoadMovies()
        {
            allMovies = Core.Context.Movie.ToList();
            MoviesList.ItemsSource = allMovies;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            var filtered = allMovies.Where(m =>
                m.Title.ToLower().Contains(query)).ToList();
            MoviesList.ItemsSource = filtered;
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortBox.SelectedIndex == 0)
            {
                MoviesList.ItemsSource = allMovies.OrderBy(m => m.Title).ToList();
            }
            else if (SortBox.SelectedIndex == 1)
            {
                MoviesList.ItemsSource = allMovies.OrderByDescending(m => m.Rating).ToList();
            }
        }

        private void MovieDetail_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int movieId = Convert.ToInt32(button.Tag);

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Content = new MovieDetailPage(movieId);
            }
        }
    }
}