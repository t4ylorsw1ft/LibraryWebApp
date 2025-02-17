namespace Library.Application.DTOs.Users
{
    public class JwtPairDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
