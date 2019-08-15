namespace LiveScore.Core.Models.Matches
{
    public interface IMatchEvent
    {
        string MatchId { get; }

        IMatchResult MatchResult { get; }

        ITimelineEvent Timeline { get; }
    }
}