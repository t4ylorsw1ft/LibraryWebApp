using Library.Application.DTOs.Authors;
using MediatR;

namespace Library.Application.UseCases.Authors.Queries.GetAuthorById
{
    public record GetAuthorByIdQuery(Guid Id) : IRequest<AuthorDetailsDto>;
}
