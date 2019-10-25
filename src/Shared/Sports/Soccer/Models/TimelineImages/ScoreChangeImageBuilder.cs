using System.Collections.Generic;
using LiveScore.Soccer.Enumerations;
using Images = LiveScore.Soccer.Enumerations.Images;

namespace LiveScore.Soccer.Models.TimelineImages
{
    public class ScoreChangeImageBuilder : ITimelineEventImageBuilder
    {
        private static readonly IDictionary<string, string> GoalImages = new Dictionary<string, string>
        {
            { GoalMethod.OwnGoal, Images.OwnGoal.Value },
            { GoalMethod.Penalty, Images.PenaltyGoal.Value },
            { string.Empty, Images.Goal.Value },
        };

        public string BuildImageSource(TimelineEventImage timelineEvent)
        {
            var goalMethod = timelineEvent?.GoalScorer?.Method ?? string.Empty;

            return GoalImages.ContainsKey(goalMethod)
                ? GoalImages[goalMethod]
                : Images.Goal.Value; // Have case header
        }
    }
}