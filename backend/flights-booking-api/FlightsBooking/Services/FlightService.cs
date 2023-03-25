using FlightsBooking.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsBooking.Services
{
    public class FlightService : IFlightService
    {
        private readonly IMongoCollection<Flight> _flightCollection;

        public FlightService()
        {
            var mongoClient = new MongoClient("mongodb://user:user@localhost:27017/?authSource=admin");

            var mongoDatabase = mongoClient.GetDatabase("BookingApp");

            _flightCollection = mongoDatabase.GetCollection<Flight>("Flights");
        }

        public async Task<List<Flight>> GetAsync() =>
        await _flightCollection.Find(_ => true).ToListAsync();

        public async Task<Flight?> GetAsync(Guid id) =>
            await _flightCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Flight newFlight) =>
            await _flightCollection.InsertOneAsync(newFlight);

        public async Task UpdateAsync(Guid id, Flight updatedFlight) =>
            await _flightCollection.ReplaceOneAsync(x => x.Id == id, updatedFlight);

        public async Task RemoveAsync(Guid id) =>
            await _flightCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<Flight>> SearchAsync(string arrivalPlace, string departurePlace, DateTime departureDate, int availableTickets)
        {
            if (arrivalPlace == null) arrivalPlace = "";
            if (departurePlace == null) departurePlace = "";
            if (availableTickets == null) availableTickets = 0;
            var flights = await _flightCollection.Find(_ => true).ToListAsync();
            List<Flight> result = new List<Flight>();
            foreach(Flight flight in flights)
            {
                if (!flight.Arrival.City.ToLower().Contains(arrivalPlace.ToLower()))
                    continue;
                if (!flight.Departure.City.ToLower().Contains(departurePlace.ToLower()))
                    continue;
                if (departureDate.Date!= default(DateTime) && !flight.Departure.Time.Date.Equals(departureDate.Date))
                    continue;
                if(flight.CalculateNumberOfAvailableTicketForTheFlight()<availableTickets) 
                    continue;
                result.Add(flight);
            }
            return result;
        }
        
    }
}
