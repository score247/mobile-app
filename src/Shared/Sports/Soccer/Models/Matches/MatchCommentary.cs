using System;
using System.Collections.Generic;
using LiveScore.Core.Enumerations;
using MessagePack;

namespace LiveScore.Soccer.Models.Matches
{
    [MessagePackObject]
    public class MatchCommentary
    {
#pragma warning disable S107 // Methods should not have too many parameters
        [SerializationConstructor]
        public MatchCommentary(
            string timelineId,
            EventType timelineType,
            DateTimeOffset time,
            byte matchTime,
            string stoppageTime,
            IEnumerable<Commentary> commentaries,
            GoalScorer goalScorer,
            bool isPenaltyShootOutScored)
        {
            TimelineId = timelineId;
            TimelineType = timelineType;
            Time = time;
            MatchTime = matchTime;
            StoppageTime = stoppageTime;
            Commentaries = commentaries;
            GoalScorer = goalScorer;
            IsPenaltyShootOutScored = isPenaltyShootOutScored;
        }
#pragma warning restore S107 // Methods should not have too many parameters

        [Key(0)]
        public string TimelineId { get; }

        [Key(1)]
        public EventType TimelineType { get; private set; }

        [Key(2)]
        public DateTimeOffset Time { get; }

        [Key(3)]
        public byte MatchTime { get; }

        [Key(4)]
        public string StoppageTime { get; private set; }

        [Key(5)]
        public IEnumerable<Commentary> Commentaries { get; private set; }

        [Key(6)]
        public GoalScorer GoalScorer { get; }

        [Key(7)]
        public bool IsPenaltyShootOutScored { get; }
    }

    [MessagePackObject]
    public class Commentary
    {
        [Key(0)]
        public string Text { get; set; }
    }

    /// <summary>
    /// Temp class for Message Pack generate AOT class
    /// </summary>
    [MessagePackObject]
    public class MatchCommentaryList
    {
        [Key(0)]
        public IEnumerable<MatchCommentary> MatchCommentaries { get; set; }
    }
}