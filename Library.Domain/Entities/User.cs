namespace Library.Domain.Entities
{
    public enum UserRole
    {
        User,
        Admin
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;

        public List<BookBorrow> BookBorrows { get; set; } = new List<BookBorrow>();
    }

}
