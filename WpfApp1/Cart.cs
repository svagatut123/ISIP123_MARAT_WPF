using System.Linq;
using System.Collections.Generic;

namespace WpfApp1
{
    public static class Cart
    {
        public static List<CartItem> Items { get; } =
            new List<CartItem>();

        public static decimal Total => Items.Sum(i => i.Total);

        public static void AddProduct(Products product) 
        {
            var existing = Items.FirstOrDefault(i => i.Product.ProductId == product.ProductId);
            if (existing != null)
                existing.Quantity++;
            else
                Items.Add(new CartItem { Product = product, Quantity = 1 });
        }

        public static void Clear() => Items.Clear();
    }
}