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
    public sealed class CreateAccommodationGradingCommandHandler : ICommandHandler<CreateAccommodationGradingCommand, AccommodationGrading>
    {
        private readonly IAccommodationGradingRepository _repository;

        private Channel channel;

        private AccommodationGradingNotificationGrpcService.AccommodationGradingNotificationGrpcServiceClient client;
        public CreateAccommodationGradingCommandHandler(IAccommodationGradingRepository repository)
        {
            _repository = repository;
        }

        public IAccommodationGradingRepository Get_repository()
        {
            return _repository;
        }

        public async Task<AccommodationGrading> Handle(CreateAccommodationGradingCommand request, CancellationToken cancellationToken)
        {
            AccommodationGrading accommodationGrading = AccommodationGrading.Create(Guid.NewGuid(), request.createAccommodationGradingDTO.AccommodationName, request.createAccommodationGradingDTO.HostEmail, request.createAccommodationGradingDTO.GuestEmail, DateTime.Now, request.createAccommodationGradingDTO.Grade);

            channel = new Channel("127.0.0.1:8791", ChannelCredentials.Insecure);
            client = new AccommodationGradingNotificationGrpcService.AccommodationGradingNotificationGrpcServiceClient(channel);
            MessageResponseProto5 response = await client.accommodationGradingAsync(new MessageProto5() { Email = accommodationGrading.HostEmail.EmailAddress, Accommodation = accommodationGrading.AccommodationName, Grade = accommodationGrading.Grade });
            Console.WriteLine(response);

            return _repository.Create(accommodationGrading).Result;
        }
    }
}
