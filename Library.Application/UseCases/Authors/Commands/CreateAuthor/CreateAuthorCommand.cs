using Library.Application.DTOs.Authors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Authors.Commands.CreateAuthor
{
    public record CreateAuthorCommand(CreateAuthorDto CreateAuthorDto) : IRequest<AuthorDetailsDto>;
}
