using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces.Security
{
    public interface IJWTProvider
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
