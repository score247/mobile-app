namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems
{
    using System.Collections.Generic;
    using Core;
    using Enumerations;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds.OddsItems.AsianHdp;
    using LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds.OddsItems.OneXTwo;
    using LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds.OddsItems.OverUnder;
    using Prism.Navigation;
    using Views.Templates.MatchDetails.DetailOdds.OddsItems;
    using Xamarin.Forms;

    public class BaseHeaderViewModel : ViewModelBase
    {
        private static readonly IDictionary<BetType, DataTemplate> TemplateMapper = new Dictionary<BetType, DataTemplate>
        {
            { BetType.OneXTwo, new OneXTwoHeaderTemplate() },
            { BetType.AsianHDP, new AsianHdpHeaderTemplate() },
            { BetType.OverUnder, new OverUnderHeaderTemplate() },
        };

        private readonly bool hasData;

        public BaseHeaderViewModel(
            BetType betType,
            bool hasData,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver)
        {
            BetType = betType;
            this.hasData = hasData;
        }

        public BetType BetType { get; }

        public DataTemplate CreateTemplate()
        {
            if (hasData && TemplateMapper.ContainsKey(BetType))
            {
                return TemplateMapper[BetType];
            }

            return new NoDataTemplate();
        }
    }
}