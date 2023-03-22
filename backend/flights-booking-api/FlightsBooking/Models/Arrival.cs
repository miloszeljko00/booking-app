using System;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using FlightsBooking.Support.ErrorHandler.Model;

namespace FlightsBooking.Models
{ 
    public class Arrival
    {
        [Required]
        public DateTime Time { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        public Arrival(DateTime time, string city)
        {
            ValidateTime(time);
            Time = time;
            City = city;
        }

        private void ValidateTime(DateTime time)
        {
            if(time < DateTime.Now)
            {
                throw new InvalidArrivalTimeException();
            }
        }
    }
}
