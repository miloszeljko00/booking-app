using Notification.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Notification.Support.Email
{
    public class EmailService : IEmailService
    {
        private readonly MailAddress _fromMailAddress;

        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            _fromMailAddress = new("holja2208sm022@gmail.com");
            _smtpClient = new("smtp.elasticemail.com", 2525);
            System.Net.NetworkCredential credentials =
                new("bookingapp@gmail.com", "3D2A27F9308C30EDEF26B875B4FD140EB323");
            _smtpClient.Credentials = credentials;
            _smtpClient.EnableSsl = true;
        }
        public void SendGuestNotification(string email, string operation, string accommodation, string startDate, string endDate)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.To.Add(email);
            mailMsg.From = _fromMailAddress;

            mailMsg.Subject = "Odgovor na zahtev za rezervaciju";
            mailMsg.Body = "Postovani," +
                "\nVas zahtev za rezervaciju u \"" + accommodation + "\" smestaju od " + startDate + " do " + endDate + " je " + operation;

            SendMessage(mailMsg);
        }
        public void SendHostRequestNotification(string email, string accommodation, string startDate, string endDate)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.To.Add(email);
            mailMsg.From = _fromMailAddress;

            mailMsg.Subject = "Zahtev za rezervaciju";
            mailMsg.Body = "Postovani," +
                "\nPristigao vam je zahtev za rezervaciju u \"" + accommodation + "\" smestaju od " + startDate + " do " + endDate;

            SendMessage(mailMsg);
        }

        public void SendHostCancelReservationNotification(string email, string accommodation, string startDate, string endDate)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.To.Add(email);
            mailMsg.From = _fromMailAddress;

            mailMsg.Subject = "Obavestenje o otkazivanju rezervacije";
            mailMsg.Body = "Postovani," +
                "\nOtkazana je rezervacija u \"" + accommodation + "\" smestaju od " + startDate + " do " + endDate;

            SendMessage(mailMsg);
        }

        public void SendHostGradingNotification(string email, int grade)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.To.Add(email);
            mailMsg.From = _fromMailAddress;

            mailMsg.Subject = "Obavestenje o oceni hosta";
            mailMsg.Body = "Postovani," +
                "\nUpravo ste ocenjeni ocenom " + grade;

            SendMessage(mailMsg);
        }

        public void SendAccommodationGradingNotification(string email, string accommodation, int grade)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.To.Add(email);
            mailMsg.From = _fromMailAddress;

            mailMsg.Subject = "Obavestenje o oceni smestaja";
            mailMsg.Body = "Postovani," +
                "\nVas smestaj \"" + accommodation + "\" je upravo ocenjen ocenom " + grade;

            SendMessage(mailMsg);
        }

        public void SendHighlightedHostNotification(string email, string status)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.To.Add(email);
            mailMsg.From = _fromMailAddress;

            mailMsg.Subject = "Obavestenje o statusu istaknutog hosta";
            mailMsg.Body = "Postovani," +
                "\nObavestavamo vas da ste " + status + " status istaknutog hosta";

            SendMessage(mailMsg);
        }
        private void SendMessage(MailMessage mailMsg)
        {
            try
            {
                _smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
