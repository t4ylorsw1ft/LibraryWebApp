using Library.Domain.Entities;

namespace Library.Domain.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllPagedAsync(int page, int size, CancellationToken cancellationToken);
        Task<IEnumerable<Book>> GetAllByAuthorAsync(Guid AuthorId, CancellationToken cancellationToken);
        Task<IEnumerable<Book>> GetAllByUserAsync(Guid UserId, CancellationToken cancellationToken);
        Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Book?> GetByISBNAsync(string isbn, CancellationToken cancellationToken);
        Task<Book> AddAsync(Book book, CancellationToken cancellationToken);
        Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken);
        Task DeleteAsync(Book book, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    }

}
