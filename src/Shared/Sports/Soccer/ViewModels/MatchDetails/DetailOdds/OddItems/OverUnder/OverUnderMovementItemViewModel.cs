namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems
{
    using System.Linq;
    using Core;
    using LiveScore.Core.Models.Odds;
    using Enumerations;
    using Extensions;
    using Prism.Navigation;

    public class OverUnderMovementItemViewModel : BaseMovementItemViewModel
    {
        public OverUnderMovementItemViewModel(
            IOddsMovement oddsMovement,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(BetType.OverUnder, oddsMovement, navigationService, dependencyResolver)
        {
            Initialize(oddsMovement);
        }

        public string MatchTime { get; private set; }

        public string MatchScore { get; private set; }

        public string OverOdds { get; private set; }

        public string OverOddsTrend { get; private set; }

        public string OptionValue { get; private set; }

        public string UnderOdds { get; private set; }

        public string UnderOddsTrend { get; private set; }

        private void Initialize(IOddsMovement oddsMovement)
        {
            MatchScore = oddsMovement.IsMatchStarted
                ? $"{oddsMovement.HomeScore} - {oddsMovement.AwayScore}"
                : string.Empty;

            MatchTime = oddsMovement.MatchTime;

            BuildOverOdds(oddsMovement);

            BuildBetOptionsValue(oddsMovement);

            BuildUnderOdds(oddsMovement);
        }

        private void BuildBetOptionsValue(IOddsMovement oddsMovement)
        {
            var homeOdds = GetOddsInfo(BetOption.Over, oddsMovement);

            if (homeOdds != null)
            {
                OptionValue = homeOdds.OptionValue.ToOddsOptionFormat();
            }
        }

        private void BuildOverOdds(IOddsMovement oddsMovement)
        {
            var homeOdds = GetOddsInfo(BetOption.Over, oddsMovement);

            if (homeOdds != null)
            {
                OverOdds = homeOdds.LiveOdds.ToOddsFormat();
                OverOddsTrend = homeOdds.OddsTrend.Value.ToString();
            }
        }

        private void BuildUnderOdds(IOddsMovement oddsMovement)
        {
            var awayOdds = GetOddsInfo(BetOption.Under, oddsMovement);

            if (awayOdds != null)
            {
                UnderOdds = awayOdds.LiveOdds.ToOddsFormat();
                UnderOddsTrend = awayOdds.OddsTrend.Value.ToString();
            }
        }

        private static BetOptionOdds GetOddsInfo(string option, IOddsMovement oddsMovement)
            => oddsMovement.BetOptions.FirstOrDefault(x => x.Type.Equals(option));
    }
}