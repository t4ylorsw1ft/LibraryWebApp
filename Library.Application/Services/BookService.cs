using AutoMapper;
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

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
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
            var existingBook = await _bookRepository.GetByIdAsync(bookDto.Id);

            if (existingBook == null)
                throw new NotFoundException(typeof(Book), bookDto.Id);

            _mapper.Map(bookDto, existingBook);

            var updatedBook = await _bookRepository.UpdateAsync(existingBook);

            return _mapper.Map<BookDetailsDto>(updatedBook);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!await _bookRepository.DeleteAsync(id))
                throw new NotFoundException(typeof(Book), id);
        }
    }
}
