namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Soccer.Enumerations;
    using Prism.Navigation;

    public class OverUnderItemViewModel : BaseItemViewModel
    {
        private const string OddsNumerFormat = "0.00";

        private readonly IBetTypeOdds betTypeOdds;

        public OverUnderItemViewModel(
            BetType betType,
            IBetTypeOdds betTypeOdds,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(betType, betTypeOdds, navigationService, depdendencyResolver)
        {
            this.betTypeOdds = betTypeOdds;

            Initialize();
        }

        public string Bookmaker { get; private set; }

        public string OverLiveOdds { get; private set; }

        public string OverOpeningOdds { get; private set; }

        public string OverOddsTrend { get; private set; }

        public string UnderLiveOdds { get; private set; }

        public string UnderOpeningOdds { get; private set; }

        public string UnderOddsTrend { get; private set; }

        public string LiveOverOptionValue { get; private set; }

        public string OpeningOverOptionValue { get; private set; }

        protected override void Initialize()
        {
            Bookmaker = betTypeOdds.Bookmaker?.Name;

            BuildOverOdds();

            BuildBetOptionsValue();

            BuildUnderOdds();
        }

        private void BuildUnderOdds()
        {
            var underOdds = GetOddsInfo(BetOption.Under);

            if (underOdds != null)
            {
                UnderOpeningOdds = underOdds.OpeningOdds.ToString(OddsNumerFormat);
                UnderLiveOdds = underOdds.LiveOdds.ToString(OddsNumerFormat);
                UnderOddsTrend = underOdds.OddsTrend.Value;
            }
        }

        private void BuildBetOptionsValue()
        {
            var overOdds = GetOddsInfo(BetOption.Over);

            if (overOdds != null)
            {
                OpeningOverOptionValue = overOdds.OpeningOptionValue;
                LiveOverOptionValue = overOdds.OptionValue;
            }
        }

        private void BuildOverOdds()
        {
            var overOdds = GetOddsInfo(BetOption.Over);

            if (overOdds != null)
            {
                OverLiveOdds = overOdds.LiveOdds.ToString(OddsNumerFormat);
                OverOpeningOdds = overOdds.OpeningOdds.ToString(OddsNumerFormat);
                OverOddsTrend = overOdds.OddsTrend.Value;
            }
        }

        private BetOptionOdds GetOddsInfo(string option) => betTypeOdds.BetOptions.FirstOrDefault(x => x.Type.Equals(option));
    }
}
