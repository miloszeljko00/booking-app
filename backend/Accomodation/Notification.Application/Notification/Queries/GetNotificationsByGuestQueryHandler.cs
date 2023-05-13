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
    public sealed class GetNotificationsByGuestQueryHandler : IQueryHandler<GetNotificationsByGuestQuery, GuestNotificationDTO>
    {
        private readonly INotificationRepository _repository;
        public GetNotificationsByGuestQueryHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<GuestNotificationDTO> Handle(GetNotificationsByGuestQuery request, CancellationToken cancellationToken)
        {
            List<GuestNotification> guestNotifications = _repository.GetAllAsync().Result.ToList();
            foreach (GuestNotification gn in guestNotifications)
            {
                if (gn.GuestEmail.EmailAddress.Equals(request.guestEmail))
                {
                    return new GuestNotificationDTO
                    {
                        LastModified = gn.LastModified,
                        GuestEmail = gn.GuestEmail.EmailAddress,
                        ReceiveAnswer = gn.ReceiveAnswer
                    };
                }
            }
            throw new Exception("Notification settings not found");
        }
    }
}
