using AccomodationGrading.Infrastructure.Grading;
using AccomodationGradingApplication.Dtos;
using AccomodationGradingApplication.Grading.Commands;
using AccomodationGradingDomain.Entities;
using AccomodationGradingDomain.Interfaces;
using MediatR;
using Notification.Application.Notification.Queries;

namespace AccomodationGrading.Configuration
{
    //Extension methods
    public static class MyDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IHostGradingRepository, MongoHostGradingRepository>();
            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<CreateHostGradingCommand, HostGrading>, CreateHostGradingCommandHandler>();
            services.AddScoped<IRequestHandler<GetHostGradingQuery, List<HostGradingDTO>>, GetHostGradingQueryHandler>();
            services.AddScoped<IRequestHandler<UpdateHostGradingCommand, HostGrading>, UpdateHostGradingCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteHostGradingCommand, HostGrading>, DeleteHostGradingCommandHandler>();

            return services;
        }
    }
}
