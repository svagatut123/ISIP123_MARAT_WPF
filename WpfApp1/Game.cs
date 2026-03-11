using RoguelikeGame.Models;
using System;
using System.Collections.Generic;
using WpfApp1.models;
using WpfApp1.Models;

namespace RoguelikeGame
{
    public class GameEngine
    {
        private Random _random;
        public Player Player { get; set; }
        public List<Enemy> CurrentEnemies { get; set; }
        public List<string> EventLog { get; set; }
        public bool GameOver { get; set; }
        public bool Victory { get; set; }

        public GameEngine()
        {
            _random = new Random();
            Player = new Player();
            CurrentEnemies = new List<Enemy>();
            EventLog = new List<string>();
            GameOver = false;
        }

        public void StartGame()
        {
            Player = new Player();
            CurrentEnemies.Clear();
            EventLog.Clear();
            EventLog.Add("Игра началась! Удачи!");
            GameOver = false;
            Victory = false;
            GenerateEncounter();
        }

        public void GenerateEncounter()
        {
            if (Player.Floor % 10 == 0)
            {
                SpawnBoss();
            }
            else
            {
                int chance = _random.Next(100);
                if (chance < 50)
                {
                    SpawnEnemy();
                }
                else
                {
                    SpawnChest();
                }
            }
        }

        private void SpawnEnemy()
        {
            CurrentEnemies.Clear();
            int enemyCount = _random.Next(1, 4); 

            for (int i = 0; i < enemyCount; i++)
            {
                Enemy enemy = CreateRandomEnemy();
                CurrentEnemies.Add(enemy);
                EventLog.Add($"Появился враг: {enemy.Name} (HP: {enemy.CurrentHp}/{enemy.MaxHp})");
            }
        }

        private Enemy CreateRandomEnemy()
        {
            int type = _random.Next(3);
            Enemy enemy = null;

            switch (type)
            {
                case 0: 
                    enemy = new Enemy("Гоблин", 30, 12, 3, "Goblin");
                    enemy.CritChance = 0.20;
                    break;
                case 1: 
                    enemy = new Enemy("Скелет", 40, 10, 5, "Skeleton");
                    enemy.IgnoreArmor = true;
                    break;
                case 2: 
                    enemy = new Enemy("Маг", 25, 15, 2, "Mage");
                    enemy.FreezeChance = 0.15;
                    break;
            }

            return enemy;
        }

        private void SpawnBoss()
        {
            CurrentEnemies.Clear();
            int bossType = _random.Next(4);
            Enemy boss = null;

            switch (bossType)
            {
                case 0:
                    boss = new Enemy("ВВГ", (int)(30 * 2.0), (int)(12 * 1.5), (int)(3 * 1.2), "Goblin");
                    boss.CritChance = 0.30;
                    break;
                case 1: 
                    boss = new Enemy("Ковальский", (int)(40 * 2.5), (int)(10 * 1.3), (int)(5 * 1.4), "Skeleton");
                    boss.IgnoreArmor = true;
                    break;
                case 2: 
                    boss = new Enemy("Архимаг С++", (int)(25 * 1.8), (int)(15 * 1.6), (int)(2 * 1.1), "Mage");
                    boss.FreezeChance = 0.25;
                    break;
                case 3: 
                    boss = new Enemy("Пестов С--", (int)(40 * 1.3), (int)(10 * 1.8), (int)(5 * 0.6), "Skeleton");
                    boss.FreezeChance = 0.15;
                    break;
            }

            boss.IsBoss = true;
            CurrentEnemies.Add(boss);
            EventLog.Add($"!!! БОСС: {boss.Name} появился! (HP: {boss.CurrentHp}/{boss.MaxHp})");
        }

        private void SpawnChest()
        {
            CurrentEnemies.Clear();
            EventLog.Add("Вы нашли сундук!");

            int itemType = _random.Next(3);

            switch (itemType)
            {
                case 0: 
                    Player.CurrentHp = Player.MaxHp;
                    EventLog.Add("Получено: Зелье лечения! HP восстановлено полностью.");
                    break;
                case 1: 
                    GiveRandomWeapon();
                    break;
                case 2: 
                    GiveRandomArmor();
                    break;
            }
        }

        private void GiveRandomWeapon()
        {
            string[] weapons = { "Меч", "Топор", "Кинжал", "Молот" };
            string name = weapons[_random.Next(weapons.Length)];
            int attack = _random.Next(5, 15);

            Weapon newWeapon = new Weapon(name, attack, 0);
            EventLog.Add($"Получено оружие: {newWeapon.Name} (Атака: +{newWeapon.AttackBonus})");
            EventLog.Add($"Текущее оружие: {Player.EquippedWeapon.Name} (Атака: +{Player.EquippedWeapon.AttackBonus})");
            EventLog.Add("Заменить оружие? (кнопки: Взять / Выбросить)");

            Player.Tag = newWeapon;
        }

        private void GiveRandomArmor()
        {
            string[] armors = { "Кольчуга", "Латы", "Кожанка", "Щит" };
            string name = armors[_random.Next(armors.Length)];
            int defense = _random.Next(3, 12);

            Armor newArmor = new Armor(name, defense, 0);
            EventLog.Add($"Получены доспехи: {newArmor.Name} (Защита: +{newArmor.DefenseBonus})");
            EventLog.Add($"Текущие доспехи: {Player.EquippedArmor.Name} (Защита: +{Player.EquippedArmor.DefenseBonus})");
            EventLog.Add("Заменить доспехи? (кнопки: Взять / Выбросить)");

            Player.Tag = newArmor;
        }

        public void PlayerAttack()
        {
            if (Player.IsFrozen)
            {
                EventLog.Add("Вы заморожены и пропускаете ход!");
                Player.IsFrozen = false;
                EnemyTurn();
                return;
            }

            Player.IsDefending = false;
            int damage = Player.GetTotalAttack();

            if (CurrentEnemies.Count > 0)
            {
                Enemy target = CurrentEnemies[0];
                int actualDamage = Math.Max(1, damage - target.Defense);
                target.CurrentHp -= actualDamage;

                EventLog.Add($"Вы атаковали {target.Name} и нанесли {actualDamage} урона.");

                if (target.CurrentHp <= 0)
                {
                    EventLog.Add($"{target.Name} повержен!");
                    CurrentEnemies.Remove(target);
                }
            }

            if (CurrentEnemies.Count == 0)
            {
                Player.Floor++;
                EventLog.Add($"Этаж {Player.Floor}");
                GenerateEncounter();
            }
            else
            {
                EnemyTurn();
            }
        }

        public void PlayerDefend()
        {
            if (Player.IsFrozen)
            {
                EventLog.Add("Вы заморожены и пропускаете ход!");
                Player.IsFrozen = false;
                EnemyTurn();
                return;
            }

            Player.IsDefending = true;
            EventLog.Add("Вы перешли в защиту (40% шанс уклонения).");
            EnemyTurn();
        }

        private void EnemyTurn()
        {
            if (GameOver) return;

            foreach (Enemy enemy in CurrentEnemies)
            {
                if (enemy.CurrentHp <= 0) continue;

                // Check dodge
                if (Player.IsDefending)
                {
                    int dodgeChance = _random.Next(100);
                    if (dodgeChance < 40)
                    {
                        EventLog.Add($"{enemy.Name} атакует, но вы уклонились!");
                        continue;
                    }
                }

                // Calculate damage
                int damage = enemy.Attack;
                int playerDefense = Player.GetTotalDefense();

                if (enemy.IgnoreArmor)
                {
                    playerDefense = 0;
                    EventLog.Add($"{enemy.Name} игнорирует вашу броню!");
                }

                // Check block reduction
                if (Player.IsDefending)
                {
                    int blockPercent = _random.Next(70, 101); // 70-100%
                    damage = damage * (100 - blockPercent) / 100;
                }

                damage = Math.Max(1, damage - playerDefense);

                // Check crit
                if (_random.NextDouble() < enemy.CritChance)
                {
                    damage *= 2;
                    EventLog.Add($"{enemy.Name} нанес критический удар!");
                }

                Player.CurrentHp -= damage;
                EventLog.Add($"{enemy.Name} атакует и наносит {damage} урона. Ваше HP: {Player.CurrentHp}/{Player.MaxHp}");

                // Check freeze
                if (_random.NextDouble() < enemy.FreezeChance)
                {
                    Player.IsFrozen = true;
                    EventLog.Add("Вы заморожены! Следующий ход будет пропущен.");
                }

                if (Player.CurrentHp <= 0)
                {
                    Player.CurrentHp = 0;
                    GameOver = true;
                    EventLog.Add("ВЫ ПОГИБЛИ! Игра окончена.");
                    break;
                }
            }

            Player.IsDefending = false;
        }

        public void TakeItem(bool take)
        {
            if (Player.Tag is Weapon weapon)
            {
                if (take)
                {
                    Player.EquippedWeapon = weapon;
                    EventLog.Add($"Вы взяли {weapon.Name}.");
                }
                else
                {
                    EventLog.Add($"Вы выбросили {weapon.Name}.");
                }
                Player.Tag = null;
                GenerateEncounter();
            }
            else if (Player.Tag is Armor armor)
            {
                if (take)
                {
                    Player.EquippedArmor = armor;
                    EventLog.Add($"Вы взяли {armor.Name}.");
                }
                else
                {
                    EventLog.Add($"Вы выбросили {armor.Name}.");
                }
                Player.Tag = null;
                GenerateEncounter();
            }
        }
    }
}