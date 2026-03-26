using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace WpfApp1.Tests
{
    [TestClass]
    public class CoreTests
    {
        [TestMethod]
        public void DatabaseConnectionTest()
        {
            Assert.IsNotNull(Core.Context, "Контекст базы данных не должен быть null");
        }

        [TestMethod]
        public void UsersTableNotEmptyTest()
        {
            var users = Core.Context.users.ToList();
            Assert.IsNotNull(users, "Таблица пользователей не должна быть null");
        }

        [TestMethod]
        public void MoviesTableNotEmptyTest()
        {
            var movies = Core.Context.Movies.ToList();
            Assert.IsNotNull(movies, "Таблица фильмов не должна быть null");
        }
    }
}