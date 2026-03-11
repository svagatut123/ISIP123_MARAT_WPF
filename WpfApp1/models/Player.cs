using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.models
{

    public class Player
    {
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public Weapon EquippedWeapon { get; set; }
        public Armor EquippedArmor { get; set; }
        public int Floor { get; set; }
        public bool IsFrozen { get; set; }
        public bool IsDefending { get; set; }

        public Player()
        {
            MaxHp = 100;
            CurrentHp = 100;
            Attack = 10;
            Defense = 5;
            Floor = 1;
            EquippedWeapon = new Weapon("Кулаки", 5, 0);
            EquippedArmor = new Armor("Одежда", 2, 0);
        }

        public int GetTotalAttack()
        {
            return Attack + (EquippedWeapon != null ? EquippedWeapon.AttackBonus : 0);
        }

        public int GetTotalDefense()
        {
            return Defense + (EquippedArmor != null ? EquippedArmor.DefenseBonus : 0);
        }
    }

}
