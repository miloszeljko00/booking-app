using Grpc.Core;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Abstractions.Connections;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using UserManagement.Domain.ValueObjects;
using UserManagementApplication.Abstractions.Messaging;
using Microsoft.Extensions.Hosting;
using UserManagement.Application.Users.Support.Grpc.Protos;

namespace UserManagement.Application.Users.Commands;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User?>
{
    private readonly IKeyCloakConnection _keyCloakConnection;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _env;
    
    private CreateGuestGrpcService.CreateGuestGrpcServiceClient client;
    public CreateUserCommandHandler(IKeyCloakConnection keyCloakConnection, IUserRepository userRepository, IConfiguration configuration, IHostEnvironment env)
    {
        _keyCloakConnection = keyCloakConnection;
        _userRepository = userRepository;
        _configuration = configuration;
        _env = env;
    }

    public async Task<User?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var response = await _keyCloakConnection.CreateUserAsync(request);

        if (response is false) return null;

        string? userId = await _keyCloakConnection.GetUserIdAsync(request.Email);
        if (userId is null) return null;

        var user = new User(
            Guid.Parse(userId),
            request.Email,
            request.Name,
            request.Surname,
            Address.Create(request.Country, request.City, request.Street, request.Number),
        request.Roles[0]);
        if (request.Roles[0].Equals("guest"))
        { 
            if (_env.EnvironmentName != "Cloud")
            {
                var channel = new Channel(_configuration.GetValue<string>("GrpcDruzina:AccommodationSuggestion:Address") + ":" + _configuration.GetValue<int>("GrpcDruzina:AccommodationSUggestion:Port"), ChannelCredentials.Insecure);
                client = new CreateGuestGrpcService.CreateGuestGrpcServiceClient(channel);
                CreateGuestProtoResponse response1 = await client.createGuestAsync(new CreateGuestProto() { GuestEmail = request.Email });

            }
            else
            {
                using var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcDruzina:AccommodationSuggestion:Address"), new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                client = new CreateGuestGrpcService.CreateGuestGrpcServiceClient(channel);
                CreateGuestProtoResponse response1 = await client.createGuestAsync(new CreateGuestProto() { GuestEmail = request.Email});

            }
        }

        return await _userRepository.CreateAsync(user);    
    }
}
