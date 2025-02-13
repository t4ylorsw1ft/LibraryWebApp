using Library.Domain.Entities;

namespace Library.Application.Interfaces.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author?> GetByIdAsync(Guid id);
        Task<IEnumerable<Author>> GetAllPagedAsync(int page, int size);
        Task<Author> AddAsync(Author author);
        Task<Author> UpdateAsync(Author author);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
