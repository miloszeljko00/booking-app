using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Infrastructure.Persistence.Settings
{
    public class DatabaseSettings
    {
        public const string OptionName = "Neo4jDatabaseSettings";
        public string Uri { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
