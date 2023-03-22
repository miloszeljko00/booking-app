using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsBooking.Support.ErrorHandler.Model
{
    public class InvalidPriceException : Exception
    {
        public InvalidPriceException():base(){}
    }
}
