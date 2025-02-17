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

        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook(Guid bookId)
        {
            var userId = Guid.Parse(User.FindFirst("userId")?.Value);
            await _bookBorrowService.BorrowBookAsync(userId, bookId);
            return Ok();
        }

        [HttpPost("return/{bookBorrowId}")]
        public async Task<IActionResult> ReturnBook(Guid bookBorrowId)
        {
            await _bookBorrowService.ReturnBookAsync(bookBorrowId);
            return Ok();
        }

        [HttpGet("user")]
        public async Task<ActionResult<List<BookBorrowLookupDto>>> GetAllByUser()
        {
            var userId = Guid.Parse(User.FindFirst("userId")?.Value);
            var bookBorrows = await _bookBorrowService.GetAllByUserAsync(userId);
            return Ok(bookBorrows);
        }
    }
}
