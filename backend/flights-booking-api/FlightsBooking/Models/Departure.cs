using System.ComponentModel.DataAnnotations;

namespace FlightsBooking.Models
{
    public class Departure
    {
        [Required]
        public DateTime Time { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        public Departure(DateTime time, string city)
        {
            Time = time;
            City = city;
        }

    }
}