using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.BookBorrows.Commands.BorrowBook
{
    public record BorrowBookCommand(Guid UserId, Guid BookId) : IRequest;
}
