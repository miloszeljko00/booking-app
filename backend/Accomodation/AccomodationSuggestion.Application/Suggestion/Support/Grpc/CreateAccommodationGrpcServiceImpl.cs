using AccommodationSuggestionApplication.Suggestion.Support.Grpc.Protos;
using AccomodationSuggestion.Domain.Interfaces;
using DnsClient.Protocol;
using Grpc.Core;
using Rs.Ac.Uns.Ftn.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Application.Suggestion.Support.Grpc
{
    public class CreateAccommodationGrpcServiceImpl : CreateAccommodationGrpcService.CreateAccommodationGrpcServiceBase
    {
        private readonly IAccommodationSuggestionRepository _repository;
        public CreateAccommodationGrpcServiceImpl(IAccommodationSuggestionRepository repository)
        {
            _repository = repository;
        }
        public override async Task<CreateAccommodationProtoResponse> createAccommodation(CreateAccommodationProto request, ServerCallContext context)
        {
            var response = new CreateAccommodationProtoResponse();
            response.IsCreated = true;
            return response;
        }
    }
}
