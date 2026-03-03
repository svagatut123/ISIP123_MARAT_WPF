using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class Page3 : Page
    {
        private List<basepart> _buildItems;

        public Page3(List<basepart> items)
        {
            InitializeComponent();
            _buildItems = items;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = BuildNameBox.Text.Trim();
            string author = AuthorBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(author))
            {
                MessageBox.Show("Заполните все поля", "Ошибка");
                return;
            }

            try
            {
                var newBuild = new assembly
                {
                    name = name,
                    author = author
                };

                Core.Context.assembly.Add(newBuild);
                Core.Context.SaveChanges();

                foreach (var item in _buildItems)
                {
                    Core.Context.partassembly.Add(new partassembly
                    {
                        partid = item.id,
                        assemblyid = newBuild.id
                    });
                }
                Core.Context.SaveChanges();

                MessageBox.Show("Сборка успешно сохранена!", "Успех");
                NavigationService.Navigate(new Page2());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка БД: {ex.Message}", "Ошибка");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}