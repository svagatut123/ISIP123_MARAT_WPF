using System;

namespace WpfApp1.Models
{
    public class Player
    {
        public int HP { get;  set; }
        public int MaxHP { get;  set; }
        public Weapon CurrentWeapon { get;  set; }
        public Armor CurrentArmor { get;  set; }
        public bool Frozen { get; set; }

        public int TotalAttack => CurrentWeapon?.Attack ?? 0;
        public int TotalDefense => CurrentArmor?.Defense ?? 0;

        public Player(int maxHP)
        {
            MaxHP = maxHP;
            HP = maxHP;
            CurrentWeapon = new Weapon("меч", 5, 3);
            CurrentArmor = new Armor("армор", 5, 2);
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0) HP = 0;
        }

        public void Heal(int amount)
        {
            HP += amount;
            if (HP > MaxHP) HP = MaxHP;
        }

        public void EquipWeapon(Weapon weapon) => CurrentWeapon = weapon;
        public void EquipArmor(Armor armor) => CurrentArmor = armor;

        public int CalculateDamage() => TotalAttack;

        public bool TryDefend()
        {
            return RandomGenerator.NextDouble() < 0.4;
        }

        public int CalculateBlockedDamage(int incomingDamage)
        {
            double blockPercentage = 0.7 + (RandomGenerator.NextDouble() * 0.3);
            int blockedDamage = (int)(TotalDefense * blockPercentage);
            return Math.Max(0, incomingDamage - blockedDamage);
        }

        public string GetStatus()
        {
            return $"HP: {HP}/{MaxHP} | Атака: {TotalAttack} | Защита: {TotalDefense}";
        }

        public string GetEquipment()
        {
            return $"Оружие: {CurrentWeapon?.Name ?? "Нет"}\nДоспех: {CurrentArmor?.Name ?? "Нет"}";
        }
    }
}