using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            allMovies = Core.Context.Movies.ToList();
            MoviesList.ItemsSource = allMovies;
        }

        private void MovieBorder_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                var movie = border.DataContext as Movies;
                if (movie != null)
                {
                    mainWindow.MainFrame.Content = new MoviePage(mainWindow, movie.movie_id);
                }
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchBox.Text.Trim().ToLower();
            var filteredMovies = allMovies.Where(m => m.title.ToLower().Contains(searchTerm)).ToList();
            MoviesList.ItemsSource = filteredMovies;
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            if (SortComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите критерий сортировки");
                return;
            }

            string selectedSort = (SortComboBox.SelectedItem as ComboBoxItem).Content.ToString();

            if (selectedSort == "По названию")
            {
                allMovies = allMovies.OrderBy(m => m.title).ToList();
            }
            else if (selectedSort == "По рейтингу")
            {
                allMovies = allMovies.OrderByDescending(m => m.rating).ToList();
            }

            MoviesList.ItemsSource = allMovies;
        }
    }
}