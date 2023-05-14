using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.ValueObjects;
using UserManagementDomain.Primitives;

namespace UserManagement.Domain.Entities
{
    public class User : Entity
    {
        public User(Guid id, string email, string name, string surname, Address address, string role) : base(id)
        {
            Email = email;
            Name = name;
            Surname = surname;
            Address = address;
            Role = role;
        }

        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Address Address { get; set; }
    }
}
