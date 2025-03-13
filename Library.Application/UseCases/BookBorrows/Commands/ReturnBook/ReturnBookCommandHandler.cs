using Library.Application.Common.Exceptions;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.BookBorrows.Commands.ReturnBook
{
    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookBorrowRepository _bookBorrowRepository;

        public ReturnBookCommandHandler(IBookRepository bookRepository, IBookBorrowRepository bookBorrowRepository)
        {
            _bookRepository = bookRepository;
            _bookBorrowRepository = bookBorrowRepository;
        }

        public async Task Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            BookBorrow? bookBorrow = await _bookBorrowRepository.GetByIdAsync(request.BookBorrowId, cancellationToken);

            if (request.UserId != bookBorrow.UserId)
                throw new UnauthorizedAccessException();

            if (bookBorrow == null)
                throw new NotFoundException(typeof(BookBorrow), request.BookBorrowId);

            Book book = bookBorrow.Book;

            if (bookBorrow.IsReturned)
                throw new AlreadyExistsException();

            bookBorrow.IsReturned = true;
            book.AvaliableQuantity++;

            await _bookRepository.UpdateAsync(book, cancellationToken);
            await _bookBorrowRepository.UpdateAsync(bookBorrow, cancellationToken);
        }
    }
}
