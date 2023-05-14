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

public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, User?>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IKeyCloakConnection keyCloakConnection, IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {

        var user = await _userRepository.GetAsync(Guid.Parse(request.UserId));

        if (user is null) return null;

        user.Name = request.Name;
        user.Surname = request.Surname;
        user.Address = Address.Create(request.Country, request.City, request.Street, request.Number);

        await _userRepository.UpdateAsync(user.Id, user);

        return user;
    }
}
