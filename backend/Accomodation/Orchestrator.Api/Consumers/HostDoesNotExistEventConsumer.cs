using MassTransit;
using SharedEvents;

namespace Orchestrator.Api.Consumers
{
    public class HostDoesNotExistEventConsumer : IConsumer<HostDoesNotExistEvent>
    {
        private IPublishEndpoint _publishEndpoint;
        public HostDoesNotExistEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<HostDoesNotExistEvent> context)
        {
            var @event = new DeleteHostGradeEvent()
            {
                Email = context.Message.Email,
                HostGradingId = context.Message.HostGradingId
            };
            await _publishEndpoint.Publish(@event);
        }
    }
}
