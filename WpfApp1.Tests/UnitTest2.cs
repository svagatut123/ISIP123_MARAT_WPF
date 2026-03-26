using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApp1.Pages;
using System.Linq;

namespace WpfApp1.RegisterTests
{
    [TestClass]
    public class RegisterTests
    {
        private MainWindow mainWindow;
        private RegisterPage registerPage;

        [TestInitialize]
        public void Setup()
        {
            mainWindow = new MainWindow();
            registerPage = new RegisterPage(mainWindow);
        }

        [TestCleanup]
        public void Cleanup()
        {
            var testUsers = Core.Context.users.Where(u => u.email.StartsWith("test")).ToList();
            foreach (var user in testUsers)
            {
                Core.Context.users.Remove(user);
            }
            Core.Context.SaveChanges();
        }

        // Позитивный тест: успешная регистрация
        [TestMethod]
        public void RegisterTestSuccess()
        {
            string email = "newuser@example.com";
            string password = "password123";
            string firstname = "Иван";
            string lastname = "Иванов";
            string phone = "+79001234567";

            var existingUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            if (existingUser != null)
            {
                Core.Context.users.Remove(existingUser);
                Core.Context.SaveChanges();
            }

            bool result = registerPage.Register(email, password, firstname, lastname, phone);

            Assert.IsTrue(result);

            var registeredUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            Assert.IsNotNull(registeredUser);
            Assert.AreEqual(firstname, registeredUser.firstname);
        }

        // Негативный тест: пустой email
        [TestMethod]
        public void RegisterTestEmptyEmail()
        {
            bool result = registerPage.Register("", "password123", "Иван", "Иванов", "");
            Assert.IsFalse(result);
        }

        // Негативный тест: короткий пароль
        [TestMethod]
        public void RegisterTestShortPassword()
        {
            bool result = registerPage.Register("newuser@example.com", "123", "Иван", "Иванов", "");
            Assert.IsFalse(result);
        }

        // Негативный тест: пользователь уже существует
        [TestMethod]
        public void RegisterTestUserAlreadyExists()
        {
            string email = "existing@example.com";
            string password = "password123";
            string firstname = "Иван";
            string lastname = "Иванов";

            var existingUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            if (existingUser == null)
            {
                existingUser = new users
                {
                    email = email,
                    password = password,
                    firstname = firstname,
                    lastname = lastname
                };
                Core.Context.users.Add(existingUser);
                Core.Context.SaveChanges();
            }

            bool result = registerPage.Register(email, password, firstname, lastname, "");
            Assert.IsFalse(result);
        }
    }
}