using Library.Application.DTOs.Books;
using MediatR;

namespace Library.Application.UseCases.Books.Queries.GetAllBooksByAuthor
{
    public record GetAllBooksByAuthorQuery(Guid AuthorId) : IRequest<List<BookLookupDto>>;
}
