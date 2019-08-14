namespace LiveScore.Soccer.Extensions
{
    using System.Linq;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    public static class TimelineExtension
    {
        private static readonly EventType[] DetailInfoEventTypes = new[] {
            EventType.ScoreChange,
            EventType.PenaltyMissed,
            EventType.YellowCard,
            EventType.RedCard,
            EventType.YellowRedCard,
            EventType.PenaltyShootout,
            EventType.BreakStart,
            EventType.MatchEnded
        };

        public static bool IsHalfTimeBreak(this ITimelineEvent timeline)
            => timeline?.Type == EventType.BreakStart && timeline?.PeriodType == PeriodType.Pause;

        public static bool IsAwaitingExtraTimeBreak(this ITimelineEvent timeline)
            => timeline?.Type == EventType.BreakStart && timeline?.PeriodType == PeriodType.AwaitingExtraTime;

        public static bool IsAwaitingPenaltiesBreak(this ITimelineEvent timeline)
            => timeline?.Type == EventType.BreakStart && timeline?.PeriodType == PeriodType.AwaitingPenalties;

        public static bool IsNotExtraTimeHalfTimeBreak(this ITimelineEvent timeline)
            => timeline?.Type == EventType.BreakStart && timeline?.PeriodType != PeriodType.ExtraTimeHalfTime;

        public static bool IsExtraTimeHalfTimeBreak(this ITimelineEvent timeline)
            => timeline?.Type == EventType.BreakStart && timeline?.PeriodType == PeriodType.ExtraTimeHalfTime;

        public static bool IsMatchEndedFullTime(this ITimelineEvent timeline, IMatchResult matchResult)
            => timeline?.Type == EventType.MatchEnded && matchResult?.MatchStatus?.IsEnded == true;

        public static bool IsMatchEndedExtraTime(this ITimelineEvent timeline, IMatchResult matchResult)
           => timeline?.Type == EventType.MatchEnded && matchResult?.MatchStatus?.IsAfterExtraTime == true;

        public static bool IsMatchEndAfterPenalty(this ITimelineEvent timeline, IMatchResult matchResult)
           => timeline?.Type == EventType.MatchEnded && matchResult?.MatchStatus?.IsAfterPenalties == true;

        public static bool OfHomeTeam(this ITimelineEvent timeline)
            => timeline?.Team == "home";

        public static bool IsDetailInfoEvent(this ITimelineEvent timeline)
            => DetailInfoEventTypes.Contains(timeline.Type);
    }
}