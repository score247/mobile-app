﻿using System.Collections.Generic;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Soccer.Models.Matches;
using Prism.Events;

namespace LiveScore.Soccer.PubSubEvents.Matches
{
    public class LiveMatchMessage : ILiveMatchMessage
    {
        public const string HubMethod = "LiveMatches";

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

        public static void Publish(IEventAggregator eventAggregator, object data)
            => eventAggregator.GetEvent<LiveMatchPubSubEvent>().Publish(data as LiveMatchMessage);
    }
}