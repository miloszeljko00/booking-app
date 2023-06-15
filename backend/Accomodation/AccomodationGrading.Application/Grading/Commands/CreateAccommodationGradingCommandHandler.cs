using AccommodationGradingApplication.Grading.Support.Grpc.Protos;
using AccomodationGradingApplication.Abstractions.Messaging;
using AccomodationGradingDomain.Entities;
using AccomodationGradingDomain.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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

        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;
        private AccommodationGradingNotificationGrpcService.AccommodationGradingNotificationGrpcServiceClient client;
        private CreateGradeGrpcService.CreateGradeGrpcServiceClient gradeClient;
        public CreateAccommodationGradingCommandHandler(IAccommodationGradingRepository repository, IConfiguration configuration, IHostEnvironment env)
        {
            _repository = repository;
            _configuration = configuration;
            _env = env;
        }

        public IAccommodationGradingRepository Get_repository()
        {
            return _repository;
        }

        public async Task<AccommodationGrading> Handle(CreateAccommodationGradingCommand request, CancellationToken cancellationToken)
        {
            AccommodationGrading accommodationGrading = AccommodationGrading.Create(Guid.NewGuid(), request.createAccommodationGradingDTO.AccommodationName, request.createAccommodationGradingDTO.HostEmail, request.createAccommodationGradingDTO.GuestEmail, DateTime.Now, request.createAccommodationGradingDTO.Grade);

            if (_env.EnvironmentName != "Cloud")
            {
                var channel = new Channel(_configuration.GetValue<string>("GrpcDruzina:Notification:Address") + ":" + _configuration.GetValue<int>("GrpcDruzina:Notification:Port"), ChannelCredentials.Insecure);
                client = new AccommodationGradingNotificationGrpcService.AccommodationGradingNotificationGrpcServiceClient(channel);
                MessageResponseProto5 response = await client.accommodationGradingAsync(new MessageProto5() { Email = accommodationGrading.HostEmail.EmailAddress, Accommodation = accommodationGrading.AccommodationName, Grade = accommodationGrading.Grade });
                var gradeChannel = new Channel(_configuration.GetValue<string>("GrpcDruzina:AccommodationSuggestion:Address") + ":" + _configuration.GetValue<int>("GrpcDruzina:AccommodationSuggestion:Port"), ChannelCredentials.Insecure);
                gradeClient = new CreateGradeGrpcService.CreateGradeGrpcServiceClient(gradeChannel);
                CreateGradeProtoResponse gradeResponse = await gradeClient.createGradeAsync(new CreateGradeProto() { AccommodationName = accommodationGrading.AccommodationName, GuestEmail= request.createAccommodationGradingDTO.GuestEmail, Grade = accommodationGrading.Grade, Date = DateTime.Now.ToString("yyyy-MM-dd") });
            }
            else
            {
                using var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcDruzina:Notification:Address"), new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                client = new AccommodationGradingNotificationGrpcService.AccommodationGradingNotificationGrpcServiceClient(channel);
                MessageResponseProto5 response = await client.accommodationGradingAsync(new MessageProto5() { Email = accommodationGrading.HostEmail.EmailAddress, Accommodation = accommodationGrading.AccommodationName, Grade = accommodationGrading.Grade });
                using var gradeChannel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcDruzina:AccommodationSuggestion:Address"), new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                gradeClient = new CreateGradeGrpcService.CreateGradeGrpcServiceClient(gradeChannel);
                CreateGradeProtoResponse gradeResponse = await gradeClient.createGradeAsync(new CreateGradeProto() { AccommodationName = accommodationGrading.AccommodationName, GuestEmail = request.createAccommodationGradingDTO.GuestEmail, Grade = accommodationGrading.Grade, Date = DateTime.Now.ToString("yyyy-MM-dd") });
            }
            

            return _repository.Create(accommodationGrading).Result;
        }
    }
}
