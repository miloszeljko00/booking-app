using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccomodationGrading.Infrastructure.Persistance.Settings;
using AccomodationGradingDomain.Entities;
using AccomodationGradingDomain.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AccomodationGrading.Infrastructure.Grading
{
    public class MongoAccommodationGradingRepository : IAccommodationGradingRepository
    {
        private readonly IMongoCollection<AccommodationGrading> _gradeCollection;

        public MongoAccommodationGradingRepository(IOptions<DatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _gradeCollection = mongoDatabase.GetCollection<AccommodationGrading>(dbSettings.Value.AccommodationGradingCollectionName);
        }


        public async Task<AccommodationGrading> Create(AccommodationGrading grade)
        {
            await _gradeCollection.InsertOneAsync(grade);
            return grade;
        }
        
        public async Task<ICollection<AccommodationGrading>> GetAllAsync()
        {
            return await _gradeCollection.Find(_ => true).ToListAsync();
        }

        public async Task<AccommodationGrading> GetAsync(Guid id) =>
            await _gradeCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(Guid id, AccommodationGrading updatedAccommodationGrading) =>
            await _gradeCollection.ReplaceOneAsync(x => x.Id == id, updatedAccommodationGrading);

        public async Task RemoveAsync(Guid id) =>
            await _gradeCollection.DeleteOneAsync(x => x.Id == id);
    }
}
