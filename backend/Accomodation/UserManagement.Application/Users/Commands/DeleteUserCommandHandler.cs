﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Abstractions.Connections;
using UserManagement.Domain.Interfaces;
using UserManagementApplication.Abstractions.Messaging;

namespace UserManagement.Application.Users.Commands;

public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly IKeyCloakConnection _keyCloakConnection;
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IKeyCloakConnection keyCloakConnection, IUserRepository userRepository)
    {
        _keyCloakConnection = keyCloakConnection;
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId is null) return false;

        var result = await _keyCloakConnection.DeleteUserAsync(request.UserId);
        if(result is true)
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
