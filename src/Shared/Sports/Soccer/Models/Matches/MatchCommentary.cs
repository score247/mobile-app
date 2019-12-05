using System;
using System.Collections.Generic;
using LiveScore.Core.Enumerations;
using MessagePack;

namespace LiveScore.Soccer.Models.Matches
{
    [MessagePackObject(keyAsPropertyName: true)]
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

        public string TimelineId { get; }

        public EventType TimelineType { get; private set; }

        public DateTimeOffset Time { get; }

        public byte MatchTime { get; }

        public string StoppageTime { get; private set; }

        public IEnumerable<Commentary> Commentaries { get; private set; }

        public GoalScorer GoalScorer { get; }

        public bool IsPenaltyShootOutScored { get; }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class Commentary
    {
        public string Text { get; set; }
    }

    /// <summary>
    /// Temp class for Message Pack generate AOT class
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchCommentaryList
    {
        public IEnumerable<MatchCommentary> MatchCommentaries { get; set; }
    }
}