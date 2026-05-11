using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class BookDetailsPage : Page
    {
        private Books _currentBook;

        public BookDetailsPage(Books book)
        {
            InitializeComponent();
            _currentBook = book;
            DataContext = _currentBook;

            // заполняем данные
            TitleTxt.Text = book.Title;
            AuthorTxt.Text = "автор: " + book.Users.DisplayName;
            DescTxt.Text = book.Description;
            ContentTxt.Text = book.Content;

            LoadReviews();
        }

        private void LoadReviews()
        {
            ReviewsList.ItemsSource = Core.Context.Reviews.Where(r => r.BookId == _currentBook.BookId).ToList();
        }

        private void AddToList_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null)
            {
                MessageBox.Show("пожалуйста, войдите в систему, чтобы сохранять книги");
                return;
            }

            // получаем текст выбранного статуса (В планах, Читаю и т.д.)
            string selectedStatus = (StatusSelectCombo.SelectedItem as ComboBoxItem).Content.ToString();

            // проверяем, нет ли уже этой книги в списках пользователя
            var existingRecord = Core.Context.ReadingLists.FirstOrDefault(rl =>
                rl.UserId == Core.CurrentUser.UserId &&
                rl.BookId == _currentBook.BookId);

            if (existingRecord != null)
            {
                // если книга уже есть, просто обновим её статус
                existingRecord.Status = selectedStatus;
                MessageBox.Show("статус книги обновлен на: " + selectedStatus);
            }
            else
            {
                // если книги нет, создаем новую запись
                ReadingLists newItem = new ReadingLists()
                {
                    UserId = Core.CurrentUser.UserId,
                    BookId = _currentBook.BookId,
                    Status = selectedStatus
                };
                Core.Context.ReadingLists.Add(newItem);
                MessageBox.Show("книга добавлена в список: " + selectedStatus);
            }

            Core.Context.SaveChanges();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null)
            {
                MessageBox.Show("авторизуйтесь, чтобы отправить жалобу");
                return;
            }

            // создаем объект жалобы
            Complaints newComplaint = new Complaints()
            {
                UserId = Core.CurrentUser.UserId,
                TargetId = _currentBook.BookId,
                TargetType = "Книга",
                Reason = "нарушение правил",

                // вот здесь мы исправляем ошибку с датой:
                ComplaintDate = System.DateTime.Now
            };

            try
            {
                Core.Context.Complaints.Add(newComplaint);
                Core.Context.SaveChanges();
                MessageBox.Show("жалоба успешно отправлена");
            }
            catch (Exception ex)
            {
                // если ошибка осталась, это поможет увидеть детали
                MessageBox.Show("ошибка при сохранении: " + ex.Message);
            }
        }
    }
}