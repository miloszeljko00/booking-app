using System.ComponentModel.DataAnnotations;

namespace FlightsBooking.Models
{ 
    public class FlightTicket
    {
        [Required]
        public Guid FlightTicketId { get; set; }
        [Required]
        public DateTime Purchased { get; set; }
        [Required]
        public double Price { get; set; }

        public FlightTicket(Guid flightTicketId, DateTime purchased, double price)
        {
            //TODO: add validation for Purchased, Price
            FlightTicketId = flightTicketId;
            Purchased = purchased;
            Price = price;
        }
    }
}
