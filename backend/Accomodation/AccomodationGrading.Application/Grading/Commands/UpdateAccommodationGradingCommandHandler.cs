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
    public sealed class UpdateAccommodationGradingCommandHandler : ICommandHandler<UpdateAccommodationGradingCommand, AccommodationGrading>
    {
        private readonly IAccommodationGradingRepository _repository;

        private Channel channel;

        private AccommodationGradingNotificationGrpcService.AccommodationGradingNotificationGrpcServiceClient client;
        public UpdateAccommodationGradingCommandHandler(IAccommodationGradingRepository repository)
        {
            _repository = repository;
        }

        public IAccommodationGradingRepository Get_repository()
        {
            return _repository;
        }

        public async Task<AccommodationGrading> Handle(UpdateAccommodationGradingCommand request, CancellationToken cancellationToken)
        {
            AccommodationGrading ag = _repository.GetAsync(request.updateAccommodationGradingDTO.Id).Result;
            if(ag is null)
            {
                throw new Exception("Grade with this id does not exist");
            }
            AccommodationGrading accommodationGrading = AccommodationGrading.Create(ag.Id, ag.AccommodationName, ag.HostEmail.EmailAddress, ag.GuestEmail.EmailAddress, DateTime.Now, request.updateAccommodationGradingDTO.Grade);

            channel = new Channel("127.0.0.1:8791", ChannelCredentials.Insecure);
            client = new AccommodationGradingNotificationGrpcService.AccommodationGradingNotificationGrpcServiceClient(channel);
            MessageResponseProto5 response = await client.accommodationGradingAsync(new MessageProto5() { Email = accommodationGrading.HostEmail.EmailAddress, Accommodation = accommodationGrading.AccommodationName, Grade = accommodationGrading.Grade });
            Console.WriteLine(response);

            _repository.UpdateAsync(ag.Id, accommodationGrading);
            return accommodationGrading;
        }
    }
}
