using AccomodationGradingApplication.Dtos;
using AccomodationGradingApplication.Grading.Commands;
using AccomodationGradingDomain.ValueObjects;
using MassTransit;
using MediatR;
using SharedEvents;

namespace AccomodationGrading.Consumers
{
    public class CreateHostGradeEventConsumer : IConsumer<CreateHostGradeEvent>
    {
        private IMediator _mediator;
        private IPublishEndpoint _publishEndpoint;
        private readonly ILogger<CreateHostGradeEventConsumer> logger;
        public CreateHostGradeEventConsumer(IMediator mediator, IPublishEndpoint publishEndpoint, ILogger<CreateHostGradeEventConsumer> logger)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateHostGradeEvent> context)
        {
            Console.WriteLine("KREIRAM HOST GRADE");
            CreateHostGradingDTO createHostGradingDTO = new CreateHostGradingDTO()
            {
                Grade = context.Message.Grade,
                GuestEmail = context.Message.GuestEmail,
                HostEmail=context.Message.HostEmail
            };
            var command = new CreateHostGradingCommand(createHostGradingDTO);

            var result = await _mediator.Send(command);
            var @event = new HostGradeCreatedEvent()
            {
                Email = result.HostEmail.EmailAddress,
                Grade = result.Grade,
                HostGradingId = result.Id
            };
            Console.WriteLine("PUBLISHUJEM EVENT HOSTGRADECREATED");
            await _publishEndpoint.Publish(@event);
        }
    }
}
