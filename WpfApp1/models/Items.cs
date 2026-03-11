using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.models
{
    
        public class Weapon
        {
            public string Name { get; set; }
            public int AttackBonus { get; set; }
            public int DefenseBonus { get; set; }

            public Weapon(string name, int attack, int defense)
            {
                Name = name;
                AttackBonus = attack;
                DefenseBonus = defense;
            }
        }

        public class Armor
        {
            public string Name { get; set; }
            public int DefenseBonus { get; set; }
            public int AttackBonus { get; set; }

            public Armor(string name, int defense, int attack)
            {
                Name = name;
                DefenseBonus = defense;
                AttackBonus = attack;
            }
        }

        public class Item
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public object Data { get; set; }
        }
    
}

