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
    public sealed class UpdateAccommodationGradingCommandHandler : ICommandHandler<UpdateAccommodationGradingCommand, AccommodationGrading>
    {
        private readonly IAccommodationGradingRepository _repository;
        public UpdateAccommodationGradingCommandHandler(IAccommodationGradingRepository repository)
        {
            _repository = repository;
        }

        public IAccommodationGradingRepository Get_repository()
        {
            return _repository;
        }

        public Task<AccommodationGrading> Handle(UpdateAccommodationGradingCommand request, CancellationToken cancellationToken)
        {
            AccommodationGrading ag = _repository.GetAsync(request.updateAccommodationGradingDTO.Id).Result;
            if(ag is null)
            {
                throw new Exception("Grade with this id does not exist");
            }
            AccommodationGrading accommodationGrading = AccommodationGrading.Create(ag.Id, ag.AccommodationName, ag.HostEmail.EmailAddress, ag.GuestEmail.EmailAddress, DateTime.Now, request.updateAccommodationGradingDTO.Grade);
            _repository.UpdateAsync(ag.Id, accommodationGrading);
            return Task.FromResult(accommodationGrading);
        }
    }
}
