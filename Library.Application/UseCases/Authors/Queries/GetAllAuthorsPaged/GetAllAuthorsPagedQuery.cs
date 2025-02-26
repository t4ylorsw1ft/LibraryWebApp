using Library.Application.UseCases.Authors.DTOs;
using MediatR;

namespace Library.Application.UseCases.Authors.Queries.GetAllAuthorsPaged
{
    public record GetAllAuthorsPagedQuery(int Page, int Size) : IRequest<List<AuthorLookupDto>>;
}
