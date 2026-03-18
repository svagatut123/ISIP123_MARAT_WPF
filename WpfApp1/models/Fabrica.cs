using System;

namespace WpfApp1.Models
{
    public class Fabrica
    {
        public Enemy CreateEnemy(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Goblin: return new Goblin();
                case EnemyType.Skeleton: return new Skeleton();
                case EnemyType.Mage: return new Mage();
                case EnemyType.Slime: return new Slime();
                case EnemyType.VVG: return new VVG();
                case EnemyType.Kovalsky: return new Kovalsky();
                case EnemyType.ArchimageCPP: return new ArchimageCPP();
                case EnemyType.PestovCmm: return new PestovCmm();
                default:
                    Console.WriteLine($"Ошибка: неизвестный тип врага: {type}");
                    return new Goblin(); 
            }
        }

        public Enemy CreateRandomEnemy()
        {
            var types = new[] { EnemyType.Goblin, EnemyType.Skeleton, EnemyType.Mage, EnemyType.Slime };
            return CreateEnemy(RandomGenerator.GetRandomItem(types));
        }

        public Enemy CreateRandomBoss()
        {
            var types = new[] { EnemyType.VVG, EnemyType.Kovalsky, EnemyType.ArchimageCPP, EnemyType.PestovCmm };
            return CreateEnemy(RandomGenerator.GetRandomItem(types));
        }
    }
}