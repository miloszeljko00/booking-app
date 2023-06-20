using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedEvents
{
    public class CreateHostGradeEvent
    {
        public string GuestEmail { get; set; } = null!;
        public string HostEmail { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Grade { get; set; }
    }
}
