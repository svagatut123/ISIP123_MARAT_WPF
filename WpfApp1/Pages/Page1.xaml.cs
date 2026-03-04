using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class Page1 : Page
    {
        private List<basepart> _allProducts;
        private List<basepart> _currentBuild = new List<basepart>();

        public Page1()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _allProducts = Core.Context.basepart.ToList();
            ProductsGrid.ItemsSource = _allProducts;

            var types = Core.Context.parttype.ToList();
            TypeFilter.Items.Add(new ComboBoxItem { Content = "Все типы" });
            foreach (var t in types)
            {
                TypeFilter.Items.Add(new ComboBoxItem { Content = t.name });
            }

            var manufacturers = Core.Context.manufacturer.ToList();
            ManFilter.Items.Add(new ComboBoxItem { Content = "Все производители" });
            foreach (var m in manufacturers)
            {
                ManFilter.Items.Add(new ComboBoxItem { Content = m.name });
            }
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }
        private void RemoveFromBuild_Click(object sender, RoutedEventArgs e)
        {
            if (BuildList.SelectedItem is basepart selectedProduct)
            {
                _currentBuild.Remove(selectedProduct);
                UpdateBuildUI();
                CheckCompatibility();
            }
            else
            {
                MessageBox.Show("Выберите компонент для удаления из списка");
            }
        }
        private void ApplyFilters()
        {
            string search = SearchBox.Text.ToLower();
            string typeFilter = (TypeFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            string manFilter = (ManFilter.SelectedItem as ComboBoxItem)?.Content.ToString();

            var query = _allProducts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.name.ToLower().Contains(search));

            if (typeFilter != null && typeFilter != "Все типы")
                query = query.Where(p => p.parttype.name == typeFilter);

            if (manFilter != null && manFilter != "Все производители")
                query = query.Where(p => p.manufacturer.name == manFilter);

            ProductsGrid.ItemsSource = query.ToList();
        }

        private void AddToBuild_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is basepart selectedProduct)
            {
                if (_currentBuild.Any(p => p.parttypeid == selectedProduct.parttypeid))
                {
                    MessageBox.Show($"Компонент типа '{selectedProduct.parttype.name}' уже выбран.", "Ошибка");
                    return;
                }

                _currentBuild.Add(selectedProduct);
                UpdateBuildUI();
                CheckCompatibility();
            }
            else
            {
                MessageBox.Show("Выберите товар из списка");
            }
        }

        private void ClearBuild_Click(object sender, RoutedEventArgs e)
        {
            _currentBuild.Clear();
            UpdateBuildUI();
            ErrorText.Text = "";
        }

        private void UpdateBuildUI()
        {
            BuildList.ItemsSource = null;
            BuildList.ItemsSource = _currentBuild;

            decimal total = _currentBuild.Sum(p => p.price);
            TotalPriceText.Text = $"{total} руб.";
        }

        private void CheckCompatibility()
        {
            ErrorText.Text = "";
            List<string> errors = new List<string>();

            var cpu = _currentBuild.FirstOrDefault(p => p.parttype.name == "CPU");
            var mobo = _currentBuild.FirstOrDefault(p => p.parttype.name == "Motherboard");
            var cooler = _currentBuild.FirstOrDefault(p => p.parttype.name == "ProcessorCooler");
            var ram = _currentBuild.FirstOrDefault(p => p.parttype.name == "RAM");
            var psu = _currentBuild.FirstOrDefault(p => p.parttype.name == "PowerSupply");
            var gpu = _currentBuild.FirstOrDefault(p => p.parttype.name == "GPU");
            var casePc = _currentBuild.FirstOrDefault(p => p.parttype.name == "Case");

            if (cpu != null && mobo != null)
            {
                var cpuData = Core.Context.cpu.FirstOrDefault(c => c.id == cpu.id);
                var moboData = Core.Context.motherboard.FirstOrDefault(m => m.id == mobo.id);

                if (cpuData != null && moboData != null)
                {
                    if (cpuData.socketid != moboData.socketid)
                    {
                        var cpuSocket = Core.Context.socket.FirstOrDefault(s => s.id == cpuData.socketid);
                        var moboSocket = Core.Context.socket.FirstOrDefault(s => s.id == moboData.socketid);
                        errors.Add($"Несовместимы сокет процессора ({cpuSocket?.name} ) и материнской платы ( {moboSocket?.name})");
                    }
                }
            }

            if (cpu != null && cooler != null)
            {
                var cpuData = Core.Context.cpu.FirstOrDefault(c => c.id == cpu.id);
                var coolerSupported = Core.Context.socketprocessorcooler
                    .Any(spc => spc.processorcoolerid == cooler.id && spc.socketid == cpuData.socketid);

                if (!coolerSupported)
                    errors.Add("Кулер не поддерживает сокет процессора");
            }

            if (mobo != null && casePc != null)
            {
                var moboData = Core.Context.motherboard.FirstOrDefault(m => m.id == mobo.id);
                var caseSupported = Core.Context.boardformfactorcase
                    .Any(bfc => bfc.caseid == casePc.id && bfc.formfactorid == moboData.formfactorid);

                if (!caseSupported)
                    errors.Add("Материнская плата не подходит к корпусу по форм-фактору");
            }

            if (mobo != null && ram != null)
            {
                var moboData = Core.Context.motherboard.FirstOrDefault(m => m.id == mobo.id);
                var ramData = Core.Context.ram.FirstOrDefault(r => r.id == ram.id);

                if (moboData != null && ramData != null)
                {
                    if (moboData.memorytypeid != ramData.memorytypeid)
                    {
                        var moboMem = Core.Context.memorytype.FirstOrDefault(m => m.id == moboData.memorytypeid);
                        var ramMem = Core.Context.memorytype.FirstOrDefault(m => m.id == ramData.memorytypeid);
                        errors.Add($"Тип памяти не совпадает: Плата ({moboMem?.name}), ОЗУ ({ramMem?.name})");
                    }
                }
            }

            if (psu != null && gpu != null)
            {
                var psuData = Core.Context.powersupply.FirstOrDefault(p => p.id == psu.id);
                var gpuData = Core.Context.gpu.FirstOrDefault(g => g.id == gpu.id);

                if (psuData != null && gpuData != null)
                {
                    int totalPowerNeed = (gpuData.recommendpower ?? 0) + 150;
                    if (psuData.power < totalPowerNeed)
                        errors.Add($"Блок питания слишком слабый! Нужно минимум {totalPowerNeed}W, у вас {psuData.power}W");
                }
            }

            if (errors.Count > 0)
            {
                ErrorText.Text = "Ошибки совместимости:\n- " + string.Join("\n- ", errors);
            }
        }

        private void SaveBuild_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBuild.Count == 0)
            {
                MessageBox.Show("Сборка пуста");
                return;
            }

            var page3 = new Page3(_currentBuild);
            NavigationService.Navigate(page3);
        }
    }
}