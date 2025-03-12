using Library.Domain.Interfaces.Repositories;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllPagedAsync(int page, int size, CancellationToken cancellationToken)
        {
            return await _context.Books.Include(b => b.Author)
                .OrderBy(b => b.Title)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Book>> GetAllByAuthorAsync(Guid authorId, CancellationToken cancellationToken)
        {
            return await _context.Books.Where(b => b.AuthorId == authorId)
                .Include(b => b.Author)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Book>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.BookBorrows
                .Where(bb => bb.UserId == userId)
                .Select(bb => bb.Book)
                .Include(b => b.Author)
                .ToListAsync(cancellationToken);
        }

        public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Books.Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<Book?> GetByISBNAsync(string isbn, CancellationToken cancellationToken)
        {
            return await _context.Books.Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.ISBN == isbn, cancellationToken);
        }

        public async Task<Book> AddAsync(Book book, CancellationToken cancellationToken)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync(cancellationToken);
            return book;
        }

        public async Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync(cancellationToken);
            return book;
        }

        public async Task DeleteAsync(Book book, CancellationToken cancellationToken)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Books.AnyAsync(b => b.Id == id, cancellationToken);
        }
    }
}
