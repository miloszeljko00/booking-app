using Grpc.Core;
using MediatR;
using Notification.Application.Dtos;
using Notification.Application.Notification.Commands;
using Notification.Application.Notification.Queries;
using Notification.Application.Notification.Support.Email;
using Notification.Application.Notification.Support.Grpc.Protos;
using Notification.Application.Notification.Support.Grpc;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Notification.Infrastructure.Notification;
using Rs.Ac.Uns.Ftn.Grpc;
using Notification.Infrastructure.Persistance.Settings;

namespace Accomodation.Configuration
{
    //Extension methods
    public static class MyDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IGuestNotificationRepository, MongoGuestNotificationRepository>();
            services.AddScoped<IHostNotificationRepository, MongoHostNotificationRepository>();
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<GetNotificationsByGuestQuery, GuestNotificationDTO>, GetNotificationsByGuestQueryHandler>();
            services.AddScoped<IRequestHandler<SetGuestNotificationCommand, GuestNotification>, SetGuestNotificationCommandHandler>();
            services.AddScoped<IRequestHandler<GetNotificationsByHostQuery, HostNotificationDTO>, GetNotificationsByHostQueryHandler>();
            services.AddScoped<IRequestHandler<SetHostNotificationCommand, HostNotification>, SetHostNotificationCommandHandler>();
            return services;
        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
        {
            services.Configure<DatabaseSettings>(builderConfiguration.GetSection(DatabaseSettings.OptionName));
            return services;
        }
    }
}
