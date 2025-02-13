using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces.Repositories
{
    public interface IBookBorrowRepository
    {
        Task<IEnumerable<BookBorrow>> GetAllByUserAsync(Guid userId);
        Task<BookBorrow?> GetByIdAsync(Guid id);
        Task<BookBorrow> AddAsync(BookBorrow book);
        Task<BookBorrow> UpdateAsync(BookBorrow book);
        Task<bool> ActiveBorrowExistsAsync(Guid userId, Guid bookId);
    }
}
