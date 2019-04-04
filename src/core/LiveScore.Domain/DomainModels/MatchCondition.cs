namespace LiveScore.Domain.DomainModels
{
    public interface IMatchCondition
    {
        int Attendance { get; }

        IVenue Venue { get; }

        Referee Referee { get; }
    }

    public class MatchCondition : IMatchCondition
    {
        public int Attendance { get; set; }

        public IVenue Venue { get; set; }

        public Referee Referee { get; set; }
    }
}