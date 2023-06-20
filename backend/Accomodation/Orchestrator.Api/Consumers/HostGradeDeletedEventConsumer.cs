using MassTransit;
using SharedEvents;

namespace Orchestrator.Api.Consumers
{
    public class HostGradeDeletedEventConsumer : IConsumer<HostGradeDeletedEvent>
    {
        public async Task Consume(ConsumeContext<HostGradeDeletedEvent> context)
        {
            if (context.Message.isDeleted)
            {
                Console.WriteLine("ROOL BACK ODRADJEN: ORKESTRATOR: HOST GRADING USPESNO OBRISAN");
            }
            else
            {
                Console.WriteLine("ROOL BACK FAIL: ORKESTRATOR: HOST GRADE NIJE USPESNO OBRISAN");
            }
            await Task.CompletedTask;
        }
    }
}
