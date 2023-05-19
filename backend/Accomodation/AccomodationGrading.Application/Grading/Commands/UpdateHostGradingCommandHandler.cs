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
    public sealed class UpdateHostGradingCommandHandler : ICommandHandler<UpdateHostGradingCommand, HostGrading>
    {
        private readonly IHostGradingRepository _repository;
        public UpdateHostGradingCommandHandler(IHostGradingRepository repository)
        {
            _repository = repository;
        }

        public IHostGradingRepository Get_repository()
        {
            return _repository;
        }

        public Task<HostGrading> Handle(UpdateHostGradingCommand request, CancellationToken cancellationToken)
        {
            HostGrading hg = _repository.GetAsync(request.updateHostGradingDTO.Id).Result;
            if(hg is null)
            {
                throw new Exception("Grade with this id does not exist");
            }
            HostGrading hostGrading = HostGrading.Create(hg.Id, hg.HostEmail.EmailAddress, hg.GuestEmail.EmailAddress, DateTime.Now, request.updateHostGradingDTO.Grade);
            _repository.UpdateAsync(hg.Id, hostGrading);
            return Task.FromResult(hostGrading);
        }
    }
}
