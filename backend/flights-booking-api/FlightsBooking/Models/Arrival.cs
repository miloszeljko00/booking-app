using System;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

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
            //TODO: create validation for DateTime
            Time = time;
            City = city;
        }

    }
}
