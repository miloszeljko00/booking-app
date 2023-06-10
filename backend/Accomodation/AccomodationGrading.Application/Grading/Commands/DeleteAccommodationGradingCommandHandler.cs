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
    public sealed class DeleteAccommodationGradingCommandHandler : ICommandHandler<DeleteAccommodationGradingCommand, AccommodationGrading>
    {
        private readonly IAccommodationGradingRepository _repository;
        public DeleteAccommodationGradingCommandHandler(IAccommodationGradingRepository repository)
        {
            _repository = repository;
        }

        public IAccommodationGradingRepository Get_repository()
        {
            return _repository;
        }

        public Task<AccommodationGrading> Handle(DeleteAccommodationGradingCommand request, CancellationToken cancellationToken)
        {
            AccommodationGrading ag = _repository.GetAsync(request.accommodationGradingId).Result;
            if(ag is null)
            {
                throw new Exception("Grade with this id does not exist");
            }
            _repository.RemoveAsync(ag.Id);
            return Task.FromResult(ag);
        }
    }
}
