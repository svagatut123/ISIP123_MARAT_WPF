using System.Windows.Controls;
using System.Linq;
namespace WpfApp1.Pages
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            LoadHistory();
        }

        private void LoadHistory()
        {
            HistoryGrid.ItemsSource = Core.Context.assembly.ToList();
        }
    }
}