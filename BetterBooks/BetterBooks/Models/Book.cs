namespace BetterBooks.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Book
    {
        public Book()
        {
            RequestedByUsers = new HashSet<BookRequest>();
            //OfferedToUsers = new HashSet<ApplicationUser>();
            //GivenAwayByUsers = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title {get; set;}

        [Required]
        [StringLength(50)]
        public string Author { get; set; }

        [StringLength(100)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        public int? Year { get; set; }

        public virtual ICollection<BookRequest> RequestedByUsers { get; set; }
        //public virtual ICollection<ApplicationUser> OfferedToUsers { get; set; }
        //public virtual ICollection<ApplicationUser> GivenAwayByUsers { get; set; }
    }
}
