using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Soccer.Converters.TimelineImages
{
    public interface ITimelineEventImageConverter
    {
        string BuildImageSource(TimelineEventImage timelineEvent);
    }

    public class TimelineEventImage
    {
        public TimelineEventImage(EventType type, GoalScorer goalScorer = null)
        {
            Type = type;
            GoalScorer = goalScorer;
        }

        public EventType Type { get; }

        public GoalScorer GoalScorer { get; }
    }
}