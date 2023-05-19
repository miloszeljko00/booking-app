using AccomodationGrading.Infrastructure.Grading;
using AccomodationGradingApplication.Grading.Commands;
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
            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<CreateHostGradingCommand, HostGrading>, CreateHostGradingCommandHandler>();
            return services;
        }
    }
}
