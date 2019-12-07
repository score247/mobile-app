using System.Linq;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Extensions;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.TimelineImages;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.Matches.MatchDetails.Information.InfoItems
{
    public class DefaultItemViewModel : BaseItemViewModel
    {
        private static readonly EventType[] VisibleScoreEvents =
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

        public override BaseItemViewModel BuildData()
        {
            base.BuildData();

            if (TimelineEvent != null && !string.IsNullOrWhiteSpace(TimelineEvent.Type.DisplayName))
            {
                var imageSource = ImageConverter.BuildImageSource(new TimelineEventImage(TimelineEvent.Type));

                if (!string.IsNullOrEmpty(imageSource))
                {
                    ImageSource = imageSource;
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

            return this;
        }
    }
}