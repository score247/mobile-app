namespace LiveScore.Basketball.Models.Matches
{
    using LiveScore.Core.Models.Matches;

    public class MatchCondition : IMatchCondition
    {
        public int Attendance { get; set; }

        public IVenue Venue { get; set; }

        public string Referee { get; set; }
    }
}