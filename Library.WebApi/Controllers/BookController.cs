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

        /// <summary>
        /// Get a paginated list of books.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="size">Number of books per page.</param>
        /// <returns>A list of books.</returns>
        [HttpGet]
        public async Task<ActionResult<List<BookLookupDto>>> GetAllPaged([FromQuery] int page, [FromQuery] int size)
        {
            var books = await _bookService.GetAllPagedAsync(page, size);
            return Ok(books);
        }

        /// <summary>
        /// Get all books by a specific author.
        /// </summary>
        /// <param name="authorId">Author's ID.</param>
        /// <returns>A list of books by the author.</returns>
        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<List<BookLookupDto>>> GetAllByAuthor(Guid authorId)
        {
            var books = await _bookService.GetAllByAuthor(authorId);
            return Ok(books);
        }

        /// <summary>
        /// Get a book by its ID.
        /// </summary>
        /// <param name="id">Book's ID.</param>
        /// <returns>Details of the book.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetById(Guid id)
        {
            var book = await _bookService.GetByIdAsync(id);
            return Ok(book);
        }

        /// <summary>
        /// Get a book by its ISBN.
        /// </summary>
        /// <param name="isbn">Book's ISBN.</param>
        /// <returns>Details of the book.</returns>
        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult<BookDetailsDto>> GetByISBN(string isbn)
        {
            var book = await _bookService.GetByISBNAsync(isbn);
            return Ok(book);
        }

        /// <summary>
        /// Create a new book.
        /// Requires Admin role.
        /// </summary>
        /// <param name="bookDto">Book data for creation.</param>
        /// <returns>The created book details.</returns>
        [Authorize("AdminPolicy")]
        [HttpPost]
        public async Task<ActionResult<BookDetailsDto>> Create([FromBody] CreateBookDto bookDto)
        {
            var createdBook = await _bookService.CreateAsync(bookDto);
            return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
        }

        /// <summary>
        /// Update an existing book.
        /// Requires Admin role.
        /// </summary>
        /// <param name="bookDto">Updated book data.</param>
        /// <returns>The updated book details.</returns>
        [Authorize("AdminPolicy")]
        [HttpPut]
        public async Task<ActionResult<BookDetailsDto>> Update([FromBody] UpdateBookDto bookDto)
        {
            var updatedBook = await _bookService.UpdateAsync(bookDto);
            return Ok(updatedBook);
        }

        /// <summary>
        /// Delete a book by its ID.
        /// Requires Admin role.
        /// </summary>
        /// <param name="id">Book's ID.</param>
        /// <returns>No content response.</returns>
        [Authorize("AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookService.DeleteAsync(id);
            return NoContent();
        }
    }
}
