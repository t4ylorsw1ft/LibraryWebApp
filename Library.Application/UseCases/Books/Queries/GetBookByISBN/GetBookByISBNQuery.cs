using Library.Application.DTOs.Books;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Books.Queries.GetBookByISBN
{
    public record GetBookByISBNQuery(string ISBN) : IRequest<BookDetailsDto>;
}
