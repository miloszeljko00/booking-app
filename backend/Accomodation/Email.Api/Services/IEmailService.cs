using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Email.Api.Services.Email
{
    public interface IEmailService
    {
        void SendGuestNotification(string email, string operation, string accommodation, string startDate, string endDate);
        void SendHostRequestNotification(string email, string accommodation, string startDate, string endDate);
        void SendHostCancelReservationNotification(string email, string accommodation, string startDate, string endDate);
        void SendHostGradingNotification(string email, int grade);
        void SendAccommodationGradingNotification(string email, string accommodation, int grade);
        void SendHighlightedHostNotification(string email, string status);
    }
}
