using AccommodationSuggestionApplication.Suggestion.Support.Grpc.Protos;
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
    public class CreateGradeGrpcServiceImpl: CreateGradeGrpcService.CreateGradeGrpcServiceBase
    {
        private readonly IAccommodationSuggestionRepository _repository;

        public CreateGradeGrpcServiceImpl(IAccommodationSuggestionRepository repository)
        {
            _repository = repository;
        }
        public override async Task<CreateGradeProtoResponse> createGrade(CreateGradeProto request, ServerCallContext context)
        {
            var response = new CreateGradeProtoResponse();
            var createdrelationship = _repository.createGradeRelationship(request.Grade, request.AccommodationName, request.GuestEmail, request.Date);
            response.IsCreated = true;
            return response;
        }
    }
}
