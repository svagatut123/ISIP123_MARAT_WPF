using System.Windows;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private Game _game;

        public MainWindow()
        {
            InitializeComponent();
            _game = new Game();
            SetupEvents();
        }

        private void SetupEvents()
        {
            _game.OnLogUpdated += AddLog;
            _game.OnPlayerStatsUpdated += UpdatePlayerUI;
            _game.OnGameStateChanged += UpdateGameStateUI;
            _game.OnItemFound += ShowItemInfo;
        }

        private void AddLog(string message)
        {
            LogText.Text = message + "\n" + LogText.Text;
        }

        private void UpdatePlayerUI()
        {
            HealthText.Text = $"HP: {_game.Player.HP}/{_game.Player.MaxHP}";
            HealthBar.Value = _game.Player.HP;
            FloorText.Text = $"Этаж: {_game.TurnCount}";
            EquipText.Text = $"{_game.Player.CurrentWeapon.Name} | {_game.Player.CurrentArmor.Name}";
        }

        private void UpdateGameStateUI()
        {
            // Скрыть/показать кнопки в зависимости от состояния
            if (_game.IsCombat)
            {
                AttackBtn.Visibility = Visibility.Visible;
                DefendBtn.Visibility = Visibility.Visible;
                TakeBtn.Visibility = Visibility.Collapsed;
                DropBtn.Visibility = Visibility.Collapsed;

                EnemyDisplay.Text = _game.CurrentEnemy?.GetStatus() ?? "";
                EnemyDisplay.Visibility = Visibility.Visible;
                ChestDisplay.Visibility = Visibility.Collapsed;
            }
            else if (_game.IsChest && _game.CurrentChestItem != null)
            {
                AttackBtn.Visibility = Visibility.Collapsed;
                DefendBtn.Visibility = Visibility.Collapsed;
                TakeBtn.Visibility = Visibility.Visible;
                DropBtn.Visibility = Visibility.Visible;

                EnemyDisplay.Visibility = Visibility.Collapsed;
                ChestDisplay.Visibility = Visibility.Visible;
            }
            else
            {
                AttackBtn.Visibility = Visibility.Visible;
                DefendBtn.Visibility = Visibility.Visible;
                TakeBtn.Visibility = Visibility.Collapsed;
                DropBtn.Visibility = Visibility.Collapsed;
                EnemyDisplay.Text = "Исследование...";
                EnemyDisplay.Visibility = Visibility.Visible;
                ChestDisplay.Visibility = Visibility.Collapsed;
            }

            // Проверка конца игры
            if (_game.GameOver)
            {
                FinalFloor.Text = $"Достигнут этаж: {_game.TurnCount}";
                GameOverScreen.Visibility = Visibility.Visible;
                GameScreen.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowItemInfo(Item item)
        {
            ItemInfo.Text = item?.ToString() ?? "";
        }

        

        private void AttackBtn_Click(object sender, RoutedEventArgs e)
        {
            _game.PlayerAttack();
        }

        private void DefendBtn_Click(object sender, RoutedEventArgs e)
        {
            _game.PlayerDefend();
        }

        private void TakeBtn_Click(object sender, RoutedEventArgs e)
        {
            _game.TakeItem(true);
        }

        private void DropBtn_Click(object sender, RoutedEventArgs e)
        {
            _game.TakeItem(false);
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            GameOverScreen.Visibility = Visibility.Collapsed;
            LogText.Text = "";
            ItemInfo.Text = "";
        }
    }
}