using Notification.Application.Dtos;
using Notification.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Notification.Queries
{
    public sealed record GetNotificationsByHostQuery(string hostEmail) : IQuery<HostNotificationDTO>
    {
    }
}
