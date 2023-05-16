using Notification.Domain.Primitives;
using Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Entities
{
    public class HostNotification : Entity
    {
        public Email HostEmail { get; init; }
        public DateTime LastModified { get; init; }
        public bool ReceiveAnswerForCreatedRequest { get; init; }
        public bool ReceiveAnswerForCanceledReservation { get; init; }
        public bool ReceiveAnswerForHostRating { get; init; }
        public bool ReceiveAnswerForAccommodationRating { get; init; }
        public bool ReceiveAnswerForHighlightedHostStatus { get; init; }

        private HostNotification(Guid id, Email hostEmail, DateTime lastModified, bool receiveAnswerForCreatedRequest,
            bool receiveAnswerForCanceledReservation, bool receiveAnswerForHostRating, bool receiveAnswerForAccommodationRating,
            bool receiveAnswerForHighlightedHostStatus) : base(id)
        {
            HostEmail = hostEmail;
            LastModified = lastModified;
            ReceiveAnswerForCreatedRequest = receiveAnswerForCreatedRequest;
            ReceiveAnswerForCanceledReservation = receiveAnswerForCanceledReservation;
            ReceiveAnswerForHostRating = receiveAnswerForHostRating;
            ReceiveAnswerForAccommodationRating = receiveAnswerForAccommodationRating;
            ReceiveAnswerForHighlightedHostStatus = receiveAnswerForHighlightedHostStatus;
        }

        public static HostNotification Create(Guid id, string hostEmail, DateTime lastModified, bool receiveAnswerForCreatedRequest,
            bool receiveAnswerForCanceledReservation, bool receiveAnswerForHostRating, bool receiveAnswerForAccommodationRating,
            bool receiveAnswerForHighlightedHostStatus)
        {
            return new HostNotification(id, Email.Create(hostEmail), lastModified, receiveAnswerForCreatedRequest,
            receiveAnswerForCanceledReservation, receiveAnswerForHostRating, receiveAnswerForAccommodationRating,
            receiveAnswerForHighlightedHostStatus);
        }
    }
}
