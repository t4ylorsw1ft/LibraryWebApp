using AutoMapper;
using FluentValidation;
using Library.Application.Common.Exceptions;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces.Repositories;
using Library.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDetailsDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookDto> _validator;

        public CreateBookCommandHandler(
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            IMapper mapper,
            IValidator<CreateBookDto> validator)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<BookDetailsDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.BookDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (!await _authorRepository.ExistsAsync(request.BookDto.AuthorId, cancellationToken))
                throw new NotFoundException(typeof(Author), request.BookDto.AuthorId);

            if (await _bookRepository.GetByISBNAsync(request.BookDto.ISBN, cancellationToken) != null)
                throw new AlreadyExistsException("ISBN");

            var book = _mapper.Map<Book>(request.BookDto);
            book.AvaliableQuantity = book.Quantity;

            var createdBook = await _bookRepository.AddAsync(book, cancellationToken);
            return _mapper.Map<BookDetailsDto>(createdBook);
        }
    }
}
