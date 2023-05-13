using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Notification.Application.Dtos;
using Notification.Application.Notification.Support.Email;
using Notification.Application.Notification.Support.Grpc.Protos;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Rs.Ac.Uns.Ftn.Grpc;

namespace Notification.Application.Notification.Support.Grpc
{
    public class ServerGrpcServiceImpl : GuestNotificationGrpcService.GuestNotificationGrpcServiceBase
    {
        private readonly IEmailService _emailService;
        private readonly INotificationRepository _repository;
        public ServerGrpcServiceImpl(INotificationRepository repository)
        {
            _emailService = new EmailService();
            _repository = repository;
        }
        public override Task<MessageResponseProto> communicate(MessageProto request, ServerCallContext context)
        {
            List<GuestNotification> guestNotifications = _repository.GetAllAsync().Result.ToList();
            MessageResponseProto response = new MessageResponseProto(); ;

            foreach (GuestNotification gn in guestNotifications)
            {
                if (gn.GuestEmail.EmailAddress.Equals(request.Email) && gn.ReceiveAnswer)
                {
                    _emailService.SendGuestNotification(request.Email, request.Operation, request.Accommodation, request.StartDate, request.EndDate);
                    response.Status = "SENT";
                }
                else if (gn.GuestEmail.EmailAddress.Equals(request.Email) && !gn.ReceiveAnswer)
                {
                    response.Status = "NOT SENT";
                }
                else
                {
                    response.Status = "NOT FOUND";
                }
            }
            return Task.FromResult(response);
        }
    }
}
