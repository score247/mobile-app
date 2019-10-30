using LiveScore.Core;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.TimelineImages;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.MatchDetails.Information.InfoItems
{
    public class PenaltyShootOutViewModel : BaseItemViewModel
    {
        public PenaltyShootOutViewModel(
            TimelineEvent timelineEvent,
            MatchInfo matchInfo,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
             : base(timelineEvent, matchInfo, navigationService, dependencyResolver)
        {
        }

        public string HomeImageSource { get; private set; }

        public string AwayImageSource { get; private set; }

        public override BaseItemViewModel BuildData()
        {
            base.BuildData();
            var homeScore = TimelineEvent.ShootoutHomeScore;
            var awayScore = TimelineEvent.ShootoutAwayScore;

            if (TimelineEvent.HomeShootoutPlayer != null)
            {
                HomeImageSource = ImageConverter.BuildImageSource(
                    new TimelineEventImage(TimelineEvent.Type, isPenaltyScored: TimelineEvent.IsHomeShootoutScored));

                HomePlayerName = TimelineEvent.HomeShootoutPlayer?.Name;
            }

            if (TimelineEvent.AwayShootoutPlayer != null)
            {
                AwayImageSource = ImageConverter.BuildImageSource(
                    new TimelineEventImage(TimelineEvent.Type, isPenaltyScored: TimelineEvent.IsAwayShootoutScored));

                AwayPlayerName = TimelineEvent.AwayShootoutPlayer?.Name;
            }

            Score = $"{homeScore} - {awayScore}";

            return this;
        }
    }
}