using AccomodationSuggestion.Application.Dtos;
using AccomodationSuggestion.Application.Suggestion.Queries;
using AccomodationSuggestion.Domain.Entities;
using AccomodationSuggestion.Domain.Interfaces;
using AccomodationSuggestion.Infrastructure.Persistence.Settings;
using AccomodationSuggestion.Infrastructure.Repositories;
using MediatR;

namespace Accomodation.Configuration
{
    //Extension methods
    public static class MyDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccommodationSuggestionRepository, Neo4jAccommodationSuggestionRepository>();
            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<GetAllSuggestedFlightsQuery, GetAllSuggestedFlightsResponse>, GetAllSuggestedFlightsQueryHandler>();
            services.AddScoped<IRequestHandler<BookFlightQuery, bool>, BookFlightQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllUserNodesQuery, UserNode>, GetAllUserNodesQueryHandler>();
            services.AddScoped<IRequestHandler<GetRecommendedAccommodationQuery, List<AccommodationNode>>, GetRecommendedAccommodationQueryHandler>();
            return services;
        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager builderConfiguration)
        {
            services.Configure<DatabaseSettings>(builderConfiguration.GetSection(DatabaseSettings.OptionName));
            return services;
        }
    }
}
