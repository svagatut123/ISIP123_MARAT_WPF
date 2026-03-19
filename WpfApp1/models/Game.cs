using System;
using System.Collections.Generic;
using WpfApp1.Models;

namespace WpfApp1
{
    public class Game
    {

        public Player Player { get; private set; }
        public Enemy CurrentEnemy { get; private set; }
        public Item CurrentChestItem { get; private set; }

        public bool IsCombat { get; private set; }
        public bool IsChest { get; private set; }
        public bool GameOver { get; private set; }
        public int TurnCount { get; private set; }
        public List<string> Log { get; private set; }  

        private Fabrica fabrica;
        private List<Weapon> weapons;
        private List<Armor> armors;

        public Game()
        {
            fabrica = new Fabrica();
            Log = new List<string>();
            weapons = new List<Weapon>
            {
                new Weapon("Деревянный меч", 5, 5),
                new Weapon("Железный меч", 10, 10),
                new Weapon("Зачарованный лук", 15, 15),
                new Weapon("Аганим скипетр", 20, 20),
                new Weapon("Алмазный меч", 35, 25)
            };
            armors = new List<Armor>
            {
                new Armor("Кожаная броня", 5, 5),
                new Armor("Кольчуга", 8, 8),
                new Armor("Железная броня", 12, 12),
                new Armor("Алмазная броня", 14, 14),
                new Armor("Незеритовая броня", 25, 25)
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
            Log.Clear();
            Log.Add("=== Игра началась! ===");
            NextEncounter();
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
        }

        private void EncounterEnemy()
        {
            CurrentEnemy = fabrica.CreateRandomEnemy();
            IsCombat = true;
            Log.Add($"Появился: {CurrentEnemy.Name}!");
        }

        private void EncounterBoss()
        {
            CurrentEnemy = fabrica.CreateRandomBoss();
            IsCombat = true;
            Log.Add($"БОСС: {CurrentEnemy.Name}!");
        }

        private void EncounterChest()
        {
            IsChest = true;
            Log.Add("Вы нашли сундук!");

            int itemType = RandomGenerator.Next(3);
            Item item = null;

            switch (itemType)
            {
                case 0:
                    item = new HealthPotion("Зелье лечения", 15);
                    Log.Add("В сундуке: Зелье лечения");
                    break;
                case 1:
                    item = RandomGenerator.GetRandomItem(weapons.ToArray());
                    Log.Add($"В сундуке: {item.Name} (Атака +{((Weapon)item).Attack})");
                    break;
                case 2:
                    item = RandomGenerator.GetRandomItem(armors.ToArray());
                    Log.Add($"В сундуке: {item.Name} (Защита +{((Armor)item).Defense})");
                    break;
            }

            CurrentChestItem = item;
        }

        public void PlayerAttack()
        {
            if (!IsCombat || CurrentEnemy == null) return;

            if (Player.Frozen)
            {
                Log.Add("Вы заморожены! Пропуск хода.");
                Player.Frozen = false;
                EnemyTurn();
                return;
            }

            int damage = Player.CalculateDamage();
            CurrentEnemy.TakeDamage(damage);
            Log.Add($"Вы нанесли {damage} урона!");

            if (!CurrentEnemy.IsAlive)
            {
                Log.Add($"{CurrentEnemy.Name} побеждён!");
                TurnCount++;
                Log.Add($"Ход: {TurnCount}");
                NextEncounter();
            }
            else
            {
                EnemyTurn();
            }
        }

        public void PlayerDefend()
        {
            if (!IsCombat || CurrentEnemy == null) return;

            if (Player.Frozen)
            {
                Log.Add("Вы заморожены! Пропуск хода.");
                Player.Frozen = false;
                EnemyTurn();
                return;
            }

            bool dodged = Player.TryDefend();
            Log.Add(dodged ? "Вы уклонились!" : "Вы встали в защиту.");
            EnemyTurn(dodged);
        }

        private void EnemyTurn(bool playerDodged = false)
        {
            if (CurrentEnemy == null || !CurrentEnemy.IsAlive || playerDodged) return;

            int damage = CurrentEnemy.CalculateDamage(Player);
            int finalDamage = Player.CalculateBlockedDamage(damage);

            Player.TakeDamage(finalDamage);
            Log.Add($"{CurrentEnemy.Name} наносит {finalDamage} урона! (HP: {Player.HP})");

            CurrentEnemy.ApplyEffectDamage(Player);

            if (Player.HP <= 0)
            {
                Player.HP = 0;
                GameOver = true;
                Log.Add("ВЫ ПОГИБЛИ! Игра окончена.");
            }
        }

        public void TakeItem(bool take)
        {
            if (CurrentChestItem == null) return;

            if (take)
            {
                CurrentChestItem.ApplyEffect(Player);
                Log.Add($"Вы взяли: {CurrentChestItem.Name}");
            }
            else
            {
                Log.Add($"Вы выбросили: {CurrentChestItem.Name}");
            }

            CurrentChestItem = null;
            IsChest = false;
            NextEncounter();
        }
    }
}