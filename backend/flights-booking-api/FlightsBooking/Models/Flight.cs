using FlightsBooking.Support.ErrorHandler.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FlightsBooking.Models
{ 
    public class Flight
    {
        [Key]
        [BsonId]
        public Guid Id { get; set; }

        [Required]
        public Departure Departure { get; set; }
        [Required]
        public Arrival Arrival { get; set; }
        [Required]
        public int TotalTickets { get; set; }
        [Required]
        public double TicketPrice { get; set; }
        [Required]
        public List<SoldTicket> SoldTickets { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        public Flight(Departure departure, Arrival arrival, int totalTickets, double ticketPrice, List<SoldTicket> soldTickets)
        {
            ValidateTicketPrice(ticketPrice);
            ValidateTotalTicketsCount(totalTickets);
            CheckIfDepartureIsBeforeArrival(departure, arrival);
            Departure = departure;
            Arrival = arrival;
            TotalTickets = totalTickets;
            TicketPrice = ticketPrice;
            SoldTickets = soldTickets;
            IsDeleted = false;
        }

        private void CheckIfDepartureIsBeforeArrival(Departure departure, Arrival arrival)
        {
            if(departure.Time > arrival.Time)
            {
                throw new DepartureIsAfterArrivalException();
            }
        }

        private void ValidateTotalTicketsCount(int totalTickets)
        {
            if (totalTickets < 0)
                throw new InvalidTotalTicketsCountException();
        }

        private void ValidateTicketPrice(double ticketPrice)
        {
            if (ticketPrice < 0)
                throw new InvalidPriceException();
        }

        public bool IsFlightPassed()
        {
            return this.Departure.Time < DateTime.UtcNow;
        }
        public double CalculateTotalPriceForFlight()
        {
            double totalPrice = 0;
            foreach (var soldTicket in this.SoldTickets)
            {
                totalPrice += soldTicket.Price;
            }
            return totalPrice;
        }
        public int CalculateNumberOfAvailableTicketForTheFlight()
        {
            return this.TotalTickets - this.SoldTickets.Count();
        }
    }
}
