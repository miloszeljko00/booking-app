﻿using AccomodationDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationDomain.Interfaces
{
    public interface IAccommodationRepository
    {
        Task<IReadOnlyCollection<Accommodation>> GetAllAsync();
        Task<Accommodation> Create(Accommodation accommodation);
    }
}
