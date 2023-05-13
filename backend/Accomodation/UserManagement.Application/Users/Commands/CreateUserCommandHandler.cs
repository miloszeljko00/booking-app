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

namespace UserManagement.Application.Users.Commands;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User?>
{
    private readonly IKeyCloakConnection _keyCloakConnection;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IKeyCloakConnection keyCloakConnection, IUserRepository userRepository)
    {
        _keyCloakConnection = keyCloakConnection;
        _userRepository = userRepository;
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
            Address.Create(request.Country, request.City, request.Street, request.Number));

        return await _userRepository.CreateAsync(user);    
    }
}
