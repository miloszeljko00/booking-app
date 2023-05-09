using MediatR;
using UserManagement.Application.Abstractions.Connections;
using UserManagement.Application.Users.Commands;
using UserManagement.Infrastructure.Connections;

namespace UserManagement.Configuration
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
            services.AddScoped<IRequestHandler<DeleteUserCommand, bool>, DeleteUserCommandHandler>();
            return services;
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddScoped<IKeyCloakConnection, KeyCloakConnection>();
            return services;
        }
    }
}
