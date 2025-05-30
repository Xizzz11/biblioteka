# 📚 Библиотека: Система управления

🚀 Консольное приложение на C# для управления библиотекой. Помогает библиотекарю вести учет книг, авторов, читателей, заказов и отзывов через интуитивное меню.

![GitHub License](https://img.shields.io/github/license/<ваш_репозиторий>?style=flat-square&color=brightgreen) ![.NET 8.0](https://img.shields.io/badge/.NET-8.0-5C2D91?style=flat-square&logo=dotnet) ![SQLite](https://img.shields.io/badge/SQLite-003B57?style=flat-square&logo=sqlite)

---

## ✨ Ключевые возможности

- 📖 **Книги**: Добавляйте, просматривайте, редактируйте и удаляйте книги.
- ✍️ **Авторы**: Регистрируйте новых авторов.
- 👤 **Читатели**: Управляйте читателями, выдавайте и принимайте книги.
- ⭐ **Отзывы**: Добавляйте и просматривайте отзывы о книгах.
- 🔍 **Поиск**: Ищите книги по названию или автору.
- 💾 **База данных**: Храните данные в SQLite с Entity Framework Core.

---

## 🎮 Команды меню

| Действие                  | Что делает?                                                           |
|---------------------------|----------------------------------------------------------------------|
| 📕 **Добавить книгу**     | Вводите название, ID автора, год и количество копий.                 |
| 📚 **Все книги**          | Показывает список книг с авторами, годом и копиями.                  |
| 🔎 **Детали книги**       | Выводит информацию о книге по ID.                                    |
| ✏️ **Обновить книгу**     | Меняйте название, автора, год или копии.                             |
| 🗑️ **Удалить книгу**      | Удаляет книгу по ID.                                                 |
| 🖋️ **Добавить автора**    | Вводите имя и фамилию автора.                                        |
| 👨‍💼 **Новый читатель**   | Регистрируйте читателя (имя, фамилия, email).                        |
| 📤 **Выдать книгу**       | Создает заказ с ID книги и читателя.                                 |
| 📥 **Вернуть книгу**      | Фиксирует возврат по ID заказа, увеличивает копии.                   |
| 🌟 **Добавить отзыв**     | Вводите ID книги, читателя, рейтинг (1-5) и комментарий.             |
| 📜 **Просмотреть отзывы** | Показывает отзывы для книги по ID.                                   |
| 🔎 **Поиск книг**         | Ищет книги по названию или автору.                                   |
| 🚪 **Выход**              | Завершает работу приложения.                                         |



![image](https://github.com/user-attachments/assets/a307619e-fc36-4d0f-962b-adfbb40649a3)

![image](https://github.com/user-attachments/assets/f84d0778-e3db-4323-afde-bf592599ed20)

![image](https://github.com/user-attachments/assets/1ec54060-ac76-455c-8bc2-4f381b38f77f)

![image](https://github.com/user-attachments/assets/7576cf7a-84ed-456f-bd58-48bde9bb8a94)

![image](https://github.com/user-attachments/assets/a8f9baef-cc7e-4849-86de-caaac8e9fc37)

![image](https://github.com/user-attachments/assets/cd3bee02-af94-44a6-b455-35ac7737104e)

![image](https://github.com/user-attachments/assets/84dc22ed-b095-4f89-98be-0f3d992c7da6)

![image](https://github.com/user-attachments/assets/d780a969-3f76-4a5d-aba7-005b501ecb1d)

![image](https://github.com/user-attachments/assets/2b9cefbb-a83a-4545-9840-17240d0d4784)

![image](https://github.com/user-attachments/assets/b58f073f-204b-436b-bd62-f31e1900a13c)

![image](https://github.com/user-attachments/assets/d416377f-bc8b-4ede-b91c-66f3d3802010)

![image](https://github.com/user-attachments/assets/54c0783e-d263-4050-b626-dbc707557452)


---

## 🗃️ База данных

### 📑 Таблицы

| Таблица     | Назначение                              | Поля                                                         |
|-------------|----------------------------------------|-------------------------------------------------------------|
| **Books**   | Хранит книги                           | `BookID` (PK), `Title`, `AuthorID` (FK), `AuthorName`, `PublicationYear`, `AvailableCopies` |
| **Authors** | Хранит авторов                         | `AuthorID` (PK), `FirstName`, `LastName`                    |
| **Readers** | Хранит читателей                       | `ReaderID` (PK), `FirstName`, `LastName`, `Email`           |
| **Orders**  | Отслеживает выдачу/возврат             | `OrderID` (PK), `BookID` (FK), `ReaderID` (FK), `IssueDate`, `ReturnDate` |
| **Reviews** | Хранит отзывы                          | `ReviewID` (PK), `BookID` (FK), `ReaderID` (FK), `Rating`, `Comment` |

### 🔗 Связи
- **Один-ко-многим**:
  - `Authors` → `Books`: Один автор — много книг.
  - `Books` → `Orders`: Одна книга — много заказов.
  - `Readers` → `Orders`: Один читатель — много заказов.
  - `Books` → `Reviews`: Одна книга — много отзывов.
  - `Readers` → `Reviews`: Один читатель — много отзывов.

### 💽 СУБД
- **SQLite** (файл: `library.db`), управляется через Entity Framework Core.

### 🛠️ Инициализация
База создается автоматически при запуске. Для миграций смотрите раздел "Установка".

---

## 📈 Диаграммы

### 1. 🔄 Последовательности
**Сценарий**: Выдача книги читателю.  
**Описание**: Показывает взаимодействие `Program`, `LibraryService`, `LibraryRepository` и `LibraryContext`.  
**UML**: Акторы, lifeline, сообщения.

![image](https://github.com/user-attachments/assets/084eecb6-686c-463d-84fe-28dc3c4e2431)


### 2. 🎯 Варианты использования
**Сценарий**: Действия библиотекаря.  
**Описание**: Описывает функции (добавление книг, выдача, отзывы).  
**UML**: Акторы, use case, связи include.

![image](https://github.com/user-attachments/assets/93efe121-e8e1-4268-b484-bdaf39ecd7c5)

### 3. 🧱 Классы
**Сценарий**: Структура данных.  
**Описание**: Отображает модели (`Book`, `Author`, `Reader`, `Order`, `Review`) и связи.  
**UML**: Классы, атрибуты, методы, отношения.

![image](https://github.com/user-attachments/assets/3978dd98-77d2-43b3-b2e8-1c4bcfa43596)



> 📌 Диаграммы можно создать в PlantUML или аналогичных инструментах.

---

## 🛠️ Установка и запуск

1. **Требования**:
   - .NET 8.0 SDK
   - SQLite (встроен)

2. **Установка**:
   ```bash
   dotnet restore
   ```

3. **Запуск**:
   ```bash
   dotnet build
   dotnet run
   ```

4. **Миграции (опционально)**:
   - Добавьте в `biblioteka-main.csproj`:
     ```xml
     <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.4">
       <PrivateAssets>all</PrivateAssets>
       <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
     </PackageReference>
     ```
   - Установите `dotnet-ef`:
     ```bash
     dotnet tool install --global dotnet-ef
     ```
   - Создайте миграцию:
     ```bash
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```

---

## 🔍 Доступ к данным

- **СУБД**: SQLite (`library.db`).
- **Технология**: Entity Framework Core.
- **Репозиторий**: `LibraryRepository` реализует CRUD через `ILibraryRepository`.
- **Сервис**: `LibraryService` добавляет проверки бизнес-логики.
- **Безопасность**: Параметризованные запросы, валидация данных.
- **Методы**:
  - 📝 CRUD для книг, авторов, читателей, заказов, отзывов.
  - ✅ Проверки: наличие копий, рейтинг 1-5, валидный email.

---

## 🧩 Основные классы

### 📖 Модели
- **Book**: Книга (название, автор, год, копии). Связана с заказами и отзывами.
- **Author**: Автор (имя, фамилия). Связан с книгами.
- **Reader**: Читатель (имя, фамилия, email). Связан с заказами и отзывами.
- **Order**: Заказ (ID книги, читателя, даты выдачи/возврата).
- **Review**: Отзыв (ID книги, читателя, рейтинг, комментарий).

### 💾 Данные
- **LibraryContext**: Контекст EF Core, настраивает таблицы и связи.
- **ILibraryRepository**: Интерфейс для операций с данными.
- **LibraryRepository**: Реализует CRUD и валидацию.

### ⚙️ Сервисы
- **ILibraryService**: Интерфейс бизнес-логики.
- **LibraryService**: Проверяет данные, вызывает репозиторий.

### 🎬 Программа
- **Program**: Консольное меню, вызывает `ILibraryService`.

---

