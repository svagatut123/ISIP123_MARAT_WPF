namespace WpfApp1
{
    public class CartItem
    {
        public Products Product { get; set; } // Products с "s"!
        public int Quantity { get; set; } = 1;
        public decimal Total => Product.Cena * Quantity;
    }
}