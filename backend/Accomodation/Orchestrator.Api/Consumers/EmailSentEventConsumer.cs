using MassTransit;
using SharedEvents;

namespace Orchestrator.Api.Consumers
{
    public class EmailSentEventConsumer : IConsumer<EmailSentEvent>
    {
        public async Task Consume(ConsumeContext<EmailSentEvent> context)
        {
            //TODO: U nekoj zdravoj prici ovde bi se slao odgovor na neki hub/socket ovako ide samo log
            Console.WriteLine("Transakcija uspesno odradjena: Vas Saga Pattern <3");
            await Task.CompletedTask;
        }
    }
}
