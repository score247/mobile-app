namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Soccer.Enumerations;
    using Prism.Navigation;

    public class OneXTwoViewModel : BaseItemViewModel
    {
        private const string OddsNumerFormat = "0.00";

        public string Bookmaker { get; set; }
    
        public string HomeLiveOdds { get; set; }
        public string HomeOpeningOdds { get; set; }
        public string HomeOddsTrend { get; set; }

        public string AwayLiveOdds { get; set; }
        public string AwayOpeningOdds { get; set; }
        public string AwayOddsTrend { get; set; }

        public string DrawLiveOdds { get; set; }
        public string DrawOpeningOdds { get; set; }
        public string DrawOddsTrend { get; set; }

        private readonly IBetTypeOdds betTypeOdds;
        public OneXTwoViewModel(
            BetTypeEnum betType,
            IBetTypeOdds betTypeOdds,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(betType, betTypeOdds, navigationService, depdendencyResolver)
        {
            this.betTypeOdds = betTypeOdds;          

            Init();
        }

        private void Init()
        {

            Bookmaker = betTypeOdds.Bookmaker.Name;

            var homeOdds = GetOddsInfo(BetOption.Home);

            HomeLiveOdds = homeOdds.LiveOdds.ToString(OddsNumerFormat);
            HomeOpeningOdds = homeOdds.OpeningOdds.ToString(OddsNumerFormat);
            HomeOddsTrend = homeOdds.OddsTrend.Value;

            var drawOdds = GetOddsInfo(BetOption.Draw);
            DrawOpeningOdds = drawOdds.OpeningOdds.ToString(OddsNumerFormat);
            DrawLiveOdds = drawOdds.LiveOdds.ToString(OddsNumerFormat);
            DrawOddsTrend = drawOdds.OddsTrend.Value;

            var awayOdds = GetOddsInfo(BetOption.Away);
            AwayOpeningOdds = awayOdds.OpeningOdds.ToString(OddsNumerFormat);
            AwayLiveOdds = awayOdds.LiveOdds.ToString(OddsNumerFormat);
            AwayOddsTrend = awayOdds.OddsTrend.Value;

        }

        private BetOptionOdds GetOddsInfo(string option) => betTypeOdds.BetOptions.First(x => x.Type.Equals(option));
    }
}
