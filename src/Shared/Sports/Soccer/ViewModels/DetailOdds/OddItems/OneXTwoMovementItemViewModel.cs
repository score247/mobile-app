namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Extensions;
    using Prism.Navigation;

    public class OneXTwoMovementItemViewModel : BaseMovementItemViewModel 
    {
        public OneXTwoMovementItemViewModel(
            OddsMovement oddsMovement, 
            INavigationService navigationService, 
            IDependencyResolver depdendencyResolver) 
            : base(BetType.OneXTwo, oddsMovement, navigationService, depdendencyResolver)
        {
            Initialize(oddsMovement);
        }

        public string MatchTime { get; private set; }

        public string MatchScore { get; private set; }

        public string HomeOdds { get; private set; }

        public string HomeOddsTrend { get; private set; }

        public string DrawOdds { get; private set; }

        public string DrawOddsTrend { get; private set; }

        public string AwayOdds { get; private set; }

        public string AwayOddsTrend { get; private set; }

        public string UpdateTime { get; private set; }

        private void Initialize(OddsMovement oddsMovement)        
        {
            MatchScore = $"{oddsMovement.HomeScore} - {oddsMovement.AwayScore}";
            MatchTime = oddsMovement.MatchTime;
            HomeOdds = oddsMovement.BetOptions.First(x => x.Type == "home").LiveOdds.ToOddsFormat();
            HomeOddsTrend = oddsMovement.BetOptions.First(x => x.Type == "home").OddsTrend.Value;
            DrawOdds = oddsMovement.BetOptions.First(x => x.Type == "draw").LiveOdds.ToOddsFormat();
            DrawOddsTrend = oddsMovement.BetOptions.First(x => x.Type == "draw").OddsTrend.Value;
            AwayOdds = oddsMovement.BetOptions.First(x => x.Type == "away").LiveOdds.ToOddsFormat();
            AwayOddsTrend = oddsMovement.BetOptions.First(x => x.Type == "away").OddsTrend.Value;
            UpdateTime = oddsMovement.UpdateTime.ToString("dd-MM HH:mm"); //TODO convert to gmt+7
        }
    }
}
