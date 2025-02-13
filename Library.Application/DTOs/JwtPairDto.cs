using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs
{
    public class JwtPairDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
