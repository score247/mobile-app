namespace LiveScore.Core.Events
{
    using LiveScore.Core.Models.Matches;
    using Prism.Events;

    public class MatchEventPubSubEvent : PubSubEvent<IMatchEventMessage>
    {
    }

    public interface IMatchEventMessage
    {
        byte SportId { get; }

        IMatchEvent MatchEvent { get; }
    }
}