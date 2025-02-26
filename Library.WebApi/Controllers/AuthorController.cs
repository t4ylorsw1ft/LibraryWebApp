using Library.Application.UseCases.Authors.Commands.CreateAuthor;
using Library.Application.UseCases.Authors.Commands.DeleteAuthor;
using Library.Application.UseCases.Authors.Commands.UpdateAuthor;
using Library.Application.UseCases.Authors.DTOs;
using Library.Application.UseCases.Authors.Queries.GetAllAuthorsPaged;
using Library.Application.UseCases.Authors.Queries.GetAuthorById;
using Library.Application.UseCases.Books.DTOs;
using Library.Application.UseCases.Books.Queries.GetAllBooksByAuthor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebApi.Controllers
{
    [Route("api/authors")]
    [ApiController]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get a paginated list of authors.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="size">Number of authors per page.</param>
        /// <returns>A list of authors.</returns>
        [HttpGet]
        public async Task<ActionResult<List<AuthorLookupDto>>> GetAllPaged(
            [FromQuery] int page,
            [FromQuery] int size,
            CancellationToken cancellationToken)
        {
            var authors = await _mediator.Send(new GetAllAuthorsPagedQuery(page, size), cancellationToken);
            return Ok(authors);
        }

        /// <summary>
        /// Get an author by their ID.
        /// </summary>
        /// <param name="id">Author's ID.</param>
        /// <returns>Details of the author.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDetailsDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var author = await _mediator.Send(new GetAuthorByIdQuery(id), cancellationToken);
            return Ok(author);
        }

        /// <summary>
        /// Create a new author.
        /// Requires Admin role.
        /// </summary>
        /// <param name="authorDto">Author data for creation.</param>
        /// <returns>The created author details.</returns>
        [Authorize("AdminPolicy")]
        [HttpPost]
        public async Task<ActionResult<AuthorDetailsDto>> Create(
            [FromBody] CreateAuthorDto authorDto,
            CancellationToken cancellationToken)
        {
            var createdAuthor = await _mediator.Send(new CreateAuthorCommand(authorDto), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = createdAuthor.Id }, createdAuthor);
        }

        /// <summary>
        /// Update an existing author.
        /// Requires Admin role.
        /// </summary>
        /// <param name="authorDto">Updated author data.</param>
        /// <returns>The updated author details.</returns>
        [Authorize("AdminPolicy")]
        [HttpPut]
        public async Task<ActionResult<AuthorDetailsDto>> Update(
            [FromBody] UpdateAuthorDto authorDto,
            CancellationToken cancellationToken)
        {
            var updatedAuthor = await _mediator.Send(new UpdateAuthorCommand(authorDto), cancellationToken);
            return Ok(updatedAuthor);
        }

        /// <summary>
        /// Delete an author by their ID.
        /// Requires Admin role.
        /// </summary>
        /// <param name="id">Author's ID.</param>
        /// <returns>No content response.</returns>
        [Authorize("AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteAuthorCommand(id), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Get all books by a specific author.
        /// </summary>
        /// <param name="id">Author's ID.</param>
        /// <returns>A list of books by the author.</returns>
        [HttpGet("{id}/books")]
        public async Task<ActionResult<List<BookLookupDto>>> GetAllByAuthor(Guid id, CancellationToken cancellationToken)
        {
            var books = await _mediator.Send(new GetAllBooksByAuthorQuery(id), cancellationToken);
            return Ok(books);
        }
    }
}
