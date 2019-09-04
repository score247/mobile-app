namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems
{
    using System.Linq;
    using Core;
    using Enumerations;
    using Extensions;
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
            Initialize(oddsMovement);
        }

        public string MatchTime { get; private set; }

        public string MatchScore { get; private set; }

        public string HomeOdds { get; private set; }

        public string HomeOddsTrend { get; private set; }

        public string OptionValue { get; private set; }

        public string AwayOdds { get; private set; }

        public string AwayOddsTrend { get; private set; }

        private void Initialize(IOddsMovement oddsMovement)
        {
            MatchScore = oddsMovement.IsMatchStarted
                ? $"{oddsMovement.HomeScore} - {oddsMovement.AwayScore}"
                : string.Empty;

            MatchTime = oddsMovement.MatchTime;

            BuildHomeOdds(oddsMovement);

            BuildBetOptionsValue(oddsMovement);

            BuildAwayOdds(oddsMovement);
        }

        private void BuildAwayOdds(IOddsMovement oddsMovement)
        {
            var awayOdds = GetOddsInfo(BetOption.Away, oddsMovement);

            if (awayOdds != null)
            {
                AwayOdds = awayOdds.LiveOdds.ToOddsFormat();
                AwayOddsTrend = awayOdds.OddsTrend.Value.ToString();
            }
        }

        private void BuildBetOptionsValue(IOddsMovement oddsMovement)
        {
            var homeOdds = GetOddsInfo(BetOption.Home, oddsMovement);

            if (homeOdds != null)
            {
                OptionValue = homeOdds.OptionValue.ToOddsOptionFormat();
            }
        }

        private void BuildHomeOdds(IOddsMovement oddsMovement)
        {
            var homeOdds = GetOddsInfo(BetOption.Home, oddsMovement);

            if (homeOdds != null)
            {
                HomeOdds = homeOdds.LiveOdds.ToOddsFormat();
                HomeOddsTrend = homeOdds.OddsTrend.Value.ToString();
            }
        }

        private static BetOptionOdds GetOddsInfo(string option, IOddsMovement oddsMovement)
            => oddsMovement.BetOptions.FirstOrDefault(x => x.Type.Equals(option));
    }
}