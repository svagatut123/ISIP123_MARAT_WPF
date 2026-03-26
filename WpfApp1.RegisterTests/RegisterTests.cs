using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApp1;
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
            // Создаем главное окно и страницу регистрации
            mainWindow = new MainWindow();
            registerPage = new RegisterPage(mainWindow);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Удаляем тестовых пользователей после каждого теста
            var testUsers = Core.Context.users.Where(u => u.email.StartsWith("test")).ToList();
            foreach (var user in testUsers)
            {
                Core.Context.users.Remove(user);
            }
            Core.Context.SaveChanges();
        }

        // ============================================
        // ПОЗИТИВНЫЕ ТЕСТЫ (успешная регистрация)
        // ============================================

        [TestMethod]
        public void RegisterTestSuccess_ValidData()
        {
            // Arrange - подготовка тестовых данных
            string email = "newuser@example.com";
            string password = "password123";
            string firstname = "Иван";
            string lastname = "Иванов";
            string phone = "+79001234567";

            // Удаляем пользователя, если он уже существует
            var existingUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            if (existingUser != null)
            {
                Core.Context.users.Remove(existingUser);
                Core.Context.SaveChanges();
            }

            // Act - выполнение тестируемого метода
            bool result = registerPage.Register(email, password, firstname, lastname, phone);

            // Assert - проверка результатов
            Assert.IsTrue(result, "Регистрация должна быть успешной");

            // Проверяем, что пользователь добавлен в базу
            var registeredUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            Assert.IsNotNull(registeredUser, "Пользователь должен быть добавлен в базу");
            Assert.AreEqual(firstname, registeredUser.firstname);
            Assert.AreEqual(lastname, registeredUser.lastname);
            Assert.AreEqual(email, registeredUser.email);
        }

        [TestMethod]
        public void RegisterTestSuccess_WithoutPhone()
        {
            // Arrange
            string email = "userwithoutphone@example.com";
            string password = "password123";
            string firstname = "Петр";
            string lastname = "Петров";
            string phone = ""; // Пустой телефон

            // Удаляем пользователя, если он уже существует
            var existingUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            if (existingUser != null)
            {
                Core.Context.users.Remove(existingUser);
                Core.Context.SaveChanges();
            }

            // Act
            bool result = registerPage.Register(email, password, firstname, lastname, phone);

            // Assert
            Assert.IsTrue(result, "Регистрация должна быть успешной даже без телефона");

            var registeredUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            Assert.IsNotNull(registeredUser);
        }

        [TestMethod]
        public void RegisterTestSuccess_DifferentUser()
        {
            // Arrange
            string email = "anotheruser@example.com";
            string password = "securepass789";
            string firstname = "Мария";
            string lastname = "Сидорова";
            string phone = "+79112223344";

            // Удаляем пользователя, если он уже существует
            var existingUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            if (existingUser != null)
            {
                Core.Context.users.Remove(existingUser);
                Core.Context.SaveChanges();
            }

            // Act
            bool result = registerPage.Register(email, password, firstname, lastname, phone);

            // Assert
            Assert.IsTrue(result);

            var registeredUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            Assert.IsNotNull(registeredUser);
            Assert.AreEqual("Мария", registeredUser.firstname);
            Assert.AreEqual("Сидорова", registeredUser.lastname);
        }

        // ============================================
        // НЕГАТИВНЫЕ ТЕСТЫ (неудачная регистрация)
        // ============================================

        [TestMethod]
        public void RegisterTestFail_EmptyEmail()
        {
            // Arrange
            string emptyEmail = "";
            string password = "password123";
            string firstname = "Иван";
            string lastname = "Иванов";
            string phone = "+79001234567";

            // Act
            bool result = registerPage.Register(emptyEmail, password, firstname, lastname, phone);

            // Assert
            Assert.IsFalse(result, "Регистрация должна быть неуспешной");
        }

        [TestMethod]
        public void RegisterTestFail_EmptyPassword()
        {
            // Arrange
            string email = "newuser@example.com";
            string emptyPassword = "";
            string firstname = "Иван";
            string lastname = "Иванов";
            string phone = "+79001234567";

            // Act
            bool result = registerPage.Register(email, emptyPassword, firstname, lastname, phone);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RegisterTestFail_ShortPassword()
        {
            // Arrange
            string email = "newuser@example.com";
            string shortPassword = "123"; // Меньше 6 символов
            string firstname = "Иван";
            string lastname = "Иванов";
            string phone = "+79001234567";

            // Act
            bool result = registerPage.Register(email, shortPassword, firstname, lastname, phone);

            // Assert
            Assert.IsFalse(result, "Регистрация должна быть неуспешной при коротком пароле");
        }

        [TestMethod]
        public void RegisterTestFail_EmptyFirstName()
        {
            // Arrange
            string email = "newuser@example.com";
            string password = "password123";
            string emptyFirstname = "";
            string lastname = "Иванов";
            string phone = "+79001234567";

            // Act
            bool result = registerPage.Register(email, password, emptyFirstname, lastname, phone);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RegisterTestFail_EmptyLastName()
        {
            // Arrange
            string email = "newuser@example.com";
            string password = "password123";
            string firstname = "Иван";
            string emptyLastname = "";
            string phone = "+79001234567";

            // Act
            bool result = registerPage.Register(email, password, firstname, emptyLastname, phone);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RegisterTestFail_UserAlreadyExists()
        {
            // Arrange
            string email = "existing@example.com";
            string password = "password123";
            string firstname = "Иван";
            string lastname = "Иванов";
            string phone = "+79001234567";

            // Создаем пользователя заранее
            var existingUser = Core.Context.users.FirstOrDefault(u => u.email == email);
            if (existingUser == null)
            {
                existingUser = new users
                {
                    email = email,
                    password = password,
                    firstname = firstname,
                    lastname = lastname,
                    phone_number = phone
                };
                Core.Context.users.Add(existingUser);
                Core.Context.SaveChanges();
            }

            // Act - пытаемся зарегистрировать пользователя с тем же email
            bool result = registerPage.Register(email, password, firstname, lastname, phone);

            // Assert
            Assert.IsFalse(result, "Регистрация должна быть неуспешной, если пользователь уже существует");
        }

        [TestMethod]
        public void RegisterTestFail_WhitespaceEmail()
        {
            // Arrange
            string whitespaceEmail = "   ";
            string password = "password123";
            string firstname = "Иван";
            string lastname = "Иванов";
            string phone = "+79001234567";

            // Act
            bool result = registerPage.Register(whitespaceEmail, password, firstname, lastname, phone);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RegisterTestFail_WhitespacePassword()
        {
            // Arrange
            string email = "newuser@example.com";
            string whitespacePassword = "   ";
            string firstname = "Иван";
            string lastname = "Иванов";
            string phone = "+79001234567";

            // Act
            bool result = registerPage.Register(email, whitespacePassword, firstname, lastname, phone);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RegisterTestFail_WhitespaceFirstName()
        {
            // Arrange
            string email = "newuser@example.com";
            string password = "password123";
            string whitespaceFirstname = "   ";
            string lastname = "Иванов";
            string phone = "+79001234567";

            // Act
            bool result = registerPage.Register(email, password, whitespaceFirstname, lastname, phone);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RegisterTestFail_WhitespaceLastName()
        {
            // Arrange
            string email = "newuser@example.com";
            string password = "password123";
            string firstname = "Иван";
            string whitespaceLastname = "   ";
            string phone = "+79001234567";

            // Act
            bool result = registerPage.Register(email, password, firstname, whitespaceLastname, phone);

            // Assert
            Assert.IsFalse(result);
        }
    }
}