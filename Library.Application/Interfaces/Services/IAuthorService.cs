using Library.Application.DTOs.Authors;
using Library.Domain.Entities;

namespace Library.Application.Interfaces.Services
{
    public interface IAuthorService
    {
        Task<List<AuthorLookupDto>> GetAllPagedAsync(int page, int size);
        Task<AuthorDetailsDto> GetByIdAsync(Guid id);
        Task<AuthorDetailsDto> CreateAsync(CreateAuthorDto author);
        Task<AuthorDetailsDto> UpdateAsync(UpdateAuthorDto author);
        Task DeleteAsync(Guid id);
    }
}
