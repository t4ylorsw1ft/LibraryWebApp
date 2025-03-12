using Library.Domain.Interfaces.Repositories;
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

        public async Task<IEnumerable<BookBorrow>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.BookBorrows
                .Where(bb => bb.UserId == userId)
                .Include(bb => bb.Book)
                .ThenInclude(bb => bb.Author)
                .ToListAsync(cancellationToken);
        }

        public async Task<BookBorrow?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.BookBorrows
                .Include(bb => bb.Book)
                .FirstOrDefaultAsync(bb => bb.Id == id, cancellationToken);
        }

        public async Task<BookBorrow> AddAsync(BookBorrow bookBorrow, CancellationToken cancellationToken)
        {
            await _context.BookBorrows.AddAsync(bookBorrow);
            await _context.SaveChangesAsync(cancellationToken);
            return bookBorrow;
        }

        public async Task<BookBorrow> UpdateAsync(BookBorrow bookBorrow, CancellationToken cancellationToken)
        {
            _context.BookBorrows.Update(bookBorrow);
            await _context.SaveChangesAsync(cancellationToken);
            return bookBorrow;
        }

        public async Task<bool> ActiveBorrowExistsAsync(Guid userId, Guid bookId, CancellationToken cancellationToken)
        {
            return await _context.BookBorrows.AnyAsync(
            bb => bb.UserId == userId && 
            bb.BookId == bookId && 
            !bb.IsReturned, 
            cancellationToken);
        }
    }
}
