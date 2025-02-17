using Library.Application.DTOs.Users;
using Library.Domain.Entities;

namespace Library.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<JwtPairDto> RegisterAsync(RegisterDto registerDto);
        Task<JwtPairDto> LoginAsync(LoginDto loginDto);
        Task<JwtPairDto> RefreshAsync(RefreshDto refreshDto);
    }
}
