using Library.Application.Common.Exceptions;
using Library.Domain.Interfaces.Repositories;
using Library.Application.UseCases.Authors.Commands.DeleteAuthor;
using Library.Domain.Entities;
using Moq;

namespace Library.Tests.Commands
{
    public class DeleteAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly DeleteAuthorCommandHandler _handler;

        public DeleteAuthorCommandHandlerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _handler = new DeleteAuthorCommandHandler(_authorRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingAuthor_ShouldDeleteAuthor()
        {
            // arrange
            var authorId = Guid.NewGuid();

            var author = new Author 
            { 
                Id = authorId,
                FirstName = "Ernest",
                LastName = "Hemingway",
                BirthDate = new DateTime(1899, 7, 21),
                Country = "USA"
            };

            _authorRepositoryMock
                .Setup(r => r.GetByIdAsync(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);

            _authorRepositoryMock
                .Setup(r => r.DeleteAsync(author, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new DeleteAuthorCommand(authorId);

            // act
            await _handler.Handle(command, CancellationToken.None);

            // assert
            _authorRepositoryMock.Verify(r => r.GetByIdAsync(authorId, It.IsAny<CancellationToken>()), Times.Once);
            _authorRepositoryMock.Verify(r => r.DeleteAsync(author, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingAuthor_ShouldThrowNotFoundException()
        {
            // arrange
            var authorId = Guid.NewGuid();

            _authorRepositoryMock
                .Setup(r => r.GetByIdAsync(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Author?)null);

            var command = new DeleteAuthorCommand(authorId);

            // act & assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(command, CancellationToken.None));

            Assert.Equal($"Entity {nameof(Author)} with key {authorId} was not found.", exception.Message);

            _authorRepositoryMock.Verify(r => r.GetByIdAsync(authorId, It.IsAny<CancellationToken>()), Times.Once);
            _authorRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
