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
    public sealed class DeleteHostGradingCommandHandler : ICommandHandler<DeleteHostGradingCommand, HostGrading>
    {
        private readonly IHostGradingRepository _repository;
        public DeleteHostGradingCommandHandler(IHostGradingRepository repository)
        {
            _repository = repository;
        }

        public IHostGradingRepository Get_repository()
        {
            return _repository;
        }

        public Task<HostGrading> Handle(DeleteHostGradingCommand request, CancellationToken cancellationToken)
        {
            HostGrading hg = _repository.GetAsync(request.hostGradingId).Result;
            if(hg is null)
            {
                throw new Exception("Grade with this id does not exist");
            }
            _repository.RemoveAsync(hg.Id);
            return Task.FromResult(hg);
        }
    }
}
