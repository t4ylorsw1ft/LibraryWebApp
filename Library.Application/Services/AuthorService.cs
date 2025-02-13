using Library.Application.Common.Exceptions;
using Library.Application.Interfaces.Repositories;
using Library.Application.Interfaces.Services;
using Library.Domain.Entities;

namespace Library.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<Author>> GetAllPagedAsync(int page, int size)
        {
            return await _authorRepository.GetAllPagedAsync(page, size);
        }

        public async Task<Author> GetByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
                throw new NotFoundException(typeof(Author), id);
            return author;
        }

        public async Task<Author> CreateAsync(Author author)
        {
            return await _authorRepository.AddAsync(author);
        }

        public async Task<Author> UpdateAsync(Author author)
        {
            return await _authorRepository.UpdateAsync(author);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!await _authorRepository.DeleteAsync(id))
                throw new NotFoundException(typeof(Author), id);
        }
    }
}