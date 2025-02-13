using Library.Domain.Entities;

namespace Library.Application.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllPagedAsync(int page, int size);
        Task<IEnumerable<Book>> GetAllByAuthorAsync(Guid AuthorId);
        Task<IEnumerable<Book>> GetAllByUserAsync(Guid UserId);
        Task<Book?> GetByIdAsync(Guid id);
        Task<Book?> GetByISBNAsync(string isbn);
        Task<Book> AddAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }

}
