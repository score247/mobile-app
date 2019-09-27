namespace LiveScore.Core.PubSubEvents.Matches
{
    using LiveScore.Core.Models.Matches;
    using System.Collections.Generic;

    public interface ILiveMatchMessage
    {
        byte SportId { get; }

        IEnumerable<IMatch> NewMatches { get; }

        string[] RemoveMatchIds { get; }
    }
}