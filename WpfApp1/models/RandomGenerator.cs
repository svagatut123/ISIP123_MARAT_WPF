using System;
using System.Linq;

namespace WpfApp1.Models
{
    public static class RandomGenerator
    {
        private static Random _random = new Random();

        public static int Next(int maxValue) => _random.Next(maxValue);
        public static int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);
        public static double NextDouble() => _random.NextDouble();
        public static bool NextBool() => _random.Next(2) == 0;

        public static T GetRandomItem<T>(T[] items)
        {
            return items[_random.Next(items.Length)];
        }
    }
}