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
            return services;
        }
    }
}
