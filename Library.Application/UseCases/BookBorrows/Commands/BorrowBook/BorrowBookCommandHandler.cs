using Library.Application.Common.Exceptions;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.BookBorrows.Commands.BorrowBook
{
    public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookBorrowRepository _bookBorrowRepository;

        public BorrowBookCommandHandler(IBookRepository bookRepository,
            IUserRepository userRepository,
            IBookBorrowRepository bookBorrowRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _bookBorrowRepository = bookBorrowRepository;
        }

        public async Task Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken);
            if (book == null)
                throw new NotFoundException(typeof(Book), request.BookId);

            if (!await _userRepository.ExistsAsync(request.UserId, cancellationToken))
                throw new NotFoundException(typeof(User), request.UserId);

            if (await _bookBorrowRepository.ActiveBorrowExistsAsync(request.UserId, request.BookId, cancellationToken))
                throw new AlreadyExistsException();

            if (book.AvaliableQuantity < 1)
                throw new Exception("No available copies of the book.");

            book.AvaliableQuantity--;
            await _bookRepository.UpdateAsync(book, cancellationToken);

            var bookBorrow = new BookBorrow
            {
                UserId = request.UserId,
                BookId = request.BookId
            };

            await _bookBorrowRepository.AddAsync(bookBorrow, cancellationToken);
        }
    }
}
