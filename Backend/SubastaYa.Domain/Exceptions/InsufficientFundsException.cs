using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Domain.Exceptions
{
    internal class InsufficientFundsException : DomainException
    {
        public InsufficientFundsException(string message) : base(message)
        {
        }
    }
}
