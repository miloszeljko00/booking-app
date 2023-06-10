using AccommodationGradingApplication.Grading.Support.Grpc.Protos;
using AccomodationGradingApplication.Abstractions.Messaging;
using AccomodationGradingDomain.Entities;
using AccomodationGradingDomain.Interfaces;
using Grpc.Core;
using Rs.Ac.Uns.Ftn.Grpc;
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

        private Channel channel;

        private HostGradingNotificationGrpcService.HostGradingNotificationGrpcServiceClient client;
        public UpdateHostGradingCommandHandler(IHostGradingRepository repository)
        {
            _repository = repository;
        }

        public IHostGradingRepository Get_repository()
        {
            return _repository;
        }

        public async Task<HostGrading> Handle(UpdateHostGradingCommand request, CancellationToken cancellationToken)
        {
            HostGrading hg = _repository.GetAsync(request.updateHostGradingDTO.Id).Result;
            if(hg is null)
            {
                throw new Exception("Grade with this id does not exist");
            }
            HostGrading hostGrading = HostGrading.Create(hg.Id, hg.HostEmail.EmailAddress, hg.GuestEmail.EmailAddress, DateTime.Now, request.updateHostGradingDTO.Grade);

            channel = new Channel("127.0.0.1:8790", ChannelCredentials.Insecure);
            client = new HostGradingNotificationGrpcService.HostGradingNotificationGrpcServiceClient(channel);
            MessageResponseProto4 response = await client.hostGradingAsync(new MessageProto4() { Email = hostGrading.HostEmail.EmailAddress, Grade = hostGrading.Grade });
            Console.WriteLine(response);

            _repository.UpdateAsync(hg.Id, hostGrading);
            return hostGrading;
        }
    }
}
