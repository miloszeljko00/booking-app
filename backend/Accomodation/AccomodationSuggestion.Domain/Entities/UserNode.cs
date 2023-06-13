using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Domain.Entities
{
    public class UserNode
    {
        public string Email { get; set; }
        public UserNode(string email)
        {
            Email = email;
        }
        
    }
}
