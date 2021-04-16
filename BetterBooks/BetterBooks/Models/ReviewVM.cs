using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterBooks.Models
{
    public class ReviewVM
    {
       public List<BookReview> BookReviews { get; set; }
       public BookReview BookReview { get; set; }
        public Book Book { get; set; }
        public int Revcount { get; set; }
        
    }
}