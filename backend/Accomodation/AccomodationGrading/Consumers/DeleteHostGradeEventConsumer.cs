using AccomodationGradingApplication.Dtos;
using AccomodationGradingApplication.Grading.Commands;
using AccomodationGradingDomain.Entities;
using MassTransit;
using MediatR;
using SharedEvents;

namespace AccomodationGrading.Consumers
{
    public class DeleteHostGradeEventConsumer : IConsumer<DeleteHostGradeEvent>
    {
        private IMediator _mediator;
        private IPublishEndpoint _publishEndpoint;
        public DeleteHostGradeEventConsumer(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<DeleteHostGradeEvent> context)
        {
            Console.WriteLine("BRISEM HOST GRADE");
            var command = new DeleteHostGradingCommand(context.Message.HostGradingId){};
            try
            {
                var result = await _mediator.Send(command);
                if (result.GetType()==typeof(HostGrading))
                {
                    var @event = new HostGradeDeletedEvent()
                    {
                        isDeleted = true,
                    };
                    Console.WriteLine("PUBLISHUJEM EVENT HOSTGRADEDELETED");
                    await _publishEndpoint.Publish(@event);
                } else
                {
                    var @event = new HostGradeDeletedEvent()
                    {
                        isDeleted = false,
                    };
                    Console.WriteLine("*****HOST SA ID-em ZA BRISANJE NE POSTOJI*****");
                    await _publishEndpoint.Publish(@event);
                }
            }
            catch
            {
                var @event = new HostGradeDeletedEvent()
                {
                    isDeleted = false,
                };
                Console.WriteLine("***** PUKLO BRI BRISANJU HOST GRADINGA *****");
                await _publishEndpoint.Publish(@event);
            }
            
        }
    }
}
