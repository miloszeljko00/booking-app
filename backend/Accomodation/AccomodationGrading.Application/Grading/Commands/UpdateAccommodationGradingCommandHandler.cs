using AccommodationGradingApplication.Grading.Support.Grpc.Protos;
using AccomodationGradingApplication.Abstractions.Messaging;
using AccomodationGradingDomain.Entities;
using AccomodationGradingDomain.Interfaces;
using Grpc.Core;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Rs.Ac.Uns.Ftn.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AccomodationGradingApplication.Grading.Commands
{
    public sealed class UpdateAccommodationGradingCommandHandler : ICommandHandler<UpdateAccommodationGradingCommand, AccommodationGrading>
    {
        private readonly IAccommodationGradingRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;

        private AccommodationGradingNotificationGrpcService.AccommodationGradingNotificationGrpcServiceClient client;
        public UpdateAccommodationGradingCommandHandler(IAccommodationGradingRepository repository, IConfiguration configuration, IHostEnvironment env)
        {
            _repository = repository;
            _configuration = configuration;
            _env = env;
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

            if (_env.EnvironmentName != "Cloud")
            {
                var channel = new Channel(_configuration.GetValue<string>("GrpcDruzina:Notification:Address") + ":" + _configuration.GetValue<int>("GrpcDruzina:Notification:Port"), ChannelCredentials.Insecure);
                client = new AccommodationGradingNotificationGrpcService.AccommodationGradingNotificationGrpcServiceClient(channel);
                MessageResponseProto5 response = await client.accommodationGradingAsync(new MessageProto5() { Email = accommodationGrading.HostEmail.EmailAddress, Accommodation = accommodationGrading.AccommodationName, Grade = accommodationGrading.Grade });
            }
            else
            {
                using var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcDruzina:Notification:Address"), new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                client = new AccommodationGradingNotificationGrpcService.AccommodationGradingNotificationGrpcServiceClient(channel);
                MessageResponseProto5 response = await client.accommodationGradingAsync(new MessageProto5() { Email = accommodationGrading.HostEmail.EmailAddress, Accommodation = accommodationGrading.AccommodationName, Grade = accommodationGrading.Grade });
            }

            

            _repository.UpdateAsync(ag.Id, accommodationGrading);
            return accommodationGrading;
        }
    }
}
