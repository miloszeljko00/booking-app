using AccomodationGrading.Infrastructure.Grading;
using AccomodationGrading.Infrastructure.Persistance.Settings;
using AccomodationGradingApplication.Dtos;
using AccomodationGradingApplication.Grading.Commands;
using AccomodationGradingApplication.Grading.Queries;
using AccomodationGradingDomain.Entities;
using AccomodationGradingDomain.Interfaces;
using MediatR;

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

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager builderConfiguration)
        {
            services.Configure<DatabaseSettings>(builderConfiguration.GetSection(DatabaseSettings.OptionName));
            return services;
        }
    }
}
