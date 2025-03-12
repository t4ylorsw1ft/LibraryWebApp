using AutoMapper;
using Library.Application.Common.Exceptions;
using Library.Domain.Interfaces.Repositories;
using Library.Application.UseCases.Books.DTOs;
using Library.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Books.Queries.GetBookByISBN
{
    public class GetBookByISBNQueryHandler : IRequestHandler<GetBookByISBNQuery, BookDetailsDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetBookByISBNQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookDetailsDto> Handle(GetBookByISBNQuery request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByISBNAsync(request.ISBN, cancellationToken);

            if (book == null)
                throw new NotFoundException(typeof(Book), request.ISBN);

            return _mapper.Map<BookDetailsDto>(book);
        }
    }
}
