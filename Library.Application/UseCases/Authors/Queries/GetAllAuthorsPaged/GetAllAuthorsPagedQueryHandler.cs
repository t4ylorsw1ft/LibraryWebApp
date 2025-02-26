using AutoMapper;
using Library.Application.Interfaces.Repositories;
using Library.Application.UseCases.Authors.DTOs;
using MediatR;

namespace Library.Application.UseCases.Authors.Queries.GetAllAuthorsPaged
{
    public class GetAllAuthorsPagedQueryHandler : IRequestHandler<GetAllAuthorsPagedQuery, List<AuthorLookupDto>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public GetAllAuthorsPagedQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<List<AuthorLookupDto>> Handle(GetAllAuthorsPagedQuery request, CancellationToken cancellationToken)
        {
            var authors = await _authorRepository.GetAllPagedAsync(request.Page, request.Size, cancellationToken);
            return _mapper.Map<List<AuthorLookupDto>>(authors);
        }
    }
}
