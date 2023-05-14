using Accomodation.Domain.Primitives.Enums;
using AccomodationSuggestionDomain.Exceptions.CustomExceptions;
using AccomodationSuggestionDomain.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestionDomain.ValueObjects
{
    public class Price : ValueObject
    {
        public double Value { get; set; }
        public DateRange DateRange { get; set; }
        public Price() { }
        private Price(double value, DateRange dateRange)
        {
            DateRange = dateRange;
            Value = value;
        }
        public static Price Create(double value, DateRange dateRange)
        {
            if (CheckIfPriceIsValid(value))
            {
                return new Price(value, dateRange);
            }
            else
            {
                throw new InvalidPriceException();
            }
        }

        private static bool CheckIfPriceIsValid(double value)
        {
            if(value < 0) { return false; }
            else return true;
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
