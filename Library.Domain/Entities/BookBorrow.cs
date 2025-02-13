namespace Library.Domain.Entities
{
    public class BookBorrow
    {
        public Guid Id { get; set; }

        public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;
        public DateTime ReturnBy { get; set; } = DateTime.UtcNow.AddDays(14);
        public bool IsReturned { get; set; } = false;

        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

    }
}
