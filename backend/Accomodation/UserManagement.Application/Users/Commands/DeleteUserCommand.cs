using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Abstractions.Messaging;

namespace UserManagement.Application.Users.Commands;

public sealed record DeleteUserCommand(string UserId) : ICommand<bool> {}
