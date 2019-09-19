namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems
{
    using System.Linq;
    using Core;
    using Enumerations;
    using Extensions;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using Prism.Navigation;

    public class AsianHdpMovementItemViewModel : BaseMovementItemViewModel
    {
        public AsianHdpMovementItemViewModel(
            IOddsMovement oddsMovement,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(BetType.AsianHDP, oddsMovement, navigationService, dependencyResolver)
        {
            BuildHomeOddsAndOptionValue();

            BuildAwayOdds();
        }

        public string HomeOdds { get; private set; }

        public string HomeOddsTrend { get; private set; }

        public string OptionValue { get; private set; }

        public string AwayOdds { get; private set; }

        public string AwayOddsTrend { get; private set; }

        private void BuildHomeOddsAndOptionValue()
        {
            var homeOdds = OddsMovement.BetOptions?.FirstOrDefault(x => x.Type.Equals(BetOption.Home.DisplayName));

            if (homeOdds == null)
            {
                return;
            }

            HomeOdds = homeOdds.LiveOdds.ToOddsFormat();
            HomeOddsTrend = homeOdds.OddsTrend == null ? OddsTrend.Neutral.DisplayName : homeOdds.OddsTrend.Value.ToString();
            OptionValue = homeOdds.OptionValue.ToOddsOptionFormat();
        }

        private void BuildAwayOdds()
        {
            var awayOdds = OddsMovement.BetOptions?.FirstOrDefault(x => x.Type.Equals(BetOption.Away.DisplayName));

            if (awayOdds == null)
            {
                return;
            }

            AwayOdds = awayOdds.LiveOdds.ToOddsFormat();
            AwayOddsTrend = awayOdds.OddsTrend == null ? OddsTrend.Neutral.DisplayName : awayOdds.OddsTrend.Value.ToString();
        }
    }
}