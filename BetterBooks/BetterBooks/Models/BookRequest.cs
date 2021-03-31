namespace BetterBooks.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BookRequest
    {
        [Key, Column(Order = 1)]
        public string UserId { get; set; }

        [Key, Column(Order = 2)]
        public int BookId { get; set; }

        public string Status { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Book Book { get; set; }
    }
}
