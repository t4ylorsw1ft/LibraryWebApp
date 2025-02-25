using Library.Application.DTOs.Books;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Books.Commands.UpdateBook
{
    public record UpdateBookCommand(UpdateBookDto BookDto) : IRequest<BookDetailsDto>;
}
