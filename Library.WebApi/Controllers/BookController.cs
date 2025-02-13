using AutoMapper;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Получить список книг с пагинацией.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<BookLookupDto>>> GetAllPaged([FromQuery] int page, [FromQuery] int size)
        {
            var books = await _bookService.GetAllPagedAsync(page, size);
            return Ok(books);
        }

        /// <summary>
        /// Получить книги по автору.
        /// </summary>
        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<List<BookLookupDto>>> GetAllByAuthor(Guid authorId)
        {
            var books = await _bookService.GetAllByAuthor(authorId);
            return Ok(books);
        }

        /// <summary>
        /// Получить книгу по идентификатору.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetById(Guid id)
        {
            var book = await _bookService.GetByIdAsync(id);
            return Ok(book);
        }

        /// <summary>
        /// Получить книгу по ISBN.
        /// </summary>
        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult<BookDetailsDto>> GetByISBN(string isbn)
        {
            var book = await _bookService.GetByISBNAsync(isbn);
            return Ok(book);
        }

        /// <summary>
        /// Создать новую книгу.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BookDetailsDto>> Create([FromBody] CreateBookDto bookDto)
        {
            var createdBook = await _bookService.CreateAsync(bookDto);
            return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
        }

        /// <summary>
        /// Обновить данные книги.
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<BookDetailsDto>> Update([FromBody] UpdateBookDto bookDto)
        {
            var updatedBook = await _bookService.UpdateAsync(bookDto);
            return Ok(updatedBook);
        }

        /// <summary>
        /// Удалить книгу.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookService.DeleteAsync(id);
            return NoContent();
        }
    }
}
