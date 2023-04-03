using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AccommodationOfferFolder.Queries
{
    public sealed class GetAllAccommodationOffersQueryHandler : IQueryHandler<GetAllAccommodationOffersQuery, List<AccommodationOffer>>
    {
        private readonly IAccommodationOfferRepository _repository;

        public GetAllAccommodationOffersQueryHandler(IAccommodationOfferRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AccommodationOffer>> Handle(GetAllAccommodationOffersQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the list of accommodation offers from the data source
            Console.WriteLine("Usao sam u Handle metodu, sada cekam 10 sekundi");
            Thread.Sleep(10000);
            Console.WriteLine("Sada proveravam da li je doslo do otkazivanja zahteva");
            cancellationToken.ThrowIfCancellationRequested();
            Console.WriteLine("Nije doslo do otkazivanja, pokusavam da dobavim podatke...");
            return await _repository.GetAllAsync();
        }
    }
}
