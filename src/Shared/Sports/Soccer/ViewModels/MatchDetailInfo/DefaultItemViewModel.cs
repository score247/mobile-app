namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System.Collections.Generic;
    using System.Linq;

    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    using Prism.Navigation;

    public class DefaultItemViewModel : BaseItemViewModel
    {
        private static readonly IDictionary<string, string> EventImages = new Dictionary<string, string>
        {
            { EventTypes.YellowCard, "images/common/yellow_card.png" },
            { EventTypes.YellowRedCard, "images/common/red_yellow_card.png" },
            { EventTypes.RedCard, "images/common/red_card.png" },
            { EventTypes.PenaltyMissed, "images/common/missed_penalty_goal.png" },
        };

        private static readonly string[] VisibleScoreEvents = new[]
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

            var eventType = TimelineEvent?.Type ?? string.Empty;

            if (EventImages.ContainsKey(eventType))
            {
                ImageSource = EventImages[eventType];
            }

            if (VisibleScoreEvents.Contains(eventType))
            {
                VisibleScore = true;
            }

            if (TimelineEvent.Team == "home")
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