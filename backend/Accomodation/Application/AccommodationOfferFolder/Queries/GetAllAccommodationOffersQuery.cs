using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Application.Abstractions.Messaging;

namespace Application.AccommodationOfferFolder.Queries
{
    public class GetAllAccommodationOffersQuery :IQuery<List<AccommodationOffer>>//najpravilnije bi bilo vratiti readonly listu
    {
    }
}
