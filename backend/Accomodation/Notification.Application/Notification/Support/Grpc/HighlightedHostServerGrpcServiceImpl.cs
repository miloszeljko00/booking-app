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
    public class HighlightedHostServerGrpcServiceImpl : HighlightedHostGrpcService.HighlightedHostGrpcServiceBase
    {
        private readonly IEmailService _emailService;
        private readonly IHostNotificationRepository _repository;

        public HighlightedHostServerGrpcServiceImpl(IHostNotificationRepository repository)
        {
            _repository = repository;
            _emailService = new EmailService();
        }
        public override Task<MessageResponseProto7> check(MessageProto7 request, ServerCallContext context)
        {
            List<HostNotification> hostNotifications = _repository.GetAllAsync().Result.ToList();
            MessageResponseProto7 response = new MessageResponseProto7();

            foreach (HostNotification hn in hostNotifications)
            {
                if (hn.HostEmail.EmailAddress.Equals(request.Email) && hn.ReceiveAnswerForHighlightedHostStatus)
                {
                    _emailService.SendHighlightedHostNotification(request.Email, request.Status);
                    response.Status = "SENT";
                }
                else if (hn.HostEmail.EmailAddress.Equals(request.Email) && !hn.ReceiveAnswerForHighlightedHostStatus)
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
