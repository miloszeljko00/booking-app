using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationDomain.Primitives
{
    public abstract class Entity : IEquatable<Entity>
    {
        [Key]
        public Guid Id { get; private init; }
        public static bool operator ==(Entity? first, Entity? second)
        {
            return first is not null && second is not null && first.Equals(second);
        }
        public static bool operator !=(Entity? first, Entity? second)
        {
            return first is not null && second is not null && first.Equals(second);
        }
        protected Entity(Guid id)
        {
            Id = id;
        }
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            if (obj is not Entity entity)
            {
                return false;
            }
            return entity.Id == Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() * 42;
        }

        public bool Equals(Entity? other)
        {
            if (other is null)
            {
                return false;
            }
            if (other.GetType() != GetType())
            {
                return false;
            }
            return other.Id == Id;
        }
    }
}
