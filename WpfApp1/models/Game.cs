using System;
using System.Collections.Generic;
using WpfApp1.Models;

namespace WpfApp1
{
    public class Game
    {
        public event Action<string> OnLogUpdated;
        public event Action OnPlayerStatsUpdated;
        public event Action OnGameStateChanged;
        public event Action<Item> OnItemFound;

        public Player Player { get; private set; }
        public Enemy CurrentEnemy { get; private set; }
        public Item CurrentChestItem { get; private set; }

        public bool IsCombat { get; private set; }
        public bool IsChest { get; private set; }
        public bool GameOver { get; private set; }
        public int TurnCount { get; private set; }

        private Fabrica _fabrica;
        private List<Weapon> _weapons;
        private List<Armor> _armors;

        public Game()
        {
            _fabrica = new Fabrica();
            _weapons = new List<Weapon>
            {
                new Weapon("Деревянный меч", 10, 10),
                new Weapon("Железный меч", 20, 20),
                new Weapon("Зачарованный лук", 25, 25),
                new Weapon("Аганим скипетр", 30, 30),
                new Weapon("Алмазный меч", 50, 50)
            };
            _armors = new List<Armor>
            {
                new Armor("Кожаная броня", 10, 10),
                new Armor("Кольчуга", 20, 20),
                new Armor("Железная броня", 30, 30),
                new Armor("Алмазная броня", 25, 25),
                new Armor("Незеритовая броня", 50, 50)
            };
        }

        public void StartNewGame()
        {
            Player = new Player(100);
            CurrentEnemy = null;
            CurrentChestItem = null;
            TurnCount = 0;
            GameOver = false;
            IsCombat = false;
            IsChest = false;

            Log("=== Игра началась! ===");
            UpdateStats();
            NextEncounter();
        }

        private void Log(string message)
        {
            OnLogUpdated?.Invoke(message);
        }

        private void UpdateStats()
        {
            OnPlayerStatsUpdated?.Invoke();
        }

        private void UpdateGameState()
        {
            OnGameStateChanged?.Invoke();
        }

        public void NextEncounter()
        {
            CurrentEnemy = null;
            CurrentChestItem = null;
            IsCombat = false;
            IsChest = false;

            TurnCount++;

            if (TurnCount % 10 == 0)
            {
                EncounterBoss();
            }
            else
            {
                if (RandomGenerator.NextBool())
                {
                    EncounterChest();
                }
                else
                {
                    EncounterEnemy();
                }
            }

            UpdateGameState();
        }

        private void EncounterEnemy()
        {
            CurrentEnemy = _fabrica.CreateRandomEnemy();
            IsCombat = true;
            Log($"⚔️ Появился: {CurrentEnemy.Name}!");
        }

        private void EncounterBoss()
        {
            CurrentEnemy = _fabrica.CreateRandomBoss();
            IsCombat = true;
            Log($"БОСС: {CurrentEnemy.Name}!");
        }

        private void EncounterChest()
        {
            IsChest = true;
            Log("Вы нашли сундук!");

            int itemType = RandomGenerator.Next(3);
            Item item = null;

            switch (itemType)
            {
                case 0:
                    item = new HealthPotion("Зелье лечения", 15);
                    Log("В сундуке: Зелье лечения");
                    break;
                case 1:
                    item = RandomGenerator.GetRandomItem(_weapons.ToArray());
                    Log($"В сундуке: {item.Name} (Атака +{((Weapon)item).Attack})");
                    break;
                case 2:
                    item = RandomGenerator.GetRandomItem(_armors.ToArray());
                    Log($"В сундуке: {item.Name} (Защита +{((Armor)item).Defense})");
                    break;
            }

            CurrentChestItem = item;
            OnItemFound?.Invoke(item);
        }


        public void PlayerAttack()
        {
            if (!IsCombat || CurrentEnemy == null) return;

            if (Player.Frozen)
            {
                Log("Вы заморожены! Пропуск хода.");
                Player.Frozen = false;
                EnemyTurn();
                return;
            }

            int damage = Player.CalculateDamage();
            CurrentEnemy.TakeDamage(damage);
            Log($"⚔️ Вы нанесли {damage} урона!");

            if (!CurrentEnemy.IsAlive)
            {
                Log($"{CurrentEnemy.Name} побеждён!");
                TurnCount++;
                Log($"Этаж: {TurnCount}");
                NextEncounter();
            }
            else
            {
                EnemyTurn();
            }

            UpdateStats();
            UpdateGameState();
        }

        public void PlayerDefend()
        {
            if (!IsCombat || CurrentEnemy == null) return;

            if (Player.Frozen)
            {
                Log("Вы заморожены! Пропуск хода.");
                Player.Frozen = false;
                EnemyTurn();
                return;
            }

            bool dodged = Player.TryDefend();
            if (dodged)
            {
                Log("Вы уклонились от атаки!");
            }
            else
            {
                Log("Вы встали в защиту.");
            }

            EnemyTurn(dodged);
            UpdateStats();
            UpdateGameState();
        }

        private void EnemyTurn(bool playerDodged = false)
        {
            if (CurrentEnemy == null || !CurrentEnemy.IsAlive) return;

            if (playerDodged) return;

            int damage = CurrentEnemy.CalculateDamage(Player);
            int finalDamage = Player.CalculateBlockedDamage(damage);

            Player.TakeDamage(finalDamage);
            Log($" {CurrentEnemy.Name} наносит {finalDamage} урона! (ваше HP: {Player.HP})");

            CurrentEnemy.ApplyEffectDamage(Player);

            if (Player.HP <= 0)
            {
                Player.HP = 0;
                GameOver = true;
                Log(" ВЫ ПОГИБЛИ! Игра окончена.");
            }

            UpdateStats();
        }

        public void TakeItem(bool take)
        {
            if (CurrentChestItem == null) return;

            if (take)
            {
                CurrentChestItem.ApplyEffect(Player);
                Log($" Вы взяли: {CurrentChestItem.Name}");
            }
            else
            {
                Log($" Вы выбросили: {CurrentChestItem.Name}");
            }

            if (CurrentChestItem is HealthPotion)
            {
                UpdateStats();
            }

            CurrentChestItem = null;
            IsChest = false;
            NextEncounter();
        }

        public void Restart()
        {
            StartNewGame();
        }
    }
}