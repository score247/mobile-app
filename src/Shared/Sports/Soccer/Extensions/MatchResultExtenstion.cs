namespace LiveScore.Soccer.Extensions
{
    using System.Linq;
    using LiveScore.Core.Models.Matches;

    public static class MatchResultExtenstion
    {
        private const int NumberOfFullTimePeriodsResult = 2;

        public static MatchPeriod GetPenaltyResult(this IMatchResult matchResult)
            => matchResult?.MatchPeriods?.FirstOrDefault(p => p.PeriodType?.IsPenalties == true);

        public static MatchPeriod GetOvertimeResult(this IMatchResult matchResult)
          => matchResult?.MatchPeriods?.FirstOrDefault(p => p.PeriodType?.IsOvertime == true);

        public static bool HasFullTimeResult(this IMatchResult matchResult)
            => matchResult?.MatchPeriods != null && matchResult.MatchPeriods.Count() >= NumberOfFullTimePeriodsResult;
    }
}