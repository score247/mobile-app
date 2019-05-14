namespace LiveScore.Core.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using PropertyChanged;

    public interface IMatchResult
    {
        MatchStatus MatchStatus { get; }

        MatchStatus EventStatus { get; }

        int HomeScore { get; }

        int AwayScore { get; }

        string WinnerId { get; }

        IEnumerable<MatchPeriod> MatchPeriods { get; }

        string MatchTime { get; set; }

        int MatchTimeMinute { get; }
    }

    [AddINotifyPropertyChangedInterface]
    public class MatchResult : IMatchResult
    {
        public MatchStatus MatchStatus { get; set; }

        public MatchStatus EventStatus { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public string WinnerId { get; set; }

        public IEnumerable<MatchPeriod> MatchPeriods { get; set; }

        public string MatchTime { get; set; }

        public int MatchTimeMinute => string.IsNullOrEmpty(MatchTime) ? 0 : Convert.ToInt32(MatchTime.Split(':')[0]);
    }
}