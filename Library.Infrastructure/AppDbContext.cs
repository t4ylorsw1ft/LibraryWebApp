using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Configurations;
using Library.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookBorrow> BookBorrows { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) 
        {

            if (!Users.Any())
            {
                var admin = new User
                {
                    Username = "admin",
                    Email = "admin@mail.ru",
                    PasswordHash = "$2a$11$TJlQpqthplkTTIO30qfKYed0/nXXcfHVu/SszmD2U2K6cdDmB/z3q",
                    RefreshToken = "aaaaaa",
                    Role = UserRole.Admin
                };


                Users.Add(admin);
                SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new BookBorrowConfiguration());
        }
    }
}
