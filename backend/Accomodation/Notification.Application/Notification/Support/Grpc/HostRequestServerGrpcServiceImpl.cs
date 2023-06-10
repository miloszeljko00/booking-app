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
    public class HostRequestServerGrpcServiceImpl : HostRequestNotificationGrpcService.HostRequestNotificationGrpcServiceBase
    {
        private readonly IEmailService _emailService;
        private readonly IHostNotificationRepository _repository;
        public HostRequestServerGrpcServiceImpl(IHostNotificationRepository repository)
        {
            _emailService = new EmailService();
            _repository = repository;
        }
        public override Task<MessageResponseProto3> communicate(MessageProto3 request, ServerCallContext context)
        {
            List<HostNotification> hostNotifications = _repository.GetAllAsync().Result.ToList();
            MessageResponseProto3 response = new MessageResponseProto3(); ;

            foreach (HostNotification hn in hostNotifications)
            {
                if (hn.HostEmail.EmailAddress.Equals(request.Email) && hn.ReceiveAnswerForCreatedRequest)
                {
                    _emailService.SendHostRequestNotification(request.Email, request.Accommodation, request.StartDate, request.EndDate);
                    response.Status = "SENT";
                }
                else if (hn.HostEmail.EmailAddress.Equals(request.Email) && !hn.ReceiveAnswerForCreatedRequest)
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
