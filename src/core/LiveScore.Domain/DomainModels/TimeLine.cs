namespace LiveScore.Domain.DomainModels
{
    using System;
    using System.Collections.Generic;

    public interface ITimeLine : IEntity<long, string>
    {
        string Type { get; }

        DateTime Time { get; }

        int MatchTime { get; }

        string MatchClock { get; }

        string Team { get; }

        int Period { get; }

        string PeriodType { get; }

        int HomeScore { get; }

        int AwayScore { get; }

        Player GoalScorer { get; }

        IEnumerable<Commentary> Commentaries { get; }
    }

    public class TimeLine : Entity<long, string>, ITimeLine
    {
        public string Type { get; set; }

        public DateTime Time { get; set; }

        public int MatchTime { get; set; }

        public string MatchClock { get; set; }

        public string Team { get; set; }

        public int Period { get; set; }

        public string PeriodType { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public Player GoalScorer { get; set; }

        public IEnumerable<Commentary> Commentaries { get; set; }
    }

    public class Commentary
    {
        public string Text { get; set; }
    }
}