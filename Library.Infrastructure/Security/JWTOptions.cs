namespace Library.Infrastructure.Security
{
    public class JWTOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresHours { get; set; }
        public int RefreshExpiresDays { get; set; }
    }
}
