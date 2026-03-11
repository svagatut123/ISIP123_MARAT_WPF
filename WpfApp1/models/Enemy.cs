using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Enemy
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHp { get; set; }
        public int Attack {  get; set; }
        public int Defense { get; set; }
        public string Type { get; set; }
        public bool IsBoss { get; set; }
        public double CritChance { get; set; }
        public bool IgnoreArmor { get; set; }

    }
}
