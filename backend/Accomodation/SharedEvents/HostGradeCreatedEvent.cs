namespace SharedEvents
{
    public class HostGradeCreatedEvent
    {
        public string Email { get;set; }
        public int Grade { get;set; }
        public Guid HostGradingId { get; set; }
    }
}