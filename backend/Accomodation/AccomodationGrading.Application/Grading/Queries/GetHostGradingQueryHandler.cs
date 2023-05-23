using AccomodationGradingApplication.Abstractions.Messaging;
using AccomodationGradingApplication.Dtos;
using AccomodationGradingDomain.Entities;
using AccomodationGradingDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationGradingApplication.Grading.Queries
{
    public sealed class GetHostGradingQueryHandler : IQueryHandler<GetHostGradingQuery, List<HostGradingDTO>>
    {
        private readonly IHostGradingRepository _repository;
        public GetHostGradingQueryHandler(IHostGradingRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<HostGradingDTO>> Handle(GetHostGradingQuery request, CancellationToken cancellationToken)
        {
            List<HostGrading> hostGradings = _repository.GetAllAsync().Result.ToList();
            List<HostGradingDTO> hostGradingDTOs = new List<HostGradingDTO>();
            foreach(HostGrading hg in hostGradings)
            {
                HostGradingDTO hostGradingDTO = new HostGradingDTO
                {
                    Id = hg.Id,
                    Date = hg.Date,
                    GuestEmail = hg.GuestEmail.EmailAddress,
                    HostEmail = hg.HostEmail.EmailAddress,
                    Grade = hg.Grade,
                    AverageGrade = AverageGradeByHost(hg.HostEmail.EmailAddress)
                };
                hostGradingDTOs.Add(hostGradingDTO);
            }
            return hostGradingDTOs;
        }

        private double AverageGradeByHost(string hostEmail)
        {
            int sumOfGrades = 0;
            int numberOfGrades = 0;
            List<HostGrading> hostGradings = _repository.GetAllAsync().Result.ToList();
            foreach (HostGrading hg in hostGradings)
            {
                if (hg.HostEmail.EmailAddress.Equals(hostEmail))
                {
                    sumOfGrades += hg.Grade;
                    numberOfGrades++;
                }
            }
            return (double)sumOfGrades/numberOfGrades;
        }
    }
}
