using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;
using UserManagementApplication.Abstractions.Messaging;

namespace UserManagement.Application.Users.Commands;

public sealed record CreateUserCommand(
    string Email,
    string Password,
    List<string> Roles,
    string Name,
    string Surname,
    string Country,
    string City,
    string Street,
    string Number
    ) : ICommand<User> {}
