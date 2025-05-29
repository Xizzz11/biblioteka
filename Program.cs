using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using biblioteka_main.DataAccess;
using biblioteka_main.Models;
using biblioteka_main.Services;
using Microsoft.EntityFrameworkCore;

namespace biblioteka_main
{
    class Program
    {
        private const string ConnectionString = "Data Source=library.db";
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            // Настройка DI контейнера
            ConfigureServices();

            // Получение сервиса
            var libraryService = _serviceProvider.GetService<ILibraryService>();

            Console.WriteLine("=== Система управления библиотекой ===");

            while (true)
            {
                try
                {
                    DisplayMenu();
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddBook(libraryService);
                            break;
                        case "2":
                            ViewAllBooks(libraryService);
                            break;
                        case "3":
                            ViewBookDetails(libraryService);
                            break;
                        case "4":
                            UpdateBook(libraryService);
                            break;
                        case "5":
                            DeleteBook(libraryService);
                            break;
                        case "6":
                            AddAuthor(libraryService);
                            break;
                        case "7":
                            RegisterReader(libraryService);
                            break;
                        case "8":
                            IssueBook(libraryService);
                            break;
                        case "9":
                            ReturnBook(libraryService);
                            break;
                        case "10":
                            AddReview(libraryService);
                            break;
                        case "11":
                            ViewBookReviews(libraryService);
                            break;
                        case "12":
                            SearchBooks(libraryService);
                            break;
                        case "0":
                            Console.WriteLine("Выход из программы...");
                            return;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Регистрация зависимостей
            services.AddSingleton(ConnectionString);
            services.AddDbContext<LibraryContext>(options => options.UseSqlite(ConnectionString));
            services.AddTransient<ILibraryRepository, LibraryRepository>();
            services.AddTransient<ILibraryService, LibraryService>();
            services.AddLogging(builder => builder.AddConsole());

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("\n=== Главное меню ===");
            Console.WriteLine("1. Добавить книгу");
            Console.WriteLine("2. Просмотреть все книги");
            Console.WriteLine("3. Просмотреть детали книги");
            Console.WriteLine("4. Обновить информацию о книге");
            Console.WriteLine("5. Удалить книгу");
            Console.WriteLine("6. Добавить автора");
            Console.WriteLine("7. Зарегистрировать читателя");
            Console.WriteLine("8. Выдать книгу читателю");
            Console.WriteLine("9. Вернуть книгу в библиот Barlow");
            Console.WriteLine("10. Добавить отзыв о книге");
            Console.WriteLine("11. Просмотреть отзывы о книге");
            Console.WriteLine("12. Поиск книг");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");
        }

        private static void AddBook(ILibraryService service)
        {
            Console.Clear();
            Console.WriteLine("=== Добавление новой книги ===");

            var book = new Book();

            Console.Write("Введите название книги: ");
            book.Title = Console.ReadLine();

            Console.Write("Введите ID автора: ");
            if (!int.TryParse(Console.ReadLine(), out int authorId))
                throw new ArgumentException("Неверный формат ID автора.");
            book.AuthorID = authorId;

            Console.Write("Введите год публикации: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
                throw new ArgumentException("Неверный формат года.");
            book.PublicationYear = year;

            Console.Write("Введите количество копий: ");
            if (!int.TryParse(Console.ReadLine(), out int copies))
                throw new ArgumentException("Неверный формат количества копий.");
            book.AvailableCopies = copies;

            service.AddBook(book);
            Console.WriteLine("Книга успешно добавлена!");
        }

        private static void ViewAllBooks(ILibraryService service)
        {
            Console.Clear();
            Console.WriteLine("=== Список всех книг ===");

            var books = service.ViewAllBooks().ToList();

            if (!books.Any())
            {
                Console.WriteLine("В библиотеке нет книг.");
                return;
            }

            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.BookID}");
                Console.WriteLine($"Название: {book.Title}");
                Console.WriteLine($"Автор: {book.AuthorName}");
                Console.WriteLine($"Год: {book.PublicationYear}");
                Console.WriteLine($"Доступно копий: {book.AvailableCopies}");
                Console.WriteLine(new string('-', 30));
            }
        }

        private static void ViewBookDetails(ILibraryService service)
        {
            Console.Clear();
            Console.WriteLine("=== Просмотр информации о книге ===");

            Console.Write("Введите ID книги: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Неверный формат ID книги.");

            var book = service.ViewBookDetails(bookId);

            Console.WriteLine($"\nID: {book.BookID}");
            Console.WriteLine($"Название: {book.Title}");
            Console.WriteLine($"Автор: {book.AuthorName}");
            Console.WriteLine($"Год публикации: {book.PublicationYear}");
            Console.WriteLine($"Доступно копий: {book.AvailableCopies}");
        }

        private static void UpdateBook(ILibraryService service)
        {
            Console.Clear();
            Console.WriteLine("=== Обновление информации о книге ===");

            Console.Write("Введите ID книги для обновления: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Неверный формат ID книги.");

            var book = service.ViewBookDetails(bookId);

            Console.WriteLine($"\nТекущая информация:");
            Console.WriteLine($"Название: {book.Title}");
            Console.WriteLine($"Автор (ID): {book.AuthorID}");
            Console.WriteLine($"Год публикации: {book.PublicationYear}");
            Console.WriteLine($"Доступно копий: {book.AvailableCopies}");

            Console.Write("\nВведите новое название (оставьте пустым, чтобы не менять): ");
            var newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
                book.Title = newTitle;

            Console.Write("Введите новый ID автора (оставьте пустым, чтобы не менять): ");
            var authorInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(authorInput)
                && int.TryParse(authorInput, out int newAuthorId))
                book.AuthorID = newAuthorId;

            Console.Write("Введите новый год публикации (оставьте пустым, чтобы не менять): ");
            var yearInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(yearInput)
                && int.TryParse(yearInput, out int newYear))
                book.PublicationYear = newYear;

            Console.Write("Введите новое количество копий (оставьте пустым, чтобы не менять): ");
            var copiesInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(copiesInput)
                && int.TryParse(copiesInput, out int newCopies))
                book.AvailableCopies = newCopies;

            service.UpdateBook(book);
            Console.WriteLine("Информация о книге успешно обновлена!");
        }

        private static void DeleteBook(ILibraryService service)
        {
            Console.Clear();
            Console.WriteLine("=== Удаление книги ===");

            Console.Write("Введите ID книги для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Неверный формат ID книги.");

            service.RemoveBook(bookId);
            Console.WriteLine("Книга успешно удалена!");
        }

        private static void AddAuthor(ILibraryService service)
        {
            Console.Clear();
            Console.WriteLine("=== Добавление нового автора ===");

            var author = new Author();

            Console.Write("Введите имя автора: ");
            author.FirstName = Console.ReadLine();

            Console.Write("Введите фамилию автора: ");
            author.LastName = Console.ReadLine();

            service.AddAuthor(author);
            Console.WriteLine("Автор успешно добавлен!");
        }

        private static void RegisterReader(ILibraryService service)
        {
            Console.Clear();
            Console.WriteLine("=== Регистрация нового читателя ===");

            var reader = new Reader();

            Console.Write("Введите имя читателя: ");
            reader.FirstName = Console.ReadLine();

            Console.Write("Введите фамилию читателя: ");
            reader.LastName = Console.ReadLine();

            Console.Write("Введите email читателя: ");
            reader.Email = Console.ReadLine();

            service.RegisterReader(reader);
            Console.WriteLine("Читатель успешно зарегистрирован!");
        }

        private static void IssueBook(ILibraryService service)
        {
            Console.Clear();
            Console.WriteLine("=== Выдача книги читателю ===");

            Console.Write("Введите ID книги: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Неверный формат ID книги.");

            Console.Write("Введите ID читателя: ");
            if (!int.TryParse(Console.ReadLine(), out int readerId))
                throw new ArgumentException("Неверный формат ID читателя.");

            service.IssueBookToReader(bookId, readerId);
            Console.WriteLine("Книга успешно выдана читателю!");
        }

        private static void ReturnBook(ILibraryService service)
        {
            Console.Clear();
            Console.WriteLine("=== Возврат книги в библиотеку ===");

            Console.Write("Введите ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
                throw new ArgumentException("Неверный формат ID заказа.");

            service.ReturnBookToLibrary(orderId);
            Console.WriteLine("Книга успешно возвращена в библиотеку!");
        }

        private static void AddReview(ILibraryService service)
        {
            Console.Clear();
            Console.WriteLine("=== Добавление отзыва о книге ===");

            var review = new Review();

            Console.Write("Введите ID книги: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Неверный формат ID книги.");
            review.BookID = bookId;

            Console.Write("Введите ID читателя: ");
            if (!int.TryParse(Console.ReadLine(), out int readerId))
                throw new ArgumentException("Неверный формат ID читателя.");
            review.ReaderID = readerId;

            Console.Write("Введите оценку (1-5): ");
            if (!int.TryParse(Console.ReadLine(), out int rating) || rating < 1 || rating > 5)
                throw new ArgumentException("Оценка должна быть от 1 до 5.");
            review.Rating = rating;

            Console.Write("Введите комментарий: ");
            review.Comment = Console.ReadLine();

            service.AddBookReview(review);
            Console.WriteLine("Отзыв успешно добавлен!");
        }

        private static void ViewBookReviews(ILibraryService service)
        {
            Console.Clear();
            Console.WriteLine("=== Просмотр отзывов о книге ===");

            Console.Write("Введите ID книги: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Неверный формат ID книги.");

            var reviews = service.ViewBookReviews(bookId).ToList();

            if (!reviews.Any())
            {
                Console.WriteLine("Для этой книги пока нет отзывов.");
                return;
            }

            Console.WriteLine($"\nОтзывы о книге (ID: {bookId}):");
            foreach (var review in reviews)
            {
                Console.WriteLine($"Оценка: {review.Rating}/5");
                Console.WriteLine($"Комментарий: {review.Comment}");
                Console.WriteLine(new string('-', 30));
            }
        }

        private static void SearchBooks(ILibraryService service)
        {
            Console.Clear();
            Console.WriteLine("=== Поиск книг ===");

            Console.Write("Введите поисковый запрос (название или автор): ");
            var searchTerm = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine("Поисковый запрос не может быть пустым.");
                return;
            }

            var books = service.SearchBooks(searchTerm).ToList();

            if (!books.Any())
            {
                Console.WriteLine("По вашему запросу ничего не найдено.");
                return;
            }

            Console.WriteLine("\nРезультаты поиска:");
            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.BookID}");
                Console.WriteLine($"Название: {book.Title}");
                Console.WriteLine($"Автор: {book.AuthorName}");
                Console.WriteLine(new string('-', 30));
            }
        }
    }
}