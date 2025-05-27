using System;
using biblioteka_main.Models;
using biblioteka_main.Services;

namespace biblioteka_main
{
    class Program
    {
        private static string connectionString = "Server=.;Database=LibraryDB;Trusted_Connection=True;";
        private static LibraryService service;

        static void Main(string[] args)
        {
            service = new LibraryService(connectionString);

            while (true)
            {
                Console.WriteLine("\n=== Система управления библиотекой ===");
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Просмотреть книги");
                Console.WriteLine("3. Обновить книгу");
                Console.WriteLine("4. Удалить книгу");
                Console.WriteLine("5. Добавить читателя");
                Console.WriteLine("6. Выдать книгу");
                Console.WriteLine("7. Вернуть книгу");
                Console.WriteLine("8. Добавить отзыв");
                Console.WriteLine("9. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddBook();
                            break;
                        case "2":
                            ViewBooks();
                            break;
                        case "3":
                            UpdateBook();
                            break;
                        case "4":
                            DeleteBook();
                            break;
                        case "5":
                            AddReader();
                            break;
                        case "6":
                            IssueBook();
                            break;
                        case "7":
                            ReturnBook();
                            break;
                        case "8":
                            AddReview();
                            break;
                        case "9":
                            return;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        static void AddBook()
        {
            Console.Write("Введите название книги: ");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название книги не может быть пустым.");

            Console.Write("Введите ID автора: ");
            if (!int.TryParse(Console.ReadLine(), out int authorId))
                throw new ArgumentException("Неверный формат ID автора.");

            Console.Write("Введите год публикации: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
                throw new ArgumentException("Неверный формат года.");

            Console.Write("Введите количество копий: ");
            if (!int.TryParse(Console.ReadLine(), out int copies) || copies < 0)
                throw new ArgumentException("Количество копий должно быть неотрицательным.");

            var book = new Book
            {
                Title = title,
                AuthorID = authorId,
                PublicationYear = year,
                AvailableCopies = copies
            };
            service.AddBook(book);
            Console.WriteLine("Книга добавлена успешно.");
        }

        static void ViewBooks()
        {
            service.ViewBooks();
        }

        static void UpdateBook()
        {
            Console.Write("Введите ID книги: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Неверный формат ID книги.");

            Console.Write("Введите новое название книги: ");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название книги не может быть пустым.");

            Console.Write("Введите новое количество копий: ");
            if (!int.TryParse(Console.ReadLine(), out int copies) || copies < 0)
                throw new ArgumentException("Количество копий должно быть неотрицательным.");

            service.UpdateBook(bookId, title, copies);
            Console.WriteLine("Книга обновлена успешно.");
        }

        static void DeleteBook()
        {
            Console.Write("Введите ID книги: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Неверный формат ID книги.");

            service.DeleteBook(bookId);
            Console.WriteLine("Книга удалена успешно.");
        }

        static void AddReader()
        {
            Console.Write("Введите имя читателя: ");
            string firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Имя не может быть пустым.");

            Console.Write("Введите фамилию читателя: ");
            string lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Фамилия не может быть пустой.");

            Console.Write("Введите email читателя: ");
            string email = Console.ReadLine();

            var reader = new Reader
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };
            service.AddReader(reader);
            Console.WriteLine("Читатель добавлен успешно.");
        }

        static void IssueBook()
        {
            Console.Write("Введите ID книги: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Неверный формат ID книги.");

            Console.Write("Введите ID читателя: ");
            if (!int.TryParse(Console.ReadLine(), out int readerId))
                throw new ArgumentException("Неверный формат ID читателя.");

            service.IssueBook(bookId, readerId);
            Console.WriteLine("Книга выдана успешно.");
        }

        static void ReturnBook()
        {
            Console.Write("Введите ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
                throw new ArgumentException("Неверный формат ID заказа.");

            service.ReturnBook(orderId);
            Console.WriteLine("Книга возвращена успешно.");
        }

        static void AddReview()
        {
            Console.Write("Введите ID книги: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Неверный формат ID книги.");

            Console.Write("Введите ID читателя: ");
            if (!int.TryParse(Console.ReadLine(), out int readerId))
                throw new ArgumentException("Неверный формат ID читателя.");

            Console.Write("Введите рейтинг (1-5): ");
            if (!int.TryParse(Console.ReadLine(), out int rating) || rating < 1 || rating > 5)
                throw new ArgumentException("Рейтинг должен быть от 1 до 5.");

            Console.Write("Введите комментарий: ");
            string comment = Console.ReadLine();

            var review = new Review
            {
                BookID = bookId,
                ReaderID = readerId,
                Rating = rating,
                Comment = comment
            };
            service.AddReview(review);
            Console.WriteLine("Отзыв добавлен успешно.");
        }
    }
}