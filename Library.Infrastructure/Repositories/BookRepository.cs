using Library.Application.Interfaces.Repositories;
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

        public async Task<IEnumerable<Book>> GetAllPagedAsync(int page, int size)
        {
            return await _context.Books.Include(b => b.Author)
                .OrderBy(b => b.Title)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllByAuthorAsync(Guid authorId)
        {
            return await _context.Books.Where(b => b.AuthorId == authorId)
                .Include(b => b.Author)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllByUserAsync(Guid userId)
        {
            return await _context.BookBorrows
                .Where(bb => bb.UserId == userId)
                .Select(bb => bb.Book)
                .Include(b => b.Author)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            return await _context.Books.Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _context.Books.Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<Book> AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Books.AnyAsync(b => b.Id == id);
        }
    }
}
