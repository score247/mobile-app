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
            string[] removeMatchIds)
        {
            SportId = sportId;
            NewMatches = newMatches;
            RemoveMatchIds = removeMatchIds;
        }

        public byte SportId { get; }

        public IEnumerable<IMatch> NewMatches { get; }

        public string[] RemoveMatchIds { get; }
    }
}