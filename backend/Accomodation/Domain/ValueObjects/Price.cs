using AccomodationDomain.Exceptions.CustomExceptions;
using AccomodationDomain.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationDomain.ValueObjects
{
    public class Price : ValueObject
    {
        public double Value { get; set; }

        public Price() { } //obrisati kad se kreiraju dtovi
        private Price(double value)
        {
            Value = value;
        }
        public static Price Create(double value)
        {
            if (CheckIfPriceIsValid(value))
            {
                return new Price(value);
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
