namespace LiveScore.Domain.DomainModels
{
    public interface IMatchResult
    {
        string Status { get; }

        int HomeScore { get; }

        int AwayScore { get; }
    }

    public class MatchResult : IMatchResult
    {
        public string Status { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }
    }
}