namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Extensions;
    using LiveScore.Soccer.Models.Matches;
    using Prism.Navigation;

    public class DefaultItemViewModel : BaseItemViewModel
    {
        private static readonly IDictionary<EventType, string> EventImages = new Dictionary<EventType, string>
        {
            { EventType.YellowCard, Enumerations.Images.YellowCard.Value },
            { EventType.YellowRedCard, Enumerations.Images.RedYellowCard.Value },
            { EventType.RedCard, Enumerations.Images.RedCard.Value },
            { EventType.PenaltyMissed, Enumerations.Images.MissPenaltyGoal.Value },
        };

        private static readonly EventType[] VisibleScoreEvents = new[]
        {
            EventType.PenaltyMissed
        };

        public DefaultItemViewModel(
            TimelineEvent timelineEvent,
            MatchInfo matchInfo,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
             : base(timelineEvent, matchInfo, navigationService, dependencyResolver)
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