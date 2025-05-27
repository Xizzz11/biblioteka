using biblioteka_main.DataAccess;
using biblioteka_main.Models;

namespace biblioteka_main.Services
{
    public class LibraryService
    {
        private readonly LibraryRepository repository;

        public LibraryService(string connectionString)
        {
            repository = new LibraryRepository(connectionString);
        }

        public void AddBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Название книги не может быть пустым.");
            if (book.AvailableCopies < 0)
                throw new ArgumentException("Количество копий должно быть неотрицательным.");

            repository.AddBook(book);
        }

        public void ViewBooks()
        {
            repository.ViewBooks();
        }

        public void UpdateBook(int bookId, string title, int copies)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название книги не может быть пустым.");
            if (copies < 0)
                throw new ArgumentException("Количество копий должно быть неотрицательным.");

            repository.UpdateBook(bookId, title, copies);
        }

        public void DeleteBook(int bookId)
        {
            repository.DeleteBook(bookId);
        }

        public void AddReader(Reader reader)
        {
            if (string.IsNullOrWhiteSpace(reader.FirstName))
                throw new ArgumentException("Имя не может быть пустым.");
            if (string.IsNullOrWhiteSpace(reader.LastName))
                throw new ArgumentException("Фамилия не может быть пустой.");

            repository.AddReader(reader);
        }

        public void IssueBook(int bookId, int readerId)
        {
            repository.IssueBook(bookId, readerId);
        }

        public void ReturnBook(int orderId)
        {
            repository.ReturnBook(orderId);
        }

        public void AddReview(Review review)
        {
            if (review.Rating < 1 || review.Rating > 5)
                throw new ArgumentException("Рейтинг должен быть от 1 до 5.");

            repository.AddReview(review);
        }
    }
}