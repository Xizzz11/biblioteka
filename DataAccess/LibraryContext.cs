using Microsoft.EntityFrameworkCore;
using biblioteka_main.Models;

namespace biblioteka_main.DataAccess
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }

        private readonly string _connectionString;

        public LibraryContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany()
                .HasForeignKey(b => b.AuthorID);

            modelBuilder.Entity<Order>()
                .HasOne<Book>()
                .WithMany(b => b.Orders)
                .HasForeignKey(o => o.BookID);

            modelBuilder.Entity<Order>()
                .HasOne<Reader>()
                .WithMany()
                .HasForeignKey(o => o.ReaderID);

            modelBuilder.Entity<Review>()
                .HasOne<Book>()
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookID);

            modelBuilder.Entity<Review>()
                .HasOne<Reader>()
                .WithMany()
                .HasForeignKey(r => r.ReaderID);
        }
    }
}