using Notification.Domain.Primitives;
using Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Entities
{
    public class GuestNotification : Entity
    {
        public Email GuestEmail { get; init; }
        public DateTime LastModified { get; init; }
        public bool ReceiveAnswer { get; init; }

        private GuestNotification(Guid id, Email guestEmail, DateTime lastModified, bool receiveAnswer) : base(id)
        {
            GuestEmail = guestEmail;
            LastModified = lastModified;
            ReceiveAnswer = receiveAnswer;
        }

        public static GuestNotification Create(Guid id, string email, DateTime lastModified, bool receiveAnswer)
        {
            return new GuestNotification(id, Email.Create(email), lastModified, receiveAnswer);
        }
    }
}
