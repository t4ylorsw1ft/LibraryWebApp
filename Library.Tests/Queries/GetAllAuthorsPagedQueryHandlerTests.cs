using AutoMapper;
using Library.Domain.Interfaces.Repositories;
using Library.Application.UseCases.Authors.DTOs;
using Library.Application.UseCases.Authors.Queries.GetAllAuthorsPaged;
using Library.Domain.Entities;
using Moq;
using Xunit;

namespace Library.Tests.Queries
{
    public class GetAllAuthorsPagedQueryHandlerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllAuthorsPagedQueryHandler _handler;

        public GetAllAuthorsPagedQueryHandlerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new GetAllAuthorsPagedQueryHandler(
                _authorRepositoryMock.Object, 
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnPagedAuthors()
        {
            // arrange
            var page = 1;
            var size = 2;
            var authors = new List<Author>
            {
                new Author  
                {
                    FirstName = "Ernest",
                    LastName = "Hemingway",
                    BirthDate = new DateTime(1899, 7, 21),
                    Country = "USA"
                },
                new Author
                {
                    FirstName = "Albert",
                    LastName = "Camus",
                    BirthDate = new DateTime(1913, 11, 7),
                    Country = "USA"
                }
            };

            var authorDtos = new List<AuthorLookupDto>
            {
                new AuthorLookupDto 
                { 
                    Id = authors[0].Id,
                    FirstName = "Ernest",
                    LastName = "Hemingway",
                    Country = "USA"
                },
                new AuthorLookupDto 
                { 
                    Id = authors[1].Id,
                    FirstName = "Albert",
                    LastName = "Camus",
                    Country = "USA"
                }
            };

            _authorRepositoryMock
                .Setup(r => r.GetAllPagedAsync(page, size, It.IsAny<CancellationToken>()))
                .ReturnsAsync(authors);

            _mapperMock
                .Setup(m => m.Map<List<AuthorLookupDto>>(authors))
                .Returns(authorDtos);

            var query = new GetAllAuthorsPagedQuery(page, size);

            // act
            var result = await _handler.Handle(query, CancellationToken.None);

            // assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(authors[0].Id, result[0].Id);
            Assert.Equal(authors[1].Id, result[1].Id);

            _authorRepositoryMock.Verify(r => r.GetAllPagedAsync(page, size, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<List<AuthorLookupDto>>(authors), Times.Once);
        }

        [Fact]
        public async Task Handle_NoAuthors_ShouldReturnEmptyList()
        {
            // arrange
            var page = 1;
            var size = 5;
            var authors = new List<Author>();
            var authorDtos = new List<AuthorLookupDto>();

            _authorRepositoryMock
                .Setup(r => r.GetAllPagedAsync(page, size, It.IsAny<CancellationToken>()))
                .ReturnsAsync(authors);

            _mapperMock
                .Setup(m => m.Map<List<AuthorLookupDto>>(authors))
                .Returns(authorDtos);

            var query = new GetAllAuthorsPagedQuery(page, size);

            // act
            var result = await _handler.Handle(query, CancellationToken.None);

            // assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _authorRepositoryMock.Verify(r => r.GetAllPagedAsync(page, size, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<List<AuthorLookupDto>>(authors), Times.Once);
        }
    }
}
