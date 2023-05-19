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
    public sealed class CreateHostGradingCommandHandler : ICommandHandler<CreateHostGradingCommand, HostGrading>
    {
        private readonly IHostGradingRepository _repository;
        public CreateHostGradingCommandHandler(IHostGradingRepository repository)
        {
            _repository = repository;
        }

        public IHostGradingRepository Get_repository()
        {
            return _repository;
        }

        public Task<HostGrading> Handle(CreateHostGradingCommand request, CancellationToken cancellationToken)
        {
            HostGrading hostGrading = HostGrading.Create(Guid.NewGuid(), request.createHostGradingDTO.HostEmail, request.createHostGradingDTO.GuestEmail, DateTime.Now, request.createHostGradingDTO.Grade);
            return _repository.Create(hostGrading);
        }
    }
}
