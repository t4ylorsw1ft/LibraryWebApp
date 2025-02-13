using Library.Application.DTOs;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string username, string email, string password);
        Task<JwtPairDto> LoginAsync(string email, string password);
        Task<JwtPairDto> RefreshAsync(string refreshToken);
    }
}
