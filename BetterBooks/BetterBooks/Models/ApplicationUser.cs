using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public virtual ICollection<BookRequest> RequestedBooks { get; set; }
        //public virtual ICollection<Book> GivingAwayBooks { get; set; }

        public ApplicationUser()
        {
            //BookReviews = new HashSet<BookReview>();
            //BorrowedBooks = new HashSet<BorrowedBook>();
            Books = new HashSet<Book>();
            RequestedBooks = new HashSet<BookRequest>();
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
}