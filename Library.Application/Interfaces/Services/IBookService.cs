using Library.Application.DTOs.Books;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces.Services
{
    public interface IBookService
    {
        Task<List<BookLookupDto>> GetAllPagedAsync(int page, int size);
        Task<List<BookLookupDto>> GetAllByAuthor(Guid authorId);
        Task<BookDetailsDto> GetByIdAsync(Guid id);
        Task<BookDetailsDto> GetByISBNAsync(string isbn);
        Task<BookDetailsDto> CreateAsync(CreateBookDto book);
        Task<BookDetailsDto> UpdateAsync(UpdateBookDto book);
        Task DeleteAsync(Guid bookId);
    }
}
