using Notification.Domain.Exceptions.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.ValueObjects
{
    public class Email
    {
        public string EmailAddress { get;init;}
        private Email(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
        public static Email Create(string emailAddress)
        {
            var email = new Email(emailAddress);
            if (email.IsValidEmail())
            {
                return email;
            }
            else
            {
                throw new InvalidEmailException();
            }
        }

        private bool IsValidEmail()
        {
            if (string.IsNullOrEmpty(EmailAddress))
            {
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(EmailAddress);
                return addr.Address == EmailAddress;
            }
            catch
            {
                return false;
            }
        }
    }
}
