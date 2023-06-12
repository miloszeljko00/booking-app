using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Abstractions.Connections;
using UserManagement.Application.Users.Support.Grpc.Protos;
using UserManagement.Domain.Interfaces;
using UserManagementApplication.Abstractions.Messaging;

namespace UserManagement.Application.Users.Commands;

public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly IKeyCloakConnection _keyCloakConnection;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _env;
    public DeleteUserCommandHandler(IKeyCloakConnection keyCloakConnection, IUserRepository userRepository, IConfiguration configuration, IHostEnvironment env)
    {
        _keyCloakConnection = keyCloakConnection;
        _userRepository = userRepository;
        _configuration = configuration;
        _env = env;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId is null) return false;

        var user = await _userRepository.GetAsync(Guid.Parse(request.UserId));
        if (user is null) return false;


        if (_env.EnvironmentName != "Cloud")
        {
            var channel = new Channel(_configuration.GetValue<string>("GrpcDruzina:Accommodation:Address") + ":" + _configuration.GetValue<int>("GrpcDruzina:Accommodation:Port"), ChannelCredentials.Insecure);
            var client = new AccomodationGrpcService.AccomodationGrpcServiceClient(channel);
            MessageResponseProto2 response = await client.communicateAsync(new MessageProto2() { UserEmail = user.Email, UserRole = user.Role });

            if (response is null) return false;
            if (response.CanDelete is false) return false;


            var result = await _keyCloakConnection.DeleteUserAsync(request.UserId);
            if (result is true)
            {
                await _userRepository.RemoveAsync(Guid.Parse(request.UserId));
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            using var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcDruzina:Accommodation:Address"), new GrpcChannelOptions
            {
                HttpHandler = new GrpcWebHandler(new HttpClientHandler())
            });
            var client = new AccomodationGrpcService.AccomodationGrpcServiceClient(channel);
            MessageResponseProto2 response = await client.communicateAsync(new MessageProto2() { UserEmail = user.Email, UserRole = user.Role });

            if (response is null) return false;
            if (response.CanDelete is false) return false;


            var result = await _keyCloakConnection.DeleteUserAsync(request.UserId);
            if (result is true)
            {
                await _userRepository.RemoveAsync(Guid.Parse(request.UserId));
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
