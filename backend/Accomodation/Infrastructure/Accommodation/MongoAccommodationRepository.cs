﻿using AccomodationDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Accomodation.Infrastructure.Accommodation
{
    public class MongoAccommodationRepository : IAccommodationRepository
    {
        private readonly IMongoCollection<AccomodationDomain.Entities.Accommodation> _accommodationCollection;

        public MongoAccommodationRepository()
        {
            var mongoClient = new MongoClient("mongodb://user:user@localhost:27017/?authSource=admin");

            var mongoDatabase = mongoClient.GetDatabase("BookingApp");

            _accommodationCollection = mongoDatabase.GetCollection<AccomodationDomain.Entities.Accommodation>("Accommodation");
        }


        public async Task<AccomodationDomain.Entities.Accommodation> Create(AccomodationDomain.Entities.Accommodation accommodation)
        {
            await _accommodationCollection.InsertOneAsync(accommodation);
            return accommodation;
        }
        
        public async Task<IReadOnlyCollection<AccomodationDomain.Entities.Accommodation>> GetAllAsync()
        {
            return await _accommodationCollection.Find(_ => true).ToListAsync();
        }
    }
}