﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Books.Commands.DeleteBook
{
    public record DeleteBookCommand(Guid Id) : IRequest;
}
