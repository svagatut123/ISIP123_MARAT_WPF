using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApp1.Pages;
using System.Windows;
using System.Linq;

namespace WpfApp1.AuthTests
{
    [TestClass]
    public class AuthTests
    {
        private MainWindow mainWindow;
        private LoginPage loginPage;

        [TestInitialize]
        public void Setup()
        {
            mainWindow = new MainWindow();
            loginPage = new LoginPage(mainWindow);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Core.CurrentUser = null;
        }

        // Позитивный тест: успешная авторизация
        [TestMethod]
        public void AuthTestSuccess()
        {
            string validEmail = "test@example.com";
            string validPassword = "password123";

            var testUser = Core.Context.users.FirstOrDefault(u => u.email == validEmail);
            if (testUser == null)
            {
                testUser = new users
                {
                    email = validEmail,
                    password = validPassword,
                    firstname = "Тест",
                    lastname = "Пользователь"
                };
                Core.Context.users.Add(testUser);
                Core.Context.SaveChanges();
            }

            bool result = loginPage.Auth(validEmail, validPassword);

            Assert.IsTrue(result);
            Assert.IsNotNull(Core.CurrentUser);
            Assert.AreEqual(validEmail, Core.CurrentUser.email);
        }

        // Негативный тест: пустой email
        [TestMethod]
        public void AuthTestEmptyEmail()
        {
            bool result = loginPage.Auth("", "password123");
            Assert.IsFalse(result);
            Assert.IsNull(Core.CurrentUser);
        }

        // Негативный тест: пустой пароль
        [TestMethod]
        public void AuthTestEmptyPassword()
        {
            bool result = loginPage.Auth("test@example.com", "");
            Assert.IsFalse(result);
            Assert.IsNull(Core.CurrentUser);
        }

        // Негативный тест: неверный пароль
        [TestMethod]
        public void AuthTestWrongPassword()
        {
            bool result = loginPage.Auth("test@example.com", "wrongpassword");
            Assert.IsFalse(result);
            Assert.IsNull(Core.CurrentUser);
        }

        // Негативный тест: несуществующий пользователь
        [TestMethod]
        public void AuthTestNonExistentUser()
        {
            bool result = loginPage.Auth("nonexistent@example.com", "password123");
            Assert.IsFalse(result);
            Assert.IsNull(Core.CurrentUser);
        }
    }
}