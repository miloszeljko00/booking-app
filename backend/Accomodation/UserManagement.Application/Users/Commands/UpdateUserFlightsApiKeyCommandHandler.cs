using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Abstractions.Connections;
using UserManagement.Application.Dtos;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using UserManagement.Domain.ValueObjects;
using UserManagementApplication.Abstractions.Messaging;

namespace UserManagement.Application.Users.Commands;

public sealed class UpdateUserFlightsApiKeyCommandHandler : ICommandHandler<UpdateUserFlightsApiKeyCommand, UserApiKeyDto?>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserFlightsApiKeyCommandHandler(IKeyCloakConnection keyCloakConnection, IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserApiKeyDto?> Handle(UpdateUserFlightsApiKeyCommand request, CancellationToken cancellationToken)
    {

        var user = await _userRepository.GetAsync(Guid.Parse(request.UserId));

        if (user is null) return null;

        user.FlightsApiKey = request.ApiKey;
        await _userRepository.UpdateAsync(user.Id, user);

        return new UserApiKeyDto()
        {
            UserId = user.Id.ToString(),
            ApiKey = user.FlightsApiKey,
        };
    }
}
