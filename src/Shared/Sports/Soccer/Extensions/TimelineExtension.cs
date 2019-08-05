namespace LiveScore.Soccer.Extensions
{
    using System.Linq;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    public static class TimelineExtension
    {
        private static readonly string[] DetailInfoEventTypes = new[] {
            EventTypes.ScoreChange.DisplayName,
            EventTypes.PenaltyMissed.DisplayName,
            EventTypes.YellowCard.DisplayName,
            EventTypes.RedCard.DisplayName,
            EventTypes.YellowRedCard.DisplayName,
            EventTypes.PenaltyShootout.DisplayName
        };

        public static bool IsHalfTimeBreak(this ITimeline timeline)
            => timeline?.Type == EventTypes.BreakStart.DisplayName && timeline?.PeriodType == PeriodTypes.Pause.DisplayName;

        public static bool IsAwaitingExtraTimeBreak(this ITimeline timeline)
            => timeline?.Type == EventTypes.BreakStart.DisplayName && timeline?.PeriodType == PeriodTypes.AwaitingExtraTime.DisplayName;

        public static bool IsAwaitingPenaltiesBreak(this ITimeline timeline)
            => timeline?.Type == EventTypes.BreakStart.DisplayName && timeline?.PeriodType == PeriodTypes.AwaitingPenalties.DisplayName;

        public static bool IsNotExtraTimeHalfTimeBreak(this ITimeline timeline)
            => timeline?.Type == EventTypes.BreakStart.DisplayName && timeline?.PeriodType != PeriodTypes.ExtraTimeHalfTime.DisplayName;

        public static bool IsMatchEndedFullTime(this ITimeline timeline, IMatchResult matchResult)
            => timeline?.Type == EventTypes.MatchEnded.DisplayName && matchResult?.MatchStatus?.IsEnded == true;

        public static bool IsMatchEndedExtraTime(this ITimeline timeline, IMatchResult matchResult)
           => timeline?.Type == EventTypes.MatchEnded.DisplayName && matchResult?.MatchStatus?.IsAfterExtraTime == true;

        public static bool IsMatchEndNotAfterPenalty(this ITimeline timeline, IMatchResult matchResult)
            => timeline.Type == EventTypes.MatchEnded.DisplayName && !matchResult.MatchStatus.IsAfterPenalties;

        public static bool IsPenaltyShootOutStart(this ITimeline timeline)
            => timeline?.Type == EventTypes.PeriodStart.DisplayName && timeline?.PeriodType == PeriodTypes.Penalties.DisplayName;

        public static bool IsPenaltyFirstShoot(this ITimeline timeline)
            => timeline.Type == EventTypes.PenaltyShootout.DisplayName && timeline.IsFirstShoot;

        public static bool OfHomeTeam(this ITimeline timeline)
            => timeline?.Team == "home";

        public static bool IsDetailInfoEvent(this ITimeline timeline)
            => DetailInfoEventTypes.Contains(timeline.Type);
    }
}