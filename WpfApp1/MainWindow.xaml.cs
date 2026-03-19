using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private Game game;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
        }

        private void UpdateUI()
        {
            HealthText.Text = $"HP: {game.Player.HP}/{game.Player.MaxHP}";
            HealthBar.Value = game.Player.HP;
            FloorText.Text = $"Этаж: {game.TurnCount}";
            EquipText.Text = $"{game.Player.CurrentWeapon.Name} | {game.Player.CurrentArmor.Name}";

            HealthBar.Foreground = game.Player.HP < 30 ? Brushes.Red :
                                   game.Player.HP < 60 ? Brushes.Orange : Brushes.Green;

            int start = Math.Max(0, game.Log.Count - 10);
            int count = Math.Min(10, game.Log.Count);
            LogText.Text = string.Join("\n", game.Log.GetRange(start, count));

            if (game.IsCombat && game.CurrentEnemy != null)
            {
                AttackBtn.Visibility = Visibility.Visible;
                DefendBtn.Visibility = Visibility.Visible;
                TakeBtn.Visibility = Visibility.Collapsed;
                DropBtn.Visibility = Visibility.Collapsed;

                EnemyImage.Visibility = Visibility.Visible;
                ChestImage.Visibility = Visibility.Collapsed;

                try
                {
                    EnemyImage.Source = new BitmapImage(
                        new Uri(game.CurrentEnemy.ImagePath, UriKind.Relative));
                }
                catch
                {
                    EnemyImage.Source = null;
                }

                EnemyText.Text = game.CurrentEnemy.Name +
                    (game.CurrentEnemy.IsBoss ? " (БОСС)" : "");
                EnemyHpText.Text = $"HP: {game.CurrentEnemy.HP}/{game.CurrentEnemy.MaxHP}";
                ItemInfo.Text = "—";
            }
            else if (game.IsChest && game.CurrentChestItem != null)
            {
                AttackBtn.Visibility = Visibility.Collapsed;
                DefendBtn.Visibility = Visibility.Collapsed;
                TakeBtn.Visibility = Visibility.Visible;
                DropBtn.Visibility = Visibility.Visible;

                EnemyImage.Visibility = Visibility.Collapsed;
                ChestImage.Visibility = Visibility.Visible;
                EnemyText.Text = "Сундук открыт!";
                EnemyHpText.Text = "";
                ItemInfo.Text = game.CurrentChestItem.ToString();
            }
            else
            {
                AttackBtn.Visibility = Visibility.Visible;
                DefendBtn.Visibility = Visibility.Visible;
                TakeBtn.Visibility = Visibility.Collapsed;
                DropBtn.Visibility = Visibility.Collapsed;

                EnemyImage.Visibility = Visibility.Collapsed;
                ChestImage.Visibility = Visibility.Collapsed;
                EnemyText.Text = "Исследование этажа...";
                EnemyHpText.Text = "";
                ItemInfo.Text = "—";
            }

            if (game.GameOver)
            {
                FinalFloor.Text = $"Достигнут этаж: {game.TurnCount}";
                GameOverScreen.Visibility = Visibility.Visible;
                GameScreen.Visibility = Visibility.Collapsed;
            }
        }


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartScreen.Visibility = Visibility.Collapsed;
            GameScreen.Visibility = Visibility.Visible;
            game.StartNewGame();
            UpdateUI();
        }

        private void AttackBtn_Click(object sender, RoutedEventArgs e)
        {
            game.PlayerAttack();
            UpdateUI();
        }

        private void DefendBtn_Click(object sender, RoutedEventArgs e)
        {
            game.PlayerDefend();
            UpdateUI();
        }

        private void TakeBtn_Click(object sender, RoutedEventArgs e)
        {
            game.TakeItem(true);
            UpdateUI();
        }

        private void DropBtn_Click(object sender, RoutedEventArgs e)
        {
            game.TakeItem(false);
            UpdateUI();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            GameOverScreen.Visibility = Visibility.Collapsed;
            StartScreen.Visibility = Visibility.Visible;
            GameScreen.Visibility = Visibility.Collapsed;
        }
    }
}