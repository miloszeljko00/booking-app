
using System.ComponentModel.DataAnnotations;

namespace FlightsBooking.Models
{ 
    
    public class UserFlightTicket
    {
        [Required]
        public Guid FlightTicketId { get; set; }

        
        [Required]
        public DateTime Purchased { get; set; }

       
        [Required]
        public double Price { get; set; }

        
        [Required]
        public UserFlight Flight { get; set; }

        public UserFlightTicket(Guid filghtTicketId, DateTime purchased, double price, UserFlight flight)
        {
            FlightTicketId= filghtTicketId;
            Purchased= purchased;
            Price= price;
            Flight= flight;
        }

       
    }
}
