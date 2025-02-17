using Library.Application.DTOs.Authors;
using Library.Application.Interfaces.Services;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Get a paginated list of authors.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="size">Number of authors per page.</param>
        /// <returns>A list of authors.</returns>
        [HttpGet]
        public async Task<ActionResult<List<AuthorLookupDto>>> GetAllPaged([FromQuery] int page, [FromQuery] int size)
        {
            var authors = await _authorService.GetAllPagedAsync(page, size);
            return Ok(authors);
        }

        /// <summary>
        /// Get an author by their ID.
        /// </summary>
        /// <param name="id">Author's ID.</param>
        /// <returns>Details of the author.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetById(Guid id)
        {
            var author = await _authorService.GetByIdAsync(id);
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
        public async Task<ActionResult<Author>> Create([FromBody] CreateAuthorDto authorDto)
        {
            var createdAuthor = await _authorService.CreateAsync(authorDto);
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
        public async Task<ActionResult<Author>> Update([FromBody] UpdateAuthorDto authorDto)
        {
            var updatedAuthor = await _authorService.UpdateAsync(authorDto);
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
        public async Task<IActionResult> Delete(Guid id)
        {
            await _authorService.DeleteAsync(id);
            return NoContent();
        }
    }
}
