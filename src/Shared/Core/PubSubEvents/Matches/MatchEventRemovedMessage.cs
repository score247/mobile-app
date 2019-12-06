using System;
namespace LiveScore.Core.PubSubEvents.Matches
{
    public interface IMatchEventRemovedMessage
    {
        string MatchId { get; }

        string[] TimelineIds { get; }
    }
}
