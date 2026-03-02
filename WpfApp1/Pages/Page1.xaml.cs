using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1.Pages
{
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
            LoadFilters();
            LoadParts();
            UpdateSelected();
        }

        private void LoadFilters()
        {
            TypeFilter.ItemsSource = Core.Context.parttype_.ToList();
            TypeFilter.DisplayMemberPath = "name";
            TypeFilter.SelectedValuePath = "id";

            ManufFilter.ItemsSource = Core.Context.manufacturer_.ToList();
            ManufFilter.DisplayMemberPath = "name";
            ManufFilter.SelectedValuePath = "id";
        }

        private void LoadParts()
        {
            var query = Core.Context.basepart_.Include("manufacturer").AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                query = query.Where(p => p.name.Contains(SearchBox.Text));

            if (TypeFilter.SelectedValue != null)
                query = query.Where(p => p.parttypeid == (int)TypeFilter.SelectedValue);

            if (ManufFilter.SelectedValue != null)
                query = query.Where(p => p.manufacturerid == (int)ManufFilter.SelectedValue);

            PartsGrid.ItemsSource = query.ToList();
        }

        private void SearchChanged(object sender, TextChangedEventArgs e) => LoadParts();
        private void FilterChanged(object sender, SelectionChangedEventArgs e) => LoadParts();

        private void PartsGridDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PartsGrid.SelectedItem is basepart_ part)
            {
                BuildData.SelectedParts[part.parttypeid] = part;
                UpdateSelected();
            }
        }

        private void UpdateSelected()
        {
            SelectedList.ItemsSource = BuildData.SelectedParts.ToList();
            TotalText.Text = $"Итого: {BuildData.GetTotalPrice():N0} ₽";
        }

        private void CheckClick(object sender, RoutedEventArgs e)
        {
            var errors = BuildData.CheckCompatibility();
            MessageBox.Show(errors.Count == 0 ? "Все совместимо" : string.Join("\n", errors), "Результат");
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            var wnd = Window.GetWindow(this) as MainWindow;
            wnd?.SaveClick(this, e);
        }
    }
}