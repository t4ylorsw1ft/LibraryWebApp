using AutoMapper;
using FluentValidation;
using Library.Application.Common.Exceptions;
using Library.Application.Interfaces.Repositories;
using Library.Application.UseCases.Books.DTOs;
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
            var bookDto = request.BookDto;

            var validationResult = await _validator.ValidateAsync(bookDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingBook = await _bookRepository.GetByIdAsync(bookDto.Id, cancellationToken);
            if (existingBook == null)
                throw new NotFoundException(typeof(Book), bookDto.Id);

            var bookWithSameISBN = await _bookRepository.GetByISBNAsync(bookDto.ISBN, cancellationToken);

            if (bookWithSameISBN != null && bookWithSameISBN.Id != bookDto.Id)
                throw new AlreadyExistsException("ISBN");

            _mapper.Map(request.BookDto, existingBook);
            var updatedBook = await _bookRepository.UpdateAsync(existingBook, cancellationToken);

            return _mapper.Map<BookDetailsDto>(updatedBook);
        }
    }
}
