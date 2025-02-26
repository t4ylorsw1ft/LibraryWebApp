using AutoMapper;
using Library.Application.Common.Exceptions;
using Library.Application.Interfaces.Repositories;
using Library.Application.UseCases.Authors.DTOs;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.UseCases.Authors.Queries.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDetailsDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<AuthorDetailsDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(request.Id, cancellationToken);
            if (author == null)
                throw new NotFoundException(typeof(Author), request.Id);

            return _mapper.Map<AuthorDetailsDto>(author);
        }
    }
}
