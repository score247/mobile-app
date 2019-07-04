namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Soccer.Enumerations;
    using Prism.Navigation;

    public class OneXTwoViewModel: BaseItemViewModel
    {
        public string Bookmaker { get; set; }

        public BetOptionOdds HomeOdds { get; set; }

        public BetOptionOdds AwayOdds { get; set; }

        public BetOptionOdds DrawOdds { get; set; }

        public OneXTwoViewModel(
            BetType betType,
            IBetTypeOdds betTypeOdds,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(betType ,betTypeOdds, navigationService, depdendencyResolver)
        {
            Bookmaker = betTypeOdds.Bookmaker.Name;

            HomeOdds = betTypeOdds.BetOptions.First(x => x.Type.Equals(BetOption.Home));
            DrawOdds = betTypeOdds.BetOptions.First(x => x.Type.Equals(BetOption.Draw));
            AwayOdds = betTypeOdds.BetOptions.First(x => x.Type.Equals(BetOption.Away));
        }
    }
}
