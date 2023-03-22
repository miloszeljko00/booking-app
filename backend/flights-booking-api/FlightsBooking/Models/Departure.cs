using FlightsBooking.Support.ErrorHandler.Model;
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
            ValidateTime(time);
            Time = time;
            City = city;
        }
        private void ValidateTime(DateTime time)
        {
            if (time < DateTime.Now)
            {
                throw new InvalidDepartureTimeException();
            }
        }
    }
}