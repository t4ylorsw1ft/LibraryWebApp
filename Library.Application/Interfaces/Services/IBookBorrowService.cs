using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces.Services
{
    public interface IBookBorrowService
    {
        Task BorrowBookAsync(Guid userId, Guid bookId);
        Task ReturnBookAsync(Guid bookBorrowId);
        Task<IEnumerable<BookBorrow>> GetAllByUserAsync(Guid userId);
    }
}
