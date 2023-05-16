using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Notification.Application.Dtos
{
    public class CreateHostNotificationDTO
    {
        public string HostEmail { get; set; }
        public bool ReceiveAnswerForCreatedRequest { get; set; }
        public bool ReceiveAnswerForCanceledReservation { get; set; }
        public bool ReceiveAnswerForHostRating { get; set; }
        public bool ReceiveAnswerForAccommodationRating { get; set; }
        public bool ReceiveAnswerForHighlightedHostStatus { get; set; }
    }
}
