using LiveScore.Core;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.Converters.TimelineImages;
using LiveScore.Soccer.Extensions;
using LiveScore.Soccer.Models.Matches;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    public class SubstitutionItemViewModel : BaseItemViewModel
    {
        public SubstitutionItemViewModel(
            TimelineEvent timelineEvent,
            MatchInfo matchInfo,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
             : base(timelineEvent, matchInfo, navigationService, dependencyResolver)
        {
        }

        public string HomePlayerOutName { get; private set; }

        public string HomePlayerInName { get; private set; }

        public string AwayPlayerOutName { get; private set; }

        public string AwayPlayerInName { get; private set; }

        public string PlayerOutImageSource { get; private set; }

        public string PlayerInImageSource { get; private set; }

        public override BaseItemViewModel BuildData()
        {
            base.BuildData();

            //TODO binding in/out icons
            PlayerOutImageSource = ImageConverter.BuildImageSource(new TimelineEventImage(TimelineEvent?.Type));
            PlayerInImageSource = ImageConverter.BuildImageSource(new TimelineEventImage(TimelineEvent?.Type));

            if (TimelineEvent.OfHomeTeam())
            {
                BuildHomeInfo();
            }
            else
            {
                BuildAwayInfo();
            }

            return this;
        }

        private void BuildAwayInfo()
        {
            HomePlayerOutName = TimelineEvent.PlayerOut?.Name;
            HomePlayerInName = TimelineEvent.PlayerIn?.Name;
        }

        private void BuildHomeInfo()
        {
            AwayPlayerOutName = TimelineEvent.PlayerOut?.Name;
            AwayPlayerInName = TimelineEvent.PlayerIn?.Name;
        }
    }
}