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

namespace Library.Application.UseCases.Books.Queries.GetBookById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDetailsDto>
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;

        public GetBookByIdQueryHandler(IMapper mapper, IBookRepository bookRepository) 
        { 
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookDetailsDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);

            if (book == null)
                throw new NotFoundException(typeof(Book), request.Id);

            var bookDto = _mapper.Map<BookDetailsDto>(book);

            return bookDto;
        }
    }
}
