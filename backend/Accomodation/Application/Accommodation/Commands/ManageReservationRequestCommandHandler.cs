using AccommodationApplication.Accommodation.Support.Grpc.Protos;
using Accomodation.Domain.Primitives.Enums;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Interfaces;
using AccomodationSuggestionDomain.Primitives.Enums;
using AccomodationSuggestionDomain.ValueObjects;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Commands
{
    public sealed class ManageReservationRequestCommandHandler : ICommandHandler<ManageReservationRequestCommand, AccomodationSuggestionDomain.Entities.Accommodation>
    {
        private readonly IAccommodationRepository _repository;

        private Channel channel;

        private GuestNotificationGrpcService.GuestNotificationGrpcServiceClient client;

        public AccommodationBuilder AccommodationBuilder { get; set; } = new AccommodationBuilder();
        public ManageReservationRequestCommandHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public IAccommodationRepository Get_repository()
        {
            return _repository;
        }

        public async Task<AccomodationSuggestionDomain.Entities.Accommodation> Handle(ManageReservationRequestCommand request, CancellationToken cancellationToken)
        {
            var acc = _repository.GetAsync(request.reservationRequestManagementDTO.AccommodationId);
            AccomodationSuggestionDomain.Entities.Accommodation accommodation = acc.Result;
            if(accommodation is null) 
            {
                throw new Exception("Accommodation does not exist");
            }
            List<ReservationRequest> reservationRequests = accommodation.ReservationRequests;
            ReservationRequest req = null;
            foreach (ReservationRequest reservationRequest in reservationRequests)
            {
                if (reservationRequest.Id.Equals(request.reservationRequestManagementDTO.ReservationId))
                {
                    req = reservationRequest;
                    reservationRequests.Remove(reservationRequest);
                    break;
                }
            }
            string operation;
            if (req is not null)
            {
                if (request.reservationRequestManagementDTO.Operation.Equals("ACCEPT"))
                {
                    operation = "PRIHVACEN";
                    List<ReservationRequest> reqs = accommodation.GetReservationRequestsOverlappingDateRange(req.ReservationDate);
                    foreach (ReservationRequest reservationRequest in reqs)
                    {
                        reservationRequests.Remove(reservationRequest);
                        accommodation.CreateReservationRequest(reservationRequest.GuestEmail.EmailAddress, reservationRequest.ReservationDate.Start, reservationRequest.ReservationDate.End, reservationRequest.GuestNumber, ReservationRequestStatus.REJECTED);
                    }
                    accommodation.CreateReservationRequest(req.GuestEmail.EmailAddress, req.ReservationDate.Start, req.ReservationDate.End, req.GuestNumber, ReservationRequestStatus.ACCEPTED);
                    accommodation.CreateReservation(req.GuestEmail.EmailAddress, req.ReservationDate.Start, req.ReservationDate.End, req.GuestNumber, accommodation.PriceCalculation == PriceCalculation.PER_PERSON ? true : false, (int)accommodation.GetPriceForSpecificDate(req.ReservationDate.Start).Value, false);
                }
                else if (request.reservationRequestManagementDTO.Operation.Equals("REJECT"))
                {
                    operation = "ODBIJEN";
                    accommodation.CreateReservationRequest(req.GuestEmail.EmailAddress, req.ReservationDate.Start, req.ReservationDate.End, req.GuestNumber, ReservationRequestStatus.REJECTED);
                }
                else
                {
                    throw new Exception("Invalid operation selected");
                }
            }
            else
                throw new Exception("Reservation request does not exist");

            channel = new Channel("127.0.0.1:8787", ChannelCredentials.Insecure);
            client = new GuestNotificationGrpcService.GuestNotificationGrpcServiceClient(channel);
            MessageResponseProto response = await client.communicateAsync(new MessageProto() { Email = req.GuestEmail.EmailAddress, Accommodation = accommodation.Name, StartDate = req.ReservationDate.Start.ToString("dd.MM.yyy."), EndDate = req.ReservationDate.End.ToString("dd.MM.yyy."), Operation = operation });
            Console.WriteLine(response);

            _repository.UpdateAsync(accommodation.Id, accommodation);
            return accommodation;
        }
    }
}
