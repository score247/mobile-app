using LiveScore.Core;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Extensions;
using LiveScore.Soccer.Models.Matches;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.MatchDetails.Information.InfoItems
{
    public class SubstitutionItemViewModel : BaseItemViewModel
    {
        private const string WhiteSpace = " ";

        public SubstitutionItemViewModel(
            TimelineEvent timelineEvent,
            MatchInfo matchInfo,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
             : base(timelineEvent, matchInfo, navigationService, dependencyResolver)
        {
            PlayerOutImageSource = Images.SubstitutionOut.Value;
            PlayerInImageSource = Images.SubstitutionIn.Value;
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

        private void BuildHomeInfo()
        {
            HomePlayerOutName = BuildPlayerName(TimelineEvent.PlayerOut?.Name);
            HomePlayerInName = BuildPlayerName(TimelineEvent.PlayerIn?.Name);

            VisibleHomeImage = true;
        }

        private void BuildAwayInfo()
        {
            AwayPlayerOutName = BuildPlayerName(TimelineEvent.PlayerOut?.Name);
            AwayPlayerInName = BuildPlayerName(TimelineEvent.PlayerIn?.Name);

            VisibleAwayImage = true;
        }

        private static string BuildPlayerName(string name)
            => string.IsNullOrEmpty(name) ? WhiteSpace : name;
    }
}