using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Library.Application.DTOs.Authors;
using Library.Application.Interfaces.Repositories;
using Library.Application.Services;
using Library.Application.Validators.Authors;
using Library.Domain.Entities;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Library.Tests.Services
{
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IValidator<CreateAuthorDto>> _createValidatorMock;
        private readonly Mock<IValidator<UpdateAuthorDto>> _updateValidatorMock;
        private readonly IMapper _mapper;
        private readonly AuthorService _service;

        public AuthorServiceTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _createValidatorMock = new Mock<IValidator<CreateAuthorDto>>();
            _updateValidatorMock = new Mock<IValidator<UpdateAuthorDto>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorDetailsDto>();
                cfg.CreateMap<CreateAuthorDto, Author>();
            });
            _mapper = config.CreateMapper();

            _service = new AuthorService(
                _authorRepositoryMock.Object,
                _mapper,
                _createValidatorMock.Object,
                _updateValidatorMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAuthor_WhenExists()
        {
            var author = new Author { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };
            _authorRepositoryMock.Setup(repo => repo.GetByIdAsync(author.Id))
                .ReturnsAsync(author);

            var result = await _service.GetByIdAsync(author.Id);

            Assert.NotNull(result);
            Assert.Equal(author.Id, result.Id);
            _authorRepositoryMock.Verify(repo => repo.GetByIdAsync(author.Id), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenNotExists()
        {
            _authorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Author?)null);

            await Assert.ThrowsAsync<Library.Application.Common.Exceptions.NotFoundException>(
                async () => await _service.GetByIdAsync(Guid.NewGuid())
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldAddAuthor_WhenValid()
        {
            var createDto = new CreateAuthorDto { FirstName = "Jane", LastName = "Doe" };
            var author = new Author { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe" };

            _createValidatorMock.Setup(v => v.ValidateAsync(createDto, default))
                .ReturnsAsync(new ValidationResult());

            _authorRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Author>()))
                .ReturnsAsync(author);

            var result = await _service.CreateAsync(createDto);

            Assert.NotNull(result);
            Assert.Equal("Jane", result.FirstName);
            _authorRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Author>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowValidationException_WhenInvalid()
        {
            var createDto = new CreateAuthorDto();
            var validationFailure = new ValidationFailure("FirstName", "First name is required");

            _createValidatorMock.Setup(v => v.ValidateAsync(createDto, default))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { validationFailure }));

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                async () => await _service.CreateAsync(createDto)
            );
        }
    }
}
