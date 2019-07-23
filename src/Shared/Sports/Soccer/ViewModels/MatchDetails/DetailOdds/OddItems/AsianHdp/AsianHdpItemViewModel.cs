namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Extensions;
    using Prism.Navigation;

    public class AsianHdpItemViewModel : BaseItemViewModel
    {
        private readonly IBetTypeOdds betTypeOdds;

        public AsianHdpItemViewModel(
            IBetTypeOdds betTypeOdds,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(BetType.AsianHDP, betTypeOdds, navigationService, depdendencyResolver)
        {
            this.betTypeOdds = betTypeOdds;

            Initialize();
        }

        public string HomeLiveOdds { get; private set; }

        public string HomeOpeningOdds { get; private set; }

        public string HomeOddsTrend { get; private set; }

        public string AwayLiveOdds { get; private set; }

        public string AwayOpeningOdds { get; private set; }

        public string AwayOddsTrend { get; private set; }

        public string LiveHdp { get; private set; }

        public string OpeningHdp { get; private set; }

        protected override void Initialize()
        {
            BuildHomeOdds();

            BuildBetOptionsValue();

            BuildAwayOdds();
        }

        private void BuildAwayOdds()
        {
            var awayOdds = GetOddsInfo(BetOption.Away);

            if (awayOdds != null)
            {
                AwayOpeningOdds = awayOdds.OpeningOdds.ToOddsFormat();
                AwayLiveOdds = awayOdds.LiveOdds.ToOddsFormat();
                AwayOddsTrend = awayOdds.OddsTrend.Value;
            }
        }

        private void BuildBetOptionsValue()
        {
            var homeOdds = GetOddsInfo(BetOption.Home);

            if (homeOdds != null)
            {
                OpeningHdp = homeOdds.OpeningOptionValue.ToOddsOptionFormat();
                LiveHdp = homeOdds.OptionValue.ToOddsOptionFormat();
            }
        }

        private void BuildHomeOdds()
        {
            var homeOdds = GetOddsInfo(BetOption.Home);

            if (homeOdds != null)
            {
                HomeLiveOdds = homeOdds.LiveOdds.ToOddsFormat();
                HomeOpeningOdds = homeOdds.OpeningOdds.ToOddsFormat();
                HomeOddsTrend = homeOdds.OddsTrend.Value;
            }
        }

        private BetOptionOdds GetOddsInfo(string option) => betTypeOdds.BetOptions.FirstOrDefault(x => x.Type.Equals(option));
    }
}