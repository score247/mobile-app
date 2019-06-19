namespace LiveScore.Core.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core.Models.Teams;

    public interface ITimeline : IEntity<long, string>
    {
        string Type { get; }

        DateTime Time { get; }

        int MatchTime { get; }

        string StoppageTime { get; }

        string MatchClock { get; }

        string Team { get; }

        int Period { get; }

        string PeriodType { get; }

        int HomeScore { get; }

        int AwayScore { get; }

        GoalScorer GoalScorer { get; }

        Player Assist { get; }

        Player PlayerOut { get; }

        Player PlayerIn { get; }

        Player Player { get; }

        int InjuryTimeAnnounced { get; }

        string Description { get; }

        string Outcome { get; }

        IEnumerable<Commentary> Commentaries { get; }

        Player HomeShootoutPlayer { get; }

        bool IsHomeShootoutScored { get; }

        Player AwayShootoutPlayer { get; }

        bool IsAwayShootoutScored { get; }

        int ShootoutHomeScore { get; }

        int ShootoutAwayScore { get; }
    }

    public class Timeline : Entity<long, string>, ITimeline
    {
        public string Type { get; set; }

        public DateTime Time { get; set; }

        public int MatchTime { get; set; }

        public string StoppageTime { get; set; }

        public string MatchClock { get; set; }

        public string Team { get; set; }

        public int Period { get; set; }

        public string PeriodType { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public GoalScorer GoalScorer { get; set; }

        public IEnumerable<Commentary> Commentaries { get; set; }

        public Player Assist { get; set; }

        public Player PlayerOut { get; set; }

        public Player PlayerIn { get; set; }

        public Player Player { get; set; }

        public int InjuryTimeAnnounced { get; set; }

        public string Description { get; set; }

        public string Outcome { get; set; }

        public Player HomeShootoutPlayer { get; set; }

        public bool IsHomeShootoutScored { get; set; }

        public Player AwayShootoutPlayer { get; set; }

        public bool IsAwayShootoutScored { get; set; }

        public int ShootoutHomeScore { get; set; }

        public int ShootoutAwayScore { get; set; }
    }

    public class Commentary
    {
        public string Text { get; set; }
    }

    public interface IGoalScorer : IEntity<string, string>
    {
        string Method { get; }
    }

    public class GoalScorer : Entity<string, string>, IGoalScorer
    {
        public string Method { get; set; }
    }
}