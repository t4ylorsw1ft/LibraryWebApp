using AutoMapper;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Books.Queries.GetAllBooksByAuthor
{
    public class GetAllBooksByAuthorQueryHandler : IRequestHandler<GetAllBooksByAuthorQuery, List<BookLookupDto>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetAllBooksByAuthorQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<List<BookLookupDto>> Handle(GetAllBooksByAuthorQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAllByAuthorAsync(request.AuthorId, cancellationToken);
            return _mapper.Map<List<BookLookupDto>>(books);
        }
    }
}
