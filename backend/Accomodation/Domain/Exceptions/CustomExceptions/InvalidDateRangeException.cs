﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestionDomain.Exceptions.CustomExceptions
{
    public sealed class InvalidDateRangeException : Exception
    {
        public InvalidDateRangeException() : base() { }
    }
}
