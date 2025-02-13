using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configurations
{
    internal class BookBorrowConfiguration : IEntityTypeConfiguration<BookBorrow>
    {
        public void Configure(EntityTypeBuilder<BookBorrow> builder)
        {
            builder.HasKey(bb => bb.Id);

            builder.Property(bb => bb.BorrowedAt)
                .IsRequired();

            builder.Property(bb => bb.ReturnBy)
                .IsRequired();

            builder.Property(bb => bb.IsReturned)
                .IsRequired();

            builder.HasOne(bb => bb.Book)
                .WithMany(b => b.BookBorrows)
                .HasForeignKey(bb => bb.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bb => bb.User)
                .WithMany(u => u.BookBorrows)
                .HasForeignKey(bb => bb.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
