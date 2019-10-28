using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.PubSubEvents.Matches
{
    public interface IMatchEventMessage
    {
        byte SportId { get; }

        IMatchEvent MatchEvent { get; }
    }
}