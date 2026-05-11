using System.Windows;

namespace WpfApp1
{
    public static class Core
    {
        // Статический контекст Entity Framework
        public static up_11Entities Context = new up_11Entities();

        // Хранение текущего пользователя
        public static Users CurrentUser { get; set; }
    }
}