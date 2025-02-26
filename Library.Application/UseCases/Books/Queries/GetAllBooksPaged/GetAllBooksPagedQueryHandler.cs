using AutoMapper;
using Library.Application.Interfaces.Repositories;
using Library.Application.UseCases.Books.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Books.Queries.GetAllBooksPaged
{
    public class GetAllBooksPagedQueryHandler : IRequestHandler<GetAllBooksPagedQuery, List<BookLookupDto>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetAllBooksPagedQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<List<BookLookupDto>> Handle(GetAllBooksPagedQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAllPagedAsync(request.Page, request.Size, cancellationToken);
            return _mapper.Map<List<BookLookupDto>>(books);
        }
    }
}
