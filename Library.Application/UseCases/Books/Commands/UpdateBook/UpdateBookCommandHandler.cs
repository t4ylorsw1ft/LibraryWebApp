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

namespace Library.Application.UseCases.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDetailsDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateBookDto> _validator;

        public UpdateBookCommandHandler(IBookRepository bookRepository, IMapper mapper, IValidator<UpdateBookDto> validator)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<BookDetailsDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.BookDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingBook = await _bookRepository.GetByIdAsync(request.BookDto.Id, cancellationToken);
            if (existingBook == null)
                throw new NotFoundException(typeof(Book), request.BookDto.Id);

            if (await _bookRepository.GetByISBNAsync(request.BookDto.ISBN, cancellationToken) != null)
                throw new AlreadyExistsException("ISBN");

            _mapper.Map(request.BookDto, existingBook);
            var updatedBook = await _bookRepository.UpdateAsync(existingBook, cancellationToken);

            return _mapper.Map<BookDetailsDto>(updatedBook);
        }
    }
}
