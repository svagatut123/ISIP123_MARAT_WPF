using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApp1;
using WpfApp1.Pages;
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
            // Создаем главное окно и страницу авторизации
            mainWindow = new MainWindow();
            loginPage = new LoginPage(mainWindow);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Очищаем текущего пользователя после каждого теста
            Core.CurrentUser = null;
        }

        // ============================================
        // ПОЗИТИВНЫЕ ТЕСТЫ (успешная авторизация)
        // ============================================

        [TestMethod]
        public void AuthTestSuccess_ValidCredentials()
        {
            // Arrange - подготовка тестовых данных
            string validEmail = "test@example.com";
            string validPassword = "password123";

            // Создаем тестового пользователя в базе данных
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

            // Act - выполнение тестируемого метода
            bool result = loginPage.Auth(validEmail, validPassword);

            // Assert - проверка результатов
            Assert.IsTrue(result, "Авторизация должна быть успешной");
            Assert.IsNotNull(Core.CurrentUser, "CurrentUser не должен быть null");
            Assert.AreEqual(validEmail, Core.CurrentUser.email, "Email должен совпадать");
        }

        [TestMethod]
        public void AuthTestSuccess_DifferentUser()
        {
            // Arrange
            string email = "user2@example.com";
            string password = "pass456";

            var testUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            if (testUser == null)
            {
                testUser = new users
                {
                    email = email,
                    password = password,
                    firstname = "Иван",
                    lastname = "Иванов"
                };
                Core.Context.users.Add(testUser);
                Core.Context.SaveChanges();
            }

            // Act
            bool result = loginPage.Auth(email, password);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(Core.CurrentUser);
            Assert.AreEqual("Иван", Core.CurrentUser.firstname);
        }

        // ============================================
        // НЕГАТИВНЫЕ ТЕСТЫ (неудачная авторизация)
        // ============================================

        [TestMethod]
        public void AuthTestFail_EmptyEmail()
        {
            // Arrange
            string emptyEmail = "";
            string password = "password123";

            // Act
            bool result = loginPage.Auth(emptyEmail, password);

            // Assert
            Assert.IsFalse(result, "Авторизация должна быть неуспешной");
            Assert.IsNull(Core.CurrentUser, "CurrentUser должен быть null");
        }

        [TestMethod]
        public void AuthTestFail_EmptyPassword()
        {
            // Arrange
            string email = "test@example.com";
            string emptyPassword = "";

            // Act
            bool result = loginPage.Auth(email, emptyPassword);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(Core.CurrentUser);
        }

        [TestMethod]
        public void AuthTestFail_WhitespaceEmail()
        {
            // Arrange
            string whitespaceEmail = "   ";
            string password = "password123";

            // Act
            bool result = loginPage.Auth(whitespaceEmail, password);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(Core.CurrentUser);
        }

        [TestMethod]
        public void AuthTestFail_WhitespacePassword()
        {
            // Arrange
            string email = "test@example.com";
            string whitespacePassword = "   ";

            // Act
            bool result = loginPage.Auth(email, whitespacePassword);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(Core.CurrentUser);
        }

        [TestMethod]
        public void AuthTestFail_WrongPassword()
        {
            // Arrange
            string email = "test@example.com";
            string wrongPassword = "wrongpassword";

            // Создаем пользователя с правильным паролем
            var testUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            if (testUser == null)
            {
                testUser = new users
                {
                    email = email,
                    password = "password123",
                    firstname = "Тест",
                    lastname = "Пользователь"
                };
                Core.Context.users.Add(testUser);
                Core.Context.SaveChanges();
            }

            // Act - пытаемся войти с неправильным паролем
            bool result = loginPage.Auth(email, wrongPassword);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(Core.CurrentUser);
        }

        [TestMethod]
        public void AuthTestFail_NonExistentUser()
        {
            // Arrange
            string nonExistentEmail = "nonexistent@example.com";
            string password = "password123";

            // Act - пытаемся войти несуществующим пользователем
            bool result = loginPage.Auth(nonExistentEmail, password);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(Core.CurrentUser);
        }

        [TestMethod]
        public void AuthTestFail_TrimmedInput()
        {
            // Arrange
            string email = "test@example.com";
            string password = "password123";

            // Создаем пользователя
            var testUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            if (testUser == null)
            {
                testUser = new users
                {
                    email = email,
                    password = password,
                    firstname = "Тест",
                    lastname = "Пользователь"
                };
                Core.Context.users.Add(testUser);
                Core.Context.SaveChanges();
            }

            // Act - передаем данные с пробелами в начале и конце
            bool result = loginPage.Auth("  " + email + "  ", "  " + password + "  ");

            // Assert - метод должен обработать пробелы и авторизовать успешно
            Assert.IsTrue(result);
            Assert.IsNotNull(Core.CurrentUser);
        }

        [TestMethod]
        public void AuthTestFail_NullEmail()
        {
            // Arrange
            string nullEmail = null;
            string password = "password123";

            // Act
            bool result = loginPage.Auth(nullEmail, password);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(Core.CurrentUser);
        }

        [TestMethod]
        public void AuthTestFail_NullPassword()
        {
            // Arrange
            string email = "test@example.com";
            string nullPassword = null;

            // Act
            bool result = loginPage.Auth(email, nullPassword);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(Core.CurrentUser);
        }
    }
}