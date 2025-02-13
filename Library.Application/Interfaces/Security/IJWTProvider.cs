using Library.Domain.Entities;

namespace Library.Application.Interfaces.Security
{
    public interface IJWTProvider
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
