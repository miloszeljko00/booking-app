using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Abstractions.Connections;
using UserManagementApplication.Abstractions.Messaging;

namespace UserManagement.Application.Users.Commands;

public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly IKeyCloakConnection _keyCloakConnection;

    public DeleteUserCommandHandler(IKeyCloakConnection keyCloakConnection)
    {
        _keyCloakConnection = keyCloakConnection;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId is null) return false;

        return await _keyCloakConnection.DeleteUserAsync(request.UserId);
    }
}
