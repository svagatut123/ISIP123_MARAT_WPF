using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            LoadProfile();
        }

        private void LoadProfile()
        {
            if (Core.CurrentUser != null)
            {
                NameText.Text = Core.CurrentUser.DisplayName;
                LoginText.Text = Core.CurrentUser.Login;
                EmailText.Text = Core.CurrentUser.Email;

                if (Core.CurrentUser.RoleId == 3) RoleText.Text = "Администратор";
                else if (Core.CurrentUser.RoleId == 2) RoleText.Text = "Автор";
                else RoleText.Text = "Читатель";

                if (Core.CurrentUser.IsFrozen == true)
                {
                    FrozenPanel.Visibility = Visibility.Visible;
                    FreezeReasonText.Text = Core.CurrentUser.FreezeReason ?? "Причина не указана администратором платформы";
                }
            }
        }

        private void AppealUser_Click(object sender, RoutedEventArgs e)
        {
            var req = new UnfreezeRequests
            {
                UserId = Core.CurrentUser.UserId,
                TargetUserId = Core.CurrentUser.UserId,
                TargetBookId = null,
                Reason = "Не согласен с блокировкой аккаунта, прошу перепроверить действия",
                RequestDate = DateTime.Now,
                Status = "На рассмотрении"
            };

            Core.Context.UnfreezeRequests.Add(req);
            Core.Context.SaveChanges();
            MessageBox.Show("Апелляционная заявка на разморозку профиля успешно отправлена.");
        }
    }
}