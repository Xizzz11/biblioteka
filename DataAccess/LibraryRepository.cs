using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using biblioteka_main.Models;

namespace biblioteka_main.DataAccess
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly LibraryContext _context;

        public LibraryRepository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            var author = _context.Authors.Find(book.AuthorID);
            if (author == null)
                throw new ArgumentException("Author not found.");

            book.AuthorName = $"{author.FirstName} {author.LastName}";
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public List<Book> ViewAllBooks()
        {
            return _context.Books
                .Include(b => b.Author)
                .ToList();
        }

        public Book ViewBookDetails(int bookId)
        {
            var book = _context.Books
                .Include(b => b.Author)
                .FirstOrDefault(b => b.BookID == bookId);
            if (book == null)
                throw new ArgumentException("Book not found.");
            return book;
        }

        public void UpdateBook(Book book)
        {
            var existingBook = _context.Books.Find(book.BookID);
            if (existingBook == null)
                throw new ArgumentException("Book not found.");

            var author = _context.Authors.Find(book.AuthorID);
            if (author == null)
                throw new ArgumentException("Author not found.");

            existingBook.Title = book.Title;
            existingBook.AuthorID = book.AuthorID;
            existingBook.AuthorName = $"{author.FirstName} {author.LastName}";
            existingBook.PublicationYear = book.PublicationYear;
            existingBook.AvailableCopies = book.AvailableCopies;

            _context.SaveChanges();
        }

        public void RemoveBook(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if (book == null)
                throw new ArgumentException("Book not found.");

            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public void AddAuthor(Author author)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));

            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        public void RegisterReader(Reader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            _context.Readers.Add(reader);
            _context.SaveChanges();
        }

        public void IssueBookToReader(int bookId, int readerId)
        {
            var book = _context.Books.Find(bookId);
            if (book == null)
                throw new ArgumentException("Book not found.");

            if (book.AvailableCopies <= 0)
                throw new ArgumentException("No available copies of the book.");

            var reader = _context.Readers.Find(readerId);
            if (reader == null)
                throw new ArgumentException("Reader not found.");

            var order = new Order
            {
                BookID = bookId,
                ReaderID = readerId,
                IssueDate = DateTime.Now
            };

            book.AvailableCopies--;
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void ReturnBookToLibrary(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null)
                throw new ArgumentException("Order not found.");

            if (order.ReturnDate != null)
                throw new ArgumentException("Book already returned.");

            var book = _context.Books.Find(order.BookID);
            if (book == null)
                throw new ArgumentException("Book not found.");

            order.ReturnDate = DateTime.Now;
            book.AvailableCopies++;
            _context.SaveChanges();
        }

        public void AddBookReview(Review review)
        {
            if (review == null)
                throw new ArgumentNullException(nameof(review));

            var book = _context.Books.Find(review.BookID);
            if (book == null)
                throw new ArgumentException("Book not found.");

            var reader = _context.Readers.Find(review.ReaderID);
            if (reader == null)
                throw new ArgumentException("Reader not found.");

            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        public List<Review> ViewBookReviews(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if (book == null)
                throw new ArgumentException("Book not found.");

            return _context.Reviews
                .Where(r => r.BookID == bookId)
                .ToList();
        }

        public List<Book> SearchBooks(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("Search term cannot be empty.");

            return _context.Books
                .Include(b => b.Author)
                .Where(b => b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            b.AuthorName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}