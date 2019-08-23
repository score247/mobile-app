namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Extensions;
    using Prism.Navigation;

    public class OverUnderItemViewModel : BaseItemViewModel
    {
        public OverUnderItemViewModel(
            IBetTypeOdds betTypeOdds,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(BetType.OverUnder, betTypeOdds, navigationService, depdendencyResolver)
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
            BuildOverOdds();

            BuildBetOptionsValue();

            BuildUnderOdds();
        }

        private void BuildUnderOdds()
        {
            var underOdds = GetOddsInfo(BetOption.Under);

            if (underOdds != null)
            {
                UnderOpeningOdds = underOdds.OpeningOdds.ToOddsFormat();
                UnderLiveOdds = underOdds.LiveOdds.ToOddsFormat();
                UnderOddsTrend = underOdds.OddsTrend.Value.ToString();
            }
        }

        private void BuildBetOptionsValue()
        {
            var overOdds = GetOddsInfo(BetOption.Over);

            if (overOdds != null)
            {
                OpeningOverOptionValue = overOdds.OpeningOptionValue.ToOddsOptionFormat();
                LiveOverOptionValue = overOdds.OptionValue.ToOddsOptionFormat();
            }
        }

        private void BuildOverOdds()
        {
            var overOdds = GetOddsInfo(BetOption.Over);

            if (overOdds != null)
            {
                OverLiveOdds = overOdds.LiveOdds.ToOddsFormat();
                OverOpeningOdds = overOdds.OpeningOdds.ToOddsFormat();
                OverOddsTrend = overOdds.OddsTrend.Value.ToString();
            }
        }

        private BetOptionOdds GetOddsInfo(string option) => BetTypeOdds.BetOptions.FirstOrDefault(x => x.Type.Equals(option));
    }
}