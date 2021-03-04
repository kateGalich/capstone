using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using BetterBooks.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BetterBooks.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(150)]
        public string HomeAddress { get; set; }

        //public virtual ICollection<BookReview> BookReviews { get; set; }
        //public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Book> RequestedBooks { get; set; }
        //public virtual ICollection<Book> GivingAwayBooks { get; set; }

        public ApplicationUser()
        {
            //BookReviews = new HashSet<BookReview>();
            //BorrowedBooks = new HashSet<BorrowedBook>();
            Books = new HashSet<Book>();
            RequestedBooks = new HashSet<Book>();
            //BooksOfferedToMe = new HashSet<Book>();
            //GivingAwayBooks = new HashSet<Book>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

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