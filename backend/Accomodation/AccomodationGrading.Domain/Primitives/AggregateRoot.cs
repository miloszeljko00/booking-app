﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationGradingDomain.Primitives
{
    public abstract class AggregateRoot : Entity
    {
        protected AggregateRoot(Guid id) : base(id) { }
    }
}
