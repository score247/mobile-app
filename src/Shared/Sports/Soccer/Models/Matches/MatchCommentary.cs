using System;
using System.Collections.Generic;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using MessagePack;

namespace LiveScore.Soccer.Models.Matches
{
    [MessagePackObject]
    public class MatchCommentary
    {
        [SerializationConstructor]
        public MatchCommentary(
            string timelineId,
            EventType timelineType,
            DateTimeOffset time,
            byte matchTime,
            string stoppageTime,
            IEnumerable<Commentary> commentaries,
            GoalScorer goalScorer)
        {
            TimelineId = timelineId;
            TimelineType = timelineType;
            Time = time;
            MatchTime = matchTime;
            StoppageTime = stoppageTime;
            Commentaries = commentaries;
            GoalScorer = goalScorer;
        }

        [Key(0)]
        public string TimelineId { get; }

        [Key(1)]
        public EventType TimelineType { get; }

        [Key(2)]
        public DateTimeOffset Time { get; }

        [Key(3)]
        public byte MatchTime { get; }

        [Key(4)]
        public string StoppageTime { get; }

        [Key(5)]
        public IEnumerable<Commentary> Commentaries { get; }

        [Key(6)]
        public GoalScorer GoalScorer { get; }
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