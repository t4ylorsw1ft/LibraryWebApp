using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string username, string email, string password);
        Task<JwtPairDto> LoginAsync(string email, string password);
        Task<JwtPairDto> RefreshAsync(string refreshToken);
    }
}
