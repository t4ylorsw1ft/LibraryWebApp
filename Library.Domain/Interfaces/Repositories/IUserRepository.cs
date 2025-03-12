using Library.Domain.Entities;

namespace Library.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
        Task<User?> GetByRefreshToken(string email, CancellationToken cancellationToken);
        Task<User> AddAsync(User user, CancellationToken cancellationToken);
        Task<User> UpdateAsync(User user, CancellationToken cancellationToken);
        Task DeleteAsync(User user, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    }
}
