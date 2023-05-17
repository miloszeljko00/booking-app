using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Infrastructure.Persistance.Settings
{
    public class DatabaseSettings
    {
        public const string OptionName = "MongoDatabaseSettings";
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string AccommodationCollectionName { get; set; } = null!;
    }
}
