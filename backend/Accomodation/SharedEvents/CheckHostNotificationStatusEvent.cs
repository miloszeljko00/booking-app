﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedEvents
{
    public class CheckHostNotificationStatusEvent
    {
        public string Email { get; set; }
        public int Grade { get; set; }
        public Guid HostGradingId { get; set; }
    }
}
