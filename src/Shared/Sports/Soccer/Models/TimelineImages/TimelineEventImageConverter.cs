using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Soccer.Models.TimelineImages
{
    public interface ITimelineEventImageBuilder
    {
        string BuildImageSource(TimelineEventImage timelineEvent);
    }

    public class TimelineEventImage
    {
        public TimelineEventImage(EventType type,
            GoalScorer goalScorer = null,
            bool isPenaltyScored = false)
        {
            Type = type;
            GoalScorer = goalScorer;
            IsPenaltyScored = isPenaltyScored;
        }

        public EventType Type { get; }

        public GoalScorer GoalScorer { get; }

        public bool IsPenaltyScored { get; }
    }
}