using FlightsBooking.Grpc.Protos;
using FlightsBooking.Models;
using FlightsBooking.Services;
using Grpc.Core;
using Rs.Ac.Uns.Ftn.Grpc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FlightsBooking.Grpc
{
    public class BookFlightServerGrpcServiceImpl : BookFlightGrpcService.BookFlightGrpcServiceBase
    {
        IFlightService _flightService;
        IUserService _userService;
        public BookFlightServerGrpcServiceImpl(IFlightService flightService, IUserService userService)
        {
            _flightService = flightService;
            _userService = userService;
        }

        public override async Task<BookFlightMessageProtoResponse> bookFlight(BookFlightMessageProto request, ServerCallContext context)
        {
            try
            {
                var user = await _userService.GetByKeyAsync(request.ApiKey);
                if (user == null)
                {
                    return new BookFlightMessageProtoResponse()
                    {
                        IsBooked = true.ToString()
                    };
                }
                Flight flight = await _flightService.GetAsync(new Guid(request.FlightId));
                List<UserFlightTicket> userFlightTickets = new List<UserFlightTicket>();
                for (int i = 0; i < int.Parse(request.NumberOfTickets); i++)
                {
                    SoldTicket soldTicket = new SoldTicket(Guid.NewGuid(), DateTime.Now, user.Email, flight.TicketPrice);
                    userFlightTickets.Add(new UserFlightTicket
                    {
                        FlightTicketId = soldTicket.Id,
                        Purchased = soldTicket.Purchased,
                        Price = soldTicket.Price,
                        Flight = new UserFlight
                        {
                            FlightId = flight.Id,
                            Arrival = new Arrival { City = flight.Arrival.City, Time = flight.Arrival.Time },
                            Departure = new Departure { City = flight.Departure.City, Time = flight.Departure.Time },
                            Passed = flight.IsFlightPassed(),
                            Canceled = flight.IsDeleted
                        }
                    });
                    flight.SoldTickets.Add(soldTicket);
                }
                await _flightService.UpdateAsync(new Guid(request.FlightId), flight);
                return new BookFlightMessageProtoResponse()
                {
                    IsBooked = true.ToString()
                };
            }
            catch (Exception e)
            {
                return new BookFlightMessageProtoResponse()
                {
                    IsBooked = false.ToString()
                };
            }
        }
    }
    [DataContract]
    public partial class UserFlightTicket : IEquatable<UserFlightTicket>
    {
        /// <summary>
        /// Identifikator karte leta.
        /// </summary>
        /// <value>Identifikator karte leta.</value>
        [Required]
        [DataMember(Name = "flightTicketId", EmitDefaultValue = false)]
        public Guid FlightTicketId { get; set; }

        /// <summary>
        /// Trenutak kada je karta kupljena.
        /// </summary>
        /// <value>Trenutak kada je karta kupljena.</value>
        [Required]
        [DataMember(Name = "purchased", EmitDefaultValue = false)]
        public DateTime Purchased { get; set; }

        /// <summary>
        /// Cena karte u trenutku kupovine.
        /// </summary>
        /// <value>Cena karte u trenutku kupovine.</value>
        [Required]
        [DataMember(Name = "price", EmitDefaultValue = true)]
        public double Price { get; set; }

        /// <summary>
        /// Gets or Sets Flight
        /// </summary>
        [Required]
        [DataMember(Name = "flight", EmitDefaultValue = false)]
        public UserFlight Flight { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UserFlightTicket {\n");
            sb.Append("  FlightTicketId: ").Append(FlightTicketId).Append("\n");
            sb.Append("  Purchased: ").Append(Purchased).Append("\n");
            sb.Append("  Price: ").Append(Price).Append("\n");
            sb.Append("  Flight: ").Append(Flight).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((UserFlightTicket)obj);
        }

        /// <summary>
        /// Returns true if UserFlightTicket instances are equal
        /// </summary>
        /// <param name="other">Instance of UserFlightTicket to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserFlightTicket other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    FlightTicketId == other.FlightTicketId ||
                    FlightTicketId != null &&
                    FlightTicketId.Equals(other.FlightTicketId)
                ) &&
                (
                    Purchased == other.Purchased ||
                    Purchased != null &&
                    Purchased.Equals(other.Purchased)
                ) &&
                (
                    Price == other.Price ||

                    Price.Equals(other.Price)
                ) &&
                (
                    Flight == other.Flight ||
                    Flight != null &&
                    Flight.Equals(other.Flight)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                if (FlightTicketId != null)
                    hashCode = hashCode * 59 + FlightTicketId.GetHashCode();
                if (Purchased != null)
                    hashCode = hashCode * 59 + Purchased.GetHashCode();

                hashCode = hashCode * 59 + Price.GetHashCode();
                if (Flight != null)
                    hashCode = hashCode * 59 + Flight.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(UserFlightTicket left, UserFlightTicket right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserFlightTicket left, UserFlightTicket right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }
    [DataContract]
    public partial class UserFlight : IEquatable<UserFlight>
    {
        /// <summary>
        /// Jedinstveni identifikator leta.
        /// </summary>
        /// <value>Jedinstveni identifikator leta.</value>
        [Required]
        [DataMember(Name = "flightId", EmitDefaultValue = false)]
        public Guid FlightId { get; set; }

        /// <summary>
        /// Gets or Sets Departure
        /// </summary>
        [Required]
        [DataMember(Name = "departure", EmitDefaultValue = false)]
        public Departure Departure { get; set; }

        /// <summary>
        /// Gets or Sets Arrival
        /// </summary>
        [Required]
        [DataMember(Name = "arrival", EmitDefaultValue = false)]
        public Arrival Arrival { get; set; }

        /// <summary>
        /// Podatak o tome da li je proslo vreme polaska.
        /// </summary>
        /// <value>Podatak o tome da li je proslo vreme polaska.</value>
        [Required]
        [DataMember(Name = "passed", EmitDefaultValue = true)]
        public bool Passed { get; set; }

        /// <summary>
        /// Podatak o tome da li je let otkazan.
        /// </summary>
        /// <value>Podatak o tome da li je let otkazan.</value>
        [Required]
        [DataMember(Name = "canceled", EmitDefaultValue = true)]
        public bool Canceled { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UserFlight {\n");
            sb.Append("  FlightId: ").Append(FlightId).Append("\n");
            sb.Append("  Departure: ").Append(Departure).Append("\n");
            sb.Append("  Arrival: ").Append(Arrival).Append("\n");
            sb.Append("  Passed: ").Append(Passed).Append("\n");
            sb.Append("  Canceled: ").Append(Canceled).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((UserFlight)obj);
        }

        /// <summary>
        /// Returns true if UserFlight instances are equal
        /// </summary>
        /// <param name="other">Instance of UserFlight to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserFlight other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    FlightId == other.FlightId ||
                    FlightId != null &&
                    FlightId.Equals(other.FlightId)
                ) &&
                (
                    Departure == other.Departure ||
                    Departure != null &&
                    Departure.Equals(other.Departure)
                ) &&
                (
                    Arrival == other.Arrival ||
                    Arrival != null &&
                    Arrival.Equals(other.Arrival)
                ) &&
                (
                    Passed == other.Passed ||

                    Passed.Equals(other.Passed)
                ) &&
                (
                    Canceled == other.Canceled ||

                    Canceled.Equals(other.Canceled)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                if (FlightId != null)
                    hashCode = hashCode * 59 + FlightId.GetHashCode();
                if (Departure != null)
                    hashCode = hashCode * 59 + Departure.GetHashCode();
                if (Arrival != null)
                    hashCode = hashCode * 59 + Arrival.GetHashCode();

                hashCode = hashCode * 59 + Passed.GetHashCode();

                hashCode = hashCode * 59 + Canceled.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(UserFlight left, UserFlight right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserFlight left, UserFlight right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }
    [DataContract]
    public partial class Arrival : IEquatable<Arrival>
    {
        /// <summary>
        /// Vreme i datum dolaska.
        /// </summary>
        /// <value>Vreme i datum dolaska.</value>
        [Required]
        [DataMember(Name = "time", EmitDefaultValue = false)]
        public DateTime Time { get; set; }

        /// <summary>
        /// Mesto dolaska.
        /// </summary>
        /// <value>Mesto dolaska.</value>
        [Required]
        [DataMember(Name = "city", EmitDefaultValue = false)]
        public string City { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Arrival {\n");
            sb.Append("  Time: ").Append(Time).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Arrival)obj);
        }

        /// <summary>
        /// Returns true if Arrival instances are equal
        /// </summary>
        /// <param name="other">Instance of Arrival to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Arrival other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    Time == other.Time ||
                    Time != null &&
                    Time.Equals(other.Time)
                ) &&
                (
                    City == other.City ||
                    City != null &&
                    City.Equals(other.City)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                if (Time != null)
                    hashCode = hashCode * 59 + Time.GetHashCode();
                if (City != null)
                    hashCode = hashCode * 59 + City.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(Arrival left, Arrival right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Arrival left, Arrival right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }
    [DataContract]
    public partial class Departure : IEquatable<Departure>
    {
        /// <summary>
        /// Vreme i datum polaska.
        /// </summary>
        /// <value>Vreme i datum polaska.</value>
        [Required]
        [DataMember(Name = "time", EmitDefaultValue = false)]
        public DateTime Time { get; set; }

        /// <summary>
        /// Mesto polaska.
        /// </summary>
        /// <value>Mesto polaska.</value>
        [Required]
        [DataMember(Name = "city", EmitDefaultValue = false)]
        public string City { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Departure {\n");
            sb.Append("  Time: ").Append(Time).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Departure)obj);
        }

        /// <summary>
        /// Returns true if Departure instances are equal
        /// </summary>
        /// <param name="other">Instance of Departure to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Departure other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    Time == other.Time ||
                    Time != null &&
                    Time.Equals(other.Time)
                ) &&
                (
                    City == other.City ||
                    City != null &&
                    City.Equals(other.City)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                if (Time != null)
                    hashCode = hashCode * 59 + Time.GetHashCode();
                if (City != null)
                    hashCode = hashCode * 59 + City.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(Departure left, Departure right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Departure left, Departure right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }
}
