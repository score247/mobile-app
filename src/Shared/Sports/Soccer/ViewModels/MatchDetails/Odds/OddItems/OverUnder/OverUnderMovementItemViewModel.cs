using System.Linq;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Odds;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Extensions;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.OverUnder
{
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
            var overOdds = OddsMovement.BetOptions?.FirstOrDefault(x => x.Type.Equals(BetOption.Over.DisplayName));

            if (overOdds == null)
            {
                return;
            }

            OverOdds = overOdds.LiveOdds.ToOddsFormat();
            OverOddsTrend = overOdds.OddsTrend == null ? OddsTrend.Neutral.DisplayName : overOdds.OddsTrend.Value.ToString();
            OptionValue = overOdds.OptionValue.ToOddsOptionFormat();
        }

        private void BuildUnderOdds()
        {
            var underOdds = OddsMovement.BetOptions?.FirstOrDefault(x => x.Type.Equals(BetOption.Under.DisplayName));

            if (underOdds == null)
            {
                return;
            }

            UnderOdds = underOdds.LiveOdds.ToOddsFormat();
            UnderOddsTrend = underOdds.OddsTrend == null ? OddsTrend.Neutral.DisplayName : underOdds.OddsTrend.Value.ToString();
        }
    }
}