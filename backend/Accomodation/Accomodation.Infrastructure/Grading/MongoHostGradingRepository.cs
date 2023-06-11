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
    public class MongoHostGradingRepository : IHostGradingRepository
    {
        private readonly IMongoCollection<HostGrading> _gradeCollection;

        public MongoHostGradingRepository(IOptions<DatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _gradeCollection = mongoDatabase.GetCollection<HostGrading>(dbSettings.Value.HostGradingCollectionName);
        }


        public async Task<HostGrading> Create(HostGrading grade)
        {
            await _gradeCollection.InsertOneAsync(grade);
            return grade;
        }
        
        public async Task<ICollection<HostGrading>> GetAllAsync()
        {
            return await _gradeCollection.Find(_ => true).ToListAsync();
        }

        public async Task<HostGrading> GetAsync(Guid id) =>
            await _gradeCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(Guid id, HostGrading updatedHostGrading) =>
            await _gradeCollection.ReplaceOneAsync(x => x.Id == id, updatedHostGrading);

        public async Task RemoveAsync(Guid id) =>
            await _gradeCollection.DeleteOneAsync(x => x.Id == id);
    }
}
