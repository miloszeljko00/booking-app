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
    public class AccommodationGradingServerGrpcServiceImpl : AccommodationGradingNotificationGrpcService.AccommodationGradingNotificationGrpcServiceBase
    {
        private readonly IEmailService _emailService;
        private readonly IHostNotificationRepository _repository;
        public AccommodationGradingServerGrpcServiceImpl(IHostNotificationRepository repository)
        {
            _emailService = new EmailService();
            _repository = repository;
        }
        public override Task<MessageResponseProto5> accommodationGrading(MessageProto5 request, ServerCallContext context)
        {
            List<HostNotification> hostNotifications = _repository.GetAllAsync().Result.ToList();
            MessageResponseProto5 response = new MessageResponseProto5();

            foreach (HostNotification hn in hostNotifications)
            {
                if (hn.HostEmail.EmailAddress.Equals(request.Email) && hn.ReceiveAnswerForAccommodationRating)
                {
                    _emailService.SendAccommodationGradingNotification(request.Email, request.Accommodation, request.Grade);
                    response.Status = "SENT";
                }
                else if (hn.HostEmail.EmailAddress.Equals(request.Email) && !hn.ReceiveAnswerForAccommodationRating)
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
