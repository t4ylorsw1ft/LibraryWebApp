using Library.Domain.Entities;

namespace Library.Application.Interfaces.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Author>> GetAllPagedAsync(int page, int size, CancellationToken cancellationToken);
        Task<Author> AddAsync(Author author, CancellationToken cancellationToken);
        Task<Author> UpdateAsync(Author author, CancellationToken cancellationToken);
        Task DeleteAsync(Author author, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    }
}
