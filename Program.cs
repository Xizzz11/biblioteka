using System;
using System.Data.SqlClient;

namespace LibraryManagement
{
    class Program
    {
        private static string connectionString = "Server=.;Database=LibraryDB;Trusted_Connection=True;";

        static void Main(string[] args)
        {
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Books (Title, AuthorID, PublicationYear, AvailableCopies) VALUES (@Title, @AuthorID, @PublicationYear, @AvailableCopies)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@AuthorID", authorId);
                    cmd.Parameters.AddWithValue("@PublicationYear", year);
                    cmd.Parameters.AddWithValue("@AvailableCopies", copies);
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Книга добавлена успешно.");
        }

        static void ViewBooks()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT b.BookID, b.Title, a.FirstName, a.LastName, b.PublicationYear, b.AvailableCopies " +
                               "FROM Books b JOIN Authors a ON b.AuthorID = a.AuthorID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["BookID"]}, Название: {reader["Title"]}, " +
                                              $"Автор: {reader["FirstName"]} {reader["LastName"]}, " +
                                              $"Год: {reader["PublicationYear"]}, Копии: {reader["AvailableCopies"]}");
                        }
                    }
                }
            }
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Books SET Title = @Title, AvailableCopies = @AvailableCopies WHERE BookID = @BookID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@AvailableCopies", copies);
                    cmd.Parameters.AddWithValue("@BookID", bookId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new Exception("Книга с таким ID не найдена.");
                }
            }
            Console.WriteLine("Книга обновлена успешно.");
        }

        static void DeleteBook()
        {
            Console.Write("Введите ID книги: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
                throw new ArgumentException("Неверный формат ID книги.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Books WHERE BookID = @BookID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookID", bookId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new Exception("Книга с таким ID не найдена.");
                }
            }
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Readers (FirstName, LastName, Email) VALUES (@FirstName, @LastName, @Email)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.ExecuteNonQuery();
                }
            }
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Проверка доступности книги
                string checkQuery = "SELECT AvailableCopies FROM Books WHERE BookID = @BookID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@BookID", bookId);
                    object result = checkCmd.ExecuteScalar();
                    if (result == null || (int)result <= 0)
                        throw new Exception("Книга недоступна или не существует.");
                }

                // Выдача книги
                string query = "INSERT INTO Orders (BookID, ReaderID, IssueDate) VALUES (@BookID, @ReaderID, @IssueDate); " +
                              "UPDATE Books SET AvailableCopies = AvailableCopies - 1 WHERE BookID = @BookID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookID", bookId);
                    cmd.Parameters.AddWithValue("@ReaderID", readerId);
                    cmd.Parameters.AddWithValue("@IssueDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Книга выдана успешно.");
        }

        static void ReturnBook()
        {
            Console.Write("Введите ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
                throw new ArgumentException("Неверный формат ID заказа.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Orders SET ReturnDate = @ReturnDate WHERE OrderID = @OrderID; " +
                              "UPDATE Books SET AvailableCopies = AvailableCopies + 1 WHERE BookID = (SELECT BookID FROM Orders WHERE OrderID = @OrderID)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new Exception("Заказ с таким ID не найден.");
                }
            }
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Reviews (BookID, ReaderID, Rating, Comment) VALUES (@BookID, @ReaderID, @Rating, @Comment)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookID", bookId);
                    cmd.Parameters.AddWithValue("@ReaderID", readerId);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    cmd.Parameters.AddWithValue("@Comment", comment);
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Отзыв добавлен успешно.");
        }
    }
}