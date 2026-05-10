using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1;

namespace WpfApp1.Pages
{
    public partial class BookPage : Page
    {
        private Books currentBook;

        public BookPage(Books book)
        {
            InitializeComponent();
            currentBook = book;
            LoadBookInfo();
        }

        private void LoadBookInfo()
        {
            try
            {
                TxtTitle.Text = currentBook.Title;
                TxtAuthor.Text = "Автор: " + currentBook.Users.DisplayName;
                TxtDesc.Text = currentBook.Description ?? "Описание отсутствует";

                // Загрузка отзывов
                var reviews = Core.Context.Reviews
                    .Where(r => r.BookId == currentBook.BookId)
                    .ToList();
                GridReviews.ItemsSource = reviews;

                // Показываем кнопку заморозки только для администратора
                if (Core.CurrentUser.Roles.RoleName == "Администратор")
                {
                    BtnFreeze.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки информации о книге: " + ex.Message);
            }
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentBook.Content))
            {
                MessageBox.Show("Содержимое книги отсутствует");
                return;
            }

            // Показываем первые 500 символов
            string preview = currentBook.Content.Length > 500
                ? currentBook.Content.Substring(0, 500) + "..."
                : currentBook.Content;

            MessageBox.Show(preview, "Чтение книги", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAddToList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CmbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Выберите статус из списка");
                    return;
                }

                var status = ((ComboBoxItem)CmbStatus.SelectedItem).Content.ToString();

                // Проверяем, есть ли уже книга в списках
                var existing = Core.Context.ReadingLists
                    .FirstOrDefault(r => r.UserId == Core.CurrentUser.UserId && r.BookId == currentBook.BookId);

                if (existing != null)
                {
                    // Обновляем статус
                    existing.Status = status;
                }
                else
                {
                    // Добавляем новую запись
                    var readingList = new ReadingLists
                    {
                        UserId = Core.CurrentUser.UserId,
                        BookId = currentBook.BookId,
                        Status = status
                    };
                    Core.Context.ReadingLists.Add(readingList);
                }

                Core.Context.SaveChanges();
                MessageBox.Show("Книга добавлена в список: " + status);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении в список: " + ex.Message);
            }
        }

        private void BtnComplain_Click(object sender, RoutedEventArgs e)
        {
            string reason = ShowInputDialog("Укажите причину жалобы на книгу:");

            if (!string.IsNullOrEmpty(reason) && reason.Trim().Length > 0)
            {
                try
                {
                    var complaint = new Complaints
                    {
                        UserId = Core.CurrentUser.UserId,
                        TargetType = "Книга",
                        TargetId = currentBook.BookId,
                        Reason = reason,
                        ComplaintDate = DateTime.Now
                    };

                    Core.Context.Complaints.Add(complaint);
                    Core.Context.SaveChanges();
                    MessageBox.Show("Жалоба отправлена");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при отправке жалобы: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Жалоба не отправлена: причина не указана");
            }
        }

        private void BtnComplainAuthor_Click(object sender, RoutedEventArgs e)
        {
            string reason = ShowInputDialog("Укажите причину жалобы на автора:");

            if (!string.IsNullOrEmpty(reason) && reason.Trim().Length > 0)
            {
                try
                {
                    var complaint = new Complaints
                    {
                        UserId = Core.CurrentUser.UserId,
                        TargetType = "Пользователь",
                        TargetId = currentBook.AuthorId,
                        Reason = reason,
                        ComplaintDate = DateTime.Now
                    };

                    Core.Context.Complaints.Add(complaint);
                    Core.Context.SaveChanges();
                    MessageBox.Show("Жалоба на автора отправлена");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при отправке жалобы: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Жалоба не отправлена: причина не указана");
            }
        }

        private void BtnFreeze_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы действительно хотите заморозить эту книгу?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    currentBook.IsFrozen = true;
                    Core.Context.SaveChanges();
                    MessageBox.Show("Книга заморожена");
                    BtnFreeze.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при заморозке книги: " + ex.Message);
                }
            }
        }

        // Свой метод ввода вместо Interaction.InputBox
        private string ShowInputDialog(string prompt)
        {
            Window window = new Window
            {
                Title = "Ввод текста",
                Width = 400,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };

            StackPanel panel = new StackPanel
            {
                Margin = new Thickness(15)
            };

            TextBlock textBlock = new TextBlock
            {
                Text = prompt,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 10)
            };

            TextBox textBox = new TextBox
            {
                Height = 60,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            Button okButton = new Button
            {
                Content = "OK",
                Width = 75,
                Height = 25,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            okButton.Click += (s, e) => window.Close();

            panel.Children.Add(textBlock);
            panel.Children.Add(textBox);
            panel.Children.Add(okButton);

            window.Content = panel;
            window.ShowDialog();

            return textBox.Text;
        }
    }
}