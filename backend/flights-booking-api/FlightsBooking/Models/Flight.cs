using System.ComponentModel.DataAnnotations;

namespace FlightsBooking.Models
{ 
    public class Flight
    {
        [Required]
        public Guid FlightId { get; set; }
        [Required]
        public FlightTickets FlightTickets { get; set; }

        [Required]
        public Departure Departure { get; set; }
        [Required]
        public Arrival Arrival { get; set; }

        [Required]
        public bool Passed { get; set; }

        public Flight(Guid flightId, FlightTickets flightTickets, Departure departure, Arrival arrival, bool passed)
        {
            FlightId = flightId;
            FlightTickets = flightTickets;
            Departure = departure;
            Arrival = arrival;
            Passed = passed;
        }
    }
}
