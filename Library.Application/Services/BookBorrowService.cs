using Library.Application.Common.Exceptions;
using Library.Application.Interfaces.Repositories;
using Library.Application.Interfaces.Services;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class BookBorrowService : IBookBorrowService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookBorrowRepository _bookBorrowRepository;

        public BookBorrowService(IBookRepository bookRepository, IUserRepository userRepository, IBookBorrowRepository bookBorrowRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _bookBorrowRepository = bookBorrowRepository;
        }

        public async Task BorrowBookAsync(Guid userId, Guid bookId)
        {
            Book? book = await _bookRepository.GetByIdAsync(bookId);

            if(book == null)
                throw new NotFoundException(typeof(Book), bookId);

            if (! await _userRepository.ExistsAsync(userId))
                throw new NotFoundException(typeof(User), userId);

            if (await _bookBorrowRepository.ActiveBorrowExistsAsync(userId, bookId))
                throw new AlreadyExistsException();

            if (book.AvaliableQuantity < 1)
                throw new Exception();


            book.AvaliableQuantity--;
            await _bookRepository.UpdateAsync(book);

            BookBorrow bookBorrow = new BookBorrow()
            {
                UserId = userId,
                BookId = bookId
            };

            await _bookBorrowRepository.AddAsync(bookBorrow);
        }

        public async Task ReturnBookAsync(Guid bookBorrowId)
        {
            BookBorrow? bookBorrow = await _bookBorrowRepository.GetByIdAsync(bookBorrowId);

            if (bookBorrow == null)
                throw new NotFoundException(typeof(BookBorrow), bookBorrowId);

            Book book = bookBorrow.Book;

            if (bookBorrow.IsReturned)
                throw new AlreadyExistsException();

            bookBorrow.IsReturned = true;
            book.AvaliableQuantity++;

            await _bookRepository.UpdateAsync(book);
            await _bookBorrowRepository.UpdateAsync(bookBorrow);

        }

        public async Task<IEnumerable<BookBorrow>> GetAllByUserAsync(Guid userId)
        {
            return await _bookBorrowRepository.GetAllByUserAsync(userId);
        }
    }
}
