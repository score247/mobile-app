﻿namespace LiveScore.Core.Models.Matches
{
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

        int MatchTime { get; set; }
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

        public int MatchTime { get; set; }
    }
}