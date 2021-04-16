using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterBooks.Models
{
    public class BookVM
    {
        public Book Book { get; set; }
        public int RevCount { get; set; }
    }
}