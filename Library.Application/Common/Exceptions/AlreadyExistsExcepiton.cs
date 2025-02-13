using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Common.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException()
            : base("Such record already exists")
        {

        }

        public AlreadyExistsException(string keyName)
            : base($"Record with such {keyName} already exists")
        {

        }
    }
}
