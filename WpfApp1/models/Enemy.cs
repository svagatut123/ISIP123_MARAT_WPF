using System;

namespace WpfApp1.Models
{
    public class Enemy
    {
        public string Name { get; set; }
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public string Type { get; set; }
        public bool IsBoss { get; set; }
        public double CritChance { get; set; }
        public bool IgnoreArmor { get; set; }
        public double FreezeChance { get; set; }

        public Enemy(string name, int hp, int attack, int defense, string type)
        {
            Name = name;
            MaxHp = hp;
            CurrentHp = hp;
            Attack = attack;
            Defense = defense;
            Type = type;
            IsBoss = false;
            CritChance = 0;
            IgnoreArmor = false;
            FreezeChance = 0;
        }
    }
}