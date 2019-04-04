namespace LiveScore.Domain.DomainModels
{
    public interface IMatchCondition
    {
        int Attendance { get; }

        IVenue Venue { get; }

        IReferee Referee { get; }
    }

    public class MatchCondition : IMatchCondition
    {
        public int Attendance { get; set; }

        public IVenue Venue { get; set; }

        public IReferee Referee { get; set; }
    }
}