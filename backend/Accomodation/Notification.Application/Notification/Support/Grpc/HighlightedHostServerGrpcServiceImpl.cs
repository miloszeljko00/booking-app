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
        public HighlightedHostServerGrpcServiceImpl()
        {
            _emailService = new EmailService();
        }
        public override Task<MessageResponseProto7> check(MessageProto7 request, ServerCallContext context)
        {
            MessageResponseProto7 response = new MessageResponseProto7(); ;
            _emailService.SendHighlightedHostNotification(request.Email, request.Status);
            response.Status = "SENT";
            return Task.FromResult(response);
        }
    }
}
