namespace BetterBooks.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BookReview")]
    public partial class BookReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }


        [ForeignKey("Book")]
        public int BookId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [StringLength(500)]
        public string Review { get; set; }
        public int RateValue { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Book Book { get; set; }
    }
}
