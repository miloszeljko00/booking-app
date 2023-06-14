using AccommodationSuggestionApplication.Suggestion.Support.Grpc.Protos;
using AccomodationSuggestion.Domain.Entities;
using AccomodationSuggestion.Domain.Interfaces;
using Grpc.Core;
using Rs.Ac.Uns.Ftn.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Application.Suggestion.Support.Grpc
{
    public class CreateGuestGrpcServiceImpl: CreateGuestGrpcService.CreateGuestGrpcServiceBase
    {
        private readonly IAccommodationSuggestionRepository _repository;

        public CreateGuestGrpcServiceImpl(IAccommodationSuggestionRepository repository)
        {
            _repository = repository;
        }
        public override async Task<CreateGuestProtoResponse> createGuest(CreateGuestProto request, ServerCallContext context)
        {
            var response = new CreateGuestProtoResponse();
            var createdNode = _repository.createUserAsync(request.GuestEmail);
            response.IsCreated = true;
            return response;
        }
    }
    
}
