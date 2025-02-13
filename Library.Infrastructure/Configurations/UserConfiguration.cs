using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.RefreshToken)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<int>();

            builder.HasMany(u => u.BookBorrows)
                .WithOne(bb => bb.User)
                .HasForeignKey(bb => bb.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.RefreshToken).IsUnique();
        }
    }

}
