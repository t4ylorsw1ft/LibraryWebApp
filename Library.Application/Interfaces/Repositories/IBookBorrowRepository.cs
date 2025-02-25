using Library.Domain.Entities;

namespace Library.Application.Interfaces.Repositories
{
    public interface IBookBorrowRepository
    {
        Task<IEnumerable<BookBorrow>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<BookBorrow?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<BookBorrow> AddAsync(BookBorrow book, CancellationToken cancellationToken);
        Task<BookBorrow> UpdateAsync(BookBorrow book, CancellationToken cancellationToken);
        Task<bool> ActiveBorrowExistsAsync(Guid userId, Guid bookId, CancellationToken cancellationToken);
    }
}
