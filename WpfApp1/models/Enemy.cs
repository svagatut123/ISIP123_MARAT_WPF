using System;

namespace WpfApp1.Models
{
    // === БАЗОВЫЙ КЛАСС ===
    public abstract partial class Enemy
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public bool Frozen { get; set; }

        protected Enemy(string name, int hp, int attack, int defense)
        {
            Name = name;
            MaxHP = hp;
            HP = hp;
            Attack = attack;
            Defense = defense;
        }

        public virtual void TakeDamage(int damage)
        {
            HP = HP - damage;
            if (HP < 0) HP = 0;
        }

        public abstract int CalculateDamage(Player player);
        public abstract void ApplyEffectDamage(Player player);

        public bool IsAlive => HP > 0;

        public virtual string GetStatus()
        {
            return $"{Name} - HP: {HP}/{MaxHP}, Атака: {Attack}, Защита: {Defense}";
        }
    }

    public partial class Goblin : Enemy
    {
        private double ChanceCrit = 0.2;
        public Goblin() : base("Гоблин", 30, 8, 3) { }

        public override int CalculateDamage(Player player)
        {
            int baseDamage = Attack;
            if (RandomGenerator.NextDouble() < ChanceCrit)
            {
                baseDamage = (int)(baseDamage * 1.2);
            }
            return baseDamage;
        }

        public override void ApplyEffectDamage(Player player) { }
    }

    public partial class Skeleton : Enemy
    {
        public Skeleton() : base("Скелет", 25, 10, 2) { }

        public override int CalculateDamage(Player player) => Attack;
        public override void ApplyEffectDamage(Player player) { }
    }

    public partial class Mage : Enemy
    {
        private double freezeChance = 0.25;
        public Mage() : base("Маг", 20, 12, 1) { }

        public override int CalculateDamage(Player player) => Attack;

        public override void ApplyEffectDamage(Player player)
        {
            if (RandomGenerator.NextDouble() < freezeChance)
            {
                player.Frozen = true;
            }
        }
    }

    public partial class Slime : Enemy
    {
        public Slime() : base("Слизень", 35, 6, 5) { }

        public override void TakeDamage(int damage)
        {
            int reducedDamage = Math.Max(0, damage - 2);
            base.TakeDamage(reducedDamage);
        }

        public override int CalculateDamage(Player player) => Attack;
        public override void ApplyEffectDamage(Player player) { }
    }

    public partial class VVG : Goblin
    {
        public VVG() : base()
        {
            Name = "ВВГ (Босс)";
            MaxHP = (int)(MaxHP * 2.0); HP = MaxHP;
            Attack = (int)(Attack * 1.5);
            Defense = (int)(Defense * 1.2);
        }

        public override int CalculateDamage(Player player)
        {
            int baseDamage = Attack;
            if (RandomGenerator.NextDouble() < 0.3)
            {
                baseDamage = (int)(baseDamage * 1.8);
            }
            return baseDamage;
        }
    }

    public partial class Kovalsky : Skeleton
    {
        public Kovalsky() : base()
        {
            Name = "Ковальский (Босс)";
            MaxHP = (int)(MaxHP * 2.5); HP = MaxHP;
            Attack = (int)(Attack * 1.3);
            Defense = (int)(Defense * 1.4);
        }
    }

    public partial class ArchimageCPP : Mage
    {
        public ArchimageCPP() : base()
        {
            Name = "Архимаг C++ (Босс)";
            MaxHP = (int)(MaxHP * 1.8); HP = MaxHP;
            Attack = (int)(Attack * 1.6);
            Defense = (int)(Defense * 1.1);
        }

        public override void ApplyEffectDamage(Player player)
        {
            if (RandomGenerator.NextDouble() < 0.35)
            {
                player.Frozen = true;
            }
        }
    }

    public partial class PestovCmm : Skeleton
    {
        private double freezeChance = 0.4;
        public PestovCmm() : base()
        {
            Name = "Пестов С-- (Босс)";
            MaxHP = (int)(MaxHP * 1.3); HP = MaxHP;
            Attack = (int)(Attack * 1.8);
            Defense = (int)(Defense * 0.6);
        }

        public override int CalculateDamage(Player player) => Attack;

        public override void ApplyEffectDamage(Player player)
        {
            if (RandomGenerator.NextDouble() < freezeChance)
            {
                player.Frozen = true;
            }
        }
    }
}