using Library.Domain.Entities;

namespace Library.Application.Interfaces.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllPagedAsync(int page, int size);
        Task<Author> GetByIdAsync(Guid id);
        Task<Author> CreateAsync(Author author);
        Task<Author> UpdateAsync(Author author);
        Task DeleteAsync(Guid id);
    }
}
