using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Notification.Application.Dtos
{
    public class CreateGuestNotificationDTO
    {
        public string GuestEmail { get; set; }
        public bool ReceiveAnswer { get; set; }
    }
}
