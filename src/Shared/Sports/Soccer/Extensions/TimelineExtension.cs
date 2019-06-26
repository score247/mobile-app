namespace LiveScore.Soccer.Extensions
{
    using System.Linq;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    public static class TimelineExtension
    {
        private static readonly string[] DetailInfoEventTypes = new[] {
            EventTypes.ScoreChange,
            EventTypes.PenaltyMissed,
            EventTypes.YellowCard,
            EventTypes.RedCard,
            EventTypes.YellowRedCard,
            EventTypes.PenaltyShootout
        };

        public static bool IsHalfTimeBreak(this ITimeline timeline)
            => timeline?.Type == EventTypes.BreakStart && timeline?.PeriodType == PeriodTypes.Pause;

        public static bool IsAwaitingExtraTimeBreak(this ITimeline timeline)
            => timeline?.Type == EventTypes.BreakStart && timeline?.PeriodType == PeriodTypes.AwaitingExtraTime;

        public static bool IsAwaitingPenaltiesBreak(this ITimeline timeline)
            => timeline?.Type == EventTypes.BreakStart && timeline?.PeriodType == PeriodTypes.AwaitingPenalties;

        public static bool IsNotExtraTimeHalfTimeBreak(this ITimeline timeline)
            => timeline?.Type == EventTypes.BreakStart && timeline?.PeriodType != PeriodTypes.ExtraTimeHalfTime;

        public static bool IsMatchEndedFullTime(this ITimeline timeline, IMatchResult matchResult)
            => timeline?.Type == EventTypes.MatchEnded && matchResult?.MatchStatus?.IsEnded == true;

        public static bool IsMatchEndedExtraTime(this ITimeline timeline, IMatchResult matchResult)
           => timeline?.Type == EventTypes.MatchEnded && matchResult?.MatchStatus?.IsAfterExtraTime == true;

        public static bool IsMatchEndNotAfterPenalty(this ITimeline timeline, IMatchResult matchResult)
            => timeline.Type == EventTypes.MatchEnded && !matchResult.MatchStatus.IsAfterPenalties;

        public static bool IsPenaltyShootOutStart(this ITimeline timeline)
            => timeline?.Type == EventTypes.PeriodStart && timeline?.PeriodType == PeriodTypes.Penalties;

        public static bool IsPenaltyFirstShoot(this ITimeline timeline)
            => timeline.Type == EventTypes.PenaltyShootout && timeline.IsFirstShoot;

        public static bool OfHomeTeam(this ITimeline timeline)
            => timeline?.Team == "home";

        public static bool IsDetailInfoEvent(this ITimeline timeline)
            => DetailInfoEventTypes.Contains(timeline.Type);
    }
}