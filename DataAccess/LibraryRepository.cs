using System;
using System.Data.SqlClient;
using biblioteka_main.Models;

namespace biblioteka_main.DataAccess
{
    public class LibraryRepository
    {
        private readonly string connectionString;

        public LibraryRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddBook(Book book)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Books (Title, AuthorID, PublicationYear, AvailableCopies) VALUES (@Title, @AuthorID, @PublicationYear, @AvailableCopies)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@AuthorID", book.AuthorID);
                    cmd.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);
                    cmd.Parameters.AddWithValue("@AvailableCopies", book.AvailableCopies);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ViewBooks()
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

        public void UpdateBook(int bookId, string title, int copies)
        {
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
        }

        public void DeleteBook(int bookId)
        {
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
        }

        public void AddReader(Reader reader)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Readers (FirstName, LastName, Email) VALUES (@FirstName, @LastName, @Email)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", reader.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", reader.LastName);
                    cmd.Parameters.AddWithValue("@Email", reader.Email);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void IssueBook(int bookId, int readerId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string checkQuery = "SELECT AvailableCopies FROM Books WHERE BookID = @BookID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@BookID", bookId);
                    object result = checkCmd.ExecuteScalar();
                    if (result == null || (int)result <= 0)
                        throw new Exception("Книга недоступна или не существует.");
                }

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
        }

        public void ReturnBook(int orderId)
        {
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
        }

        public void AddReview(Review review)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Reviews (BookID, ReaderID, Rating, Comment) VALUES (@BookID, @ReaderID, @Rating, @Comment)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookID", review.BookID);
                    cmd.Parameters.AddWithValue("@ReaderID", review.ReaderID);
                    cmd.Parameters.AddWithValue("@Rating", review.Rating);
                    cmd.Parameters.AddWithValue("@Comment", review.Comment);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}