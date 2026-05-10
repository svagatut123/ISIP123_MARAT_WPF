using System.Windows;

namespace WpfApp1
{
    public static class Core
    {
        // Статический контекст Entity Framework
        public static Up11Entities Context = new Up11Entities();

        // Хранение текущего пользователя
        public static Users CurrentUser { get; set; }
    }
}