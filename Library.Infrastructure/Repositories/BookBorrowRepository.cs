using Library.Application.Interfaces.Repositories;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Repositories
{
    public class BookBorrowRepository : IBookBorrowRepository
    {
        private readonly AppDbContext _context;

        public BookBorrowRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BookBorrow> AddAsync(BookBorrow bookBorrow)
        {
            await _context.BookBorrows.AddAsync(bookBorrow);
            await _context.SaveChangesAsync();
            return bookBorrow;
        }

        public async Task<BookBorrow> UpdateAsync(BookBorrow bookBorrow)
        {
            _context.BookBorrows.Update(bookBorrow);
            await _context.SaveChangesAsync();
            return bookBorrow;
        }
    }
}
