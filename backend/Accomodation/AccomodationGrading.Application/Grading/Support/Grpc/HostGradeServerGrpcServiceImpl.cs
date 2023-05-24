using AccommodationGradingApplication.Grading.Support.Grpc.Protos;
using AccomodationGradingDomain.Entities;
using AccomodationGradingDomain.Interfaces;
using Grpc.Core;
using MediatR;
using Rs.Ac.Uns.Ftn.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationGradingApplication.Grading.Support.Grpc;

public class HostGradeServerGrpcServiceImpl : HostGradeGrpcService.HostGradeGrpcServiceBase
{
    private readonly IHostGradingRepository _repository;
    public HostGradeServerGrpcServiceImpl(IHostGradingRepository repository)
    {
        _repository = repository;
    }
    public override async Task<MessageResponseProto6> hostGrade(MessageProto6 request, ServerCallContext context)
    {
        int sumOfGrades = 0;
        int numberOfGrades = 0;
        List<HostGrading> hostGradings = _repository.GetAllAsync().Result.ToList();
        foreach (HostGrading hg in hostGradings)
        {
            if (hg.HostEmail.EmailAddress.Equals(request.Email))
            {
                sumOfGrades += hg.Grade;
                numberOfGrades++;
            }
        }
        var response = new MessageResponseProto6 { Grade = (double)sumOfGrades / numberOfGrades };
        return response;
    }
}
