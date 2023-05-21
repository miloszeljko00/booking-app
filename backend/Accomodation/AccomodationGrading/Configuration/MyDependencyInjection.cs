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
            services.AddScoped<IAccommodationGradingRepository, MongoAccommodationGradingRepository>();
            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<CreateHostGradingCommand, HostGrading>, CreateHostGradingCommandHandler>();
            services.AddScoped<IRequestHandler<GetHostGradingQuery, List<HostGradingDTO>>, GetHostGradingQueryHandler>();
            services.AddScoped<IRequestHandler<CreateAccommodationGradingCommand, AccommodationGrading>, CreateAccommodationGradingCommandHandler>();
            services.AddScoped<IRequestHandler<GetAccommodationGradingQuery, List<AccommodationGradingDTO>>, GetAccommodationGradingQueryHandler>();
            services.AddScoped<IRequestHandler<UpdateHostGradingCommand, HostGrading>, UpdateHostGradingCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteHostGradingCommand, HostGrading>, DeleteHostGradingCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateAccommodationGradingCommand, AccommodationGrading>, UpdateAccommodationGradingCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteAccommodationGradingCommand, AccommodationGrading>, DeleteAccommodationGradingCommandHandler>();
            return services;
        }
    }
}
