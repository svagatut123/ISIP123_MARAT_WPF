namespace WpfApp1.Models
{
    public abstract class Item
    {
        public string Name { get; protected set; }
        public int Value { get; protected set; }

        protected Item(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public abstract void ApplyEffect(Player player);
    }

    public class Weapon : Item
    {
        public int Attack { get; private set; }

        public Weapon(string name, int value, int attack) : base(name, value)
        {
            Attack = attack;
        }

        public override void ApplyEffect(Player player) => player.EquipWeapon(this);

        public override string ToString() => $"{Name} (Атака: {Attack})";
    }

    public class Armor : Item
    {
        public int Defense { get; private set; }

        public Armor(string name, int value, int defense) : base(name, value)
        {
            Defense = defense;
        }

        public override void ApplyEffect(Player player) => player.EquipArmor(this);

        public override string ToString() => $"{Name} (Защита: {Defense})";
    }

    public class HealthPotion : Item
    {
        public HealthPotion(string name, int value) : base(name, value) { }

        public override void ApplyEffect(Player player)
        {
            player.Heal(player.MaxHP);
        }

        public override string ToString() => $"{Name} (полное лечение)";
    }
}