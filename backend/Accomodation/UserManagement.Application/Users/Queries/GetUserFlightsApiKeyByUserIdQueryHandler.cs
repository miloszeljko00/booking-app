using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Abstractions.Connections;
using UserManagement.Application.Dtos;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using UserManagementApplication.Abstractions.Messaging;

namespace UserManagement.Application.Users.Commands;

public sealed class GetUserFlightsApiKeyByUserIdQueryHandler : ICommandHandler<GetUserFlightsApiKeyByUserIdQuery, UserApiKeyDto?>
{
    private readonly IUserRepository _userRepository;

    public GetUserFlightsApiKeyByUserIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task<UserApiKeyDto?> Handle(GetUserFlightsApiKeyByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetAsync(Guid.Parse(request.UserId));

            return new UserApiKeyDto()
            {
                UserId = user.Id.ToString(),
                ApiKey = user.FlightsApiKey,
            };
        }
        catch
        {
            return null;
        }
    }
}
