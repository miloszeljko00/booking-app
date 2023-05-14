using Notification.Application.Abstractions.Messaging;
using Notification.Application.Dtos;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Notification.Commands
{
    public sealed class SetGuestNotificationCommandHandler : ICommandHandler<SetGuestNotificationCommand, GuestNotification>
    {
        private readonly INotificationRepository _repository;
        public SetGuestNotificationCommandHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public INotificationRepository Get_repository()
        {
            return _repository;
        }

        public Task<GuestNotification> Handle(SetGuestNotificationCommand request, CancellationToken cancellationToken)
        {
            GuestNotification guestNotification = GuestNotification.Create(Guid.NewGuid(), request.createGuestNotificationDTO.GuestEmail, DateTime.Now, request.createGuestNotificationDTO.ReceiveAnswer);
            List<GuestNotification> guestNotifications = _repository.GetAllAsync().Result.ToList();
            foreach(GuestNotification gn in guestNotifications)
            {
                if (gn.GuestEmail.EmailAddress.Equals(guestNotification.GuestEmail.EmailAddress))
                {
                    _repository.RemoveAsync(gn.Id);
                    break;
                }
            }
            return _repository.Create(guestNotification);
        }
    }
}
