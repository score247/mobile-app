using Images = LiveScore.Soccer.Enumerations.Images;

namespace LiveScore.Soccer.Converters.TimelineImages
{
    public class PenaltyShootOutImageConverter : ITimelineEventImageConverter
    {
        public string BuildImageSource(TimelineEventImage timelineEvent)
        {
            return timelineEvent.IsPenaltyScored
                ? Images.PenaltyShootoutGoal.Value
                : Images.MissedPenaltyShootoutGoal.Value;
        }
    }
}