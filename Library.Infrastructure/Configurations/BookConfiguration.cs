﻿using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configurations
{
    internal class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Genre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.Quantity)
                .IsRequired();

            builder.Property(b => b.AvaliableQuantity)
                .IsRequired();

            builder.Property(b => b.Description)
                .HasMaxLength(1000);

            builder.Property(b => b.ImagePath)
                .HasMaxLength(500);

            builder.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.BookBorrows)
                .WithOne(bb => bb.Book)
                .HasForeignKey(bb => bb.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.ISBN).IsUnique();
        }
    }

}
