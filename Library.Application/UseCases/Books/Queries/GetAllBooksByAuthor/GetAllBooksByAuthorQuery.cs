using Library.Application.UseCases.Books.DTOs;
using MediatR;

namespace Library.Application.UseCases.Books.Queries.GetAllBooksByAuthor
{
    public record GetAllBooksByAuthorQuery(Guid AuthorId) : IRequest<List<BookLookupDto>>;
}
