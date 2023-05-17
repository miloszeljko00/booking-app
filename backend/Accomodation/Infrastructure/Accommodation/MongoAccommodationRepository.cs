using AccomodationSuggestionDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Accomodation.Infrastructure.Persistance.Settings;

namespace Accomodation.Infrastructure.Accommodation
{
    public class MongoAccommodationRepository : IAccommodationRepository
    {
        private readonly IMongoCollection<AccomodationSuggestionDomain.Entities.Accommodation> _accommodationCollection;

        public MongoAccommodationRepository(IOptions<DatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _accommodationCollection = mongoDatabase.GetCollection<AccomodationSuggestionDomain.Entities.Accommodation>(dbSettings.Value.AccommodationCollectionName);
        }


        public async Task<AccomodationSuggestionDomain.Entities.Accommodation> Create(AccomodationSuggestionDomain.Entities.Accommodation accommodation)
        {
            await _accommodationCollection.InsertOneAsync(accommodation);
            return accommodation;
        }
        
        public async Task<ICollection<AccomodationSuggestionDomain.Entities.Accommodation>> GetAllAsync()
        {
            return await _accommodationCollection.Find(_ => true).ToListAsync();
        }

        public async Task<AccomodationSuggestionDomain.Entities.Accommodation> GetAsync(Guid id) =>
            await _accommodationCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(Guid id, AccomodationSuggestionDomain.Entities.Accommodation updatedAccommodation) =>
            await _accommodationCollection.ReplaceOneAsync(x => x.Id == id, updatedAccommodation);

        public async Task RemoveAsync(Guid id) =>
            await _accommodationCollection.DeleteOneAsync(x => x.Id == id);
    }
}
