using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Dtos;
using UserManagement.Domain.Entities;
using UserManagementApplication.Abstractions.Messaging;

namespace UserManagement.Application.Users.Commands;

public sealed record UpdateUserFlightsApiKeyCommand(
    string UserId,
    string ApiKey
    ) : ICommand<UserApiKeyDto?> {}
