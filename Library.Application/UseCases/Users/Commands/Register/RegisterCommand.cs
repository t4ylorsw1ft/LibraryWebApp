using Library.Application.DTOs.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Users.Commands.Register
{
    public record RegisterCommand(RegisterDto registerDto) : IRequest<JwtPairDto>; 
}
