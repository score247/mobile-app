using System;
using LiveScore.Core.Enumerations;

namespace LiveScore.Core.Models.Matches
{
    public interface ITimelineEvent : IEntity<string, string>
    {
        EventType Type { get; }

        DateTimeOffset Time { get; }

        byte MatchTime { get; }

        PeriodType PeriodType { get; }
    }
}