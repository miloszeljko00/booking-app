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
    public class HostGradingServerGrpcServiceImpl : HostGradingNotificationGrpcService.HostGradingNotificationGrpcServiceBase
    {
        private readonly IEmailService _emailService;
        private readonly IHostNotificationRepository _repository;
        public HostGradingServerGrpcServiceImpl(IHostNotificationRepository repository)
        {
            _emailService = new EmailService();
            _repository = repository;
        }
        public override Task<MessageResponseProto4> hostGrading(MessageProto4 request, ServerCallContext context)
        {
            List<HostNotification> hostNotifications = _repository.GetAllAsync().Result.ToList();
            MessageResponseProto4 response = new MessageResponseProto4(); ;

            foreach (HostNotification hn in hostNotifications)
            {
                if (hn.HostEmail.EmailAddress.Equals(request.Email) && hn.ReceiveAnswerForHostRating)
                {
                    _emailService.SendHostGradingNotification(request.Email, request.Grade);
                    response.Status = "SENT";
                }
                else if (hn.HostEmail.EmailAddress.Equals(request.Email) && !hn.ReceiveAnswerForHostRating)
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
