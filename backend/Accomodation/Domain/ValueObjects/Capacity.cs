using Domain.Exceptions.CustomExceptions;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class Capacity : ValueObject
    {
        public int Max { get; init; }
        public int Min { get; init; }

        public Capacity() { } //obrisati kad se kreiraju dtovi
        private Capacity(int max, int min)
        {
            Max = max;
            Min = min;
        }
        public static Capacity Create(int max, int min)
        {
            if (CheckIfValuesAreValid(max,min))
            {
                return new Capacity(max, min);
            }
            else
            {
                throw new InvalidCapacityException();
            }
        }

        private static bool CheckIfValuesAreValid(int max, int min)
        {
            if(max < min || max <=0 || min <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Max;
            yield return Min;
        }
    }
}
