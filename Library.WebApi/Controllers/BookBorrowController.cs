using Library.Application.Interfaces.Services;
using Library.Application.UseCases.BookBorrows.Commands.BorrowBook;
using Library.Application.UseCases.BookBorrows.Commands.ReturnBook;
using Library.Application.UseCases.BookBorrows.DTOs;
using Library.Application.UseCases.BookBorrows.Queries.GetUserBookBorrows;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Library.WebApi.Controllers
{
    [Route("api/book-borrows")]
    [ApiController]
    [Authorize]
    public class BookBorrowController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookBorrowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Borrow a book by its ID.
        /// </summary>
        /// <param name="bookId">ID of the book to borrow.</param>
        /// <returns>OK response when the book is borrowed successfully.</returns>
        [HttpPost()]
        public async Task<IActionResult> BorrowBook(Guid bookId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")?.Value);
            await _mediator.Send(new BorrowBookCommand(userId, bookId), cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Return a borrowed book.
        /// </summary>
        /// <param name="bookBorrowId">ID of the book borrow record.</param>
        /// <returns>OK response when the book is returned successfully.</returns>
        [HttpPost("return/{bookBorrowId}")]
        public async Task<IActionResult> ReturnBook(Guid bookBorrowId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ReturnBookCommand(bookBorrowId), cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Get all books borrowed by the current user.
        /// </summary>
        /// <returns>A list of books borrowed by the current user.</returns>
        [HttpGet()]
        public async Task<ActionResult<List<BookBorrowLookupDto>>> GetAllByUser(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("userId")?.Value);
            var bookBorrows = await _mediator.Send(new GetUserBookBorrowsQuery(userId), cancellationToken);
            return Ok(bookBorrows);
        }
    }
}
