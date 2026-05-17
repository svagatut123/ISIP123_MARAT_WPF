using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class AdminPage : Page
    {
        public class UserDisplayModel
        {
            public int UserId { get; set; }
            public string Login { get; set; }
            public string DisplayName { get; set; }
            public string RoleText { get; set; }
        }

        public AdminPage()
        {
            InitializeComponent();
            LoadAdminData();
        }

        private void LoadAdminData()
        {
            if (Core.Context == null) return;

            try
            {
                Core.Context.ChangeTracker.Entries().ToList().ForEach(e => e.Reload());

                var rawUsers = Core.Context.Users.ToList();
                var displayList = new List<UserDisplayModel>();

                foreach (var u in rawUsers)
                {
                    displayList.Add(new UserDisplayModel
                    {
                        UserId = u.UserId,
                        Login = u.Login,
                        DisplayName = u.DisplayName,
                        RoleText = GetUserRoleValue(u) 
                    });
                }

                UsersGrid.ItemsSource = displayList;
                ComplaintsGrid.ItemsSource = Core.Context.Complaints.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления данных: " + ex.Message);
            }
        }
        private PropertyInfo FindRoleProperty(object user)
        {
            if (user == null) return null;

            return user.GetType().GetProperties().FirstOrDefault(p =>
                (p.Name.Contains("Role") || p.Name.Contains("Status") || p.Name.Contains("Type") || p.Name.Contains("Position")) &&
                (p.PropertyType == typeof(string) || p.PropertyType == typeof(int) || p.PropertyType == typeof(int?)) &&
                !p.PropertyType.IsGenericType);
        }

        private string GetUserRoleValue(object user)
        {
            var prop = FindRoleProperty(user);
            if (prop == null) return "Пользователь";

            var value = prop.GetValue(user);
            if (value == null) return "Пользователь";

            if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
            {
                int roleId = Convert.ToInt32(value);
                if (roleId == 3) return "Администратор";
                if (roleId == 2) return "Автор";
                return "Пользователь";
            }

            return value.ToString();
        }

        private void RoleCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            if (combo == null || combo.Tag == null || combo.SelectedItem == null) return;

            int userId = (int)combo.Tag;
            string newRole = (combo.SelectedItem as ComboBoxItem).Content.ToString();

            var user = Core.Context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                var prop = FindRoleProperty(user);

                if (prop != null && prop.CanWrite)
                {
                    if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                    {
                        int roleId = newRole == "Администратор" ? 3 : (newRole == "Автор" ? 2 : 1);
                        prop.SetValue(user, roleId);
                    }
                    else
                    {
                        prop.SetValue(user, newRole);
                    }

                    Core.Context.SaveChanges();

                    Dispatcher.BeginInvoke(new Action(() => LoadAdminData()));
                }
                else
                {
                    MessageBox.Show("Не удалось определить доступное для записи свойство роли в БД.");
                }
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            int userId = (int)((Button)sender).Tag;
            var user = Core.Context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                string manualPassword = ShowInputDialog($"Введите новый пароль для {user.Login}:", "Смена пароля вручную");

                if (manualPassword == null) return; // Отмена

                if (string.IsNullOrWhiteSpace(manualPassword))
                {
                    MessageBox.Show("Пароль не может быть пустым!");
                    return;
                }

                user.Password = manualPassword.Trim();
                Core.Context.SaveChanges();

                MessageBox.Show($"Пароль для пользователя {user.Login} успешно изменен!", "Успех");
                LoadAdminData();
            }
        }
        private string ShowInputDialog(string text, string title)
        {
            Window inputWindow = new Window()
            {
                Title = title,
                Width = 350,
                Height = 160,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false
            };

            StackPanel sp = new StackPanel() { Margin = new Thickness(15) };
            TextBlock lbl = new TextBlock() { Text = text, Margin = new Thickness(0, 0, 0, 10), FontWeight = FontWeights.SemiBold };
            TextBox txt = new TextBox() { Height = 25, VerticalContentAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 0, 15) };

            StackPanel buttonsPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            Button btnOk = new Button() { Content = "Сохранить", Width = 80, Height = 25, IsDefault = true, Margin = new Thickness(0, 0, 10, 0) };
            Button btnCancel = new Button() { Content = "Отмена", Width = 80, Height = 25, IsCancel = true };

            string result = null;
            btnOk.Click += (s, e) => { result = txt.Text; inputWindow.Close(); };
            btnCancel.Click += (s, e) => { inputWindow.Close(); };

            buttonsPanel.Children.Add(btnOk);
            buttonsPanel.Children.Add(btnCancel);
            sp.Children.Add(lbl);
            sp.Children.Add(txt);
            sp.Children.Add(buttonsPanel);
            inputWindow.Content = sp;

            inputWindow.ShowDialog();
            return result;
        }

        private void UnfreezeBook_Click(object sender, RoutedEventArgs e)
        {
            if (ComplaintsGrid.SelectedItem is Complaints selectedComplaint)
            {
                if (selectedComplaint.TargetType == "Appeal")
                {
                    var book = Core.Context.Books.FirstOrDefault(b => b.BookId == selectedComplaint.TargetId);
                    if (book != null) book.IsFrozen = false;

                    Core.Context.Complaints.Remove(selectedComplaint);
                    Core.Context.SaveChanges();
                    MessageBox.Show("Произведение разморожено.");
                    LoadAdminData();
                }
            }
        }
    }
}