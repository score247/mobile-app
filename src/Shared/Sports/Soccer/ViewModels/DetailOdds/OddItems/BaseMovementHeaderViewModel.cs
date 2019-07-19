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
            { BetType.OneXTwo, new OneXTwoMovementHeaderTemplate() }
        };

        public BaseMovementHeaderViewModel(
            BetType betType,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
            : base(navigationService, depdendencyResolver)
        {
            BetType = betType;
        }

        public BetType BetType { get; }

        public DataTemplate CreateTemplate()
        {
            if (TemplateMapper.ContainsKey(BetType))
            {
                return TemplateMapper[BetType];
            }

            return new OneXTwoMovementHeaderTemplate();
        }
    }
}
