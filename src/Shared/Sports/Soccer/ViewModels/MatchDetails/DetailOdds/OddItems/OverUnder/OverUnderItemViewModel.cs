namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems
{
    using System.Linq;
    using Core;
    using Enumerations;
    using Extensions;
    using LiveScore.Core.Models.Odds;
    using Prism.Navigation;

    public class OverUnderItemViewModel : BaseItemViewModel
    {
        public OverUnderItemViewModel(
            IBetTypeOdds betTypeOdds,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
             : base(BetType.OverUnder, betTypeOdds, navigationService, dependencyResolver)
        {
        }

        public string OverLiveOdds { get; private set; }

        public string OverOpeningOdds { get; private set; }

        public string OverOddsTrend { get; private set; }

        public string UnderLiveOdds { get; private set; }

        public string UnderOpeningOdds { get; private set; }

        public string UnderOddsTrend { get; private set; }

        public string LiveOverOptionValue { get; private set; }

        public string OpeningOverOptionValue { get; private set; }

        protected override void OnInitialized()
        {
            BuildOverOddsAndOptionValue();

            BuildUnderOdds();
        }

        private void BuildUnderOdds()
        {
            var underOdds = BetTypeOdds.BetOptions.FirstOrDefault(x => x.Type.Equals(BetOption.Under.DisplayName));

            if (underOdds == null)
            {
                return;
            }

            UnderOpeningOdds = underOdds.OpeningOdds.ToOddsFormat();
            UnderLiveOdds = underOdds.LiveOdds.ToOddsFormat();
            UnderOddsTrend = underOdds.OddsTrend.Value.ToString();
        }


        private void BuildOverOddsAndOptionValue()
        {
            var overOdds = BetTypeOdds.BetOptions.FirstOrDefault(x => x.Type.Equals(BetOption.Over.DisplayName));

            if (overOdds == null)
            {
                return;
            }

            OverLiveOdds = overOdds.LiveOdds.ToOddsFormat();
            OverOpeningOdds = overOdds.OpeningOdds.ToOddsFormat();
            OverOddsTrend = overOdds.OddsTrend.Value.ToString();

            OpeningOverOptionValue = overOdds.OpeningOptionValue.ToOddsOptionFormat();
            LiveOverOptionValue = overOdds.OptionValue.ToOddsOptionFormat();
        }
    }
}