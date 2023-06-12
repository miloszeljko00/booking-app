using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Dtos
{
    public class UserApiKeyDto
    {
        public string UserId { get; set; } = "";
        public string ApiKey { get; set; } = "";
    }
}
