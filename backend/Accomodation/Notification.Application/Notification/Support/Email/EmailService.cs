using Notification.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Notification.Support.Email
{
    public class EmailService : IEmailService
    {
        public void SendGuestNotification(string email, string operation, string accommodation, string startDate, string endDate)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.To.Add(email);

            MailAddress mailAddress = new MailAddress("holja2208sm022@gmail.com");
            mailMsg.From = mailAddress;

            mailMsg.Subject = "Odgovor na zahtev za rezervaciju";
            mailMsg.Body = "Postovani," +
                "\nVas zahtev za rezervaciju u \"" + accommodation + "\" smestaju od " + startDate + " do " + endDate + " je "+ operation;

            SmtpClient smtpClient = new SmtpClient("smtp.elasticemail.com", 2525);
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("bookingapp@gmail.com", "3D2A27F9308C30EDEF26B875B4FD140EB323");
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMsg);
        }
    }
}
