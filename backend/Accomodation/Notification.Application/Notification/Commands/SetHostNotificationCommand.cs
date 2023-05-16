using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Application.Dtos;
using Notification.Application.Abstractions.Messaging;
using Notification.Domain.Entities;

namespace Notification.Application.Notification.Commands
{
    public sealed record SetHostNotificationCommand(
        CreateHostNotificationDTO createHostNotificationDTO
        ) : ICommand<HostNotification>
    {
    }
}
