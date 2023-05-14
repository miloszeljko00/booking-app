using MediatR;
using UserManagement.Application.Abstractions.Connections;
using UserManagement.Application.Users.Commands;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using UserManagement.Infrastructure.Connections;
using UserManagement.Infrastructure.Repositories;

namespace UserManagement.Configuration
{
    //Extension methods
    public static class MyDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<DeleteUserCommand, bool>, DeleteUserCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateUserCommand, User?>, UpdateUserCommandHandler>();
            services.AddScoped<IRequestHandler<GetUserByIdQuery, User?>, GetUserByIdQueryHandler>();
            services.AddScoped<IRequestHandler<CreateUserCommand, User?>, CreateUserCommandHandler>();
            return services;
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddScoped<IKeyCloakConnection, KeyCloakConnection>();
            return services;
        }
    }
}
