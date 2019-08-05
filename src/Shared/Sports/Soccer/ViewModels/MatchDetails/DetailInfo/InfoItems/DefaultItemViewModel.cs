namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Extensions;
    using Prism.Navigation;

    public class DefaultItemViewModel : BaseItemViewModel
    {
        private static readonly IDictionary<EventTypes, string> EventImages = new Dictionary<EventTypes, string>
        {
            { EventTypes.YellowCard, Images.YellowCard.Value },
            { EventTypes.YellowRedCard, Images.RedYellowCard.Value },
            { EventTypes.RedCard, Images.RedCard.Value },
            { EventTypes.PenaltyMissed, Images.MissPenaltyGoal.Value },
        };

        private static readonly EventTypes[] VisibleScoreEvents = new[]
        {
            EventTypes.PenaltyMissed
        };

        public DefaultItemViewModel(

            ITimeline timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(timelineEvent, matchResult, navigationService, depdendencyResolver)
        {
        }

        protected override void BuildInfo()
        {
            base.BuildInfo();

            if (TimelineEvent != null && !string.IsNullOrWhiteSpace(TimelineEvent.Type))
            {
                var eventType = Enumeration.FromDisplayName<EventTypes>(TimelineEvent.Type);

                if (EventImages.ContainsKey(eventType))
                {
                    ImageSource = EventImages[eventType];
                }

                if (VisibleScoreEvents.Contains(eventType))
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