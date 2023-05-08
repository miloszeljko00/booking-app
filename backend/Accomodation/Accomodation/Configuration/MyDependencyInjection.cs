using Accomodation.Infrastructure.Accommodation;
using AccomodationApplication.Accommodation.Commands;
using AccomodationApplication.Accommodation.Queries;
using AccomodationDomain.Entities;
using AccomodationDomain.Interfaces;
using AccomodationInfrastructure;
using AccomodationInfrastructure.Accommodation;
using MediatR;

namespace Accomodation.Configuration
{
    //Extension methods
    public static class MyDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            //services.AddScoped<IAccommodationRepository, InMemoryAccommodationRepository>();
            services.AddScoped<IAccommodationRepository, MongoAccommodationRepository>();
            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<GetAllAccommodationsQuery, IReadOnlyCollection<Accommodation>>, GetAllAccommodationsQueryHandler>();
            services.AddScoped<IRequestHandler<CreateAccommodationCommand, Accommodation>, CreateAccommodationCommandHandler>();
            return services;
        }
    }
}
