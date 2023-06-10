using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationGradingApplication.Dtos
{
    public class UpdateAccommodationGradingDTO
    {
        public Guid Id { get; set; }
        public int Grade { get; set; }
    }
}
