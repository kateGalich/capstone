using System.Data.Entity;
using BetterBooks.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BetterBooks.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>(useSuppliedContext: true));
            Database.Initialize(true);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Book> Books { get; set; }
        //public virtual DbSet<BookReview> BookReviews { get; set; }
        //public virtual DbSet<BorrowedBook> BorrowedBooks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasMany(book => book.RequestedByUsers)
                .WithMany(user => user.RequestedBooks)
                .Map(m => m.ToTable("RequestedBooks").MapLeftKey("bookId").MapRightKey("userId"));

            //modelBuilder.Entity<Book>()
            //    .HasMany(e => e.Users)
            //    .WithMany(e => e.Books)
            //    .Map(m => m.ToTable("BookRequest").MapLeftKey("bookId").MapRightKey("userId"));

            //modelBuilder.Entity<Book>()
            //    .HasMany(e => e.OfferedToUsers)
            //    .WithMany(e => e.BooksOfferedToMe)
            //    .Map(m => m.ToTable("BookOffer").MapLeftKey("bookId").MapRightKey("userId"));

            //modelBuilder.Entity<Book>()
            //    .HasMany(e => e.GivenAwayByUsers)
            //    .WithMany(e => e.GivingAwayBooks)
            //    .Map(m => m.ToTable("GiveAway").MapLeftKey("bookId").MapRightKey("userId"));

            //modelBuilder.Entity<ApplicationUser>()
            //    .HasMany(e => e.BorrowedBooks)
            //    .WithRequired(e => e.User)
            //    .HasForeignKey(e => e.bookId);
        }
    }
}