using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Notification.Support.Email
{
    public interface IEmailService
    {
        void SendGuestNotification(string email, string operation, string accommodation, string startDate, string endDate);
    }
}
