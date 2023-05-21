using AccomodationGradingApplication.Abstractions.Messaging;
using AccomodationGradingDomain.Entities;
using AccomodationGradingDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationGradingApplication.Grading.Commands
{
    public sealed class CreateAccommodationGradingCommandHandler : ICommandHandler<CreateAccommodationGradingCommand, AccommodationGrading>
    {
        private readonly IAccommodationGradingRepository _repository;
        public CreateAccommodationGradingCommandHandler(IAccommodationGradingRepository repository)
        {
            _repository = repository;
        }

        public IAccommodationGradingRepository Get_repository()
        {
            return _repository;
        }

        public Task<AccommodationGrading> Handle(CreateAccommodationGradingCommand request, CancellationToken cancellationToken)
        {
            AccommodationGrading accommodationGrading = AccommodationGrading.Create(Guid.NewGuid(), request.createAccommodationGradingDTO.AccommodationName, request.createAccommodationGradingDTO.HostEmail, request.createAccommodationGradingDTO.GuestEmail, DateTime.Now, request.createAccommodationGradingDTO.Grade);
            return _repository.Create(accommodationGrading);
        }
    }
}
