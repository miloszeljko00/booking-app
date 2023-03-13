using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FlightsBooking.Models
{ 
    public class UserFlight
    {
        [Required]
        public Guid FlightId { get; set; }
        [Required]
        public Departure Departure { get; set; }
        [Required]
        public Arrival Arrival { get; set; }
        [Required]
        public bool Passed { get; set; }
        [Required]
        public bool Canceled { get; set; }

        public UserFlight(Guid flightId, Departure departure, Arrival arrival, bool passed, bool canceled)
        {
            FlightId = flightId;
            Departure = departure;
            Arrival = arrival;
            Passed = passed;
            Canceled = canceled;
        }
    }
}
