using AccommodationApplication.Accommodation.Support.Grpc.Protos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Interfaces;
using AccomodationSuggestionDomain.Primitives.Enums;
using Grpc.Core;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Commands
{
    public sealed class CreateAccommodationCommandHandler : ICommandHandler<CreateAccommodationCommand, AccomodationSuggestionDomain.Entities.Accommodation>
    {
        private readonly IAccommodationRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;
        private CreateAccommodationGrpcService.CreateAccommodationGrpcServiceClient client;

        public AccommodationBuilder AccommodationBuilder { get; set; } = new AccommodationBuilder();
        public CreateAccommodationCommandHandler(IAccommodationRepository repository, IConfiguration configuration, IHostEnvironment env)
        {
            _repository = repository;
            _configuration = configuration;
            _env = env;
        }

        public async Task<AccomodationSuggestionDomain.Entities.Accommodation> Handle(CreateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var accommodationBuilder = AccommodationBuilder.withAddress(request.AccommodationDto.Address.Country,
                request.AccommodationDto.Address.City, request.AccommodationDto.Address.Street, request.AccommodationDto.Address.Number)
                                .withPriceCalculation(request.AccommodationDto.PriceCalculation)
                                .withPricePerGuest(request.AccommodationDto.PricePerGuest)
                                .withName(request.AccommodationDto.Name)
                                .withCapacity(request.AccommodationDto.Capacity.Max, request.AccommodationDto.Capacity.Min)
                                .withAutomaticallyReservation(request.AccommodationDto.ReserveAutomatically)
                                .withHostEmail(request.AccommodationDto.HostEmail);
            
            foreach (var picture in request.AccommodationDto.Pictures)
            { 
                accommodationBuilder.withPicture(picture.FileName, picture.Base64);
            }
            //var benefits = request.AccommodationDto.Benefits
            //    .Select(b => (Benefit)Enum.Parse(typeof(Benefit), b))
            //    .ToList();
            var benefits = new List<Benefit>();
            foreach (var benefit in request.AccommodationDto.Benefits)
            {
                benefits.Add(benefit);
            }
            accommodationBuilder.withBenefits(benefits);
            var accommodation = accommodationBuilder.build();
            if (_env.EnvironmentName != "Cloud")
            {
                var channel = new Channel(_configuration.GetValue<string>("GrpcDruzina:AccommodationSuggestion:Address") + ":" + _configuration.GetValue<int>("GrpcDruzina:AccommodationSUggestion:Port"), ChannelCredentials.Insecure);
                client = new CreateAccommodationGrpcService.CreateAccommodationGrpcServiceClient(channel);
                CreateAccommodationProtoResponse response = await client.createAccommodationAsync(new CreateAccommodationProto() { AccomodationName = accommodation.Name, HostEmail = accommodation.HostEmail.EmailAddress});

                double averageGrade = 5;

            }
            else
            {
                using var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcDruzina:AccommodationSuggestion:Address"), new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                client = new CreateAccommodationGrpcService.CreateAccommodationGrpcServiceClient(channel);
                CreateAccommodationProtoResponse response = await client.createAccommodationAsync(new CreateAccommodationProto() { AccomodationName = accommodation.Name, HostEmail = accommodation.HostEmail.EmailAddress });

                double averageGrade = 5;

                
            }
            return _repository.Create(accommodation).Result;
        }
    }
}
