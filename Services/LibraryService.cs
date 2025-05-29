using System;
using System.Collections.Generic;
using biblioteka_main.DataAccess;
using biblioteka_main.Models;

namespace biblioteka_main.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryRepository _repository;

        public LibraryService(ILibraryRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void AddBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Book title cannot be empty.");

            if (book.PublicationYear < 0 || book.PublicationYear > DateTime.Now.Year)
                throw new ArgumentException("Invalid publication year.");

            if (book.AvailableCopies < 0)
                throw new ArgumentException("Available copies cannot be negative.");

            _repository.AddBook(book);
        }

        public List<Book> ViewAllBooks()
        {
            return _repository.ViewAllBooks();
        }

        public Book ViewBookDetails(int bookId)
        {
            if (bookId <= 0)
                throw new ArgumentException("Invalid book ID.");
            return _repository.ViewBookDetails(bookId);
        }

        public void UpdateBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Book title cannot be empty.");

            if (book.PublicationYear < 0 || book.PublicationYear > DateTime.Now.Year)
                throw new ArgumentException("Invalid publication year.");

            if (book.AvailableCopies < 0)
                throw new ArgumentException("Available copies cannot be negative.");

            _repository.UpdateBook(book);
        }

        public void RemoveBook(int bookId)
        {
            if (bookId <= 0)
                throw new ArgumentException("Invalid book ID.");
            _repository.RemoveBook(bookId);
        }

        public void AddAuthor(Author author)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));

            if (string.IsNullOrWhiteSpace(author.FirstName) || string.IsNullOrWhiteSpace(author.LastName))
                throw new ArgumentException("Author's first name and last name cannot be empty.");

            _repository.AddAuthor(author);
        }

        public void RegisterReader(Reader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (string.IsNullOrWhiteSpace(reader.FirstName) || string.IsNullOrWhiteSpace(reader.LastName))
                throw new ArgumentException("Reader's first name and last name cannot be empty.");

            if (string.IsNullOrWhiteSpace(reader.Email) || !reader.Email.Contains("@"))
                throw new ArgumentException("Invalid email address.");

            _repository.RegisterReader(reader);
        }

        public void IssueBookToReader(int bookId, int readerId)
        {
            if (bookId <= 0)
                throw new ArgumentException("Invalid book ID.");

            if (readerId <= 0)
                throw new ArgumentException("Invalid reader ID.");

            _repository.IssueBookToReader(bookId, readerId);
        }

        public void ReturnBookToLibrary(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Invalid order ID.");
            _repository.ReturnBookToLibrary(orderId);
        }

        public void AddBookReview(Review review)
        {
            if (review == null)
                throw new ArgumentNullException(nameof(review));

            if (review.BookID <= 0)
                throw new ArgumentException("Invalid book ID.");

            if (review.ReaderID <= 0)
                throw new ArgumentException("Invalid reader ID.");

            if (review.Rating < 1 || review.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            _repository.AddBookReview(review);
        }

        public List<Review> ViewBookReviews(int bookId)
        {
            if (bookId <= 0)
                throw new ArgumentException("Invalid book ID.");
            return _repository.ViewBookReviews(bookId);
        }

        public List<Book> SearchBooks(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("Search term cannot be empty.");
            return _repository.SearchBooks(searchTerm);
        }
    }
}