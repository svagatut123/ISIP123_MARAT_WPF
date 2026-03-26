# Практическая работа №6 ч.3: Модульное тестирование

Русаков Артем, Худайбердин Марат 

## Модульное тестирование

### Тесты авторизации (WpfApp1.AuthTests)

**Позитивные тесты:**
-  `AuthTestSuccess_ValidCredentials` - успешная авторизация
-  `AuthTestSuccess_DifferentUser` - авторизация другим пользователем
-  `AuthTestSuccess_TrimmedInput` - авторизация с пробелами (тримминг)

**Негативные тесты:**
-  `AuthTestFail_EmptyEmail` - пустой email
-  `AuthTestFail_EmptyPassword` - пустой пароль
-  `AuthTestFail_WhitespaceEmail` - пробелы в email
-  `AuthTestFail_WhitespacePassword` - пробелы в пароле
-  `AuthTestFail_WrongPassword` - неверный пароль
-  `AuthTestFail_NonExistentUser` - несуществующий пользователь
-  `AuthTestFail_NullEmail` - null email
-  `AuthTestFail_NullPassword` - null пароль

### Тесты регистрации (WpfApp1.RegisterTests)

**Позитивные тесты:**
-  `RegisterTestSuccess_ValidData` - успешная регистрация
-  `RegisterTestSuccess_WithoutPhone` - регистрация без телефона
-  `RegisterTestSuccess_DifferentUser` - регистрация другого пользователя

**Негативные тесты:**
-  `RegisterTestFail_EmptyEmail` - пустой email
-  `RegisterTestFail_EmptyPassword` - пустой пароль
-  `RegisterTestFail_ShortPassword` - короткий пароль (<6 символов)
-  `RegisterTestFail_EmptyFirstName` - пустое имя
-  `RegisterTestFail_EmptyLastName` - пустая фамилия
-  `RegisterTestFail_UserAlreadyExists` - пользователь уже существует
-  `RegisterTestFail_WhitespaceEmail` - пробелы в email
-  `RegisterTestFail_WhitespacePassword` - пробелы в пароле
-  `RegisterTestFail_WhitespaceFirstName` - пробелы в имени
-  `RegisterTestFail_WhitespaceLastName` - пробелы в фамилии

##  Результаты тестирования

**Всего тестов:** 22  
**Успешно:** 22 (100%)  
**Неудачно:** 0 (0%)

##  Структура проекта

```
WpfApp1.Solution/
├── WpfApp1/
│   ├── Pages/
│   │   ├── HomePage.xaml / .cs
│   │   ├── LoginPage.xaml / .cs
│   │   ├── RegisterPage.xaml / .cs
│   │   ├── MoviePage.xaml / .cs
│   │   ├── SessionPage.xaml / .cs
│   │   ├── TicketPage.xaml / .cs
│   │   └── ProfilePage.xaml / .cs
│   ├── Core.cs
│   ├── MainWindow.xaml / .cs
│   └── App.config
│
├── WpfApp1.AuthTests/
│   ├── AuthTests.cs
│   └── App.config
│
└── WpfApp1.RegisterTests/ 
    ├── RegisterTests.cs
    └── App.config
```

### Обозреватель тестов

<img width="462" height="203" alt="image" src="https://github.com/user-attachments/assets/8716abff-a628-4ba9-ae14-6475734196cd" />

###Таблица пользователей

<img width="621" height="177" alt="image" src="https://github.com/user-attachments/assets/ec6575dd-ceee-4fd6-839f-3640be1f4a9e" />


## Вывод

Все модульные тесты успешно пройдены. Модули авторизации и регистрации работают корректно:

- Проверка пустых полей
- Проверка пробелов в начале и конце строк
- Проверка на null значения
- Проверка длины пароля
- Проверка уникальности email
- Корректная работа с базой данных

## 👨‍🎓 Автор

[Ваше имя, группа]
