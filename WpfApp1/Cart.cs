using System.Linq;

namespace WpfApp1
{
    public static class Cart
    {
        public static System.Collections.Generic.List<CartItem> Items { get; } =
            new System.Collections.Generic.List<CartItem>();

        public static decimal Total => Items.Sum(i => i.Total);

        public static void AddProduct(Products product) // Products с "s"!
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