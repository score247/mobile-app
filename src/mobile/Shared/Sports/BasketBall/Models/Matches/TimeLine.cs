namespace LiveScore.Basketball.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;

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

        public IPlayer GoalScorer { get; set; }

        public IEnumerable<Commentary> Commentaries { get; set; }

        public IPlayer Assist { get; set; }

        public IPlayer PlayerOut { get; set; }

        public IPlayer PlayerIn { get; set; }

        public int InjuryTimeAnnounced { get; set; }

        public string Description { get; set; }

        public string Outcome { get; set; }

    }
}