using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.BookBorrows
{
    public class BorrowBookDto
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }

    }
}
