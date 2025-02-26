using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Library.Application.Interfaces.Repositories;
using Library.Application.UseCases.Authors.Commands.UpdateAuthor;
using Library.Application.UseCases.Authors.DTOs;
using Library.Domain.Entities;
using Moq;

namespace Library.Tests.Commands
{
    public class UpdateAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<UpdateAuthorDto>> _validatorMock;
        private readonly UpdateAuthorCommandHandler _handler;

        public UpdateAuthorCommandHandlerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<UpdateAuthorDto>>();

            _handler = new UpdateAuthorCommandHandler(
                _authorRepositoryMock.Object,
                _mapperMock.Object,
                _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldUpdateAuthor()
        {
            // arrange
            var updateAuthorDto = new UpdateAuthorDto
            {
                Id = Guid.NewGuid(),
                FirstName = "Ernest",
                LastName = "Hemingway",
                BirthDate = new DateTime(1899, 7, 21),
                Country = "USA"
            };

            var author = new Author
            {
                Id = updateAuthorDto.Id,
                FirstName = updateAuthorDto.FirstName,
                LastName = updateAuthorDto.LastName,
                BirthDate = updateAuthorDto.BirthDate,
                Country = updateAuthorDto.Country
            };

            var authorDetailsDto = new AuthorDetailsDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                BirthDate = author.BirthDate,
                Country = author.Country
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(updateAuthorDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mapperMock
                .Setup(m => m.Map<Author>(updateAuthorDto))
                .Returns(author);

            _mapperMock
                .Setup(m => m.Map<AuthorDetailsDto>(author))
                .Returns(authorDetailsDto);

            _authorRepositoryMock
                .Setup(r => r.UpdateAsync(author, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);

            var command = new UpdateAuthorCommand(updateAuthorDto);

            // act
            var result = await _handler.Handle(command, CancellationToken.None);

            // assert
            Assert.NotNull(result);
            Assert.Equal(authorDetailsDto.Id, result.Id);
            Assert.Equal(authorDetailsDto.FirstName, result.FirstName);
            Assert.Equal(authorDetailsDto.LastName, result.LastName);
            Assert.Equal(authorDetailsDto.BirthDate, result.BirthDate);
            Assert.Equal(authorDetailsDto.Country, result.Country);

            _authorRepositoryMock.Verify(r => r.UpdateAsync(author, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidRequest_ShouldThrowValidationException()
        {
            // arrange
            var updateAuthorDto = new UpdateAuthorDto
            {
                Id = Guid.NewGuid(),
                FirstName = "",
                LastName = "Hemingway",
                BirthDate = new DateTime(1899, 7, 21),
                Country = "USA"
            };

            var validationErrors = new ValidationResult(new[]
            {
                new ValidationFailure("FirstName", "First name is required")
            });

            _validatorMock
                .Setup(v => v.ValidateAsync(updateAuthorDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationErrors);

            var command = new UpdateAuthorCommand(updateAuthorDto);

            // act & assert
            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => _handler.Handle(command, CancellationToken.None));

            Assert.Contains("First name is required", exception.Message);
            _authorRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
