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
    public sealed class CreateHostGradingCommandHandler : ICommandHandler<CreateHostGradingCommand, HostGrading>
    {
        private readonly IHostGradingRepository _repository;

        private Channel channel;

        private HostGradingNotificationGrpcService.HostGradingNotificationGrpcServiceClient client;
        public CreateHostGradingCommandHandler(IHostGradingRepository repository)
        {
            _repository = repository;
        }

        public IHostGradingRepository Get_repository()
        {
            return _repository;
        }

        public async Task<HostGrading> Handle(CreateHostGradingCommand request, CancellationToken cancellationToken)
        {
            HostGrading hostGrading = HostGrading.Create(Guid.NewGuid(), request.createHostGradingDTO.HostEmail, request.createHostGradingDTO.GuestEmail, DateTime.Now, request.createHostGradingDTO.Grade);

            channel = new Channel("127.0.0.1:8790", ChannelCredentials.Insecure);
            client = new HostGradingNotificationGrpcService.HostGradingNotificationGrpcServiceClient(channel);
            MessageResponseProto4 response = await client.hostGradingAsync(new MessageProto4() { Email = hostGrading.HostEmail.EmailAddress, Grade = hostGrading.Grade });
            Console.WriteLine(response);

            return _repository.Create(hostGrading).Result;
        }
    }
}
