using Library.Application.DTOs.Books;
using Library.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookLookupDto>>> GetAllPaged([FromQuery] int page, [FromQuery] int size)
        {
            var books = await _bookService.GetAllPagedAsync(page, size);
            return Ok(books);
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<List<BookLookupDto>>> GetAllByAuthor(Guid authorId)
        {
            var books = await _bookService.GetAllByAuthor(authorId);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetById(Guid id)
        {
            var book = await _bookService.GetByIdAsync(id);
            return Ok(book);
        }

        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult<BookDetailsDto>> GetByISBN(string isbn)
        {
            var book = await _bookService.GetByISBNAsync(isbn);
            return Ok(book);
        }

        [Authorize("AdminPolicy")]
        [HttpPost]
        public async Task<ActionResult<BookDetailsDto>> Create([FromBody] CreateBookDto bookDto)
        {
            var createdBook = await _bookService.CreateAsync(bookDto);
            return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
        }

        [Authorize("AdminPolicy")]
        [HttpPut]
        public async Task<ActionResult<BookDetailsDto>> Update([FromBody] UpdateBookDto bookDto)
        {
            var updatedBook = await _bookService.UpdateAsync(bookDto);
            return Ok(updatedBook);
        }

        [Authorize("AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookService.DeleteAsync(id);
            return NoContent();
        }
    }
}
