using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Library.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int AvaliableQuantity { get; set; }

        public string? ImagePath { get; set; }

        public Guid AuthorId { get; set; }
        public Author Author { get; set; }

        public List<BookBorrow> BookBorrows { get; set; } = new List<BookBorrow>();
    }
}
