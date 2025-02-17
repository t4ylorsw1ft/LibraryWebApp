using Library.Domain.Entities;
using Library.Infrastructure;
using Library.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Repositories
{
    public class AuthorRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly AuthorRepository _authorRepository;

        public AuthorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _authorRepository = new AuthorRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_AuthorExists_ReturnsAuthor()
        {
            var author = new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1980, 1, 1),
                Country = "USA"
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var result = await _authorRepository.GetByIdAsync(author.Id);

            Assert.NotNull(result);
            Assert.Equal(author.Id, result.Id);
            Assert.Equal(author.FirstName, result.FirstName);
            Assert.Equal(author.LastName, result.LastName);
            Assert.Equal(author.BirthDate, result.BirthDate);
            Assert.Equal(author.Country, result.Country);
        }

        [Fact]
        public async Task GetByIdAsync_AuthorDoesNotExist_ReturnsNull()
        {
            var result = await _authorRepository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ValidAuthor_SavesToDatabase()
        {
            var author = new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "Ernest",
                LastName = "Hemingway",
                BirthDate = new DateTime(1899, 7, 21),
                Country = "USA"
            };

            await _authorRepository.AddAsync(author);
            var result = await _context.Authors.FindAsync(author.Id);

            Assert.NotNull(result);
            Assert.Equal(author.Id, result.Id);
            Assert.Equal(author.FirstName, result.FirstName);
            Assert.Equal(author.LastName, result.LastName);
            Assert.Equal(author.BirthDate, result.BirthDate);
            Assert.Equal(author.Country, result.Country);
        }

        [Fact]
        public async Task AddAsync_DuplicateId_ThrowsException()
        {
            var authorId = Guid.NewGuid();
            var author1 = new Author
            {
                Id = authorId,
                FirstName = "Ernest",
                LastName = "Hemingway",
                BirthDate = new DateTime(1899, 7, 21),
                Country = "USA"
            };

            var author2 = new Author
            {
                Id = authorId,
                FirstName = "William",
                LastName = "Blake",
                BirthDate = new DateTime(1757, 11, 28),
                Country = "England"
            };

            await _authorRepository.AddAsync(author1);
            await _context.SaveChangesAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _authorRepository.AddAsync(author2));
        }
    }
}