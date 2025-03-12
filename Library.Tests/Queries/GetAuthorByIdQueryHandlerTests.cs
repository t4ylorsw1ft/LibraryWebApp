using AutoMapper;
using Library.Application.Common.Exceptions;
using Library.Domain.Interfaces.Repositories;
using Library.Application.UseCases.Authors.DTOs;
using Library.Application.UseCases.Authors.Queries.GetAuthorById;
using Library.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Queries
{
    public class GetAuthorByIdQueryHandlerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAuthorByIdQueryHandler _handler;

        public GetAuthorByIdQueryHandlerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAuthorByIdQueryHandler(_authorRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingAuthor_ShouldReturnAuthorDetailsDto()
        {
            // arrange
            var authorId = Guid.NewGuid();
            var author = new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "Ernest",
                LastName = "Hemingway",
                BirthDate = new DateTime(1899, 7, 21),
                Country = "USA"
            };

            var authorDetailsDto = new AuthorDetailsDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                BirthDate = author.BirthDate,
                Country = author.Country
            };

            _authorRepositoryMock
                .Setup(r => r.GetByIdAsync(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);

            _mapperMock
                .Setup(m => m.Map<AuthorDetailsDto>(author))
                .Returns(authorDetailsDto);

            var query = new GetAuthorByIdQuery(authorId);

            // act
            var result = await _handler.Handle(query, CancellationToken.None);

            // assert
            Assert.NotNull(result);
            Assert.Equal(authorDetailsDto.Id, result.Id);
            Assert.Equal(authorDetailsDto.FirstName, result.FirstName);
            Assert.Equal(authorDetailsDto.LastName, result.LastName);
            Assert.Equal(authorDetailsDto.Country, result.Country);

            _authorRepositoryMock.Verify(r => r.GetByIdAsync(authorId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<AuthorDetailsDto>(author), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingAuthor_ShouldThrowNotFoundException()
        {
            // arrange
            var authorId = Guid.NewGuid();

            _authorRepositoryMock
                .Setup(r => r.GetByIdAsync(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Author?)null);

            var query = new GetAuthorByIdQuery(authorId);

            // act & assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(query, CancellationToken.None));

            Assert.Equal($"Entity {nameof(Author)} with key {authorId} was not found.", exception.Message);

            _authorRepositoryMock.Verify(r => r.GetByIdAsync(authorId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<AuthorDetailsDto>(It.IsAny<Author>()), Times.Never);
        }
    }
}
