using AccommodationApplication.Accommodation.Support.Grpc.Protos;
using AccomodationApplication.Accommodation.Queries;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Interfaces;
using Grpc.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Accommodation;

public class ServerGrpcServiceImpl : AccomodationGrpcService.AccomodationGrpcServiceBase
{
    private readonly IAccommodationRepository _repository;

    private readonly IMediator _mediator;
    public ServerGrpcServiceImpl(IMediator mediator, IAccommodationRepository repository)
    {
        _mediator = mediator;
        _repository = repository;
    }
    public override async Task<MessageResponseProto2> communicate(MessageProto2 request, ServerCallContext context)
    {
        var response = new MessageResponseProto2();
        response.CanDelete = true;
        if (request.UserRole.Equals("guest"))
        {
            var reservations = await _mediator.Send(new GetAllReservationsByGuestQuery(request.UserEmail));
            if(reservations is not null)
            {
                reservations = reservations.Where(reservation => reservation.End > DateTime.UtcNow).ToList();
                if(reservations.Count > 0)
                {
                    response.CanDelete = false;
                    return response;
                }
            }
        }
        else
        {
            var accommodations = await _repository.GetAllAsync();
            accommodations = accommodations.Where(accommodation => accommodation.HostEmail.EmailAddress.Equals(request.UserEmail)).ToList();

            var accomodationsWithActiveReservations = accommodations.Where(accommodation => accommodation.GetReservationCountAfterDate(DateTime.UtcNow) > 0).ToList();

            if(accomodationsWithActiveReservations.Count > 0)
            {
                response.CanDelete = false;
                return response;
            }
            else
            {
                foreach (var accommodation in accommodations)
                {
                    await _repository.RemoveAsync(accommodation.Id);
                }
            }

        }


        return response;
    }
}
