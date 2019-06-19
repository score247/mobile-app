namespace LiveScore.Core.Models.Matches
{
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Core.Enumerations;
    using PropertyChanged;

    public interface IMatchResult
    {
        MatchStatus MatchStatus { get; }

        MatchStatus EventStatus { get; }

        int HomeScore { get; }

        int AwayScore { get; }

        int Period { get; }

        int MatchTime { get; set; }

        IEnumerable<MatchPeriod> MatchPeriods { get; }

        string WinnerId { get; }

        int AggregateHomeScore { get; }

        int AggregateAwayScore { get; }

        string AggregateWinnerId { get; }
    }

    [AddINotifyPropertyChangedInterface]
    public class MatchResult : IMatchResult
    {
        public MatchStatus MatchStatus { get; set; }

        public MatchStatus EventStatus { get; set; }

        public int Period { get; set; }

        public IEnumerable<MatchPeriod> MatchPeriods { get; set; }

        public int MatchTime { get; set; }

        public string WinnerId { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public int AggregateHomeScore { get; set; }

        public int AggregateAwayScore { get; set; }

        public string AggregateWinnerId { get; set; }
    }
}