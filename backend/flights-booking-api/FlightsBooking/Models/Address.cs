using System.ComponentModel.DataAnnotations;

namespace FlightsBooking.Models
{ 
    public class Address 
    {
        [Required]
        [MaxLength(300)]
        public string _Address { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        public Address(string address, string city, string country)
        {
            _Address = address;
            City = city;
            Country = country;
        }
        
    }
}
