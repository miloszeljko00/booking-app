using MassTransit;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using SharedEvents;

namespace Notification.Consumers
{
    public class CheckHostNotificationStatusEventConsumer : IConsumer<CheckHostNotificationStatusEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private IHostNotificationRepository _hostNotificationRepository;
        public CheckHostNotificationStatusEventConsumer(IPublishEndpoint publishEndpoint, IHostNotificationRepository hostNotificationRepository)
        {
            _publishEndpoint = publishEndpoint;
            _hostNotificationRepository = hostNotificationRepository;
        }

        public async Task Consume(ConsumeContext<CheckHostNotificationStatusEvent> context)
        {
            Console.WriteLine("PROVERAVAM DA LI HOST DOZVOLJAVA NOTIFIKACIJE");
            List<HostNotification> hostNotifications = _hostNotificationRepository.GetAllAsync().Result.ToList();

            foreach (HostNotification hn in hostNotifications)
            {
                if (hn.HostEmail.EmailAddress.Equals(context.Message.Email) && hn.ReceiveAnswerForHostRating)
                {
                    var @event = new HostNotificationStatusEvent()
                    {
                        isTurnedOn = true,
                        Email = context.Message.Email,
                        Grade = context.Message.Grade
                    };
                    await _publishEndpoint.Publish(@event);
                }
                else if (hn.HostEmail.EmailAddress.Equals(context.Message.Email) && !hn.ReceiveAnswerForHostRating)
                {
                    var @event = new HostNotificationStatusEvent()
                    {
                        isTurnedOn = false,
                        Email = context.Message.Email,
                        Grade = context.Message.Grade
                    };
                    await _publishEndpoint.Publish(@event);
                }
                else
                {
                    //TODO: ovde mozda neki error case?
                    var @event = new HostDoesNotExistEvent()
                    {
                        Email = context.Message.Email,
                        HostGradingId=context.Message.HostGradingId

                    };
                    await _publishEndpoint.Publish(@event);
                }
            }
        }
    }
}
