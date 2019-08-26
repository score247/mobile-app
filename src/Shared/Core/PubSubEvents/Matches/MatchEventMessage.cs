namespace LiveScore.Core.PubSubEvents.Matches
{
    using LiveScore.Core.Models.Matches;

    public interface IMatchEventMessage
    {
        byte SportId { get; }

        IMatchEvent MatchEvent { get; }
    }
}