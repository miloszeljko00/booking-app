using MassTransit;
using SharedEvents;

namespace Orchestrator.Api.Consumers
{
    public class HostGradeCreatedEventConsumer : IConsumer<HostGradeCreatedEvent>
    {
        private IPublishEndpoint _publishEndpoint;
        public HostGradeCreatedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<HostGradeCreatedEvent> context)
        {
            Console.WriteLine("ORKESTRATOR: HOST GRADE KREIRAN, SALJEM EVENT NOTIFICATION SERVISU");
            var @event = new CheckHostNotificationStatusEvent()
            {
                Email = context.Message.Email,
                Grade=context.Message.Grade,
                HostGradingId = context.Message.HostGradingId
            };
            await _publishEndpoint.Publish(@event);
        }
    }
}
