using Library.Domain.Entities;

namespace Library.Application.Interfaces.Security
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
