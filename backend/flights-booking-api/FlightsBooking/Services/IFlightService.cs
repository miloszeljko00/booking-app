using FlightsBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsBooking.Services
{
    public interface IFlightService
    {
        public Task<List<Flight>> GetAsync();

        public Task<Flight?> GetAsync(Guid id);

        public Task CreateAsync(Flight newFlight);

        public Task UpdateAsync(Guid id, Flight updatedFlight);

        public  Task RemoveAsync(Guid id);
        
        public Task<List<Flight>> SearchAsync(string arrivalPlace, string departurePlace, string departureDate, decimal? availableTickets);
    }   
}
