namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System.Collections.Generic;
    using LiveScore.Core;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Views.Templates.DetailOdds.OddsItems;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class BaseHeaderViewModel : ViewModelBase
    {
        private static readonly IDictionary<BetType, DataTemplate> TemplateMapper = new Dictionary<BetType, DataTemplate>
        {
            { BetType.OneXTwo, new OneXTwoHeaderTemplate() },
            { BetType.AsianHDP, new AsianHdpHeaderTemplate() },
            { BetType.OverUnder, new OverUnderHeaderTemplate() },
        };

        private readonly bool HasData;

        public BaseHeaderViewModel(
            BetType betType,
            bool hasData,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
            : base(navigationService, depdendencyResolver)
        {
            BetType = betType;
            HasData = hasData;
        }

        public BetType BetType { get; }

        public DataTemplate CreateTemplate()
        {
            if (HasData && TemplateMapper.ContainsKey(BetType))
            {
                return TemplateMapper[BetType];
            }

            return new NoDataTemplate();
        }
    }
}