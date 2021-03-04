namespace BetterBooks.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BorrowedBook
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int bookId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string borrowerId { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime start { get; set; }

        [Column(TypeName = "date")]
        public DateTime? until { get; set; }

        public virtual Book Book { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
