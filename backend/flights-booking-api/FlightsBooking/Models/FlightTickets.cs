using System.ComponentModel.DataAnnotations;

namespace FlightsBooking.Models
{ 
    public class FlightTickets
    {
        [Required]
        public int TotalTickets { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public List<FlightTicket> SoldTickets { get; set; }
        [Required]
        public int AvailableTickets { get; set; }
        [Required]
        public double TotalPrice { get; set; }

        public FlightTickets(int totalTickets, double price, List<FlightTicket> soldTickets, int availableTickets, double totalPrice)
        {
            //TODO add all required validations
            TotalTickets = totalTickets;
            Price = price;
            SoldTickets = soldTickets;
            AvailableTickets = availableTickets;
            TotalPrice = totalPrice;
        }

    }
}
