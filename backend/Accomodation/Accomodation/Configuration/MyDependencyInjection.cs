using Application.Accommodation.Commands;
using Application.Accommodation.Queries;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Accommodation;
using MediatR;

namespace Accomodation.Configuration
{
    //Extension methods
    public static class MyDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccommodationRepository, InMemoryAccommodationRepository>();
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
