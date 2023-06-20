using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedEvents
{
    public class HostNotificationStatusEvent
    {
        public bool isTurnedOn { get; set; }
        public string Email { get; set; }
        public int Grade { get; set; }
    }
}
