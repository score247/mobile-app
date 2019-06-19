namespace LiveScore.Soccer.Extensions
{
    using System.Linq;
    using LiveScore.Core.Models.Matches;

    public static class MatchResultExtenstion
    {
        public static MatchPeriod GetPenaltyResult(this IMatchResult matchResult)
            => matchResult?.MatchPeriods?.FirstOrDefault(p => p.PeriodType?.IsPenalties == true);
    }
}