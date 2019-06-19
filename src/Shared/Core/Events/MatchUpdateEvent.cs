namespace LiveScore.Core.Events
{
    using LiveScore.Core.Models.Matches;
    using Prism.Events;

    public class MatchUpdateEvent : PubSubEvent<IMatch>
    {
    }
}