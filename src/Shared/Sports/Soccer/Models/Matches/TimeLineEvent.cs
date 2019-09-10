using MessagePack;

namespace LiveScore.Core.Models.Matches
{
    using System;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Teams;
    using MessagePack;

    [MessagePackObject(keyAsPropertyName: true)]
    public class TimelineEvent : ITimelineEvent
    {
        [Key(0)]
        public string Id { get; set; }

        [Key(100)]
        public string Name { get; set; }

        [Key(2)]
        public EventType Type { get; set; }

        [Key(3)]
        public DateTimeOffset Time { get; set; }

        [Key(4)]
        public byte MatchTime { get; set; }

        [Key(5)]
        public string StoppageTime { get; set; }

        [Key(6)]
        public string MatchClock { get; set; }

        [Key(7)]
        public string Team { get; set; }

        [Key(8)]
        public byte Period { get; set; }

        [Key(9)]
        public PeriodType PeriodType { get; set; }

        [Key(10)]
        public byte HomeScore { get; set; }

        [Key(11)]
        public byte AwayScore { get; set; }

        [Key(12)]
        public GoalScorer GoalScorer { get; set; }

        [Key(13)]
        public Player Assist { get; set; }

        [Key(14)]
        public Player Player { get; set; }

        [Key(15)]
        public byte InjuryTimeAnnounced { get; set; }

        [Key(16)]
        public Player HomeShootoutPlayer { get; set; }

        [Key(17)]
        public bool IsHomeShootoutScored { get; set; }

        [Key(18)]
        public Player AwayShootoutPlayer { get; set; }

        [Key(19)]
        public bool IsAwayShootoutScored { get; set; }

        [Key(20)]
        public byte ShootoutHomeScore { get; set; }

        [Key(21)]
        public byte ShootoutAwayScore { get; set; }

        [Key(22)]
        public bool IsFirstShoot { get; set; }

        [Key(23)]
        public bool IsHome { get; set; }

        [Key(24)]
        public string TestString { get; set; }

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

    [MessagePackObject]
    public class GoalScorer
    {
        [Key(0)]
        public string Id { get; set; }

        [Key(1)]
        public string Name { get; set; }

        [Key(2)]
        public string Method { get; set; }
    }
}