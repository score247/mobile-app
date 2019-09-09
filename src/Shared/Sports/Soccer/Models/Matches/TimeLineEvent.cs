namespace LiveScore.Core.Models.Matches
{
    using System;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Teams;

    public class TimelineEvent : Entity<string, string>, ITimelineEvent
    {
        public EventType Type { get; set; }

        public DateTimeOffset Time { get; set; }

        public byte MatchTime { get; set; }

        public string StoppageTime { get; set; }

        public string MatchClock { get; set; }

        public string Team { get; set; }

        public byte Period { get; set; }

        public PeriodType PeriodType { get; set; }

        public byte HomeScore { get; set; }

        public byte AwayScore { get; set; }

        public GoalScorer GoalScorer { get; set; }

        public Player Assist { get; set; }

        public Player Player { get; set; }

        public byte InjuryTimeAnnounced { get; set; }

        public Player HomeShootoutPlayer { get; set; }

        public bool IsHomeShootoutScored { get; set; }

        public Player AwayShootoutPlayer { get; set; }

        public bool IsAwayShootoutScored { get; set; }

        public byte ShootoutHomeScore { get; set; }

        public byte ShootoutAwayScore { get; set; }

        public bool IsFirstShoot { get; set; }

        public bool IsHome => Team == "home";

        public override bool Equals(object obj)
        {
            if (!(obj is TimelineEvent actualObj))
            {
                return false;
            }

            return Id == actualObj.Id;
        }

        public override int GetHashCode() => Id?.GetHashCode() ?? 0;
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