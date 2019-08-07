namespace LiveScore.Soccer.Extensions
{
    using System.Linq;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    public static class TimelineExtension
    {
        private static readonly EventTypes[] DetailInfoEventTypes = new[] {
            EventTypes.ScoreChange,
            EventTypes.PenaltyMissed,
            EventTypes.YellowCard,
            EventTypes.RedCard,
            EventTypes.YellowRedCard,
            EventTypes.PenaltyShootout,
            EventTypes.BreakStart,
            EventTypes.MatchEnded
        };

        public static bool IsHalfTimeBreak(this ITimelineEvent timeline)
            => timeline?.Type == EventTypes.BreakStart && timeline?.PeriodType == PeriodTypes.Pause;

        public static bool IsAwaitingExtraTimeBreak(this ITimelineEvent timeline)
            => timeline?.Type == EventTypes.BreakStart && timeline?.PeriodType == PeriodTypes.AwaitingExtraTime;

        public static bool IsAwaitingPenaltiesBreak(this ITimelineEvent timeline)
            => timeline?.Type == EventTypes.BreakStart && timeline?.PeriodType == PeriodTypes.AwaitingPenalties;

        public static bool IsNotExtraTimeHalfTimeBreak(this ITimelineEvent timeline)
            => timeline?.Type == EventTypes.BreakStart && timeline?.PeriodType != PeriodTypes.ExtraTimeHalfTime;

        public static bool IsExtraTimeHalfTimeBreak(this ITimelineEvent timeline)
            => timeline?.Type == EventTypes.BreakStart && timeline?.PeriodType == PeriodTypes.ExtraTimeHalfTime;

        public static bool IsMatchEndedFullTime(this ITimelineEvent timeline, IMatchResult matchResult)
            => timeline?.Type == EventTypes.MatchEnded && matchResult?.MatchStatus?.IsEnded == true;

        public static bool IsMatchEndedExtraTime(this ITimelineEvent timeline, IMatchResult matchResult)
           => timeline?.Type == EventTypes.MatchEnded && matchResult?.MatchStatus?.IsAfterExtraTime == true;

        public static bool IsMatchEndAfterPenalty(this ITimelineEvent timeline, IMatchResult matchResult)
           => timeline?.Type == EventTypes.MatchEnded && matchResult?.MatchStatus?.IsAfterPenalties == true;

        public static bool OfHomeTeam(this ITimelineEvent timeline)
            => timeline?.Team == "home";

        public static bool IsDetailInfoEvent(this ITimelineEvent timeline)
            => DetailInfoEventTypes.Contains(timeline.Type);
    }
}