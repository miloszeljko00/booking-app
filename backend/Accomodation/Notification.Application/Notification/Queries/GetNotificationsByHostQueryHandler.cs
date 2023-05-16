using Notification.Application.Abstractions.Messaging;
using Notification.Application.Dtos;
using Notification.Application.Notification.Queries;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Notification.Queries
{
    public sealed class GetNotificationsByHostQueryHandler : IQueryHandler<GetNotificationsByHostQuery, HostNotificationDTO>
    {
        private readonly IHostNotificationRepository _repository;
        public GetNotificationsByHostQueryHandler(IHostNotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<HostNotificationDTO> Handle(GetNotificationsByHostQuery request, CancellationToken cancellationToken)
        {
            List<HostNotification> hostNotifications = _repository.GetAllAsync().Result.ToList();
            foreach (HostNotification hn in hostNotifications)
            {
                if (hn.HostEmail.EmailAddress.Equals(request.hostEmail))
                {
                    return new HostNotificationDTO
                    {
                        LastModified = hn.LastModified,
                        HostEmail = hn.HostEmail.EmailAddress,
                        ReceiveAnswerForCreatedRequest = hn.ReceiveAnswerForCreatedRequest,
                        ReceiveAnswerForCanceledReservation = hn.ReceiveAnswerForCanceledReservation,
                        ReceiveAnswerForHostRating = hn.ReceiveAnswerForHostRating,
                        ReceiveAnswerForAccommodationRating = hn.ReceiveAnswerForAccommodationRating,
                        ReceiveAnswerForHighlightedHostStatus = hn.ReceiveAnswerForHighlightedHostStatus
                    };
                }
            }
            throw new Exception("Notification settings not found");
        }
    }
}
