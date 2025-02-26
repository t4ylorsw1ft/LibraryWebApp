using Library.Application.UseCases.Authors.DTOs;
using MediatR;

namespace Library.Application.UseCases.Authors.Queries.GetAuthorById
{
    public record GetAuthorByIdQuery(Guid Id) : IRequest<AuthorDetailsDto>;
}
