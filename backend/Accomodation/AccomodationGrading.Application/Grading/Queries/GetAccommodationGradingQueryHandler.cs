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

namespace Notification.Application.Notification.Queries
{
    public sealed class GetAccommodationGradingQueryHandler : IQueryHandler<GetAccommodationGradingQuery, List<AccommodationGradingDTO>>
    {
        private readonly IAccommodationGradingRepository _repository;
        public GetAccommodationGradingQueryHandler(IAccommodationGradingRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AccommodationGradingDTO>> Handle(GetAccommodationGradingQuery request, CancellationToken cancellationToken)
        {
            List<AccommodationGrading> accommodationGradings = _repository.GetAllAsync().Result.ToList();
            List<AccommodationGradingDTO> accommodationGradingDTOs = new List<AccommodationGradingDTO>();
            foreach(AccommodationGrading ag in accommodationGradings)
            {
                AccommodationGradingDTO accommodationGradingDTO = new AccommodationGradingDTO
                {
                    Id = ag.Id,
                    Date = ag.Date,
                    GuestEmail = ag.GuestEmail.EmailAddress,
                    HostEmail = ag.HostEmail.EmailAddress,
                    AccommodationName = ag.AccommodationName,
                    Grade = ag.Grade,
                    AverageGrade = AverageGradeByAccommodation(ag.HostEmail.EmailAddress, ag.AccommodationName)
                };
                accommodationGradingDTOs.Add(accommodationGradingDTO);
            }
            return accommodationGradingDTOs;
        }

        private double AverageGradeByAccommodation(string hostEmail, string accommodationName)
        {
            int sumOfGrades = 0;
            int numberOfGrades = 0;
            List<AccommodationGrading> accommodationGradings = _repository.GetAllAsync().Result.ToList();
            foreach (AccommodationGrading ag in accommodationGradings)
            {
                if (ag.HostEmail.EmailAddress.Equals(hostEmail) && ag.AccommodationName.Equals(accommodationName))
                {
                    sumOfGrades += ag.Grade;
                    numberOfGrades++;
                }
            }
            return (double)sumOfGrades/numberOfGrades;
        }
    }
}
