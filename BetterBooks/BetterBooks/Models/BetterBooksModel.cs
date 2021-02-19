using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BetterBooks.Models
{
    public partial class BetterBooksModel : DbContext
    {
        public BetterBooksModel()
            : base("name=BetterBooksModel")
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<BookReview> BookReviews { get; set; }
        public virtual DbSet<BorrowedBook> BorrowedBooks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Books)
                .Map(m => m.ToTable("BookRequest").MapLeftKey("bookId").MapRightKey("userId"));

            modelBuilder.Entity<Book>()
                .HasMany(e => e.OfferedToUsers)
                .WithMany(e => e.BooksOfferedToMe)
                .Map(m => m.ToTable("BookOffer").MapLeftKey("bookId").MapRightKey("userId"));

            modelBuilder.Entity<Book>()
                .HasMany(e => e.GivenAwayByUsers)
                .WithMany(e => e.GivingAwayBooks)
                .Map(m => m.ToTable("GiveAway").MapLeftKey("bookId").MapRightKey("userId"));

            modelBuilder.Entity<User>()
                .HasMany(e => e.BorrowedBooks)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.bookId);
        }
    }
}
