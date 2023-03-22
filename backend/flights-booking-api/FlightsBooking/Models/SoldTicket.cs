using FlightsBooking.Support.ErrorHandler.Model;
using System.ComponentModel.DataAnnotations;

namespace FlightsBooking.Models
{ 
    public class SoldTicket
    {
        [Key]
        public Guid FlightTicketId { get; set; }
        [Required]
        public DateTime Purchased { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public double Price { get; set; }

        public SoldTicket(Guid flightTicketId, DateTime purchased, Guid userId, double price)
        {
            ValidateTime(Purchased);
            ValidatePrice(price);
            FlightTicketId = flightTicketId;
            Purchased = purchased;
            UserId = userId;
            Price = price;
        }
        private void ValidateTime(DateTime purchased)
        {
            if (purchased > DateTime.Now)
            {
                throw new InvalidPurchasedDateException();
            }
        }
        private void ValidatePrice(double price)
        {
            if(price < 0)
            {
                throw new InvalidPriceException();
            }
        }

    }
}
