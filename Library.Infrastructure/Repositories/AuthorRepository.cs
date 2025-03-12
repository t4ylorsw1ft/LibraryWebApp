using Library.Domain.Interfaces.Repositories;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Library.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Authors.Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Author>> GetAllPagedAsync(int page, int size, CancellationToken cancellationToken)
        {
            return await _context.Authors.Include(a => a.Books)
                .OrderBy(a => a.LastName)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }

        public async Task<Author> AddAsync(Author author, CancellationToken cancellationToken)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync(cancellationToken);
            return author;
        }

        public async Task<Author> UpdateAsync(Author author, CancellationToken cancellationToken)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync(cancellationToken);
            return author;
        }

        public async Task DeleteAsync(Author author, CancellationToken cancellationToken)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Authors.AnyAsync(u => u.Id == id, cancellationToken);
        }
    }
}
