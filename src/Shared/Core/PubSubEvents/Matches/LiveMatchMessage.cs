using System.Collections.Generic;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.PubSubEvents.Matches
{
    public interface ILiveMatchMessage
    {
        byte SportId { get; }

        IEnumerable<IMatch> NewMatches { get; }

        string[] RemoveMatchIds { get; }
    }
}