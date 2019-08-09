namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Views.Templates.DetailOdds.OddsItems;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class BaseMovementItemViewModel : ViewModelBase
    {
        private static readonly IDictionary<BetType, Type> ViewModelMapper = new Dictionary<BetType, Type>
        {
            { BetType.OneXTwo, typeof(OneXTwoMovementItemViewModel) },
            { BetType.AsianHDP, typeof(AsianHdpMovementItemViewModel) },
            { BetType.OverUnder, typeof(OverUnderMovementItemViewModel) },
        };

        private static readonly IDictionary<BetType, DataTemplate> TemplateMapper = new Dictionary<BetType, DataTemplate>
        {
            { BetType.OneXTwo, new OneXTwoMovementItemTemplate() },
            { BetType.AsianHDP, new AsianHdpMovementItemTemplate() },
            { BetType.OverUnder, new OverUnderMovementItemTemplate() },
        };

        public BaseMovementItemViewModel(
            BetType betType,
            IOddsMovement oddsMovement,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
            : base(navigationService, depdendencyResolver)
        {
            OddsMovement = oddsMovement;
            BetType = betType;

            UpdateTime = oddsMovement.UpdateTime.ToLocalDateAndTime(); 
            FullUpdateTime = oddsMovement.UpdateTime.ToFullLocalDateAndTime();
        }

        public IOddsMovement OddsMovement { get; }

        public BetType BetType { get; }

        public string UpdateTime { get; }

        public string FullUpdateTime { get; }

        public DataTemplate CreateTemplate()
        {
            if (TemplateMapper.ContainsKey(BetType))
            {
                return TemplateMapper[BetType];
            }

            return new OneXTwoMovementItemTemplate();
        }

        public BaseMovementItemViewModel CreateInstance()
        {
            if (ViewModelMapper.ContainsKey(BetType))
            {
                return Activator.CreateInstance(
                    ViewModelMapper[BetType],
                    OddsMovement, NavigationService, DependencyResolver) as BaseMovementItemViewModel;
            }

            return new BaseMovementItemViewModel(BetType, OddsMovement, NavigationService, DependencyResolver);
        }
    }
}