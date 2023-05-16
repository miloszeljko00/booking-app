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
    public sealed class SetHostNotificationCommandHandler : ICommandHandler<SetHostNotificationCommand, HostNotification>
    {
        private readonly IHostNotificationRepository _repository;
        public SetHostNotificationCommandHandler(IHostNotificationRepository repository)
        {
            _repository = repository;
        }

        public IHostNotificationRepository Get_repository()
        {
            return _repository;
        }

        public Task<HostNotification> Handle(SetHostNotificationCommand request, CancellationToken cancellationToken)
        {
            HostNotification hostNotification = HostNotification.Create(Guid.NewGuid(), request.createHostNotificationDTO.HostEmail, DateTime.Now, request.createHostNotificationDTO.ReceiveAnswerForCreatedRequest,
                request.createHostNotificationDTO.ReceiveAnswerForCanceledReservation, request.createHostNotificationDTO.ReceiveAnswerForHostRating, request.createHostNotificationDTO.ReceiveAnswerForAccommodationRating, request.createHostNotificationDTO.ReceiveAnswerForHighlightedHostStatus);
            List<HostNotification> hostNotifications = _repository.GetAllAsync().Result.ToList();
            foreach(HostNotification hn in hostNotifications)
            {
                if (hn.HostEmail.EmailAddress.Equals(hostNotification.HostEmail.EmailAddress))
                {
                    _repository.RemoveAsync(hn.Id);
                    break;
                }
            }
            return _repository.Create(hostNotification);
        }
    }
}
