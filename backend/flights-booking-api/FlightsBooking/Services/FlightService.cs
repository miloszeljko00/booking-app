using FlightsBooking.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            var mongoClient = new MongoClient("mongodb+srv://svenadev:0mSCxqslvdQV46wi@booking-app.udfgsob.mongodb.net/");

            var mongoDatabase = mongoClient.GetDatabase("FlightsApp");

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

        public async Task<List<Flight>> SearchAsync(string arrivalPlace, string departurePlace, string departureDate, decimal? availableTickets)
        {
            if (arrivalPlace == null) arrivalPlace = "";
            if (departurePlace == null) departurePlace = "";
            if (availableTickets == null) availableTickets = 0;
            bool validDate = !(departureDate == null);
            var flights = await _flightCollection.Find(_ => true).ToListAsync();
            List<Flight> result = new List<Flight>();
            
            string format = "MM/dd/yyyy";
            

            foreach (Flight flight in flights)
            {
                if (!flight.Arrival.City.ToLower().Contains(arrivalPlace.ToLower()))
                    continue;
                if (!flight.Departure.City.ToLower().Contains(departurePlace.ToLower()))
                    continue;
                if (validDate)
                    try
                    {
                        if (!flight.Departure.Time.Date.Equals(DateTime.ParseExact(departureDate, format, CultureInfo.InvariantCulture)))
                        continue;
                    }
                    catch (FormatException e)
                    { break; }                      
                   
                if(flight.CalculateNumberOfAvailableTicketForTheFlight()<availableTickets) 
                    continue;
                result.Add(flight);
            }
            return result;
        }
        public async Task<List<Flight>> getUserFlights(string userId)
        {
            var flights = await _flightCollection.Find(_ => true).ToListAsync();
            List<Flight> userFlights = new List<Flight>();
            foreach (var flight in flights)
            {
                foreach(var soldTicket in flight.SoldTickets)
                {
                    if (soldTicket.UserId.Equals(userId))
                    {
                        userFlights.Add(flight);
                        break;
                    }
                }
            }
            return userFlights;
       }

    }
}
