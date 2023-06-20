using MassTransit;
using Microsoft.Extensions.Options;
using Orchestrator.Api.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection(MessageBrokerSettings.SectionName));
builder.Services.AddSingleton(provider => provider.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddMassTransit(configurator =>
{
    var assembly = typeof(Program).Assembly;
    configurator.SetKebabCaseEndpointNameFormatter();
    configurator.AddConsumers(assembly);
    configurator.AddSagaStateMachines(assembly);
    configurator.AddSagas(assembly);
    configurator.AddActivities(assembly);
    configurator.UsingRabbitMq((context, cfg) =>
    {
        var messageBrokerSettings = context.GetRequiredService<MessageBrokerSettings>();
        cfg.Host(new Uri(messageBrokerSettings.Host), h =>
        {
            h.Username(messageBrokerSettings.Username);
            h.Password(messageBrokerSettings.Password);
        });
        cfg.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
