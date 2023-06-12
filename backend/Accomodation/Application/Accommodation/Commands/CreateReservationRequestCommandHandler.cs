using AccommodationApplication.Accommodation.Support.Grpc.Protos;
using Accomodation.Domain.Primitives.Enums;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Interfaces;
using AccomodationSuggestionDomain.Primitives.Enums;
using AccomodationSuggestionDomain.ValueObjects;
using Grpc.Core;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AccomodationApplication.Accommodation.Commands
{
    public sealed class CreateReservationRequestCommandHandler : ICommandHandler<CreateReservationRequestCommand, AccomodationSuggestionDomain.Entities.Accommodation>
    {
        private readonly IAccommodationRepository _repository;
        private readonly IConfiguration _configuration;
        private IHostEnvironment _env;

        private HostRequestNotificationGrpcService.HostRequestNotificationGrpcServiceClient client;
        public AccommodationBuilder AccommodationBuilder { get; set; } = new AccommodationBuilder();
        public CreateReservationRequestCommandHandler(IAccommodationRepository repository, IConfiguration configuration, IHostEnvironment env)
        {
            _configuration = configuration;
            _repository = repository;
            _env = env;
        }

        public IAccommodationRepository Get_repository()
        {
            return _repository;
        }

        public async Task<AccomodationSuggestionDomain.Entities.Accommodation> Handle(CreateReservationRequestCommand request, CancellationToken cancellationToken)
        {
            var acc = _repository.GetAsync(request.reservationRequestDTO.AccommodationId);
            AccomodationSuggestionDomain.Entities.Accommodation accommodation = acc.Result;
            if(accommodation is null) 
            {
                throw new Exception("Accommodation does not exist");
            }
            if (!accommodation.IsDateRangeOfReservationValid(DateRange.Create(request.reservationRequestDTO.Start, request.reservationRequestDTO.End)))
            {
                throw new Exception("Accommodation is not available in this date range");
            }
            if (accommodation.IsReservationDateRangeTaken(DateRange.Create(request.reservationRequestDTO.Start, request.reservationRequestDTO.End)))
            {
                throw new Exception("This date range is already reserved");
            }
            if (accommodation.ReserveAutomatically)
            {
                bool isPerPerson = accommodation.PriceCalculation == PriceCalculation.PER_PERSON ? true : false;
                accommodation.CreateReservationRequest(request.reservationRequestDTO.GuestEmail, request.reservationRequestDTO.Start, request.reservationRequestDTO.End, request.reservationRequestDTO.numberOfGuests, ReservationRequestStatus.ACCEPTED);
                accommodation.CreateReservation(request.reservationRequestDTO.GuestEmail, request.reservationRequestDTO.Start, request.reservationRequestDTO.End, request.reservationRequestDTO.numberOfGuests, isPerPerson, (int)accommodation.GetPriceForSpecificDate(request.reservationRequestDTO.Start).Value, false);
            }
            else
            {
                accommodation.CreateReservationRequest(request.reservationRequestDTO.GuestEmail, request.reservationRequestDTO.Start, request.reservationRequestDTO.End, request.reservationRequestDTO.numberOfGuests, ReservationRequestStatus.PENDING);
            }

            
            if(_env.EnvironmentName != "Cloud")
            {
                var channel = new Channel(_configuration.GetValue<string>("GrpcDruzina:Notification:Address") + ":" + _configuration.GetValue<int>("GrpcDruzina:Notification:Port"), ChannelCredentials.Insecure);
                client = new HostRequestNotificationGrpcService.HostRequestNotificationGrpcServiceClient(channel);
                MessageResponseProto3 response = await client.communicateAsync(new MessageProto3() { Email = accommodation.HostEmail.EmailAddress, Accommodation = accommodation.Name, StartDate = request.reservationRequestDTO.Start.ToString("dd.MM.yyy."), EndDate = request.reservationRequestDTO.End.ToString("dd.MM.yyy.") });
            }
            else
            {
                using var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcDruzina:Notification:Address"), new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                client = new HostRequestNotificationGrpcService.HostRequestNotificationGrpcServiceClient(channel);
                MessageResponseProto3 response = await client.communicateAsync(new MessageProto3() { Email = accommodation.HostEmail.EmailAddress, Accommodation = accommodation.Name, StartDate = request.reservationRequestDTO.Start.ToString("dd.MM.yyy."), EndDate = request.reservationRequestDTO.End.ToString("dd.MM.yyy.") });
            }

            _repository.UpdateAsync(accommodation.Id, accommodation);
            return accommodation;
        }
    }
}
