using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.CustomExceptions
{
    sealed class InvalidPictureException : Exception
    {
        public InvalidPictureException() : base() { }
    }
}
