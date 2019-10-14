using System.Collections.Generic;
using LiveScore.Core.Enumerations;

namespace LiveScore.Soccer.Converters.TimelineImages
{
    public class DefaultEventImageConverter : ITimelineEventImageConverter
    {
        private static readonly IDictionary<EventType, string> EventImages = new Dictionary<EventType, string>
        {
            { EventType.YellowCard, Enumerations.Images.YellowCard.Value },
            { EventType.YellowRedCard, Enumerations.Images.RedYellowCard.Value },
            { EventType.RedCard, Enumerations.Images.RedCard.Value },
            { EventType.PenaltyMissed, Enumerations.Images.MissPenaltyGoal.Value },
            { EventType.Substitution, Enumerations.Images.Substitution.Value },
        };

        public string BuildImageSource(TimelineEventImage timelineEvent)
            => EventImages.ContainsKey(timelineEvent.Type) ? EventImages[timelineEvent.Type] : string.Empty;
    }
}