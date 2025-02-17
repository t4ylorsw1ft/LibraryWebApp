using AutoMapper;
using FluentValidation;
using Library.Application.Common.Exceptions;
using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces.Repositories;
using Library.Application.Interfaces.Services;
using Library.Domain.Entities;

namespace Library.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateAuthorDto> _createAuthorDtoValidator;
        private readonly IValidator<UpdateAuthorDto> _updateAuthorDtoValidator;

        public AuthorService(
            IAuthorRepository authorRepository,
            IMapper mapper,
            IValidator<CreateAuthorDto> createAuthorDtoValidator,
            IValidator<UpdateAuthorDto> updateAuthorDtoValidator)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _createAuthorDtoValidator = createAuthorDtoValidator;
            _updateAuthorDtoValidator = updateAuthorDtoValidator;
        }

        public async Task<List<AuthorLookupDto>> GetAllPagedAsync(int page, int size)
        {
            var authors = await _authorRepository.GetAllPagedAsync(page, size);
            var authorsDto = _mapper.Map<List<AuthorLookupDto>>(authors);
            return authorsDto;
        }

        public async Task<AuthorDetailsDto> GetByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
                throw new NotFoundException(typeof(Author), id);
            var authorDto = _mapper.Map<AuthorDetailsDto>(author);
            return authorDto;
        }

        public async Task<AuthorDetailsDto> CreateAsync(CreateAuthorDto createAuthorDto)
        {
            var validationResult = await _createAuthorDtoValidator.ValidateAsync(createAuthorDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var author = _mapper.Map<Author>(createAuthorDto);
            await _authorRepository.AddAsync(author);
            var authorDto = _mapper.Map<AuthorDetailsDto>(author);
            return authorDto;
        }

        public async Task<AuthorDetailsDto> UpdateAsync(UpdateAuthorDto updateAuthorDto)
        {
            var validationResult = await _updateAuthorDtoValidator.ValidateAsync(updateAuthorDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var author = _mapper.Map<Author>(updateAuthorDto);
            await _authorRepository.UpdateAsync(author);
            var authorDto = _mapper.Map<AuthorDetailsDto>(author);
            return authorDto;
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!await _authorRepository.DeleteAsync(id))
                throw new NotFoundException(typeof(Author), id);
        }
    }
}