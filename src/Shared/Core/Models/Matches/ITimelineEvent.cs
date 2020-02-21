using System;
using LiveScore.Core.Enumerations;

namespace LiveScore.Core.Models.Matches
{
    public interface ITimelineEvent
    {
        string Id { get; }

        string Name { get; }

        EventType Type { get; }

        DateTimeOffset Time { get; }

        byte MatchTime { get; }

        PeriodType PeriodType { get; }
    }
}