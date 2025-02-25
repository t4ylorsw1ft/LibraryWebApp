using Library.Application.DTOs.BookBorrows;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.BookBorrows.Queries.GetUserBookBorrows
{
    public record GetUserBookBorrowsQuery(Guid UserId) : IRequest<List<BookBorrowLookupDto>>;
}
