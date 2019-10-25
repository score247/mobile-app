using Images = LiveScore.Soccer.Enumerations.Images;

namespace LiveScore.Soccer.Models.TimelineImages
{
    public class PenaltyShootOutImageBuilder : ITimelineEventImageBuilder
    {
        public string BuildImageSource(TimelineEventImage timelineEvent)
            => timelineEvent.IsPenaltyScored
                ? Images.PenaltyShootoutGoal.Value
                : Images.MissedPenaltyShootoutGoal.Value;
    }
}