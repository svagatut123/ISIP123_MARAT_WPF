using System.Windows;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private Game game;
        private bool gameStarted = false;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
            SetupEvents();

            AttackBtn.Visibility = Visibility.Collapsed;
            DefendBtn.Visibility = Visibility.Collapsed;
            TakeBtn.Visibility = Visibility.Collapsed;
            DropBtn.Visibility = Visibility.Collapsed;
        }

        private void SetupEvents()
        {
            game.OnLogUpdated += AddLog;
            game.OnPlayerStatsUpdated += UpdatePlayerUI;
            game.OnGameStateChanged += UpdateGameStateUI;
            game.OnItemFound += ShowItemInfo;
        }

        private void AddLog(string message)
        {
            LogText.Text = message + "\n" + LogText.Text;
        }

        private void UpdatePlayerUI()
        {
            if (game.Player == null) return;

            HealthText.Text = $"HP: {game.Player.HP}/{game.Player.MaxHP}";
            HealthBar.Value = game.Player.HP;
            FloorText.Text = $"уровень: {game.TurnCount}";
            EquipText.Text = $"{game.Player.CurrentWeapon.Name}, {game.Player.CurrentArmor.Name}";
        }

        private void UpdateGameStateUI()
        {
            if (!gameStarted) return;

            if (game.IsCombat)
            {
                AttackBtn.Visibility = Visibility.Visible;
                DefendBtn.Visibility = Visibility.Visible;
                TakeBtn.Visibility = Visibility.Collapsed;
                DropBtn.Visibility = Visibility.Collapsed;
                StartBtn.Visibility = Visibility.Collapsed;

                EnemyDisplay.Text = game.CurrentEnemy?.GetStatus() ?? "";
                EnemyDisplay.Visibility = Visibility.Visible;
                ChestDisplay.Visibility = Visibility.Collapsed;
            }
            else if (game.IsChest && game.CurrentChestItem != null)
            {
                AttackBtn.Visibility = Visibility.Collapsed;
                DefendBtn.Visibility = Visibility.Collapsed;
                TakeBtn.Visibility = Visibility.Visible;
                DropBtn.Visibility = Visibility.Visible;
                StartBtn.Visibility = Visibility.Collapsed;

                EnemyDisplay.Visibility = Visibility.Collapsed;
                ChestDisplay.Visibility = Visibility.Visible;
            }
            else
            {
                AttackBtn.Visibility = Visibility.Visible;
                DefendBtn.Visibility = Visibility.Visible;
                TakeBtn.Visibility = Visibility.Collapsed;
                DropBtn.Visibility = Visibility.Collapsed;
                StartBtn.Visibility = Visibility.Collapsed;

                EnemyDisplay.Text = "Исследование...";
                EnemyDisplay.Visibility = Visibility.Visible;
                ChestDisplay.Visibility = Visibility.Collapsed;
            }

            if (game.GameOver)
            {
                FinalFloor.Text = $"Достигнут уровень: {game.TurnCount}";
                GameOverScreen.Visibility = Visibility.Visible;
            }
        }

        private void ShowItemInfo(Item item)
        {
            ItemInfo.Text = item?.ToString() ?? "";
        }


        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            gameStarted = true;
            StartBtn.Visibility = Visibility.Collapsed;
            AttackBtn.Visibility = Visibility.Visible;
            DefendBtn.Visibility = Visibility.Visible;

            game.StartNewGame();
        }

        private void AttackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!gameStarted) return;
            game.PlayerAttack();
        }

        private void DefendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!gameStarted) return;
            game.PlayerDefend();
        }

        private void TakeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!gameStarted) return;
            game.TakeItem(true);
        }

        private void DropBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!gameStarted) return;
            game.TakeItem(false);
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            GameOverScreen.Visibility = Visibility.Collapsed;
            LogText.Text = "";
            ItemInfo.Text = "";
            EnemyDisplay.Text = "";

            gameStarted = false;
            StartBtn.Visibility = Visibility.Visible;
            AttackBtn.Visibility = Visibility.Collapsed;
            DefendBtn.Visibility = Visibility.Collapsed;
            TakeBtn.Visibility = Visibility.Collapsed;
            DropBtn.Visibility = Visibility.Collapsed;
        }
    }
}