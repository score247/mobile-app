namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System.Collections.Generic;
    using LiveScore.Core;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Views.Templates.DetailOdds.OddsItems;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class BaseMovementHeaderViewModel : ViewModelBase
    {
        private static readonly IDictionary<BetType, DataTemplate> TemplateMapper = new Dictionary<BetType, DataTemplate>
        {
            { BetType.OneXTwo, new OneXTwoMovementHeaderTemplate() },
            { BetType.AsianHDP, new AsianHdpMovementHeaderTemplate() },
            { BetType.OverUnder, new OverUnderMovementHeaderTemplate() }
        };

        private readonly bool HasData;

        public BaseMovementHeaderViewModel(
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
