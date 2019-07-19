namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System.Linq;
    using LiveScore.Common.Extensions;
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

            MatchScore = oddsMovement.IsMatchStarted 
                ? $"{oddsMovement.HomeScore} - {oddsMovement.AwayScore}" 
                : string.Empty;

            MatchTime = oddsMovement.MatchTime;
            UpdateTime = oddsMovement.UpdateTime.ToDateAndTime(); //TODO convert to gmt+7

            BuildHomeOdds(oddsMovement);

            BuildDrawOdds(oddsMovement);

            BuildAwayOdds(oddsMovement);
        }

        private void BuildAwayOdds(OddsMovement oddsMovement)
        {
            var awayOdds = GetOddsInfo(BetOption.Away, oddsMovement);

            if (awayOdds != null)
            {               
                AwayOdds = awayOdds.LiveOdds.ToOddsFormat();
                AwayOddsTrend = awayOdds.OddsTrend.Value;
            }
        }

        private void BuildDrawOdds(OddsMovement oddsMovement)
        {
            var drawOdds = GetOddsInfo(BetOption.Draw, oddsMovement);

            if (drawOdds != null)
            {
                DrawOdds = drawOdds.LiveOdds.ToOddsFormat();
                DrawOddsTrend = drawOdds.OddsTrend.Value;
            }
        }

        private void BuildHomeOdds(OddsMovement oddsMovement)
        {
            var homeOdds = GetOddsInfo(BetOption.Home, oddsMovement);

            if (homeOdds != null)
            {
                HomeOdds = homeOdds.LiveOdds.ToOddsFormat();
                HomeOddsTrend = homeOdds.OddsTrend.Value;
            }
        }

        private static BetOptionOdds GetOddsInfo(string option, OddsMovement oddsMovement) => oddsMovement.BetOptions.FirstOrDefault(x => x.Type.Equals(option));
    }
}
