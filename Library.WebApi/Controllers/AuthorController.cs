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

        [HttpGet]
        public async Task<ActionResult<List<AuthorLookupDto>>> GetAllPaged([FromQuery] int page, [FromQuery] int size)
        {
            var authors = await _authorService.GetAllPagedAsync(page, size);
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetById(Guid id)
        {
            var author = await _authorService.GetByIdAsync(id);
            return Ok(author);
        }

        [Authorize("AdminPolicy")]
        [HttpPost]
        public async Task<ActionResult<Author>> Create([FromBody] CreateAuthorDto authorDto)
        {
            var createdAuthor = await _authorService.CreateAsync(authorDto);
            return CreatedAtAction(nameof(GetById), new { id = createdAuthor.Id }, createdAuthor);
        }

        [Authorize("AdminPolicy")]
        [HttpPut]
        public async Task<ActionResult<Author>> Update([FromBody] UpdateAuthorDto authorDto)
        {
            var updatedAuthor = await _authorService.UpdateAsync(authorDto);
            return Ok(updatedAuthor);
        }

        [Authorize("AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _authorService.DeleteAsync(id);
            return NoContent();
        }
    }
}

