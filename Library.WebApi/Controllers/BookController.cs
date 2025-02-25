using Library.Application.DTOs.Books;
using Library.Application.UseCases.Books.Commands.CreateBook;
using Library.Application.UseCases.Books.Commands.DeleteBook;
using Library.Application.UseCases.Books.Commands.UpdateBook;
using Library.Application.UseCases.Books.Queries.GetAllBooksByAuthor;
using Library.Application.UseCases.Books.Queries.GetAllBooksPaged;
using Library.Application.UseCases.Books.Queries.GetBookById;
using Library.Application.UseCases.Books.Queries.GetBookByISBN;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get a paginated list of books.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="size">Number of books per page.</param>
        /// <returns>A list of books.</returns>
        [HttpGet]
        public async Task<ActionResult<List<BookLookupDto>>> GetAllPaged([FromQuery] int page, [FromQuery] int size, CancellationToken cancellationToken)
        {
            var books = await _mediator.Send(new GetAllBooksPagedQuery(page, size), cancellationToken);
            return Ok(books);
        }

        /// <summary>
        /// Get all books by a specific author.
        /// </summary>
        /// <param name="authorId">Author's ID.</param>
        /// <returns>A list of books by the author.</returns>
        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<List<BookLookupDto>>> GetAllByAuthor(Guid authorId, CancellationToken cancellationToken)
        {
            var books = await _mediator.Send(new GetAllBooksByAuthorQuery(authorId), cancellationToken);
            return Ok(books);
        }

        /// <summary>
        /// Get a book by its ID.
        /// </summary>
        /// <param name="id">Book's ID.</param>
        /// <returns>Details of the book.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id), cancellationToken);
            return Ok(book);
        }

        /// <summary>
        /// Get a book by its ISBN.
        /// </summary>
        /// <param name="isbn">Book's ISBN.</param>
        /// <returns>Details of the book.</returns>
        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult<BookDetailsDto>> GetByISBN(string isbn, CancellationToken cancellationToken)
        {
            var book = await _mediator.Send(new GetBookByISBNQuery(isbn), cancellationToken);
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
        public async Task<ActionResult<BookDetailsDto>> Create([FromBody] CreateBookDto bookDto, CancellationToken cancellationToken)
        {
            var createdBook = await _mediator.Send(new CreateBookCommand(bookDto), cancellationToken);
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
        public async Task<ActionResult<BookDetailsDto>> Update([FromBody] UpdateBookDto bookDto, CancellationToken cancellationToken)
        {
            var updatedBook = await _mediator.Send(new UpdateBookCommand(bookDto), cancellationToken);
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
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteBookCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
