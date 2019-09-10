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
            BuildOverOddsAndOptionValue();

            BuildUnderOdds();
        }

        public string OverOdds { get; private set; }

        public string OverOddsTrend { get; private set; }

        public string OptionValue { get; private set; }

        public string UnderOdds { get; private set; }

        public string UnderOddsTrend { get; private set; }       

        private void BuildOverOddsAndOptionValue()
        {
            var overOdds = OddsMovement.BetOptions.FirstOrDefault(x => x.Type.Equals(BetOption.Over.DisplayName));

            if (overOdds == null)
            {
                return;
            }

            OverOdds = overOdds.LiveOdds.ToOddsFormat();
            OverOddsTrend = overOdds.OddsTrend.Value.ToString();

            OptionValue = overOdds.OptionValue.ToOddsOptionFormat();
        }

        private void BuildUnderOdds()
        {
            var underOdds = OddsMovement.BetOptions.FirstOrDefault(x => x.Type.Equals(BetOption.Under.DisplayName));

            if (underOdds == null)
            {
                return;
            }

            UnderOdds = underOdds.LiveOdds.ToOddsFormat();
            UnderOddsTrend = underOdds.OddsTrend.Value.ToString();
        }
    }
}