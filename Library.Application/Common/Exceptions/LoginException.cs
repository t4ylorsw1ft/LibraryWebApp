using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Common.Exceptions
{
    public class LoginException : Exception
    {
        public LoginException() 
            : base($"Invalid email or password")
        {

        }
    }
}
