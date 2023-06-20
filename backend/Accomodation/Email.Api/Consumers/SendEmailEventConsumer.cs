using Email.Api.Services.Email;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedEvents;

namespace Email.Api.Consumers;

public class SendEmailEventConsumer: IConsumer<SendEmailEvent>
{
    private readonly ILogger<SendEmailEventConsumer> _logger;
    private IEmailService _emailService;
    private IPublishEndpoint _publishEndpoint;

    public SendEmailEventConsumer(ILogger<SendEmailEventConsumer> logger, IEmailService emailService, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _emailService = emailService;
        _publishEndpoint = publishEndpoint;
    }
    public async Task Consume(ConsumeContext<SendEmailEvent> context)
    {
        _logger.LogInformation(@"Send email event received!!!");
        var mesage = context.Message;
        try
        {
            _emailService.SendHostGradingNotification(mesage.Email, mesage.Grade);
            _logger.LogInformation("Email sent to: {@Guest}", mesage.Email);
            var @event = new EmailSentEvent()
            {
                isSent = true
            };
            await _publishEndpoint.Publish(@event);
        }
        catch(Exception e)
        {
            //TODO: return to orchestrator failure
            _logger.LogInformation("USAO U CATCH");
        }
    }
}