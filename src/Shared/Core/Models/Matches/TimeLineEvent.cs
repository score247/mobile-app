namespace LiveScore.Core.Models.Matches
{
    using System;
    using Enumerations;

    public interface ITimelineEvent : IEntity<string, string>
    {
        EventType Type { get; }

        DateTimeOffset Time { get; }

        byte MatchTime { get; }

        PeriodType PeriodType { get; }
    }
}