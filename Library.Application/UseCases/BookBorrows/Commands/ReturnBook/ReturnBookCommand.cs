using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.BookBorrows.Commands.ReturnBook
{
    public record ReturnBookCommand(Guid BookBorrowId) : IRequest;
}
