using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsBooking.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string ApiKey { get; set; } = "";
    }
}
