using AutoMapper;
using Library.Domain.Interfaces.Repositories;
using Library.Application.UseCases.BookBorrows.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.BookBorrows.Queries.GetUserBookBorrows
{
    public class GetUserBookBorrowsQueryHandler : IRequestHandler<GetUserBookBorrowsQuery, List<BookBorrowLookupDto>>
    {
        private readonly IBookBorrowRepository _bookBorrowRepository;
        private readonly IMapper _mapper;

        public GetUserBookBorrowsQueryHandler(IBookBorrowRepository bookBorrowRepository, IMapper mapper)
        {
            _bookBorrowRepository = bookBorrowRepository;
            _mapper = mapper;
        }

        public async Task<List<BookBorrowLookupDto>> Handle(GetUserBookBorrowsQuery request, CancellationToken cancellationToken)
        {
            var bookBorrows = await _bookBorrowRepository.GetAllByUserAsync(request.UserId, cancellationToken);
            return _mapper.Map<List<BookBorrowLookupDto>>(bookBorrows);
        }
    }
}
