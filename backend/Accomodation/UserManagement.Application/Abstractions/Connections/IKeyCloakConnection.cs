using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Abstractions.Connections
{
    public interface IKeyCloakConnection
    {
        public Task<bool> DeleteUserAsync(string userId);
    }
}
