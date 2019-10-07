using System.Collections.Generic;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Soccer.Models.Matches;

namespace LiveScore.Soccer.PubSubEvents.Matches
{
    public class LiveMatchMessage : ILiveMatchMessage
    {
        public LiveMatchMessage(
            byte sportId,
            IEnumerable<Match> newMatches,
            string[] removeMatchIds,
            int liveMatchCount)
        {
            SportId = sportId;
            NewMatches = newMatches;
            RemoveMatchIds = removeMatchIds;
            LiveMatchCount = liveMatchCount;
        }

        public byte SportId { get; }

        public IEnumerable<IMatch> NewMatches { get; }

        public string[] RemoveMatchIds { get; }

        public int LiveMatchCount { get; }
    }
}