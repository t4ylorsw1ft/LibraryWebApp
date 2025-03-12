namespace Library.Application.Interfaces.Security
{
    public interface IJwtValidator
    {
        bool ValidateTokenByExpiration(string token);
    }
}