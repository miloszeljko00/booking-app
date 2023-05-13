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

namespace Accomodation.Configuration
{
    //Extension methods
    public static class MyDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<INotificationRepository, MongoNotificationRepository>();
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<GetNotificationsByGuestQuery, GuestNotificationDTO>, GetNotificationsByGuestQueryHandler>();
            services.AddScoped<IRequestHandler<SetGuestNotificationCommand, GuestNotification>, SetGuestNotificationCommandHandler>();
            return services;
        }
    }
}
