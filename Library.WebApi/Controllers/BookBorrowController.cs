using Library.Application.DTOs.BookBorrows;
using Library.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookBorrowController : ControllerBase
    {
        private readonly IBookBorrowService _bookBorrowService;

        public BookBorrowController(IBookBorrowService bookBorrowService)
        {
            _bookBorrowService = bookBorrowService;
        }

        /// <summary>
        /// Borrow a book by its ID.
        /// </summary>
        /// <param name="bookId">ID of the book to borrow.</param>
        /// <returns>OK response when the book is borrowed successfully.</returns>
        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook(Guid bookId)
        {
            var userId = Guid.Parse(User.FindFirst("userId")?.Value);
            await _bookBorrowService.BorrowBookAsync(userId, bookId);
            return Ok();
        }

        /// <summary>
        /// Return a borrowed book.
        /// </summary>
        /// <param name="bookBorrowId">ID of the book borrow record.</param>
        /// <returns>OK response when the book is returned successfully.</returns>
        [HttpPost("return/{bookBorrowId}")]
        public async Task<IActionResult> ReturnBook(Guid bookBorrowId)
        {
            await _bookBorrowService.ReturnBookAsync(bookBorrowId);
            return Ok();
        }

        /// <summary>
        /// Get all books borrowed by the current user.
        /// </summary>
        /// <returns>A list of books borrowed by the current user.</returns>
        [HttpGet("user")]
        public async Task<ActionResult<List<BookBorrowLookupDto>>> GetAllByUser()
        {
            var userId = Guid.Parse(User.FindFirst("userId")?.Value);
            var bookBorrows = await _bookBorrowService.GetAllByUserAsync(userId);
            return Ok(bookBorrows);
        }
    }
}
