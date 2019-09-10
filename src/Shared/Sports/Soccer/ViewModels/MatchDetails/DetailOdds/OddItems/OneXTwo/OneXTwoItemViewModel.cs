namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems
{
    using System.Linq;
    using Core;
    using Enumerations;
    using Extensions;
    using LiveScore.Core.Models.Odds;
    using Prism.Navigation;

    public class OneXTwoItemViewModel : BaseItemViewModel
    {
        public OneXTwoItemViewModel(
            IBetTypeOdds betTypeOdds,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
             : base(BetType.OneXTwo, betTypeOdds, navigationService, dependencyResolver)
        {
        }

        public string HomeLiveOdds { get; private set; }

        public string HomeOpeningOdds { get; private set; }

        public string HomeOddsTrend { get; private set; }

        public string AwayLiveOdds { get; private set; }

        public string AwayOpeningOdds { get; private set; }

        public string AwayOddsTrend { get; private set; }

        public string DrawLiveOdds { get; private set; }

        public string DrawOpeningOdds { get; private set; }

        public string DrawOddsTrend { get; private set; }

        protected override void OnInitialized()
        {
            BuildHomeOdds();

            BuildDrawOdds();

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

        private void BuildDrawOdds()
        {
            var drawOdds = BetTypeOdds.BetOptions.FirstOrDefault(x => x.Type.Equals(BetOption.Draw.DisplayName));

            if (drawOdds == null)
            {
                return;
            }

            DrawOpeningOdds = drawOdds.OpeningOdds.ToOddsFormat();
            DrawLiveOdds = drawOdds.LiveOdds.ToOddsFormat();
            DrawOddsTrend = drawOdds.OddsTrend.Value.ToString();

        }

        private void BuildHomeOdds()
        {
            var homeOdds = BetTypeOdds.BetOptions.FirstOrDefault(x => x.Type.Equals(BetOption.Home.DisplayName));

            if (homeOdds == null)
            {
                return;
            }

            HomeLiveOdds = homeOdds.LiveOdds.ToOddsFormat();
            HomeOpeningOdds = homeOdds.OpeningOdds.ToOddsFormat();
            HomeOddsTrend = homeOdds.OddsTrend.Value.ToString();

        }
    }
}