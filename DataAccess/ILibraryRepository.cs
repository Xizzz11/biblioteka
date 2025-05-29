using System.Collections.Generic;
using biblioteka_main.Models;

namespace biblioteka_main.DataAccess
{
    public interface ILibraryRepository
    {
        void AddBook(Book book);
        List<Book> ViewAllBooks();
        Book ViewBookDetails(int bookId);
        void UpdateBook(Book book);
        void RemoveBook(int bookId);
        void AddAuthor(Author author);
        void RegisterReader(Reader reader);
        void IssueBookToReader(int bookId, int readerId);
        void ReturnBookToLibrary(int orderId);
        void AddBookReview(Review review);
        List<Review> ViewBookReviews(int bookId);
        List<Book> SearchBooks(string searchTerm);
    }
}