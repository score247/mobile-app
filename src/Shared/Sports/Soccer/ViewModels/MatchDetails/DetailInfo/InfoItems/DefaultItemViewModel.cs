namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Extensions;
    using Prism.Navigation;

    public class DefaultItemViewModel : BaseItemViewModel
    {
        private static readonly IDictionary<EventTypes, string> EventImages = new Dictionary<EventTypes, string>
        {
            { EventTypes.YellowCard, Enumerations.Images.YellowCard.Value },
            { EventTypes.YellowRedCard, Enumerations.Images.RedYellowCard.Value },
            { EventTypes.RedCard, Enumerations.Images.RedCard.Value },
            { EventTypes.PenaltyMissed, Enumerations.Images.MissPenaltyGoal.Value },
        };

        private static readonly EventTypes[] VisibleScoreEvents = new[]
        {
            EventTypes.PenaltyMissed
        };

        public DefaultItemViewModel(

            ITimelineEvent timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(timelineEvent, matchResult, navigationService, depdendencyResolver)
        {
        }

        protected override void BuildInfo()
        {
            base.BuildInfo();

            if (TimelineEvent != null && !string.IsNullOrWhiteSpace(TimelineEvent.Type.DisplayName))
            {
                if (EventImages.ContainsKey(TimelineEvent.Type))
                {
                    ImageSource = EventImages[TimelineEvent.Type];
                }

                if (VisibleScoreEvents.Contains(TimelineEvent.Type))
                {
                    VisibleScore = true;
                }
            }

            if (TimelineEvent.OfHomeTeam())
            {
                HomePlayerName = TimelineEvent?.Player?.Name;
                VisibleHomeImage = true;
            }
            else
            {
                AwayPlayerName = TimelineEvent?.Player?.Name;
                VisibleAwayImage = true;
            }
        }
    }
}