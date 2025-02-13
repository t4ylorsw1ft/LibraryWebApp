using Library.Application.Interfaces.Repositories;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class BookBorrowRepository : IBookBorrowRepository
    {
        private readonly AppDbContext _context;

        public BookBorrowRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookBorrow>> GetAllByUserAsync(Guid userId)
        {
            return await _context.BookBorrows
                .Where(bb => bb.UserId == userId)
                .Include(bb => bb.Book)
                .ToListAsync();
        }

        public async Task<BookBorrow?> GetByIdAsync(Guid id)
        {
            return await _context.BookBorrows.FirstOrDefaultAsync(bb => bb.Id == id);
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

        public async Task<bool> ActiveBorrowExistsAsync(Guid userId, Guid bookId)
        {
            return await _context.BookBorrows.AnyAsync(bb => bb.UserId == userId && bb.BookId == bookId && !bb.IsReturned);
        }
    }
}
