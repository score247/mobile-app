﻿namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Soccer.Enumerations;
    using Prism.Navigation;

    public class AsianHdpItemViewModel : BaseItemViewModel
    {
        private const string OddsNumerFormat = "0.00";

        private readonly IBetTypeOdds betTypeOdds;

        public AsianHdpItemViewModel(
            BetType betType,
            IBetTypeOdds betTypeOdds,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(betType, betTypeOdds, navigationService, depdendencyResolver)
        {
            this.betTypeOdds = betTypeOdds;

            Initialize();
        }

        public string Bookmaker { get; private set; }

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
            Bookmaker = betTypeOdds.Bookmaker?.Name;

            BuildHomeOdds();

            BuildBetOptionsValue();

            BuildAwayOdds();
        }

        private void BuildAwayOdds()
        {
            var awayOdds = GetOddsInfo(BetOption.Away);

            if (awayOdds != null)
            {
                AwayOpeningOdds = awayOdds.OpeningOdds.ToString(OddsNumerFormat);
                AwayLiveOdds = awayOdds.LiveOdds.ToString(OddsNumerFormat);
                AwayOddsTrend = awayOdds.OddsTrend.Value;
            }
        }

        private void BuildBetOptionsValue()
        {
            var homeOdds = GetOddsInfo(BetOption.Home);

            if (homeOdds != null)
            {
                OpeningHdp = homeOdds.OpenningOptionValue;
                LiveHdp = homeOdds.OptionValue;
            }
        }

        private void BuildHomeOdds()
        {
            var homeOdds = GetOddsInfo(BetOption.Home);

            if (homeOdds != null)
            {
                HomeLiveOdds = homeOdds.LiveOdds.ToString(OddsNumerFormat);
                HomeOpeningOdds = homeOdds.OpeningOdds.ToString(OddsNumerFormat);
                HomeOddsTrend = homeOdds.OddsTrend.Value;
            }
        }

        private BetOptionOdds GetOddsInfo(string option) => betTypeOdds.BetOptions.FirstOrDefault(x => x.Type.Equals(option));
    }
}
