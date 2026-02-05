using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Core
    {
       public static MaratPrac13Entities5 Context = new MaratPrac13Entities5();
       public static List <CartItem> Cart = new List<CartItem> ();

    }
    public class CartItem
    {
        public int ProductId { get; set; }
        public int _kolichestvo { get; set; }
        public string NazvanieTovara { get; set; }
        public decimal Cena { get; set; }
        public string IzobrazhenieTovara { get; set; }
    }
    public static class Cart
    {
        public static List<CartItem> Tovary {  get; set; } = new List<CartItem> ();
    }
    public class OrderFormData
    {
        public string FIO { get; set; }
        public string Email { get; set; }
        public string AdressDostavki { get; set; }
    }
}
