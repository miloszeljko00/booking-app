using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Dtos;

public class Credentials
{
    public string Type;
    public string Value;
    public bool Temporary;

    public Credentials(string type, string value, bool temporary)
    {
        Type = type;
        Value = value;
        Temporary = temporary;
    }
}

