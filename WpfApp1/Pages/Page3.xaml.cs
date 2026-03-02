using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1.Pages
{
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
            AssembliesGrid.ItemsSource = Core.Context.assembly_.ToList();
        }

        private void ShowDetails(object sender, MouseButtonEventArgs e)
        {
            if (AssembliesGrid.SelectedItem is assembly_ asm)
            {
                var parts = Core.Context.partassembly_.Where(pa => pa.assemblyid == asm.id)
                    .Select(pa => pa.basepart_).ToList();

                string msg = $"Сборка: {asm.name}\nАвтор: {asm.author}\n\n";
                foreach (var p in parts) msg += $"{p.name} - {p.price:N0} ₽\n";

                MessageBox.Show(msg);
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            var wnd = Window.GetWindow(this) as MainWindow;
            wnd?.MainFrame.Navigate(new Page1());
        }
    }
}