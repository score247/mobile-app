namespace LiveScore.Domain.DomainModels
{
    public interface IEventCondition
    {
        int Attendance { get; }

        IVenue Venue { get; }

        IReferee Referee { get; }
    }

    public class EventCondition : IEventCondition
    {
        public int Attendance { get; set; }

        public IVenue Venue { get; set; }

        public IReferee Referee { get; set; }
    }
}