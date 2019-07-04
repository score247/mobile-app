namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using Prism.Navigation;

    public class OneXTwoViewModel: BaseItemViewModel
    {
        public string Bookmaker { get; set; }

        public decimal HomeLiveOdds { get; set; }

        public OddsTrend HomeLiveTrend { get; set; }

        public decimal DrawLiveOdds { get; set; }

        public OddsTrend DrawLiveTrend { get; set; }

        public decimal AwayLiveOdds { get; set; }

        public OddsTrend AwayLiveTrend { get; set; }

        public decimal HomeOpeningOdds { get; set; }

        public decimal DrawOpeningOdds { get; set; }

        public decimal AwayOpeningOdds { get; set; }

        public OneXTwoViewModel(
            BetType betType,
            IBetTypeOdds betTypeOdds,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(betType ,betTypeOdds, navigationService, depdendencyResolver)
        {
            Bookmaker = betTypeOdds.Bookmaker.Name;
            HomeLiveOdds = betTypeOdds.BetOptions.First(x => x.Type == "home").LiveOdds;
            HomeLiveTrend = betTypeOdds.BetOptions.First(x => x.Type == "home").OddsTrend;
            HomeOpeningOdds = betTypeOdds.BetOptions.First(x => x.Type == "home").OpeningOdds;
            DrawLiveOdds = betTypeOdds.BetOptions.First(x => x.Type == "draw").LiveOdds;
            DrawLiveTrend = betTypeOdds.BetOptions.First(x => x.Type == "draw").OddsTrend;
            DrawOpeningOdds = betTypeOdds.BetOptions.First(x => x.Type == "draw").OpeningOdds;
            AwayLiveOdds = betTypeOdds.BetOptions.First(x => x.Type == "away").LiveOdds;
            AwayLiveTrend = betTypeOdds.BetOptions.First(x => x.Type == "away").OddsTrend;
            AwayOpeningOdds = betTypeOdds.BetOptions.First(x => x.Type == "away").OpeningOdds;

        }
    }
}
