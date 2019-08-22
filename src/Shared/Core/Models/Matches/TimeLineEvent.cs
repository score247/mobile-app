namespace LiveScore.Core.Models.Matches
{
    using System;
    using LiveScore.Core.Enumerations;

    public interface ITimelineEvent
    {
        EventType Type { get; }

        DateTime Time { get; }

        byte MatchTime { get; }

        PeriodType PeriodType { get; }
    }
}