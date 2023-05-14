using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Users.Commands;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Abstractions.Connections
{
    public interface IKeyCloakConnection
    {
        public Task<bool> DeleteUserAsync(string userId);
        public Task<bool> CreateUserAsync(CreateUserCommand createUserCommand);
        public Task<string?> GetUserIdAsync(string email);
    }
}
