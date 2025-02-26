using Library.Application.UseCases.Books.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Books.Commands.CreateBook
{
    public record CreateBookCommand(CreateBookDto BookDto) : IRequest<BookDetailsDto>;
}
