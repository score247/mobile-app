namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems
{
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Extensions;
    using Prism.Navigation;

    public class AsianHdpItemViewModel : BaseItemViewModel
    {
        public AsianHdpItemViewModel(
            IBetTypeOdds betTypeOdds,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
             : base(BetType.AsianHDP, betTypeOdds, navigationService, dependencyResolver)
        {
        }

        public string HomeLiveOdds { get; private set; }

        public string HomeOpeningOdds { get; private set; }

        public string HomeOddsTrend { get; private set; }

        public string AwayLiveOdds { get; private set; }

        public string AwayOpeningOdds { get; private set; }

        public string AwayOddsTrend { get; private set; }

        public string LiveHdp { get; private set; }

        public string OpeningHdp { get; private set; }

        protected void OnInitialized()
        {
            BuildHomeOddsAndOptionValue();

            BuildAwayOdds();
        }

        private void BuildAwayOdds()
        {            
            var awayOdds = BetTypeOdds.BetOptions.FirstOrDefault(x => x.Type.Equals(BetOption.Away.DisplayName));

            if (awayOdds == null)
            {
                return;
            }

            AwayOpeningOdds = awayOdds.OpeningOdds.ToOddsFormat();
            AwayLiveOdds = awayOdds.LiveOdds.ToOddsFormat();
            AwayOddsTrend = awayOdds.OddsTrend.Value.ToString();
        }      

        private void BuildHomeOddsAndOptionValue()
        {
            var homeOdds = BetTypeOdds.BetOptions.FirstOrDefault(x => x.Type.Equals(BetOption.Home.DisplayName)); 

            if (homeOdds == null)
            {
                return;
            }

            HomeLiveOdds = homeOdds.LiveOdds.ToOddsFormat();
            HomeOpeningOdds = homeOdds.OpeningOdds.ToOddsFormat();
            HomeOddsTrend = homeOdds.OddsTrend.Value.ToString();

            OpeningHdp = homeOdds.OpeningOptionValue.ToOddsOptionFormat();
            LiveHdp = homeOdds.OptionValue.ToOddsOptionFormat();
        }
    }
}