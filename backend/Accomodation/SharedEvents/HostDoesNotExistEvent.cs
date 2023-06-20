using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedEvents
{
    public class HostDoesNotExistEvent
    {
        public string Email { get; set; }
        public Guid HostGradingId { get; set; }
    }
}
