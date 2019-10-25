using System.Collections.Generic;
using LiveScore.Core.Enumerations;

namespace LiveScore.Core.Models.Matches
{
    public interface IMatchResult
    {
        MatchStatus MatchStatus { get; }

        MatchStatus EventStatus { get; }

        byte Period { get; }

        byte MatchTime { get; }

        IEnumerable<MatchPeriod> MatchPeriods { get; }

        string WinnerId { get; }

        void UpdateMatchTime(byte matchTime);
    }
}