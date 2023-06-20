using MassTransit;
using SharedEvents;

namespace Orchestrator.Api.Consumers
{
    public class HostNotificationStatusEventConsumer : IConsumer<HostNotificationStatusEvent>
    {
        private IPublishEndpoint _publishEndpoint;
        public HostNotificationStatusEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<HostNotificationStatusEvent> context)
        {
            Console.WriteLine("KONZUMIRAO SAM HOSTNOTIFICATIONSTATUSEVENT");
            if (context.Message.isTurnedOn)
            {
                var @event = new SendEmailEvent()
                {
                    Email = context.Message.Email,
                    Grade = context.Message.Grade
                };
                await _publishEndpoint.Publish(@event);
            }
        }
    }
}
