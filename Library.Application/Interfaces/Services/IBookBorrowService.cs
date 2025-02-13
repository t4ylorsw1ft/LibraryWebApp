using Library.Domain.Entities;

namespace Library.Application.Interfaces.Services
{
    public interface IBookBorrowService
    {
        Task BorrowBookAsync(Guid userId, Guid bookId);
        Task ReturnBookAsync(Guid bookBorrowId);
        Task<IEnumerable<BookBorrow>> GetAllByUserAsync(Guid userId);
    }
}
