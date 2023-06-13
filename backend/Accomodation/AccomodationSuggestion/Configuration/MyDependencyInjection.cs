using AccomodationSuggestion.Application.Dtos;
using AccomodationSuggestion.Application.Suggestion.Queries;
using MediatR;

namespace Accomodation.Configuration
{
    //Extension methods
    public static class MyDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            
            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<GetAllSuggestedFlightsQuery, GetAllSuggestedFlightsResponse>, GetAllSuggestedFlightsQueryHandler>();
            services.AddScoped<IRequestHandler<BookFlightQuery, bool>, BookFlightQueryHandler>();
            return services;
        }
    }
}
