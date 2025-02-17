using AutoMapper;
using FluentValidation;
using Library.Application.Common.Exceptions;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces.Repositories;
using Library.Application.Interfaces.Services;
using Library.Domain.Entities;

namespace Library.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        private readonly IValidator<CreateBookDto> _createBookDtoValidator;
        private readonly IValidator<UpdateBookDto> _updateBookDtoValidator;

        public BookService(
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            IMapper mapper,
            IFileStorageService fileStorageService,
            IValidator<CreateBookDto> createBookDtoValidator,
            IValidator<UpdateBookDto> updateBookDtoValidator)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _createBookDtoValidator = createBookDtoValidator;
            _updateBookDtoValidator = updateBookDtoValidator;
        }

        public async Task<List<BookLookupDto>> GetAllPagedAsync(int page, int size)
        {
            var books = await _bookRepository.GetAllPagedAsync(page, size);
            var booksDto = _mapper.Map<List<BookLookupDto>>(books);
            return booksDto;
        }

        public async Task<List<BookLookupDto>> GetAllByAuthor(Guid AuthorId)
        {
            var books = await _bookRepository.GetAllByAuthorAsync(AuthorId);
            var booksDto = _mapper.Map<List<BookLookupDto>>(books);
            return booksDto;
        }

        public async Task<BookDetailsDto> GetByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
                throw new NotFoundException(typeof(Book), id);

            var bookDto = _mapper.Map<BookDetailsDto>(book);

            return bookDto;
        }

        public async Task<BookDetailsDto> GetByISBNAsync(string isbn)
        {
            var book = await _bookRepository.GetByISBNAsync(isbn);

            if (book == null)
                throw new NotFoundException(typeof(Book), isbn);

            var bookDto = _mapper.Map<BookDetailsDto>(book);

            return bookDto;
        }

        public async Task<BookDetailsDto> CreateAsync(CreateBookDto bookDto)
        {
            var validationResult = await _createBookDtoValidator.ValidateAsync(bookDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (!await _authorRepository.ExistsAsync(bookDto.AuthorId))
                throw new NotFoundException(typeof(Author), bookDto.AuthorId);

            if (await _bookRepository.GetByISBNAsync(bookDto.ISBN) != null)
                throw new AlreadyExistsException("ISBN");

            var book = _mapper.Map<Book>(bookDto);

            book.AvaliableQuantity = book.Quantity;

            var createdBook = await _bookRepository.AddAsync(book);

            var createdBookWithAuthor = await _bookRepository.GetByIdAsync(book.Id);

            var createdBookDto = _mapper.Map<BookDetailsDto>(createdBookWithAuthor);

            return createdBookDto;
        }

        public async Task<BookDetailsDto> UpdateAsync(UpdateBookDto bookDto)
        {
            var validationResult = await _updateBookDtoValidator.ValidateAsync(bookDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingBook = await _bookRepository.GetByIdAsync(bookDto.Id);

            if (existingBook == null)
                throw new NotFoundException(typeof(Book), bookDto.Id);

            if (await _bookRepository.GetByISBNAsync(bookDto.ISBN) != null)
                throw new AlreadyExistsException("ISBN");

            if (existingBook.ImagePath != bookDto.ImagePath && existingBook.ImagePath != null)
                await _fileStorageService.DeleteFileAsync(existingBook.ImagePath);

            _mapper.Map(bookDto, existingBook);

            var updatedBook = await _bookRepository.UpdateAsync(existingBook);

            return _mapper.Map<BookDetailsDto>(updatedBook);
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book.ImagePath != null)
                await _fileStorageService.DeleteFileAsync(book.ImagePath);

            if (!await _bookRepository.DeleteAsync(id))
                throw new NotFoundException(typeof(Book), id);
        }
    }
}
