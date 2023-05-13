using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Exceptions.CustomExceptions;
using UserManagementDomain.Primitives;

namespace UserManagement.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        [MaxLength(100)]
        [Required]
        public string Country { get; init; }
        [MaxLength(100)]
        [Required]
        public string City { get; init; }
        [MaxLength(100)]
        [Required]
        public string Street { get; init; }
        [MaxLength(100)]
        [Required]
        public string Number { get; init; }

        private Address(string country, string city, string street, string number)
        {
            Country = country;
            City = city;
            Street = street;
            Number = number;
        }

        public static Address Create(string country, string city, string street, string number)
        {
            var address = new Address(country, city, street, number);
            var validationResult = address.CheckIfPropsAreValid(address);
            if (validationResult.IsValid)
            {
                return address;
            }
            else
            {
                throw new InvalidAddressException();
            }
        }

        private FluentValidation.Results.ValidationResult CheckIfPropsAreValid(Address address)
        {
            var addressValidator = new AddressValidator();
            return addressValidator.Validate(address);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Country;
            yield return City;
            yield return Street;
            yield return Number;
        }
    }
    internal class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
            RuleFor(x => x.City).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Street).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Number).NotEmpty().MaximumLength(100);
        }
    }
}
