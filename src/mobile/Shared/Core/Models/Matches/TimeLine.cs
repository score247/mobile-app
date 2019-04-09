namespace LiveScore.Core.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core.Models.Teams;

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

        IPlayer GoalScorer { get; }

        IPlayer Assist { get; }

        IPlayer PlayerOut { get; }

        IPlayer PlayerIn { get; }

        int InjuryTimeAnnounced { get; }

        string Description { get; }

        string Outcome { get; }

        IEnumerable<Commentary> Commentaries { get; }
    }

    public class Commentary
    {
        public string Text { get; set; }
    }
}